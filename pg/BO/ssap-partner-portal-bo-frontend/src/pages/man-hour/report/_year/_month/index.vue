<template>
  <TemplateSummaryManHourByTypeList
    :response="manHours"
    :is-loading="isLoading"
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
import TemplateSummaryManHourByTypeList, {
  isLoading,
} from '~/components/man-hour/templates/SummaryManHourByTypeList.vue'
import {
  GetSummaryManHourTypeResponse,
  GetSummaryManHourTypeRequest,
  GetSummaryManHourType,
} from '@/models/ManHour'

export default BasePage.extend({
  name: 'ManHourReportIndex',
  middleware: ['roleCheck'],
  components: {
    TemplateSummaryManHourByTypeList,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.report.index.name') as string,
    }
  },
  data(): {
    manHours: GetSummaryManHourTypeResponse
    exportManHours: any
    isLoading: isLoading
  } {
    return {
      manHours: new GetSummaryManHourTypeResponse(),
      exportManHours: {} as any,
      isLoading: {
        manHours: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getSummaryManHourType()])
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
     * GetSummaryManHourTypeAPIを叩いて、月次工数分類別工数一覧（月次工数集計表）を取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     */
    async getSummaryManHourType(
      params: GetSummaryManHourTypeRequest = new GetSummaryManHourTypeRequest()
    ) {
      this.clearErrorBar()
      params.year = this.year()
      params.month = this.month()
      await GetSummaryManHourType(params).then((res) => {
        this.exportManHours = cloneDeep(res.data)
        this.manHours = res.data
        if (this.manHours.manHours.length <= 0) {
          this.showErrorBarWithScrollPageTop(
            this.$t('msg.error.csvExportDisabled')
          )
        }
        this.isLoading.manHours = false
      })
    },
    /**
     * CSV出力
     */
    csvOutput() {
      const parentHeaders: any = ['', '', '', '']
      const childHeaders: any = {
        manHourTypeName: this.$t(
          'man-hour.pages.report.index.header.division1'
        ),
        subName: this.$t('man-hour.pages.report.index.header.division2'),
        serviceTypeName: this.$t('man-hour.pages.report.index.header.category'),
        roleName: this.$t('man-hour.pages.report.index.header.role'),
      }
      const supporterOrganizationManHours =
        this.exportManHours.header.supporterOrganizationManHours
      const supporterOrganizationTotal =
        this.exportManHours.header.supporterOrganizationTotal
      const totalCount = supporterOrganizationTotal.length
      parentHeaders.push(this.$t('man-hour.pages.report.index.header.total'))
      for (let i = 0; i < totalCount; i++) {
        parentHeaders.push('')
      }

      //合計の子要素supporterOrganization
      for (const i in supporterOrganizationTotal) {
        const supporterOrganizationId = supporterOrganizationTotal[i]
          .supporterOrganizationId
          ? supporterOrganizationTotal[i].supporterOrganizationId
          : 'Unknown-SupporterOrganizationId-' + i
        const supporterOrganizationName = supporterOrganizationTotal[i]
          .supporterOrganizationName
          ? supporterOrganizationTotal[i].supporterOrganizationName
          : ''
        childHeaders[supporterOrganizationId + 'Total'] =
          supporterOrganizationName
      }

      //合計の子要素の合計
      childHeaders.Total = this.$t('man-hour.pages.report.index.header.total')

      //各supporterOrganization
      for (const i in supporterOrganizationManHours) {
        const supporterOrganizationId = supporterOrganizationManHours[i]
          .supporterOrganizationId
          ? supporterOrganizationManHours[i].supporterOrganizationId
          : 'Unknown-SupporterOrganizationId-' + i
        const supporterOrganizationName = supporterOrganizationManHours[i]
          .supporterOrganizationName
          ? supporterOrganizationManHours[i].supporterOrganizationName
          : ''
        parentHeaders.push(supporterOrganizationName)
        for (const i2 in supporterOrganizationManHours[i].supporters) {
          const supporterId = supporterOrganizationManHours[i].supporters[i2].id
            ? supporterOrganizationManHours[i].supporters[i2].id
            : 'Unknown-SupporterId-' + i2
          const supporterName = supporterOrganizationManHours[i].supporters[i2]
            .name
            ? supporterOrganizationManHours[i].supporters[i2].name
            : ''
          childHeaders[supporterOrganizationId + supporterId + 'Member'] =
            supporterName
          if (parseInt(i2) !== 0) {
            parentHeaders.push('')
          }
        }
      }

      let parentHeaderContent = ''
      for (const i in parentHeaders) {
        parentHeaderContent += '"' + String(parentHeaders[i]) + '",'
      }
      parentHeaderContent += '\r\n'

      const items = []
      for (const i in this.exportManHours.manHours) {
        const manHour = this.exportManHours.manHours[i]
        const row: any = {
          manHourTypeName: manHour.manHourTypeName,
          subName: manHour.subName,
          serviceTypeName: manHour.serviceTypeName,
          roleName: manHour.roleName,
        }

        let Total = 0
        //子要素supporterOrganizationのvalue
        for (const i2 in manHour.supporterOrganizationTotal) {
          Total = Total + manHour.supporterOrganizationTotal[i2].manHour
          const supporterOrganizationId = manHour.supporterOrganizationTotal[i2]
            .supporterOrganizationId
            ? manHour.supporterOrganizationTotal[i2].supporterOrganizationId
            : 'Unknown-SupporterOrganizationId-' + i2
          row[supporterOrganizationId + 'Total'] =
            manHour.supporterOrganizationTotal[i2].manHour
        }
        row.Total = Total

        //子要素supportersのvalue
        for (const i2 in manHour.supporterOrganizationManHours) {
          for (const i3 in manHour.supporterOrganizationManHours[i2]
            .supporters) {
            const supporterId = manHour.supporterOrganizationManHours[i2]
              .supporters[i3].id
              ? manHour.supporterOrganizationManHours[i2].supporters[i3].id
              : 'Unknown-SupporterId-' + i3
            const supporterOrganizationId = manHour
              .supporterOrganizationManHours[i2].supporterOrganizationId
              ? manHour.supporterOrganizationManHours[i2]
                  .supporterOrganizationId
              : 'Unknown-SupporterOrganizationId-' + i2
            row[supporterOrganizationId + supporterId + 'Member'] =
              manHour.supporterOrganizationManHours[i2].supporters[i3].manHour
          }
        }
        items.push(row)
      }
      const content = parentHeaderContent + dataToCsv(items, childHeaders)
      const fileName =
        this.$t('man-hour.pages.report.index.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
  },
})
</script>
