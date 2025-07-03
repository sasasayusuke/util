<template>
  <UtilizationRateList
    ref="UtilizationRateList"
    class="solver-utilization-rate-wrap"
    :response="getSolversResponse"
    :price-and-operating-rate-update-at="
      getSolverCorporationByIdResponse.priceAndOperatingRateUpdateAt
    "
    :is-loading="isLoading"
    :snackbar="snackbar"
    @updateSolverUtilizationRate="updateSolverUtilizationRate"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectList from '~/components/project/templates/ProjectList.vue'
import UtilizationRateList, {
  isLoading,
} from '~/components/solver/templates/UtilizationRateList.vue'
import {
  GetSolvers,
  GetSolversResponse,
  UpdateUtilizationRate,
  UpdateUtilizationRateRequest,
} from '~/models/Solver'
import {
  GetSolverCorporationById,
  GetSolverCorporationByIdResponse,
} from '~/models/SolverCorporation'
import { ENUM_GET_SOLVERS_REQUEST_SORT } from '~/types/Solver'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

export default BasePage.extend({
  layout: 'BaseLayoutWide',
  name: 'SolverUtilizationRateList',
  components: {
    TemplateProjectList,
    UtilizationRateList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.utilizationRate.name') as string,
    }
  },
  data(): {
    getSolversResponse: GetSolversResponse
    getSolverCorporationByIdResponse: GetSolverCorporationByIdResponse
    isLoading: isLoading
    snackbar: boolean
  } {
    return {
      /** GetSolvers APIのレスポンス */
      getSolversResponse: new GetSolversResponse(),
      /** GetSolverCorporationById APIのレスポンス */
      getSolverCorporationByIdResponse: new GetSolverCorporationByIdResponse(),
      /** ローディング中かどうか */
      isLoading: {
        solverCorporation: false,
        solver: false,
        solverUtilizationRate: false,
      },
      snackbar: false,
    }
  },
  mounted() {
    this.displayLoading([this.getSolvers(), this.getSolverCorporationById()])
  },
  computed: {
    /** アライアンス担当と法人ソルバー担当のみが画面表示可能 */
    isAllowAccess() {
      return hasRole([ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF])
    },
  },
  methods: {
    /** GetSolverCorporationById API: 法人ソルバーIDから法人ソルバー情報を取得する */
    async getSolverCorporationById() {
      this.isLoading.solverCorporation = true
      const id = this.$route.params.solver_corporation_id

      /**
       * 法人ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverCorporationById(id)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolverCorporationByIdResponse = res.data
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

    /** GetSolvers API: 1つの法人に紐づく個人ソルバーの一覧情報を取得する */
    async getSolvers() {
      this.isLoading.solver = true
      const request = {
        id: this.$route.params.solver_corporation_id,
        solverType: 'certificated_solver',
        sex: 'all',
        operatingStatus: 'all',
        sort: ENUM_GET_SOLVERS_REQUEST_SORT.PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC,
      }

      /**
       * ソルバー一覧取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolvers(request)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolversResponse = res.data
          this.isLoading.solver = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** UpdateSolverUtilizationRate API: 法人に属する全ての個人ソルバーの稼働率と単価を一括更新する */
    updateSolverUtilizationRate(
      params: UpdateUtilizationRateRequest = new UpdateUtilizationRateRequest()
    ) {
      this.isLoading.solverUtilizationRate = true
      this.clearErrorBar()
      const solverCorporationId = this.$route.params.solver_corporation_id
      const utilizationRateVersion =
        this.getSolverCorporationByIdResponse.utilizationRateVersion
      this.snackbar = false

      /**
       * 稼働率・単価更新APIを叩く
       * @param リクエストパラメータ
       */
      UpdateUtilizationRate(solverCorporationId, utilizationRateVersion, params)
        .then((res) => {
          this.getSolvers()
          this.getSolverCorporationById()
          if (res.data.deleted.length > 0) {
            this.showErrorBar(
              res.data.deleted.join('、') +
                this.$t(
                  'solver.pages.utilizationRate.errorMessage.notFoundSolver'
                )
            )
          }

          // 子コンポーネントのデータを更新
          // @ts-ignore
          this.$refs.UtilizationRateList.isFirstChange = false
          // @ts-ignore
          this.$refs.UtilizationRateList.buttonDisabled = true
          // スナックバー表示
          this.snackbar = true
        })
        .catch((error) => {
          this.apiErrorHandle(error)

          if (error.response && error.response.status === 400) {
            this.showErrorBar(
              this.$t(
                'solver.pages.utilizationRate.errorMessage.notFoundSolver'
              )
            )
          } else if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else if (error.response && error.response.status === 409) {
            this.showErrorBar(
              this.$t('solver.pages.utilizationRate.errorMessage.conflict')
            )
          } else {
            this.apiErrorHandle(error)
          }
        })
        .finally(() => {
          this.isLoading.solverUtilizationRate = false
        })
    },
  },
})
</script>
<style lang="scss" scoped>
.solver-utilization-rate-wrap {
  width: 1350px !important;
}
</style>
