import Raven from 'raven-js'

const sentryErrorReporter = store => next => action => {
  try {
    return next(action)
  } catch (err) {
    Raven.captureException(err, {
      extra: {
        action,
        state: store.getState()
      }
    })
    throw err
  }
}

export default sentryErrorReporter