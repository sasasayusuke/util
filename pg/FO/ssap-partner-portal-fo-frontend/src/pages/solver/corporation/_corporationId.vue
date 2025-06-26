<!-- 法人ソルバー詳細・変更画面ページ -->
<template>
  <TemplateSolverCorporationDetail
    :solver-corporation="getSolverCorporationByIdResponse"
    :issue-map50="getSelectItemsResponse"
    :is-solver-corporation-loading="isLoading"
    :is-check-corporation-registration="ischeckCorporationRegistration"
    :before-solver-corporation="beforeSolverCorporation"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSolverCorporationDetail from '~/components/solver-corporation/templates/SolverCorporationDetail.vue'
import {
  GetSolverCorporationById,
  GetSolverCorporationByIdResponse,
} from '~/models/SolverCorporation'
import { GetSelectItems, GetSelectItemsResponse } from '~/models/Master'
import { solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

export interface isLoading {
  selectItem: boolean
  solverCorporation: boolean
}

export default BasePage.extend({
  name: 'SolverCorporationDetail',
  components: {
    TemplateSolverCorporationDetail,
  },
  middleware: ['roleCheck'],
  data(): {
    /** GetolverCorporationById APIのレスポンス */
    getSolverCorporationByIdResponse: GetSolverCorporationByIdResponse
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    isLoading: isLoading
    ischeckCorporationRegistration: boolean
    corporationId: string
    beforeSolverCorporation: GetSolverCorporationByIdResponse
  } {
    return {
      /** GetSolverCorporationById APIのレスポンス */
      getSolverCorporationByIdResponse: new GetSolverCorporationByIdResponse(),
      /** GetSelectItems APIのレスポンス */
      getSelectItemsResponse: new GetSelectItemsResponse(),
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        solverCorporation: false,
      },
      ischeckCorporationRegistration: false,
      corporationId: '',
      beforeSolverCorporation: new GetSolverCorporationByIdResponse(),
    }
  },
  created() {
    this.corporationId = this.$route.params.corporationId
  },
  async mounted() {
    await this.displayLoading([this.getSelectItems('issue_map50')])
    await this.displayLoading([
      this.getSolverCorporationById(this.corporationId),
    ])
    this.checkCorporationRegistration()
  },
  computed: {
    /** アライアンス担当と法人ソルバー担当のみが画面表示可能 */
    isAllowAccess() {
      return hasRole([ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF])
    },
  },
  methods: {
    /** GetSelectItems API: 課題マップ50の選択肢を取得する */
    async getSelectItems(dataTypeValue: string) {
      this.isLoading.selectItem = true
      const request = { dataType: dataTypeValue }

      /**
       * 選択肢取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSelectItems(request)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          if (dataTypeValue === 'issue_map50') {
            this.getSelectItemsResponse = res.data
          }
          this.isLoading.selectItem = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /** GetSolverCorporationById API: 法人IDから法人ソルバー情報を取得する */
    async getSolverCorporationById(corporationId: string) {
      this.isLoading.solverCorporation = true
      /**
       * 法人ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverCorporationById(corporationId)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolverCorporationByIdResponse = res.data
          this.beforeSolverCorporation = JSON.parse(JSON.stringify(res.data))

          solverCorporationStore.setResponse(res.data)
          this.isLoading.solverCorporation = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /** 法人ソルバーの法人情報が初回登録されているか判定 */
    checkCorporationRegistration() {
      if (this.getSolverCorporationByIdResponse.updateAt === null) {
        this.ischeckCorporationRegistration = true
      } else {
        this.ischeckCorporationRegistration = false
      }
    },
    /**
     * 再度GetSolverCorporationByIdを叩き、法人ソルバーをIDで一意に取得
     */
    refresh() {
      this.displayLoading([this.getSolverCorporationById(this.corporationId)])
    },
  },
})
</script>
