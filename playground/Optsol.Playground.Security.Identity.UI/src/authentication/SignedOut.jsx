import React, { Component } from 'react'
import queryString from 'query-string'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { checkSignedOutContext } from '~/redux/modules/signed-out-context'
import toJS from '~/to-js'
import * as S from './styles'

const ClientRedirect = ({ name, uri }) => (
  <p>
    Retornar para <a href={uri}>{name}</a>.
  </p>
)

class SignedOut extends Component {
  componentDidMount() {
    const qs = queryString.parse(window.location.search)
    this.props.checkSignedOutContext(qs.logoutId)
  }

  render() {
    if (this.props.isLoading) {
      return (
        <S.BaseContainer>
          <p>Processando...</p>
        </S.BaseContainer>
      )
    }

    if (this.props.error) {
      return (
        <S.BaseContainer>
          <p>Ocorreu um erro.</p>
        </S.BaseContainer>
      )
    }

    if (!this.props.data) {
      return <div />
    }

    const { data } = this.props

    const clientName = data.clientName || 'client'
    const redirectToClient = data.postLogoutRedirectUri ? (
      <ClientRedirect uri={data.postLogoutRedirectUri} name={clientName} />
    ) : null

    return (
      <S.BaseContainer>
        <p>Você foi desconectado.</p>
        {redirectToClient}
      </S.BaseContainer>
    )
  }
}

SignedOut.propTypes = {
  checkSignedOutContext: PropTypes.func.isRequired,
  isLoading: PropTypes.bool.isRequired,
  data: PropTypes.object,
  error: PropTypes.object,
}

const mapStateToProps = state => ({
  isLoading: state.get('signedOutContext').get('isLoading'),
  data: state.get('signedOutContext').get('data'),
  error: state.get('signedOutContext').get('error'),
})

const mapDispatchToProps = dispatch =>
  bindActionCreators({ checkSignedOutContext }, dispatch)

export default connect(mapStateToProps, mapDispatchToProps)(toJS(SignedOut))
