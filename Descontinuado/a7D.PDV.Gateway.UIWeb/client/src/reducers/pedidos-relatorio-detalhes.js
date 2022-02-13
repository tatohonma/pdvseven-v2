import { ABRIR_DETALHES, FECHAR_DETALHES } from '../actions'

const initialState = {}

export default function pedidosRelatorioDetalhes (state = initialState, action) {
  switch (action.type) {
    case ABRIR_DETALHES:
      return Object.assign({}, state, {[action.idPedido]: true})
    case FECHAR_DETALHES:
      return Object.assign({}, state, {[action.idPedido]: false})
    default:
      return state;
  }
}