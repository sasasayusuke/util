// 表示中のページ情報を保持するストア
// this.$store.commit('current-page-data/setValues')でセット、this.$store.state['current-page-data']で取得可能

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface CurrentPageDataState {
  projectName: string
  customerName: string
  npfProjectId?: string
  ppProjectId?: string
}

const sampleCurrentPageData = {
  projectName: '',
  customerName: '',
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'current-page-data',
})
export default class extends VuexModule implements CurrentPageDataState {
  projectName = ''
  customerName = ''
  ppProjectId = ''
  npfProjectId = ''

  @Mutation
  setValues(response = sampleCurrentPageData) {
    this.projectName = response.projectName
    this.customerName = response.customerName
  }

  @Mutation
  setProjectName(value = '') {
    this.projectName = value
  }

  @Mutation
  setCustomerName(value = '') {
    this.customerName = value
  }

  @Mutation
  setPpProjectId(value = '') {
    this.ppProjectId = value
  }

  @Mutation
  setNpfProjectId(value = '') {
    this.npfProjectId = value
  }

  @Mutation
  clear() {
    this.projectName = ''
    this.customerName = ''
    this.ppProjectId = ''
    this.npfProjectId = ''
  }
}
