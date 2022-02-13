import { sat, log } from '../api/index'

export const obterStatusAsync = () => {
  return dispatch => {
    dispatch({
      type: 'FETCH_STATUS_REQUEST'
    })

    return sat().then(
      response => {
        dispatch({
          type: 'FETCH_STATUS_SUCCESS',
          payload: response
        })
      }
    ).catch(err => {
      dispatch({
        type: 'FETCH_STATUS_ERROR',
        payload: err
      })
    })
  }
}

export const obterLogAsync = (codigoDeAtivacao) => {
  return dispatch => {
    dispatch({
      type: 'FETCH_LOG_REQUEST'
    })

    return log(codigoDeAtivacao)
      .then(response => {
        dispatch({
          type: 'FETCH_LOG_SUCCESS',
          payload: response
        })
      }).catch(err => {
        dispatch({
          type: 'FETCH_LOG_ERR',
          payload: err
        })
      })
  }
}

export const limparLog = () => {
  return {
    type: 'CLEAN_LOG'
  }
}
