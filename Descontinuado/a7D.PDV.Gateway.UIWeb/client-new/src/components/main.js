import React, { Component } from 'react'

export default class Main extends Component {
  render () {
    return (
      <div className='container'>
        <nav className='navbar navbar-default navbar-fixed-top' style={{minHeight: '75px'}}>
          <div className='container-fluid'>
            <div className='navbar-header'>
              <a className='navbar-brand' to='/'>
                <img style={{maxWidth: '55px'}} src='../brand.png' alt='Logo PDVSeven'/>
              </a>
            </div>
          </div>
        </nav>
        <div className='content'>
            {this.props.children}
        </div>
      </div>
    )
  }
}

Main.propTypes = {
  children: React.PropTypes.element
}
