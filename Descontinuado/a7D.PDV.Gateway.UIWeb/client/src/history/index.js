import { syncHistoryWithStore } from 'react-router-redux'
import { browserHistory } from 'react-router'

export function createHistory (store) {
  return syncHistoryWithStore(browserHistory, store)
}
