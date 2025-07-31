import { EsbuildPlugin } from "esbuild-loader";
import { WebpackObfuscatorPlugin } from "webpack-obfuscator/dist/plugin/index.js";
import { default as obfuscatorConfig } from "../obfuscatorConfig.mjs";

/** @type {import("webpack").Configuration} */
const webpack_prod = {
    mode: "production",
    stats: "none",
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
    plugins: [new WebpackObfuscatorPlugin(obfuscatorConfig)],
};

export default webpack_prod;
