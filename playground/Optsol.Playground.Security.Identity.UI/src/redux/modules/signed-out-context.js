import { handleActions } from 'redux-actions'
import { handle } from 'redux-pack'
import { fromJS } from 'immutable'
import authenticationService from '~/services/authentication-service'
import history from '~/history'

const SIGNED_OUT_CONTEXT_START = 'identity/signed-out-context/START'

const initialState = fromJS({
  isLoading: false,
  data: null,
  error: null,
})

const reducer = handleActions(
  {
    [SIGNED_OUT_CONTEXT_START]: (state, action) =>
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

const checkSignedOutContextImpl = async signOutId => {
  const context = await authenticationService.getSignedOutContext(signOutId)

  if (context.data.automaticRedirectAfterSignOut) {
    history.push({
      pathname: context.data.postLogoutRedirectUri,
    })
  }

  return context
}

export const checkSignedOutContext = signOutId => ({
  type: SIGNED_OUT_CONTEXT_START,
  promise: checkSignedOutContextImpl(signOutId),
})
