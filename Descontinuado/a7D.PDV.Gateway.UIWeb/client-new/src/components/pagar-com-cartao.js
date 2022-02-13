import React, { Component } from 'react'
import params from '../constants/pagar-me-parametros'
import { postbackServer } from '../constants/endpoints'
import { EncryptionKey } from '../constants/pagar-me-keys'

class PagarComCartao extends Component {

  abrirFormPagarMe (fatura) {
    let postbackUrl = window.encodeURI(`${postbackServer}/faturas/${fatura.idFatura}`)
    const myParams = Object.assign(
      {},
      params,
      {
        amount: fatura.contaReceber.saldo * 100,
        paymentMethods: 'credit_card',
        postbackUrl
      })
    const checkout = new window.PagarMeCheckout.Checkout({
      encryption_key: EncryptionKey,
      success: function () {
        alert('sucesso');
      }
    })
    checkout.open(myParams)
  }

  render () {
    return (
      <button className='btn btn-danger btn-block' type='button' onClick={this.abrirFormPagarMe.bind(null, this.props.fatura)}>
        <span className='hidden-md'><i className='fa fa-fw fa-credit-card'></i>&nbsp;Pagar com cartão de crédito</span>
        <span className='visible-md-block'><i className='fa fa-fw fa-credit-card'></i>&nbsp;Pagar com cartão</span>
      </button>
    )
  }
}

PagarComCartao.propTypes = {
  fatura: React.PropTypes.object.isRequired
}

export default PagarComCartao