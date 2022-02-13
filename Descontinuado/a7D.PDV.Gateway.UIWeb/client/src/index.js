// Set up your application entry point here...
/* eslint-disable import/default */

if (!global.Intl) {
  global.Intl = require('intl')
  require('intl/locale-data/jsonp/pt-BR.js')
}

import React from 'react'
import { render } from 'react-dom'
import { Router, Route, IndexRoute } from 'react-router'
import { Provider } from 'react-redux'
import { IntlProvider, addLocaleData } from 'react-intl'
import pt from 'react-intl/locale-data/pt'
require('./index.css')
require('react-datepicker/dist/react-datepicker.css')

addLocaleData([...pt])

import configureStore from './store/configureStore'
import { createHistory } from './history'

const store = configureStore()
const history = createHistory(store)

import App from './components/app'
import Pagamento from './components/pagamento'
import Home from './components/home'
import ConfirmacaoPagamento from './components/confirmacao-pagamento'
import NaoEncontrado from './components/nao-encontrado'
import Login from './components/login'
import ControlPanel from './components/control-panel'
import necessitaAutenticacao from './components/necessita-autenticacao'
import { loginSuccess } from './actions'
const router = (
  <IntlProvider locale='pt-BR'>
    <Provider store={store}>
      <Router history={history}>
        <Route path='/' component={App} >
          <IndexRoute component={Home} />
          <Route path='/pagamento/:idPedido' component={ConfirmacaoPagamento} />
          <Route path='/login' component={Login} />
          <Route path='/adm' component={necessitaAutenticacao(ControlPanel)} />
          <Route path='/:chaveAtivacao' component={Pagamento} />
          <Route path='*' component={NaoEncontrado} />
        </Route>
      </Router>
    </Provider>
  </IntlProvider>
)

let token = localStorage.getItem('token')
if (token !== null) {
  store.dispatch(loginSuccess(token))
}

render(router, document.getElementById('app'))
