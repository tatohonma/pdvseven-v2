import { BOLETO_REQUEST, BOLETO_SUCCESS, BOLETO_ERRO, PEDIDOS_REQUEST, PEDIDOS_SUCCESS, PEDIDOS_ERRO, PEDIDO_CANCELADO } from '../actions'
import _ from 'lodash'

const initialState = {
  dados: [],
  erro: false,
  obtendo: false,
  mensagem: null,
  obterBoletos: {}
}

const obterBoletos = {
  obtendo: true,
  erro: false,
  sucesso: false
}

export default function pedidosPendentes (state = initialState, action) {
  switch (action.type) {
    case PEDIDOS_REQUEST:
      return Object.assign({}, initialState, { obtendo: true })
    case PEDIDOS_SUCCESS:
      return Object.assign({}, initialState, { dados: [...action.payload.data] })
    case PEDIDOS_ERRO:
      return Object.assign({}, initialState, { erro: true, mensagem: action.payload.data })
    case BOLETO_REQUEST: {
      const st = Object.assign({}, state)
      st.obterBoletos = Object.assign({}, st.obterBoletos, {[action.IdPedido]: Object.assign({}, obterBoletos)})
      return st
    }
    case BOLETO_SUCCESS: {
      const st = Object.assign({}, state)
      const pedido = action.payload.data
      st.obterBoletos = Object.assign({}, st.obterBoletos, {[action.IdPedido]: Object.assign({}, obterBoletos, {sucesso: true, obtendo: false})})
      st.dados = st.dados.map(p => {
        if (p.IdPedido === pedido.IdPedido) {
          return pedido
        }
        return p
      })
      return st
    }
    case BOLETO_ERRO: {
      const st = Object.assign({}, state)
      st.obterBoletos = Object.assign({}, st.obterBoletos, {[action.IdPedido]: Object.assign({}, obterBoletos, {erro: true, obtendo: false})})
      return st
    }
    case PEDIDO_CANCELADO: {
      let st = Object.assign({}, state)
      const index = _.findIndex(st.dados, pedido => pedido.IdPedido === action.idPedido)
      if (index !== -1) {
        st.dados = [
          ...st.dados.slice(0, index),
          ...st.dados.slice(index + 1)
        ]
      }
      return st;
    }
    default:
      return state
  }
}
