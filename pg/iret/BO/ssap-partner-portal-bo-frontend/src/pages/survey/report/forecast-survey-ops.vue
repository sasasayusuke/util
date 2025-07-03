<template>
  <TemplateSurveyForecast
    :response="getSurveyPlansResponse"
    :is-loading="isLoading"
    :terms-from="termsFrom"
    :terms-to="termsTo"
    :total="total"
    :sort-by="sortBy"
    :sort-desc="sortDesc"
    @getSurveysBySummaryMonth="getSurveysBySummaryMonth"
    @getSurveysByPlanSurveyResponseDate="getSurveysByPlanSurveyResponseDate"
    @update:sort-by-survey-ops="emitSortBy"
    @update:sort-desc-survey-ops="emitSortDesc"
  />
</template>

<script lang="ts">
import { format, parse } from 'date-fns'
import { getCurrentDate, toYearMonthInt } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateSurveyForecast from '~/components/survey/templates/SurveyForecast.vue'
import { GetSurveyPlans, GetSurveyPlansResponse } from '~/models/Survey'
import { IGetSurveyPlansRequest } from '~/types/Survey'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { hasRole } from '~/utils/role-authorizer'

export default BasePage.extend({
  name: 'SurveyReportForecastSurveyOps',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyForecast,
  },
  head() {
    return {
      title: this.$t('survey.pages.forecast.name') as string,
    }
  },
  data() {
    return {
      getSurveyPlansResponse: new GetSurveyPlansResponse(),
      isLoading: true,
      termsFrom: '',
      termsTo: '',
      total: 0,
      sortDefines: [
        'salesUserName',
        'serviceTypeName',
        'customerName',
        'projectName',
        'surveyType',
        'mainSupporterUser',
        'supporterUsers',
        'surveyUserType',
        'surveyUserName',
        'surveyUserEmail',
        'customerSuccess',
        'planSurveyRequestDatetime',
        'actualSurveyRequestDatetime',
        'planSurveyResponseDatetime',
        'actualSurveyResponseDatetime',
        'summaryMonth',
        'supporterOrganizationName',
        'supportDateFrom',
        'supportDateTo',
        'phase',
        'isCountManHour',
      ],
      orderDefines: ['acs', 'desc'],
      sortBy: [] as string[],
      sortDesc: [] as boolean[],
      clickedSortButton: false,
      isFirstLoadingSortBy: true,
      isFirstLoadingSortDesc: true,
    }
  },
  mounted() {
    this.setSortOrder()
    this.displayLoading([this.getSurveysBySummaryMonth()])
  },
  methods: {
    /**
     * getSurveyPlans APIを叩き、アンケートの一覧を取得して代入。
     * @param params 受信予定日ベースのgetSurveyPlans APIのリクエストパラメータを指定
     */
    async getSurveysByPlanSurveyResponseDate(
      params: IGetSurveyPlansRequest = {
        planSurveyResponseDateFrom: parseInt(
          format(getCurrentDate(), 'yyyyMMdd')
        ),
        planSurveyResponseDateTo: parseInt(
          format(getCurrentDate(), 'yyyyMMdd')
        ),
      }
    ) {
      this.clearErrorBar()
      this.isLoading = true
      const dateObjectFrom = parse(
        String(params.planSurveyResponseDateFrom),
        'yyyyMMdd',
        getCurrentDate()
      )
      const dateObjectTo = parse(
        String(params.planSurveyResponseDateTo),
        'yyyyMMdd',
        getCurrentDate()
      )
      this.termsFrom = format(dateObjectFrom, 'yyyy/MM/dd')
      this.termsTo = format(dateObjectTo, 'yyyy/MM/dd')
      if (
        params.planSurveyResponseDateFrom != null &&
        !Number.isInteger(params.planSurveyResponseDateFrom)
      ) {
        params.planSurveyResponseDateFrom = parseInt(
          format(new Date(params.planSurveyResponseDateFrom), 'yyyyMMdd')
        )
      }
      if (
        params.planSurveyResponseDateTo != null &&
        !Number.isInteger(params.planSurveyResponseDateTo)
      ) {
        params.planSurveyResponseDateTo = parseInt(
          format(new Date(params.planSurveyResponseDateTo), 'yyyyMMdd')
        )
      }
      await GetSurveyPlans(params).then((res) => {
        this.getSurveyPlansResponse = res.data
        this.total = this.getSurveyPlansResponse.surveys.length
        if (this.total <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
        this.isLoading = false
      })
    },
    /**
     * getSurveyPlans APIを叩き、アンケートの一覧を取得して代入。
     * @param params 集計月ベースのgetSurveyPlans APIのリクエストパラメータを指定
     */
    async getSurveysBySummaryMonth(
      params: IGetSurveyPlansRequest = {
        summaryMonthFrom: toYearMonthInt(getCurrentDate()),
        summaryMonthTo: toYearMonthInt(getCurrentDate()),
      }
    ) {
      this.clearErrorBar()
      this.isLoading = true
      const dateObjectFrom = parse(
        String(params.summaryMonthFrom),
        'yyyyMM',
        getCurrentDate()
      )
      const dateObjectTo = parse(
        String(params.summaryMonthTo),
        'yyyyMM',
        getCurrentDate()
      )
      this.termsFrom = format(dateObjectFrom, 'yyyy/MM')
      this.termsTo = format(dateObjectTo, 'yyyy/MM')
      if (
        params.summaryMonthFrom != null &&
        !Number.isInteger(params.summaryMonthFrom)
      ) {
        params.summaryMonthFrom = parseInt(
          format(new Date(params.summaryMonthFrom), 'yyyyMM')
        )
      }
      if (
        params.summaryMonthTo != null &&
        !Number.isInteger(params.summaryMonthTo)
      ) {
        params.summaryMonthTo = parseInt(
          format(new Date(params.summaryMonthTo), 'yyyyMM')
        )
      }
      await GetSurveyPlans(params).then((res) => {
        this.getSurveyPlansResponse = res.data
        this.total = this.getSurveyPlansResponse.surveys.length
        if (this.total <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
        this.isLoading = false
      })
    },
    // クエリパラメータからソート対象および並び順を抽出して値をセット
    setSortOrder() {
      this.sortBy = [] as string[]
      this.sortDesc = [] as boolean[]
      if (this.$route.query.sort) {
        if (Array.isArray(this.$route.query.sort)) {
          for (const i in this.$route.query.sort) {
            this.adjustSortOrder(this.$route.query.sort[i])
          }
        } else {
          this.adjustSortOrder(this.$route.query.sort)
        }
      } else {
        this.setDefaultSortOrder()
      }
    },
    // 権限によってデフォルトのソート対象および並び順を設定する
    setDefaultSortOrder() {
      this.sortBy = [] as string[]
      this.sortDesc = [] as boolean[]
      if (hasRole([ENUM_ADMIN_ROLE.SALES, ENUM_ADMIN_ROLE.SALES_MGR])) {
        this.sortBy.push('salesUserName')
        this.sortDesc.push(false)
      } else {
        this.sortBy.push('serviceTypeName')
        this.sortDesc.push(false)
      }
    },
    // クエリパラメータから有効なソート対象および並び順のみを抽出する
    adjustSortOrder(querySortOrder: string) {
      querySortOrder = decodeURIComponent(querySortOrder)
      const arrSortOrder = querySortOrder.split(':')
      const querySort = arrSortOrder[0] ? arrSortOrder[0].trim() : ''
      let queryOrder = arrSortOrder[1] ? arrSortOrder[1].trim() : ''
      if (queryOrder === '' || !this.orderDefines.includes(queryOrder)) {
        queryOrder = 'asc'
      }
      if (this.sortDefines.includes(querySort)) {
        this.sortBy.push(querySort)
        if (queryOrder === 'desc') {
          this.sortDesc.push(true)
        } else {
          this.sortDesc.push(false)
        }
      }
    },
    // 子コンポーネントから受け取ったソート対象変更イベントを精査して値のセットおよびURLの変更を行う
    emitSortBy(event: string[]) {
      this.sortBy = event
      const querySortOrders: string[] = [] as string[]
      const routeQuerySortOrders: string[] = [] as string[]
      for (const i in this.sortBy) {
        const order = this.sortDesc[i] ? 'desc' : 'asc'
        const value = `${this.sortBy[i]}:${order}`
        routeQuerySortOrders.push(value)
        querySortOrders.push(`sort=${encodeURIComponent(value)}`)
      }
      this.$route.query.sort = routeQuerySortOrders
      if (!this.isFirstLoadingSortBy) {
        history.pushState(
          {},
          '',
          `/survey/report/forecast-survey-ops?${querySortOrders.join('&')}`
        )
      } else {
        this.isFirstLoadingSortBy = false
      }
    },
    // 子コンポーネントから受け取ったソート並び順変更イベントを精査して値のセットおよびURLの変更を行う
    emitSortDesc(event: boolean[]) {
      this.sortDesc = event
      const querySortOrders: string[] = [] as string[]
      const routeQuerySortOrders: string[] = [] as string[]
      for (const i in this.sortDesc) {
        const order = this.sortDesc[i] ? 'desc' : 'asc'
        const value = `${this.sortBy[i]}:${order}`
        routeQuerySortOrders.push(value)
        querySortOrders.push(`sort=${encodeURIComponent(value)}`)
      }
      this.$route.query.sort = routeQuerySortOrders
      if (!this.isFirstLoadingSortDesc) {
        history.pushState(
          {},
          '',
          `/survey/report/forecast-survey-ops?${querySortOrders.join('&')}`
        )
      } else {
        this.isFirstLoadingSortDesc = false
      }
    },
  },
})
</script>
