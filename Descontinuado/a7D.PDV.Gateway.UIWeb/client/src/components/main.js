import React, { Component } from 'react'

export default class Main extends Component {
  render () {
    return (
      <div className='container'>
        <nav className='navbar navbar-default navbar-fixed-top'>
          <div className='container-fluid'>
            <div className='navbar-header'>
              <button type='button' className='navbar-toggle collapsed' data-toggle='collapse' data-target='#navbar' aria-expanded='false' aria-controls='navbar'>
                <span className='sr-only'>Toggle navigation</span>
                <span className='icon-bar'></span>
                <span className='icon-bar'></span>
                <span className='icon-bar'></span>
              </button>
              <span className='navbar-brand' to='/'>PDVSeven Pagamentos</span>
            </div>
            <div id='navbar' className='navbar-collapse collapse'>
            </div>
          </div>
        </nav>
        <div className='content'>
            {React.cloneElement(this.props.children, this.props)}
        </div>
      </div>
    )
  }
}

Main.propTypes = {
  children: React.PropTypes.element,
  isAuthenticated: React.PropTypes.bool.isRequired,
  logout: React.PropTypes.func.isRequired
}
