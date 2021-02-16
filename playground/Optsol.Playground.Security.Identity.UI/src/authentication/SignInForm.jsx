import { Button, TextField } from '@material-ui/core'
import React from 'react'
import { Field, reduxForm } from 'redux-form/immutable'
import * as S from './styles'

let SignInForm = props => {
  const { handleSubmit } = props
  return (
    <S.SignInContainer>
      <h2>Login</h2>

      <form onSubmit={handleSubmit}>
        <Field
          name="username"
          component={() => (
            <TextField
              className="field"
              name="username"
              label="Usuário"
              fullWidth
              variant="outlined"
            />
          )}
          type="text"
        />

        <Field
          name="password"
          component={() => (
            <TextField
              className="field"
              name="password"
              type="password"
              label="Senha"
              fullWidth
              variant="outlined"
            />
          )}
          type="text"
        />

        {/* <div>
          <Field name="rememberMe" component="input" type="checkbox" />
          <label htmlFor="rememberMe">Remember Me?</label>
        </div> */}

        <Button
          fullWidth
          variant="contained"
          type="submit"
          color="primary"
          size="large"
          style={{ padding: '16px 20px' }}
        >
          Entrar
        </Button>
      </form>
    </S.SignInContainer>
  )
}

SignInForm = reduxForm({
  form: 'sign-in',
})(SignInForm)

export default SignInForm
