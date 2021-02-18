import React, { Component } from 'react'
import { Iterable } from 'immutable'

const toJS = WrappedComponent => {
  return class ImmutableWrapper extends Component {
    constructor(props) {
      super(props)

      this.updateNewProps = this.updateNewProps.bind(this)
      this.newProps = this.updateNewProps(this.props)
    }

    updateNewProps(currentProps) {
      const entries = x =>
        Object.keys(x).reduce((y, z) => y.push([z, x[z]]) && y, [])

      const objectEntries = entries(currentProps)

      return objectEntries.reduce((newProps, entry) => {
        newProps[entry[0]] = Iterable.isIterable(entry[1])
          ? entry[1].toJS()
          : entry[1]
        return newProps
      }, {})
    }

    componentWillReceiveProps(nextProps) {
      this.newProps = this.updateNewProps(nextProps)
    }

    render() {
      return <WrappedComponent {...this.newProps} />
    }
  }
}

export default toJS
