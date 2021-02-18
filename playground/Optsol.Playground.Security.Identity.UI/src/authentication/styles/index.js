import { Field } from 'redux-form'
import styled from 'styled-components'
import { Colors } from '../../shared/colors'

export const BaseContainer = styled.div`
  background-color: ${Colors.white};
  border-radius: 20px;
  width: 460px;
  min-height: 200px;
  padding: 60px 50px;
  display: flex;
  flex-direction: column;
  text-align: center;
  align-items: center;
  justify-content: center;

  & > h2 {
    margin-bottom: 20px;
    text-align: center;
  }
`
export const SignInContainer = styled(BaseContainer)`
  & .field {
    margin-bottom: 12px;
  }
`
