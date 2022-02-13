import React, { Component } from 'react'
import { FormattedNumber, FormattedDate } from 'react-intl'
import { Modal, Button, ProgressBar } from 'react-bootstrap'
import { withRouter } from 'react-router'
import moment from 'moment'
import Decimal from 'decimal.js';

import _ from 'lodash'

import params from '../constants/pagar-me-parametros'
import { EncryptionKey } from '../constants/pagar-me-keys'
import { postbackServer } from '../constants/endpoints'

class Subtotal extends Component {

  componentWillReceiveProps (nextProps) {
    if (nextProps.pedido.enviar === true) {
      const router = nextProps.router
      const idPedido = nextProps.pedido.dados.IdPedido
      let postbackUrl = window.encodeURI(`${postbackServer}/transacoes/${idPedido}`)
      const amount = this.obterValor().times(100).ceil().toNumber()
      const myParams = Object.assign(
        {},
        params,
        {
          amount,
          boletoExpirationDate: this.obterDataBoleto().toISOString(),
          postbackUrl
        })
      const checkout = new window.PagarMeCheckout.Checkout({
        encryption_key: EncryptionKey,
        success: function () {
          router.push(`/pagamento/${idPedido}`)
        }
      })
      checkout.open(myParams)
      nextProps.limparPedido()
    }
  }

  renderizarLinha (conta) {
    return (
      <tr key={conta.IdContaReceber}>
        <td>{conta.Historico}</td>
        <td><FormattedDate value={conta.Vencimento} /></td>
        <td><FormattedNumber value={conta.Saldo} style='currency' currency='BRL' /></td>
      </tr>
    )
  }

  obterValor () {
    const valor = _.sumBy(this.obterSelecionados(), 'Saldo')
    return isNaN(valor) ? new Decimal(0) : new Decimal(parseFloat(parseFloat(valor).toFixed(2)))
  }

  obterSelecionados () {
    return _.union(
      _.filter(this.props.contasReceber.mesAtualOuVencido, { selecionado: true }),
      _.filter(this.props.contasReceber.outrosMeses, { selecionado: true })
    )
  }

  obterDataBoleto () {
    const datas = this.obterSelecionados().map(s => moment(s.Vencimento))
    const menorData = moment.min(datas)
    if (menorData.isSameOrBefore(moment())) {
      return moment().add(1, 'd')
    }
    return menorData
  }

  pagarMeCheckout () {
    this.props.obterPedido(this.obterSelecionados())
  }

  fecharModal () {
    this.props.limparPedido()
  }

  render () {
    return (
      <div className='subtotal'>
        <div className='panel panel-default'>
          <div className='panel-body'>
            <h2>Subtotal</h2>
            <div className='pull-right total'>
              <span className='valor-total'><FormattedNumber style='currency' currency='BRL' value={this.obterValor()} />*</span>
            </div>
            <span>
              <small>*Valor à vista ou em 1x no cartão de crédito</small>
            </span>
            <button disabled={!(this.obterValor() > 0)} className='btn-pagar btn btn-primary btn-block btn-lg' type='button' onClick={this.pagarMeCheckout.bind(this)} ><i className='fa fa-shopping-cart'/>&nbsp;Pagar Agora</button>
          </div>
          <table className='table'>
            <thead>
              <tr>
                <th>Item</th>
                <th>Vencimento</th>
                <th>Valor</th>
              </tr>
            </thead>
            <tbody>
              {
                this.obterSelecionados().map(conta => this.renderizarLinha(conta))
              }
            </tbody>
          </table>
        </div>
        <Modal show={this.props.pedido.erro === true}>
          <Modal.Header>
            <Modal.Title>Ocorreu um erro</Modal.Title>
          </Modal.Header>
          <Modal.Body><p>Ocorreu um erro ao processar seu pedido, por favor tente novamente</p></Modal.Body>
          <Modal.Footer>
            <Button onClick={this.fecharModal.bind(this)}>OK</Button>
          </Modal.Footer>
        </Modal>
        <Modal show={this.props.pedido.obtendo === true}>
          <Modal.Body>
            <p>Processando seu pagamento.</p>
            <p>Não feche essa aba ou saia do navegador</p>
            <ProgressBar now={100} active />
          </Modal.Body>
        </Modal>
      </div>
    )
  }
}

Subtotal.propTypes = {
  contasReceber: React.PropTypes.object,
  obterPedido: React.PropTypes.func,
  pedido: React.PropTypes.object,
  limparPedido: React.PropTypes.func,
  obterStatusPagamento: React.PropTypes.func,
  router: React.PropTypes.object.isRequired
}

export default withRouter(Subtotal)
