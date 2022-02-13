import axios from 'axios'
import { postbackServer } from '../constants/endpoints'

export function rqContasPagar (chaveLicenca) {
  return axios.get(`${postbackServer}/contasReceber`, {
    params: { chaveLicenca }
  })
}

export function rqClientes (chaveLicenca) {
  return axios.get(`${postbackServer}/clientes`, {
    params: { chaveLicenca }
  })
}

export function rqPedidos (contasSelecionadas) {
  const url = `${postbackServer}/pedidos`
  return axios({
    method: 'post',
    data: contasSelecionadas,
    url
  })
}

export function rqPedido (idPedido) {
  return axios.get(`${postbackServer}/pedidos/${idPedido}`)
}

export function rqPedidosPendentes (chaveLicenca) {
  return axios.get(`${postbackServer}/pedidos`, {
    params: { chaveLicenca }
  })
}

export function rqBoleto (idPedido) {
  const url = `${postbackServer}/transacoes/${idPedido}/novoboleto`
  return axios({
    method: 'post',
    url
  })
}

export function rqLogin (email, senha) {
  return fetch(`${postbackServer}/auth`, {
    method: 'post',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ email, senha })
  })
}

export function rqSincronizar (token) {
  return fetch(`${postbackServer}/sync`, {
    method: 'post',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'x-auth-token': token
    }
  })
}

export function rqObterTempos (token) {
  return fetch(`${postbackServer}/sync`, {
    method: 'get',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'x-auth-token': token
    }
  })
}

export function rqObterPedidosPendentes (token) {
  return fetch(`${postbackServer}/pedidos`, {
    method: 'get',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'x-auth-token': token
    }
  })
}

export function rqCancelarPedido (idPedido) {
  const url = `${postbackServer}/pedidos/${idPedido}`
  return axios({
    method: 'delete',
    url
  })
}
