import { routerReducer } from 'react-router-redux'
import { combineReducers } from 'redux-immutable'
import { reducer as formReducer } from 'redux-form'
import signIn from './sign-in'
import signOut from './sign-out'
import signOutContext from './sign-out-context'
import signedOutContext from './signed-out-context'

export default combineReducers({
  routing: routerReducer,
  form: formReducer,
  signIn,
  signOut,
  signOutContext,
  signedOutContext,
})
