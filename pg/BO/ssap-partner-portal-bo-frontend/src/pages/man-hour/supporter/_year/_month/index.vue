<template>
  <TemplateMonthlyManHourBySupporterList
    :response="response"
    :supporter-organizations="supporterOrganizations"
    :batch-control="batchControl"
    :is-loading="isLoading"
    @getSummarySupporterManHours="getSummarySupporterManHours"
    @getSummarySupporterOrganizationsManHours="
      getSummarySupporterOrganizationsManHours
    "
    @csvOutput="csvOutput"
  />
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import { format } from 'date-fns'
import {
  getCurrentDate,
  dataToCsv,
  downloadFile,
} from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateMonthlyManHourBySupporterList, {
  isLoading,
} from '~/components/man-hour/templates/MonthlyManHourBySupporterList.vue'
import {
  GetSummarySupporterManHoursRequest,
  GetSummarySupporterManHoursResponse,
  GetSummarySupporterManHours,
  GetSummaryServiceTypesManHoursRequest,
  GetSummaryServiceTypesManHoursResponse,
  GetSummaryServiceTypesManHours,
  GetSummarySupporterOrganizationsManHoursRequest,
  GetSummarySupporterOrganizationsManHourResponse,
  GetSummarySupporterOrganizationsManHours,
} from '@/models/ManHour'
import {
  GetSupporterOrganizationsResponse,
  GetSupporterOrganizations,
  GetBatchControlByIdResponse,
  GetBatchControlById,
  GetAlertSettings,
  GetAlertSettingsResponse,
} from '@/models/Master'

