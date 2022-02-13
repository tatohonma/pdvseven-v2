const Path = require('path')

module.exports = {
  entry: ['./src/js/index.js'],
  output: {
    path: Path.resolve(process.cwd(), 'dist/js'),
    publicPath: '/dist/js/',
    filename: 'bundle.js'
  },
  devtool: 'source-map',
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

    { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: "file" },
			{ test: /\.(woff|woff2)$/, loader:"url?prefix=font/&limit=5000" },
			{ test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=application/octet-stream" },
			{ test: /\.svg(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=image/svg+xml" }
    ]
  },
  resolve: {
    extension: ['', '.js', '.css'],
    modulesDirectories: ['node_modules']
  }
}
