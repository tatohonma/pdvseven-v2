import React from 'react'

export default class Erro extends React.Component {
  render () {
    return (
      <div className='alert alert-danger' role='alert'>
        <p><strong>Erro!</strong></p>
        <p>Ocorreu um erro em sua solicitação.</p>
        <p>Por favor entre em contato com a PDSeven.</p>
        <pre>
          <p>{this.props.mensagem}</p>
        </pre>
      </div>
    )
  }
}

Erro.propTypes = {
  mensagem: React.PropTypes.string.isRequired
}