/* eslint-disable react/no-set-state */
import React, { Component } from 'react'
import moment from 'moment'
import { Collapse } from 'react-bootstrap'
import ContaReceber from '../components/conta-receber'

export default class ContasReceber extends Component {
  constructor (args) {
    super(...args)

    this.state = {
      open: true
    }
  }

  componentWillMount () {
    
  }

  componentWillReceiveProps (nextProps) {
    const pendencias = nextProps.contas.length > 0
    if (!pendencias) {
      this.setState({ open: false })
    } else {
      this.setState({ open: true })
    }
  }

  mesAtualOuVencido (dataVencimento) {
    return moment(dataVencimento).isSameOrBefore(moment(), 'month')
  }

  renderizarContaReceber (conta, i, alterarSelecionado) {
    return (
      <ContaReceber conta={conta} key={i} alterarSelecionado={alterarSelecionado} />
    )
  }

  render () {
    const pendencias = this.props.contas.length > 0
    return (
      <div className='panel panel-default'>
        <div className='panel-heading' onClick={() => this.setState({ open: !this.state.open })}>
          <h4 className='panel-title'>
            {this.props.titulo}&nbsp;<span className='badge'>{this.props.contas.length}</span>
          </h4>
        </div>
        <Collapse in={this.state.open}>
          <div className='list-group'>
            {
              pendencias
              ? this.props.contas.map((conta, i) => this.renderizarContaReceber(conta, i, this.props.alterarSelecionado))
              : <div className='list-group-item'>
                  <div className='well'>
                    <div className='media'>
                      <div className='media-body'>
                        <h4 className='media-heading'>
                          <i className='fa fa-thumbs-o-up fa-fx'></i>&nbsp;Sem pendencias
                        </h4>
                      </div>
                    </div>
                  </div>
                </div>
            }
          </div>
        </Collapse>
      </div>
    )
  }
}

ContasReceber.propTypes = {
  contas: React.PropTypes.array,
  alterarSelecionado: React.PropTypes.func,
  titulo: React.PropTypes.string.isRequired
}
