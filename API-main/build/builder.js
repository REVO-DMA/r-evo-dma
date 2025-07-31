import fse from "fs-extra";
import fs from "fs/promises";
import path from "path";
import { fileURLToPath } from "url";
import log from "./logger.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const srcDir = path.join(__dirname, "../src");
const distDir = path.join(__dirname, "../dist");

const command = process.argv[2];

if (command !== "dev" && command !== "prod") {
    console.log("Invalid command. Available commands:\n\tdev\n\tprod");
    process.exit(1);
}

(async () => {
    log.info(`[PRE-BUILD] ${log.info("Clearing dist directory...", false)}`);
    fse.emptyDirSync(distDir);

    log.info(`[BUILD] ${log.info("Building API...", false)}`);

    await fse.copy(srcDir, distDir);
    await fs.copyFile(path.join(__dirname, "../package.json"), path.join(distDir, "package.json"));
    await fs.copyFile(path.join(__dirname, "../yarn.lock"), path.join(distDir, "yarn.lock"));

    if (command == "dev") {
        await fs.writeFile(path.join(distDir, "ENVIRONMENT"), "DEV");
    } else if (command === "prod") {
        await fs.writeFile(path.join(distDir, "ENVIRONMENT"), "PROD");
    }

    log.success(`[BUILD] ${log.info("Successfully built API!", false)}`);
})();
