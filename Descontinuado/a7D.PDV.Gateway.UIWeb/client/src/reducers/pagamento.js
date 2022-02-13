import { PAGAMENTO_REQUEST, PAGAMENTO_SUCCESS, PAGAMENTO_ERRO } from '../actions'
const initialState = {
  obtendo: false,
  erro: false,
  mensagem: null,
  dados: {}
}

export default function pagamento (state = initialState, action) {
  switch (action.type) {
    case PAGAMENTO_REQUEST:
      return Object.assign({}, initialState, { obtendo: true })
    case PAGAMENTO_SUCCESS:
      return Object.assign({}, initialState, { dados: action.payload.data })
    case PAGAMENTO_ERRO:
      return Object.assign({}, initialState, { erro: true, mensagem: action.payload.data.message })
    default:
      return state
  }
}
