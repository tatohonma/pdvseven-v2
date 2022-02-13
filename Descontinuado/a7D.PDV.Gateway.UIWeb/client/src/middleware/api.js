import { Schema, arrayOf, normalize } from 'normalizr'
import { camelizeKeys } from 'humps'
import 'isomorphic-fetch'
import { postbackServer } from '../constants/endpoints'

export const CALL_API = 'API'

const pedidoSchema = new Schema('pedidos', {
  idAttribute: pedido => pedido.idPedido
})

const itemSchema = new Schema('itens', {
  idAttribute: item => item.idItemPedido
})

pedidoSchema.define({
  itens: arrayOf(itemSchema)
})

export const Schemas = {
  PEDIDO: pedidoSchema,
  PEDIDO_ARRAY: arrayOf(pedidoSchema),
  ITEM: itemSchema,
  ITEM_ARRAY: arrayOf(itemSchema)
}

const API_ROOT = `${postbackServer}/`

function callApi (endpoint, schema, token) {
  const fullUrl = (endpoint.indexOf(API_ROOT) === -1) ? API_ROOT + endpoint : endpoint
  return fetch(fullUrl, {
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'x-auth-token': token
    }
  })
  .then(response => 
    response.json().then(json => ({ json, response }))
  )
  .then(({ json, response }) => {
    if (!response.ok) {
      return Promise.reject(json)
    }
    const camelizedJson = camelizeKeys(json)
    if (schema) {
      return Object.assign({},
        normalize(camelizedJson, schema))
    } else {
      return Object.assign({}, camelizedJson)
    }
  })
}

export default store => next => action => {
  const callAPI = action[CALL_API]
  if (typeof callAPI === 'undefined') {
    return next(action)
  }

  let { endpoint } = callAPI
  const { schema, types, jwt } = callAPI

  if (typeof endpoint === 'function') {
    endpoint = endpoint(store.getState())
  }
  if (typeof endpoint !== 'string') {
    throw new Error('Especifique uma URL de endpoint')
  }
  if (!Array.isArray(types) || types.length !== 3) {
    throw new Error('É esperado um array com 3 tipos')
  }
  if (!types.every(type => typeof type === 'string')) {
    throw new Error('As ações precisam ser strings')
  }

  function actionWith (data) {
    const finalAction = Object.assign({}, action, data)
    delete finalAction[CALL_API]
    return finalAction
  }

  const [requestType, successType, failureType] = types
  next(actionWith({ type: requestType }))

  return callApi(endpoint, schema, jwt).then(
    response => next(actionWith({
      response,
      type: successType
    })),
    error => next(actionWith({
      type: failureType,
      error: error.message || 'Ocorreu um erro'
    }))
  )
}
