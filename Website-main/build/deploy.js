const fs = require("fs/promises");
const path = require("path");
const { Client: sshClient } = require("ssh2");
const sftpClient = require("ssh2-sftp-client");

/** @type {sftpClient} */
let sftp;
/** @type {sshClient} */
let ssh;

// Read command-line arguments
let [environment, host, port, username, privateKey, passphrase] = process.argv.slice(2);

if (environment == null || environment === "") {
    console.log("ERROR: Missing environment argument! Choose one of:\n\tprod\n\tdev");
    process.exit(1);
}

/**
 * The mode of configuration. 1 = arguments, 2 = environment vars.
 * @type {(1|2)}
 */
let workingMode = 1;

// If any of the args were nullish, get sftp/ssh from environment variables
if (host == null || port == null || username == null || privateKey == null || passphrase == null) {
    host = process.env.HOST;
    port = process.env.PORT;
    username = process.env.USERNAME;
    privateKey = process.env.KEY;
    passphrase = process.env.PASSPHRASE;

    workingMode = 2;
}

(async () => {
    if (workingMode === 1) {
        const privKey = await fs.readFile(privateKey);

        await initSFTP(privKey);
        await initSSH(privKey);
    } else if (workingMode === 2) {
        await initSFTP(privateKey);
        await initSSH(privateKey);
    }

    await copyNginxConfigs();
    await deployWeb();
    await fixWebPermissions();

    await sftp.end();
    ssh.end();
})();

/**
 * @param {Buffer|string} privateKey
 * @returns {Promise<void>}
 */
function initSSH(privateKey) {
    return new Promise((resolve) => {
        ssh = new sshClient();
        ssh.on("ready", () => {
            resolve();
        }).connect({
            host: host,
            port: Number(port),
            username: username,
            privateKey: privateKey,
            passphrase: passphrase,
        });
    });
}

/**
 * @param {string} command
 * @returns {Promise<void>}
 */
async function sshExec(command) {
    return new Promise((resolve) => {
        ssh.exec(command, (err, stream) => {
            if (err) throw err;

            let outputBuffer = "";

            const handleOutput = (string) => {
                if (outputBuffer.length === 0) console.log(`(${command}) ==== BEGIN ====`);

                outputBuffer += string;

                console.log(string);
            };

            stream
                .on("close", (code, signal) => {
                    if (outputBuffer.length === 0) console.log(`(${command}) ==== NO OUTPUT [${code}] ====\n`);
                    else console.log(`(${command}) ==== END [${code}] ====\n`);

                    resolve();
                })
                .on("data", (data) => {
                    handleOutput(data.toString());
                })
                .stderr.on("data", (data) => {
                    handleOutput(data.toString());
                });
        });
    });
}

/**
 * @param {Buffer|string} privateKey
 */
async function initSFTP(privateKey) {
    sftp = new sftpClient();
    await sftp.connect({
        host: host,
        port: Number(port),
        username: username,
        privateKey: privateKey,
        passphrase: passphrase,
    });
}

/**
 * Returns a custom formatted environment string.
 */
function getENV() {
    return `[ENV:${environment.toUpperCase()}]`;
}

/**
 * Copies all NGINX configs from the root `nginx` folder to the remote `conf.d` folder and restarts NGINX.
 */
async function copyNginxConfigs() {
    const remoteDest = "/etc/nginx/conf.d/";
    const nginxDir = path.join(__dirname, "../nginx");

    console.log(`${getENV()} Emptying ${remoteDest}...`);
    await sshExec(`rm -rf ${remoteDest}*`);

    const files = await fs.readdir(nginxDir);

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const source = path.join(nginxDir, file);
        const destination = `${remoteDest}${file}`;

        console.log(`${getENV()} Copying NGINX config ${file} from local "${source}" to remote "${destination}"...`);

        await sftp.put(source, destination, {
            writeStreamOptions: {
                flags: "w",
                encoding: null,
                mode: 0o666,
            },
        });
    }

    console.log("Checking new configs & restarting NGINX...");
    await sshExec("nginx -t");
    await sshExec("systemctl restart nginx");
}

/**
 * Copies all files from the root `dist` folder to the remote web directory (served by NGINX) respective of the configured environment.
 */
async function deployWeb() {
    const source = path.join(__dirname, "../dist");

    let destination = "/var/www/";
    if (environment === "prod") destination += "evodma.com";
    else if (environment === "dev") destination += "dev-web.evodma.com";

    console.log(`${getENV()} Emptying "${destination}"...`);
    await sshExec(`rm -rf ${destination}/*`);

    console.log(`${getENV()} Deploying "${source}" to "${destination}"...`);

    const result = await sftp.uploadDir(source, destination);
    console.log(result);
}

/**
 * Sets proper permissions and ownership on all files/folders in and below `/var/www`.
 */
async function fixWebPermissions() {
    console.log("Fixing web permissions and ownership...");

    await sshExec("find /var/www -type d -exec chmod 755 {} \\; && find /var/www -type f -exec chmod 644 {} \\;");
    await sshExec("chown -R www-data /var/www");
}
