// ログイン中の管理ユーザーの情報を保持するストア
// this.$store.commit('me/setResponse')でセット、this.$store.state.meで取得可能

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'
import { SupporterOrganization, GetAdminByMineResponse } from '@/models/Admin'

interface MeState {
  id: string
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizations: SupporterOrganization[]
  organizationName: string
  disabled: boolean
  totalNotifications: number
  showNotifications: boolean
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'me',
})
export default class extends VuexModule implements MeState {
  // OTPデータ
  otp = {
    user_id: '',
    otp_verified_token: '',
  }

  id = ''
  name = ''
  email = ''
  roles: string[] = []
  company = ''
  job = ''
  supporterOrganizations: SupporterOrganization[] = []
  organizationName = ''
  disabled = false
  totalNotifications = 0
  showNotifications = false

  @Mutation
  setResponse(response: GetAdminByMineResponse) {
    this.id = response.id
    this.name = response.name
    this.email = response.email
    this.roles = response.roles
    this.company = response.company
    this.job = response.job
    this.supporterOrganizations = response.supporterOrganizations
    this.organizationName = response.organizationName
    this.disabled = response.disabled
    this.totalNotifications = response.totalNotifications
    this.showNotifications = response.showNotifications
  }

  /**
   * OTPを登録
   * TODO: OTPのresponse型を定義する
   * @param otp ワンタイムパスワード
   */
  @Mutation
  addOTP(otp: any) {
    this.otp = otp
  }

  @Mutation
  toggleNotifications(state: boolean) {
    this.showNotifications = state
  }

  @Mutation
  clear() {
    this.id = ''
    this.name = ''
    this.email = ''
    this.roles = []
    this.company = ''
    this.job = ''
    this.supporterOrganizations = []
    this.organizationName = ''
    this.disabled = false
    this.totalNotifications = 0
    this.showNotifications = false
  }
}
