import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface AnonymousSurveyState {
  isAuthorized: boolean
  token: string
  password: string
}

export class AnonymousSurveyAuthorized {
  public token: string = ''
  public password: string = ''
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'anonymous-survey',
})
export default class extends VuexModule implements AnonymousSurveyState {
  isAuthorized = false
  token = ''
  password = ''

  // 認証情報のセット
  @Mutation
  setAuthorized(authorized: AnonymousSurveyAuthorized) {
    this.isAuthorized = true
    this.token = authorized.token
    this.password = authorized.password
  }

  // 認証情報のリセット
  @Mutation
  resetAuthorized() {
    this.isAuthorized = false
    this.token = ''
    this.password = ''
  }
}
