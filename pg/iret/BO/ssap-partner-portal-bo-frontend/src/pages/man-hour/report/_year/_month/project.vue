<template>
  <TemplateMonthlyProjectsList
    :response="monthlyProjects"
    :is-loading="isLoading"
    @csvOutput="csvOutput"
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
import TemplateMonthlyProjectsList, {
  isLoading,
} from '~/components/man-hour/templates/MonthlyProjectsList.vue'
import {
  GetMonthlyProjectsResponse,
  GetMonthlyProjects,
} from '@/models/Project'

export default BasePage.extend({
  name: 'ManHourReportProject',
  middleware: ['roleCheck'],
  components: {
    TemplateMonthlyProjectsList,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.report.project.name') as string,
    }
  },
  data(): {
    monthlyProjects: GetMonthlyProjectsResponse
    isLoading: isLoading
  } {
    return {
      monthlyProjects: new GetMonthlyProjectsResponse(),
      isLoading: {
        monthlyProjects: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getMonthlyProjects()])
  },
  methods: {
    /**
     * GetMonthlyProjectsAPIを叩いて、月次案件情報を取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    async getMonthlyProjects() {
      this.clearErrorBar()
      this.isLoading.monthlyProjects = true
      const params = {
        year: this.$route.params.year,
        month: this.$route.params.month,
      }
      await GetMonthlyProjects(params.year, params.month)
        .then((res) => {
          this.monthlyProjects = res.data
          if (this.monthlyProjects.details.length <= 0) {
            this.showErrorBarWithScrollPageTop(
              this.$t('msg.error.csvExportDisabled')
            )
          }
          this.isLoading.monthlyProjects = false
        })
        .catch((error) => {
          if (error.status && error.status === 404) {
            this.showErrorBarWithScrollPageTop(
              this.$t('msg.error.csvExportDisabled')
            )
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
          this.isLoading.monthlyProjects = false
        })
    },
    /**
     * CSV出力
     */
    csvOutput() {
      const parentHeaders = ['', '', '', '', '']
      const beginningHeaders = {
        serviceTypeName: this.$t(
          'man-hour.pages.report.project.header.serviceTypeName'
        ),
        name: this.$t('man-hour.pages.report.project.header.name'),
        contractType: this.$t(
          'man-hour.pages.report.project.header.contractType'
        ),
        mainSalesUserName: this.$t(
          'man-hour.pages.report.project.header.mainSalesUserName'
        ),
      }
      const remainingHeaders = {
        thisMonthContractTime: this.$t(
          'man-hour.pages.report.project.header.thisMonthContractTime'
        ),
        thisMonthProfit: this.$t(
          'man-hour.pages.report.project.header.thisMonthProfit'
        ),
        supportDateFrom: this.$t(
          'man-hour.pages.report.project.header.supportDateFrom'
        ),
        supportDateTo: this.$t(
          'man-hour.pages.report.project.header.supportDateTo'
        ),
        totalProfit: this.$t(
          'man-hour.pages.report.project.header.totalProfit'
        ),
        totalContractTime: this.$t(
          'man-hour.pages.report.project.header.totalContractTime'
        ),
      }

      this.monthlyProjects.header.map((elm: any) => {
        const parentSupOrgsHeaders = [elm.supporterOrganizationName, '', '']
        const supOrgsHeader: any = {}

        supOrgsHeader[elm.supporterOrganizationId + 'Producer'] = this.$t(
          'man-hour.pages.report.project.header.producer'
        )
        supOrgsHeader[elm.supporterOrganizationId + 'Accelerator'] = this.$t(
          'man-hour.pages.report.project.header.accelerator'
        )
        supOrgsHeader[elm.supporterOrganizationId + 'Total'] = this.$t(
          'man-hour.pages.report.project.header.total'
        )
        Object.assign(beginningHeaders, supOrgsHeader)
        return parentHeaders.push(...parentSupOrgsHeaders)
      })

      let parentHeaderContent = ''
      for (const i in parentHeaders) {
        parentHeaderContent += '"' + String(parentHeaders[i]) + '",'
      }
      parentHeaderContent += '\r\n'

      const headers = {
        ...beginningHeaders,
        ...remainingHeaders,
      }
      const items = []
      for (const i in this.monthlyProjects.details) {
        const project: any = this.monthlyProjects.details[i]
        const row = {
          serviceTypeName: project.serviceTypeName,
          name: project.name,
          contractType: project.contractType,
          mainSalesUserName: project.mainSalesUserName,
          thisMonthContractTime: project.thisMonthContractTime,
          thisMonthProfit: project.thisMonthProfit,
          supportDateFrom: project.supportDateFrom,
          supportDateTo: project.supportDateTo,
          totalProfit: project.totalProfit,
          totalContractTime: project.totalContractTime,
        }
        const organizationsRow: any[] = []
        project.organizations.map((elm: any) => {
          organizationsRow[
            (elm.supporterOrganizationId + 'Total') as typeof elm
          ] = elm.members.length
          const producerName: string[] = []
          const acceleratorName: string[] = []

          elm.members.map((e: any) => {
            if (e.role === 'プロデューサー') {
              producerName.push(e.supporterName)
              return (organizationsRow[
                (elm.supporterOrganizationId + 'Producer') as typeof elm
              ] = producerName.join('、'))
            }
            acceleratorName.push(e.supporterName)
            return (organizationsRow[
              (elm.supporterOrganizationId + 'Accelerator') as typeof elm
            ] = acceleratorName.join('、'))
          })
          return Object.assign(row, organizationsRow)
        })
        items.push(row)
      }
      const content = parentHeaderContent + dataToCsv(items, headers)
      const fileName =
        this.$t('man-hour.pages.report.project.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
  },
})
</script>
