import React, { Component } from 'react'
import Sucesso from './sucesso'
import ClienteNaoEncontrado from './cliente-nao-encontrado'
import Carregando from './carregando'

export default class Pagamento extends Component {

  componentDidMount () {
    this.props.obterCliente(this.props.params.chaveAtivacao)
  }

  render () {
    return (
      <div>
        {this.props.cliente.erro ? <ClienteNaoEncontrado {...this.props} chaveAtivacao={this.props.params.chaveAtivacao} /> : null}
        {(!this.props.cliente.erro && this.props.cliente.IdCliente === 0) ? <Carregando {...this.props} /> : null}
        {(!this.props.cliente.erro && this.props.cliente.IdCliente !== 0) ? <Sucesso {...this.props} /> : null}
      </div>
    )
  }
}

Pagamento.propTypes = { cliente: React.PropTypes.object, params: React.PropTypes.object, obterCliente: React.PropTypes.func }
