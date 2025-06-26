<template>
  <component
    :is="isCustomer ? 'TemplateHomeCustomer' : 'TemplateHomeSupporter'"
    v-if="isLoading === false"
    v-bind="attributes"
    @getSurveySummarySupporterOrganizations="
      getSurveySummarySupporterOrganizations
    "
    @getSurveySummaryAll="getSurveySummaryAll"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import { Auth } from 'aws-amplify'
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateHomeCustomer from '~/components/top/templates/HomeCustomer.vue'
import TemplateHomeSupporter from '~/components/top/templates/HomeSupporter.vue'
import { GetKartenLatest } from '~/models/Karte'
import { GetProjectsDateRequest, GetProjects } from '~/models/Project'
import {
  GetSurveysByMine,
  GetSurveySummaryByMineRequest,
  GetSurveySummaryByMineResponse,
  GetSurveySummaryByMine,
  GetSurveySummarySupporterOrganizationsByMineRequest,
  GetSurveySummarySupporterOrganizationsByMineResponse,
  GetSurveySummarySupporterOrganizationsByMine,
} from '~/models/Survey'
import {
  IGetSurveysByMineRequest,
  ENUM_GET_SURVEYS_BY_MINE_SORT,
  ENUM_GET_SURVEYS_BY_MINE_TYPE,
} from '@/types/Survey'

import {
  GetManHourByMineRequest,
  GetManHourByMineResponse,
  GetManHourByMine,
} from '~/models/ManHour'
import {
  GetBatchControlByIdResponse,
  GetBatchControlById,
} from '~/models/Master'
import { meStore, redirectStore } from '~/store'
import { ENUM_GET_PROJECTS_REQUEST_SORT } from '~/types/Project'
import { getAllowedRoles } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'

