import React from 'react'
import { withRouter } from 'react-router'

export default function necessitaAutenticacao (Component) {
  class ComponentAutenticado extends React.Component {
    componentWillMount () {
      this.checkAuth(this.props.isAuthenticated)
    }

    componentWillReceiveProps (nextProps) {
      this.checkAuth(nextProps.isAuthenticated)
    }

    checkAuth (isAuthenticated) {
      if (!isAuthenticated) {
        let redirectAfterLogin = this.props.location.pathname
        this.props.router.replace(`login?next=${redirectAfterLogin}`)
      }
    }

    render () {
      return (
        <div>
          {this.props.isAuthenticated === true
          ? <Component {...this.props} />
          : null}
        </div>
      )
    }
  }

  ComponentAutenticado.propTypes = {
    isAuthenticated: React.PropTypes.bool.isRequired,
    router: React.PropTypes.object.isRequired,
    location: React.PropTypes.object.isRequired
  }

  return withRouter(ComponentAutenticado)
}
