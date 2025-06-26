<template>
  <DetailContainer
    :title="$t('survey.pages.index.section.kpi')"
    :is-editing="false"
    is-hide-header-button
    is-hide-footer
    is-hide-button1
    is-hide-button2
    note-head="required"
  >
    <template #date>
      {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
    </template>
    <Sheet class="px-7 pt-1 pb-7">
      <SurveyTopKpiSort
        :param="searchParam"
        @update="update"
        @sort="search"
        @clear="clear"
      />
      <v-container fluid pa-0>
        <v-row>
          <v-col>
            <SurveyTopKpiTable
              v-if="!isLoading"
              :surveys="surveySummaryAll.termSummaryResult"
              :set-type="type"
              @changeChart="changeChart"
            />
          </v-col>
          <v-col class="pl-10">
            <Chart
              type="line"
              chart-id="SurveyTopKpiContainer-chart-1"
              :width="530"
              :height="180"
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
import { getCurrentDate } from '~/utils/common-functions'
import { Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyTopKpiSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyTopKpiSort.vue'
import SurveyTopKpiTable from '~/components/survey/molecules/SurveyTopKpiTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import { GetSurveySummaryAllResponse } from '~/models/Survey'
import { GetBatchControlByIdResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    Sheet,
    DetailContainer,
    SurveyTopKpiSort,
    SurveyTopKpiTable,
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
    /** アンケート全集計結果 */
    surveySummaryAll: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** 最終集計日時 */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
    },
  },
  data() {
    return {
      type: 'satisfaction',
      labels: {
        satisfaction: this.$t('survey.pages.index.table.label.satisfaction'),
        continuation: this.$t('survey.pages.index.table.label.continuation'),
        recommended: this.$t('survey.pages.index.table.label.recommended'),
        sales: this.$t('survey.pages.index.table.label.sales'),
      } as any,
      searchParam: new SearchParam(),
      apiName: 'getSurveySummaryAll',
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
    update(item: keyof SearchParam, event: any) {
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
      this.$emit(this.apiName, 'Kpi', request)
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
     * 表示するグラフ種別を変更
     * @param グラフ種別
     */
    changeChart(type: string) {
      this.type = type
    },
    /**
     * サービスアンケートのグラフデータのオブジェクトを返す
     * @returns グラフデータオブジェクト
     */
    getServiceData() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === 'satisfaction') {
          results[i] = 0
        } else if (this.type === 'continuation') {
          results[i] = undefined
        } else if (this.type === 'recommended') {
          results[i] = undefined
        } else if (this.type === 'sales') {
          results[i] = undefined
        }
        const label = labels[i]
        if (
          this.surveySummaryAll.surveys &&
          this.surveySummaryAll.surveys.length
        ) {
          for (const i2 in this.surveySummaryAll.surveys) {
            const elm = this.surveySummaryAll.surveys[i2]
            // 対象の値が存在する場合は値を代入
            if (elm.summaryMonth === label) {
              if (this.type === 'satisfaction') {
                results[i] = elm.service.satisfactionAverage
                  ? elm.service.satisfactionAverage
                  : 0
              } else if (this.type === 'continuation') {
                results[i] = undefined
              } else if (this.type === 'recommended') {
                results[i] = undefined
              } else if (this.type === 'sales') {
                results[i] = undefined
              }
              break
            }
          }
        }
      }
      return results
    },
    /**
     * 修了アンケートのグラフデータのオブジェクトを返す
     * @returns グラフデータオブジェクト
     */
    getCompletionData() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        results[i] = 0
        const label = labels[i]
        if (
          this.surveySummaryAll.surveys &&
          this.surveySummaryAll.surveys.length
        ) {
          for (const i2 in this.surveySummaryAll.surveys) {
            const elm = this.surveySummaryAll.surveys[i2]
            // 対象の値が存在する場合は値を代入
            if (elm.summaryMonth === label) {
              if (this.type === 'satisfaction') {
                results[i] = elm.completion.satisfactionAverage
                  ? elm.completion.satisfactionAverage
                  : 0
              } else if (this.type === 'continuation') {
                results[i] = elm.completion.continuation.positivePercent
                  ? elm.completion.continuation.positivePercent
                  : 0
              } else if (this.type === 'recommended') {
                results[i] = elm.completion.recommendedAverage
                  ? elm.completion.recommendedAverage
                  : 0
              } else if (this.type === 'sales') {
                results[i] = elm.completion.salesAverage
                  ? elm.completion.salesAverage
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
     * サービスアンケート/修了アンケート合算のグラフデータのオブジェクトを返す
     * @returns グラフデータオブジェクト
     */
    getServiceAndCompletion() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === 'satisfaction') {
          results[i] = 0
        } else if (this.type === 'continuation') {
          results[i] = undefined
        } else if (this.type === 'recommended') {
          results[i] = undefined
        } else if (this.type === 'sales') {
          results[i] = undefined
        }
        const label = labels[i]
        if (
          this.surveySummaryAll.surveys &&
          this.surveySummaryAll.surveys.length
        ) {
          for (const i2 in this.surveySummaryAll.surveys) {
            const elm = this.surveySummaryAll.surveys[i2]
            // 対象の値が存在する場合は値を代入
            if (elm.summaryMonth === label) {
              const service = elm.service.satisfactionAverage
                ? elm.service.satisfactionSummary
                : 0
              const completion = elm.completion.satisfactionAverage
                ? elm.completion.satisfactionSummary
                : 0
              const serviceReceiveCount = elm.service.receiveCount
                ? elm.service.receiveCount
                : 0
              const completionReceiveCount = elm.completion.receiveCount
                ? elm.completion.receiveCount
                : 0
              const serviceAndCompletionSatisfactionSummary =
                service + completion
              const serviceAndCompletionReceiveCount =
                serviceReceiveCount + completionReceiveCount

              if (this.type === 'satisfaction') {
                if (
                  serviceAndCompletionSatisfactionSummary === 0 &&
                  serviceAndCompletionReceiveCount === 0
                ) {
                  results[i] = 0
                } else {
                  results[i] = +(
                    serviceAndCompletionSatisfactionSummary /
                    serviceAndCompletionReceiveCount
                  ).toFixed(1)
                }
              } else if (this.type === 'continuation') {
                results[i] = undefined
              } else if (this.type === 'recommended') {
                results[i] = undefined
              } else if (this.type === 'sales') {
                results[i] = undefined
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
        datasets: [
          {
            type: 'line',
            label: this.$t('survey.pages.index.table.label.service'),
            data: this.getServiceData(),
            borderColor: '#ff061b',
            backgroundColor: '#ff061b',
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          },
          {
            type: 'line',
            label: this.$t('survey.pages.index.table.label.completion'),
            data: this.getCompletionData(),
            borderColor: '#297aed',
            backgroundColor: '#297aed',
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          },
          {
            type: 'line',
            label: this.$t(
              'survey.pages.index.table.label.serviceAndCompletion'
            ),
            data: this.getServiceAndCompletion(),
            borderColor: '#ebbb2d',
            backgroundColor: '#ebbb2d',
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          },
        ],
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
              stepSize() {
                if (self.type === 'continuation') {
                  return 25
                } else {
                  return 1
                }
              },
              callback(value: any) {
                if (self.type === 'continuation') {
                  return value + '%'
                } else {
                  return value
                }
              },
            },
            position: 'left',
            suggestedMin: 0,
            suggestedMax() {
              // TODO: 他の項目のMAX値が不明なので必要なら設定してください
              if (self.type === 'continuation') {
                return 100
              } else {
                return 5
              }
            },
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
