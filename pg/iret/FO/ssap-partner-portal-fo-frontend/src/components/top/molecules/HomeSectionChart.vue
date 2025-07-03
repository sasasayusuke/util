<template>
  <Sheet>
    <v-container fluid pa-0>
      <v-row no-gutters>
        <v-col>
          <HomeSectionTable
            :surveys="surveySummarySupporterOrganizations.termSummaryResult"
            :total="surveySummarySupporterOrganizations.totalSummaryResult"
            @changeChart="changeChart"
          />
        </v-col>
        <v-col class="pl-10">
          <Chart
            type="line"
            chart-id="HomeSectionChart-1"
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
import { Sheet, TextLink, Button } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import HomeSectionTable from '~/components/top/molecules/HomeSectionTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import {
  SupporterOrganizationSurveysItem,
  SupporterOrganizationsItem,
} from '~/models/Survey'
import { ENUM_SURVEY_SUMMARY } from '~/types/Survey'

export class SupporterOrganizationsItemWithSummaryMonth extends SupporterOrganizationsItem {
  summaryMonth: string = ''
}

export default BaseComponent.extend({
  components: {
    Sheet,
    TextLink,
    Button,
    DetailContainer,
    HomeSectionTable,
    Chart,
  },
  props: {
    /**
     * 課別集計 表・グラフデータ
     */
    surveySummarySupporterOrganizations: {
      type: Object,
      required: true,
    },
    /**
     * 表示期間 絞り込み指定年月
     */
    surveySummarySupporterOrganizationsParams: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      type: 'serviceOverallSatisfaction',
      labels: {
        serviceOverallSatisfaction: this.$t(
          'top.pages.home.table.service.overallSatisfaction'
        ),
        completionOverallSatisfaction: this.$t(
          'top.pages.home.table.completion.overallSatisfaction'
        ),
        continuation: this.$t('top.pages.home.table.completion.continuation'),
        recommended: this.$t('top.pages.home.table.completion.recommended2'),
        evaluation: this.$t('top.pages.home.table.label.chartSales'),
      } as { [key: string]: string },
      colors: [
        '#4688f1',
        '#e8453c',
        '#f9bb2d',
        '#3aa757',
        '#fd6d22',
        '#4cbdc5',
        '#7dacf4',
      ],
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
          this.surveySummarySupporterOrganizationsParams.yearMonthFrom,
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
          this.surveySummarySupporterOrganizationsParams.yearMonthTo,
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
     * 各課の集計値を取得
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
     * APIから取得した全ての課の集計値を取得
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
    getOrganizationData(
      organization: SupporterOrganizationsItemWithSummaryMonth[]
    ) {
      const results: (number | undefined)[] = []
      const labels = this.getLabels()
      for (const i in labels) {
        // 表示項目による初期値を設定
        if (this.type === ENUM_SURVEY_SUMMARY.SERVICEOVERALLSATISFACTION) {
          results[i] = 0
        } else if (
          this.type === ENUM_SURVEY_SUMMARY.COMPLETIONOVERALLSATISFACTION
        ) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
          results[i] = 0
        } else if (this.type === ENUM_SURVEY_SUMMARY.EVALUATION) {
          results[i] = 0
        }
        const label = labels[i]
        if (organization && organization.length) {
          for (const i2 in organization) {
            const elm = organization[i2]
            // 対象の値が存在する場合は値を代入
            if (elm.summaryMonth === label) {
              if (
                this.type === ENUM_SURVEY_SUMMARY.SERVICEOVERALLSATISFACTION &&
                elm
              ) {
                results[i] = elm.serviceSatisfactionAverage
                  ? elm.serviceSatisfactionAverage
                  : 0
              } else if (
                this.type === ENUM_SURVEY_SUMMARY.COMPLETIONOVERALLSATISFACTION
              ) {
                results[i] = elm.completionSatisfactionAverage
                  ? elm.completionSatisfactionAverage
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.CONTINUATION) {
                results[i] = elm.completionContinuationPercent
                  ? elm.completionContinuationPercent
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.RECOMMENDED) {
                results[i] = elm.completionRecommendedAverage
                  ? elm.completionRecommendedAverage
                  : 0
              } else if (this.type === ENUM_SURVEY_SUMMARY.EVALUATION) {
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
     * グラフのデータやラベルの生成
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
