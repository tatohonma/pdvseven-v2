import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as actionCretors from '../actions'
import _ from 'lodash'
const $ = window.jQuery

function mapStateToProps (state) {
  return {
    contas: state.contasReceber
  }
}

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCretors, dispatch)
}

class BotaoCarrinho extends Component {

  obterSelecionados () {
    return _.union(
      _.filter(this.props.contas.mesAtualOuVencido, { selecionado: true }),
      _.filter(this.props.contas.outrosMeses, { selecionado: true })
    )
  }

  scrollToBottom (event) {
    event.preventDefault()
    $('html, body').animate({
      scrollTop: $('#bottom').offset().top
    }, 1000);
  }

  render () {
    const quantidade = this.obterSelecionados().length
    return (
        <button className='btn btn-primary navbar-btn visible-xs-block visible-sm-block' type='button' onClick={this.scrollToBottom.bind(this)}>
          <i className='fa fa-fw fa-shopping-cart'></i>&nbsp;
          <span className='badge'>{quantidade}</span>
        </button>
      )
  }
}

BotaoCarrinho.propTypes = {
  contas: React.PropTypes.object.isRequired
}

export default connect(mapStateToProps, mapDispatchToProps)(BotaoCarrinho)