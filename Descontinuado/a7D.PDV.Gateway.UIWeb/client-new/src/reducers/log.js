import { LOG_SUCCESS, LOG_REQUEST, LOG_ERR } from '../actions'

const initialState = {
  isFetching: false,
  error: false,
  log: ''
}

export default function log (state = initialState, action) {
  switch (action.type) {
    case LOG_REQUEST:
      return Object.assign({}, initialState, {isFetching: true, log: 'Obtendo logs...'})
    case LOG_SUCCESS:
      return Object.assign({}, initialState, {log: action.response.log})
    case LOG_ERR:
      return Object.assign({}, initialState, {error: true, log: action.error})
    default:
      return state
  }
}
