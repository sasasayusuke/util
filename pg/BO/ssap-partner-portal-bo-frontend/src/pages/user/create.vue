<template>
  <TemplateUserCreate
    :user="user"
    :suggest-customers="suggestCustomersResponse"
    :suggest-solver-corporations="SuggestSolverCorporationsResponse"
    :get-supporter-organizations-response="getSupporterOrganizationsResponse"
    :is-loading="isLoading"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateUserCreate from '~/components/user/templates/UserCreate.vue'
import { CreateUserRequest } from '~/models/User'
import { SuggestCustomersResponse, SuggestCustomers } from '~/models/Customer'
import {
  SuggestSolverCorporationsResponse,
  SuggestSolverCorporations,
} from '~/models/SolverCorporation'
import {
  GetSupporterOrganizationsResponse,
  GetSupporterOrganizations,
} from '~/models/Master'

export default BasePage.extend({
  name: 'UserCreate',
  middleware: ['roleCheck'],
  components: {
    TemplateUserCreate,
  },
  head() {
    return {
      title: this.$t('user.pages.create.name') as string,
    }
  },
  data(): {
    suggestCustomersResponse: SuggestCustomersResponse
    SuggestSolverCorporationsResponse: SuggestSolverCorporationsResponse
    getSupporterOrganizationsResponse: GetSupporterOrganizationsResponse
    user: CreateUserRequest
    isLoading: boolean
  } {
    return {
      /** CreateUser APIのレスポンス */
      user: new CreateUserRequest(),
      /** SuggestCustomers APIのレスポンス */
      suggestCustomersResponse: new SuggestCustomersResponse(),
      /** SuggestSolverCorporations APIのレスポンス */
      SuggestSolverCorporationsResponse:
        new SuggestSolverCorporationsResponse(),
      /** getSupporterOrganizationsのレスポンス */
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      isLoading: false,
    }
  },
  mounted() {
    this.displayLoading([
      this.suggestCustomers(),
      this.SuggestSolverCorporations(),
      this.getSupporterOrganizations(),
    ])
  },
  methods: {
    /**
     * SuggestCustomersAPIを叩き、取引先のサジェスト用情報を取得
     */
    async suggestCustomers() {
      this.isLoading = true

      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
        this.isLoading = false
      })
    },
    /**
     * SuggestSolverCorporationsAPIを叩き、法人名のサジェスト用情報を取得
     */
    async SuggestSolverCorporations() {
      this.isLoading = true

      await SuggestSolverCorporations().then((res) => {
        this.SuggestSolverCorporationsResponse = res.data
        this.isLoading = false
      })
    },
    /**
     * getSupporterOrganizationsAPIを叩き、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      this.isLoading = true

      await GetSupporterOrganizations().then((res) => {
        this.getSupporterOrganizationsResponse = res.data
        this.isLoading = false
      })
    },
  },
})
</script>
