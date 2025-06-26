<template>
  <DetailContainer
    :title="$t('survey.pages.index.section.section')"
    :is-editing="false"
    is-hide-footer
    is-hide-button1
    is-hide-button2
    note-head="required"
  >
    <template #date>
      {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
    </template>
    <template #uniqueButtons>
      <Button
        style-set="detailHeaderPositiveStretch"
        outlined
        @click="exportCsv('organization')"
      >
        {{ $t('common.button.csvOutputSection') }}
      </Button>
      <Button
        style-set="detailHeaderPositiveStretch"
        outlined
        class="ml-2"
        @click="exportCsv('supporter')"
      >
        {{ $t('common.button.csvOutputSupporter') }}
      </Button>
    </template>
    <Sheet class="px-7 pt-1 pb-7">
      <SurveyTopSectionSort
        :param="searchParam"
        @update="update"
        @sort="search"
        @clear="clear"
      />
      <v-container fluid pa-0>
        <v-row no-gutters>
          <v-col>
            <SurveyTopSectionTable
              v-if="!isLoading"
              :surveys="surveySummarySupporterOrganizations.termSummaryResult"
              :total="surveySummarySupporterOrganizations.totalSummaryResult"
              :set-type="type"
              @changeChart="changeChart"
            />
          </v-col>
          <v-col class="pl-10">
            <Chart
              type="line"
              chart-id="SurveyTopSectionContainer-chart-1"
              :width="530"
              :height="200"
              :chart-data="chartData()"
              :chart-options="chartOptions()"
            />
          </v-col>
        </v-row>
      </v-container>
    </Sheet>
  </DetailContainer>
</template>

<script lang="ts">
import { format, parse, parseISO } from 'date-fns'
import {
  getCurrentDate,
  getFiscalYearStart,
  getFiscalYearEnd,
} from '~/utils/common-functions'
import { Sheet, TextLink, Button } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyTopSectionSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyTopSectionSort.vue'
import SurveyTopSectionTable from '~/components/survey/molecules/SurveyTopSectionTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import {
  SupporterOrganizationSurveysItem,
  SupporterOrganizationsItem,
} from '~/models/Survey'
import { GetBatchControlByIdResponse } from '~/models/Master'

export class ExportParam {
  summaryMonthFrom: string
  summaryMonthTo: string

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.summaryMonthFrom = format(fiscalyearStart, 'yyyy/MM')

    const fiscalyearEnd = getFiscalYearEnd()
    this.summaryMonthTo = format(fiscalyearEnd, 'yyyy/MM')
  }

  mode = ''
}

export class SupporterOrganizationsItemWithSummaryMonth extends SupporterOrganizationsItem {
  summaryMonth: string = ''
}

