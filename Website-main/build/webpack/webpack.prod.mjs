import { execSync } from "child_process";
import { EsbuildPlugin } from "esbuild-loader";
import FaviconsWebpackPlugin from "favicons-webpack-plugin";
import fs from "fs";
import { GitRevisionPlugin } from "git-revision-webpack-plugin";
import ImageMinimizerPlugin from "image-minimizer-webpack-plugin";
import path from "path";
import { fileURLToPath } from "url";
import webpack from "webpack";

const gitRevisionPlugin = new GitRevisionPlugin();

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const proj_root = path.join(__dirname, "../../");

const version = JSON.parse(fs.readFileSync(path.join(proj_root, "package.json"))).version;

const gitRevision = execSync("git rev-parse HEAD").toString().trim();

/** @type {import("webpack").Configuration} */
const webpack_prod = {
    mode: "production",
    stats: "none",
    optimization: {
        moduleIds: "deterministic",
        runtimeChunk: "single",
        splitChunks: {
            chunks: "all",
        },
        minimizer: [
            new ImageMinimizerPlugin({
                minimizer: {
                    implementation: ImageMinimizerPlugin.imageminMinify,
                    options: {
                        plugins: [
                            ["jpegtran", { progressive: true }],
                            ["optipng", { optimizationLevel: 3 }],
                        ],
                    },
                },
                exclude: [/^[^.]+.svg$/],
            }),
            new EsbuildPlugin({
                target: "es2015",
                css: true,
            }),
        ],
    },
    plugins: [
        new FaviconsWebpackPlugin({
            logo: path.join(proj_root, "src/img/favicon.svg"),
            outputPath: "img/assets",
            prefix: "img/assets/",
            mode: "webapp",
            cache: true,
            inject: true,
            favicons: {
                appName: "EVO DMA",
                appDescription: "EVO DMA Website",
                developerName: "EVO DMA",
                developerURL: null,
                version: version,
                logging: true,
                icons: {
                    android: false,
                    appleIcon: false,
                    appleStartup: false,
                    coast: false,
                    favicons: true,
                    firefox: false,
                    windows: false,
                    yandex: false,
                },
            },
        }),
        gitRevisionPlugin,
        new webpack.DefinePlugin({
            VERSION: JSON.stringify(version),
            COMMIT: JSON.stringify(gitRevisionPlugin.version()),
            COMMITHASH: JSON.stringify(gitRevisionPlugin.commithash()),
            LASTCOMMITDATETIME: JSON.stringify(gitRevisionPlugin.lastcommitdatetime()),
            GIT_REVISION: JSON.stringify(gitRevision),
        }),
    ],
};

export default webpack_prod;
