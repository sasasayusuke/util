<template>
  <TemplateSurveyMonthlyReport
    v-if="!isLoading"
    :survey-summary-report="surveySummaryReport"
    :is-loading="isLoading"
    @getSurveySummaryReport="getSurveySummaryReport"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateSurveyMonthlyReport from '~/components/survey/templates/SurveyMonthlyReport.vue'
import {
  GetSurveySummaryReportResponse,
  GetSurveySummaryReport,
} from '~/models/Survey'
import { IGetSurveySummaryReportRequest } from '~/types/Survey'

export default BasePage.extend({
  name: 'SurveyReportMonthly',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyMonthlyReport,
  },
  head() {
    return {
      title: this.$t('survey.pages.monthly.name') as string,
    }
  },
  data() {
    return {
      surveySummaryAll: {},
      surveySummaryReport: new GetSurveySummaryReportResponse(),
      isLoading: true,
    }
  },
  mounted() {
    this.displayLoading([this.getSurveySummaryReport()])
  },
  methods: {
    /**
     * getSurveySummaryReportAPIを叩き、アンケート月次レポートを取得して代入。
     * @param params getSurveySummaryReportAPIのリクエストパラメータを指定
     */
    async getSurveySummaryReport(
      params: IGetSurveySummaryReportRequest = {
        summaryMonth: parseInt(format(getCurrentDate(), 'yyyyMM')),
      }
    ) {
      this.isLoading = true
      if (
        params.summaryMonth != null &&
        !Number.isInteger(params.summaryMonth)
      ) {
        params.summaryMonth = parseInt(
          format(new Date(params.summaryMonth), 'yyyyMM')
        )
      }
      await GetSurveySummaryReport(params).then((res) => {
        this.surveySummaryReport = res.data
        this.isLoading = false
      })
    },
  },
})
</script>
