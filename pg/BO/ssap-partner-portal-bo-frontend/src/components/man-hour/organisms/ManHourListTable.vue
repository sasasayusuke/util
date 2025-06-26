<template>
  <CommonDataTable
    :is-loading="isLoading"
    :total="total"
    :loading="formattedSummaryProjectManHourAlerts.length > 0"
    :headers="manHourHeaders"
    :items="formattedSummaryProjectManHourAlerts"
    :additional-text="
      String(summaryProjectManHourAlerts.summaryThisMonthContractTime)
    "
    :shows-pagination="false"
    :total-contract-time="
      summaryProjectManHourAlerts.summaryThisMonthContractTime
    "
    is-total-contract-time
    :shows-header-text="true"
    link-prefix="/man-hour"
    is-scroll
    short-text
    class="mt-7"
  >
    <!-- 契約時間<br/>(延べ)-->
    <template #[`header.totalContractTime`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.totalContractTime1') }}<br />{{
            $t('man-hour.pages.index.header.totalContractTime2')
          }}
        </th>
      </thead>
    </template>
    <!-- 契約時間<br/>(集計月)-->
    <template #[`header.thisMonthContractTime`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.thisMonthContractTime1') }}<br />{{
            $t('man-hour.pages.index.header.thisMonthContractTime2')
          }}
        </th>
      </thead>
    </template>
    <!-- 粗利額<br/>(延べ)-->
    <template #[`header.totalProfit`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.totalProfit1') }}<br />{{
            $t('man-hour.pages.index.header.totalProfit2')
          }}
        </th>
      </thead>
    </template>
    <!-- 粗利額<br/>(集計月)-->
    <template #[`header.thisMonthProfit`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.thisMonthProfit1') }}<br />{{
            $t('man-hour.pages.index.header.thisMonthProfit2')
          }}
        </th>
      </thead>
    </template>
    <!-- 直接支援<br/>工数累積値-->
    <template #[`header.summaryDirectSupportManHour`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.summaryDirectSupportManHour1')
          }}<br />{{
            $t('man-hour.pages.index.header.summaryDirectSupportManHour2')
          }}
        </th>
      </thead>
    </template>
    <!-- 仕込工数<br/>累積値-->
    <template #[`header.summaryPreSupportManHour`]>
      <thead>
        <th>
          {{ $t('man-hour.pages.index.header.summaryPreSupportManHour1')
          }}<br />{{
            $t('man-hour.pages.index.header.summaryPreSupportManHour2')
          }}
        </th>
      </thead>
    </template>
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable, {
  IDataTableHeader,
} from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'ManHourListTable',
  components: {
    CommonDataTable,
  },
  props: {
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
    /** 案件毎工数状況とアラート一覧 */
    summaryProjectManHourAlerts: {
      type: Object,
      required: true,
    },
    /** 一覧件数 */
    total: {
      type: Number,
      required: true,
    },
  },
  data(): { manHourHeaders: IDataTableHeader[] } {
    return {
      manHourHeaders: [
        {
          text: this.$t('man-hour.pages.index.header.serviceTypeName'),
          value: 'serviceTypeName',
          sortable: false,
          width: '84px',
        },
        {
          text: this.$t('man-hour.pages.index.header.projectName'),
          value: 'projectName',
          sortable: false,
          width: '160px',
          maxLength: 13,
          link: {
            prefix: '/project/',
            value: 'projectId',
            suffix: `?source_url=${encodeURIComponent(this.$route.fullPath)}`,
          },
        },
        {
          text: this.$t('man-hour.pages.index.header.customerName'),
          value: 'customerName',
          sortable: false,
          width: '200px',
          maxLength: 16,
        },
        {
          text: this.$t(
            'man-hour.pages.index.header.supporterOrganizationName'
          ),
          value: 'supporterOrganizationName',
          sortable: false,
          width: '20px',
        },
        {
          text: this.$t('man-hour.pages.index.header.supportDateFrom'),
          value: 'supportDateFromTo',
          sortable: false,
          width: '143px',
        },
        {
          text: 'totalContractTime',
          value: 'totalContractTime',
          sortable: false,
          width: '48px',
        },
        {
          text: 'thisMonthContractTime',
          value: 'thisMonthContractTime',
          sortable: false,
          width: '48px',
        },
        {
          text: 'totalProfit',
          value: 'totalProfit',
          sortable: false,
          width: '66px',
        },
        {
          text: 'thisMonthProfit',
          value: 'thisMonthProfit',
          sortable: false,
          width: '84px',
        },
        {
          text: 'summaryDirectSupportManHour',
          value: 'summaryDirectSupportManHour',
          sortable: false,
          width: '60px',
        },
        {
          text: this.$t('man-hour.pages.index.header.summaryPreSupportManHour'),
          value: 'summaryPreSupportManHour',
          sortable: false,
          width: '48px',
        },
      ],
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedSummaryProjectManHourAlerts(): any {
      if (this.summaryProjectManHourAlerts.projects) {
        return this.summaryProjectManHourAlerts.projects.map((project: any) => {
          project.supportDateFromTo =
            project.supportDateFrom + ' ~ ' + project.supportDateTo
          project.totalContractTime += 'h'
          project.thisMonthContractTime += 'h/月'
          project.totalProfit = project.totalProfit.toLocaleString()
          project.totalProfit = `¥${project.totalProfit}`
          project.thisMonthProfit = project.thisMonthProfit.toLocaleString()
          project.thisMonthProfit = `¥${project.thisMonthProfit}/月`
          project.summaryDirectSupportManHour += 'h'
          project.summaryPreSupportManHour += 'h'
          return project
        })
      } else {
        return []
      }
    },
  },
})
</script>

<style lang="scss"></style>
