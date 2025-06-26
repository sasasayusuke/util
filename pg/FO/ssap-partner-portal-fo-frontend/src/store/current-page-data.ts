// 表示中のページ情報を保持するストア
// this.$store.commit('current-page-data/setValues')でセット、this.$store.state['current-page-data']で取得可能

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface CurrentPageDataState {
  projectId: string
  projectName: string
  customerName: string
  npfProjectId?: string
}

const sampleCurrentPageData = {
  projectId: '',
  projectName: '',
  customerName: '',
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'current-page-data',
})
export default class extends VuexModule implements CurrentPageDataState {
  projectId = ''
  projectName = ''
  customerName = ''
  npfProjectId = ''

  @Mutation
  setValues(response = sampleCurrentPageData) {
    this.projectId = response.projectId
    this.projectName = response.projectName
    this.customerName = response.customerName
  }

  @Mutation
  setProjectId(value = '') {
    this.projectId = value
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
  setNpfProjectId(value = '') {
    this.npfProjectId = value
  }

  @Mutation
  clear() {
    this.projectId = ''
    this.projectName = ''
    this.customerName = ''
  }
}
