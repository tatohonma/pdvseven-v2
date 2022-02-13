import React, { Component } from 'react'
import { Jumbotron, Button } from 'react-bootstrap'
import { withRouter } from 'react-router'

class NaoEncontrado extends Component {
  render () {
    return (
      <Jumbotron>
        <h1>Não encontrado <small>erro 404</small></h1>
        <p>A página que você estava procurando não existe!</p>
        <Button onClick={() => this.props.router.push('/')}><i className='fa fa-home fa-fw'></i>&nbsp;Página inicial</Button>
      </Jumbotron>
    )
  }
}

NaoEncontrado.propTypes = {
  router: React.PropTypes.object.isRequired
}

export default withRouter(NaoEncontrado)
