<template>
  <TemplateManHourAlertList
    :param="searchParam"
    :year="year"
    :month="month"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :total="total"
    :total-contract-time="totalContractTime"
    :summary-project-man-hour-alerts="summaryProjectManHourAlerts"
    :alert-settings="alertSettings"
    :supporter-organizations="supporterOrganizations"
    :service-types="serviceTypes"
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
import TemplateManHourAlertList, {
  SearchParam,
} from '~/components/man-hour/templates/AlertList.vue'
import { GetSummaryProjectManHourAlerts } from '@/models/ManHour'
import { IgetSummaryProjectManHourAlertsResponse } from '@/types/ManHour'
import {
  GetServiceTypes,
  GetSupporterOrganizations,
  GetAlertSettings,
} from '~/models/Master'
import { IServiceTypeItems, ISupporterOrganizationItems } from '~/types/Master'

export default BasePage.extend({
  name: 'ManHourAlertList',
  middleware: ['roleCheck'],
  components: {
    TemplateManHourAlertList,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.alert.list.name') as string,
    }
  },
  data() {
    return {
      offsetPage: 1,
      maxPage: 10,
      total: 0,
      totalContractTime: 0,
      summaryProjectManHourAlerts:
        {} as IgetSummaryProjectManHourAlertsResponse,
      alertSettings: {},
      supporterOrganizations: [] as ISupporterOrganizationItems[],
      serviceTypes: [] as IServiceTypeItems[],
      supporterOrganizationId: '',
      serviceTypeId: '',
      year: 0,
      month: 0,
      searchParam: new SearchParam(),
    }
  },
  mounted() {
    this.getCurrentDate()
    this.displayLoading([
      this.getAlertSetting(),
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
     * 延べ契約時間の合計を計算
     * @param {number} totalContractTime 延べ契約時間の合計
     */
    getTotalContractTime() {
      let totalContractTime = 0
      this.summaryProjectManHourAlerts.projects.forEach((project: any) => {
        totalContractTime += project.totalContractTime
      })
      this.totalContractTime = totalContractTime
    },
    /**
     * 案件の合計数を計算
     * @param {number} totalContractTime 案件の合計数
     */
    getTotalNumberOfProject() {
      let totalNumberOfProject = 0
      this.summaryProjectManHourAlerts.projects.forEach(() => {
        totalNumberOfProject += 1
      })
      this.total = totalNumberOfProject
    },
    /**
     * GetSummaryProjectManHourAlertsAPIを叩いて、案件毎の工数状況とアラート一覧を取得
     * @param {number} year getCurrentDateで取得した現在の年 || 選択された年
     * @param {number} month getCurrentDateで取得した現在の月 || 選択された月
     * @param {string} supporterOrganizationId 支援者組織ID
     * @param {string} serviceTypeId サービス種別ID
     */
    async getSummaryProjectManHourAlerts() {
      this.clearErrorBar()
      const params = {
        year: this.year,
        month: this.month,
        supporterOrganizationId: this.supporterOrganizationId,
        serviceTypeId: this.serviceTypeId,
      }
      await GetSummaryProjectManHourAlerts(params).then((res) => {
        this.summaryProjectManHourAlerts = res.data
        this.getTotalContractTime()
        this.getTotalNumberOfProject()
        if (this.total <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
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
    /**
     * GetSupporterOrganizationsAPIを叩いて、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await GetSupporterOrganizations().then((res) => {
        this.supporterOrganizations = res.data.supporterOrganizations
      })
    },
    /**
     * GetServiceTypesAPIを叩いて、サービス種別の一覧を取得
     */
    async getServiceTypes() {
      await GetServiceTypes().then((res) => {
        this.serviceTypes = res.data.serviceTypes
      })
    },
    /**
     * CSV出力
     */
    exportCsv() {
      const parentHeaders = [
        '',
        '',
        '',
        '',
        '',
        '',
        '',
        this.$t('man-hour.pages.alert.list.header.summarySupportManHour'),
        '',
        this.$t('man-hour.pages.alert.list.header.thisMonthSupportManHour'),
        '',
        this.$t(
          'man-hour.pages.alert.list.header.summaryTheoreticalSupportManHour'
        ),
        '',
        this.$t(
          'man-hour.pages.alert.list.header.thisMonthTheoreticalSupportManHour'
        ),
        '',
        this.$t('man-hour.pages.alert.list.header.alertSupportManHour'),
        '',
      ]
      let parentHeaderContent = ''
      for (const i in parentHeaders) {
        parentHeaderContent += '"' + String(parentHeaders[i]) + '",'
      }
      parentHeaderContent += '\r\n'

      const headers = {
        projectName: this.$t('man-hour.pages.alert.list.header.projectName'),
        customerName: this.$t('man-hour.pages.alert.list.header.customerName'),
        serviceTypeName: this.$t(
          'man-hour.pages.alert.list.header.serviceTypeName'
        ),
        supportDateFromTo: this.$t(
          'man-hour.pages.alert.list.header.supportDateFromTo'
        ),
        totalContractTime: this.$t(
          'man-hour.pages.alert.list.header.totalContractTime'
        ),
        mainSupporterUserName: this.$t(
          'man-hour.pages.alert.list.header.mainSupporterUserName'
        ),
        supporterUsersName: this.$t(
          'man-hour.pages.alert.list.header.supporterUsersName'
        ),
        summaryDirectSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.summaryDirectSupportManHour'
        ),
        summaryPreSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.summaryPreSupportManHour'
        ),
        summaryTheoreticalDirectSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.summaryTheoreticalDirectSupportManHour'
        ),
        summaryTheoreticalPreSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.summaryTheoreticalPreSupportManHour'
        ),
        thisMonthDirectSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.thisMonthDirectSupportManHour'
        ),
        thisMonthPreSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.thisMonthPreSupportManHour'
        ),
        thisMonthTheoreticalDirectSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.thisMonthTheoreticalDirectSupportManHour'
        ),
        thisMonthTheoreticalPreSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.thisMonthTheoreticalPreSupportManHour'
        ),
        alertDirectSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.alertDirectSupportManHour'
        ),
        alertPreSupportManHour: this.$t(
          'man-hour.pages.alert.list.header.alertPreSupportManHour'
        ),
      }
      const items = []
      for (const i in this.summaryProjectManHourAlerts.projects) {
        const alert: any = this.summaryProjectManHourAlerts.projects[i]
        const producerName =
          alert.mainSupporterUser && alert.mainSupporterUser.name
            ? alert.mainSupporterUser.name
            : ''
        const accelerators = []
        if (alert.supporterUsers !== null && alert.supporterUsers.length) {
          for (const i2 in alert.supporterUsers) {
            if (alert.supporterUsers[i2].name) {
              accelerators.push(alert.supporterUsers[i2].name)
            }
          }
        }
        let alertDirectSupportManHour = ''
        if (alert.alertDirectSupportManHour === 0) {
          alertDirectSupportManHour = ''
        } else if (alert.alertDirectSupportManHour === 1) {
          alertDirectSupportManHour = this.$t(
            'common.label.alertProspectDirectSupportManHour'
          )
        } else if (alert.alertDirectSupportManHour === 2) {
          alertDirectSupportManHour = this.$t(
            'common.label.alertDirectSupportManHour'
          )
        }
        let alertPreSupportManHour = ''
        if (alert.alertPreSupportManHour === 0) {
          alertPreSupportManHour = ''
        } else if (alert.alertPreSupportManHour === 1) {
          alertPreSupportManHour = this.$t(
            'common.label.alertPreSupportManHour'
          )
        } else if (alert.alertPreSupportManHour === 2) {
          alertPreSupportManHour = this.$t(
            'common.label.alertPreSupportManHour'
          )
        }
        const acceleratorName = accelerators.join('\n')
        const row = {
          projectName: alert.projectName,
          customerName: alert.customerName,
          serviceTypeName: alert.serviceTypeName,
          supportDateFromTo: alert.supportDateFrom + '～' + alert.supportDateTo,
          totalContractTime: alert.totalContractTime,
          mainSupporterUserName: producerName,
          supporterUsersName: acceleratorName,
          summaryDirectSupportManHour: alert.summaryDirectSupportManHour,
          summaryPreSupportManHour: alert.summaryPreSupportManHour,
          thisMonthDirectSupportManHour:
            alert.summaryTheoreticalDirectSupportManHour,
          thisMonthPreSupportManHour: alert.summaryTheoreticalPreSupportManHour,
          summaryTheoreticalDirectSupportManHour:
            alert.thisMonthDirectSupportManHour,
          summaryTheoreticalPreSupportManHour: alert.thisMonthPreSupportManHour,
          thisMonthTheoreticalDirectSupportManHour:
            alert.thisMonthTheoreticalDirectSupportManHour,
          thisMonthTheoreticalPreSupportManHour:
            alert.thisMonthTheoreticalPreSupportManHour,
          alertDirectSupportManHour,
          alertPreSupportManHour,
        }
        items.push(row)
      }
      const content = parentHeaderContent + dataToCsv(items, headers)
      const fileName =
        this.$t('man-hour.pages.alert.list.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
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
    onClickPrev() {},
    onClickNext() {},
  },
})
</script>
