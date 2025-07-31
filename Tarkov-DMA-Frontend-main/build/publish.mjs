import { Client as ftpClient } from "basic-ftp";
import { fileURLToPath } from "url";
import * as path from "path";
import * as fs from "fs/promises";
import { existsSync } from "fs";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

(async () => {
    const client = new ftpClient(60000);

    await client.access({
        host: "ny.storage.bunnycdn.com",
        port: 21,
        user: "software-updates",
        password: "f17a83e1-00d6-4bd4-94f555d33651-d5f0-4651",
        secure: true
    });

    const privateDir = path.join(__dirname, "../dist/private");
    const publicDir = path.join(__dirname, "../dist/public");

    if (existsSync(privateDir))
    {
        const files = await fs.readdir(privateDir);

        const exeFile = files.filter(file => path.extname(file).toLowerCase() === '.exe')[0];

        console.log("Uploading private exe...");

        await client.uploadFrom(path.join(privateDir, exeFile), `eft-dma-radar-toolkit/${exeFile}`);

        let channel = "latest";
        if (exeFile.includes("-beta"))
        {
            await client.uploadFrom(path.join(privateDir, "beta.yml"), "eft-dma-radar-toolkit/beta.yml");
            channel = "beta";
        }
        else if (exeFile.includes("-alpha"))
        {
            await client.uploadFrom(path.join(privateDir, "alpha.yml"), "eft-dma-radar-toolkit/alpha.yml");
            channel = "alpha";
        }
        else
        {
            await client.uploadFrom(path.join(privateDir, "beta.yml"), "eft-dma-radar-toolkit/beta.yml");
            await client.uploadFrom(path.join(privateDir, "alpha.yml"), "eft-dma-radar-toolkit/alpha.yml");
            await client.uploadFrom(path.join(privateDir, "latest.yml"), "eft-dma-radar-toolkit/latest.yml");
        }

        console.log(`Uploaded private exe and bumped the ${channel} version.`);
    }
    
    if (existsSync(publicDir))
    {
        const files = await fs.readdir(publicDir);

        const exeFile = files.filter(file => path.extname(file).toLowerCase() === '.exe')[0];

        console.log("Uploading public exe...");

        await client.uploadFrom(path.join(publicDir, exeFile), `eft-dma-radar-toolkit-public/${exeFile}`);

        let channel = "latest";
        if (exeFile.includes("-beta"))
        {
            await client.uploadFrom(path.join(publicDir, "beta.yml"), "eft-dma-radar-toolkit-public/beta.yml");
            channel = "beta";
        }
        else if (exeFile.includes("-alpha"))
        {
            await client.uploadFrom(path.join(publicDir, "alpha.yml"), "eft-dma-radar-toolkit-public/alpha.yml");
            channel = "alpha";
        }
        else
        {
            await client.uploadFrom(path.join(publicDir, "beta.yml"), "eft-dma-radar-toolkit-public/beta.yml");
            await client.uploadFrom(path.join(publicDir, "alpha.yml"), "eft-dma-radar-toolkit-public/alpha.yml");
            await client.uploadFrom(path.join(publicDir, "latest.yml"), "eft-dma-radar-toolkit-public/latest.yml");
        }

        console.log(`Uploaded public exe and bumped the ${channel} version.`);
    }

    client.close();

    console.log("All data uploaded to Bunny!");
})();