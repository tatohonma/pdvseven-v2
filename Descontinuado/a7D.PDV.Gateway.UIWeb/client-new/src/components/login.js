import React, { Component } from 'react'
import { Row, Col, Button, Alert } from 'react-bootstrap'

export default class Login extends Component {

  constructor (props) {
    super(props)

    const redirectRoute = this.props.location.query.next || '/'

    this.state = {
      redirectTo: redirectRoute
    }
  }

  login (e) {
    e.preventDefault()
    const email = this.refs.email.value
    const pwd = this.refs.pwd.value
    this.props.loginUser(email, pwd, this.state.redirectTo)
  }

  render () {
    return (
      <Row>
        <Col mdOffset={5} md={3}>
          {
            this.props.auth.statusText
            ? <Alert bsStyle='danger' onDismiss={() => this.props.logout()}>{this.props.auth.statusText}</Alert>
            : null
          }
          <h4>Login</h4>
          <form ref='login' onSubmit={!this.props.isAuthenticating && this.login.bind(this)}>
            <input type='email' className='form-control input-sm chat-input' placeholder='email' ref='email'/>
            <br />
            <input type='password' className='form-control input-sm chat-input' placeholder='password' ref='pwd'/>
            <br />
            <div className='wrapper'></div>
            <span className='group-btn'>
              <Button
                type='submit'
                bsStyle='primary'
                disabled={this.props.isAuthenticating}
                >
                {this.props.isAuthenticating ? <span>logando... <i className='fa fa-spinner fa-pulse fa-fw'></i></span> : <span>login <i className='fa fa-sign-in fa-fw'></i></span>}
              </Button>
            </span>
          </form>
        </Col>
      </Row>
    )
  }
}

Login.propTypes = {
  isAuthenticating: React.PropTypes.bool.isRequired,
  auth: React.PropTypes.object.isRequired,
  loginUser: React.PropTypes.func.isRequired,
  location: React.PropTypes.object.isRequired,
  logout: React.PropTypes.func.isRequired
}
