import axios from 'axios';

interface GenericResponse {
    Success: boolean;
    Message: string;
}

interface InitializeResponse {
    Success: boolean;
    Message: string;
    SessionID: string;
}

export class KeyAuth {
    Initialized: boolean = false;
    
    name: string;
    ownerId: string;
    secret: string;
    version: string;

    SessionID: string = "";

    constructor(name: string, ownerId: string, secret: string, version: string) {
        this.name = name
        this.ownerId = ownerId
        this.secret = secret
        this.version = version
    };

    async Initialize(): Promise<boolean> {
        const post_data = {
            type: 'init',
            ver: this.version,
            name: this.name,
            ownerid: this.ownerId
        }

        const resp = await this.RequestInit(new URLSearchParams(post_data));

        if (resp == null) return false;
        else if (resp.Success === true) {
            this.Initialized = true;
            this.SessionID = resp.SessionID;

            return true;
        }

        return false;
    }

    async LicenseValid(key: string, hwid: string): Promise<boolean> {
        if (!this.Initialized) {
            console.error("[KA LIB] Error: Not initialized!");
            return false;
        }

        const post_data = {
            type: 'license',
            key: key,
            hwid: hwid,
            sessionid: this.SessionID,
            name: this.name,
            ownerid: this.ownerId
        }

        const resp = await this.Request(new URLSearchParams(post_data))

        if (resp == null) return false;
        else if (resp.Success === true) return true;

        return false;
    }

    async IsBlacklisted(hwid: string): Promise<boolean> {
        if (!this.Initialized) {
            console.error("[KA LIB] Error: Not initialized!");
            return false;
        }

        const post_data = {
            type: 'checkblacklist',
            hwid: hwid,
            sessionid: this.SessionID,
            name: this.name,
            ownerid: this.ownerId
        };

        const resp = await this.Request(new URLSearchParams(post_data));

        if (resp == null) return false;
        else if (resp.Success === true) return true;

        return false;
    }

    async Request(data: URLSearchParams): Promise<GenericResponse | null> {
        const d = await this._RequestRaw(data);

        if (d != null) {
            return {
                Success: d.success,
                Message: d.message,
            }
        }
        else return null;
    }

    async RequestInit(data: URLSearchParams): Promise<InitializeResponse | null> {
        const d = await this._RequestRaw(data);

        if (d != null) {
            return {
                Success: d.success,
                Message: d.message,
                SessionID: d.sessionid,
            }
        }
        else return null;
    }

    async _RequestRaw(data: URLSearchParams): Promise<any | null> {
        const request = await axios({
            method: 'POST',
            url: 'https://keyauth.win/api/1.1/',
            data: data.toString()
        }).catch((err: any) => {
            console.error(err)
        });

        if (request != null && request.data) return request.data;
        else return null;
    }
}