<template>
  <TemplateUserDetail
    :user="user"
    :suggest-solver-corporations="SuggestSolverCorporationsResponse"
    :get-supporter-organizations-response="getSupporterOrganizationsResponse"
    :is-loading="isLoading"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateUserDetail from '~/components/user/templates/UserDetail.vue'
import { GetUserByIdResponse, GetUserById } from '~/models/User'
import {
  SuggestSolverCorporationsResponse,
  SuggestSolverCorporations,
} from '~/models/SolverCorporation'
import {
  GetSupporterOrganizationsResponse,
  GetSupporterOrganizations,
} from '~/models/Master'

export default BasePage.extend({
  name: 'UserDetail',
  layout: 'noPageHeader',
  middleware: ['roleCheck'],
  components: {
    TemplateUserDetail,
  },
  mounted() {
    this.displayLoading([
      this.getUserById(),
      this.SuggestSolverCorporations(),
      this.getSupporterOrganizations(),
    ])
  },
  data(): {
    user: GetUserByIdResponse
    SuggestSolverCorporationsResponse: SuggestSolverCorporationsResponse
    getSupporterOrganizationsResponse: GetSupporterOrganizationsResponse
    isLoading: boolean
  } {
    return {
      user: new GetUserByIdResponse(),
      /** SuggestSolverCorporations APIのレスポンス */
      SuggestSolverCorporationsResponse:
        new SuggestSolverCorporationsResponse(),
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      isLoading: false,
    }
  },
  methods: {
    /**
     * GetUserByIdAPIを叩き、一般ユーザーをIDで一意に取得
     */
    async getUserById() {
      const id: string = this.$route.params.userId

      await GetUserById(id).then((res) => {
        this.user = res.data
      })
    },
    /**
     * 再度GetUserByIdAPIを叩き、一般ユーザーをIDで一意に取得
     */
    refresh() {
      this.displayLoading([this.getUserById()])
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
        // APIではshortNameがついているがselectBOXに出力する際は不要なため削除の処理
        for (const val of res.data.supporterOrganizations) {
          delete val.shortName
        }
        this.getSupporterOrganizationsResponse = res.data
        this.isLoading = false
      })
    },
  },
})
</script>
