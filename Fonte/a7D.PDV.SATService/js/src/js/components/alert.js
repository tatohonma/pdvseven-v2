import React, { Component } from 'react'

export default class Alert extends Component {
  render () {
    if (this.props.payload !== null) {
      const { statusText, data } = this.props.payload
      const message = data.Message
      return (
      <div className={'alert ' + this.props.type} role='alert'>
        <strong>{statusText}</strong>&nbsp;
        {message}
      </div>
      )
    }
    return null
  }
}
