<template>
  <TemplateSurveyList
    :is-loading="isLoading"
    :surveys-response="surveysResponse"
    :service-types-response="serviceTypesResponse"
    :supporter-organizations-response="supporterOrganizationsResponse"
    :customers="suggestCustomers"
    :projects="suggestProjects"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :limit="limit"
    :total="total"
    :query-type="queryType"
    @getSurveysBySummaryMonth="getSurveysBySummaryMonth"
    @getSurveysByPlanSurveyResponseDate="getSurveysByPlanSurveyResponseDate"
    @exportSurveys="exportSurveys"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import {
  getCurrentDate,
  getFiscalYearStart,
  getFiscalYearEnd,
  toYearMonthInt,
} from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateSurveyList from '~/components/survey/templates/SurveyList.vue'
import { GetSurveysResponse, GetSurveys, ExportSurveys } from '~/models/Survey'
import { IGetSurveysRequest, IExportSurveysRequest } from '~/types/Survey'

import {
  SuggestProjectsRequest,
  SuggestProjectsResponse,
  GetSuggestProjects,
} from '~/models/Project'

import {
  SuggestCustomersRequest,
  SuggestCustomersResponse,
  SuggestCustomers,
} from '~/models/Customer'

import {
  GetServiceTypes,
  GetServiceTypesResponse,
  GetSupporterOrganizations,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyList',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyList,
  },
  head() {
    return {
      title: this.$t('survey.pages.list.name') as string,
    }
  },
  data() {
    return {
      surveysResponse: new GetSurveysResponse(),
      suggestProjects: new SuggestProjectsResponse(),
      suggestCustomers: new SuggestCustomersResponse(),
      serviceTypesResponse: new GetServiceTypesResponse(),
      supporterOrganizationsResponse: new GetSupporterOrganizationsResponse(),
      isLoading: {
        surveys: true,
        suggestProjects: true,
        suggestCustomers: true,
        serviceTypesResponse: true,
        supporterOrganizationsResponse: true,
      },
      limit: 20,
      offsetPage: 1,
      maxPage: 1,
      total: 0,
      queryType: '',
    }
  },
  mounted(this: any) {
    // クエリパラメータにてアンケート種別が含まれている場合はqueryTypeに代入。
    if (
      this.$route.query.type &&
      (this.$route.query.type === 'service' ||
        this.$route.query.type === 'completion' ||
        this.$route.query.type === 'service_and_completion' ||
        this.$route.query.type === 'quick')
    ) {
      this.queryType = this.$route.query.type
    }
    // クエリパラメータにてアンケート種別が含まれている場合はアドレスバーからクエリパラメータを削除
    if (this.$route.query.type) {
      this.$router.push('/survey/list')
    }
    this.displayLoading([
      this.getSurveysBySummaryMonth(),
      this.getSuggestProjects(),
      this.getSuggestCustomers(),
      this.getSupporterOrganizations(),
      this.getServiceTypes(),
    ])
  },
  methods: {
    /**
     * getSurveysAPIを叩き、アンケートの一覧を取得して代入。
     * @param params 集計月ベースのgetSurveysAPIのリクエストパラメータを指定
     */
    async getSurveysBySummaryMonth(
      this: any,
      params: IGetSurveysRequest = {
        type: this.queryType !== '' ? this.queryType : 'service_and_completion',
        summaryMonthFrom: toYearMonthInt(getFiscalYearStart()),
        summaryMonthTo: toYearMonthInt(getFiscalYearEnd()),
        projectId: '',
        customerId: '',
        organizationIds: '',
        serviceTypeId: '',
        isFinished: true,
      }
    ) {
      this.clearErrorBar()
      this.isLoading.surveys = true
      await GetSurveys(params)
        .then((res) => {
          this.surveysResponse = res.data
          this.total = this.surveysResponse.surveys.length
          this.offsetPage = 1
          if (Number(this.total) === 0) {
            this.maxPage = 1
          } else {
            this.maxPage = Math.ceil(
              this.surveysResponse.surveys.length / this.limit
            )
          }
          if (this.total <= 0) {
            this.showErrorBarWithScrollPageTop(
              this.$t('msg.error.csvExportDisabled')
            )
          }
        })
        .finally(() => {
          this.isLoading.surveys = false
        })
    },
    /**
     * getSurveysAPIを叩き、アンケートの一覧を取得して代入。
     * @param params 受信予定日ベースのgetSurveysAPIのリクエストパラメータを指定
     */
    async getSurveysByPlanSurveyResponseDate(
      this: any,
      params: IGetSurveysRequest = {
        type: this.queryType !== '' ? this.queryType : 'service_and_completion',
        planSurveyResponseDateFrom: parseInt(
          format(getFiscalYearStart(), 'yyyyMMdd')
        ),
        planSurveyResponseDateTo: parseInt(
          format(getFiscalYearEnd(), 'yyyyMMdd')
        ),
        projectId: '',
        customerId: '',
        organizationIds: '',
        serviceTypeId: '',
        isFinished: true,
      }
    ) {
      this.clearErrorBar()
      this.isLoading.surveys = true
      await GetSurveys(params)
        .then((res) => {
          this.surveysResponse = res.data
          this.total = this.surveysResponse.surveys.length
          this.offsetPage = 1
          if (Number(this.total) === 0) {
            this.maxPage = 1
          } else {
            this.maxPage = Math.ceil(
              this.surveysResponse.surveys.length / this.limit
            )
          }
          if (this.total <= 0) {
            this.showErrorBarWithScrollPageTop(
              this.$t('msg.error.csvExportDisabled')
            )
          }
        })
        .finally(() => {
          this.isLoading.surveys = false
        })
    },
    /**
     * getServiceTypesAPIを叩き、サービス種別の一覧を取得して代入。
     */
    async getServiceTypes() {
      this.isLoading.serviceTypesResponse = true

      await GetServiceTypes().then((res) => {
        this.serviceTypesResponse = res.data
        this.serviceTypesResponse.serviceTypes.unshift({
          id: '',
          name: this.$t('survey.pages.list.sort_input.serviceNameItems.all'),
        })
        this.isLoading.serviceTypesResponse = false
      })
    },
    /**
     * getSupporterOrganizationsAPIを叩き、支援者組織の一覧を取得して代入。
     */
    async getSupporterOrganizations() {
      this.isLoading.supporterOrganizationsResponse = true

      await GetSupporterOrganizations().then((res) => {
        this.supporterOrganizationsResponse = res.data
        this.isLoading.supporterOrganizationsResponse = false
      })
    },
    /**
     * suggestProjectsAPIを叩き、案件のサジェスト用情報を取得して代入。
     * @param params suggestProjectsAPIのリクエストパラメータを指定
     */
    async getSuggestProjects(
      params: SuggestProjectsRequest = new SuggestProjectsRequest()
    ) {
      this.isLoading.suggestProjects = true
      await GetSuggestProjects(params).then((res) => {
        this.suggestProjects = res.data
        this.isLoading.suggestProjects = false
      })
    },
    /**
     * suggestCustomersAPIを叩き、取引先のサジェスト用情報を取得して代入。
     * @param params suggestCustomersAPIのリクエストパラメータを指定
     */
    async getSuggestCustomers(
      params: SuggestCustomersRequest = new SuggestCustomersRequest()
    ) {
      this.isLoading.suggestCustomers = true
      await SuggestCustomers(params).then((res) => {
        this.suggestCustomers = res.data
        this.isLoading.suggestCustomers = false
      })
    },
    /**
     * exportSurveysAPIを叩き、アンケートデータCSVを出力。
     * @param params exportSurveysAPIのリクエストパラメータを指定
     */
    async exportSurveys(
      this: any,
      params: IExportSurveysRequest = {
        type: this.queryType !== '' ? this.queryType : 'service_and_completion',
        summaryMonthFrom: 0,
        summaryMonthTo: 0,
        projectId: '',
        organizationIds: '',
        mode: '',
      }
    ) {
      const fileName =
        this.$t(`survey.pages.list.formatName`) +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      await ExportSurveys(params)
        .then((res) => {
          if (res.data && res.data.url) {
            const url = res.data.url
            const link = document.createElement('a')
            link.download = fileName
            link.href = url
            document.body.appendChild(link)
            link.click()
            document.body.removeChild(link)
          }
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
  },
})
</script>
