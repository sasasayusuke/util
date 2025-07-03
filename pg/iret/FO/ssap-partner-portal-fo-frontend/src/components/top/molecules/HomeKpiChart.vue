<template>
  <!-- <Sheet class="px-7 pt-1 pb-7"> -->
  <Sheet>
    <v-container fluid pa-0>
      <v-row>
        <v-col>
          <HomeKpiTable
            :surveys="surveySummaryAll.termSummaryResult"
            @changeChart="changeChart"
          />
        </v-col>
        <v-col class="pl-10">
          <Chart
            type="line"
            chart-id="HomeKpiChart-1"
            :width="530"
            :height="180"
            :chart-data="chartData()"
            :chart-options="chartOptions()"
          />
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import { format, parse } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import { Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import HomeKpiTable from '~/components/top/molecules/HomeKpiTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import { ENUM_SURVEY_SUMMARY } from '~/types/Survey'

export default BaseComponent.extend({
  components: {
    Sheet,
    HomeKpiTable,
    Chart,
  },
  props: {
    /**
     * 担当案件集計データ
     */
    surveySummaryAll: {
      type: Object,
      required: true,
    },
    /**
     * 担当案件集計 表示期間絞り込み
     */
    surveySummaryAllParams: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      type: 'satisfaction',
      labels: {
        satisfaction: this.$t('top.pages.home.table.label.satisfaction'),
        continuation: this.$t('top.pages.home.table.label.continuation'),
        recommended: this.$t('top.pages.home.table.label.recommended'),
        sales: this.$t('top.pages.home.table.label.sales'),
      } as { [key: string]: string },
    }
  },
  methods: {
    /**
     * アンケート種別変更によるグラフの再描画
     */
    changeChart(type: string) {
      this.type = type
    },
    /**
     * 表示期間Fromの書式整形
     */
    getFromParam() {
      return format(
        parse(
          this.surveySummaryAllParams.yearMonthFrom,
          this.$t('common.format.date_ym3') as string,
          getCurrentDate()
        ),
        this.$t('common.format.date_ym2') as string
      ) as string
    },
    /**
     * 表示期間Toの書式整形
     */
    getToParam() {
      return format(
        parse(
          this.surveySummaryAllParams.yearMonthTo,
          this.$t('common.format.date_ym3') as string,
          getCurrentDate()
        ),
        this.$t('common.format.date_ym2') as string
      ) as string
    },
    /**
     * グラフの横軸ラベルの取得
     */
    getLabels() {
      const results: string[] = []
      let i = 0
      while (true) {
        const yearMonth = parse(
          this.getFromParam(),
          this.$t('common.format.date_ym2') as string,
          getCurrentDate()
        )
        yearMonth.setMonth(yearMonth.getMonth() + i)
        const date = format(
          yearMonth,
          this.$t('common.format.date_ym2') as string
        ) as string
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
     * サービスアンケートの各集計結果を取得
     */
    getServiceData() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
          results[i] = undefined
        } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
          results[i] = undefined
        } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
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
              if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
                results[i] = elm.serviceSatisfactionAverage
                  ? elm.serviceSatisfactionAverage
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
                results[i] = undefined
              } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
                results[i] = undefined
              } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
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
     * 修了アンケートの各集計結果を取得
     */
    getCompletionData() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
          results[i] = 0
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
              if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
                results[i] = elm.completionSatisfactionAverage
                  ? elm.completionSatisfactionAverage
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
                results[i] = elm.completionContinuation.positivePercent
                  ? elm.completionContinuation.positivePercent
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
                results[i] = elm.completionRecommendedAverage
                  ? elm.completionRecommendedAverage
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
                results[i] = elm.completionSalesAverage
                  ? elm.completionSalesAverage
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
     * サービス／修了アンケート合計の各集計結果を取得
     */
    getServiceAndCompletion() {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
          results[i] = undefined
        } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
          results[i] = undefined
        } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
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
              const serviceSummary = elm.serviceSatisfactionSummary
                ? elm.serviceSatisfactionSummary
                : 0
              const completionSummary = elm.completionSatisfactionSummary
                ? elm.completionSatisfactionSummary
                : 0
              const serviceReceive = elm.serviceReceive ? elm.serviceReceive : 0
              const completionReceive = elm.completionReceive
                ? elm.completionReceive
                : 0
              if (this.type === ENUM_SURVEY_SUMMARY.SATISFACTION) {
                results[i] =
                  (serviceSummary + completionSummary) /
                  (serviceReceive + completionReceive)
              } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
                results[i] = undefined
              } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
                results[i] = undefined
              } else if (this.type === ENUM_SURVEY_SUMMARY.SALES) {
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
     * グラフのデータやラベルの生成
     */
    chartData() {
      // TODO: ダミーのデータが全部０でわかりにくいので
      // +Math.round(Math.random() * 5)などで足しているので実際のデータがきたら消してください。
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
            label: this.$t('top.pages.home.table.label.service'),
            data: this.getServiceData(),
            borderColor: '#297aed',
            backgroundColor: '#297aed',
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          },
          {
            type: 'line',
            label: this.$t('top.pages.home.table.label.completion'),
            data: this.getCompletionData(),
            borderColor: '#ff061b',
            backgroundColor: '#ff061b',
            pointRadius: 0,
            borderWidth: 2,
            yAxisID: 'A',
          },
          {
            type: 'line',
            label: this.$t('top.pages.home.table.label.serviceAndCompletion'),
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
     * グラフの目盛りの単位など各種設定
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
                if (self.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
                  return 25
                } else {
                  return 1
                }
              },
              callback(value: any) {
                if (self.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
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
              if (self.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
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
