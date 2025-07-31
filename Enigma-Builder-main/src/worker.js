const { execFile } = require('child_process');
const path = require('path');

const enigmaRunner = "../bin/enigmaRunner/EnigmaRunner.exe";

function runExecutable(args) {
    return new Promise((resolve, reject) => {
        execFile(path.join(__dirname, enigmaRunner), args, (error, stdout, stderr) => {
            if (error) {
                reject({ error, stdout, stderr });
            } else {
                resolve({ stdout, stderr });
            }
        });
    });
}

module.exports = (args, callback = null) => {
    runExecutable(args).then(({ stdout, stderr }) => {
        console.log("================[OK] Enigma Runner Output================");
        console.log(stdout);
        console.log(stderr);
        console.log("==============[OK] End Enigma Runner Output==============");
    }).catch(({ error, stdout, stderr }) => {
        console.log("================[ERR] Enigma Runner Output================");
        console.log(error);
        console.log(stdout);
        console.log(stderr);
        console.log("==============[ERR] End Enigma Runner Output==============");
    });
};