export default BasePage.extend({
  name: 'ManHourSupporterIndex',
  middleware: ['roleCheck'],
  components: {
    TemplateMonthlyManHourBySupporterList,
  },

  head() {
    return {
      title: this.$t('man-hour.pages.supporter.index.name') as string,
    }
  },

  data(): {
    supporterOrganizations: GetSupporterOrganizationsResponse
    exportManHours: any
    batchControl: GetBatchControlByIdResponse
    isLoading: isLoading
    response: {
      supporterManHours: GetSummarySupporterManHoursResponse
      serviceTypesManHours: GetSummaryServiceTypesManHoursResponse[]
      supporterOrganizationsManHours: GetSummarySupporterOrganizationsManHourResponse[]
      alertSettings: GetAlertSettingsResponse
    }
  } {
    return {
      supporterOrganizations: new GetSupporterOrganizationsResponse(),
      exportManHours: {} as any,
      batchControl: new GetBatchControlByIdResponse(),
      isLoading: {
        supporterOrganizations: true,
        supporterManHours: true,
        serviceTypesManHours: true,
        supporterOrganizationsManHours: true,
        batchControl: true,
        alertSettings: true,
      },
      response: {
        supporterManHours: new GetSummarySupporterManHoursResponse(),
        serviceTypesManHours: [] as any[],
        supporterOrganizationsManHours: [] as any[],
        alertSettings: new GetAlertSettingsResponse(),
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.getSupporterOrganizations(),
      this.getSummarySupporterManHours(),
      this.getSummaryServiceTypesManHours(),
      this.getSummarySupporterOrganizationsManHours(),
      this.getBatchControlById(),
      this.getAlertSettings(),
    ])
  },
  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year(this: any) {
      return parseInt(this.$route.params.year)
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month(this: any) {
      return parseInt(this.$route.params.month)
    },
    /**
     * GetSupporterOrganizationsAPIを叩いて、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations().then((res) => {
        this.supporterOrganizations = res.data
        this.isLoading.supporterOrganizations = false
      })
    },
    /**
     * GetSummarySupporterManHoursAPIを叩いて、月次担当者別工数一覧を取得
     * (担当者別月次工数集計で利用)
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    async getSummarySupporterManHours(
      params: GetSummarySupporterManHoursRequest = new GetSummarySupporterManHoursRequest()
    ) {
      this.clearErrorBar()
      params.year = this.year()
      params.month = this.month()
      await GetSummarySupporterManHours(params).then((res) => {
        this.exportManHours = cloneDeep(res.data)
        this.response.supporterManHours = res.data
        if (this.response.supporterManHours.manHours.length <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
        this.isLoading.supporterManHours = false
      })
    },
    /**
     * GetSummaryServiceTypesManHoursAPIを叩いて、サービス種別別工数指標を取得
     * (月別実績 案件別契約時間と直接寄与工数（対面時間）内訳 描画用)
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    async getSummaryServiceTypesManHours(
      params: GetSummaryServiceTypesManHoursRequest = new GetSummaryServiceTypesManHoursRequest()
    ) {
      params.year = this.year()
      params.month = this.month()
      await GetSummaryServiceTypesManHours(params).then((res) => {
        this.response.serviceTypesManHours = res.data
        this.isLoading.serviceTypesManHours = false
      })
    },
    /**
     * GetSummarySupporterOrganizationsManHoursAPIを叩いて、支援者組織（課）別工数指標を取得
     * (月別実績 Sony Acceleration Platform粗利シミレーションとキーとなる指標用)
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    async getSummarySupporterOrganizationsManHours(
      params: GetSummarySupporterOrganizationsManHoursRequest = new GetSummarySupporterOrganizationsManHoursRequest()
    ) {
      params.year = this.year()
      params.month = this.month()
      await GetSummarySupporterOrganizationsManHours(params).then((res) => {
        this.response.supporterOrganizationsManHours = res.data
        this.isLoading.supporterOrganizationsManHours = false
      })
    },
    /**
     * GetBatchControlByIdAPIを叩いて、各種集計バッチ処理の最終完了日時を取得
     * @param {string} batchControlId バッチ処理関数ID
     */
    async getBatchControlById() {
      const batchControlId = encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      )
      await GetBatchControlById(String(batchControlId)).then((res) => {
        this.batchControl = res.data
        this.isLoading.batchControl = false
      })
    },
    /**
     * GetAlertSettingAPIを叩いて、工数アラート設定を取得
     */
    async getAlertSettings() {
      await GetAlertSettings().then((res: any) => {
        this.response.alertSettings = res.data.attributes
        this.isLoading.alertSettings = false
      })
    },
    /**
     * CSV出力
     */
    csvOutput() {
      const headers = {
        supporterOrganizationName: this.$t(
          'man-hour.pages.supporter.header.supporterOrganizationName'
        ),
        supporterName: this.$t('man-hour.pages.supporter.header.supporterName'),
        isConfirm: this.$t('man-hour.pages.supporter.header.isConfirm'),
        summaryManHourDirect: this.$t(
          'man-hour.pages.supporter.header.summaryManHourDirect'
        ),
        summaryManHourPre: this.$t(
          'man-hour.pages.supporter.header.summaryManHourPre'
        ),
        summaryManHourSales: this.$t(
          'man-hour.pages.supporter.header.summaryManHourSales'
        ),
        summaryManHourSsap: this.$t(
          'man-hour.pages.supporter.header.summaryManHourSsap'
        ),
        summaryManHourOthers: this.$t(
          'man-hour.pages.supporter.header.summaryManHourOthers'
        ),
        summaryManHourTotal: this.$t(
          'man-hour.pages.supporter.header.summaryManHourTotal'
        ),
        contractTimeMain: this.$t(
          'man-hour.pages.supporter.header.contractTimeMain'
        ),
        contractTimeOther: this.$t(
          'man-hour.pages.supporter.header.contractTimeOther'
        ),
        contractTimeTotal: this.$t(
          'man-hour.pages.supporter.header.contractTimeTotal'
        ),
      }
      const items = []
      for (const i in this.exportManHours.manHours) {
        const supporter = this.exportManHours.manHours[i]
        const row = {
          supporterOrganizationName: supporter.supporterOrganizationName,
          supporterName: supporter.supporterName,
          isConfirm: supporter.isConfirm
            ? null
            : this.$t('common.label.unsettled'),
          summaryManHourDirect: String(supporter.summaryManHour.direct) + 'h',
          summaryManHourPre: String(supporter.summaryManHour.pre) + 'h',
          summaryManHourSales: String(supporter.summaryManHour.sales) + 'h',
          summaryManHourSsap: String(supporter.summaryManHour.ssap) + 'h',
          summaryManHourOthers: String(supporter.summaryManHour.others) + 'h',
          summaryManHourTotal: String(supporter.summaryManHour.total) + 'h',
          contractTimeMain: String(supporter.contractTime.producer) + 'h',
          contractTimeOther: String(supporter.contractTime.accelerator) + 'h',
          contractTimeTotal: String(supporter.contractTime.total) + 'h',
        }
        items.push(row)
      }
      const content = dataToCsv(items, headers)
      const fileName =
        this.$t('man-hour.pages.supporter.index.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
  },
})
</script>
