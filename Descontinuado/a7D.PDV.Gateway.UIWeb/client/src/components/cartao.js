import React, { Component } from 'react'
import { FormattedNumber } from 'react-intl'

export default class Cartao extends Component {

  obterCartao (bandeira) {
    switch (bandeira) {
      case 'visa':
        return 'fa-cc-visa'
      case 'mastercard':
        return 'fa-cc-mastercard'
      case 'amex':
        return 'fa-cc-amex'
      default:
        return 'fa-credit-card-alt'
    }
  }

  render () {
    const { parcelas, valor, bandeira, ultimosDigitos } = this.props
    return (
      <div className='well'>
        <p>
          <strong>{parcelas} &times; </strong>
          <FormattedNumber value={parseFloat(valor / parcelas).toFixed(2)} style='currency' currency='BRL' />
        </p>
        <p><i className={'fa fa-fw ' + this.obterCartao(bandeira)}></i>&nbsp;final&nbsp;{ultimosDigitos}</p>
      </div>
    )
  }
}

Cartao.propTypes = {
  parcelas: React.PropTypes.number.isRequired,
  valor: React.PropTypes.number.isRequired,
  bandeira: React.PropTypes.string.isRequired,
  ultimosDigitos: React.PropTypes.string.isRequired
}
