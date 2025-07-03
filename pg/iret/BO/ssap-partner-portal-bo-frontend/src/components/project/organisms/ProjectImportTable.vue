<template>
  <v-data-table
    :headers="projectHeaders"
    :items="formattedProject"
    hide-default-footer
    :loading="isLoading"
    :no-data-text="$t('common.label.noData')"
    :loading-text="$t('common.label.loading')"
    disable-pagination
  >
    <template #top="{ pagination }">
      <Paragraph style-set="confirm-text">
        {{ $t('project.pages.import.confirm.processed') }}
        <strong>
          <strong>{{ pagination.itemsLength || '-' }} </strong>
          {{ $t('project.pages.import.confirm.number') }}</strong
        >
      </Paragraph>
    </template>
    <template #[`footer.page-text`]></template>

    <!-- (アンケート)アンケート記載時のプロジェクト名 -->
    <template #[`header.name`]="{ header }">
      <OverflowTooltip :text="header.text" :max="16" />
    </template>
  </v-data-table>
</template>

<script lang="ts">
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import { formatDateStr } from '~/utils/common-functions'

import { Paragraph } from '~/components/common/atoms/index'
import { ImportedProject } from '~/types/Project'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Paragraph,
    OverflowTooltip,
  },
  props: {
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 案件情報 */
    projects: {
      type: Array as PropType<ImportedProject[]>,
      required: true,
    },
  },
  data(): { projectHeaders: IDataTableHeader[] } {
    return {
      projectHeaders: [
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceOpportunityId'
          ),
          value: 'salesforceOpportunityId',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.customerId'),
          value: 'customerId',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.salesforceUpdateAt'),
          value: 'salesforceUpdateAt',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.name'),
          value: 'name',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.customerName'),
          value: 'customerName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.serviceTypeName'),
          value: 'serviceTypeName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.supporterOrganizationName'
          ),
          value: 'supporterOrganizationName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceUsePackage'
          ),
          value: 'salesforceUsePackage',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.createNew'),
          value: 'createNew',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.continued'),
          value: 'continued',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.salesforceViaPr'),
          value: 'salesforceViaPr',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceMainCustomer.name'
          ),
          value: 'salesforceMainCustomer.name',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceMainCustomer.email'
          ),
          value: 'salesforceMainCustomer.email',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceMainCustomer.organizationName'
          ),
          value: 'salesforceMainCustomer.organizationName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceMainCustomer.job'
          ),
          value: 'salesforceMainCustomer.job',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.mainSalesUserName'),
          value: 'mainSalesUserName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.contractDate'),
          value: 'contractDate',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.phase'),
          value: 'phase',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.customerSuccess'),
          value: 'customerSuccess',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.supportDateFrom'),
          value: 'supportDateFrom',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.supportDateTo'),
          value: 'supportDateTo',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.april'
          ),
          value: 'profit.monthly[3]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.monthly.may'),
          value: 'profit.monthly[4]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.monthly.june'),
          value: 'profit.monthly[5]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.monthly.july'),
          value: 'profit.monthly[6]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.august'
          ),
          value: 'profit.monthly[7]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.september'
          ),
          value: 'profit.monthly[8]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.october'
          ),
          value: 'profit.monthly[9]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.november'
          ),
          value: 'profit.monthly[10]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.december'
          ),
          value: 'profit.monthly[11]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.january'
          ),
          value: 'profit.monthly[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.february'
          ),
          value: 'profit.monthly[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.monthly.march'
          ),
          value: 'profit.monthly[2]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.quarterly.first'
          ),
          value: 'profit.quarterly[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.quarterly.second'
          ),
          value: 'profit.quarterly[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.quarterly.third'
          ),
          value: 'profit.quarterly[2]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.profit.quarterly.forth'
          ),
          value: 'profit.quarterly[3]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.half.first'),
          value: 'profit.half[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.half.second'),
          value: 'profit.half[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.profit.year'),
          value: 'profit.year',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.monthly.april'),
          value: 'gross.monthly[3]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.monthly.may'),
          value: 'gross.monthly[4]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.monthly.june'),
          value: 'gross.monthly[5]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.monthly.july'),
          value: 'gross.monthly[6]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.august'
          ),
          value: 'gross.monthly[7]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.september'
          ),
          value: 'gross.monthly[8]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.october'
          ),
          value: 'gross.monthly[9]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.november'
          ),
          value: 'gross.monthly[10]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.december'
          ),
          value: 'gross.monthly[11]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.january'
          ),
          value: 'gross.monthly[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.monthly.february'
          ),
          value: 'gross.monthly[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.monthly.march'),
          value: 'gross.monthly[2]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.quarterly.first'
          ),
          value: 'gross.quarterly[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.quarterly.second'
          ),
          value: 'gross.quarterly[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.quarterly.third'
          ),
          value: 'gross.quarterly[2]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.gross.quarterly.forth'
          ),
          value: 'gross.quarterly[3]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.half.first'),
          value: 'gross.half[0]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.half.second'),
          value: 'gross.half[1]',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.gross.year'),
          value: 'gross.year',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceMainSupporterUserName'
          ),
          value: 'salesforceMainSupporterUserName',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t(
            'project.pages.import.tableHeader.salesforceSupporterUserNames'
          ),
          value: 'salesforceSupporterUserNames',
          sortable: false,
          width: '200px',
        },
        {
          text: this.$t('project.pages.import.tableHeader.totalContractTime'),
          value: 'totalContractTime',
          sortable: false,
          width: '200px',
        },
      ],
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換した案件情報を返す
     * @returns フォーマット済み案件情報配列
     */
    formattedProject(): ImportedProject[] {
      return this.projects.map((project: any) => {
        const rtn = Object.assign(new ImportedProject(), project)
        //最終更新日時
        rtn.salesforceUpdateAt = formatDateStr(
          project.salesforceUpdateAt,
          'yyyy/MM/dd HH:mm'
        )
        //完了予定日
        rtn.contractDate = formatDateStr(project.contractDate, 'yyyy/MM/dd')
        //支援開始日
        rtn.supportDateFrom = formatDateStr(
          project.supportDateFrom,
          'yyyy/MM/dd'
        )
        //支援終了日
        rtn.supportDateTo = formatDateStr(project.supportDateTo, 'yyyy/MM/dd')

        //前年度で契約し期をまたぐ商談
        rtn.continued = project.continued
          ? this.$t('common.detail.yes')
          : this.$t('common.detail.no')

        //PKG利用
        rtn.salesforceUsePackage = project.salesforceUsePackage
          ? this.$t('common.detail.yes')
          : this.$t('common.detail.no')

        //PR経由
        rtn.salesforceViaPr = project.salesforceViaPr
          ? this.$t('common.detail.yes')
          : this.$t('common.detail.no')

        return rtn
      })
    },
  },
})
</script>
