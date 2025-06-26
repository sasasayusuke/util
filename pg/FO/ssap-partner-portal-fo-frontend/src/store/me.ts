// ログイン中の管理ユーザーの情報を保持するストア
// this.$store.commit('me/setResponse')でセット、this.$store.state.meで取得可能

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'
import { ISupporterOrganization } from '~/types/User'

interface MeState {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  customerName: string
  job: string
  company: string
  solverCorporationId: string
  supporterOrganizations: Object[]
  organizationName: string
  isInputManHour: boolean
  projectIds: String[]
  agreed: boolean
  lastLoginAt: string
  disabled: boolean
  totalNotifications: number
  showNotifications: boolean
}

// TODO: sampleに関する記述を削除する
const sampleMe = {
  id: '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
  name: '山田太郎',
  email: 'yamada@example.com',
  role: 'customer',
  customerId: '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
  customerName: '○○株式会社',
  job: '部長',
  company: 'ソニーグループ株式会社',
  solverCorporationId: '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
  supporterOrganizations: [
    {
      id: '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
      name: 'IST',
    },
  ],
  organizationName: '管理部',
  isInputManHour: true,
  projectIds: ['89cbe2ed-f44c-4a1c-9408-c67b0ca2270d'],
  agreed: true,
  lastLoginAt: '2020-10-23T03:21:39.356872Z',
  disabled: false,
  totalNotifications: 10,
  showNotifications: false,
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'me',
})
export default class extends VuexModule implements MeState {
  id = ''
  name = ''
  email = ''
  role = ''
  customerId = ''
  customerName = ''
  job = ''
  company = ''
  solverCorporationId = ''
  supporterOrganizations: ISupporterOrganization[] = []
  organizationName = ''
  isInputManHour = false
  projectIds: String[] = []
  agreed = false
  lastLoginAt = ''
  disabled = false
  totalNotifications = 0
  showNotifications = false

  @Mutation
  setResponse(response = sampleMe) {
    this.id = response.id
    this.name = response.name
    this.email = response.email
    this.role = response.role
    this.customerId = response.customerId
    this.customerName = response.customerName
    this.job = response.job
    this.company = response.company
    this.solverCorporationId = response.solverCorporationId
    this.supporterOrganizations = response.supporterOrganizations
    this.organizationName = response.organizationName
    this.isInputManHour = response.isInputManHour
    this.projectIds = response.projectIds
    this.agreed = response.agreed
    this.lastLoginAt = response.lastLoginAt
    this.disabled = response.disabled
    this.totalNotifications = response.totalNotifications
    this.showNotifications = response.showNotifications
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
    this.role = ''
    this.customerId = ''
    this.customerName = ''
    this.job = ''
    this.company = ''
    this.solverCorporationId = ''
    this.supporterOrganizations = []
    this.organizationName = ''
    this.isInputManHour = false
    this.projectIds = []
    this.agreed = false
    this.lastLoginAt = ''
    this.disabled = false
    this.totalNotifications = 0
    this.showNotifications = false
  }
}
