const Path = require('path')
const Webpack = require('webpack')

module.exports = {
  entry: ['./src/js/index.js'],
  output: {
    path: Path.resolve(process.cwd(), 'dist/js'),
    publicPath: '/dist/js/',
    filename: 'bundle.js'
  },
  module: {
    loaders: [{
      test: /\.js$/,
      include: [
        Path.resolve(process.cwd(), 'src/js')
      ],
      exclude: /'node_modules/,
      loader: 'babel',
      query: {
        presets: ['es2015', 'stage-0', 'react']
      }
    },
    { test: /\.jsx?$/, exclude: /(node_modules|bower_components)/, loader: 'babel' },
    { test: /\.css$/, loader: 'style-loader!css-loader' },
    { test: /bootstrap\/js\//, loader: 'imports?jQuery=jquery' },

      // Needed for the css-loader when [bootstrap-webpack](https://github.com/bline/bootstrap-webpack)
    // loads bootstrap's css.
    { test: /\.woff(\?v=\d+\.\d+\.\d+)?$/, loader: 'url?limit=10000&mimetype=application/font-woff' },
    { test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/, loader: 'url?limit=10000&mimetype=application/octet-stream' },
    { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: 'file' },
    { test: /\.svg(\?v=\d+\.\d+\.\d+)?$/, loader: 'url?limit=10000&mimetype=image/svg+xml' }
    ]
  },
  resolve: {
    extension: ['', '.js', '.css'],
    modulesDirectories: ['node_modules']
  },
  plugins: [
    new Webpack.optimize.UglifyJsPlugin({
      compress: {
        warnings: false
      }
    }),
    new Webpack.DefinePlugin({
      'process.env': {
        'NODE_ENV': JSON.stringify('production')
      }
    })
  ]
}
