import axios from 'axios'

export const sat = () => {
  return axios.get('/api/sat')
}

export const log = (codigoDeAtivacao) => {
  return axios.get('/api/sat/log', {
    params: {
      codigoDeAtivacao
    }
  })
}
