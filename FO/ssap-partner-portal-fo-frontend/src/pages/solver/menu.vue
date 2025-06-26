<!-- ソルバーメニュー画面ページ -->
<template>
  <TemplateSolverTop
    :solver-corporations="getSolverCorporationsResponse"
    :is-menu-display="isMenuDisplay"
    :is-modal-open="isModalOpen"
    :solver-corporation-name="solverCorporationName"
    @closeModal="closeModal"
    @changeSolverCorporation="checkCorporationRegistration"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSolverTop from '~/components/solver/templates/SolverTop.vue'
import {
  GetSolverCorporationById,
  GetSolverCorporationByIdResponse,
  GetSolverCorporations,
  GetSolverCorporationsResponse,
} from '~/models/SolverCorporation'
import {
  fullScreenLoadingStore,
  meStore,
  solverCorporationStore,
} from '~/store'
import { ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT } from '~/types/SolverCorporation'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

export interface isLoading {
  solverCorporation: boolean
  solverCorporationAll: boolean
}

export default BasePage.extend({
  name: 'SolverMenu',
  layout: 'noPageHeader',
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.menu.title') as string,
    }
  },
  components: {
    TemplateSolverTop,
  },
  data(): {
    getSolverCorporationByIdResponse: GetSolverCorporationByIdResponse
    getSolverCorporationsResponse: GetSolverCorporationsResponse
    isLoading: isLoading
    isMenuDisplay: boolean
    isModalOpen: boolean
    solverCorporationName: string
  } {
    return {
      /** GetSolverCorporationById APIのレスポンス */
      getSolverCorporationByIdResponse: new GetSolverCorporationByIdResponse(),
      /** GetSolverCorporations APIのレスポンス */
      getSolverCorporationsResponse: new GetSolverCorporationsResponse(),
      /** ローディング中か否か */
      isLoading: {
        solverCorporation: false,
        solverCorporationAll: false,
      },
      /** メニューを表示するか否か */
      isMenuDisplay: true,
      /** 法人ソルバー情報入力誘導モーダルを表示するか否か */
      isModalOpen: false,
      /** 企業名 */
      solverCorporationName: '',
    }
  },
  mounted() {
    if (meStore.role === ENUM_USER_ROLE.SOLVER_STAFF) {
      this.displayLoading([
        this.checkCorporationRegistration(meStore.solverCorporationId),
      ])
      fullScreenLoadingStore.clearHold()
    } else if (
      meStore.role === ENUM_USER_ROLE.APT &&
      solverCorporationStore.id
    ) {
      this.displayLoading([
        this.getSolverCorporations(),
        this.checkCorporationRegistration(solverCorporationStore.id),
      ])
      fullScreenLoadingStore.clearHold()
    } else {
      this.displayLoading([this.getSolverCorporations()])
      this.isMenuDisplay = false
      fullScreenLoadingStore.clearHold()
    }
  },
  computed: {
    /** アライアンス担当と法人ソルバー担当のみが画面表示可能 */
    isAllowAccess() {
      return hasRole([ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF])
    },
  },
  methods: {
    /** GetSolverCorporationById API: 法人ソルバーIDから法人ソルバー情報を取得する */
    async getSolverCorporationById(id: string) {
      this.isLoading.solverCorporation = true
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

    /** GetSolverCorporations API: 法人ソルバーの一覧情報を取得する */
    async getSolverCorporations() {
      this.isLoading.solverCorporationAll = true
      const request = {
        disabled: false,
        sort: ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT.NAME_ASC,
      }
      /**
       * 法人ソルバー一覧取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverCorporations(request)
        .then((res) => {
          /** アライアンス担当と法人ソルバー担当のみが画面表示可能 */
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolverCorporationsResponse = res.data
          this.isLoading.solverCorporationAll = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** 法人ソルバーの法人情報が初回登録されているか判定する */
    async checkCorporationRegistration(newValue: string) {
      await this.getSolverCorporationById(newValue)

      this.solverCorporationName = this.getSolverCorporationByIdResponse.name

      if (this.getSolverCorporationByIdResponse.updateAt) {
        if (meStore.role === ENUM_USER_ROLE.APT) {
          this.isMenuDisplay = true
        }
        this.isModalOpen = false
      } else {
        if (meStore.role === ENUM_USER_ROLE.APT) {
          this.isMenuDisplay = false
        }
        this.isModalOpen = true
      }
    },

    /** 法人ソルバー情報入力誘導モーダルを閉じる */
    closeModal() {
      this.isModalOpen = false
    },
  },
})
</script>
