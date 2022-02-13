import React, { Component } from 'react'
import { Link } from 'react-router'

export default class ClienteNaoEncontrado extends Component {

  recarregar () {
    this.props.obterContasPagar(this.props.chaveAtivacao)
  }

  render () {
    const { chaveAtivacao } = this.props
    return (
      <div className='jumbotron'>
        <h1>Ops! Cliente não encontrado</h1>
        <p>O cliente com a chave de ativação {chaveAtivacao} não foi encontrado.</p>
        <p><a className='btn btn-primary btn-lg' onClick={this.recarregar.bind(this)}>Tente novamente</a></p>
        <p><Link to='/' className='btn btn-primary btn-lg'>Tentar outra chave</Link></p>
      </div>
    )
  }
}

ClienteNaoEncontrado.propTypes = { chaveAtivacao: React.PropTypes.string, obterContasPagar: React.PropTypes.func }
