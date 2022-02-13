import React, { Component } from 'react'
import NovaData from './nova-data'
import { FormattedNumber } from 'react-intl'
import PagarComBoleto from '../containers/pagar-com-boleto'
import PagarComCartao from './pagar-com-cartao'

class Fatura extends Component {
  render () {
    const { fatura } = this.props
    return (
      <div className='row pagamento vdivide'>
        <div className='col-md-1 vencimento'>
          <h4 className='titulo centralizado'>Venc.</h4>
          <NovaData data={fatura.contaReceber.vencimento} />
        </div>
        <div className='col-md-6 servicos'>
          <h4 className='titulo'>Serviços</h4>
          <h4 className='servico-titulo'>{fatura.contaReceber.historico}</h4>
        </div>
        <div className='col-md-2 total'>
          <h4 className='titulo centralizado'>Total</h4>
          <h4 className='valor-total centralizado'><FormattedNumber value={fatura.contaReceber.saldo} style='currency' currency='BRL'/></h4>
        </div>
        <div className='col-md-3 acoes'>
          <h4 className='titulo'></h4>
          <PagarComBoleto fatura={fatura}/>
          <PagarComCartao fatura={fatura}/>
        </div>
      </div>
    )
  }
}

Fatura.propTypes = {
  fatura: React.PropTypes.object.isRequired
}

export default Fatura