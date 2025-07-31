import { initialize as initializeDatabase } from "./database";
import { initialize as initializeServer } from "./server";

(async () => {
  await initializeDatabase();
  await initializeServer();
})();