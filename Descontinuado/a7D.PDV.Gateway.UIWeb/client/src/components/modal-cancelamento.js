import React, { Component } from 'react'
import { Modal, Button } from 'react-bootstrap'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as actionCreators from '../actions/index'

function mapStateToProps (state) {
  return {
    modalCancelamento: state.modalCancelamento
  }
}

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

class ModalCancelamento extends Component {

  cancelar () {
    const { cancelarPedido, idPedido, fecharModal } = this.props
    cancelarPedido(idPedido)
    fecharModal()
  }

  render () {
    return (
      <Modal show={this.props.modalCancelamento} onHide={this.props.fecharModal}>
        <Modal.Header closeButton>
          <Modal.Title>Atenção!</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Só cancele o pedido caso deseje pagar com outra forma de pagamento.</p>
          <p>Se você já efetuou o pagamento do boleto <strong>não cancele</strong> o pedido, entre em contato com a PDVSeven.</p>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.props.fecharModal}>
          Fechar
          </Button>
          <Button bsStyle='danger' onClick={this.cancelar.bind(this)}>
          Cancelar Pedido
          </Button>
        </Modal.Footer>
      </Modal>
    )
  }
}

ModalCancelamento.propTypes = {
  modalCancelamento: React.PropTypes.bool.isRequired,
  fecharModal: React.PropTypes.func.isRequired,
  idPedido: React.PropTypes.number.isRequired,
  cancelarPedido: React.PropTypes.func.isRequired
}

export default connect(mapStateToProps, mapDispatchToProps)(ModalCancelamento)