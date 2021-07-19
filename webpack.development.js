const path = require('path');
const TerserJSPlugin = require('terser-webpack-plugin');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = [{
    mode: 'production',
    performance: {
        hints: false
    },
    optimization: {
        minimize: false
    },
    entry: {
        bundle: './bundle.js', 
        light: './_assets/bootstrap-light.scss', 
        dark: './_assets/bootstrap-dark.scss'
    },
    output: {
        filename: "[name].js",
        path: path.resolve(__dirname, "packed")
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css",
            chunkFilename: "[name].css"
        }),
        new CleanWebpackPlugin()
    ],
    module: {
        rules: [
            {
                test: require.resolve("jquery"),
                loader: "expose-loader",
                options: {
                    exposes: ["$", "jQuery"],
                },
            },
            {
                test: require.resolve("lunr"),
                loader: "expose-loader",
                options: {
                    exposes: ["lunr"],
                },
            },
            {
                test: require.resolve("marked"),
                loader: "expose-loader",
                options: {
                    exposes: ["marked"],
                },
            },
            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                use: [
                    {
                        loader: "url-loader",
                        options: {
                            limit: 8000,
                            mimetype: "application/font-woff"
                        }
                    }
                ],
            },
            {
                test: /\.html$/,
                use: ["html-loader"]
            },
            {
                test: /\.(ttf|eot|svg|png|jpg|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "file-loader",
                options: {
                    name: "[name].[hash].[ext]",
                    outputPath: "images"
                }
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "sass-loader"
                ]
            },
        ]
    }
}];