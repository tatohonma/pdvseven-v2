import React, { Component } from 'react'
import { withRouter } from 'react-router'
import { FormattedDate } from 'react-intl'
import Carregando from './carregando'

class ConfirmacaoPagamento extends Component {

  constructor (props) {
    super(props)
    this.voltar = this.voltar.bind(this)
  }

  componentWillMount () {
    this.props.obterStatusPagamento(this.props.params.idPedido)
  }

  voltar () {
    this.props.router.goBack()
  }

  navegarUrl (url) {
    const w = window.open(url, '_blank')
    w.focus()
  }

  render () {
    const { pagamento } = this.props
    const { IdPedido, Status, DataVencimentoBoleto, UrlBoleto } = pagamento.dados

    let component = (
      <div className='jumbotron'>
        <h1>Pagamento enviado</h1>
        <p>Pagamento nº <strong>{IdPedido}</strong></p>
        <p>Status: <strong>{Status}</strong></p>
        {
          UrlBoleto
          ? <div>
            <p>Vencimento: <strong><FormattedDate value={DataVencimentoBoleto}/></strong></p>
            <button className='btn btn-primary' type='button' onClick={this.navegarUrl.bind(null, UrlBoleto)}>
              <i className='fa fa-barcode fa-fw'></i>&nbsp;Visualizar Boleto
            </button>
            </div>
          : null
        }
        <p></p>
        <button className='btn btn-primary' type='button' onClick={this.voltar}>Voltar</button>
      </div>
    )

    if (this.props.pagamento.obtendo === true) {
      component = <Carregando />
    } else if (this.props.pagamento.erro === true) {
      component = (
        <div className='jumbotron'>
          <h1>Ops!</h1>
          <p>O pagamento nº<strong>{this.props.params.idPedido}</strong> não foi encontrado.</p>
          <button className='btn btn-primary' type='button' onClick={this.voltar}>Voltar</button>
        </div>
      )
    }

    return component
  }
}

ConfirmacaoPagamento.propTypes = {
  pagamento: React.PropTypes.object.isRequired,
  params: React.PropTypes.object,
  router: React.PropTypes.object.isRequired,
  obterStatusPagamento: React.PropTypes.func
}

export default withRouter(ConfirmacaoPagamento)
