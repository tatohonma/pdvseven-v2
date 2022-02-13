import React, { Component } from 'react'
import { withRouter } from 'react-router'
import { Button, Tabs, Tab } from 'react-bootstrap'
import { FormattedDate, FormattedTime } from 'react-intl'

import RelatorioPedidos from './relatorio-pedidos'

class ControlPanel extends Component {

  constructor (props) {
    super(props)
    this.state = {
      linhas: 15
    }
    this.linhasChanged = this.linhasChanged.bind(this)
    this.obterLog = this.obterLog.bind(this)
  }

  componentWillMount () {
    this.props.obterTempos(this.props.auth.token)
  }

  sincronizar () {
    this.props.sincronizarAgora(this.props.auth.token)
  }

  obterLog (tipo) {
    this.props.obterLog(this.props.auth.token, this.state.linhas, tipo)
  }

  /* eslint-disable react/no-set-state */
  linhasChanged (event) {
    this.setState({ linhas: parseInt(this.refs.select_linhas.value, 10) })
  }
  /* eslint-enable */

  render () {
    const { last, next } = this.props.sync
    return (
      <Tabs defaultActiveKey={1} animation={false}>
        <Tab eventKey={1} title='Relatório'>
          <RelatorioPedidos {...this.props} />
        </Tab>
        <Tab eventKey={2} title='Sincronização'>
          <div className='jumbotron'>
            Ultima sincronização: {last ? <span><FormattedDate value={last}/> às <FormattedTime value={last} /></span> : 'nenhuma'}
            <br />
            Próxima sincronizacão: {next ? <span><FormattedDate value={next} /> às <FormattedTime value={last} /></span> : 'nenhuma'}
            <br />
            <Button
              bsType='primary'
              disabled={this.props.syncing}
              onClick={this.props.syncing ? null : this.sincronizar.bind(this)}>
              {
                this.props.syncing
                ? <span><i className='fa fa-refresh fa-spin fa-fw'></i>Sincronizar agora</span>
                : <span><i className='fa fa-refresh fa-fw'></i>Sincronizar agora</span>
              }
              </Button>
          </div>
        </Tab>
        <Tab eventKey={3} title='Logs'>
          <br />
          <div className='row'>
            <div className='col-md-4'>
              <div className='input-group'>
                <span className='input-group-addon'>
                  Linhas
                </span>
                <select type='select' className='form-control' ref='select_linhas' onChange={this.linhasChanged}>
                  <option value='15'>15</option>
                  <option value='30'>30</option>
                  <option value='60'>60</option>
                </select>
                <span className='input-group-btn'>
                  <button className='btn btn-default button-dropdown' type='button' data-toggle='dropdown'>
                    <span className='caret'></span>
                    &nbsp;Logs
                  </button>
                  <ul className='dropdown-menu'>
                    <li>
                      <a className='fake-a' onClick={this.obterLog.bind(null, '')}>Info</a>
                    </li>
                    <li>
                      <a className='fake-a' onClick={this.obterLog.bind(null, 'debug')}>Debug</a>
                    </li>
                    <li>
                      <a className='fake-a' onClick={this.obterLog.bind(null, 'error')}>Error</a>
                    </li>
                  </ul>
                </span>
              </div>
            </div>
          </div>
          <br />
          <div className='row'>
            <div className='col-md-10'>
              <textarea 
                style={{ minWidth: '100%' }} 
                value={this.props.log.log}
                rows={this.state.linhas}>
              </textarea>
            </div>
          </div>
        </Tab>
      </Tabs>
    )
  }
}

ControlPanel.propTypes = {
  sincronizarAgora: React.PropTypes.func.isRequired,
  auth: React.PropTypes.object.isRequired,
  syncing: React.PropTypes.bool.isRequired,
  sync: React.PropTypes.object.isRequired,
  obterTempos: React.PropTypes.func.isRequired,
  obterLog: React.PropTypes.func.isRequired,
  log: React.PropTypes.object.isRequired
}

export default withRouter(ControlPanel)
