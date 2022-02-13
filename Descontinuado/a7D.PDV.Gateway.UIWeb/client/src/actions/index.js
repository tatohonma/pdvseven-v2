import * as rqApi from '../api/index'
import { CALL_API, Schemas } from '../middleware/api'
import { browserHistory } from 'react-router'
import { checkHttpStatus, parseJSON } from '../utils'
import moment from 'moment'

export const ALTERAR_ESTADO_CONTA = 'ALTERAR_ESTADO_CONTA'
export const CONTAS_PAGAR_REQUEST = 'CONTAS_PAGAR_REQUEST'
export const CONTAS_PAGAR_SUCCESS = 'CONTAS_PAGAR_SUCCESS'
export const CONTAS_PAGAR_ERRO = 'CONTAS_PAGAR_ERRO'
export const CLIENTE_REQUEST = 'CLIENTE_REQUEST'
export const CLIENTE_SUCCESS = 'CLIENTE_SUCCESS'
export const CLIENTE_ERRO = 'CLIENTE_ERRO'
export const PEDIDO_REQUEST = 'PEDIDO_REQUEST'
export const PEDIDO_SUCCESS = 'PEDIDO_SUCCESS'
export const PEDIDO_ERRO = 'PEDIDO_ERRO'
export const PEDIDO_CANCELADO = 'PEDIDO_CANCELADO'
export const PEDIDO_LIMPAR = 'PEDIDO_LIMPAR'
export const PEDIDOS_REQUEST = 'PEDIDOS_REQUEST'
export const PEDIDOS_SUCCESS = 'PEDIDOS_SUCCESS'
export const PEDIDOS_ERRO = 'PEDIDOS_ERRO'
export const PEDIDOS_RELATORIO_REQUEST = 'PEDIDOS_RELATORIO_REQUEST'
export const PEDIDOS_RELATORIO_SUCCESS = 'PEDIDOS_RELATORIO_SUCCESS'
export const PEDIDOS_RELATORIO_ERRO = 'PEDIDOS_RELATORIO_ERRO'
export const PAGAMENTO_REQUEST = 'PAGAMENTO_REQUEST'
export const PAGAMENTO_SUCCESS = 'PAGAMENTO_SUCCESS'
export const PAGAMENTO_ERRO = 'PAGAMENTO_ERRO'
export const BOLETO_REQUEST = 'BOLETO_REQUEST'
export const BOLETO_SUCCESS = 'BOLETO_SUCCESS'
export const BOLETO_ERRO = 'BOLETO_ERRO'
export const CLEAN = 'CLEAN'
export const LOGIN_REQUEST = 'LOGIN_REQUEST'
export const LOGIN_SUCCESS = 'LOGIN_SUCCESS'
export const LOGIN_ERROR = 'LOGIN_ERROR'
export const LOGOUT = 'LOGOUT'
export const SINCRONIZAR_REQUEST = 'SINCRONIZAR_REQUEST'
export const SINCRONIZAR_SUCESSO = 'SINCRONIZAR_SUCESSO'
export const SINCRONIZAR_ERRO = 'SINCRONIZAR_ERRO'
export const LOG_REQUEST = 'LOG_REQUEST'
export const LOG_SUCCESS = 'LOG_SUCCESS'
export const LOG_ERR = 'LOG_ERR'
export const ABRIR_DETALHES = 'ABRIR_DETALHES'
export const FECHAR_DETALHES = 'FECHAR_DETALHES'
export const ABRIR_MODAL = 'ABRIR_MODAL'
export const FECHAR_MODAL = 'FECHAR_MODAL'
export const CANCELAR_PEDIDO_REQUEST = 'CANCELAR_PEDIDO_REQUEST'
export const CANCELAR_PEDIDO_SUCCESS = 'CANCELAR_PEDIDO_SUCCESS'
export const CANCELAR_PEDIDO_ERRO = 'CANCELAR_PEDIDO_ERRO'
export const REMOVER_SELECAO = 'REMOVER_SELECAO'
export const ADICIONAR_CONTAS = 'ADICIONAR_CONTAS'

