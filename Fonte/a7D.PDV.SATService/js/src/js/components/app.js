import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as actionCreators from '../actions/index'
import Main from './main'

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

function mapStateToProps (state) {
  return {
    sat: state.sat,
    log: state.log
  }
}

const App = connect(mapStateToProps, mapDispatchToProps)(Main)

export default App
