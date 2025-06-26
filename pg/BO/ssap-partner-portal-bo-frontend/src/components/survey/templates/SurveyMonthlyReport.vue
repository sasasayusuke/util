<template>
  <RootTemPlate>
    <ListInPageHeader
      month-change
      @buttonAction1="thisMonth"
      @buttonAction2="lastMonth"
      @buttonAction3="nextMonth"
    >
      {{ setPageName }}
    </ListInPageHeader>
    <v-sheet rounded elevation="3" color="#fff" class="mt-7 pa-7">
      <!-- サービスアンケート -->
      <Table class="report">
        <template #header>
          <tr>
            <th>&nbsp;</th>
            <th>
              {{ $t('survey.pages.monthly.table.label.aggregationPeriod') }}
            </th>
            <th>{{ $t('survey.pages.monthly.table.label.sendPlanCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receiveCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receivePercent') }}</th>
          </tr>
        </template>
        <template #body>
          <tr>
            <th rowspan="2">{{ $t('survey.pages.monthly.table.service') }}</th>
            <td>{{ setSummaryMonth }}</td>
            <td>{{ surveySummaryReport.service.plan }}</td>
            <td>{{ surveySummaryReport.service.collect }}</td>
            <td>{{ surveySummaryReport.service.percent }}%</td>
          </tr>
          <tr>
            <td>{{ $t('survey.pages.monthly.table.label.cumulativeYear') }}</td>
            <td>{{ surveySummaryReport.service.summaryPlan }}</td>
            <td>{{ surveySummaryReport.service.summaryCollect }}</td>
            <td>{{ surveySummaryReport.service.summaryPercent }}%</td>
          </tr>
        </template>
      </Table>
      <!-- 修了アンケート -->
      <Table class="report mt-8">
        <template #header>
          <tr>
            <th>&nbsp;</th>
            <th>
              {{ $t('survey.pages.monthly.table.label.aggregationPeriod') }}
            </th>
            <th>{{ $t('survey.pages.monthly.table.label.sendPlanCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receiveCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receivePercent') }}</th>
          </tr>
        </template>
        <template #body>
          <tr>
            <th rowspan="2">
              {{ $t('survey.pages.monthly.table.completion') }}
            </th>
            <td>{{ setSummaryMonth }}</td>
            <td>{{ surveySummaryReport.completion.plan }}</td>
            <td>{{ surveySummaryReport.completion.collect }}</td>
            <td>{{ surveySummaryReport.completion.percent }}%</td>
          </tr>
          <tr>
            <td>{{ $t('survey.pages.monthly.table.label.cumulativeYear') }}</td>
            <td>{{ surveySummaryReport.completion.summaryPlan }}</td>
            <td>{{ surveySummaryReport.completion.summaryCollect }}</td>
            <td>{{ surveySummaryReport.completion.summaryPercent }}%</td>
          </tr>
        </template>
      </Table>
      <!-- サービスアンケート・修了時アンケート合算 -->
      <Table class="report mt-8">
        <template #header>
          <tr>
            <th>&nbsp;</th>
            <th>
              {{ $t('survey.pages.monthly.table.label.aggregationPeriod') }}
            </th>
            <th>{{ $t('survey.pages.monthly.table.label.sendPlanCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receiveCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receivePercent') }}</th>
          </tr>
        </template>
        <template #body>
          <tr>
            <th rowspan="2">
              <span>{{
                $t('survey.pages.monthly.table.serviceAndCompletion')
              }}</span>
            </th>
            <td>{{ setSummaryMonth }}</td>
            <td>{{ surveySummaryReport.serviceAndCompletion.plan }}</td>
            <td>{{ surveySummaryReport.serviceAndCompletion.collect }}</td>
            <td>{{ surveySummaryReport.serviceAndCompletion.percent }}%</td>
          </tr>
          <tr>
            <td>{{ $t('survey.pages.monthly.table.label.cumulativeYear') }}</td>
            <td>{{ surveySummaryReport.serviceAndCompletion.summaryPlan }}</td>
            <td>
              {{ surveySummaryReport.serviceAndCompletion.summaryCollect }}
            </td>
            <td>
              {{ surveySummaryReport.serviceAndCompletion.summaryPercent }}%
            </td>
          </tr>
        </template>
      </Table>
      <!-- Partner Portal利用アンケート -->
      <Table class="report mt-8">
        <template #header>
          <tr>
            <th>&nbsp;</th>
            <th>
              {{ $t('survey.pages.monthly.table.label.aggregationPeriod') }}
            </th>
            <th>{{ $t('survey.pages.monthly.table.label.sendPlanCount') }}</th>
            <th>{{ $t('survey.pages.monthly.table.label.receiveCount') }}</th>
            <th>
              {{ $t('survey.pages.monthly.table.label.receivePercent') }}
            </th>
          </tr>
        </template>
        <template #body>
          <tr>
            <th rowspan="2">
              {{ $t('survey.pages.monthly.table.partnerPortal') }}
            </th>
            <td>{{ setSummaryMonth }}</td>
            <td>{{ surveySummaryReport.pp.plan }}</td>
            <td>{{ surveySummaryReport.pp.collect }}</td>
            <td>{{ surveySummaryReport.pp.percent }}%</td>
          </tr>
          <tr>
            <td>{{ $t('survey.pages.monthly.table.label.cumulativeYear') }}</td>
            <td>{{ surveySummaryReport.pp.summaryPlan }}</td>
            <td>{{ surveySummaryReport.pp.summaryCollect }}</td>
            <td>{{ surveySummaryReport.pp.summaryPercent }}%</td>
          </tr>
        </template>
      </Table>
    </v-sheet>
  </RootTemPlate>
