window.jQuery = window.$ = require('jquery/dist/jquery.min')
require('bootstrap')
import React from 'react'
import { render } from 'react-dom'
import store, { history } from './store/store'
import { Provider } from 'react-redux'

import Router from 'react-router/lib/Router'
import Route from 'react-router/lib/Route'
import IndexRoute from 'react-router/lib/IndexRoute'

import App from './components/app'
import Start from './components/start'
import Status from './components/status'
import Log from './components/log'

const router = (
<Provider store={store}>
  <Router history={history}>
    <Route path='/' component={App}>
      <IndexRoute component={Start} />
      <Route path='/status' component={Status} />
      <Route path='/log' component={Log} />
    </Route>
  </Router>
</Provider>
)

render(router, document.getElementById('app'))
