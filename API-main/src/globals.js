import fs from "fs/promises";
import path from "path";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

/** @type {("LOCAL"|"DEV"|"PROD")} */
export let ENVIRONMENT = "DEV";

(async () => {
    //ENVIRONMENT = (await fs.readFile(path.join(__dirname, "ENVIRONMENT"))).toString();
})();

const globals = {
    http: {
        port: 8081,
    },
    stripe: {
        secretKey: "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",
        webhookSigningSecret: "whsec_D6mjkxZlnKLZqlzv5RHrBxQbIaqJuYZf",
    },
    sessionLifetime: 14, // Session lifetime in days
    database: {
        name: "test",
        username: "postgres",
        password: "frcnXeVEkhbA4uWnR78kYW7U6Rb4z28dxmSapW3E4eQdFtFK4JimBSjeSxU5gQrrMtEk8gJQKstEhkhyDJtrUWqE2vQU9Ln5xaAJVHKUUPUZhamUPsyaBLweChBicxaR",
        host: "5.161.201.115",
        logging: console.log,
    },
    email: {
        noreply: {
            from: "noreply@evodma.com",
            host: "mail.roarkw3.com",
            port: 587,
            secure: false,
            auth: {
                user: "noreply@evodma.com",
                pass: "dBgKuntmSJj76neavuQ44uq4UTkCeF2vRfu5PC79SV8MM86dapSXJQX7beTNBGrVNmUwqPEHqeU4rwqwRTdb34DVgBUPgGxgL2CyssX6XPwkf4YBLndRs23uxGn3yvM6",
            },
        },
    },
    ui: {
        url: "http://localhost:8080",
    },
    bunny: {
        urlTokenKey: "698dff25-890d-4baf-97a9-463c87413e7d",
        ftp: {
            refreshIntervalMins: 1,
            timeout: 6000,
            hostname: "ny.storage.bunnycdn.com",
            port: "21",
            username: "evo-updates-1",
            password: "82a5f88c-01fe-4782-8c4f89529648-ca80-4425",
        }
    }
};

if (ENVIRONMENT === "PROD") {
    globals.stripe.secretKey = "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";
    globals.stripe.webhookSigningSecret = "whsec_TF6CMm1zcsA5GDqpvgMDMXez0e2Quum5";
    //globals.database.logging = false;
    globals.ui.url = "https://evodma.com";
    //globals.http.port = 8080;
} else if (ENVIRONMENT === "DEV") {
    globals.ui.url = "https://dev-web.evodma.com";
}

export default globals;
