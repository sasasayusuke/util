import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface MasterKarteListUrlState {
  params: string
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'master-karte-list-url',
})
export default class extends VuexModule implements MasterKarteListUrlState {
  params = ''

  // パラメーターをセット
  @Mutation
  setParams(newParams: string) {
    this.params = newParams
  }

  // パラメーターをクリア
  @Mutation
  clear() {
    this.params = ''
  }
}
