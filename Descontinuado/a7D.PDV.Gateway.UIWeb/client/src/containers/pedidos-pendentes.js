/* eslint-disable  react/no-set-state */
import React, { Component } from 'react'
import Pedido from '../components/pedido'
import { Collapse } from 'react-bootstrap'

export default class PedidosPendentes extends Component {
  constructor (args) {
    super(...args)

    this.state = {
      open: true
    }
  }

  componentDidMount () {
    // debugger
    this.props.obterPedidosPendentes(this.props.params.chaveAtivacao)
  }

  renderPedido (pedido) {
    return <Pedido key={pedido.IdPedido} pedido={pedido} atualizarBoleto={this.props.atualizarBoleto} boletosAtualizando={this.props.pedidosPendentes.obterBoletos} />
  }

  render () {
    const qtdPedidos = this.props.pedidosPendentes.dados.length
    if (qtdPedidos < 1) {
      return null
    }
    return (
      <div className='panel panel-default'>
        <div className='panel-heading' onClick={() => this.setState({ open: !this.state.open })}>
          <h4 className='panel-title'>Pedidos aguardando confirmação&nbsp;<span className='badge'>{qtdPedidos}</span></h4>
        </div>
        <Collapse in={this.state.open}>
          <div className='list-group'>
            {this.props.pedidosPendentes.dados.map((pedido) => this.renderPedido(pedido))}
          </div>
        </Collapse>
      </div>
    )
  }
}

PedidosPendentes.propTypes = {
  obterPedidosPendentes: React.PropTypes.func,
  pedidosPendentes: React.PropTypes.object,
  params: React.PropTypes.object,
  atualizarBoleto: React.PropTypes.func.isRequired
}
