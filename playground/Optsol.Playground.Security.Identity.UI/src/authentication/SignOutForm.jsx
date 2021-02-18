import React from 'react'
import { Field, reduxForm } from 'redux-form/immutable'
import * as S from './styles'

let SignOutForm = props => {
  const { handleSubmit } = props
  return (
    <S.BaseContainer>
      <form onSubmit={handleSubmit}>
        <p>Tem certeza que deseja se desconectar?</p>
        
        <Button
          fullWidth
          variant="contained"
          type="submit"
          color="primary"
          size="large"
          style={{ padding: '16px 20px' }}
        >
          DESCONECTAR
        </Button>
      </form>
    </S.BaseContainer>
  )
}

SignOutForm = reduxForm({
  form: 'sign-out',
})(SignOutForm)

export default SignOutForm
