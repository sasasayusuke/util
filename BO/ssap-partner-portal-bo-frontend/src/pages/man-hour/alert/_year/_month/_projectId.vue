<template>
  <TemplateManHourAlertProjectDetail
    :summary-project-man-hour-alerts="summaryProjectManHourAlerts"
    :alert-settings="alertSettings"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateManHourAlertProjectDetail from '~/components/man-hour/templates/ManHourAlertProjectDetail.vue'
import { GetAlertSettings } from '~/models/Master'
import {
  getSummaryProjectManHourAlertDetail,
  GetSummaryProjectManHourAlertDetailResponse,
  GetSummaryProjectManHourAlertDetailRequest,
} from '~/models/ManHour'

export default BasePage.extend({
  name: 'ManHourAlertProjectDetail',
  components: {
    TemplateManHourAlertProjectDetail,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.alertDetail.name') as string,
    }
  },
  data() {
    return {
      year: 0,
      month: 0,
      alertSettings: {},
      projectId: '',
      summaryProjectManHourAlerts:
        {} as GetSummaryProjectManHourAlertDetailResponse,
    }
  },
  mounted() {
    this.getProjectId()
    this.getYearMonth()
    this.displayLoading([
      this.getAlertSetting(),
      this.getSummaryProjectManHourAlertDetail(),
    ])
  },
  methods: {
    /**
     * ルートパラメータから年月を取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    getYearMonth() {
      this.year = this.$route.params.year
      this.month = this.$route.params.month
    },
    /**
     * ルートパラメータから案件IDを取得
     * @param {string} projectId ルートパラメータから取得した案件ID
     */
    getProjectId() {
      this.projectId = this.$route.params.projectId
    },
    /**
     * GetSummaryProjectManHourAlertAPIを叩いて、案件指定の工数状況とアラートを取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     * @param {string} projectId ルートパラメータから取得した案件ID
     */
    async getSummaryProjectManHourAlertDetail() {
      const params: GetSummaryProjectManHourAlertDetailRequest = {
        year: this.year,
        month: this.month,
        projectId: this.projectId,
      }
      await getSummaryProjectManHourAlertDetail(params).then((res) => {
        this.summaryProjectManHourAlerts = res.data
      })
    },
    /**
     * GetAlertSettingAPIを叩いて、工数アラート設定を取得
     */
    async getAlertSetting() {
      await GetAlertSettings().then((res: any) => {
        this.alertSettings = res.data
      })
    },
  },
})
</script>
