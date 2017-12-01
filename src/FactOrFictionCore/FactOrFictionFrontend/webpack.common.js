const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: path.join(__dirname, 'App/js/index.jsx'),
    output: {
        path: path.join(__dirname, 'wwwroot/'),
        filename: 'js/bundle.js'
    },
    module: {
        rules: [
            {
                test: [/\.scss$/, /\.css$/],
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: [
                        {
                            loader: 'css-loader',
                            query: {
                                modules: true,
                                sourceMap: true,
                                importLoaders: 2,
                                localIdentName: '[local]'
                            }
                        },
                        'sass-loader'
                    ]
                }),
            },
            {
                test: /\.jsx$/,
                use: [
                    { loader: 'babel-loader' }
                ]
            }  
        ]
    },
    plugins: [
        new ExtractTextPlugin({
            filename: 'css/style.css',
            allChunks: true
        })
    ],
    resolve: {
        extensions: ['.js', '.jsx', '.scss', '.css' ]
    }
};