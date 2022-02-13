import { PEDIDO_LIMPAR, PEDIDO_REQUEST, PEDIDO_SUCCESS, PEDIDO_ERRO } from '../actions/index'

const initialState = {
  obtendo: false,
  erro: false,
  mensagem: null,
  enviar: false,
  dados: 0
}

export default function pedido (state = initialState, action) {
  switch (action.type) {
    case PEDIDO_LIMPAR:
      return Object.assign({}, initialState)
    case PEDIDO_REQUEST:
      return Object.assign({}, initialState, { obtendo: true })
    case PEDIDO_SUCCESS:
      return Object.assign({}, initialState, { dados: action.payload.data, enviar: true })
    case PEDIDO_ERRO:
      return Object.assign({}, initialState, { erro: true, mensagem: action.payload.message })
    default:
      return state
  }
}
