import React, { Component } from 'react'
import { FormattedNumber, FormattedDate } from 'react-intl'
import moment from 'moment'

export default class ContaReceber extends Component {

  estaVencido (dataVencimento) {
    return moment(dataVencimento).isBefore(moment())
  }

  render () {
    const { conta } = this.props
    return (
      <div className={'list-group-item item-conta' + (conta.selecionado ? ' active' : '')} onClick={this.props.alterarSelecionado.bind(null, conta.IdContaReceber)} >
        <div className='media'>
          <div className='media-left'>
            {conta.selecionado ? <i className='fa fa-check-square-o fa-3x fa-fw' /> : <i className='fa fa-square-o fa-3x fa-fw' />}
          </div>
          <div className='media-body'>
            <h4 className='media-title'>{conta.Historico}</h4>
            <p className={'media-data-vencimento ' + (this.estaVencido(conta.Vencimento) ? 'vencido' : '')}><i className='fa fa-calendar-o fa-2x' />&nbsp;<FormattedDate value={conta.Vencimento} /></p>
          </div>
          <div className='media-right'>
            <nobr><p className='media-valor-pagamento'><FormattedNumber style='currency' currency='BRL' value={conta.Saldo} /></p></nobr>
          </div>
        </div>
      </div>
    )
  }
}

ContaReceber.propTypes = {
  conta: React.PropTypes.object.isRequired,
  alterarSelecionado: React.PropTypes.func.isRequired
}
