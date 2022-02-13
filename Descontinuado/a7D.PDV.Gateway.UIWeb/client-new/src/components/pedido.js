import React, { Component } from 'react'
import { FormattedNumber } from 'react-intl'
import Boleto from './boleto'
import Cartao from './cartao'

export default class Pedido extends Component {
  constructor (props) {
    super(...props)
  }

  renderizarLinha (conta, indice) {
    return (
      <tr key={indice}>
        <td>{conta.Historico}</td>
        <td><FormattedNumber style='currency' currency='BRL' value={conta.Saldo} /></td>
      </tr>
    )
  }

  navegarLink (url) {
    const win = window.open(url, '_blank')
    win.focus()
  }

  renderizarBoleto (pedido) {
    const {
      UrlBoleto,
      DataVencimentoBoleto,
      IdPedido,
      Bandeira,
      Parcelas,
      UltimosDigitosCartao,
      Valor
    } = pedido
    if (UrlBoleto) {
      const boleto = this.props.boletosAtualizando[IdPedido]
      const gerando = boleto && boleto.obtendo || false
      const erro = boleto && boleto.erro || false
      const sucesso = boleto && boleto.sucesso || false
      return (
        <Boleto
          dataVencimento={DataVencimentoBoleto}
          urlBoleto={UrlBoleto}
          atualizarBoleto={this.props.atualizarBoleto}
          idPedido={IdPedido}
          gerando={gerando}
          erro={erro}
          sucesso={sucesso}
        />
      )
    } else if (Bandeira) {
      return (
        <Cartao
          parcelas={Parcelas}
          valor={Valor}
          bandeira={Bandeira}
          ultimosDigitos={UltimosDigitosCartao}
        />
      )
    }
    return null
  }

  render () {
    const { pedido } = this.props
    if (!pedido.Itens) {
      return null
    }
    return (
      <div className='list-group-item'>
        <div className='media'>
          <div className='media-body'>
            <h4 className='media-heading'>Pedido nº {pedido.IdPedido}</h4>
            <div className='panel panel-default'>
              <table className='table table-hover table-condensed'>
                <thead>
                  <tr>
                    <th>Item</th>
                    <th>Valor</th>
                  </tr>
                </thead>
                <tbody>
                  {pedido.Itens.map((conta, i) => this.renderizarLinha(conta.ContaReceber, i))}
                </tbody>
              </table>
            </div>
            <div className='visible-xs-block'>
                <p>
                  <strong>Valor do Pedido:</strong>&nbsp;<FormattedNumber style='currency' currency='BRL' value={pedido.Valor} />
                </p>
                <p>
                  <strong>Valor Confirmado:</strong>&nbsp;<FormattedNumber style='currency' currency='BRL' value={pedido.ValorPago} />
                </p>
              <nobr>
                <p>
                  <strong>Status:</strong>&nbsp;{pedido.Status}
                </p>
              </nobr>
              {this.renderizarBoleto(pedido)}
            </div>
          </div>
          <div className='media-right hidden-xs'>
            <nobr>
              <p>
                <strong>Valor do Pedido:</strong>&nbsp;<FormattedNumber style='currency' currency='BRL' value={pedido.Valor} />
              </p>
            </nobr>
            <nobr>
              <p>
                <strong>Valor Confirmado:</strong>&nbsp;<FormattedNumber style='currency' currency='BRL' value={pedido.ValorPago} />
              </p>
            </nobr>
            <nobr>
              <p>
                <strong>Status:</strong>&nbsp;{pedido.Status}
              </p>
            </nobr>
            {this.renderizarBoleto(pedido)}
          </div>
        </div>
      </div>
    )
  }
}

Pedido.propTypes = {
  pedido: React.PropTypes.object.isRequired,
  atualizarBoleto: React.PropTypes.func.isRequired,
  boletosAtualizando: React.PropTypes.object.isRequired
}
