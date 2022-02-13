const express = require('express')
const Path = require('path')
const helmet = require('helmet')
const cors = require('cors')
const webpackDevMiddleware = require('webpack-dev-middleware')
const webpack = require('webpack')
const webpackConfig = require('./config/webpack.config.dev')

const app = express()

const compiler = webpack(webpackConfig)

app.use(cors())
app.use(helmet())

app.use(webpackDevMiddleware(compiler, {
  publicPath: '/dist/js/'
}))

app.get('*', (req, res) => {
  res.sendFile(Path.resolve('dist/index.html'))
})

module.exports = app
