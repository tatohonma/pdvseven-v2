import React, { Component } from 'react'
import Alert from './alert'

export default class Status extends Component {
  constructor (props) {
    super(props)
  }

  renderStatus (status) {
    const mensagem = status.mensagem.split('|')
    const codigo = mensagem[1]
    const texto = mensagem[2]
    return (
      <li className={'list-group-item list-group-item' + (parseInt(codigo, 10) === 8000 ? '-info' : '-warning')} key={status.numeroSessao}>
        <h4 className='list-group-item-heading'>{status.numeroSessao}</h4>
        <p className='list-group-item-text'>{codigo}</p>
        <p className='list-group-item-text'>{texto}</p>
      </li>
    )
  }

  render () {
    return (
      <div>
        <Alert visible={this.props.sat.errorMessage !== null} type='alert-danger' payload={this.props.sat.errorMessage} />
        <button disabled={this.props.sat.isFetching} onClick={this.props.obterStatusAsync} className='btn btn-default'><span className='glyphicon glyphicon-plus'></span>&nbsp; Consultar</button>
        <br />
        <ul className='list-group'>
          {this.props.sat.sat.map(status => this.renderStatus(status))}
        </ul>
      </div>
    )
  }
}
