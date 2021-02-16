import { handleActions } from 'redux-actions'
import { handle } from 'redux-pack'
import { fromJS } from 'immutable'
import authenticationService from '~/services/authentication-service'

const SIGN_IN_START = 'identity/sign-in/START'

const initialState = fromJS({
  isLoading: false,
  data: null,
  error: null,
})

const reducer = handleActions(
  {
    [SIGN_IN_START]: (state, action) =>
      handle(state, action, {
        start: state =>
          state
            .set('isLoading', true)
            .set('data', initialState.get('data'))
            .set('error', initialState.get('error')),
        finish: state => state.set('isLoading', false),
        success: state =>
          state
            .set('data', fromJS(action.payload.data))
            .set('error', initialState.get('error')),
        failure: state =>
          state
            .set('data', initialState.get('data'))
            .set('error', fromJS(action.payload)),
      }),
  },
  initialState
)

export default reducer

export const signIn = (username, password, rememberLogin, redirectUrl) => ({
  type: SIGN_IN_START,
  promise: authenticationService.signIn(
    username,
    password,
    rememberLogin,
    redirectUrl
  ),
})
