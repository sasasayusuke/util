<template>
  <Sheet class="pt-5 pb-12 px-8" elevation="3" color="#fff" rounded>
    <v-container pa-0>
      <v-row no-gutters>
        <v-col>
          <Title class="type5">
            {{ $t('survey.pages.list.service.AverageSatisfactionMonth') }}
          </Title>
          <Sheet style-set="outlined" class="mt-3 pa-3">
            <Chart
              type="line"
              chart-id="survey-list-chart-1"
              :height="140"
              :chart-data="chartData1"
              :chart-options="chartOptions1"
            />
          </Sheet>
          <Table class="mt-3">
            <template #header>
              <tr>
                <th>&nbsp;</th>
                <th v-for="(item, index) in chartData1.labels" :key="index">
                  {{ item }}
                </th>
              </tr>
            </template>
            <template #body>
              <tr v-for="(item, index) in chartData1.datasets" :key="index">
                <th>{{ item.label }}</th>
                <td v-for="(data, data_index) in item.data" :key="data_index">
                  {{ data.toLocaleString() }}
                </td>
              </tr>
            </template>
          </Table>
        </v-col>
      </v-row>
      <v-row no-gutters class="mt-10">
        <v-col>
          <Title class="type5">
            {{ $t('survey.pages.list.service.AverageSatisfactionCumulative') }}
          </Title>
          <Sheet style-set="outlined" class="mt-3 pa-3">
            <Chart
              type="line"
              chart-id="survey-list-chart-2"
              :height="140"
              :chart-data="chartData2"
              :chart-options="chartOptions2"
            />
          </Sheet>
          <Table class="mt-3">
            <template #header>
              <tr>
                <th>&nbsp;</th>
                <th v-for="(item, index) in chartData2.labels" :key="index">
                  {{ item }}
                </th>
              </tr>
            </template>
            <template #body>
              <tr v-for="(item, index) in chartData2.datasets" :key="index">
                <th>{{ item.label }}</th>
                <td v-for="(data, data_index) in item.data" :key="data_index">
                  {{ data.toLocaleString() }}
                </td>
              </tr>
            </template>
          </Table>
          <Table class="mt-3">
            <template #header>
              <tr>
                <th>&nbsp;</th>
                <th v-for="(item, index) in chartData3.labels" :key="index">
                  {{ item }}
                </th>
              </tr>
            </template>
            <template #body>
              <tr v-for="(item, index) in chartData3.datasets" :key="index">
                <th>{{ item.label }}</th>
                <td v-for="(data, data_index) in item.data" :key="data_index">
                  {{ data.toLocaleString() }}
                </td>
              </tr>
            </template>
          </Table>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Title, Table } from '~/components/common/atoms'
import Chart from '~/components/common/molecules/Chart.vue'
import { SurveyListSummaryItem, SurveyListSummaryTotal } from '~/models/Survey'

export class DataSet {
  type: string = 'line'
  label: string = ''
  data: number[] = []
  borderColor: string = ''
  backgroundColor: string = ''
  borderJoinStyle: string = ''
  pointRadius: number = 0
  borderWidth: number = 0
  stepped: boolean = false
  yAxisID: string = ''
  hidden: boolean = false
}

export class ChartData {
  labels: string[] = []
  datasets: DataSet[] = []
}

export class TotalDataSet {
  label: string = ''
  data: number[] = []
  hidden: boolean = false
}

export class TotalChartData {
  labels: string[] = []
  datasets: TotalDataSet[] = []
}

