const fs = require("fs/promises");
const path = require("path");

const distDir = path.join(__dirname, "../dist");

(async () => {
    await fs.copyFile(path.join(__dirname, "../package.json"), path.join(distDir, "package.json"));
    await fs.copyFile(path.join(__dirname, "../yarn.lock"), path.join(distDir, "yarn.lock"));
})();