export function limparState () {
  return {
    type: CLEAN
  }
}

// Conta

export function alterarSelecionado (idConta) {
  return {
    type: ALTERAR_ESTADO_CONTA,
    payload: idConta
  }
}

export function obterContasPagar (chaveLicenca) {
  return dispatch => {
    dispatch({
      type: CONTAS_PAGAR_REQUEST
    })

    rqApi.rqContasPagar(chaveLicenca)
      .then(data => {
        dispatch({
          type: CONTAS_PAGAR_SUCCESS,
          payload: data
        })
      })
      .catch(err => {
        dispatch({
          type: CONTAS_PAGAR_ERRO,
          payload: err
        })
      })
  }
}

// Pedido

export function obterPedido (contasSelecionadas) {
  return dispatch => {
    dispatch({
      type: PEDIDO_REQUEST
    })

    rqApi.rqPedidos(contasSelecionadas).then(data => {
      dispatch({
        type: PEDIDO_SUCCESS,
        payload: data
      })
    })
      .catch(err => {
        dispatch({
          type: PEDIDO_ERRO,
          payload: err
        })
      })
  }
}

export function limparPedido () {
  return {
    type: PEDIDO_LIMPAR
  }
}

// Pedidos pendentes

export function obterPedidosPendentes (chaveLicenca) {
  return dispatch => {
    dispatch({
      type: PEDIDOS_REQUEST
    })

    rqApi.rqPedidosPendentes(chaveLicenca)
      .then(data => {
        dispatch({
          type: PEDIDOS_SUCCESS,
          payload: data
        })
      })
      .catch(err => {
        dispatch({
          type: PEDIDOS_ERRO,
          payload: err
        })
      })
  }
}

// Pedidos Pendentes Relatorio

export function obterPedidosRelatorio (jwt, startDate, endDate) {
  if (typeof startDate === 'undefined') {
    startDate = moment().subtract(10, 'days')
  }

  if (typeof endDate === 'undefined') {
    endDate = moment()
  }

  return {
    [CALL_API]: {
      types: [ PEDIDOS_RELATORIO_REQUEST, PEDIDOS_RELATORIO_SUCCESS, PEDIDOS_RELATORIO_ERRO ],
      endpoint: `pedidos?s=${startDate.unix()}&e=${endDate.unix()}`,
      schema: Schemas.PEDIDO_ARRAY,
      jwt,
      startDate,
      endDate
    }
  }
}

// Pedidos Pendentes Detalhe

export function abrirDetalhes (idPedido) {
  return {
    type: ABRIR_DETALHES,
    idPedido
  }
}

export function fecharDetalhes (idPedido) {
  return {
    type: FECHAR_DETALHES,
    idPedido
  }
}

// Logs

export function obterLog (jwt, linhas, tipo) {
  return {
    [CALL_API]: {
      types: [ LOG_REQUEST, LOG_SUCCESS, LOG_ERR ],
      endpoint: `log?linhas=${linhas}&tipo=${tipo}`,
      jwt
    }
  }
}

// Pagamento

export function obterStatusPagamento (idPedido) {
  return dispatch => {
    dispatch({
      type: PAGAMENTO_REQUEST
    })

    rqApi.rqPedido(idPedido)
      .then(data => {
        dispatch({
          type: PAGAMENTO_SUCCESS,
          payload: data
        })
      })
      .catch((err) => {
        dispatch({
          type: PAGAMENTO_ERRO,
          payload: err
        })
      })
  }
}

// Cliente

export function obterCliente (chaveLicenca) {
  return dispatch => {
    dispatch({
      type: CLIENTE_REQUEST
    })

    rqApi.rqClientes(chaveLicenca)
      .then(data => {
        dispatch({
          type: CLIENTE_SUCCESS,
          payload: data
        })
      })
      .catch(err => {
        dispatch({
          type: CLIENTE_ERRO,
          payload: err
        })
      })
  }
}

// Boleto

