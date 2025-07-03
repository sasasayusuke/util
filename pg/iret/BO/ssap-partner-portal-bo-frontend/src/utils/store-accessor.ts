/* eslint-disable import/no-mutable-exports */
import { Store } from 'vuex'
import { getModule } from 'vuex-module-decorators'

import me from '~/store/me'
import errorBar from '~/store/error-bar'
import currentPageData from '~/store/current-page-data'
import redirect from '~/store/redirect'
import fullScreenLoading from '~/store/full-screen-loading'
import masterKarteListUrl from '~/store/master-karte-list-url'

let meStore: me
let errorBarStore: errorBar
let currentPageDataStore: currentPageData
let redirectStore: redirect
let fullScreenLoadingStore: fullScreenLoading
let masterKarteListUrlStore: masterKarteListUrl

// Initializer store
function initializeStores(store: Store<any>): void {
  meStore = getModule(me, store)
  errorBarStore = getModule(errorBar, store)
  currentPageDataStore = getModule(currentPageData, store)
  redirectStore = getModule(redirect, store)
  fullScreenLoadingStore = getModule(fullScreenLoading, store)
  masterKarteListUrlStore = getModule(masterKarteListUrl, store)
}

// Module export
export {
  initializeStores,
  meStore,
  errorBarStore,
  currentPageDataStore,
  redirectStore,
  fullScreenLoadingStore,
  masterKarteListUrlStore,
}
