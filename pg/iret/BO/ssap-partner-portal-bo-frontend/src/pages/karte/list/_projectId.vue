<template>
  <TemplateKarteList
    :response="project"
    :is-loading="isLoading"
    :project-support-schedules="getSupportSchedulesByIdResponse"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateKarteList, {
  isLoading,
} from '~/components/karte/templates/KarteList.vue'
import { GetProjectByIdResponse, GetProjectById } from '~/models/Project'
import { currentPageDataStore } from '~/store'
import { GetNpfProjectId } from '~/models/MasterKarte'
import {
  GetSupportSchedulesById,
  GetSupportSchedulesByIdResponse,
} from '~/models/Schedule'

export default BasePage.extend({
  name: 'ProjectKarteList',
  components: {
    TemplateKarteList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('karte.pages.list.name') as string,
    }
  },
  data(): {
    project: GetProjectByIdResponse
    isLoading: isLoading
    getSupportSchedulesByIdResponse: GetSupportSchedulesByIdResponse
  } {
    return {
      project: new GetProjectByIdResponse(),
      isLoading: {
        project: true,
        supportSchedules: true,
      },
      getSupportSchedulesByIdResponse: new GetSupportSchedulesByIdResponse(),
    }
  },
  mounted() {
    this.displayLoading([this.getProjectById(), this.getSupportSchedulesById()])
  },
  methods: {
    /**
     * GetProjectByIdを叩き、案件をprojectIdで一意に取得。ストアのcurrentPageDataStoreにデータを格納
     */
    async getProjectById() {
      this.isLoading.project = true
      const id: string = this.$route.params.projectId as string
      await GetProjectById(id).then((res) => {
        this.project = res.data
        this.isLoading.project = false

        currentPageDataStore.setValues({
          projectName: this.project.name,
          customerName: this.project.customerName,
        })
        currentPageDataStore.setPpProjectId(this.project.id)
      })
      await GetNpfProjectId(id).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
    },
    /**
     * GetSupportSchedulesByIdAPIを叩き、案件区分が「支援」の案件スケジュールを取得
     */
    async getSupportSchedulesById() {
      this.isLoading.supportSchedules = true
      const id: string = this.$route.params.projectId

      await GetSupportSchedulesById(id).then((res) => {
        this.getSupportSchedulesByIdResponse = res.data
        this.isLoading.supportSchedules = false
      })
    },
    /**
     * GetSupportSchedulesByIdAPIを叩く
     */
    refresh() {
      this.displayLoading([
        this.getProjectById(),
        this.getSupportSchedulesById(),
      ])
    },
  },
})
</script>
