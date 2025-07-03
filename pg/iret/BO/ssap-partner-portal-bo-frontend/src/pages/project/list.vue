<template>
  <TemplateProjectList
    :response="getProjectsResponse"
    :suggest-customers="suggestCustomersResponse"
    :suggest-sales-users="suggestSalesUsersResponse"
    :service-types="serviceTypesResponse"
    :supporter-organizations="supporterOrganizationsResponse"
    :is-loading="isLoading"
    @getProjects="getProjects"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectList, {
  isLoading,
} from '~/components/project/templates/ProjectList.vue'
import {
  GetProjectsRequest,
  GetProjectsResponse,
  GetProjects,
} from '~/models/Project'
import { SuggestCustomersResponse, SuggestCustomers } from '~/models/Customer'
import {
  SuggestUser,
  SuggestUsers,
  SuggestUsersRequest,
  SuggestUsersResponse,
} from '~/models/User'
import {
  GetServiceTypes,
  GetServiceTypesResponse,
  GetSupporterOrganizations,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

export default BasePage.extend({
  name: 'ProjectList',
  components: {
    TemplateProjectList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('project.pages.index.name') as string,
    }
  },
  data(): {
    getProjectsResponse: GetProjectsResponse
    suggestCustomersResponse: SuggestCustomersResponse
    suggestSalesUsersResponse: SuggestUsersResponse
    serviceTypesResponse: GetServiceTypesResponse
    supporterOrganizationsResponse: GetSupporterOrganizationsResponse
    isLoading: isLoading
  } {
    return {
      getProjectsResponse: new GetProjectsResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
      suggestSalesUsersResponse: new SuggestUsersResponse(),
      serviceTypesResponse: new GetServiceTypesResponse(),
      supporterOrganizationsResponse: new GetSupporterOrganizationsResponse(),
      isLoading: {
        projects: true,
        suggestCustomers: true,
        suggestSalesUsers: true,
        serviceTypes: true,
        supporterOrganizations: true,
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.getProjects(),
      this.suggestCustomers(),
      this.suggestSalesUsers(),
      this.getSupporterOrganizations(),
      this.getServiceTypes(),
    ])
  },
  methods: {
    /**
     * GetProjectsAPIを叩き、案件の検索・一覧を取得
     * @param GetProjectsAPIのリクエストパラメータ
     */
    async getProjects(params: GetProjectsRequest = new GetProjectsRequest()) {
      this.isLoading.projects = true

      await GetProjects(params).then((res) => {
        this.getProjectsResponse = res.data
        this.isLoading.projects = false
      })
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
     * SuggestUsersAPIを叩き、営業ユーザーのサジェスト用情報を取得
     * @params SuggestUsersAPIのリクエストパラメータ
     */
    async suggestSalesUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestSalesUsers = true

      // 営業担当者のサジェストを取得
      params.role = 'sales'
      let salesUsers: SuggestUser[] = []

      await SuggestUsers(params)
        .then((res) => {
          salesUsers = res.data
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })

      // 営業責任者のサジェストを取得
      params.role = 'sales_mgr'
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
     * GetServiceTypesAPIを叩き、サービス種別の一覧を取得
     */
    async getServiceTypes() {
      this.isLoading.serviceTypes = true

      await GetServiceTypes().then((res) => {
        this.serviceTypesResponse = res.data
        this.isLoading.serviceTypes = false
      })
    },
    /**
     * getSupporterOrganizationsAPIを叩き、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      this.isLoading.supporterOrganizations = true

      await GetSupporterOrganizations().then((res) => {
        this.supporterOrganizationsResponse = res.data
        this.isLoading.supporterOrganizations = false
      })
    },
  },
})
</script>
