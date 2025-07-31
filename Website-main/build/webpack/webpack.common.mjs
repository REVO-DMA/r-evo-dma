import HtmlWebpackPlugin from "html-webpack-plugin";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import path from "path";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const proj_root = path.join(__dirname, "../../");

/** @type {HtmlWebpackPlugin.Options} */
const HtmlWebpackPluginConfig = {
    inject: true,
    template: "index.html",
};

const MiniCssExtractPluginConfig = {
    filename: "css/[name].[contenthash].css",
};

/** @type {import("webpack").Configuration} */
const webpack_common = {
    entry: [path.join(proj_root, "src/index.js")],
    output: {
        filename: "[name].[contenthash].js",
        path: path.join(proj_root, "dist"),
        publicPath: "/",
        clean: true,
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
                    filename: "fonts/[name].[hash][ext]",
                },
            },
            {
                test: /\.(png|jpg|jpeg|svg)$/,
                type: "asset/inline",
                exclude: /(fonts)/,
            },
        ],
    },
    plugins: [new HtmlWebpackPlugin(HtmlWebpackPluginConfig), new MiniCssExtractPlugin(MiniCssExtractPluginConfig)],
};

export default webpack_common;
