import { PEDIDOS_RELATORIO_REQUEST, PEDIDOS_RELATORIO_SUCCESS, PEDIDOS_RELATORIO_ERRO } from '../actions'

const initialState = {
  isFetching: 0,
  pedidos: {},
  itens: {},
  error: 0
}

export default function pedidosRelatorio (state = initialState, action) {
  switch (action.type) {
    case PEDIDOS_RELATORIO_REQUEST:
      return Object.assign({}, initialState, {isFetching: 1})
    case PEDIDOS_RELATORIO_SUCCESS:
      return Object.assign({}, initialState, {pedidos: action.response.entities.pedidos, itens: action.response.entities.itens})
    case PEDIDOS_RELATORIO_ERRO:
      return Object.assign({}, initialState, {error: 1})
    default:
      return state
  }
}
