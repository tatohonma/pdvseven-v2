import React, { Component } from 'react'

export default class TabelaLog extends Component {

  renderLog (log, i) {
    try {
      const linha = log.split('|')
      const data = linha[0]
      const fonte = linha[1]
      const tipo = linha[2]
      const mensagem = linha[3]
      return (
      <tr key={i} className={tipo === 'erro' ? 'danger' : ''}>
        <td>{data}</td>
        <td>{fonte}</td>
        <td>{tipo}</td>
        <td>{mensagem}</td>
      </tr>
      )
    } catch (e) {
      console.log(e)
      return null
    }
  }

  render () {
    return (
      <table className='table table-hover table-condensed'>
        <thead>
          <tr>
            <th>Data</th>
            <th>Fonte</th>
            <th>Tipo</th>
            <th>Mensagem</th>
          </tr>
        </thead>
        <tbody>
          {this.props.log.reverse().map((log, i) => this.renderLog(log.log, i))}
        </tbody>
      </table>
    )
  }
}
