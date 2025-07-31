import { Client, FileInfo } from "basic-ftp";
import { DateTime } from "luxon";
import globals from "../globals.js";
import cron from "node-cron";
import path from "path";
import fs from "fs/promises";
import { minutesToSeconds, signUrl } from "../bunnySigner.js";
import { extractVersion } from "../utils.js";
import fse from "fs-extra";
import { Session } from "../database/tables/User/Session.js";
import { User } from "../database/tables/User/User.js";

const CHANGELOG_NAME = "changelog.md";

const directoriesToCheck = [
    "eft-dma-radar-toolkit",
    "eft-dma-radar-toolkit-public",
    "arena-dma",
    "temp-spoofer",
    "test-tool",
    "memory-mapper",
    "wallpaper",
];

const downloadableFiles = {
    "eft-dma-radar-toolkit": {
        name: "EFT Radar + Toolkit Private",
        isPrivate: true,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "eft-dma-radar-toolkit-public": {
        name: "EFT Radar + Toolkit Public",
        isPrivate: false,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "arena-dma": {
        name: "EFT: Arena DMA ESP + Toolkit",
        isPrivate: false,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "temp-spoofer": {
        name: "Spoofer",
        isPrivate: true,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "test-tool": {
        name: "DMA Test Tool",
        isPrivate: false,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "memory-mapper": {
        name: "Memory Mapper",
        isPrivate: false,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
    "wallpaper": {
        name: "Wallpaper",
        isPrivate: true,
        file: null,
        version: null,
        size: 0,
        updatedAt: null,
        changelog: null,
    },
}

export async function initialize() {
    await updateDownloadsList();

    cron.schedule(`*/${globals.bunny.ftp.refreshIntervalMins} * * * *`, async () => {
        console.log("[i] Updating downloads...");
      
        await updateDownloadsList();
    });
}

/**
 * @param {string} ftpTime
 */
function ftpTimeToDate(ftpTime) {
    return DateTime.fromFormat(ftpTime, "MMM dd HH:mm", { zone: "UTC" });
}

async function updateDownloadsList() {
    const client = new Client(globals.bunny.ftp.timeout);

    try {
        await client.access({
            host: globals.bunny.ftp.hostname,
            port: globals.bunny.ftp.port,
            user: globals.bunny.ftp.username,
            password: globals.bunny.ftp.password,
            secure: true
        });

        for (let i = 0; i < directoriesToCheck.length; i++) {
            const directory = directoriesToCheck[i];

            try {
                const fileInfo = await client.list(directory);
                /** @type {FileInfo} */
                let newestFile = null;
                /** @type {string} */
                let changelog = null;
                for (let ii = 0; ii < fileInfo.length; ii++) {
                    const file = fileInfo[ii];

                    // Save the changelog for this dir if it exists
                    if (file.name === CHANGELOG_NAME) {
                        const dest = `temp/${CHANGELOG_NAME}`;
                        await fse.ensureDir("temp");

                        // Remove the local changelog if it exists
                        const destExists = await fse.exists(dest);
                        if (destExists) await fse.remove(dest);

                        await client.downloadTo(dest, `${directory}/${CHANGELOG_NAME}`);
                        changelog = (await fs.readFile(dest, { encoding: "utf-8" })).toString();
                        continue;
                    }
                    
                    // Skip non-exe files and directories
                    const fileExt = path.parse(file.name).ext;
                    if (file.isDirectory || (fileExt !== ".exe" && fileExt !== ".zip")) {
                        continue;
                    }

                    if (newestFile === null) newestFile = file;
        
                    const thisDate = ftpTimeToDate(file.rawModifiedAt);
                    const newestDate = ftpTimeToDate(newestFile.rawModifiedAt);
        
                    if (thisDate.diff(newestDate, 'milliseconds').milliseconds > 0) {
                        newestFile = file;
                    }
                }

                if (newestFile === null) {
                    downloadableFiles[directory].file = null;
                    downloadableFiles[directory].version = null;
                    downloadableFiles[directory].size = 0;
                    downloadableFiles[directory].updatedAt = null;
                    downloadableFiles[directory].changelog = null;
                    console.log(`Directory: "${directory}" was empty!`);
                } else {
                    downloadableFiles[directory].file = newestFile.name;
                    downloadableFiles[directory].version = extractVersion(path.parse(newestFile.name).name);
                    downloadableFiles[directory].size = newestFile.size;
                    downloadableFiles[directory].updatedAt = ftpTimeToDate(newestFile.rawModifiedAt).toRelative({ unit: ["days", "hours", "minutes"] });
                    downloadableFiles[directory].changelog = changelog;
                    console.log(`Directory: "${directory}" -> Newest File: "${newestFile.name}"`);
                }
            } catch (error) {
                console.log(`Error processing directory "${directory}": ${error}`);
            }
        }
    }
    catch(err) {
        console.log(`FTP ERROR: ${err}`);
    }

    client.close();
}

/**
 * @param {string} Session_ID
 */
export async function getDownloads(Session_ID) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as a possible session session harvesting attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId);

    const output = [];
    let anyDownloads = false;

    for (let i = 0; i < directoriesToCheck.length; i++) {
        const directory = directoriesToCheck[i];

        const entry = downloadableFiles[directory];

        if (entry.file === null) continue;

        if (user.PublicUser && entry.isPrivate) continue;

        output.push({
            name: entry.name,
            id: directory,
            version: entry.version,
            size: entry.size,
            updatedAt: entry.updatedAt,
            changelog: entry.changelog,
        });

        anyDownloads = true;
    }

    if (anyDownloads) return { success: true, message: output };
    else return { success: false, message: ["No downloads."] };
}

/**
 * @param {string} id
 */
export function getDownloadURL(id) {
    if (!directoriesToCheck.includes(id)) return { success: false, message: ["Invalid ID."] };

    const file = downloadableFiles[id].file;
    if (file == null) return { success: false, message: ["Empty download."] };

    return { success: true, message: signUrl(`https://updates.evodma.com/${id}/${file}`, minutesToSeconds(10)) };
}