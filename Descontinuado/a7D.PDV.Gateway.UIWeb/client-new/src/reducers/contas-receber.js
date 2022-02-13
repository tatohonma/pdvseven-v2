import { CONTAS_PAGAR_REQUEST, CONTAS_PAGAR_SUCCESS, CONTAS_PAGAR_ERRO, ALTERAR_ESTADO_CONTA, CLEAN, REMOVER_SELECAO, ADICIONAR_CONTAS } from '../actions'
import moment from 'moment'
import _ from 'lodash'

const initialState = {
  mesAtualOuVencido: [],
  outrosMeses: [],
  obtendo: false
}

const mesAtualOuVencido = dataVencimento => moment(dataVencimento).isSameOrBefore(moment(), 'month')

export default function contasReceber (state = initialState, action) {
  switch (action.type) {
    case CONTAS_PAGAR_REQUEST:
      return Object.assign({}, initialState, {obtendo: true})
    case CONTAS_PAGAR_SUCCESS: {
      const res = Object.assign({}, initialState)
      res.mesAtualOuVencido = _.filter(action.payload.data, conta => mesAtualOuVencido(conta.Vencimento))
      res.mesAtualOuVencido = res.mesAtualOuVencido.map(conta => Object.assign({}, conta))
      res.outrosMeses = _.filter(action.payload.data, conta => !mesAtualOuVencido(conta.Vencimento))
      return res
    }
    case CONTAS_PAGAR_ERRO:
      return state
    case ALTERAR_ESTADO_CONTA: {
      const idConta = action.payload
      const contas = Object.assign({}, state)
      contas.mesAtualOuVencido = contas.mesAtualOuVencido.map(conta => {
        return contaReceber(conta, { type: 'ALTERAR_SELECIONADO', payload: idConta })
      })
      contas.outrosMeses = contas.outrosMeses.map(conta => {
        return contaReceber(conta, { type: 'ALTERAR_SELECIONADO', payload: idConta })
      })
      return contas
    }
    case REMOVER_SELECAO: {
      let st = Object.assign({}, state)
      st.mesAtualOuVencido.forEach(cr => { cr.selecionado = false })
      st.outrosMeses.forEach(cr => { cr.selecionado = false })
      return st
    }
    case ADICIONAR_CONTAS: {
      let st = Object.assign({}, state)
      const contas = action.contas
      for (let i = 0; i < contas.length; i++) {
        const conta = contas[i].ContaReceber
        conta.selecionado = true
        if (mesAtualOuVencido(conta.Vencimento)) {
          st.mesAtualOuVencido.push(conta)
        } else {
          st.outrosMeses.push(conta)
        }
      }
      return st
    }
    case CLEAN:
      return Object.assign({}, initialState)
    default:
      return state
  }
}

function contaReceber (state, action) {
  switch (action.type) {
    case 'ALTERAR_SELECIONADO':
      if (state.IdContaReceber === action.payload) {
        return Object.assign({}, state, { selecionado: !state.selecionado })
      }
      return state
    default:
      return state
  }
}
