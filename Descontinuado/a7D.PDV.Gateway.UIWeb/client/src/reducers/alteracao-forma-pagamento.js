import { CANCELAR_PEDIDO_REQUEST, CANCELAR_PEDIDO_SUCCESS, CANCELAR_PEDIDO_ERRO } from '../actions'

const initialState = {
  isFetching: false,
  error: false,
  message: null
}

export default function alteracaoFormaPagamento (state = initialState, action) {
  switch (action.type) {
    case CANCELAR_PEDIDO_REQUEST:
      return Object.assign({}, initialState, {isFetching: true})
    case CANCELAR_PEDIDO_SUCCESS:
      return Object.assign({}, initialState)
    case CANCELAR_PEDIDO_ERRO:
      return Object.assign({}, initialState, {error: true, message: 'Ocorrou um erro ao trocar a forma de pagamento do pedido.'})
    default:
      return state
  }
}