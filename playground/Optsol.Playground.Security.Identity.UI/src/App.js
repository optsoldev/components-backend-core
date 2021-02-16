import React from 'react'
import { Provider } from 'react-redux'
import { Router, Route } from 'react-router-dom'
import routes from './routes'
import createStore from '~/redux/create'
import history from './history'
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core'
import { Colors } from './shared/colors'
import { Lock } from '@material-ui/icons'
import styled from 'styled-components'

const StyledLock = styled(Lock)`
  font-size: 150vh !important;
  position: absolute;
  left: -25%;
  top: 20px;
  color: ${Colors.white};
  opacity: 0.18;
`

const theme = createMuiTheme({
  palette: {
    primary: {
      main: Colors.primary,
    },
    secondary: {
      main: Colors.secondary,
    },
  },
  overrides: {
    MuiIconButton: {
      root: {
        '&': {
          padding: 8,
        },
      },
    },
  },
})

const store = createStore()

const Routes = () => (
  <div>
    {routes.map((r, i) => (
      <Route
        key={`route-${i}`}
        path={r.path}
        render={props => <r.component {...props} />}
      />
    ))}
  </div>
)

const App = () => {
  return (
    <React.Fragment>
      <StyledLock />
      <MuiThemeProvider theme={theme}>
        <Provider store={store}>
          <Router history={history}>
            <Routes />
          </Router>
        </Provider>
      </MuiThemeProvider>
    </React.Fragment>
  )
}

export default App
