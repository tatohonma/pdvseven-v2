import { createStore, applyMiddleware, compose } from 'redux'
import { syncHistoryWithStore } from 'react-router-redux'
import { browserHistory } from 'react-router'
import thunk from 'redux-thunk'

import rootReducer from '../reducers/index'

const initialData = {
  log: {
    log: [],
    isFetching: false,
    errorMessage: null
  },
  sat: {sat: [],
  isFetching: false,
  errorMessage: null
}
}

const store = createStore(rootReducer, initialData, compose(applyMiddleware(thunk), window.devToolsExtension ? window.devToolsExtension() : f => f))

export const history = syncHistoryWithStore(browserHistory, store)

export default store
