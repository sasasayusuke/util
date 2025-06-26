<template>
  <AlertDataTable
    :year="year"
    :month="month"
    :headers="alertHeaders"
    :items="formattedProject"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :is-loading="formattedProject.length >= 0"
    :total="total"
    :total-contract-time="totalContractTime"
    :is-scroll="true"
    link-prefix="/project"
    @click:prev="$emit('click:prev')"
    @click:next="$emit('click:next')"
  >
  </AlertDataTable>
</template>

<script lang="ts">
import AlertDataTable from '~/components/man-hour/molecules/AlertDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import { ENUM_FORMATTED_PROJECT } from '~/types/ManHour'

export default BaseComponent.extend({
  name: 'AlertListTable',
  components: {
    AlertDataTable,
  },
  props: {
    /** 現在の年 || 選択された年 */
    year: {
      type: Number,
    },
    /** 現在の月 || 選択された月 */
    month: {
      type: Number,
    },
    /** オフセットページ */
    offsetPage: {
      type: Number,
    },
    /** 最大ページ */
    maxPage: {
      type: Number,
    },
    /** 合計数 */
    total: {
      type: Number,
    },
    /** 延べ契約時間の合計 */
    totalContractTime: {
      type: Number,
    },
    /** 案件毎の工数状況とアラート一覧情報 */
    summaryProjectManHourAlerts: {
      type: Object,
      required: true,
      default: {},
    },
  },
  data() {
    return {
      alertHeaders: [
        [
          {
            text: this.$t('man-hour.pages.alert.list.header.serviceTypeName'),
            align: 'start',
            value: 'serviceTypeName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.alert.list.header.projectName'),
            align: 'start',
            value: 'projectName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.alert.list.header.customerName'),
            align: 'start',
            value: 'customerName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.alert.list.header.supportDateFromTo'),
            align: 'start',
            value: 'supportDateFromTo',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.alert.list.header.totalContractTime'),
            align: 'start',
            value: 'totalContractTime',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.mainSupporterUserName'
            ),
            align: 'start',
            value: 'mainSupporterUserName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.supporterUsersName'
            ),
            align: 'start',
            value: 'supporterUsersName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summarySupportManHour'
            ),
            align: 'start',
            value: 'summarySupportManHour',
            sortable: false,
            colspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthSupportManHour',
            sortable: false,
            colspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summaryTheoreticalSupportManHour'
            ),
            align: 'start',
            value: 'summaryTheoreticalSupportManHour',
            sortable: false,
            colspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthTheoreticalSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthTheoreticalSupportManHour',
            sortable: false,
            colspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.alertSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthSupportManHour',
            sortable: false,
            colspan: 2,
          },
        ],
        [
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summaryDirectSupportManHour'
            ),
            align: 'start',
            value: 'summaryDirectSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summaryPreSupportManHour'
            ),
            align: 'start',
            value: 'summaryPreSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthDirectSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthDirectSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthPreSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthPreSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summaryTheoreticalDirectSupportManHour'
            ),
            align: 'start',
            value: 'summaryTheoreticalDirectSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.summaryTheoreticalPreSupportManHour'
            ),
            align: 'start',
            value: 'summaryTheoreticalPreSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthTheoreticalDirectSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthTheoreticalDirectSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.thisMonthTheoreticalPreSupportManHour'
            ),
            align: 'start',
            value: 'thisMonthTheoreticalPreSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.alertDirectSupportManHour'
            ),
            align: 'start',
            value: 'alertDirectSupportManHour',
            sortable: false,
          },
          {
            text: this.$t(
              'man-hour.pages.alert.list.header.alertPreSupportManHour'
            ),
            align: 'start',
            value: 'alertPreSupportManHour',
            sortable: false,
          },
        ],
      ],
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedProject(): any {
      if (this.summaryProjectManHourAlerts.projects) {
        return this.summaryProjectManHourAlerts.projects.map((project: any) => {
          // 対面支援工数の超過/超過見込みアラートを設置 [1:見込み, 2:超過] (判定条件は仮)
          if (
            project.thisMonthDirectSupportManHour > project.totalContractTime
          ) {
            project.alertDirectSupportManHour = ENUM_FORMATTED_PROJECT.EXCESS
          } else if (
            project.thisMonthTheoreticalDirectSupportManHour >
            project.totalContractTime
          ) {
            project.alertDirectSupportManHour =
              ENUM_FORMATTED_PROJECT.EXPECTATION
          } else {
            project.alertDirectSupportManHour = ENUM_FORMATTED_PROJECT.INIT
          }

          // 支援仕込時間の超過/超過見込みアラートを設置 [1:見込み, 2:超過] (判定条件は仮)
          if (project.thisMonthPreSupportManHour > project.totalContractTime) {
            project.alertPreSupportManHour = ENUM_FORMATTED_PROJECT.EXCESS
          } else if (
            project.thisMonthTheoreticalPreSupportManHour >
            project.totalContractTime
          ) {
            project.alertPreSupportManHour = ENUM_FORMATTED_PROJECT.EXPECTATION
          } else {
            project.alertPreSupportManHour = ENUM_FORMATTED_PROJECT.INIT
          }

          project.supportDateFromTo =
            project.supportDateFrom + ' ~ ' + project.supportDateTo
          project.totalContractTime += 'h'

          if (project.mainSupporterUser) {
            project.mainSupporterUserName = project.mainSupporterUser.name
          }

          if (project.supporterUsers) {
            const supporterUsers: any[] = []
            project.supporterUsers.forEach((user: any) => {
              supporterUsers.push(user.name)
            })
            project.supporterUsersName = supporterUsers.join(',')
          }

          project.summaryDirectSupportManHour += 'h'
          project.summaryPreSupportManHour += 'h'
          project.thisMonthDirectSupportManHour += 'h'
          project.thisMonthPreSupportManHour += 'h'
          project.summaryTheoreticalDirectSupportManHour += 'h'
          project.summaryTheoreticalPreSupportManHour += 'h'
          project.thisMonthTheoreticalDirectSupportManHour += 'h'
          project.thisMonthTheoreticalPreSupportManHour += 'h'
          return project
        })
      } else {
        return []
      }
    },
  },
})
</script>
