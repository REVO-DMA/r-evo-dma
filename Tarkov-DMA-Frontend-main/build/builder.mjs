import fse from "fs-extra";
import path from "path";
import { fileURLToPath } from "url";
import webpack from "webpack";
import WebpackDevServer from "webpack-dev-server";
import { merge } from "webpack-merge";
import log from "./logger.mjs";
import webpack_radar_common from "./webpack/radar/webpack.common.mjs";
import webpack_radar_dev from "./webpack/radar/webpack.dev.mjs";
import webpack_radar_prod from "./webpack/radar/webpack.prod.mjs";
import webpack_main from "./webpack/webpack.main.mjs";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const command = process.argv[2];

if (command !== "dev" && command !== "prod" && command !== "main" && command !== "clean") {
    console.log("Invalid command. Available commands:\n\tdev\n\tprod\nmain\nclean");
    process.exit(1);
}

(async () => {
    function runBuild() {
        log.info(`[BUILD] ${log.info(`Running ${command}...`, false)}`);

        return new Promise(async (resolve, reject) => {
            if (command === "dev") {
                // Radar Dev server
                const radarDevCompiler = webpack(webpack_radar_dev);
                const radarDevServerOptions = { ...webpack_radar_dev.devServer, open: true };
                const radarDevServer = new WebpackDevServer(radarDevServerOptions, radarDevCompiler);

                log.info(`[DEV-SERVER] ${log.success("Starting Radar dev server...", false)}`);
                await radarDevServer.start();

                resolve(log.success(`[DEV-SERVER] ${log.info("Dev server for Radar is ready!", false)}`, false));
            } else if (command === "prod") {
                webpack(merge(webpack_radar_common, webpack_radar_prod), (err, stats) => {
                    if (err || stats.hasErrors()) {
                        // Display all errors
                        if (stats != null && stats.hasErrors()) {
                            stats.compilation.errors.forEach((err) => {
                                log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                            });
                        } else {
                            log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                        }
                        reject(log.success(`[WEBPACK ERROR] ${log.info("Unable to build Radar frontend!", false)}`, false));
                    } else {
                        resolve(log.success(`[BUILD] ${log.info("Successfully built Radar frontend!", false)}`, false));
                    }
                });
            } else if (command === "main") {
                webpack(webpack_main, (err, stats) => {
                    if (err || stats.hasErrors()) {
                        // Display all errors
                        if (stats != null && stats.hasErrors()) {
                            stats.compilation.errors.forEach((err) => {
                                log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                            });
                        } else {
                            log.error(`[WEBPACK ERROR] ${log.warning(err, false)}`);
                        }
                        reject(log.success(`[WEBPACK ERROR] ${log.info("Unable to build main!", false)}`, false));
                    } else {
                        resolve(log.success(`[BUILD] ${log.info("Successfully built main!", false)}`, false));
                    }
                });
            } else if (command === "clean") {
                log.info(`[PRE-BUILD] ${log.info("Clearing webpack-output directory...", false)}`);
                fse.emptyDirSync(path.join(__dirname, "../webpack-output"));
            }
        });
    }

    await runBuild()
        .catch((onrejected) => {
            console.log(onrejected);
        })
        .then((onfulfilled) => {
            console.log(onfulfilled);
        });
})();
