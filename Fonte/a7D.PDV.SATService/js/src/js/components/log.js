import React, { Component } from 'react'
import TabelaLog from './tabela-log'
import Alert from './alert'

export default class Log extends Component {
  constructor (props) {
    super(props)
  }

  handleSubmit (e) {
    e.preventDefault()
    const codigoDeAtivacao = this.refs.cod.value
    const form = this.refs.logForm
    this.props.obterLogAsync(codigoDeAtivacao)
  }

  render () {
    return (
    <div className='col-md-12'>
      <div className='row'>
        <Alert visible={this.props.log.errorMessage !== null} type='alert-danger' payload={this.props.log.errorMessage} />
        <form ref='logForm' className='form-inline' onSubmit={this.handleSubmit.bind(this)}>
          <div className='form-group'>
            <div className='input-group col-md-12'>
              <input
                type='text'
                ref='cod'
                className='form-control'
                placeholder='Codigo de Ativação'
                defaultValue={'123456789'} />
              <span className='input-group-btn'>
                <button
                  type='submit'
                  disabled={this.props.log.isFetching}
                  type='submit'
                  className='btn btn-default'>{this.props.log.isFetching ? 'Obtendo Logs...' : 'Obter Logs'}
                </button>
                <button onClick={this.props.limparLog} className='btn btn-default' type='button' disabled={this.props.log.isFetching}>
                  <span className='glyphicon glyphicon-remove'></span>
                </button>
              </span>
            </div>
          </div>
        </form>
        <TabelaLog log={this.props.log.log} />
      </div>
    </div>
    )
  }
}
