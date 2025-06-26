<template>
  <SolverList
    :response="getSolversResponse"
    :is-loading="isLoading"
    @getSolvers="getSolvers"
    @update="PatchSolverStatusById"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import SolverList from '~/components/solver/templates/SolverList.vue'
import {
  GetSolvers,
  GetSolversRequest,
  GetSolversResponse,
  PatchSolverStatusById,
} from '~/models/Solver'
import { ENUM_GET_SOLVERS_REQUEST_SORT, ENUM_SOLVER_TYPE } from '~/types/Solver'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export interface isLoading {
  solvers: boolean
}

export default BasePage.extend({
  name: 'SolverCorporationId',
  components: {
    SolverList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.list.name') as string,
    }
  },
  data(): {
    getSolversResponse: GetSolversResponse
    solverCorporationId: string
    isLoading: isLoading
    isSort: boolean
  } {
    return {
      /** GetSolvers APIのレスポンス */
      getSolversResponse: new GetSolversResponse(),
      /** 法人ソルバーID */
      solverCorporationId: '',
      /** ローディング中かどうか */
      isLoading: {
        solvers: false,
      },
      /** 並び替えかどうか */
      isSort: false,
    }
  },
  created() {
    this.solverCorporationId = this.$route.params.solverCorporationId
  },
  mounted() {
    const solverCorporationIdFromURL = this.$route.params.solverCorporationId
    if (
      meStore.role === ENUM_USER_ROLE.SOLVER_STAFF &&
      solverCorporationIdFromURL !== meStore.solverCorporationId
    ) {
      // アクセスを許可しない 403エラーページにリダイレクト
      this.$router.push('/403')
    } else {
      this.displayLoading([this.getSolvers()])
    }
  },
  methods: {
    /** GetSolvers API: 1つの法人に紐づく個人ソルバーの一覧情報を取得する */
    async getSolvers(params: GetSolversRequest = new GetSolversRequest()) {
      this.isLoading.solvers = true

      params.id = this.solverCorporationId
      params.solverType = ENUM_SOLVER_TYPE.SOLVER
      params.limit = 10

      if (params && 'sort' in params && typeof params.sort === 'string') {
        if (params.sort.includes('sex') || params.sort.includes('birth_day')) {
          this.isSort = true
          /* eslint-disable no-self-assign */
          params.sort = params.sort
        }
        // 年齢でソートされた時、値を整形する
        else if (params.sort.includes('age')) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(/age(?=:)/g, 'birth_day')
        }
        // operatingStatusでソートされた時、値を整形する
        else if (
          params.sort.includes('operatingStatus') ||
          params.sort.includes('operating_status')
        ) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(
            /operatingStatus(?=:)/g,
            'operating_status'
          )
        }
        // providedOperatingRateでソートされた時、値を整形する
        // 提供稼働率（来月）と区別するため、コロン（：）を含めた上で判定
        else if (
          params.sort.includes('providedOperatingRate:') ||
          params.sort.includes('provided_operating_rate:')
        ) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(
            /providedOperatingRate(?=:)/g,
            'provided_operating_rate'
          )
        }
        // providedOperatingRateNextでソートされた時、値を整形する
        else if (
          params.sort.includes('providedOperatingRateNext') ||
          params.sort.includes('provided_operating_rate_next')
        ) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(
            /providedOperatingRateNext(?=:)/g,
            'provided_operating_rate_next'
          )
        }
        // pricePerPersonMonthでソートされた時、値を整形する
        else if (
          params.sort.includes('pricePerPersonMonth') ||
          params.sort.includes('price_per_person_month')
        ) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(
            /pricePerPersonMonth(?=:)/g,
            'price_per_person_month'
          )
        }
        // registrationStatusでソートされた時、値を整形する
        else if (
          params.sort.includes('registrationStatus') ||
          params.sort.includes('registration_status')
        ) {
          this.isSort = true
          // @ts-ignore
          params.sort = params.sort.replace(
            /registrationStatus(?=:)/g,
            'registration_status'
          )
        }
        // create_at:desc（デフォルト）
        else {
          params.sort = ENUM_GET_SOLVERS_REQUEST_SORT.CREATE_AT_DESC
        }
      }

      // 並び替えが押されていた場合
      if (this.isSort) {
        if (params.sex === 'all') {
          params.sex = ''
        }
        if (params.operatingStatus === 'all') {
          params.operatingStatus = ''
        }
      }

      await GetSolvers(params)
        .then((res) => {
          this.getSolversResponse = res.data
          this.isLoading.solvers = false
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
     * PatchSolverStatusById API: 個人ソルバーを「認定された個人ソルバー」として登録する
     * @param {string} solverId - 認定する個人ソルバーID
     * @param {number} version - ソルバーのバージョン
     * */
    async PatchSolverStatusById(solverId: string, version: number) {
      if (solverId && version) {
        await PatchSolverStatusById(solverId, version)
          .then(() => {
            // 個人ソルバーの更新後、再度一覧取得する
            this.getSolvers()
          })
          .catch((error) => {
            if (error.response && error.response.status === 404) {
              this.$router.push('/404')
            } else {
              this.apiErrorHandle(error)
            }
          })
      }
    },
  },
})
</script>
