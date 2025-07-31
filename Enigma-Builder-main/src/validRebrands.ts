export interface RebrandEntry {
    Name: string;
    OwnerID: string;
    Secret: string;
    Version: string;
};

export const ValidRebrands = new Map<string, RebrandEntry>([
    [
        "main",
        {
            Name: "Sickk",
            OwnerID: "cvGajjqnnF",
            Secret: "66f95ecde4bd49f4b1ab7e94b547a21538314513631fafe5b1f47067a734a8c4",
            Version: "1.2.2"
        }
    ],
    [
        "arsonReseller",
        {
            Name: "Neb Reseller",
            OwnerID: "cvGajjqnnF",
            Secret: "954c5a3e15215d5ed56143f61d8be7761cac41ef170df454c88c9d0b07c639e7",
            Version: "1.2.2"
        }
    ],
    [
        "aqua",
        {
            Name: "Aqua",
            OwnerID: "cvGajjqnnF",
            Secret: "72a34f5f745f59e4017fced593df08404b3339e01377b4d1187a0d38e282a79f",
            Version: "1.2.2"
        }
    ],
    [
        "ALLService",
        {
            Name: "All Service",
            OwnerID: "cvGajjqnnF",
            Secret: "d65bab71a7bf0b201f128793b385b9b2e16ec15e6e2ccd5eb9028e7ac7bf88ba",
            Version: "1.2.2"
        }
    ]
]);