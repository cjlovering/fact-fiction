var path = require('path');

module.exports = {
    entry: path.join(__dirname, 'wwwroot/components/index.jsx'),
    output: {
        path: path.join(__dirname, 'wwwroot/js'),
        filename: 'bundle.js'
    },
    module: {
        loaders: [
            // Transform JSX in .jsx files
            {
                test: /\.jsx$/,
                loader: 'babel-loader',
                query: {
                    presets: ['react-es2015']
                }
            }
        ],
    },
    resolve: {
        // Allow require('./blah') to require blah.jsx
        extensions: ['.js', '.jsx']
    },
    externals: {
        // Use external version of React (from CDN for client-side, or
        // bundled with ReactJS.NET for server-side)
        react: 'React'
    }
};