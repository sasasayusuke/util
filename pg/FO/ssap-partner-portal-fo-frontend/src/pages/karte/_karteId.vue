<template>
  <TemplateKarteDetail
    v-if="!isLoading.project && !isLoading.karte && !isLoading.surveys"
    :project="project"
    :karte="karte"
    :surveys="getSurveysResponse.surveys"
    :is-customer="isCustomer"
    :is-sales="isSales"
    :is-sales-mgr="isSalesMgr"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateKarteDetail, {
  isLoading,
} from '~/components/karte/templates/KarteDetail.vue'
import { GetProjectByIdResponse, GetProjectById } from '~/models/Project'
import {
  GetKarteByIdResponse,
  GetKarteById,
  Location as KarteLocation,
} from '~/models/Karte'
import { GetSurveysResponse, GetSurveys } from '~/models/Survey'
import { IGetSurveysRequest } from '@/types/Survey'
import { ENUM_USER_ROLE } from '~/types/User'
import { meStore, currentPageDataStore } from '~/store'

export default BasePage.extend({
  name: 'KarteDetail',
  components: {
    TemplateKarteDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('karte.pages.detail.name') as string,
    }
  },
  data(): {
    project: GetProjectByIdResponse
    karte: GetKarteByIdResponse
    getSurveysResponse: GetSurveysResponse
    isLoading: isLoading
  } {
    return {
      project: new GetProjectByIdResponse(),
      karte: new GetKarteByIdResponse(),
      getSurveysResponse: new GetSurveysResponse(),
      isLoading: {
        project: true,
        karte: true,
        surveys: true,
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.getKarteById(() => {
        this.getProjectById()
        this.getSurveys()
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
    /**
     * 営業ロールか否か
     * @returns 営業ロールか否か
     */
    isSales() {
      return meStore.role === ENUM_USER_ROLE.SALES
    },
    /**
     * 営業責任者か否か
     * @returns 営業責任者か否か
     */
    isSalesMgr() {
      return meStore.role === ENUM_USER_ROLE.SALES_MGR
    },
  },
  methods: {
    /**
     * IDから案件カルテを取得
     */
    async getKarteById(_callback: any) {
      const callback: any = _callback || function () {}
      this.isLoading.karte = true
      const id: string = this.$route.params.karteId as string

      await GetKarteById(id)
        .then((res) => {
          this.karte = res.data
          if (!this.karte.location) {
            this.karte.location = new KarteLocation()
          }
          this.isLoading.karte = false

          // 支援実績時間（自）の初期表示に支援開始予定時間を設定
          if (this.karte.startSupportActualTime === null) {
            this.karte.startSupportActualTime = this.karte.startTime
          }

          // 支援実績時間（至）の初期表示に支援終了予定時間を設定
          if (this.karte.endSupportActualTime === null) {
            this.karte.endSupportActualTime = this.karte.endTime
          }

          if (this.karte.isDraft === true && this.isCustomer === true) {
            this.$router.push('/404')
          } else {
            callback()
          }
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /**
     * IDからProject情報を取得
     */
    async getProjectById() {
      this.isLoading.project = true
      const id: string = this.karte.projectId as string

      await GetProjectById(id).then((res) => {
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
      })
    },
    /**
     * 案件アンケートの一覧を取得
     */
    async getSurveys(params: IGetSurveysRequest = {}) {
      this.isLoading.surveys = true
      params.projectId = this.karte.projectId as string
      await GetSurveys(params).then((res) => {
        this.getSurveysResponse = res.data
        this.isLoading.surveys = false
      })
    },
  },
})
</script>
