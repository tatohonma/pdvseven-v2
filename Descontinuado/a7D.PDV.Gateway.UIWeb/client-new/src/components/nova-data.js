import React, { Component } from 'react'
import moment from 'moment'
import { FormattedDate } from 'react-intl'

class NovaData extends Component {
  render () {
    const data = moment(this.props.data)
    const vencido = data.isSameOrBefore(moment())
    const dia = data.format('DD')
    const mes = data.format('MMM')
    const ano = data.format('Y')
    return (
        <div>
          <div className={'data visible-md-block visible-lg-block ' + (vencido ? 'text-danger' : '')}>
            <div className='dia'>{dia}</div>
            <div className={'mes ' + (vencido ? 'text-danger' : '')}>{mes}</div>
            <div className='ano'>{ano}</div>
          </div>
          <div className={'hidden-md hidden-lg centralizado ' + (vencido ? 'text-danger' : '')}>
            <h4><FormattedDate value={data} /></h4>
          </div>
        </div>
      )
  }
}

NovaData.propTypes = {
  data: React.PropTypes.string.isRequired
}

export default NovaData