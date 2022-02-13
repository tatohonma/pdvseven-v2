/* eslint-disable react/no-set-state */
import React, { Component } from 'react'
import { withRouter } from 'react-router'
import MaskedInput from 'react-maskedinput'

class Home extends Component {

  constructor (props) {
    super(props)
    this.onSubmit = this.onSubmit.bind(this)
    this.state = {
      enviado: false,
      erro: false
    }
  }

  componentDidMount () {
    this.props.limparState()
  }

  onSubmit (e) {
    e.preventDefault()
    this.setState(Object.assign({}, this.state, {enviado: true}))
    const codigo = this.refs.codigo.input.value
    if (codigo.indexOf('_') > 0) {
      this.setState({enviado: true, erro: true})
      return
    }
    const form = this.refs.frm
    form.reset()
    this.setState({enviado: false, erro: false})
    this.props.router.push(`/${codigo}`)
    this.props
  }

  render () {
    const { enviado, erro } = this.state
    return (
      <div className='container'>
        <div className='jumbotron'>
          <p>Insira seu número de licença abaixo para começar</p>
          <form ref='frm' onSubmit={this.onSubmit}>
            <div className={'input-group' + ((enviado && erro) ? ' has-error' : '')}>
              <MaskedInput mask='111-11111-11' type='text' ref='codigo' className='form-control' placeholder='000-00000-00'/>
              <span className='input-group-btn'>
                <input type='submit' value='OK' className='btn btn-default'/>
              </span>
            </div>
          </form>
        </div>
      </div>
    )
  }
}

Home.propTypes = {
  router: React.PropTypes.shape({
    push: React.PropTypes.func.isRequired
  }).isRequired,
  limparState: React.PropTypes.func.isRequired
}

export default withRouter(Home)