export default BasePage.extend({
  layout: 'home',
  components: {
    TemplateHomeCustomer,
    TemplateHomeSupporter,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('top.pages.home.management_menu') as string,
    }
  },
  data() {
    return {
      attributes: {},
      batchControlIds: {
        kpi: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#user`
        ),
        section: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#supporter_organization`
        ),
      },
      isLoading: true,
    }
  },
  beforeCreate() {
    // リダイレクトURLがセットされている場合は該当ページにリダイレクト
    const encodedRedirectUrl = redirectStore.url
    redirectStore.clearUrl()
    if (encodedRedirectUrl) {
      const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
      const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
      const allowedRoles = getAllowedRoles(decodedRedirectPath)
      if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
        this.$router.push(decodedRedirectUrl)
      }
    } else if (
      // ログインユーザーがアライアンス担当または法人ソルバー担当の場合、ソルバートップに遷移
      meStore.role === ENUM_USER_ROLE.APT ||
      meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
    ) {
      this.$router.push('/solver/menu')
    }
  },
  created() {
    // 認証情報の取得
    Auth.currentAuthenticatedUser()
      .then((user) => {
        const sub = user.signInUserSession.idToken.payload.sub
        this.$logger.info(`cognito_id: ${sub}`)
      })
      .catch((err) => {
        this.$logger.error(err)
      })
  },
  mounted() {
    if (this.isCustomer) {
      // お客様ロールの場合
      this.displayLoading([this.setCustomerAttrs()]).then(() => {
        this.isLoading = false
      })
    } else if (!this.isAptOrSolver) {
      // アライアンス担当と法人ソルバー担当以外の場合
      this.displayLoading([this.setSupporterAttrs()]).then(() => {
        this.isLoading = false
      })
    }
  },
  computed: {
    isAptOrSolver() {
      return (
        meStore.role === ENUM_USER_ROLE.APT ||
        meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
      )
    },
    isCustomer() {
      return meStore.role === ENUM_USER_ROLE.CUSTOMER
    },
    isSales() {
      return meStore.role === ENUM_USER_ROLE.SALES
    },
    isSalesMgr() {
      return meStore.role === ENUM_USER_ROLE.SALES_MGR
    },
    isBusinessMgr() {
      return meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
    },
    thisMonth() {
      const dt = getCurrentDate()
      return dt.getMonth() + 1
    },
    thisMonthYear() {
      const dt = getCurrentDate()
      return dt.getFullYear()
    },
    lastMonth() {
      const dt = getCurrentDate()
      dt.setMonth(dt.getMonth() - 1)
      return dt.getMonth() + 1
    },
    lastMonthYear() {
      const dt = getCurrentDate()
      dt.setMonth(dt.getMonth() - 1)
      return dt.getFullYear()
    },
  },
  methods: {
    async setCustomerAttrs() {
      this.$set(this.attributes, 'thisMonth', this.thisMonth)
      this.$set(this.attributes, 'thisMonthYear', this.thisMonthYear)
      this.$set(this.attributes, 'lastMonth', this.lastMonth)
      this.$set(this.attributes, 'lastMonthYear', this.lastMonthYear)
      this.$set(this.attributes, 'projects', [])
      this.$set(this.attributes, 'karten', [])
      this.$set(this.attributes, 'surveys', [])
      await this.getProjects()
      await this.getKartenLatest()
      await this.getSurveysByMine()
    },
    async setSupporterAttrs() {
      this.$set(this.attributes, 'thisMonth', this.thisMonth)
      this.$set(this.attributes, 'thisMonthYear', this.thisMonthYear)
      this.$set(this.attributes, 'lastMonth', this.lastMonth)
      this.$set(this.attributes, 'lastMonthYear', this.lastMonthYear)
      this.$set(this.attributes, 'projects', [])
      this.$set(this.attributes, 'karten', [])
      this.$set(this.attributes, 'surveys', [])
      this.$set(
        this.attributes,
        'surveySummaryAllParams',
        new GetSurveySummaryByMineRequest()
      )
      this.$set(
        this.attributes,
        'surveySummarySupporterOrganizationsParams',
        new GetSurveySummarySupporterOrganizationsByMineRequest()
      )
      this.$set(
        this.attributes,
        'thisMonthManHourStatuses',
        new GetManHourByMineResponse()
      )
      this.$set(
        this.attributes,
        'lastMonthManHourStatuses',
        new GetManHourByMineResponse()
      )
      this.$set(
        this.attributes,
        'surveySummaryAll',
        new GetSurveySummaryByMineResponse()
      )
      this.$set(
        this.attributes,
        'surveySummarySupporterOrganizations',
        new GetSurveySummarySupporterOrganizationsByMineResponse()
      )
      this.$set(
        this.attributes,
        'batchControlKpi',
        new GetBatchControlByIdResponse()
      )
      this.$set(
        this.attributes,
        'batchControlSection',
        new GetBatchControlByIdResponse()
      )
      await this.getProjects()
      await this.getSurveysByMine()
      await this.getSurveySummarySupporterOrganizations()
      await this.getSurveySummaryAll()
      await this.getBatchControlKpi()
      await this.getBatchControlSection()
      // 営業担当者、営業責任者又は事業者責任者の場合は支援工数の取得を行わない。
      if (!this.isSales && !this.isSalesMgr && !this.isBusinessMgr) {
        await this.getThisMonthManHourByMine()
        await this.getLastMonthManHourByMine()
      }
      // 営業責任者の場合はカルテの取得を行わない。
      if (!this.isSalesMgr) {
        await this.getKartenLatest()
      }
    },
    /**
     * GetProjectsAPIから案件情報を取得
     * 支援者・支援者責任者・事業者責任者・顧客ユーザーの場合は所属案件、営業担当者の場合は全案件を取得
     */
    async getProjects(
      params: GetProjectsDateRequest = new GetProjectsDateRequest()
    ) {
      params.sort = ENUM_GET_PROJECTS_REQUEST_SORT.SUPPORT_DATE_FROM_DESC
      params.all = false
      params.allAssigned = true
      if (this.isSales) {
        params.allAssigned = false
      }
      await GetProjects(params).then((res) => {
        this.$set(this.attributes, 'projects', res.data.projects)
      })
    },
    /**
     * GetKartenLatestAPIから最近更新されたカルテ一覧を取得
     */
    async getKartenLatest() {
      await GetKartenLatest().then((res) => {
        this.$set(this.attributes, 'karten', res.data)
      })
    },
    async getSurveysByMine(
      params: IGetSurveysByMineRequest = {
        sort: ENUM_GET_SURVEYS_BY_MINE_SORT.PLAN_SURVEY_RESPONSE_DATETIME_DESC,
        limit: 3,
      }
    ) {
      if (!this.isCustomer) {
        params.type = ENUM_GET_SURVEYS_BY_MINE_TYPE.PP
        params.isFinished = false
      }
      await GetSurveysByMine(params).then((res) => {
        this.$set(this.attributes, 'surveys', res.data.surveys)
      })
    },
    /**
     * GetManHourByMineAPIから支援者自身の当月の支援工数を取得
     */
    async getThisMonthManHourByMine(
      params: GetManHourByMineRequest = new GetManHourByMineRequest()
    ) {
      params.year = this.thisMonthYear
      params.month = this.thisMonth
      await GetManHourByMine(params).then((res) => {
        this.$set(this.attributes, 'thisMonthManHourStatuses', res.data)
      })
    },
    /**
     * GetManHourByMineAPIから支援者自身の先月の支援工数を取得
     */
    async getLastMonthManHourByMine(
      params: GetManHourByMineRequest = new GetManHourByMineRequest()
    ) {
      params.year = this.lastMonthYear
      params.month = this.lastMonth
      await GetManHourByMine(params).then((res) => {
        this.$set(this.attributes, 'lastMonthManHourStatuses', res.data)
      })
    },
    /**
     * GetSurveySummarySupporterOrganizationsByMineAPIからアンケートの課（粗利メイン課）別の集計結果を取得
     * 支援者、支援者責任者の場合は自身の課の集計結果を取得
     * 営業担当者、営業責任者、事業者責任者の場合は全ての課の集計結果を取得
     */
    async getSurveySummarySupporterOrganizations(
      params: GetSurveySummarySupporterOrganizationsByMineRequest = new GetSurveySummarySupporterOrganizationsByMineRequest()
    ) {
      if (
        params.yearMonthFrom != null &&
        !Number.isInteger(params.yearMonthFrom)
      ) {
        params.yearMonthFrom = parseInt(
          format(
            new Date(params.yearMonthFrom),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (params.yearMonthTo != null && !Number.isInteger(params.yearMonthTo)) {
        params.yearMonthTo = parseInt(
          format(
            new Date(params.yearMonthTo),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      this.$set(
        this.attributes,
        'surveySummarySupporterOrganizationsParams',
        params
      )
      await GetSurveySummarySupporterOrganizationsByMine(params).then((res) => {
        this.$set(
          this.attributes,
          'surveySummarySupporterOrganizations',
          res.data
        )
      })
    },
    /**
     * GetSurveySummaryByMineAPIから自身の回答対象の案件アンケートの一覧を取得
     */
    async getSurveySummaryAll(
      params: GetSurveySummaryByMineRequest = new GetSurveySummaryByMineRequest()
    ) {
      if (
        params.yearMonthFrom != null &&
        !Number.isInteger(params.yearMonthFrom)
      ) {
        params.yearMonthFrom = parseInt(
          format(
            new Date(params.yearMonthFrom),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (params.yearMonthTo != null && !Number.isInteger(params.yearMonthTo)) {
        params.yearMonthTo = parseInt(
          format(
            new Date(params.yearMonthTo),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      this.$set(this.attributes, 'surveySummaryAllParams', params)
      await GetSurveySummaryByMine(params).then((res) => {
        this.$set(this.attributes, 'surveySummaryAll', res.data)
      })
    },
    /**
     * GetBatchControlByIdAPIから担当案件集計バッチ処理の最終完了日時を取得
     */
    async getBatchControlKpi() {
      await GetBatchControlById(this.batchControlIds.kpi).then((res) => {
        this.$set(this.attributes, 'batchControlKpi', res.data)
      })
    },
    /**
     * GetBatchControlByIdAPIから課別集計バッチ処理の最終完了日時を取得
     */
    async getBatchControlSection() {
      await GetBatchControlById(this.batchControlIds.section).then((res) => {
        this.$set(this.attributes, 'batchControlSection', res.data)
      })
    },
  },
})
</script>
