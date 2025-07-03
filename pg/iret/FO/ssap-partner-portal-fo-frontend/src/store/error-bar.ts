import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface errorBarState {
  message: string
  checked: boolean
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'error-bar',
})
export default class extends VuexModule implements errorBarState {
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
