import 'babel-polyfill'
import expect from 'expect'
import * as actions from '../../../src/js/actions'

describe('fetch sat status', () => {
  it('actions.fetchSatStatus should create FETCH_SAT_SATUS_REQUEST action', () => {
    expect(actions.fetchSatStatus()).toEqual({
      type: 'FETCH_SAT_SATUS_REQUEST'
    })
  })
})