export function atualizarBoleto (IdPedido) {
  return dispatch => {
    dispatch({
      type: BOLETO_REQUEST,
      IdPedido
    })
    rqApi.rqBoleto(IdPedido)
      .then(data => {
        dispatch({
          type: BOLETO_SUCCESS,
          payload: data,
          IdPedido
        })
      })
      .catch(err => {
        dispatch({
          type: BOLETO_ERRO,
          payload: err,
          IdPedido
        })
      })
  }
}

// Login

export function login () {
  return {
    type: LOGIN_REQUEST
  }
}

export function loginFailure (error) {
  window.localStorage.removeItem('token')
  return {
    type: LOGIN_ERROR,
    payload: {
      status: error.response.status,
      statusText: error.response.statusText
    }
  }
}

export function loginSuccess (token) {
  window.localStorage.setItem('token', token)
  return {
    type: LOGIN_SUCCESS,
    payload: {
      token
    }
  }
}

export function logout () {
  window.localStorage.removeItem('token')
  return {
    type: LOGOUT
  }
}

export function loginUser (email, pwd, redirect = '/') {
  return dispatch => {
    dispatch(login())
    return rqApi.rqLogin(email, pwd)
      .then(checkHttpStatus)
      .then(parseJSON)
      .then(response => {
        try {
          dispatch(loginSuccess(response.token))
          browserHistory.push(redirect)
        } catch (e) {
          dispatch(loginFailure({
            response: {
              status: 403,
              statusText: 'Invalid token'
            }
          }))
        }
      })
      .catch(err => {
        if (err.response) {
          dispatch(loginFailure(err))
        } else {
          dispatch(loginFailure({
            response: {
              status: 0,
              statusText: 'Não foi possível se conectar ao servidor'
            }
          }))
        }
      })
  }
}

// Sincronizar

export function sincronizar () {
  return {
    type: SINCRONIZAR_REQUEST
  }
}

export function sincronizarErro () {
  return {
    type: SINCRONIZAR_ERRO
  }
}

export function sincronizarRequest () {
  return sincronizar()
}

export function sincronizarSucesso (tempos) {
  return {
    type: SINCRONIZAR_SUCESSO,
    payload: tempos
  }
}

export function obterTempos (token) {
  return dispatch => {
    dispatch(sincronizar())
    return rqApi.rqObterTempos(token)
      .then(checkHttpStatus)
      .then(parseJSON)
      .then(response => {
        dispatch(sincronizarSucesso(response))
      })
      .catch(err => {
        if (err.response.status === 401) {
          dispatch(loginFailure(err))
          browserHistory.push('/login')
        }
        dispatch(sincronizarErro())
      })
  }
}

export function sincronizarAgora (token) {
  return dispatch => {
    dispatch(sincronizarRequest())
    return rqApi.rqSincronizar(token)
      .then(checkHttpStatus)
      .then(parseJSON)
      .then(response => {
        dispatch(sincronizarSucesso(response))
      })
      .catch(err => {
        if (err.response.status === 401) {
          dispatch(loginFailure(err))
          browserHistory.push('/login')
        }
        dispatch(sincronizarErro())
      })
  }
}

// Modal Cancelamento

export function abrirModal () {
  return {
    type: ABRIR_MODAL
  }
}

export function fecharModal () {
  return {
    type: FECHAR_MODAL
  }
}

// Cancelar Pedido

export function cancelarPedido (idPedido) {
  return dispatch => {
    dispatch({type: CANCELAR_PEDIDO_REQUEST})
    rqApi.rqCancelarPedido(idPedido)
      .then(data => {
        dispatch({
          type: CANCELAR_PEDIDO_SUCCESS
        })
        dispatch({
          type: PEDIDO_CANCELADO,
          idPedido
        })
        dispatch({
          type: REMOVER_SELECAO
        })
        dispatch({
          type: ADICIONAR_CONTAS,
          contas: data.data.Itens
        })
        dispatch({
          type: PEDIDO_SUCCESS,
          payload: data
        })
      })
      .catch(err => {
        dispatch({type: CANCELAR_PEDIDO_ERRO, payload: err})
      })
  }
}