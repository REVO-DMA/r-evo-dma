export enum StreamingService
{
    Twitch,
    YouTube,
    Kick,
}

export type CheckURL = {
    Username: string;
    StreamingService: StreamingService;
    URL: string,
}

const CachedStreams = new Map<string, CheckURL>();

export function ResetCache()
{
    CachedStreams.clear();
}

export class StreamerChecker
{
    public static async IsUserLive(username: string)
    {
        const cachedStream = CachedStreams.get(username);
        if (cachedStream !== undefined)
        {
            console.log(`Served from cache: ${username}`);
            return cachedStream;
        }

        const checkURLs = this.GetStreamData(username);

        let liveURL: CheckURL = null;

        for (const checkURL of checkURLs)
        {
            const response = await this.GetURL(checkURL.URL);

            const liveData = this.GetPageLiveStream(checkURL, response);
            if (liveData != null)
            {
                liveURL = liveData;
                break;
            }
        }

        CachedStreams.set(username, liveURL);

        return liveURL;
    }

    private static async GetURL(url: string)
    {
        try
        {
            const response = await fetch(url);
            const responseText = await response.text();

            return responseText;
        }
        catch (err)
        {
            return "";
        }
    }

    private static GetPageLiveStream(streamData: CheckURL, data: string)
    {
        try
        {
            if (streamData.StreamingService === StreamingService.Twitch)
            {
                if (this.StrContains(data, `"isLiveBroadcast":true`))
                {
                    return streamData;
                }
            }
            else if (streamData.StreamingService === StreamingService.YouTube)
            {
                const searchString = `<link rel="canonical" href="https://www.youtube.com/watch?v=`;
                if (this.StrContains(data, searchString) &&
                    this.StrContains(data, `"simpleText":"Escape from Tarkov"`))
                {
                    const startIndex = data.indexOf(searchString);
                    const endIndex = data.indexOf(`"`, startIndex + searchString.length);

                    streamData.URL = data.slice(startIndex, endIndex).split(`href="`)[1];
                    
                    return streamData;
                }
            }
            else if (streamData.StreamingService === StreamingService.Kick)
            {
                const parsed = JSON.parse(data);
                if (parsed.data != null)
                {
                    streamData.URL = `https://kick.com/${streamData.Username}`;

                    return streamData;
                }
            }
        }
        catch (err)
        {
            console.log(`[STREAMER CHECKER] -> IsUserLive(): Failed to get ${streamData.URL} ~ ${err}`);
        }

        return null;
    }

    private static GetStreamData(username: string): CheckURL[]
    {
        const checkURLs: CheckURL[] = [];

        // Twitch
        {
            const siteUsername = this.CheckVariations(username, [
                "twitch",
                "ttv",
                "tv",
            ]);

            checkURLs.push({
                Username: username.toLowerCase(),
                StreamingService: StreamingService.Twitch,
                URL: `https://www.twitch.tv/${username.toLowerCase()}`,
            });

            const tvFixed = this.FixEndingTV(username.toLowerCase());
            if (tvFixed !== null)
            {
                checkURLs.push({
                    Username: tvFixed.toLowerCase(),
                    StreamingService: StreamingService.Twitch,
                    URL: `https://www.twitch.tv/${tvFixed.toLowerCase()}`,
                });
            }
    
            // Push the cleaned variation as well if found
            if (siteUsername != null)
            {
                checkURLs.push({
                    Username: siteUsername.toLowerCase(),
                    StreamingService: StreamingService.Twitch,
                    URL: `https://www.twitch.tv/${siteUsername.toLowerCase()}`,
                });
            }
        }

        // YouTube
        {
            const siteUsername = this.CheckVariations(username, [
                "youtube",
                "yt",
                "tv",
            ]);

            checkURLs.push({
                Username: username,
                StreamingService: StreamingService.YouTube,
                URL: `https://www.youtube.com/@${username}/live`,
            });

            const tvFixed = this.FixEndingTV(username);
            if (tvFixed !== null)
            {
                checkURLs.push({
                    Username: tvFixed,
                    StreamingService: StreamingService.YouTube,
                    URL: `https://www.youtube.com/@${tvFixed}/live`,
                });
            }
    
            // Push the cleaned variation as well if found
            if (siteUsername != null)
            {
                checkURLs.push({
                    Username: siteUsername,
                    StreamingService: StreamingService.YouTube,
                    URL: `https://www.youtube.com/@${siteUsername}/live`,
                });
            }
        }

        // Kick
        {
            const siteUsername = this.CheckVariations(username, [
                "kick",
                "ktv",
                "kck",
                "tv",
            ]);

            checkURLs.push({
                Username: username,
                StreamingService: StreamingService.Kick,
                URL: `https://kick.com/api/v2/channels/${username}/livestream`,
            });

            const tvFixed = this.FixEndingTV(username);
            if (tvFixed !== null)
            {
                checkURLs.push({
                    Username: tvFixed,
                    StreamingService: StreamingService.Kick,
                    URL: `https://kick.com/api/v2/channels/${tvFixed}/livestream`,
                });
            }
    
            // Push the cleaned variation as well if found
            if (siteUsername != null)
            {
                checkURLs.push({
                    Username: siteUsername,
                    StreamingService: StreamingService.Kick,
                    URL: `https://kick.com/api/v2/channels/${siteUsername}/livestream`,
                });
            }
        }

        return checkURLs;
    }

    private static StrContains(baseString: string, substring: string)
    {
        if (baseString == null || substring == null)
            return false;

        return baseString.toLowerCase().includes(substring.toLowerCase());
    }

    /**
     * Handles the specific case where a username ends with the letter t but they also have tv after it (ex. TheBurntPeanutTV).
     * In this case we remove the ending TV to create another variation of the username to check.
     */
    private static FixEndingTV(username: string): string {
        const searchSuffix = "ttv";
        if (username.toLowerCase().endsWith(searchSuffix))
        {    
            return `${username.slice(0, -searchSuffix.length)}t`;
        }

        return null;
    }

    /**
     * Matches usernames with any number of _ or - before/after the target substring.
     */
    private static NormalizeName(username: string, substring: string)
    {
        // This check prevents a username like CoolDude_ or -CoolDude from being matched since this regex will strip the _ or -
        if (!this.StrContains(username, substring)) return username;

        const pattern = new RegExp(`^[_-]*${substring}[_-]*|[_-]*${substring}[_-]*$`, 'i');
        return username.replace(pattern, '');
    }

    private static GetCleanName(name: string, substrings: string[]): string
    {
        let cleanedName = name;
        let shortestSubstringLength = name.length;

        for (const substring of substrings)
        {
            const lowerCaseName = name.toLowerCase();

            const pattern = new RegExp(`^[_-]*${substring}[_-]*|[_-]*${substring}[_-]*$`, 'i');
            const baseName = lowerCaseName.replace(pattern, '');

            if (baseName === lowerCaseName)
                continue;

            if (baseName.length < shortestSubstringLength) {
                cleanedName = baseName;
                shortestSubstringLength = baseName.length;
            }
        }

        return cleanedName;
    }

    private static CheckVariations(username: string, variations: string[])
    {
        for (const variation of variations)
        {
            const normalized = this.NormalizeName(username, variation);
            if (normalized != username) return normalized;
        }

        return null;
    }
}