import { createStore, applyMiddleware } from 'redux'
import rootReducer from '../reducers'
import thunk from 'redux-thunk'
import api from '../middleware/api'
import sentryErrorReporter from '../middleware/sentry-error-reporter'

export default function configureStore (initialState) {
  return createStore(rootReducer, initialState, applyMiddleware(sentryErrorReporter, thunk, api))
}
