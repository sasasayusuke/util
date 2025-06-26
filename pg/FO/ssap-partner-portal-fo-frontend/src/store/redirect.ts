import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface redirectState {
  url: string
  bufferUrl: string
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'redirect',
})
export default class extends VuexModule implements redirectState {
  url = ''
  bufferUrl = ''

  // 実際のリダイレクトURLを保持
  @Mutation
  setUrl(url: string) {
    this.url = url
  }

  // 一時保存のリダイレクトURLを保持
  @Mutation
  setBufferUrl(url: string) {
    this.bufferUrl = url
  }

  // 実際のリダイレクトURLをクリア
  @Mutation
  clearUrl() {
    this.url = ''
  }

  // 一時保存のリダイレクトURLをクリア
  @Mutation
  clearBufferUrl() {
    this.bufferUrl = ''
  }
}
