import { ABRIR_MODAL, FECHAR_MODAL } from '../actions'
const initialState = false

export default function modalCancelamento (state = initialState, action) {
  switch (action.type) {
    case ABRIR_MODAL:
      return true
    case FECHAR_MODAL:
      return false
    default:
      return state
  }
}