const initialState = {
  log: [],
  isFetching: false,
  errorMessage: null
}

export default function sat (state = initialState, action) {
  switch (action.type) {
    case 'FETCH_LOG_REQUEST':
      return {...initialState, isFetching: true}
    case 'FETCH_LOG_SUCCESS':
      return {
        log: action.payload.data.log,
        errorMessage: null,
        isFetching: false
      }
    case 'FETCH_LOG_ERR':
      return {
        ...state,
        isFetching: false,
        errorMessage: action.payload
      }
    case 'CLEAN_LOG':
      return initialState
    default:
      return state
  }
}
