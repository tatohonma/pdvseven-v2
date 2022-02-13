import { LOGIN_REQUEST, LOGIN_SUCCESS, LOGIN_ERROR, LOGOUT } from '../actions'

const initialState = {
  token: null,
  isAuthenticated: false,
  isAuthenticating: false,
  statusText: null
}

export default function auth (state = initialState, action) {
  switch (action.type) {
    case LOGIN_REQUEST:
      return Object.assign({}, state, {statusText: null, isAuthenticating: true})
    case LOGIN_SUCCESS:
      return Object.assign({}, state, {isAuthenticating: false, isAuthenticated: true, statusText: null, token: action.payload.token})
    case LOGIN_ERROR:
      return Object.assign({}, initialState, {statusText: action.payload.statusText})
    case LOGOUT:
      return Object.assign({}, initialState)
    default:
      return state
  }
}
