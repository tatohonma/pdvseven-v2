import React, { Component } from 'react'
import { Link } from 'react-router'

export default class Start extends Component {
  render () {
    return (
    <div className='jumbotron'>
      <Link to='/status' className='btn btn-default btn-xl'>Status</Link>
      <Link to='/log' className='btn btn-default btn-xl'>Log</Link>
    </div>
    )
  }
}
