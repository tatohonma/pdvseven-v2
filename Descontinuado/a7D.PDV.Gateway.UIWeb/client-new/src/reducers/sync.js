import { SINCRONIZAR_REQUEST, SINCRONIZAR_SUCESSO, SINCRONIZAR_ERRO } from '../actions'

const initialState = {
  sincronizando: false,
  last: null,
  next: null
}

export default function sync (state = initialState, action) {
  const payload = action.payload
  switch (action.type) {
    case SINCRONIZAR_REQUEST:
      return { sincronizando: true }
    case SINCRONIZAR_SUCESSO:
      return { sincronizando: false, last: payload.last, next: payload.next }
    case SINCRONIZAR_ERRO:
      return Object.assign({}, initialState)
    default:
      return state
  }
}
