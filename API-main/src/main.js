import { initialize as initializeCanvas } from "./canvas/canvas.js";
import { initialize as initializeDatabase, shutdown as shutdownDatabase } from "./database/initialize.js";
import { initialize as initializeEmail, shutdown as shutdownEmail } from "./emails/emails.js";
import { initialize as initializeDownloads } from "./server/downloads.js";
import { initialize as initializeServer, shutdown as shutdownServer } from "./server/server.js";
import { initialize as initializeStripe } from "./Stripe/Stripe.js";

(async () => {
    await initializeDownloads();
    await initializeCanvas();
    await initializeDatabase();
    initializeEmail();
    initializeStripe();
    initializeServer();
})();

process.on("exit", async (code) => {
    console.log(`Cleaning up before exit...`);

    await shutdownServer();
    await shutdownDatabase();
    shutdownEmail();
});

// (Ctrl+C)
process.on("SIGINT", () => {
    console.log("App received SIGINT");
    process.exit(0);
});

process.on("SIGTERM", () => {
    console.log("App received SIGTERM");
    process.exit(0);
});
