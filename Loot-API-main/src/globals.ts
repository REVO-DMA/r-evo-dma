const globals = {
    general: {
        defaultPrice: 0,
    },
    archive: {
        dateToUse: "", // The archived loot date to force serve to clients (format m-d-yyyy - 8-9-2024)
        maxAgeDays: 90, // The max age a loot date file can be before it is deleted
        directory: "../loot_archive",
    },
    loot: {
        server: "https://api.tarkov.dev/graphql",
        refreshIntervalMins: 5,
    },
    bunny: {
        urlTokenKey: "8e050484-eae3-47b6-a21b-6b4a2e375e6a",
        ftp: {
            refreshIntervalMins: 1,
            timeout: 6000,
            hostname: "ny.storage.bunnycdn.com",
            port: 21,
            username: "loot-data",
            password: "64eee820-ef43-41ac-b9b37e086411-88ac-409d",
        }
    }
};

export default globals;