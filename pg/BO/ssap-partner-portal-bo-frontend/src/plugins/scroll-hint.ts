import ScrollHint from 'scroll-hint'
import Vue from 'vue'
import 'scroll-hint/css/scroll-hint.css'

Vue.prototype.$setScrollHint = (message: string = ''): void => {
  /* eslint-disable no-new */
  const target = '.is-scroll .v-data-table__wrapper'
  const option = {
    suggestiveShadow: true,
    i18n: { scrollable: message },
  }
  setTimeout(() => {
    new ScrollHint(target, option)
  }, 10)
}
