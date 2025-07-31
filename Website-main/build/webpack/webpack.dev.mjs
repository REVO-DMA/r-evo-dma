import { execSync } from "child_process";
import fs from "fs";
import { GitRevisionPlugin } from "git-revision-webpack-plugin";
import path from "path";
import { fileURLToPath } from "url";
import webpack from "webpack";
import { merge } from "webpack-merge";
import webpack_common from "./webpack.common.mjs";

const gitRevisionPlugin = new GitRevisionPlugin();

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const proj_root = path.join(__dirname, "../../");

const version = JSON.parse(fs.readFileSync(path.join(proj_root, "package.json"))).version;

const gitRevision = execSync("git rev-parse HEAD").toString().trim();

/** @type {import("webpack-dev-server").WebpackConfiguration} */
const webpack_dev = merge(webpack_common, {
    mode: "development",
    devtool: "inline-source-map",
    devServer: {
        setupExitSignals: false,
        server: "http",
        port: 8080,
        compress: true,
        historyApiFallback: {
            disableDotRule: true,
        },
        client: {
            logging: "verbose",
            reconnect: false,
            progress: true,
        },
        watchFiles: ["src/**"],
        liveReload: true,
        hot: false,
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                loader: "esbuild-loader",
                options: {
                    target: "es2015",
                },
            },
        ],
    },
    plugins: [
        gitRevisionPlugin,
        new webpack.DefinePlugin({
            VERSION: JSON.stringify(version),
            COMMIT: JSON.stringify(gitRevisionPlugin.version()),
            COMMITHASH: JSON.stringify(gitRevisionPlugin.commithash()),
            LASTCOMMITDATETIME: JSON.stringify(gitRevisionPlugin.lastcommitdatetime()),
            GIT_REVISION: JSON.stringify(gitRevision),
        }),
    ],
});

export default webpack_dev;
