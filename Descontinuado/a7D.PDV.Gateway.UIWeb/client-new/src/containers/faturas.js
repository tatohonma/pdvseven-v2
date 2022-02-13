import React, { Component } from 'react'
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux'
import * as actionCreators from '../actions'
import Fatura from '../components/fatura'
import Erro from '../components/erro'

function mapDispatchToProps (dispatch) {
  return bindActionCreators(actionCreators, dispatch)
}

function mapStateToProps (state) {
  return {
    faturas: state.faturas
  }
}

class Faturas extends Component {

  componentWillMount () {
    this.props.obterFaturas(this.props.params.id)
  }

  renderizarFatura (fatura) {
    return (
      <Fatura key={fatura.idFatura} fatura={fatura} />
    )
  }

  first (obj) {
    for (const a in obj) return a;
  }

  render () {
    const { faturas, atuais, obtendo, proximas, erro, msg } = this.props.faturas
    const haFaturas = Object.keys(faturas).length > 0
    const nomeCliente = (haFaturas ? faturas[this.first(faturas)].contaReceber.cliente.nome : '')
    return (
      <div>
      {haFaturas
        ? ( 
          <div>
            <h1>Faturas</h1>
            <p><strong>{nomeCliente}</strong>, você tem {Object.keys(atuais).length} faturas atuais</p>
            <h2 id='atuais'>Atuais</h2>
            { 
              Object.keys(atuais).length > 0
              ? Object.keys(atuais).map(key => this.renderizarFatura(atuais[key]))
              : null
            }
            <h3>Próximas</h3>
            <p>Consulte datas e valores de suas próximas faturas</p>
            { 
              Object.keys(proximas).length > 0
              ? Object.keys(proximas).map(key => this.renderizarFatura(proximas[key]))
              : null
            }
          </div>
        )
        : null
      }
      {!haFaturas && !obtendo && !erro
        ? <p>Não há faturas pendentes</p>
        : null
      }
      {erro === true
        ? <Erro mensagem={msg} />
        : null
      }
      </div>
    )
  }
}

Faturas.propTypes = {
  params: React.PropTypes.object.isRequired,
  obterFaturas: React.PropTypes.func.isRequired,
  faturas: React.PropTypes.object.isRequired
}

export default connect(mapStateToProps, mapDispatchToProps)(Faturas)