import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as actionCreators from '../actions/index'
import Main from './main'

function mapStateToProps (state) {
  return {
    contasReceber: state.contasReceber,
    cliente: state.cliente,
    pedido: state.pedido,
    pagamento: state.pagamento,
    pedidosPendentes: state.pedidosPendentes,
    isAuthenticated: state.auth.isAuthenticated,
    isAuthenticating: state.auth.isAuthenticating,
    auth: state.auth,
    syncing: state.sync.sincronizando,
    sync: state.sync,
    pedidosRelatorio: state.pedidosRelatorio,
    pedidosRelatorioDetalhes: state.pedidosRelatorioDetalhes,
    modalCancelamento: state.modalCancelamento,
    log: state.log,
    alteracaoFormaPagamento: state.alteracaoFormaPagamento
  }
}

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

const App = connect(mapStateToProps, mapDispatchToProps)(Main)

export default App
