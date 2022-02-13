import React, { Component } from 'react'
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux'
import * as actionCreators from '../actions'
import { Popover, Modal } from 'react-bootstrap'
import moment from 'moment'

function mapStateToProps (state) {
  return {
    boleto: state.boleto
  }
}

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

class PagarComBoleto extends Component {

  componentWillReceiveProps (nextProps) {
    if (nextProps.boleto.novaFatura) {
      this.abrirLinkBoleto(nextProps.boleto.novaFatura.urlBoleto)
      nextProps.limparBoleto()
    }
  }

  gerarNovoBoleto (fatura) {
    this.props.gerarNovoBoleto(fatura.idFatura)
  }

  abrirLinkBoleto (url) {
    const w = window.open(url, '_blank')
    w.focus()
  }

  vencido (vencimento) {
    return moment().isSameOrAfter(moment(vencimento))
  }

  popoverTop (msg) {
    return (
      <Popover title='Ocorreu um erro'>
        <strong>{msg}</strong>
      </Popover>
      )
  }

  render () {
    const { urlBoleto, dataBoleto } = this.props.fatura
    const { obtendo, erro, msg } = this.props.boleto
    const vencido = this.vencido(dataBoleto)
    const click = (!vencido ? this.abrirLinkBoleto.bind(this, urlBoleto) : this.gerarNovoBoleto.bind(this, this.props.fatura))
    return (
      <button className='btn btn-danger btn-block' type='button' onClick={click}>
        {obtendo
          ? <i className='fa fa-fw fa-spin fa-circle-o-notch'></i>
          : <i className='fa fa-fw fa-barcode'></i>
        }
        {obtendo
          ? ' Obtendo novo boleto'
          : ' Pagar com Boleto'
        }
        <Modal show={erro} onHide={this.props.limparBoleto}>
          <Modal.Header closeButton>
            <Modal.Title>Ocorreu um erro</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p>{msg}</p>
          </Modal.Body>
        </Modal>
      </button>
    )
  }
}

PagarComBoleto.propTypes = {
  fatura: React.PropTypes.object.isRequired,
  gerarNovoBoleto: React.PropTypes.func.isRequired,
  boleto: React.PropTypes.object.isRequired
}

export default connect(mapStateToProps, mapDispatchToProps)(PagarComBoleto)