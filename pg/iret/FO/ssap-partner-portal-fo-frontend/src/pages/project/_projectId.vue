<template>
  <TemplateProjectDetail
    :project="getProjectByIdResponse"
    :project-survey-schedules="getSurveySchedulesByIdResponse"
    :project-support-schedules="getSupportSchedulesByIdResponse"
    :suggest-users="suggestUsersResponse"
    :suggest-sales-users="suggestSalesUsersResponse"
    :suggest-customers="suggestCustomersResponse"
    :service-types="getServiceTypesResponse"
    :survey-masters="getSurveyMastersResponse"
    :supporter-organizations="getSupporterOrganizationsResponse"
    :is-loading-items="isLoading"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectDetail, {
  isLoading,
} from '~/components/project/templates/ProjectDetail.vue'
import { SuggestCustomers, SuggestCustomersResponse } from '~/models/Customer'
import {
  GetServiceTypes,
  GetServiceTypesResponse,
  GetSupporterOrganizations,
  GetSupporterOrganizationsResponse,
  GetSurveyMasters,
  GetSurveyMastersResponse,
} from '~/models/Master'
import { GetNpfProjectId } from '~/models/MasterKarte'
import { GetProjectById, GetProjectByIdResponse } from '~/models/Project'
import {
  GetSupportSchedulesById,
  GetSupportSchedulesByIdResponse,
  GetSurveySchedulesById,
  GetSurveySchedulesByIdResponse,
} from '~/models/Schedule'
import {
  SuggestUser,
  SuggestUsers,
  SuggestUsersRequest,
  SuggestUsersResponse,
} from '~/models/User'
import { currentPageDataStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BasePage.extend({
  name: 'ProjectDetail',
  components: {
    TemplateProjectDetail,
  },
  middleware: ['roleCheck'],
  data(): {
    getProjectByIdResponse: GetProjectByIdResponse
    getSurveySchedulesByIdResponse: GetSurveySchedulesByIdResponse
    getSupportSchedulesByIdResponse: GetSupportSchedulesByIdResponse
    suggestUsersResponse: SuggestUsersResponse
    suggestSalesUsersResponse: SuggestUsersResponse
    suggestCustomersResponse: SuggestCustomersResponse
    getServiceTypesResponse: GetServiceTypesResponse
    getSurveyMastersResponse: GetSurveyMastersResponse
    getSupporterOrganizationsResponse: GetSupporterOrganizationsResponse
    isLoading: isLoading
  } {
    return {
      /** getProjectById APIのレスポンス */
      getProjectByIdResponse: new GetProjectByIdResponse(),
      /** getSurveySchedulesById APIのレスポンス */
      getSurveySchedulesByIdResponse: new GetSurveySchedulesByIdResponse(),
      /** getSupportSchedulesById APIのレスポンス */
      getSupportSchedulesByIdResponse: new GetSupportSchedulesByIdResponse(),
      /** suggestUsers APIのレスポンス */
      suggestUsersResponse: new SuggestUsersResponse(),
      /** suggestSalesUsers APIのレスポンス */
      suggestSalesUsersResponse: new SuggestUsersResponse(),
      /** suggestCustomers APIのレスポンス */
      suggestCustomersResponse: new SuggestCustomersResponse(),
      /** getServiceTypes APIのレスポンス */
      getServiceTypesResponse: new GetServiceTypesResponse(),
      /** getSurveyMasters APIのレスポンス */
      getSurveyMastersResponse: new GetSurveyMastersResponse(),
      /** getSupporterOrganizations APIのレスポンス */
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      /** ローディング中かどうか */
      isLoading: {
        project: true,
        surveySchedules: true,
        supportSchedules: true,
        suggestUsers: true,
        suggestSalesUsers: true,
        suggestCustomers: true,
        serviceTypes: true,
        userById: true,
        surveyMaster: true,
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.getProjectById(),
      this.getSurveySchedulesById(),
      this.getSupportSchedulesById(),
      this.suggestUsers(),
      this.suggestSalesUsers(),
      this.suggestCustomers(),
      this.getServiceTypes(),
      this.getSurveyMasters(),
      this.getSupporterOrganizations(),
    ])
  },
  methods: {
    /** getProjectById API: 案件をIDで一意に取得 */
    async getProjectById() {
      this.isLoading.project = true
      const id: string = this.$route.params.projectId

      await GetProjectById(id)
        .then((res) => {
          this.getProjectByIdResponse = res.data
          if (!this.getProjectByIdResponse.customerUsers) {
            this.getProjectByIdResponse.customerUsers = []
          }
          if (!this.getProjectByIdResponse.supporterUsers) {
            this.getProjectByIdResponse.supporterUsers = []
          }
          this.isLoading.project = false
          currentPageDataStore.setValues({
            projectId: this.getProjectByIdResponse.id,
            projectName: this.getProjectByIdResponse.name,
            customerName: this.getProjectByIdResponse.customerName,
          })
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
      await GetNpfProjectId(currentPageDataStore.projectId).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
    },
    /** getSurveySchedulesById API: 案件区分が「アンケート」の案件スケジュールを取得 */
    async getSurveySchedulesById() {
      this.isLoading.surveySchedules = true
      const id: string = this.$route.params.projectId

      await GetSurveySchedulesById(id)
        .then((res) => {
          this.getSurveySchedulesByIdResponse = res.data
          this.isLoading.surveySchedules = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** getSupportSchedulesById API: 案件区分が「支援」の案件スケジュールを取得 */
    async getSupportSchedulesById() {
      this.isLoading.supportSchedules = true
      const id: string = this.$route.params.projectId

      await GetSupportSchedulesById(id)
        .then((res) => {
          this.getSupportSchedulesByIdResponse = res.data
          this.isLoading.supportSchedules = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** suggestUsers API: 一般ユーザーのサジェスト用情報を取得 */
    async suggestUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestUsers = true
      params.role = ENUM_USER_ROLE.CUSTOMER
      params.disable = false

      await SuggestUsers(params)
        .then((res) => {
          this.suggestUsersResponse = res.data
          this.isLoading.suggestUsers = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** 営業担当者用のsuggestUsers API: 一般ユーザーのサジェスト用情報を取得 */
    async suggestSalesUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestSalesUsers = true

      //営業担当者のサジェストを取得
      params.role = ENUM_USER_ROLE.SALES
      params.disable = false
      let salesUsers: SuggestUser[] = []

      await SuggestUsers(params)
        .then((res) => {
          salesUsers = res.data
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
      //営業責任者のサジェストを取得
      params.role = ENUM_USER_ROLE.SALES_MGR
      params.disable = false
      let salesManagers: SuggestUser[] = []

      await SuggestUsers(params)
        .then((res) => {
          salesManagers = res.data
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
      this.suggestSalesUsersResponse = salesUsers.concat(salesManagers)
      this.isLoading.suggestSalesUsers = false
    },
    /** suggestCustomers API: 取引先のサジェスト用情報を取得 */
    async suggestCustomers() {
      this.isLoading.suggestCustomers = true

      await SuggestCustomers()
        .then((res) => {
          this.suggestCustomersResponse = res.data
          this.isLoading.suggestCustomers = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** getServiceTypes API: サービス種別の一覧を取得 */
    async getServiceTypes() {
      this.isLoading.serviceTypes = true

      await GetServiceTypes()
        .then((res) => {
          this.getServiceTypesResponse = res.data
          this.isLoading.serviceTypes = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** getSurveyMasters API: 最新バージョンのアンケートひな形の一覧を取得 */
    async getSurveyMasters() {
      this.isLoading.surveyMaster = false

      await GetSurveyMasters()
        .then((res) => {
          this.getSurveyMastersResponse = res.data
          this.isLoading.surveyMaster = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** getSupporterOrganizations API: 支援者組織の一覧を取得 */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations()
        .then((res) => {
          this.getSupporterOrganizationsResponse = res.data
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /** アンケート送信スケジュール画面のモーダル画面修了時にデータを更新 */
    refresh() {
      this.displayLoading([
        this.getProjectById(),
        this.getSurveySchedulesById(),
        this.getSupportSchedulesById(),
      ])
    },
  },
})
</script>
