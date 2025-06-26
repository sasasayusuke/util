<template>
  <TemplateKarteList
    :response="project"
    :project="project"
    :man-hour-status="manHourStatus"
    :batch-control="batchControl"
    :is-loading="isLoading"
    :is-customer="isCustomer"
    :project-support-schedules="getSupportSchedulesByIdResponse"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import { format as dateFnsFormat, parse as dateFnsParse } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateKarteList, {
  isLoading,
} from '~/components/karte/templates/KarteList.vue'

import { GetProjectByIdResponse, GetProjectById } from '~/models/Project'

import {
  GetSummaryProjectSupporterManHourStatusResponse,
  GetSummaryProjectSupporterManHourStatus,
} from '~/models/ManHour'
import { IGetSummaryProjectSupporterManHourStatusRequest } from '@/types/ManHour'

import {
  GetBatchControlByIdResponse,
  GetBatchControlById,
} from '~/models/Master'

import {
  GetSupportSchedulesById,
  GetSupportSchedulesByIdResponse,
} from '~/models/Schedule'
import { meStore, currentPageDataStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { GetNpfProjectId } from '~/models/MasterKarte'

export default BasePage.extend({
  name: 'KarteList',
  components: {
    TemplateKarteList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('karte.pages.index.name') as string,
    }
  },
  data(): {
    project: GetProjectByIdResponse
    manHourStatus: GetSummaryProjectSupporterManHourStatusResponse
    batchControl: GetBatchControlByIdResponse
    isLoading: isLoading
    batchControlId: string
    getSupportSchedulesByIdResponse: GetSupportSchedulesByIdResponse
  } {
    return {
      project: new GetProjectByIdResponse(),
      manHourStatus: new GetSummaryProjectSupporterManHourStatusResponse(),
      batchControl: new GetBatchControlByIdResponse(),
      isLoading: {
        project: true,
        manHourStatus: true,
        batchControl: true,
        supportSchedules: true,
      },
      batchControlId: encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      ),
      getSupportSchedulesByIdResponse: new GetSupportSchedulesByIdResponse(),
    }
  },
  mounted() {
    this.displayLoading([
      this.getProjectById(() => {
        this.getSummaryProjectSupporterManHourStatus()
        this.getBatchControlSection()
        this.getSupportSchedulesById()
      }),
    ])
  },
  computed: {
    /**
     * 顧客ロールか否か
     * @returns 顧客ロールか否か
     */
    isCustomer() {
      return meStore.role === ENUM_USER_ROLE.CUSTOMER
    },
  },
  methods: {
    /**
     * IDからProject情報を取得
     */
    async getProjectById(_callback: any) {
      const callback: any = _callback || function () {}
      this.isLoading.project = true
      const id: string = this.$route.params.projectId as string
      await GetProjectById(id)
        .then((res) => {
          this.project = res.data
          if (!this.project.updateUserName) {
            this.project.updateUserName = ''
          }
          this.isLoading.project = false
          currentPageDataStore.setValues({
            projectId: this.project.id,
            projectName: this.project.name,
            customerName: this.project.customerName,
          })
          callback()
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
      await GetNpfProjectId(id).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
    },
    /**
     * 案件別の支援工数状況を取得
     */
    async getSummaryProjectSupporterManHourStatus(
      params: IGetSummaryProjectSupporterManHourStatusRequest = {
        summaryMonth: 0,
        supporterUserId: '',
      }
    ) {
      this.isLoading.manHourStatus = true
      const id: string = this.$route.params.projectId as string
      const dateYmd7 = this.$t('common.format.date_ymd7') as string
      const dateYm3 = this.$t('common.format.date_ym3') as string
      const dateYmd5 = this.$t('common.format.date_ymd5') as string

      const thisDate = parseInt(dateFnsFormat(getCurrentDate(), dateYmd7))
      const thisMonth = parseInt(dateFnsFormat(getCurrentDate(), dateYm3))
      const supportDateTo = parseInt(
        dateFnsFormat(
          dateFnsParse(this.project.supportDateTo, dateYmd5, getCurrentDate()),
          dateYmd7
        )
      )
      const supportMonthTo = parseInt(
        dateFnsFormat(
          dateFnsParse(this.project.supportDateTo, dateYmd5, getCurrentDate()),
          dateYm3
        )
      )
      params.summaryMonth =
        thisDate > supportDateTo ? supportMonthTo : thisMonth
      params.supporterUserId = meStore.id
      await GetSummaryProjectSupporterManHourStatus(id, params).then((res) => {
        this.manHourStatus = res.data
        this.isLoading.manHourStatus = false
      })
    },
    /**
     * 各種集計バッチ処理の最終完了日時を取得
     */
    async getBatchControlSection() {
      this.isLoading.batchControl = true
      await GetBatchControlById(this.batchControlId).then((res) => {
        this.batchControl = res.data
        this.isLoading.batchControl = false
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
        this.getProjectById(() => {
          this.getSummaryProjectSupporterManHourStatus()
          this.getBatchControlSection()
          this.getSupportSchedulesById()
        }),
      ])
    },
  },
})
</script>
