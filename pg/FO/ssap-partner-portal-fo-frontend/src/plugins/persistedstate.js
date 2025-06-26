import createPersistedState from 'vuex-persistedstate'

export default ({ store, isHMR }) => {
  // In case of HMR, mutation occurs before nuxReady, so previously saved state
  // gets replaced with original state received from server. So, we've to skip HMR.
  // Also nuxtReady event fires for HMR as well, which results multiple registration of
  // vuex-persistedstate plugin
  if (isHMR) return

  createPersistedState({
    key: process.env.VUEX_STORAGE_KEY, // Storage Key
    paths: ['example', 'redirect', 'persisted-error-bar', 'solver-corporation'], // Stores you want to make Persisted
  })(store)
}
