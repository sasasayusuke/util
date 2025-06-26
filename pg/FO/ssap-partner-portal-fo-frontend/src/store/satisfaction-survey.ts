import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface SatisfactionSurveyState {
  isAuthorized: boolean
  token: string
}

export class SatisfactionSurveyAuthorized {
  public token: string = ''
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'satisfaction-survey',
})
export default class extends VuexModule implements SatisfactionSurveyState {
  isAuthorized = false
  token = ''

  // 認証情報のセット
  @Mutation
  setAuthorized(authorized: SatisfactionSurveyAuthorized) {
    this.isAuthorized = true
    this.token = authorized.token
  }

  // 認証情報のリセット
  @Mutation
  resetAuthorized() {
    this.isAuthorized = false
    this.token = ''
  }
}
