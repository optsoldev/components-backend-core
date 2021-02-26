import apiService from './api-service'
import queryString from 'query-string'

class AuthenticationService {
  signIn(username, password, rememberLogin, returnUrl) {
    return apiService.post('/api/sign-in', {
      username,
      password,
      rememberLogin,
      returnUrl,
    })
  }

  getSignOutContext(signOutId) {
    const qs = queryString.stringify({ signOutId })
    return apiService.get(`/api/sign-out-context?${qs}`)
  }

  getSignedOutContext(signOutId) {
    const qs = queryString.stringify({ signOutId })
    return apiService.get(`/api/signed-out-context?${qs}`)
  }

  signOut(signOutId) {
    return apiService.post(`/api/sign-out`, {
      signOutId,
    })
  }
}

export default new AuthenticationService()