</template>

<script lang="ts">
import { format, parse } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import { Sheet, Table } from '~/components/common/atoms/index'

import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetSurveySummaryReportResponse } from '~/models/Survey'
import { IGetSurveySummaryReportRequest } from '~/types/Survey'

export default BaseComponent.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    Sheet,
    Table,
  },
  computed: {
    /**
     * 表示タイトルを返す
     * @returns 表示タイトル文字列
     */
    setPageName() {
      const pageName = this.pageName as string
      const summaryMonth = this.setSummaryMonth as string
      return pageName + summaryMonth
    },
    /**
     * 集計期間文字列を返す
     * @returns 集計期間文字列
     */
    setSummaryMonth() {
      const summaryMonth = this.summaryMonth as string
      const year: string = this.getYear()
      const month: string = this.getMonth()
      return summaryMonth.replace('{year}', year).replace('{month}', month)
    },
  },
  props: {
    /** アンケート月次レポート情報 */
    surveySummaryReport: {
      type: Object as PropType<GetSurveySummaryReportResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      selected: 0,
      pageName: this.$t('survey.pages.monthly.name'),
      summaryMonth: this.$t('survey.pages.monthly.summaryMonth'),
    }
  },
  methods: {
    /**
     * 集計期間(年)を文字列で返す
     * @returns 集計期間(年)文字列
     */
    getYear() {
      const summaryDateObject = parse(
        this.surveySummaryReport.summaryMonth,
        'yyyy/M',
        getCurrentDate()
      )
      return format(summaryDateObject, 'yyyy')
    },
    /**
     * 集計期間(月)を文字列で返す
     * @returns 集計期間(月)文字列
     */
    getMonth() {
      const summaryDateObject = parse(
        this.surveySummaryReport.summaryMonth,
        'yyyy/M',
        getCurrentDate()
      )
      return format(summaryDateObject, 'M')
    },
    /** 今月のアンケート月次レポートを表示 */
    thisMonth() {
      const params: IGetSurveySummaryReportRequest = {
        summaryMonth: parseInt(format(getCurrentDate(), 'yyyyM')),
      }
      this.getSurveySummaryReport(params)
    },
    /** 前の月のアンケート月次レポートを表示 */
    lastMonth() {
      const month = parseInt(this.getMonth()) - 1
      const params: IGetSurveySummaryReportRequest = {
        summaryMonth: parseInt(
          format(new Date(parseInt(this.getYear()), month - 1), 'yyyyM')
        ),
      }
      this.getSurveySummaryReport(params)
    },
    /** 次の月のアンケート月次レポートを表示 */
    nextMonth() {
      const month = parseInt(this.getMonth()) - 1
      const params: IGetSurveySummaryReportRequest = {
        summaryMonth: parseInt(
          format(new Date(parseInt(this.getYear()), month + 1), 'yyyyM')
        ),
      }
      this.getSurveySummaryReport(params)
    },
    /**
     * アンケート月次レポートを取得
     * @param 取得対象リクエストパラメータ
     */
    getSurveySummaryReport(
      params: IGetSurveySummaryReportRequest = {
        summaryMonth: parseInt(format(getCurrentDate(), 'yyyyM')),
      }
    ) {
      this.$emit('getSurveySummaryReport', params)
    },
  },
})
</script>
