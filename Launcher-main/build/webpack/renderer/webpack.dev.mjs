import { merge } from "webpack-merge";
import webpack_common from "./webpack.common.mjs";

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
                    target: "es2020",
                },
            },
        ],
    },
});

export default webpack_dev;
