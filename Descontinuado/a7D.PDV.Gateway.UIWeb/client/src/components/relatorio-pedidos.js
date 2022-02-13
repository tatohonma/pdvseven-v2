import React from 'react'
import { Table } from 'react-bootstrap'
import { FormattedNumber, FormattedDate } from 'react-intl'
import DatePicker from 'react-datepicker'
import moment from 'moment'
import necessitaAutenticacao from './necessita-autenticacao'
import DetalheRelatorioPedido from './detalhe-relatorio-pedido'
import _ from 'lodash'

class RelatorioPedidos extends React.Component {
  constructor (props) {
    super(props)
    this.displayName = 'RelatorioPedidos'
    this.state = {
      startDate: moment().subtract(10, 'days'),
      endDate: moment(),
      somentePagos: true
    }
    moment.locale('pt-BR')
  }

  componentDidMount () {
    this.props.obterPedidosRelatorio(this.props.auth.token)
  }

  pedidoAberto (idPedido) {
    return this.props.pedidosRelatorioDetalhes[idPedido] === true
  }

  abrirFecharPedido (idPedido) {
    if (this.pedidoAberto(idPedido)) {
      this.props.fecharDetalhes(idPedido)
    } else {
      this.props.abrirDetalhes(idPedido)
    }
  }

  renderizarLinha (pedido) {
    if (this.state.somentePagos && pedido.status !== 'paid') {
      return null
    }
    const itensPedido = pedido.itens
    const itens = this.props.pedidosRelatorio.itens
    const itensPedidoRelatorio = itensPedido.map(item => itens[item])
    const cliente = itens[itensPedido[0]].contaReceber.cliente.nome
    const boleto = !pedido.bandeira
    const idPedido = pedido.idPedido;
    const pedidoAberto = this.pedidoAberto(idPedido)
    const linha = (
      <tr key={idPedido}>
        <td><i className={'fa fa-fw fake-a ' + (pedidoAberto ? 'fa-minus-square-o' : 'fa-plus-square-o')} onClick={() => this.abrirFecharPedido(idPedido)}></i></td>
        <td>{idPedido}</td>
        <td>{cliente}</td>
        <td><FormattedNumber value={pedido.valor} style='currency' currency='BRL' /></td>
        <td>{pedido.status}</td>
        <td><a href={`https://dashboard.pagar.me/#/transactions/${pedido.idTransacao}`} target='_blank'>{pedido.idTransacao}</a></td>
        <td>
        {pedido.dataAutorizacao
          ? <FormattedDate 
            year='numeric'
            month='numeric'
            day='numeric'
            hour='numeric'
            minute='numeric'
            second='numeric'
            timeZoneName='short'
            value={moment(pedido.dataAutorizacao)} />
          : '-'}
        </td>
        <td>{pedido.dataPagamento ? <FormattedDate value={moment(pedido.dataPagamento)} /> : '-'}</td>
        <td>{boleto ? 'Boleto' : `Cartão ${pedido.bandeira}`}</td>
        <td>{pedido.parcelas}</td>
      </tr>
    )
    if (itensPedido && itensPedido.length > 0 && pedidoAberto) {
      return [linha, <tr key={pedido.idPedido + 'detalhes'}>
          <td colSpan={10}>
            <DetalheRelatorioPedido itens={itensPedidoRelatorio} />
          </td>
      </tr>]
    } else {
      return linha
    }
  }

  renderizarResumo (pedidos) {
    let keys = Object.keys(pedidos)
    if (keys.length < 1) {
      return null
    }
    const grupo = keys.reduce((obj, key) => {
      const pedido = pedidos[key];
      const node = obj[pedido.status]
      if (node) {
        node.valor += pedido.valor
      } else {
        obj[pedido.status] = { valor: pedido.valor }
      }
      return obj
    }, {})
    keys = Object.keys(grupo)
    /* eslint-disable react/prop-types */
    const LinhaResumo = (props) => {
      const { status, valor } = props
      return (
          <tr>
            <td>{status}</td>
            <td><FormattedNumber value={valor} style='currency' currency='BRL' /></td>
          </tr>
        )
    }
    /* eslint-enable */
    return (
      <Table responsive hover>
        <thead>
          <tr>
            <th>Status</th>
            <th>Valor</th>
          </tr>
        </thead>
        <tbody>
          {keys.map(key => <LinhaResumo key={key} status={key} valor={grupo[key].valor} />)}
        </tbody>
      </Table>
      )
  }

