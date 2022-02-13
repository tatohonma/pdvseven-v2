const initialState = {
  sat: [],
  isFetching: false,
  errorMessage: null
}

export default function sat (state = initialState, action) {
  switch (action.type) {
    case 'FETCH_STATUS_REQUEST':
      return {...state, isFetching: true, errorMessage: null}
    case 'FETCH_STATUS_SUCCESS':
      let sat = [action.payload.data, ...state.sat]
      return {
        sat,
        errorMessage: null,
        isFetching: false
      }
    case 'FETCH_STATUS_ERROR':
      return {
        ...state,
        isFetching: false,
        errorMessage: action.payload
      }
    default:
      return state
  }
}
