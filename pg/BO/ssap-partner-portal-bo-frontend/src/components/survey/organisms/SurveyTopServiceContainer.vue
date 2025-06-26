<template>
  <DetailContainer
    :title="$t('survey.pages.index.section.service')"
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
      <SurveyTopServiceSort
        :param="searchParam"
        @update="update"
        @sort="search"
        @clear="clear"
      />
      <v-container fluid pa-0>
        <v-row no-gutters>
          <v-col>
            <SurveyTopServiceTable
              v-if="!isLoading"
              :surveys="surveySummaryAll.termSummaryResult"
              :set-type="type"
            />
            <p class="mt-5 mb-0">
              <TextLink to="/survey/list?type=service">
                {{ $t('survey.pages.buttons.showSurveyList') }}
              </TextLink>
            </p>
          </v-col>
          <v-col class="pl-10">
            <Chart
              type="line"
              chart-id="SurveyTopServiceContainer-chart-1"
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
import { Sheet, TextLink } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyTopServiceSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyTopServiceSort.vue'
import SurveyTopServiceTable from '~/components/survey/molecules/SurveyTopServiceTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import { GetSurveySummaryAllResponse } from '~/models/Survey'
import { GetBatchControlByIdResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    Sheet,
    TextLink,
    DetailContainer,
    SurveyTopServiceSort,
    SurveyTopServiceTable,
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
      header: [
        {
          name: 'hoge',
        },
        {
          name: 'fuga',
          emphasize: true,
        },
      ],
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
      this.$emit(this.apiName, 'Service', request)
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
     * 描画するグラフデータのオブジェクトを返す
     * @returns グラフデータオブジェクト
     */
    getServiceData() {
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
              results[i] = elm.service.satisfactionAverage
                ? elm.service.satisfactionAverage
                : 0
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
            display: false,
          },
          datalabels: {
            display: false,
          },
        },
        scales: {
          A: {
            title: {
              display: true,
              text: this.$t('survey.pages.index.table.label.satisfaction'),
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
