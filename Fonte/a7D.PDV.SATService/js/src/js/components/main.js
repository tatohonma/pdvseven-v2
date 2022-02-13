import React, { Component } from 'react'
import { Link } from 'react-router'

export default class Main extends Component {
  render () {
    return (
      <div>
        <nav className='navbar navbar-inverse'>
          <div className='container-fluid'>
            <div className='navbar-header'>
              <button className='navbar-toggle collapsed' type='button' data-toggle='collapse' data-target='#collapse' aria-expanded='false'>
                <span className='icon-bar'></span>
                <span className='icon-bar'></span>
                <span className='icon-bar'></span>
              </button>
              <Link to='/' className='navbar-brand'>SAT Service</Link>
            </div>
            <div className='collapse navbar-collapse' id='collapse'>
              <ul className='nav navbar-nav'>
                <li>
                  <Link to='/status'>Status</Link>
                </li>
                <li>
                  <Link to='/log'>Log</Link>
                </li>
              </ul>
            </div>
          </div>
        </nav>
        {React.cloneElement(this.props.children, { ...this.props, key: undefined, ref: undefined })}
      </div>
    )
  }
}
