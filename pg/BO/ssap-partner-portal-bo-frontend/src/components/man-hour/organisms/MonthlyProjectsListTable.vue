<template>
  <MonthlyProjectsDataTable
    class="o-MonthlyProjectsListTable"
    :headers="formattedHeader()"
    :items="formattedMonthlyProjects"
    :total="monthlyProjects.length"
    :is-loading="isLoading"
    :last-count-date="lastCountDate"
    @csvOutput="csvOutput"
  >
  </MonthlyProjectsDataTable>
</template>

<script lang="ts">
import MonthlyProjectsDataTable from '~/components/man-hour/molecules/MonthlyProjectsDataTable.vue'
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import {
  GetMonthlyProjectResponse,
  MonthlyProjectHeader,
} from '@/models/Project'

import { IFormattedMonthlyProject } from '@/types/ManHour'

export default BaseComponent.extend({
  name: 'MonthlyProjectsListTable',
  components: {
    MonthlyProjectsDataTable,
    CommonDataTable,
  },
  props: {
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
    /** 月次案件情報 */
    monthlyProjects: {
      type: Array as PropType<GetMonthlyProjectResponse[]>,
      required: true,
    },
    /** ヘッダ情報 */
    header: {
      type: Array as PropType<MonthlyProjectHeader[]>,
      required: true,
    },
    lastCountDate: {
      type: String,
      default: null,
    },
  },
  methods: {
    /** ヘッダ情報の整形
     * @returns 整形されたヘッダ情報
     */
    formattedHeader() {
      const formattedMonthlyProjectHeader: any = [
        [
          {
            text: this.$t(
              'man-hour.pages.report.project.header.serviceTypeName'
            ),
            value: 'serviceTypeName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.report.project.header.name'),
            value: 'name',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.report.project.header.contractType'),
            value: 'contractType',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t(
              'man-hour.pages.report.project.header.mainSalesUserName'
            ),
            value: 'mainSalesUserName',
            sortable: false,
            rowspan: 2,
          },
        ],
        [],
      ]

      //各supporterOrganization
      this.header.map((elm: any) => {
        const supOrgs: any = {
          text: elm.supporterOrganizationName,
          value: elm.supporterOrganizationId,
          sortable: false,
          colspan: 3,
        }
        const supOrgsChildren: any = [
          {
            text: this.$t('man-hour.pages.report.project.header.producer'),
            align: 'start',
            value: elm.supporterOrganizationId + 'Producer',
            sortable: false,
          },
          {
            text: this.$t('man-hour.pages.report.project.header.accelerator'),
            align: 'start',
            value: elm.supporterOrganizationId + 'Accelerator',
            width: '300px',
            sortable: false,
          },
          {
            text: this.$t('man-hour.pages.report.project.header.total'),
            align: 'start',
            value: elm.supporterOrganizationId + 'Total',
            sortable: false,
          },
        ]
        return (
          formattedMonthlyProjectHeader[0].push(supOrgs),
          formattedMonthlyProjectHeader[1].push(...supOrgsChildren)
        )
      })
      const remainingHeader: any = [
        {
          text: this.$t(
            'man-hour.pages.report.project.header.thisMonthContractTime'
          ),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
        {
          text: this.$t('man-hour.pages.report.project.header.thisMonthProfit'),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
        {
          text: this.$t('man-hour.pages.report.project.header.supportDateFrom'),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
        {
          text: this.$t('man-hour.pages.report.project.header.supportDateTo'),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
        {
          text: this.$t('man-hour.pages.report.project.header.totalProfit'),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
        {
          text: this.$t(
            'man-hour.pages.report.project.header.totalContractTime'
          ),
          sortable: false,
          rowspan: 2,
          colspan: 1,
        },
      ]
      formattedMonthlyProjectHeader[0].push(...remainingHeader)
      const remainingHeaderChild: any = [
        {
          value: 'thisMonthContractTime',
          sortable: false,
        },
        {
          value: 'thisMonthProfit',
          sortable: false,
        },
        {
          value: 'supportDateFrom',
          sortable: false,
        },
        {
          value: 'supportDateTo',
          sortable: false,
        },
        {
          value: 'totalProfit',
          sortable: false,
        },
        {
          value: 'totalContractTime',
          sortable: false,
        },
      ]
      formattedMonthlyProjectHeader[1].push(...remainingHeaderChild)
      return formattedMonthlyProjectHeader
    },
    csvOutput() {
      this.$emit('csvOutput')
    },
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedMonthlyProjects(): void | IFormattedMonthlyProject[] {
      if (this.monthlyProjects) {
        return this.monthlyProjects.map((monthlyProject: any) => {
          monthlyProject.organizations.map((elm: any) => {
            monthlyProject[elm.supporterOrganizationId + 'Total'] =
              elm.members.length
            const producerName: string[] = []
            const acceleratorName: string[] = []
            elm.members.map((e: any) => {
              //プロデューサのisConfirm:true時に【確】をつける
              if (e.role === 'プロデューサー') {
                e.isConfirm
                  ? producerName.push(
                      e.supporterName +
                        this.$t('man-hour.pages.report.project.isConfirm')
                    )
                  : producerName.push(e.supporterName)
                return (monthlyProject[
                  elm.supporterOrganizationId + 'Producer'
                ] = producerName.join('、'))
              }
              //アクセラレータのisConfirm:true時に【確】をつける
              e.isConfirm
                ? acceleratorName.push(
                    e.supporterName +
                      this.$t('man-hour.pages.report.project.isConfirm')
                  )
                : acceleratorName.push(e.supporterName)
              return (monthlyProject[
                elm.supporterOrganizationId + 'Accelerator'
              ] = acceleratorName.join('、'))
            })
            return monthlyProject
          })
          monthlyProject.thisMonthContractTime += '/月'
          monthlyProject.thisMonthProfit += '/月'
          return monthlyProject
        })
      }
    },
  },
})
</script>

<style lang="scss">
.o-MonthlyProjectsListTable {
  .v-data-table-header {
    tr {
      &:nth-of-type(2) {
        th {
          &:nth-last-of-type(-n + 6) {
            display: none;
          }
        }
      }
    }
  }
}
</style>
