// Set up your root reducer here...
import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'

import contasReceber from './contas-receber'
import cliente from './cliente'
import pedido from './pedido'
import pagamento from './pagamento'
import pedidosPendentes from './pedidos-pendentes'
import auth from './auth'
import sync from './sync'
import pedidosRelatorio from './pedidos-relatorio'
import log from './log'
import pedidosRelatorioDetalhes from './pedidos-relatorio-detalhes'
import modalCancelamento from './modal-cancelamento'
import alteracaoFormaPagamento from './alteracao-forma-pagamento'

const rootReducer = combineReducers(
  {
    contasReceber,
    cliente,
    pedido,
    pedidosPendentes,
    pagamento,
    auth,
    sync,
    pedidosRelatorio,
    log,
    pedidosRelatorioDetalhes,
    modalCancelamento,
    alteracaoFormaPagamento,
    routing: routerReducer
  })

export default rootReducer