export default BaseComponent.extend({
  components: {
    Sheet,
    Title,
    Table,
    Chart,
  },
  computed: {
    /**
     * 満足度の平均値（単月）のグラフ表示オブジェクトを返す
     * @returns グラフ表示オブジェクト
     */
    chartData1(this: any) {
      const result = new ChartData()
      let summaryMonthly: SurveyListSummaryItem[] = []
      if (
        this.surveysResponse.summary &&
        this.surveysResponse.summary.monthly
      ) {
        summaryMonthly = this.surveysResponse.summary.monthly
      }
      const labels = this.getLabels(summaryMonthly)
      result.labels = labels
      if (this.type !== 'quick') {
        const satisfactionAverageDataset = this.getDataSet(
          this.$t('survey.pages.list.service.AverageSatisfactionMonth'),
          'satisfactionAverage',
          labels,
          summaryMonthly
        )
        satisfactionAverageDataset.borderColor = '#ff061b'
        satisfactionAverageDataset.backgroundColor = '#ff061b'
        satisfactionAverageDataset.pointRadius = 0
        satisfactionAverageDataset.borderWidth = 2
        satisfactionAverageDataset.yAxisID = 'A'
        result.datasets.push(satisfactionAverageDataset)
      }
      const receivesDataset = this.getDataSet(
        this.$t('survey.pages.list.service.receiveMonth'),
        'receive',
        labels,
        summaryMonthly
      )
      receivesDataset.borderColor = '#297aed'
      receivesDataset.backgroundColor = '#297aed'
      receivesDataset.pointRadius = 0
      receivesDataset.borderWidth = 2
      receivesDataset.stepped = true
      receivesDataset.yAxisID = 'B'
      result.datasets.push(receivesDataset)
      return result
    },
    /**
     * 満足度の平均値（累積）のグラフ表示オブジェクトを返す
     * @returns グラフ表示オブジェクト
     */
    chartData2(this: any) {
      const result = new ChartData()
      let summaryAccumulation: SurveyListSummaryItem[] = []
      if (
        this.surveysResponse.summary &&
        this.surveysResponse.summary.accumulation
      ) {
        summaryAccumulation = this.surveysResponse.summary.accumulation
      }
      const labels = this.getLabels(summaryAccumulation)
      result.labels = labels
      if (this.type !== 'quick') {
        const satisfactionAverageDataset = this.getDataSet(
          this.$t('survey.pages.list.service.AverageSatisfactionCumulative'),
          'satisfactionAverage',
          labels,
          summaryAccumulation
        )
        satisfactionAverageDataset.borderColor = '#ff061b'
        satisfactionAverageDataset.backgroundColor = '#ff061b'
        satisfactionAverageDataset.pointRadius = 0
        satisfactionAverageDataset.borderWidth = 2
        satisfactionAverageDataset.yAxisID = 'A'
        result.datasets.push(satisfactionAverageDataset)
      }
      const receivesDataset = this.getDataSet(
        this.$t('survey.pages.list.service.receiveCumulative'),
        'receive',
        labels,
        summaryAccumulation
      )
      receivesDataset.borderColor = '#297aed'
      receivesDataset.backgroundColor = '#297aed'
      receivesDataset.borderJoinStyle = 'round'
      receivesDataset.pointRadius = 0
      receivesDataset.borderWidth = 2
      receivesDataset.stepped = true
      receivesDataset.yAxisID = 'B'
      result.datasets.push(receivesDataset)
      return result
    },
    /**
     * 満足度の平均値およびN数のテーブル表示オブジェクトを返す
     * @returns テーブル表示オブジェクト
     */
    chartData3(this: any) {
      const result = new TotalChartData()
      let summaryTotal = new SurveyListSummaryTotal()
      if (this.surveysResponse.summary && this.surveysResponse.summary.total) {
        summaryTotal = this.surveysResponse.summary.total
      }
      const labels = ['1Q', '2Q', '3Q', '4Q', '1H', '2H', 'FY']
      result.labels = labels
      if (this.type !== 'quick') {
        const satisfactionAverageDataset = new TotalDataSet()
        satisfactionAverageDataset.label = String(
          this.$t('survey.pages.list.service.AverageSatisfactionCumulative')
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.quota.quota1.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.quota.quota2.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.quota.quota3.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.quota.quota4.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.half.half1.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.half.half2.satisfactionAverage
        )
        satisfactionAverageDataset.data.push(
          summaryTotal.year.satisfactionAverage
        )
        result.datasets.push(satisfactionAverageDataset)
      }
      const receivesDataset = new TotalDataSet()
      receivesDataset.label = String(
        this.$t('survey.pages.list.service.receiveCumulative')
      )
      receivesDataset.data.push(summaryTotal.quota.quota1.receive)
      receivesDataset.data.push(summaryTotal.quota.quota2.receive)
      receivesDataset.data.push(summaryTotal.quota.quota3.receive)
      receivesDataset.data.push(summaryTotal.quota.quota4.receive)
      receivesDataset.data.push(summaryTotal.half.half1.receive)
      receivesDataset.data.push(summaryTotal.half.half2.receive)
      receivesDataset.data.push(summaryTotal.year.receive)
      result.datasets.push(receivesDataset)
      return result
    },
    /**
     * 満足度の平均値（単月）の表示設定オブジェクトを返す
     * @returns 表示設定オブジェクト
     */
    chartOptions1(this: any) {
      const boolDisplayTitle = this.type !== 'quick'
      const result = {
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
              display: boolDisplayTitle,
              text: String(
                this.$t('survey.pages.list.service.AverageSatisfactionMonth')
              ),
              padding: { top: 0, left: 0, right: 0, bottom: 30 },
            },
            ticks: {
              min: 0,
              stepSize: 1,
            },
            position: 'left',
            suggestedMin: 0,
            suggestedMax: 5,
          },
          B: {
            title: {
              display: true,
              text: String(this.$t('survey.pages.list.service.receiveMonth')),
              padding: { top: 0, left: 0, right: 0, bottom: 30 },
            },
            ticks: {
              min: 0,
              max: 10,
              stepSize: 2,
            },
            position: 'right',
            suggestedMin: 0,
            suggestedMax: 10,
            grid: {
              display: false,
            },
          },
          x: {
            grid: { display: false },
          },
        },
      }
      return result
    },
    /**
     * 満足度の平均値（累積）の表示設定オブジェクトを返す
     * @returns 表示設定オブジェクト
     */
    chartOptions2(this: any) {
      const boolDisplayTitle = this.type !== 'quick'
      const result = {
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
              display: boolDisplayTitle,
              text: String(
                this.$t(
                  'survey.pages.list.service.AverageSatisfactionCumulative'
                )
              ),
              padding: { top: 0, left: 0, right: 0, bottom: 30 },
            },
            ticks: {
              min: 0,
              stepSize: 1,
            },
            position: 'left',
            suggestedMin: 0,
            suggestedMax: 5,
          },
          B: {
            title: {
              display: true,
              text: String(
                this.$t('survey.pages.list.service.receiveCumulative')
              ),
              padding: { top: 0, left: 0, right: 0, bottom: 30 },
            },
            ticks: {
              min: 0,
              stepSize: 25,
            },
            position: 'right',
            suggestedMin: 0,
            suggestedMax: 100,
            grid: {
              display: false,
            },
          },
          x: {
            grid: { display: false },
          },
        },
      }
      return result
    },
  },
  methods: {
    /**
     * アンケートサマリ配列からグラフ表示内の月表示ラベルを配列で返す
     * @param summaryMonthly アンケートサマリ配列
     * @returns 月表示ラベル配列
     */
    getLabels(summaryMonthly: SurveyListSummaryItem[]) {
      const results = []
      if (summaryMonthly.length > 0) {
        for (const i in summaryMonthly) {
          results.push(summaryMonthly[i].month)
        }
      } else {
        for (let i = 0; i < 12; i++) {
          const year = parseInt(
            format(new Date(getCurrentDate().getFullYear(), 3 + i, 1), 'yyyy')
          )
          const month = parseInt(
            format(new Date(getCurrentDate().getFullYear(), 3 + i, 1), 'MM')
          )
          results.push(
            this.$t('common.format.yearMonth', {
              year: String(year),
              month: String(month),
            }) as string
          )
        }
      }
      return results
    },
    /**
     * 描画するグラフデータのオブジェクトを返す
     * @param title 表示タイトル
     * @param key 項目名
     * @param labels 月表示ラベル配列
     * @param data アンケートサマリ配列
     * @returns グラフデータオブジェクト
     */
    getDataSet(
      title: any,
      key: string,
      labels: string[],
      data: SurveyListSummaryItem[]
    ) {
      const result = new DataSet()
      const dataByMonth: any = {}
      for (const i in data) {
        dataByMonth[data[i].month] = {}
        dataByMonth[data[i].month].satisfactionAverage =
          data[i].satisfactionAverage
        dataByMonth[data[i].month].receive = data[i].receive
      }
      for (const i in labels) {
        const month: string = labels[i]
        const value: number = dataByMonth[month] ? dataByMonth[month][key] : 0
        result.data.push(value)
      }
      result.label = String(title)
      return result
    },
  },
  props: {
    /** 表示対象のアンケート種別 */
    type: {
      type: String,
      default: 'service',
    },
    /** アンケート一覧 */
    surveysResponse: {
      type: Object,
    },
  },
  data() {
    return {}
  },
})
</script>
