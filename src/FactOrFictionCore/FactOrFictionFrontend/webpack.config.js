/// <binding ProjectOpened='Watch - Development' />
var path = require('path');

module.exports = {
    entry: path.join(__dirname, 'App/index.jsx'),
    output: {
        path: path.join(__dirname, 'wwwroot/js'),
        filename: 'bundle.js'
    },
    module: {
        loaders: [
            // Transform JSX in .jsx files
            {
                test: /\.jsx$/,
                loader: 'babel-loader'
            }
        ]
    },
    resolve: {
        // Allow require('./blah') to require blah.jsx
        extensions: ['.js', '.jsx']
    }
};