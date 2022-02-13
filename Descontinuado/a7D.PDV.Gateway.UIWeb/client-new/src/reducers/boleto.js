import { NOVOBOLETO_RQ, NOVOBOLETO_SC, NOVOBOLETO_ER, NOVOBOLETO_CL } from '../actions'

const initialState = {
  obtendo: false,
  novaFatura: null,
  erro: false,
  msg: null
}

export default function boleto (state = initialState, action) {
  switch (action.type) {
    case NOVOBOLETO_RQ: {
      return Object.assign({}, initialState, { obtendo: true })
    }
    case NOVOBOLETO_SC: {
      return Object.assign({}, state, { novaFatura: action.response.entities.faturas[action.response.result] })
    }
    case NOVOBOLETO_ER: {
      return Object.assign({}, initialState, { erro: true, msg: action.error })
    }
    case NOVOBOLETO_CL: {
      return Object.assign({}, initialState)
    }
    default:
      return state
  }
}