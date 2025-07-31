import { initialize as initializeLootUpdates } from "./eft/update";
import { start as startServer } from "./server";

(async () => {
    startServer();
    initializeLootUpdates(); // This can take a while
})();