  /* eslint-disable react/no-set-state */
  setStartDate (date) {
    this.setState({
      startDate: date
    })
  }
  
  setEndDate (date) {
    this.setState({
      endDate: date
    })
  }

  alterarSomentePagos (e) {
    this.setState({
      somentePagos: !this.state.somentePagos
    })
  }

  /* eslint-enable */

  filtrar (event) {
    event.preventDefault();
    this.props.obterPedidosRelatorio(this.props.auth.token, this.state.startDate, this.state.endDate);
  }

  render () {
    const pedidos = this.props.pedidosRelatorio.pedidos
    const keys = !pedidos ? null : Object.keys(pedidos)
    let pedidosFlat = []
    if (keys !== null) {
      pedidosFlat = keys.map(key => pedidos[key])
      pedidosFlat = _.orderBy(pedidosFlat, ['dataPagamento'], ['desc'])
    }
    const loading = this.props.pedidosRelatorio.isFetching
    const Carregando = () => {
      return (
          <tr><td colSpan={6}>Carregando... <i className='fa fa-spinner fa-pulse fa-fw'></i></td></tr>
        )
    }
    return (
        <div className='relatorio-body'>
          <div className='row'>
            <form className='form-inline' onSubmit={this.filtrar.bind(this)}>
              <div className='form-group'>
                <label>Data de:&nbsp;</label>
                <DatePicker 
                  className='form-control' 
                  todayButton={'Hoje'}
                  maxDate={this.state.endDate}
                  startDate={this.state.startDate}
                  endDate={this.state.endDate}
                  selected={this.state.startDate} 
                onChange={this.setStartDate.bind(this)} />
              </div>
              <div className='form-group'>
                <label>Data até:&nbsp;</label>
                <DatePicker 
                  className='form-control' 
                  todayButton={'Hoje'}
                  minDate={this.state.startDate}
                  startDate={this.state.startDate}
                  endDate={this.state.endDate}
                  selected={this.state.endDate} 
                  onChange={this.setEndDate.bind(this)} />
              </div>
              <div className='form-group'>
                <button
                  className='btn btn-primary'
                  type='submit'
                  bsStyle='primary'
                  disabled={this.props.pedidosRelatorio.isFetching}>
                  <i className={'fa fa-fw ' + (this.props.pedidosRelatorio.isFetching ? 'fa-spinner fa-pulse' : 'fa-filter')}></i>
                  &nbsp;
                  {this.props.pedidosRelatorio.isFetching ? 'Filtrando...' : 'Filtrar'}
                </button>
              </div>
            </form>
          </div>
          <div className='row'>
            <div className='col-md-offset-2 col-md-3'>
              {keys ? this.renderizarResumo(pedidos) : null}
            </div>
          </div>
          <Table responsive hover>
            <thead>
              <tr>
                <th></th>
                <th>Pedido</th>
                <th>Cliente</th>
                <th>Valor</th>
                <th>Status</th>
                <th>Transação pagar.me</th>
                <th>Data Autorizacao</th>
                <th>Data Pagamento</th>
                <th>Método de Pagamento</th>
                <th>Parcelas</th>
              </tr>
            </thead>
            <tbody>
              {loading 
                ? <Carregando /> 
                : (
                    keys ? pedidosFlat.map(pedido => this.renderizarLinha(pedido)) : null
                  )
              }
            </tbody>
          </Table>
        </div>
      )
  }
}

RelatorioPedidos.propTypes = { 
  obterPedidosRelatorio: React.PropTypes.func,
  auth: React.PropTypes.object,
  pedidosRelatorio: React.PropTypes.object,
  pedidosRelatorioDetalhes: React.PropTypes.object,
  fecharDetalhes: React.PropTypes.func.isRequired,
  abrirDetalhes: React.PropTypes.func.isRequired
}

export default necessitaAutenticacao(RelatorioPedidos)
