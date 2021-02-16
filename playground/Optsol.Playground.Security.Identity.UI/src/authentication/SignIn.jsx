import React, { Component } from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import SignInForm from './SignInForm'
import { signIn } from '~/redux/modules/sign-in'
import queryString from 'query-string'
import toJS from '~/to-js'
import * as S from './styles'

class SignIn extends Component {
  signIn = form => {
    const qs = queryString.parse(window.location.search)
    this.props.signIn(
      form.username,
      form.password,
      form.rememberMe,
      qs.returnUrl
    )
  }

  render() {
    const { isLoading, error, data } = this.props

    if (isLoading) {
      return (
        <S.BaseContainer>
          <p>Autenticando...</p>
        </S.BaseContainer>
      )
    }

    if (error) {
      return (
        <S.BaseContainer>
          <p>Ocorreu um erro ao autenticar.</p>
        </S.BaseContainer>
      )
    }

    if (data) {
      window.location.href = data.uri
      return <div />
    }

    return <SignInForm onSubmit={this.signIn} />
  }
}

SignIn.propTypes = {
  signIn: PropTypes.func.isRequired,
  isLoading: PropTypes.bool.isRequired,
  error: PropTypes.object,
  data: PropTypes.object,
}

const mapStateToProps = state => ({
  isLoading: state.get('signIn').get('isLoading'),
  error: state.get('signIn').get('error'),
  data: state.get('signIn').get('data'),
})

const mapDispatchToProps = dispatch => bindActionCreators({ signIn }, dispatch)

export default connect(mapStateToProps, mapDispatchToProps)(toJS(SignIn))
