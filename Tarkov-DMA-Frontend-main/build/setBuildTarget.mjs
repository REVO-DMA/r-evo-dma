import * as fs from "fs/promises";
import * as path from "path";
import * as url from "url";
import * as fse from "fs-extra";

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));

const command = process.argv[2];

if (command !== "private" && command !== "public") {
    console.log("Invalid command. Available commands:\n\tprivate\n\tpublic");
    process.exit(1);
}

(async () => {
    const binDir = path.join(__dirname, "../src/bin");
    fse.ensureDirSync(binDir);

    const versionsDir = path.join(__dirname, "../versions");

    const envFile = path.join(__dirname, "../src/renderer/env.ts");
    const env = (await fs.readFile(envFile)).toString();

    const pkgFile = path.join(__dirname, "../package.json");
    const pkg = (await fs.readFile(pkgFile)).toString();

    const privateVersion = (await fs.readFile(path.join(versionsDir, "private.version"))).toString();
    const publicVersion = (await fs.readFile(path.join(versionsDir, "public.version"))).toString();

    if (command === "private") {
        await fs.writeFile(envFile, env.replace("export const PUBLIC_BUILD = true;", "export const PUBLIC_BUILD = false;"));

        const versionReplaced = replaceLine(pkg, `"version": "`, `\t"version": "${privateVersion}",`);
        
        await fs.writeFile(pkgFile, versionReplaced.replace("${productName} ${version} PUB.${ext}", "${productName} ${version} PRIV.${ext}")
        .replace("https://software-updates.evodma.com/eft-dma-radar-toolkit-public", "https://software-updates.evodma.com/eft-dma-radar-toolkit")
        .replace("dist/public", "dist/private"));

        const protectedEXE = path.join(__dirname, "../../Tarkov-DMA-Backend/Publish_Commercial/Tarkov_DMA_Backend.cv.vmp.exe");
        fs.copyFile(protectedEXE, path.join(binDir, "backend.exe"));

        const updateHelperEXE = path.join(__dirname, "../../Tarkov-DMA-Backend/UpdateHelper/Publish_Commercial/UpdateHelper.exe");
        fs.copyFile(updateHelperEXE, path.join(binDir, "UpdateHelper.exe"));

        console.log("ENV Changed to Private!");
    } else if (command === "public") {
        await fs.writeFile(envFile, env.replace("export const PUBLIC_BUILD = false;", "export const PUBLIC_BUILD = true;"));

        const versionReplaced = replaceLine(pkg, `"version": "`, `\t"version": "${publicVersion}",`);
        
        await fs.writeFile(pkgFile, versionReplaced.replace("${productName} ${version} PRIV.${ext}", "${productName} ${version} PUB.${ext}")
        .replace("https://software-updates.evodma.com/eft-dma-radar-toolkit", "https://software-updates.evodma.com/eft-dma-radar-toolkit-public")
        .replace("dist/private", "dist/public"));

        const protectedEXE = path.join(__dirname, "../../Tarkov-DMA-Backend/Publish_Commercial_Public/Tarkov_DMA_Backend.cv.vmp.exe");
        fs.copyFile(protectedEXE, path.join(binDir, "backend.exe"));

        const updateHelperEXE = path.join(__dirname, "../../Tarkov-DMA-Backend/UpdateHelper/Publish_Commercial_Public/UpdateHelper.exe");
        fs.copyFile(updateHelperEXE, path.join(binDir, "UpdateHelper.exe"));
        
        console.log("ENV Changed to Public!");
    }
})();

function replaceLine(text, search, replacement)
{
    const lines = text.split('\n');

    for (let i = 0; i < lines.length; i++)
    {
        if (lines[i].includes(search))
        {
            lines[i] = replacement;
            break;
        }
    }

    return lines.join('\n');
}