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
    :user="user"
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
  GetSurveyMastersRequest,
  GetSurveyMastersResponse,
} from '~/models/Master'
import { GetProjectById, GetProjectByIdResponse } from '~/models/Project'
import {
  GetSupportSchedulesById,
  GetSupportSchedulesByIdResponse,
  GetSurveySchedulesById,
  GetSurveySchedulesByIdResponse,
} from '~/models/Schedule'
import {
  GetUsers,
  GetUsersRequest,
  SuggestUser,
  SuggestUsers,
  SuggestUsersRequest,
  SuggestUsersResponse,
  UserListItem,
} from '~/models/User'
import { meStore } from '~/store'

export default BasePage.extend({
  name: 'ProjectDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateProjectDetail,
  },
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
    user: UserListItem
    isLoading: isLoading
  } {
    return {
      getProjectByIdResponse: new GetProjectByIdResponse(),
      getSurveySchedulesByIdResponse: new GetSurveySchedulesByIdResponse(),
      getSupportSchedulesByIdResponse: new GetSupportSchedulesByIdResponse(),
      suggestUsersResponse: new SuggestUsersResponse(),
      suggestSalesUsersResponse: new SuggestUsersResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
      getServiceTypesResponse: new GetServiceTypesResponse(),
      getSurveyMastersResponse: new GetSurveyMastersResponse(),
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      user: new UserListItem(),
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
      this.getUsers(),
    ])
  },
  methods: {
    /**
     * GetUsersを叩き、ログインユーザーの一般ユーザ情報を取得
     */
    async getUsers() {
      const params = new GetUsersRequest()
      params.email = meStore.email
      await GetUsers(params).then((res) => {
        this.user = res.data.users[0]
      })
    },
    /**
     * GetProjectByIdAPIを叩き、案件をIDで一意に取得
     */
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
          if (
            !this.getProjectByIdResponse.isSurveyEmailToSalesforceMainCustomer
          ) {
            this.getProjectByIdResponse.isSurveyEmailToSalesforceMainCustomer =
              false
          }
          this.isLoading.project = false
        })
        .catch((error) => {
          if (error.response) {
            if (error.response.status === 403) {
              this.$router.push(`/403`)
            } else if (error.response.status === 404) {
              this.$router.push(`/404`)
            } else {
              this.$router.push(`/500`)
            }
          } else {
            this.$router.push(`/500`)
          }
        })
    },
    /**
     * GetSurveySchedulesByIdAPIを叩き、案件区分が「アンケート」の案件スケジュールを取得
     */
    async getSurveySchedulesById() {
      this.isLoading.surveySchedules = true
      const id: string = this.$route.params.projectId

      await GetSurveySchedulesById(id).then((res) => {
        this.getSurveySchedulesByIdResponse = res.data
        this.isLoading.surveySchedules = false
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
     * SuggestUsersAPIを叩き、一般ユーザーのサジェスト用情報を取得
     * @params SuggestUsersAPIのリクエストパラメータ
     */
    async suggestUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestUsers = true
      params.role = 'customer'
      params.disabled = false

      await SuggestUsers(params).then((res) => {
        this.suggestUsersResponse = res.data
        this.isLoading.suggestUsers = false
      })
    },
    /**
     * SuggestUsersAPIを叩き、営業ユーザーのサジェスト用情報を取得
     * @params SuggestUsersAPIのリクエストパラメータ
     */
    async suggestSalesUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestSalesUsers = true

      //営業担当者のサジェストを取得
      params.role = 'sales'
      params.disabled = false
      let salesUsers: SuggestUser[] = []

      await SuggestUsers(params)
        .then((res) => {
          salesUsers = res.data
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
      //営業責任者のサジェストを取得
      params.role = 'sales_mgr'
      params.disabled = false
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
    /**
     * SuggestCustomersAPIを叩き、取引先のサジェスト用情報を取得
     */
    async suggestCustomers() {
      this.isLoading.suggestCustomers = true

      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
        this.isLoading.suggestCustomers = false
      })
    },
    /**
     * GetServiceTypesAPIを叩き、サービス種別の一覧を取得
     */
    async getServiceTypes() {
      this.isLoading.serviceTypes = true

      await GetServiceTypes().then((res) => {
        this.getServiceTypesResponse = res.data
        this.isLoading.serviceTypes = false
      })
    },
    /**
     * GetSurveyMastersAPIを叩き、アンケートマスターの検索・一覧を取得
     */
    async getSurveyMasters() {
      this.isLoading.surveyMaster = false
      const request = new GetSurveyMastersRequest()

      await GetSurveyMasters(request).then((res) => {
        this.getSurveyMastersResponse = res.data
        this.isLoading.surveyMaster = false
      })
    },
    /**
     * GetSupporterOrganizationsAPIを叩き、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations().then((res) => {
        this.getSupporterOrganizationsResponse = res.data
      })
    },
    /**
     * GetProjectById, GetSurveySchedulesById, GetSupportSchedulesByIdAPIを叩く
     */
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
