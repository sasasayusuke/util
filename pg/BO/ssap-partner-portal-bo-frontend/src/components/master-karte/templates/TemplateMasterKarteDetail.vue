<template>
  <RootTemPlate style="padding: 0 !important">
    <main class="detail-containers">
      <MasterKarteDetailCurrentTabs
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :project="project"
        :user-info="userInfo"
        @clickTab="onClickTab"
      />
      <MasterKarteDetailContainer
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :is-loading="isLoading.masterKarte"
      />
    </main>
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import PageHeader from '~/components/common/molecules/PageHeader.vue'
import {
  GetMasterKarteById,
  GetMasterKarteByIdResponseClass,
  NextProgramClass,
} from '~/models/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import { GetUsers, GetUsersRequest, UserListItem } from '~/models/User'
import { GetProjectById, GetProjectByIdResponse } from '~/models/Project'

export interface isLoading {
  projects: boolean
  suggestCustomers: boolean
}

export default BaseComponent.extend({
  name: 'MasterKarteList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    PageHeader,
  },
  data() {
    return {
      pageName: '',
      isLoading: {
        project: true,
        karten: true,
        masterKarte: true,
      },
      isCurrentProgram: true,
      masterKarteProject: new GetMasterKarteByIdResponseClass(),
      userInfo: new UserListItem(),
      project: new GetProjectByIdResponse(),
    }
  },
  mounted() {
    this.displayLoading([this.getMasterKarteById(), this.getUsers()])
  },
  methods: {
    onClickTab(isCurrentProgram: boolean) {
      this.isCurrentProgram = isCurrentProgram
    },
    /**
     * GetKartenAPIを叩き、案件カルテの一覧を取得
     * @param GetKartenAPIのリクエストパラメータを指定
     */
    async getMasterKarteById() {
      this.isLoading.project = true
      const npfProjectId = this.$route.params.npfProjectId
      await GetMasterKarteById(npfProjectId)
        .then((res: any) => {
          this.masterKarteProject = res.data
          currentPageDataStore.setValues({
            projectName: this.masterKarteProject.project,
            customerName: this.masterKarteProject.client,
          })
          currentPageDataStore.setPpProjectId(
            this.masterKarteProject.ppProjectId
          )
          currentPageDataStore.setNpfProjectId(npfProjectId)
          if (!this.masterKarteProject.nextProgram) {
            this.masterKarteProject.nextProgram = new NextProgramClass()
            // @ts-ignore
            this.masterKarteProject.nextProgram.fundamentalInformation.customerSuccessReuse =
              null
          }
          this.getProjectById()
          this.isLoading.project = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /**
     * GetProjectByIdを叩き、案件をprojectIdで一意に取得
     */
    async getProjectById() {
      const id: string = currentPageDataStore.ppProjectId
      await GetProjectById(id).then((res) => {
        this.project = res.data
      })
    },
    /**
     * GetUsersを叩き、管理ユーザーと一致する一般ユーザー情報を取得
     */
    async getUsers() {
      const request = new GetUsersRequest()
      request.email = meStore.email
      await GetUsers(request).then((res) => {
        this.userInfo = res.data.users[0]
      })
    },
  },
})
</script>

<style>
.detail-containers {
  margin: 32px 0;
}
</style>
