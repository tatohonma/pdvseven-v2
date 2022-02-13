import { FATURA_RQ, FATURA_SC, FATURA_ER, NOVOBOLETO_SC } from '../actions'
import moment from 'moment'

const initialState = {
  faturas: {},
  atuais: {},
  proximas: {},
  obtendo: false,
  erro: false,
  msg: null
}

const mesAtualOuVencido = dataVencimento => moment(dataVencimento).isSameOrBefore(moment(), 'month')
const filtrarAtuais = (obj) => {
  const result = {}
  for (const key in obj) {
    if (obj.hasOwnProperty(key) && mesAtualOuVencido(obj[key].contaReceber.vencimento)) {
      result[key] = obj[key]
    }
  }
  return result
}

const filtrarProximos = (obj) => {
  const result = {}
  for (const key in obj) {
    if (obj.hasOwnProperty(key) && !mesAtualOuVencido(obj[key].contaReceber.vencimento)) {
      result[key] = obj[key]
    }
  }
  return result
}

export default function faturas (state = initialState, action) {
  switch (action.type) {
    case FATURA_RQ: {
      return Object.assign({}, state, { obtendo: true })
    }
    case FATURA_SC: {
      const newState = Object.assign({}, state, { obtendo: false, faturas: action.response.entities.faturas })
      newState.atuais = filtrarAtuais(newState.faturas)
      newState.proximas = filtrarProximos(newState.faturas)
      return newState
    }
    case FATURA_ER: {
      return Object.assign({}, state, { obtendo: false, erro: true, msg: action.error })
    }
    case NOVOBOLETO_SC: {
      const fatura = action.response.entities.faturas[action.response.result]
      const newState = Object.assign({}, state)
      newState.faturas = Object.assign({}, newState.faturas, { [fatura.idFatura]: fatura })
      if (mesAtualOuVencido(fatura.contaReceber.vencimento)) {
        newState.atuais = Object.assign({}, newState.atuais, { [fatura.idFatura]: fatura })
      } else {
        newState.proximas = Object.assign({}, newState.proximas, { [fatura.idFatura]: fatura })
      }
      return newState
    }
    default:
      return state;
  }
}