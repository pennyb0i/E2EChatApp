const webpack = require('webpack');

module.exports = {
    resolve: {
        fallback: {
            stream: require.resolve('stream-browserify'),
            buffer: require.resolve('buffer/'),
        },
    },

    plugins: [
        new webpack.ProvidePlugin({
            Buffer: ['buffer', 'Buffer'],
        }),
        new webpack.ProvidePlugin({
            process: 'process/browser',
        }),
    ],
};