import React from 'react'
import { FormattedNumber } from 'react-intl'
import { Table } from 'react-bootstrap'

class DetalheRelatoPedido extends React.Component {

  renderizarLinha (item) {
    const { idBroker, historico, valor, saldo } = item.contaReceber
    const url = `https://www.tiny.com.br/contas_receber#edit/${idBroker}`
    return (
      <tr key={idBroker}>
        <td><a href={url} target='_blank'>{idBroker}</a></td>
        <td>{historico}</td>
        <td><FormattedNumber value={valor} style='currency' currency='BRL' /></td>
        <td><FormattedNumber value={saldo} style='currency' currency='BRL' /></td>
      </tr>
      )
  }

  render () {
    return (
      <Table responsive hover>
        <thead>
          <tr>
            <th>Transação Tiny</th>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {this.props.itens.map(item => this.renderizarLinha(item))}
        </tbody>
      </Table>
    )
  }
}

DetalheRelatoPedido.propTypes = { itens: React.PropTypes.array.isRequired }

export default DetalheRelatoPedido