export default BaseComponent.extend({
  components: {
    Sheet,
    TextLink,
    Button,
    DetailContainer,
    SurveyTopSectionSort,
    SurveyTopSectionTable,
    Chart,
  },
  props: {
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 表示期間 */
    params: {
      type: Object,
      required: true,
    },
    /** 課別集計結果 */
    surveySummarySupporterOrganizations: {
      type: Object,
      required: true,
    },
    /** 最終集計日時 */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
    },
  },
  data() {
    return {
      type: 'serviceOverallSatisfaction',
      labels: {
        serviceOverallSatisfaction:
          String(this.$t('survey.pages.index.table.label.service')) +
          ' ' +
          String(this.$t('survey.pages.index.table.label.overallSatisfaction')),
        completionOverallSatisfaction:
          String(this.$t('survey.pages.index.table.label.completion')) +
          ' ' +
          String(this.$t('survey.pages.index.table.label.overallSatisfaction')),
        continuation: this.$t(
          'survey.pages.index.table.label.chartContinuation'
        ),
        recommended: this.$t(
          'survey.pages.index.table.completion.recommended2'
        ),
        evaluation: this.$t('survey.pages.index.table.label.chartSales'),
      } as any,
      colors: [
        '#4688f1',
        '#e8453c',
        '#f9bb2d',
        '#3aa757',
        '#fd6d22',
        '#4cbdc5',
        '#7dacf4',
      ],
      searchParam: new SearchParam(),
      apiName: 'getSurveySummarySupporterOrganizations',
    }
  },
  methods: {
    /**
     * 表示期間の始点年月を取得
     * @returns 始点年月文字列
     */
    getFromParam() {
      return format(
        parse(this.params.yearMonthFrom, 'yyyyMM', getCurrentDate()),
        'yyyy/MM'
      ) as string
    },
    /**
     * 表示期間の終点年月を取得
     * @returns 終点年月文字列
     */
    getToParam() {
      return format(
        parse(this.params.yearMonthTo, 'yyyyMM', getCurrentDate()),
        'yyyy/MM'
      ) as string
    },
    /**
     * グラフ表示内の月表示ラベルを配列で返す
     * @returns 月表示ラベル配列
     */
    getLabels() {
      const results: string[] = []
      let i = 0
      while (true) {
        const yearMonth = parse(
          this.getFromParam(),
          'yyyy/MM',
          getCurrentDate()
        )
        yearMonth.setMonth(yearMonth.getMonth() + i)
        const date = format(yearMonth, 'yyyy/MM') as string
        results.push(date)
        if (date === this.getToParam()) {
          break
        } else {
          i++
        }
      }
      return results
    },
    /**
     * 他コンポーネントから表示期間の変更を受け取る
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      // @ts-ignore
      this.searchParam[item] = String(event)
    },
    /** 表示期間および表示をリセット */
    clear() {
      this.searchParam = new SearchParam()
      this.search()
    },
    /** 指定した表示期間の集計結果を取得 */
    search() {
      const strYearMonthFrom = this.searchParam.yearMonthFrom
      const strYearMonthTo = this.searchParam.yearMonthTo
      let intYearMonthFrom = null
      let intYearMonthTo = null
      if (strYearMonthFrom != null && !Number.isInteger(strYearMonthFrom)) {
        intYearMonthFrom = parseInt(
          format(new Date(strYearMonthFrom), 'yyyyMM')
        )
      }
      if (strYearMonthTo != null && !Number.isInteger(strYearMonthTo)) {
        intYearMonthTo = parseInt(format(new Date(strYearMonthTo), 'yyyyMM'))
      }
      if (
        intYearMonthFrom != null &&
        intYearMonthTo != null &&
        intYearMonthFrom > intYearMonthTo
      ) {
        this.searchParam.yearMonthFrom = strYearMonthTo
        this.searchParam.yearMonthTo = strYearMonthFrom
      }
      const request = new SearchParam()
      Object.assign(request, this.searchParam)
      this.$emit(this.apiName, request)
    },
    /**
     * ISO8601形式の最終集計日時を表示用にフォーマットした文字列を返す
     * @returns フォーマット済み最終集計日時文字列
     */
    getBatchEndAt() {
      return this.batchControl && this.batchControl.batchEndAt
        ? format(parseISO(this.batchControl.batchEndAt), 'yyyy/MM/dd HH:mm')
        : 'ー'
    },
    /**
     * 集計結果をCSV出力。
     * @param params exportSurveysAPIのリクエストパラメータを指定
     */
    exportCsv(mode: string) {
      const strYearMonthFrom = this.searchParam.yearMonthFrom
      const strYearMonthTo = this.searchParam.yearMonthTo
      const request = new ExportParam()
      // @ts-ignore
      request.summaryMonthFrom = parseInt(
        format(new Date(strYearMonthFrom), 'yyyyMM')
      )
      // @ts-ignore
      request.summaryMonthTo = parseInt(
        format(new Date(strYearMonthTo), 'yyyyMM')
      )
      request.mode = mode
      this.$emit('exportSurveys', request)
    },
    /**
     * 表示するグラフ種別を変更
     * @param グラフ種別
     */
    changeChart(type: string) {
      this.type = type
    },
    /**
     * 支援者組織名から対象の支援者組織別集計情報を返す
     * @param 支援者組織名
     * @returns 支援者組織別集計情報
     */
    getOrganization(type: string): SupporterOrganizationsItemWithSummaryMonth {
      return this.surveySummarySupporterOrganizations.surveys.map(
        (elm: SupporterOrganizationSurveysItem, index: number) => {
          const supporterOrganizations: SupporterOrganizationsItemWithSummaryMonth =
            new SupporterOrganizationsItemWithSummaryMonth()
          const filteredSupporterOrganizations:
            | SupporterOrganizationsItem
            | undefined = elm.supporterOrganizations.filter(
            (elm2: SupporterOrganizationsItem) =>
              elm2.supporterOrganizationName === type
          )[0]
          supporterOrganizations.supporterOrganizationName = type
          if (filteredSupporterOrganizations) {
            Object.assign(
              supporterOrganizations,
              filteredSupporterOrganizations
            )
          }
          supporterOrganizations.summaryMonth =
            this.surveySummarySupporterOrganizations.surveys[index].summaryMonth
          return supporterOrganizations
        }
      )
    },
    /**
     * 対象の支援者組織別集計情報配列を返す
     * @returns 支援者組織別集計情報配列
     */
    getOrganizations(): SupporterOrganizationsItemWithSummaryMonth[] {
      const list: string[] = []
      if (this.surveySummarySupporterOrganizations.termSummaryResult) {
        for (const i in this.surveySummarySupporterOrganizations
          .termSummaryResult) {
          const elm =
            this.surveySummarySupporterOrganizations.termSummaryResult[i]
          list.push(elm.supporterOrganizationName)
        }
      }
      const organizations: SupporterOrganizationsItemWithSummaryMonth[] = []
      list.forEach((elm: string) => {
        organizations.push(this.getOrganization(elm))
      })
      return organizations
    },
    /**
     * グラフデータのオブジェクトを返す
     * @returns グラフデータオブジェクト
     */
    getOrganizationData(
      organization: SupporterOrganizationsItemWithSummaryMonth[]
    ) {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === 'serviceOverallSatisfaction') {
          results[i] = 0
        } else if (this.type === 'completionOverallSatisfaction') {
          results[i] = 0
        } else if (this.type === 'continuation') {
          results[i] = 0
        } else if (this.type === 'recommended') {
          results[i] = 0
        } else if (this.type === 'evaluation') {
          results[i] = 0
        }
        const label = labels[i]
        if (organization && organization.length) {
          for (const i2 in organization) {
            const elm = organization[i2]
            // 対象の値が存在する場合は値を代入
            if (elm.summaryMonth === label) {
              if (this.type === 'serviceOverallSatisfaction' && elm) {
                results[i] = elm.serviceSatisfactionAverage
                  ? elm.serviceSatisfactionAverage
                  : 0
              } else if (this.type === 'completionOverallSatisfaction') {
                results[i] = elm.completionSatisfactionAverage
                  ? elm.completionSatisfactionAverage
                  : 0
              } else if (this.type === 'continuation') {
                results[i] = elm.completionContinuationPercent
                  ? elm.completionContinuationPercent
                  : 0
              } else if (this.type === 'recommended') {
                results[i] = elm.completionRecommendedAverage
                  ? elm.completionRecommendedAverage
                  : 0
              } else if (this.type === 'evaluation') {
                results[i] = elm.totalSatisfactionAverage
                  ? elm.totalSatisfactionAverage
                  : 0
              }
              break
            }
          }
        }
      }
      return results
    },
    /**
     * グラフ表示オブジェクトを返す
     * @returns グラフ表示オブジェクト
     */
    chartData() {
      return {
        labels: this.getLabels().map((summaryMonth: string) => {
          const yearMonth = String(summaryMonth).split('/')
          const year = yearMonth[0]
          // 月表示の0抜きをする
          const month = String(Number(yearMonth[1]))
          return this.$t('common.format.yearMonth', {
            year: String(year),
            month: String(month),
          }) as string
        }),
        datasets: this.getOrganizations().map((elm: any, index: number) => {
          return {
            type: 'line',
            label: elm[0].supporterOrganizationName,
            data: this.getOrganizationData(elm),
            borderColor: this.colors[index],
            backgroundColor: this.colors[index],
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          }
        }),
      }
    },
    /**
     * グラフ表示設定オブジェクトを返す
     * @returns 表示設定オブジェクト
     */
    chartOptions() {
      const self = this
      return {
        responsive: true,
        plugins: {
          legend: {
            labels: {
              boxWidth: 22,
              boxHeight: 10,
            },
          },
          datalabels: {
            display: false,
          },
        },
        scales: {
          A: {
            title: {
              display: true,
              text: self.labels[self.type],
              padding: { top: 0, left: 0, right: 0, bottom: 10 },
            },
            ticks: {
              stepSize: 1,
            },
            position: 'left',
            suggestedMin: 0,
            suggestedMax: 5,
          },
          B: {
            position: 'right',
            ticks: {
              display: false,
            },
            grid: { display: false },
          },
          x: {
            ticks: {
              callback(index: any) {
                const label = self.chartData().labels[index]
                if (index % 2 !== 0 && index !== 11) return label
              },
            },
          },
        },
      }
    },
  },
})
</script>

<style lang="scss" scoped></style>
