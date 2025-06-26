import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface fullScreenLoadingState {
  isHold: boolean
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'full-screen-loading',
})
export default class extends VuexModule implements fullScreenLoadingState {
  isHold = false

  // 画面を跨いだローディングを有効化
  @Mutation
  setHold() {
    this.isHold = true
  }

  // 画面を跨いだローディングを無効化
  @Mutation
  clearHold() {
    this.isHold = false
  }
}
