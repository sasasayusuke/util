<template>
  <TemplateProjectCreate
    :project="project"
    :users="getUsersResponse"
    :service-types="getServiceTypesResponse"
    :supporter-organizations="getSupporterOrganizationsResponse"
    :suggest-customers="suggestCustomersResponse"
    :suggest-sales-users="suggestSalesUsersResponse"
    :suggest-supporter-users="suggestSupporterUsersResponse"
    :suggest-main-supporter-users="suggestMainSupporterUsersResponse"
    :suggest-users="suggestUsersResponse"
    :is-loading="isLoading"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectCreate, {
  isLoading,
} from '~/components/project/templates/ProjectCreate.vue'
import { SuggestCustomers, SuggestCustomersResponse } from '~/models/Customer'
import {
  GetServiceTypes,
  GetServiceTypesResponse,
  GetSupporterOrganizations,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'
import { GetProjectByIdResponse } from '~/models/Project'
import {
  GetUsers,
  GetUsersResponse,
  SuggestUser,
  SuggestUsers,
  SuggestUsersRequest,
  SuggestUsersResponse,
} from '~/models/User'
import { ENUM_USER_ROLE } from '~/types/User'

export default BasePage.extend({
  name: 'ProjectCreate',
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('project.pages.create.name') as string,
    }
  },
  components: {
    TemplateProjectCreate,
  },
  data(): {
    project: GetProjectByIdResponse
    getUsersResponse: GetUsersResponse
    suggestSalesUsersResponse: SuggestUsersResponse
    suggestCustomersResponse: SuggestCustomersResponse
    suggestUsersResponse: SuggestUsersResponse
    suggestSupporterUsersResponse: SuggestUsersResponse
    suggestMainSupporterUsersResponse: SuggestUsersResponse
    getServiceTypesResponse: GetServiceTypesResponse
    getSupporterOrganizationsResponse: GetSupporterOrganizationsResponse
    isLoading: isLoading
  } {
    return {
      project: new GetProjectByIdResponse(),
      getUsersResponse: new GetUsersResponse(),
      suggestSalesUsersResponse: new SuggestUsersResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
      suggestUsersResponse: new SuggestUsersResponse(),
      suggestSupporterUsersResponse: new SuggestUsersResponse(),
      suggestMainSupporterUsersResponse: new SuggestUsersResponse(),
      getServiceTypesResponse: new GetServiceTypesResponse(),
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      isLoading: {
        suggestUsers: true,
        suggestCustomers: true,
        serviceTypes: true,
        suggestSalesUsers: true,
        supporterOrganization: true,
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.suggestUsers(),
      this.suggestSalesUsers(),
      this.suggestSupporterUsers(),
      this.suggestMainSupporterUsers(),
      this.suggestCustomers(),
      this.getServiceTypes(),
      this.getUsers(),
      this.getSupporterOrganizations(),
    ])
  },
  methods: {
    /**
     * SuggestUsersAPIを叩き、一般ユーザーのサジェスト用情報を取得
     * @params 一般ユーザーのサジェスト用情報を取得APIのリクエストパラメータ
     */
    async suggestUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestUsers = true
      params.role = ENUM_USER_ROLE.CUSTOMER
      params.disabled = false

      await SuggestUsers(params).then((res) => {
        this.suggestUsersResponse = res.data
        this.isLoading.suggestUsers = false
      })
    },
    /**
     * SuggestUsersAPIを叩き、営業ユーザーのサジェスト用情報を取得
     * @params 営業ユーザーのサジェスト用情報を取得APIのリクエストパラメータ
     */
    async suggestSalesUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      this.isLoading.suggestSalesUsers = true

      //営業担当者のサジェストを取得
      params.role = ENUM_USER_ROLE.SALES
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
      params.role = ENUM_USER_ROLE.SALES_MGR
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
     * SuggestUsersAPIを叩き、支援者ユーザーのサジェスト用情報を取得
     * @params 支援者ユーザーのサジェスト用情報を取得APIのリクエストパラメータ
     */
    async suggestSupporterUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      params.role = ENUM_USER_ROLE.SUPPORTER
      params.disabled = false

      await SuggestUsers(params).then((res) => {
        this.suggestSupporterUsersResponse = res.data
      })
    },
    /**
     * SuggestUsersAPIを叩き、支援者責任者ユーザーのサジェスト用情報を取得
     * @params 支援者責任者ユーザーのサジェスト用情報を取得APIのリクエストパラメータ
     */
    async suggestMainSupporterUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      params.role = ENUM_USER_ROLE.SUPPORTER_MGR
      params.disabled = false

      await SuggestUsers(params).then((res) => {
        this.suggestMainSupporterUsersResponse = res.data
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
     * GetUsersAPIを叩き、Front Officeにサインインできる一般ユーザーの検索・一覧を取得
     */
    async getUsers() {
      this.isLoading.serviceTypes = true
      const request = {}

      await GetUsers(request).then((res) => {
        this.getUsersResponse = res.data
        this.isLoading.serviceTypes = false
      })
    },
    /**
     * getSupporterOrganizationsAPIを叩き、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations().then((res) => {
        this.getSupporterOrganizationsResponse = res.data
      })
    },
  },
})
</script>
