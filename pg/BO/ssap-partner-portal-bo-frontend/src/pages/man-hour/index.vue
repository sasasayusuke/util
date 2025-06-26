<template>
  <TemplateManHourList
    :param="searchParam"
    :year="year"
    :month="month"
    :supporter-organizations="supporterOrganizations"
    :service-types="serviceTypes"
    :response="summaryProjectManHourAlerts"
    :length="length"
    :is-loading="isLoading"
    @update="update"
    @sort="sort"
    @clear="clear"
    @exportCsv="exportCsv"
    @click:prev="onClickPrev"
    @click:next="onClickNext"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import {
  getCurrentDate,
  dataToCsv,
  downloadFile,
} from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateManHourList, {
  SearchParam,
} from '~/components/man-hour/templates/ManHourList.vue'
import { GetSummaryProjectManHourAlerts } from '@/models/ManHour'
import { GetServiceTypes, GetSupporterOrganizations } from '~/models/Master'
import { IServiceTypeItems, ISupporterOrganizationItems } from '~/types/Master'
import { IgetSummaryProjectManHourAlertsResponse } from '@/types/ManHour'

export default BasePage.extend({
  name: 'ManHourIndex',
  middleware: ['roleCheck'],
  components: {
    TemplateManHourList,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.index.name') as string,
    }
  },
  data() {
    return {
      supporterOrganizations: [] as ISupporterOrganizationItems[],
      serviceTypes: [] as IServiceTypeItems[],
      summaryProjectManHourAlerts:
        {} as IgetSummaryProjectManHourAlertsResponse,
      supporterOrganizationId: '',
      serviceTypeId: '',
      year: 0,
      month: 0,
      searchParam: new SearchParam(),
      length: 0,
      isLoading: {
        supporterOrganizations: true,
        serviceTypes: true,
        summaryProjectManHourAlerts: true,
      },
    }
  },
  mounted() {
    this.getCurrentDate()
    this.displayLoading([
      this.getSupporterOrganizations(),
      this.getServiceTypes(),
      this.getSummaryProjectManHourAlerts(),
    ])
  },
  methods: {
    /**
     * 現在の年月を取得
     * @param {number} year 現在の年
     * @param {number} month 現在の月
     */
    getCurrentDate() {
      const date = getCurrentDate()
      this.year = date.getFullYear()
      this.month = date.getMonth() + 1
    },
    /**
     * GetSupporterOrganizationsAPIを叩いて、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations().then((res) => {
        this.supporterOrganizations = res.data.supporterOrganizations
        this.isLoading.supporterOrganizations = false
      })
    },
    /**
     * GetServiceTypesAPIを叩いて、サービス種別の一覧を取得
     */
    async getServiceTypes() {
      await GetServiceTypes().then((res) => {
        this.serviceTypes = res.data.serviceTypes
        this.isLoading.serviceTypes = false
      })
    },
    /**
     * GetSummaryProjectManHourAlertsAPIを叩いて、案件毎の工数状況とアラート一覧を取得
     * @param {number} year getCurrentDateで取得した現在の年 || 選択された年
     * @param {number} month getCurrentDateで取得した現在の月 || 選択された月
     * @param {string} supporterOrganizationId 支援者組織ID
     * @param {string} serviceTypeId サービス種別ID
     */
    async getSummaryProjectManHourAlerts() {
      const params = {
        year: this.year,
        month: this.month,
        supporterOrganizationId: this.supporterOrganizationId,
        serviceTypeId: this.serviceTypeId,
      }
      this.clearErrorBar()
      await GetSummaryProjectManHourAlerts(params).then((res) => {
        this.summaryProjectManHourAlerts = res.data
        this.length = this.summaryProjectManHourAlerts.projects.length
        if (this.length <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
        this.isLoading.summaryProjectManHourAlerts = false
      })
    },
    update(item: keyof SearchParam, event: any) {
      this.searchParam[item] = event
    },
    /**
     * 指定条件で絞り込み
     * @param {number} year 選択された年
     * @param {number} month 選択された月
     * @param {string} supporterOrganizationId 選択された支援者組織ID
     * @param {string} serviceTypeId 選択されたサービス種別ID
     */
    sort() {
      const strYearMonth = this.searchParam.yearMonth
      const intYear = parseInt(format(new Date(strYearMonth), 'yyyy'))
      const intMonth = parseInt(format(new Date(strYearMonth), 'MM'))

      // paramsはGetSummaryProjectManHourAlertsの検索条件
      this.year = intYear
      this.month = intMonth

      // supporterOrganizationIdがparamsに含まれていれば、選択したすべてのsupporterOrganizationIdをコンマ区切りの文字列にする
      if (this.searchParam.supporterOrganizationId) {
        const supporterOrganizationIds: string[] = []
        this.searchParam.supporterOrganizationId.forEach(
          (supporterOrganization: any) => {
            supporterOrganizationIds.push(supporterOrganization)
          }
        )
        this.supporterOrganizationId = supporterOrganizationIds.join(',')
      }

      // serviceTypeIdがparamsに含まれていれば、選択したすべてのserviceTypeIdをコンマ区切りの文字列にする
      if (this.searchParam.serviceTypeId) {
        const serviceTypeIds: string[] = []
        this.searchParam.serviceTypeId.forEach((serviceType: any) => {
          serviceTypeIds.push(serviceType)
        })
        this.serviceTypeId = serviceTypeIds.join(',')
      }

      this.getSummaryProjectManHourAlerts()
    },
    /**
     * 初期状態である「当月、課=すべて、支援タイプ=すべて」で絞り込み
     */
    clear() {
      this.getCurrentDate()
      this.supporterOrganizationId = ''
      this.serviceTypeId = ''
      this.searchParam = new SearchParam()
      this.sort()
    },
    /**
     * CSV出力
     */
    exportCsv() {
      const headers = {
        serviceTypeName: this.$t('man-hour.pages.index.header.serviceTypeName'),
        projectName: this.$t('man-hour.pages.index.header.projectName'),
        customerName: this.$t('man-hour.pages.index.header.customerName'),
        supporterOrganizationName: this.$t(
          'man-hour.pages.index.header.supporterOrganizationName'
        ),
        supportDateFromTo: this.$t(
          'man-hour.pages.index.header.supportDateFrom'
        ),
        totalContractTime:
          this.$t('man-hour.pages.index.header.totalContractTime1') +
          this.$t('man-hour.pages.index.header.totalContractTime2'),
        thisMonthContractTime:
          this.$t('man-hour.pages.index.header.thisMonthContractTime1') +
          this.$t('man-hour.pages.index.header.thisMonthContractTime2'),
        totalProfit:
          this.$t('man-hour.pages.index.header.totalProfit1') +
          this.$t('man-hour.pages.index.header.totalProfit2'),
        thisMonthProfit:
          this.$t('man-hour.pages.index.header.thisMonthProfit1') +
          this.$t('man-hour.pages.index.header.thisMonthProfit2'),
        summaryDirectSupportManHour:
          this.$t('man-hour.pages.index.header.summaryDirectSupportManHour1') +
          this.$t('man-hour.pages.index.header.summaryDirectSupportManHour2'),
        summaryPreSupportManHour:
          this.$t('man-hour.pages.index.header.summaryPreSupportManHour1') +
          this.$t('man-hour.pages.index.header.summaryPreSupportManHour2'),
      }
      const items = []
      for (const i in this.summaryProjectManHourAlerts.projects) {
        const project = this.summaryProjectManHourAlerts.projects[i]
        const filteredTotalProfit = project.totalProfit.replace('¥', '')
        const filteredThisMonthProfit = project.thisMonthProfit
          .replace('¥', '')
          .replace('/月', '')
        const filteredTotalContractTime = project.totalContractTime.replace(
          'h',
          ''
        )
        const filteredThisMonthContractTime =
          project.thisMonthContractTime.replace('h/月', '')

        const filteredSummaryDirectSupportManHour =
          project.summaryDirectSupportManHour.replace('h', '')

        const filteredSummaryPreSupportManHour =
          project.summaryPreSupportManHour.replace('h', '')

        const row = {
          serviceTypeName: project.serviceTypeName,
          projectName: project.projectName,
          customerName: project.customerName,
          supporterOrganizationName: project.supporterOrganizationName,
          supportDateFromTo:
            project.supportDateFrom + '～' + project.supportDateTo,
          totalContractTime: filteredTotalContractTime,
          thisMonthContractTime: filteredThisMonthContractTime,
          totalProfit: filteredTotalProfit,
          thisMonthProfit: filteredThisMonthProfit,
          summaryDirectSupportManHour: filteredSummaryDirectSupportManHour,
          summaryPreSupportManHour: filteredSummaryPreSupportManHour,
        }
        items.push(row)
      }
      const content = dataToCsv(items, headers)
      const fileName =
        this.$t('man-hour.pages.index.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
    onClickPrev() {},
    onClickNext() {},
  },
})
</script>
