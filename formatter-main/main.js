const { spawn } = require("child_process");
const path = require("path");
const process = require("process");

const args = [
    "prettier",
    "--config"
]

// Basic arg count check
if (process.argv.length <= 3) {
    console.log(`Invalid or no arguments supplied. Usage:\n\tnode main.js <prettier config> <target file/folder> <target file/folder> etc...`);
    process.exit(1);
}

// Get the cwd to run yarn prettier under
const spawnCwd = path.join(process.cwd(), path.parse(path.relative(process.cwd(), process.argv[1])).dir);

// Add supplied prettier config to args
args.push(path.resolve(process.cwd(), process.argv[2]));

args.push("--write");

// Convert supplied relative paths to absolute paths
for (let i = 2; i < process.argv.length; i++) {
    const arg = process.argv[i];

    args.push(path.resolve(process.cwd(), arg));
}

const cmd = spawn("yarn", args, { shell: true, cwd: spawnCwd });

cmd.stdout.on("data", (data) => {
    console.log(data.toString().trim());
});

cmd.stderr.on("data", data => {
    console.log(`Formatter Error:\n\t${data}`);
});

cmd.on('error', (error) => {
    console.log(`Formatter Error:\n\t${error.message}`);
});

cmd.on("close", (code) => {
    console.log(`Formatter exited with code: ${code}`);
});