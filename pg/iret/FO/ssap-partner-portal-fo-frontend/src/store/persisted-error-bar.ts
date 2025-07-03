import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface persistedErrorBarState {
  message: string
  checked: boolean
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'persisted-error-bar',
})
export default class extends VuexModule implements persistedErrorBarState {
  message = ''
  checked = true

  // エラーメッセージのセット(&チェック済みフラグをoffに)
  @Mutation
  setMessage(message: string) {
    this.message = message
    this.checked = false
  }

  // メッセージをチェック済みに
  @Mutation
  setCheckedTrue() {
    this.checked = true
  }

  // メッセージをクリア
  @Mutation
  clear() {
    this.message = ''
    this.checked = true
  }
}
