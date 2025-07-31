import { RebrandEntry } from "../validRebrands";
import { KeyAuth } from "./keyAuth";

export class AuthWrapper extends KeyAuth {
    LicenseKey: string;
    HWID: string;

    constructor(licenseKey: string, hwid: string, rebrand: RebrandEntry) {
        super(
            rebrand.Name,
            rebrand.OwnerID,
            rebrand.Secret,
            rebrand.Version
        );

        this.LicenseKey = licenseKey;
        this.HWID = hwid;
    }

    async IsGoodUser(): Promise<boolean> {
        if (!await this.Initialize()) return false;
        if (await this.IsBlacklisted(this.HWID)) return false;
        if (await this.LicenseValid(this.LicenseKey, this.HWID)) return true;

        return false;
    }
}