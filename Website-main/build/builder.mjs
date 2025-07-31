import fse from "fs-extra";
import path from "path";
import { fileURLToPath } from "url";
import webpack from "webpack";
import WebpackDevServer from "webpack-dev-server";
import { merge } from "webpack-merge";
import log from "./logger.mjs";
import webpack_common from "./webpack/webpack.common.mjs";
import webpack_dev from "./webpack/webpack.dev.mjs";
import webpack_prod from "./webpack/webpack.prod.mjs";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const command = process.argv[2];

if (command !== "dev" && command !== "prod") {
    console.log("Invalid command. Available commands:\n\tdev\n\tprod");
    process.exit(1);
}

(async () => {
    log.info(`[PRE-BUILD] ${log.info("Clearing dist directory...", false)}`);
    fse.emptyDirSync(path.join(__dirname, "../dist"));

    function buildWebsite() {
        log.info(`[BUILD] ${log.info("Building Website...", false)}`);

        return new Promise(async (resolve, reject) => {
            if (command === "dev") {
                const compiler = webpack(webpack_dev);
                const devServerOptions = { ...webpack_dev.devServer, open: true };
                const server = new WebpackDevServer(devServerOptions, compiler);

                log.info(`[DEV-SERVER] ${log.success("Starting dev server...", false)}`);
                await server.start();

                resolve(log.success(`[DEV-SERVER] ${log.info("Dev server ready!", false)}`, false));
            } else if (command === "prod") {
                webpack(merge(webpack_common, webpack_prod), (err, stats) => {
                    if (err || stats.hasErrors()) {
                        // Display all errors
                        if (stats != null && stats.hasErrors()) {
                            stats.compilation.errors.forEach((err) => {
                                log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                            });
                        } else {
                            log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                        }
                        reject(log.success(`[WEBPACK ERROR] ${log.info("Unable to build Website!", false)}`, false));
                    } else {
                        resolve(log.success(`[BUILD] ${log.info("Successfully built Website!", false)}`, false));
                    }
                });
            }
        });
    }

    await buildWebsite()
        .catch((onrejected) => {
            console.log(onrejected);
        })
        .then((onfulfilled) => {
            console.log(onfulfilled);
        });
})();
