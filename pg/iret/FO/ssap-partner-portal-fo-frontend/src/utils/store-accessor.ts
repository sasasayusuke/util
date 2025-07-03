/* eslint-disable import/no-mutable-exports */
import { Store } from 'vuex'
import { getModule } from 'vuex-module-decorators'

import me from '~/store/me'
import errorBar from '~/store/error-bar'
import persistedErrorBar from '~/store/persisted-error-bar'
import currentPageData from '~/store/current-page-data'
import redirect from '~/store/redirect'
import fullScreenLoading from '~/store/full-screen-loading'
import anonymousSurvey from '~/store/anonymous-survey'
import satisfactionSurvey from '~/store/satisfaction-survey'
import masterKarteListUrl from '~/store/master-karte-list-url'
import solverCorporation from '~/store/solver-corporation'

let meStore: me
let errorBarStore: errorBar
let persistedErrorBarStore: persistedErrorBar
let currentPageDataStore: currentPageData
let redirectStore: redirect
let fullScreenLoadingStore: fullScreenLoading
let anonymousSurveyStore: anonymousSurvey
let satisfactionSurveyStore: satisfactionSurvey
let masterKarteListUrlStore: masterKarteListUrl
let solverCorporationStore: solverCorporation

// Initializer store
function initializeStores(store: Store<any>): void {
  meStore = getModule(me, store)
  errorBarStore = getModule(errorBar, store)
  persistedErrorBarStore = getModule(persistedErrorBar, store)
  currentPageDataStore = getModule(currentPageData, store)
  redirectStore = getModule(redirect, store)
  fullScreenLoadingStore = getModule(fullScreenLoading, store)
  anonymousSurveyStore = getModule(anonymousSurvey, store)
  satisfactionSurveyStore = getModule(satisfactionSurvey, store)
  masterKarteListUrlStore = getModule(masterKarteListUrl, store)
  solverCorporationStore = getModule(solverCorporation, store)
}

// Module export
export {
  initializeStores,
  meStore,
  errorBarStore,
  persistedErrorBarStore,
  currentPageDataStore,
  redirectStore,
  fullScreenLoadingStore,
  anonymousSurveyStore,
  satisfactionSurveyStore,
  masterKarteListUrlStore,
  solverCorporationStore,
}
