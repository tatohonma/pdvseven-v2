import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'

import sat from './sat'
import log from './log'

const rootReducer = combineReducers({sat, log, routing: routerReducer})

export default rootReducer
