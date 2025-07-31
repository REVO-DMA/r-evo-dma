import { EsbuildPlugin } from "esbuild-loader";
import path from "path";
import { fileURLToPath } from "url";
import { WebpackObfuscatorPlugin } from "webpack-obfuscator/dist/plugin/index.js";
import { default as obfuscatorConfig } from "./obfuscatorConfig.mjs";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const proj_root = path.join(__dirname, "../../");

/** @type {import("webpack").Configuration} */
const webpack_main = {
    target: "electron-main",
    node: {
        __dirname: false,
    },
    mode: "production",
    stats: "none",
    entry: [path.join(proj_root, "src/main/main.ts")],
    output: {
        filename: "[name].js",
        path: path.join(proj_root, "webpack-output/main"),
    },
    externalsPresets: {
        electronMain: true,
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ]
    },
    optimization: {
        moduleIds: "natural",
        chunkIds: "natural",
        runtimeChunk: "single",
        splitChunks: {
            chunks: "all",
        },
        minimizer: [
            new EsbuildPlugin({
                target: "es2020",
                css: true,
                minify: true,
                legalComments: "none",
            }),
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    plugins: [new WebpackObfuscatorPlugin(obfuscatorConfig)],
};

export default webpack_main;
