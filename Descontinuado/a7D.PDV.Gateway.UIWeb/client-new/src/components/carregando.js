import React, { Component } from 'react'

export default class Carregando extends Component {
  render () {
    return (
      <i
        className='fa fa-spinner fa-pulse fa-5x fa-fw'
        style={{top: '50%', left: '50%', position: 'absolute', marginLeft: '-50px', marginTop: '-50px'}}></i>
    )
  }
}
