import React, { Component } from 'react'
import ContasReceber from '../containers/contas-receber'
import PedidosPendentes from '../containers/pedidos-pendentes'
import Subtotal from './subtotal'
import Carregando from './carregando'

export default class Sucesso extends Component {

  componentDidMount () {
    this.props.obterContasPagar(this.props.params.chaveAtivacao)
  }

  render () {
    const { props } = this
    return (
      <div>
        <div className='page-header'>
          <h1>{props.cliente.Nome}</h1>
        </div>
        <div className='col-md-8'>
          <h4>Selecione abaixo os pagamentos a serem efetuados&nbsp;<i className='fa fa-hand-o-down'></i></h4>
          {
            props.contasReceber.obtendo === true
            ? <Carregando />
            : <div className='list-group'>
                <ContasReceber
                  titulo='Pagamentos Pendentes'
                  contas={props.contasReceber.mesAtualOuVencido}
                  alterarSelecionado={props.alterarSelecionado} />
              </div>
          }
          <PedidosPendentes {...this.props} />
          {
            props.contasReceber.obtendo === true
            ? <Carregando />
            : <ContasReceber
                titulo='Próximos vencimentos'
                contas={props.contasReceber.outrosMeses}
                alterarSelecionado={props.alterarSelecionado} />
          }
        </div>
        <div className='col-md-4'>
          <Subtotal {...props} />
        </div>
      </div>
    )
  }
}

Sucesso.propTypes = {
  cliente: React.PropTypes.object,
  contasReceber: React.PropTypes.object.isRequired,
  alterarSelecionado: React.PropTypes.func.isRequired,
  obterContasPagar: React.PropTypes.func.isRequired,
  pedidosPendentes: React.PropTypes.object.isRequired,
  params: React.PropTypes.object
}
