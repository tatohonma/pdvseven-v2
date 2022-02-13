import React, { Component } from 'react'
import { Button } from 'react-bootstrap'
import { FormattedDate } from 'react-intl'
import moment from 'moment'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as actionCreators from '../actions/index'

function mapStateToProps (state) {
  return {
    cp: state.alteracaoFormaPagamento
  }
}

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

class Boleto extends Component {

  navegarLink (url) {
    const w = window.open(url, '_blank')
    w.focus()
  }

  cancelarPedido () {
    const { idPedido } = this.props
    this.props.cancelarPedido(idPedido)
  }

  render () {
    const { dataVencimento, urlBoleto, atualizarBoleto, idPedido, gerando, erro, sucesso } = this.props
    const { isFetching, error } = this.props.cp
    return (
      <div className='well'>
        <p className={sucesso ? 'changed' : ''}>
          <strong>Vencimento:</strong>&nbsp;
          <FormattedDate value={dataVencimento}/>
        </p>
        <Button bsStyle='primary' onClick={this.navegarLink.bind(null, urlBoleto)} block><i className='fa fa-barcode fa-fw'></i>&nbsp;Boleto</Button>
        {
          moment(dataVencimento).isSameOrBefore(moment())
          ? <Button
              onClick={() => atualizarBoleto(idPedido)}
              disabled={isFetching}
              block>
              {erro ? <i className='fa fa-exclamation-triangle fa-fw' style={{color: 'red'}}></i> : null}
              <i className={gerando ? 'fa fa-spinner fa-pulse fa-fw' : 'fa fa-refresh fa-fw'}></i>
              {gerando === true ? ' Emitindo...' : ' Re-emitir Boleto'}
            </Button>
          : null
        }
        <Button 
          block
          disabled={isFetching || gerando}
          onClick={!isFetching ? this.cancelarPedido.bind(this) : null}>
          {error ? <i className='fa fa-fw fa-exclamation-triangle' style={{color: 'red'}}></i> : null}
          <i className={'fa fa-fw fa-refresh' + (isFetching ? ' fa-spin' : '')}></i>
          {isFetching && !error
            ? ' Aguarde'
            : ' Alterar forma de Pagamento'
          }
          {error ? ' Ocorreu um erro ao alterar a forma de pagamento.' : ''}
        </Button>
      </div>
    )
  }
}

Boleto.propTypes = {
  dataVencimento: React.PropTypes.string.isRequired,
  urlBoleto: React.PropTypes.string.isRequired,
  atualizarBoleto: React.PropTypes.func.isRequired,
  idPedido: React.PropTypes.number.isRequired,
  gerando: React.PropTypes.bool.isRequired,
  erro: React.PropTypes.bool.isRequired,
  sucesso: React.PropTypes.bool.isRequired,
  cp: React.PropTypes.object.isRequired,
  cancelarPedido: React.PropTypes.func.isRequired
}

export default connect(mapStateToProps, mapDispatchToProps)(Boleto)
