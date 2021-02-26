const path = require('path')
const HtmlWebPackPlugin = require('html-webpack-plugin')

const outputPath = path.resolve(__dirname, '../Optsol.Playground.Security.Identity/wwwroot')

module.exports = {
    entry: ['babel-polyfill', './src/index.js'],
    output: {
        path: outputPath,
        filename: 'main.js',
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['es2015', 'env', 'stage-0', 'react']
                    }
                },
            },
            {
                test: /\.html$/,
                use: [
                    {
                        loader: 'html-loader',
                        options: { minimize: true },
                    },
                ],
            },
        ],
    },
    plugins: [
        new HtmlWebPackPlugin({
            template: './src/index.html',
            filename: './index.html',
            inject: false,
        }),
    ],
    resolve: {
        extensions: ['*', '.js', '.jsx'],
        alias: {
            '~': path.resolve(__dirname, './src'),
        },
    },
    devServer: {
        historyApiFallback: true,
    },
}
