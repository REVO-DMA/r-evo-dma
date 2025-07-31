import CopyPlugin from "copy-webpack-plugin";
import HtmlWebpackPlugin from "html-webpack-plugin";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import path from "path";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const proj_root = path.join(__dirname, "../../../");

/** @type {HtmlWebpackPlugin.Options} */
const HtmlWebpackPluginConfig = {
    inject: true,
    template: "src/renderer/index.html",
};

const MiniCssExtractPluginConfig = {
    filename: "css/[name].css",
};

/** @type {import("webpack").Configuration} */
const webpack_common = {
    target: "electron-renderer",
    entry: [path.join(proj_root, "src/renderer/renderer.ts")],
    output: {
        filename: "[name].js",
        path: path.join(proj_root, "webpack-output/renderer"),
    },
    externalsPresets: {
        electronRenderer: true,
    },
    module: {
        rules: [
            {
                test: /\.(html)$/,
                use: ["html-loader"],
            },
            {
                test: /\.(sass|scss|css)$/,
                use: [MiniCssExtractPlugin.loader, "css-loader", "sass-loader"],
            },
            {
                test: /\.(svg|eot|woff|woff2|ttf)$/,
                type: "asset/resource",
                exclude: /(img)/,
                generator: {
                    filename: "fonts/[name][ext]",
                },
            },
            {
                test: /\.(png|jpg|jpeg|svg|webp)$/,
                type: "asset",
                exclude: /(fonts)/,
                generator: {
                    filename: "img/[name][ext]",
                },
            },
            {
                test: /\.node$/,
                loader: "node-loader",
            },
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    plugins: [
        new HtmlWebpackPlugin(HtmlWebpackPluginConfig),
        new MiniCssExtractPlugin(MiniCssExtractPluginConfig),
        new CopyPlugin({
            patterns: [
                {
                    from: path.join(proj_root, "src/bin/backend.exe"),
                    to: path.join(proj_root, "webpack-output/bin/backend.exe"),
                },
                {
                    from: path.join(proj_root, "src/bin/UpdateHelper.exe"),
                    to: path.join(proj_root, "webpack-output/bin/UpdateHelper.exe"),
                },
            ],
        }),
    ],
};

export default webpack_common;
