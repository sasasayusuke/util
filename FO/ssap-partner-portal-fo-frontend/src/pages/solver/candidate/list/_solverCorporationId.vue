<template>
  <TemplateSolverCandidateList
    :response="getSolversResponse"
    :is-loading="isLoading"
    @getSolvers="getSolvers"
    @patchSolver="PatchSolverStatusById"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSolverCandidateList, {
  isLoading,
} from '~/components/solver/templates/SolverCandidateList.vue'
import {
  GetSolvers,
  GetSolversRequest,
  GetSolversResponse,
  PatchSolverStatusById,
} from '~/models/Solver'
import { ENUM_GET_SOLVERS_REQUEST_SORT, ENUM_SOLVER_TYPE } from '~/types/Solver'

export default BasePage.extend({
  name: 'SolverCandidateList',
  components: {
    TemplateSolverCandidateList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.candidateList.name') as string,
    }
  },
  data(): {
    getSolversResponse: GetSolversResponse
    solverCorporationId: string
    isLoading: isLoading
  } {
    return {
      /** GetSolvers APIのレスポンス */
      getSolversResponse: new GetSolversResponse(),
      /** 法人ソルバーID */
      solverCorporationId: '',
      /** ローディング中かどうか */
      isLoading: {
        solvers: true,
      },
    }
  },
  created() {
    this.solverCorporationId = this.$route.params.solverCorporationId
  },
  mounted() {
    this.displayLoading([this.getSolvers()])
  },
  methods: {
    /** GetSolvers API: 1つの法人に紐づくソルバー候補の一覧情報を取得する */
    async getSolvers(params: GetSolversRequest = new GetSolversRequest()) {
      this.isLoading.solvers = true

      params.id = this.solverCorporationId
      params.solverType = ENUM_SOLVER_TYPE.SOLVER_CANDIDATE
      params.limit = 10

      if (params && 'sort' in params && typeof params.sort === 'string') {
        if (params.sort.includes('sex') || params.sort.includes('birth_day')) {
          /* eslint-disable no-self-assign */
          params.sort = params.sort
        }
        // 年齢でソートされた時、値を整形する
        else if (params.sort.includes('age')) {
          // @ts-ignore
          params.sort = params.sort.replace(/age(?=:)/g, 'birth_day')
        }
        // create_at:desc（デフォルト）
        else {
          params.sort = ENUM_GET_SOLVERS_REQUEST_SORT.CREATE_AT_DESC
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
    /** PatchSolverStatusById API: ソルバー候補を認定された個人ソルバーとして登録する */
    async PatchSolverStatusById(solverId: string, version: number) {
      if (solverId && version) {
        await PatchSolverStatusById(solverId, version)
          .then(() => {
            // ソルバー候補の更新後、再度一覧取得する
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
