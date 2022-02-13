import { CLIENTE_REQUEST, CLIENTE_SUCCESS, CLIENTE_ERRO, CLEAN } from '../actions'

const initialState = {
  'IdCliente': 0,
  'IdBroker': '',
  'Nome': '',
  'CpfCnpj': null,
  'erro': false
}

export default function cliente (state = initialState, action) {
  switch (action.type) {
    case CLIENTE_REQUEST:
      return Object.assign({}, initialState)
    case CLIENTE_SUCCESS: {
      return Object.assign({}, action.payload.data, { erro: false })
    }
    case CLIENTE_ERRO:
      return Object.assign({}, initialState, { erro: true })
    case CLEAN:
      return Object.assign({}, initialState)
    default:
      return state
  }
}
