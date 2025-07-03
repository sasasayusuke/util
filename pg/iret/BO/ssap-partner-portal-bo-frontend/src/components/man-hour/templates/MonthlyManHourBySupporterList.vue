<template>
  <RootTemplate>
    <!-- ヘッダ -->
    <ListInPageHeader
      month-change
      :next-month-disabled="nextMonthDisabled"
      @buttonAction1="thisMonth"
      @buttonAction2="lastMonth"
      @buttonAction3="nextMonth"
    >
      {{ $t('man-hour.manHourTotal.name') }}&nbsp;&nbsp; {{ `${year()}`
      }}{{ $t('man-hour.unit.year') }}{{ `${month()}`
      }}{{ $t('man-hour.unit.month') }}</ListInPageHeader
    >
    <!-- ソート -->
    <ManHourSort
      :is-loading="isLoading.supporterOrganizations"
      :param="searchParam"
      :supporter-organizations="supporterOrganizations.supporterOrganizations"
      only-organization
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <!-- 月次担当者別工数一覧 -->
    <ManHourSupporterListTable
      :is-loading="isLoading.supporterManHours"
      :supporter-man-hours="response.supporterManHours.manHours"
      class="mt-6"
      @csvOutput="$emit('csvOutput')"
    />
    <!-- サービス別工数稼働率 -->
    <DetailContainer
      :is-loading="isLoading"
      :title="
        setSummaryFiscalYearMonth + $t('man-hour.pages.supporter.chart1.title')
      "
      :is-editing="false"
      is-hide-button1
      is-hide-button2
      is-hide-footer
      class="mt-8"
    >
      <!-- サービス別工数稼働率グラフ -->
      <Chart
        type="bar"
        chart-id="MonthlyManHourBySupporterList-chart-1"
        :height="330"
        :chart-data="chartData1()"
        :chart-options="chartOptions1()"
        class="px-5 pb-5"
      />
    </DetailContainer>
    <!-- 案件別契約時間と支援工数内訳（対面支援） -->
    <DetailContainer
      v-if="
        !isLoading.serviceTypesManHours &&
        !isLoading.alertSettings &&
        response.serviceTypesManHours.length
      "
      :title="setSummaryYearMonth + $t('man-hour.pages.supporter.chart2.title')"
      :is-editing="false"
      is-hide-button1
      is-hide-button2
      is-hide-header-button
      is-hide-footer
      class="mt-8"
    >
      <template #date>
        {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
      </template>
      <v-container pa-0 mt-6 mb-5 fluid>
        <v-row no-gutters justify="center" align="center">
          <v-col cols="auto">
            <!-- サービス種別名毎ボタン -->
            <GroupButton>
              <Button
                v-for="(item, index) in response.serviceTypesManHours"
                :key="index"
                :outlined="chart2Id !== item.serviceTypeId"
                style-set="group-small-auto"
                @click="chartData2((chart2Id = item.serviceTypeId))"
              >
                {{ item.serviceTypeName }}
              </Button>
            </GroupButton>
          </v-col>
        </v-row>
      </v-container>
      <!-- 案件別契約時間と支援工数内訳（対面支援）グラフ -->
      <Chart
        v-if="!isLoading.serviceTypesManHours"
        type="bar"
        chart-id="MonthlyManHourBySupporterList-chart-2"
        :height="330"
        :chart-data="chartData2()"
        :chart-options="chartOptions2()"
        class="px-5 pb-5"
      />
    </DetailContainer>
    <!-- 案件別契約時間と支援工数内訳（対面支援＋支援仕込） -->
    <DetailContainer
      v-if="
        !isLoading.serviceTypesManHours &&
        !isLoading.alertSettings &&
        response.serviceTypesManHours.length
      "
      :title="setSummaryYearMonth + $t('man-hour.pages.supporter.chart3.title')"
      :is-editing="false"
      is-hide-button1
      is-hide-button2
      is-hide-header-button
      is-hide-footer
      class="mt-8"
    >
      <template #date>
        {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
      </template>
      <v-container pa-0 mt-6 mb-5 fluid>
        <v-row no-gutters justify="center" align="center">
          <v-col cols="auto">
            <!-- サービス種別名毎ボタン -->
            <GroupButton>
              <Button
                v-for="(item, index) in response.serviceTypesManHours"
                :key="index"
                :outlined="chart3Id !== item.serviceTypeId"
                style-set="group-small-auto"
                @click="chartData3((chart3Id = item.serviceTypeId))"
              >
                {{ item.serviceTypeName }}
              </Button>
            </GroupButton>
          </v-col>
        </v-row>
      </v-container>
      <!-- 案件別契約時間と支援工数内訳（対面支援＋支援仕込）グラフ -->
      <Chart
        type="bar"
        chart-id="MonthlyManHourBySupporterList-chart-3"
        :height="330"
        :chart-data="chartData3()"
        :chart-options="chartOptions3()"
        class="px-5 pb-5"
      />
    </DetailContainer>
    <!-- Sony Acceleration Platform粗利シミュレーションとキーとなる指標 -->
    <DetailContainer
      :title="setSummaryYearMonth + $t('man-hour.pages.supporter.chart4.title')"
      :is-editing="false"
      is-hide-button1
      is-hide-button2
      is-hide-header-button
      is-hide-footer
      class="mt-8"
    >
      <template #date>
        {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
      </template>
      <div class="pa-7">
        <Table class="is-wide">
          <template #header>
            <tr>
              <th>&nbsp;</th>
              <template
                v-for="(item, index) in response.supporterOrganizationsManHours"
              >
                <th :key="index">
                  {{ item.supporterOrganizationName }}
                </th>
              </template>
              <th>{{ $t('common.label.total') }}</th>
            </tr>
          </template>
          <template #body>
            <tr v-for="(item, index) in supporterTableLabels" :key="index">
              <th>{{ item.text }}</th>
              <td
                v-for="(i, n) in response.supporterOrganizationsManHours"
                :key="n"
              >
                {{
                  formatSupporterOrganizationsManHours(i[item.key], item, false)
                }}
              </td>
              <td>
                {{
                  formatSupporterOrganizationsManHours(
                    totalData()[item.key],
                    item,
                    true
                  )
                }}
              </td>
            </tr>
          </template>
        </Table>
      </div>
    </DetailContainer>
  </RootTemplate>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import { format, parse, parseISO } from 'date-fns'
import DetailContainer from '../../common/organisms/DetailContainer.vue'
import { getCurrentDate } from '~/utils/common-functions'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import ManHourSort, {
  SummarySupporterOrganizationsManHoursSearchParam,
} from '~/components/man-hour/organisms/ManHourSort.vue'
import ManHourSupporterListTable from '~/components/man-hour/organisms/ManHourSupporterListTable.vue'
import Chart from '~/components/common/molecules/Chart.vue'
import { Table, Button } from '~/components/common/atoms'
import GroupButton from '~/components/common/molecules/GroupButton.vue'
import { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  GetSupporterOrganizationsResponse,
  GetSummarySupporterOrganizationsManHoursRequest,
} from '@/models/ManHour'
import { GetBatchControlByIdResponse } from '~/models/Master'

export interface isLoading {
  supporterOrganizations: boolean
  supporterManHours: boolean
  serviceTypesManHours: boolean
  supporterOrganizationsManHours: boolean
  batchControl: boolean
  alertSettings: boolean
}

// totalData() の型(interface)定義
export interface TotalData {
  [key: string]: number
}

// 表ラベルの型(interface)定義
export interface supporterTableLabel {
  text: string
  key: string
}

export default CommonList.extend({
  name: 'MonthlyManHourBySupporterList',
  components: {
    RootTemplate,
    ListInPageHeader,
    ManHourSort,
    ManHourSupporterListTable,
    DetailContainer,
    Chart,
    Table,
    Button,
    GroupButton,
  },
  props: {
    /**
     * 月次担当者別工数一覧
     * サービス種別別工数指標
     * 支援者組織（課）別工数指標
     * 工数アラート設定
     */
    response: {
      type: Object,
      required: true,
    },
    /** 支援者組織の一覧 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** 各種集計バッチ処理の最終完了日時 */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** ソートで課のみ指定 */
    onlyOrganization: {
      type: Boolean,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: SummarySupporterOrganizationsManHoursSearchParam,
      RequestType: GetSummarySupporterOrganizationsManHoursRequest,
      apiName: 'getSummarySupporterOrganizationsManHours',
      pageName: this.$t('man-hour.pages.supporter.index.name'),
      summaryFiscalYearMonth: this.$t(
        'man-hour.pages.supporter.fiscalYearMonth'
      ),
      summaryYearMonth: this.$t('man-hour.pages.supporter.yearMonth'),
      chart2Id: '',
      chart3Id: '',
      supporterTableLabels: [] as supporterTableLabel[],
      // this.$t('man-hour.pages.supporter.table.labels'),
    }
  },
  created() {
    // 表ラベル文字が any で定義されているため詰替
    const supporterTableLabels: supporterTableLabel[] =
      [] as supporterTableLabel[]
    Object.assign(
      supporterTableLabels,
      this.$t('man-hour.pages.supporter.table.labels')
    )
    for (const i in supporterTableLabels) {
      const supporterTableLabel: supporterTableLabel = supporterTableLabels[i]
      this.supporterTableLabels.push(supporterTableLabel)
    }
  },
  computed: {
    /**
     * 年度月をセット
     * @param {string} year 年
     * @param {string} month 月
     * @return 年度月
     */
    setSummaryFiscalYearMonth() {
      const summaryFiscalYearMonth = this.summaryFiscalYearMonth as string
      const year: string = String(this.year())
      const month: string = String(this.month())
      return summaryFiscalYearMonth
        .replace('{year}', year)
        .replace('{month}', month)
    },
    /**
     * 年月をセット
     * @param {string} year 年
     * @param {string} month 月
     * @return 年月
     */
    setSummaryYearMonth() {
      const summaryYearMonth = this.summaryYearMonth as string
      const year: string = String(this.year())
      const month: string = String(this.month())
      return summaryYearMonth.replace('{year}', year).replace('{month}', month)
    },
    /**
     * 翌月ボタン無効判定
     * (表示年月と現在年月が同じ場合無効)
     * @param now  現在日時
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @returns 翌月ボタン無効か否か
     */
    nextMonthDisabled() {
      const now = getCurrentDate()
      const currentYear = now.getFullYear().toString()
      const currentMonth = (now.getMonth() + 1).toString()
      if (
        this.$route.params.year === currentYear &&
        this.$route.params.month === currentMonth
      ) {
        return true
      } else {
        return false
      }
    },
  },

  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year(this: any) {
      return parseInt(this.$route.params.year)
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month(this: any) {
      return parseInt(this.$route.params.month)
    },
    search(): void {
      const request = new this.RequestType()
      const SummarySupporterOrganizationsManHoursSearchParam: {
        [key: string]: number | string
      } = {}
      //課
      if (this.searchParam.supporterOrganizationId) {
        SummarySupporterOrganizationsManHoursSearchParam.supporterOrganizationId =
          // @ts-ignore
          this.searchParam.supporterOrganizationId.join(',')
      }
      Object.assign(request, SummarySupporterOrganizationsManHoursSearchParam)
      this.lastSearchRequest = request
      this.$emit('getSummarySupporterManHours', request)
      this.$emit('getSummarySupporterOrganizationsManHours', request)
    },
    /**
     * 今月ボタンが押下された際、今月に遷移
     * @param {number} year 現在の年
     * @param {number} month 現在の月
     */
    thisMonth() {
      const year = format(getCurrentDate(), 'yyyy')
      const month = format(getCurrentDate(), 'MM')
      this.$router.push(`/man-hour/supporter/${year}/${month}`)
    },
    /**
     * 前月ボタンが押下された際、前月に遷移
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @param {number} year 現在表示されている年
     * @param {number} month 現在表示されている月の前月
     */
    lastMonth() {
      const yearObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const monthObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const currentYear = parseInt(format(yearObject, 'yyyy'))
      const currentMonth = parseInt(format(monthObject, 'MM')) - 1
      const year = format(new Date(currentYear, currentMonth - 1), 'yyyy')
      const month = format(new Date(currentYear, currentMonth - 1), 'MM')
      this.$router.push(`/man-hour/supporter/${year}/${month}`)
    },
    /**
     * 翌月ボタンが押下された際、翌月に遷移
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @param {number} year 現在表示されている年
     * @param {number} month 現在表示されている月の翌月
     */
    nextMonth() {
      const yearObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const monthObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const currentYear = parseInt(format(yearObject, 'yyyy'))
      const currentMonth = parseInt(format(monthObject, 'MM')) - 1
      const year = format(new Date(currentYear, currentMonth + 1), 'yyyy')
      const month = format(new Date(currentYear, currentMonth + 1), 'MM')
      this.$router.push(`/man-hour/supporter/${year}/${month}`)
    },
    /**
     * ラベルの表示長指定
     * @return 8文字以上は...で省略
     */
    omitLabel(label: any) {
      return label && label.length && label.length > 8
        ? label.slice(0, 8) + '…'
        : label
    },
    /**
     * 最終集計日時の整形
     * @return 最終集計日時 'yyyy/MM/dd HH:mm'
     */
    getBatchEndAt() {
      return this.batchControl && this.batchControl.batchEndAt
        ? format(parseISO(this.batchControl.batchEndAt), 'yyyy/MM/dd HH:mm')
        : 'ー'
    },
    /**
     * 「稼働合計時間」から「休憩・その他時間」を引いた時間を求める
     * @param target 各名目時間
     * @param total 稼働合計時間
     * @param others 休憩・その他時間
     * @return 実質稼働時間
     */
    parsedHoursWork(target: number, total: number, others: number): number {
      return (target / (total - others)) * 100
    },
    /**
     * サービス別工数稼働率グラフデータ
     * @return サービス別工数稼働率グラフデータ
     */
    chartData1() {
      const year = this.response.supporterManHours.yearMonth.split('/')[0]
      const month = this.response.supporterManHours.yearMonth.split('/')[1]
      const supporterManHours: any = cloneDeep(this.response.supporterManHours)
      const totalSupporterManHours: any = {}
      const tempManHours: any = {}
      totalSupporterManHours.yearMonth = supporterManHours.yearMonth
      totalSupporterManHours.manHours = []
      for (const i in supporterManHours.manHours) {
        const supporterOrganizationId =
          supporterManHours.manHours[i].supporterOrganizationId
        const supporterOrganizationName =
          supporterManHours.manHours[i].supporterOrganizationName
        const contractTime: any = supporterManHours.manHours[i].contractTime
        const summaryManHour: any = supporterManHours.manHours[i].summaryManHour
        const supporter: any = {
          id: supporterManHours.manHours[i].supporterId,
          name: supporterManHours.manHours[i].supporterName,
        }
        if (!tempManHours[supporterOrganizationId]) {
          tempManHours[supporterOrganizationId] = {
            supporterOrganizationId,
            supporterOrganizationName,
            contractTime,
            summaryManHour,
            supporters: [supporter],
          }
        } else {
          for (const i2 in contractTime) {
            tempManHours[supporterOrganizationId].contractTime[i2] =
              parseInt(tempManHours[supporterOrganizationId].contractTime[i2]) +
              parseInt(contractTime[i2])
          }
          for (const i2 in summaryManHour) {
            tempManHours[supporterOrganizationId].summaryManHour[i2] =
              parseInt(
                tempManHours[supporterOrganizationId].summaryManHour[i2]
              ) + parseInt(summaryManHour[i2])
          }
          tempManHours[supporterOrganizationId].supporters.push(supporter)
        }
      }
      for (const i in tempManHours) {
        totalSupporterManHours.manHours.push(tempManHours[i])
      }

      return {
        labels: totalSupporterManHours.manHours.map((elm: any) =>
          [
            this.$t('common.format.year', { year }),
            elm.supporterOrganizationName,
            this.$t('common.format.month', { month }),
            '(' +
              this.$t('man-hour.pages.supporter.chart1.supporterCount', {
                count: elm.supporters.length,
              }) +
              ')',
          ].join(' ')
        ),
        datasets: [
          {
            label: this.$t('man-hour.pages.supporter.chart1.labels.label1'),
            data: totalSupporterManHours.manHours.map((elm: any) => {
              let value
              if (elm.summaryManHour.total) {
                value = this.parsedHoursWork(
                  elm.summaryManHour.direct,
                  elm.summaryManHour.total,
                  elm.summaryManHour.others
                )
              }
              value = value?.toFixed()
              return value
            }),
            borderColor: '#5c84f0',
            backgroundColor: '#5c84f0',
          },
          {
            label: this.$t('man-hour.pages.supporter.chart1.labels.label2'),
            data: totalSupporterManHours.manHours.map((elm: any) => {
              let value
              if (elm.summaryManHour.total) {
                value = this.parsedHoursWork(
                  elm.summaryManHour.pre,
                  elm.summaryManHour.total,
                  elm.summaryManHour.others
                )
              }
              value = value?.toFixed()
              return value
            }),
            borderColor: '#cb4539',
            backgroundColor: '#cb4539',
          },
          {
            label: this.$t('man-hour.pages.supporter.chart1.labels.label3'),
            data: totalSupporterManHours.manHours.map((elm: any) => {
              let value
              if (elm.summaryManHour.total) {
                value = this.parsedHoursWork(
                  elm.summaryManHour.sales,
                  elm.summaryManHour.total,
                  elm.summaryManHour.others
                )
              }
              value = value?.toFixed()
              return value
            }),
            borderColor: '#ebbb2d',
            backgroundColor: '#ebbb2d',
            datalabels: {
              color(data: any) {
                const number = data.dataset.data[data.dataIndex]
                return number > 5 ? 'black' : 'white'
              },
            },
          },
          {
            label: this.$t('man-hour.pages.supporter.chart1.labels.label4'),
            data: totalSupporterManHours.manHours.map((elm: any) => {
              let value
              if (elm.summaryManHour.total) {
                value = this.parsedHoursWork(
                  elm.summaryManHour.ssap,
                  elm.summaryManHour.total,
                  elm.summaryManHour.others
                )
              }
              value = value?.toFixed()
              return value
            }),
            borderColor: '#67a659',
            backgroundColor: '#67a659',
          },
        ],
      }
    },
    /**
     * サービス別工数稼働率グラフチャートオプション
     * @return サービス別工数稼働率グラフチャートオプション
     */
    chartOptions1() {
      return {
        indexAxis: 'y',
        responsive: true,
        scales: {
          x: {
            stacked: true,
            ticks: {
              min: 0,
              max: 100,
              stepSize: 25,
              callback(value: any) {
                return value + '%'
              },
            },
          },
          y: {
            stacked: true,
            grid: {
              display: false,
            },
          },
        },
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              boxWidth: 22,
              boxHeight: 10,
            },
          },
          datalabels: {
            align(data: any) {
              const number = data.dataset.data[data.dataIndex]
              return number < 5 ? 'end' : 'start'
            },
            anchor: 'end',
            color: 'white',
            formatter(value: any) {
              return value + '%'
            },
          },
        },
      }
    },
    /**
     * 案件別契約時間と支援工数内訳（対面支援）グラフデータ
     * @return 案件別契約時間と支援工数内訳（対面支援）グラフデータ
     */
    chartData2(this: any) {
      let chart2Data = [{}]
      let directSupportManHourFactor: any
      const alertFactors = this.response.alertSettings.factorSetting
      const serviceTypesManHours = cloneDeep(this.response.serviceTypesManHours)
      const labelContractTypes = {
        onerous: String(
          this.$t('man-hour.pages.supporter.contractTypes.onerous')
        ),
        gratuitous: String(
          this.$t('man-hour.pages.supporter.contractTypes.gratuitous')
        ),
      }
      serviceTypesManHours.forEach((elm: any) => {
        for (const i in elm.projects) {
          if (elm.projects[i].contractType === labelContractTypes.gratuitous) {
            elm.projects[
              i
            ].projectNameWithContractTypes = `${elm.projects[i].projectName} (${labelContractTypes.gratuitous})`
          } else {
            elm.projects[
              i
            ].projectNameWithContractTypes = `${elm.projects[i].projectName}`
          }
        }
        if (elm.serviceTypeId === this.chart2Id) {
          chart2Data = elm.projects
        }
      })
      alertFactors.forEach((elm: any) => {
        if (elm.serviceTypeId === this.chart2Id) {
          directSupportManHourFactor = elm.directSupportManHour
        }
      })
      return {
        labels: chart2Data.map((elm: any) => elm.projectNameWithContractTypes),
        datasets: [
          {
            label: this.$t('man-hour.pages.supporter.chart2.labels.label1'),
            data: chart2Data.map((elm: any) => {
              return elm.thisMonthDirectSupportManHourMain
            }),
            borderColor: '#cb4539',
            backgroundColor: '#cb4539',
            order: 2,
            stack: true,
          },
          {
            label: this.$t('man-hour.pages.supporter.chart2.labels.label2'),
            data: chart2Data.map((elm: any) => {
              return elm.thisMonthDirectSupportManHourSub
            }),
            borderColor: '#ebbb2d',
            backgroundColor: '#ebbb2d',
            order: 2,
            stack: true,
          },
          {
            type: 'line',
            label:
              this.$t('man-hour.pages.supporter.chart2.labels.label3') +
              directSupportManHourFactor +
              '%',
            data: chart2Data.map((elm: any) => {
              return (
                (elm.thisMonthContractTime * directSupportManHourFactor) / 100
              )
            }),
            borderColor: '#46bdc6',
            backgroundColor: '#46bdc6',
            order: 1,
          },
          {
            type: 'scatter',
            label: this.$t('man-hour.pages.supporter.chart2.labels.label4'),
            data: chart2Data.map((elm: any) => {
              return elm.thisMonthContractTime
            }),
            borderColor: '#5c84f0',
            backgroundColor: '#5c84f0',
            showLine: true,
            order: 1,
          },
        ],
      }
    },
    /**
     * 案件別契約時間と支援工数内訳（対面支援）グラフチャートオプション
     * @return 案件別契約時間と支援工数内訳（対面支援）グラフチャートオプション
     */
    chartOptions2() {
      const data = this.chartData2()
      const labels = data.labels.map((elm: any) => this.omitLabel(elm))
      return {
        responsive: true,
        scales: {
          x: {
            grid: {
              display: false,
            },
            ticks: {
              maxRotation: 90,
              minRotation: 90,
              callback: (index: any) => {
                return labels[index]
              },
            },
          },
          y: {
            title: {
              display: true,
              text: this.$t('man-hour.pages.supporter.chart2.time'),
              padding: { top: 0, left: 0, right: 0, bottom: 10 },
            },
            ticks: {
              min: 0,
              stepSize: 25,
            },
          },
        },
        interaction: {
          intersect: false,
          mode: 'index',
        },
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              boxWidth: 22,
              boxHeight: 10,
            },
          },
          datalabels: {
            display: false,
          },
        },
      }
    },
    /**
     * 案件別契約時間と支援工数内訳（対面支援＋支援仕込）グラフデータ
     * @return 案件別契約時間と支援工数内訳（対面支援＋支援仕込）グラフデータ
     */
    chartData3(this: any) {
      let chart3Data = [{}]
      let directSupportManHourFactor: any
      let directAndPreSupportManHourFactor: any
      const alertFactors = this.response.alertSettings.factorSetting
      const serviceTypesManHours = cloneDeep(this.response.serviceTypesManHours)
      const labelContractTypes = {
        onerous: String(
          this.$t('man-hour.pages.supporter.contractTypes.onerous')
        ),
        gratuitous: String(
          this.$t('man-hour.pages.supporter.contractTypes.gratuitous')
        ),
      }
      serviceTypesManHours.forEach((elm: any) => {
        for (const i in elm.projects) {
          if (elm.projects[i].contractType === labelContractTypes.gratuitous) {
            elm.projects[
              i
            ].projectNameWithContractTypes = `${elm.projects[i].projectName} (${labelContractTypes.gratuitous})`
          } else {
            elm.projects[
              i
            ].projectNameWithContractTypes = `${elm.projects[i].projectName}`
          }
        }
        if (elm.serviceTypeId === this.chart3Id) {
          chart3Data = elm.projects
        }
      })
      alertFactors.forEach((elm: any) => {
        if (elm.serviceTypeId === this.chart3Id) {
          directSupportManHourFactor = elm.directSupportManHour
          directAndPreSupportManHourFactor = elm.directAndPreSupportManHour
        }
      })

      return {
        labels: chart3Data.map((elm: any) => elm.projectNameWithContractTypes),
        datasets: [
          {
            label: this.$t('man-hour.pages.supporter.chart3.labels.label1'),
            data: chart3Data.map((elm: any) => {
              return elm.thisMonthDirectSupportManHourMain
            }),
            borderColor: '#cb4539',
            backgroundColor: '#cb4539',
            order: 2,
            stack: true,
          },
          {
            label: this.$t('man-hour.pages.supporter.chart3.labels.label2'),
            data: chart3Data.map((elm: any) => {
              return elm.thisMonthDirectSupportManHourSub
            }),
            borderColor: '#ebbb2d',
            backgroundColor: '#ebbb2d',
            order: 2,
            stack: true,
          },
          {
            label: this.$t('man-hour.pages.supporter.chart3.labels.label3'),
            data: chart3Data.map((elm: any) => {
              return elm.thisMonthPreSupportManHour
            }),
            borderColor: '#67a659',
            backgroundColor: '#67a659',
            order: 2,
            stack: true,
          },
          {
            type: 'line',
            label: this.$t('man-hour.pages.supporter.chart3.manHourFactor', {
              manHourFactor: directAndPreSupportManHourFactor,
            }),
            data: chart3Data.map((elm: any) => {
              return elm.totalProcessYPercent
            }),
            borderColor: '#ff0afa',
            backgroundColor: '#ff0afa',
            order: 1,
          },
          {
            type: 'line',
            label:
              this.$t('man-hour.pages.supporter.chart3.contractTime') +
              '*' +
              directSupportManHourFactor +
              '%',
            data: chart3Data.map((elm: any) => {
              return (
                (elm.thisMonthContractTime * directSupportManHourFactor) / 100
              )
            }),
            borderColor: '#46bdc6',
            backgroundColor: '#46bdc6',
            order: 1,
          },
          {
            type: 'scatter',
            label: this.$t('man-hour.pages.supporter.chart3.labels.label6'),
            data: chart3Data.map((elm: any) => {
              return elm.thisMonthContractTime
            }),
            borderColor: '#5c84f0',
            backgroundColor: '#5c84f0',
            showLine: true,
            order: 1,
          },
        ],
      }
    },
    /**
     * 案件別契約時間と支援工数内訳（対面支援＋支援仕込）グラフチャートオプション
     * @return 案件別契約時間と支援工数内訳（対面支援＋支援仕込）グラフデータチャートオプション
     */
    chartOptions3() {
      const data = this.chartData3()
      const labels = data.labels.map((elm: any) => this.omitLabel(elm))
      return {
        responsive: true,
        scales: {
          x: {
            grid: {
              display: false,
            },
            ticks: {
              maxRotation: 90,
              minRotation: 90,
              callback: (index: any) => {
                return labels[index]
              },
            },
          },
          y: {
            title: {
              display: true,
              text: this.$t('man-hour.pages.supporter.chart3.time'),
              padding: { top: 0, left: 0, right: 0, bottom: 10 },
            },
            ticks: {
              min: 0,
              stepSize: 25,
            },
          },
        },
        interaction: {
          intersect: false,
          mode: 'index',
        },
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              boxWidth: 22,
              boxHeight: 10,
            },
          },
          datalabels: {
            display: false,
          },
        },
      }
    },
    /**
     * Sony Acceleration Platform粗利シミュレーションとキーとなる指標の各合計値
     * @return 項目毎合計値
     */
    totalData(): TotalData {
      const Totals = {} as TotalData
      this.supporterTableLabels.forEach((elm: supporterTableLabel) => {
        Totals[elm.key] = this.response.supporterOrganizationsManHours.reduce(
          (sum: number, i: TotalData) => sum + i[elm.key],
          0
        )
      })
      return Totals
    },
    /**
     * 支援者組織（課）別工数指標の整形
     * @return 整形された支援者組織（課）別工数指標
     */
    formatSupporterOrganizationsManHours(
      num: number,
      item: any,
      total: boolean
    ) {
      const numStr = num.toLocaleString()

      // 2 有償案件の月時間単価(万円/1時間）
      if (item.text === this.supporterTableLabels[2].text) {
        if (total) {
          // 月商 * 100 / 有償案件の月契約時間
          const monthlySales = this.totalData()[
            this.supporterTableLabels[1].key
          ] as number

          const monthlyContractTime = this.totalData()[
            this.supporterTableLabels[3].key
          ] as number

          const result = (monthlySales * 100) / monthlyContractTime

          if (!isNaN(result)) {
            return Math.round(result * 10) / 10
          } else {
            return 0
          }
        } else {
          return numStr
        }
      }

      // 5 月契約時間当たりの直接寄与工数の比率
      if (item.text === this.supporterTableLabels[5].text) {
        if (total) {
          return this.rateWorkCount(4, 3) + '%'
        } else {
          return numStr + '%'
        }
      }

      // 8 月稼働率直接寄与（有償案件分のみ）
      if (item.text === this.supporterTableLabels[8].text) {
        if (total) {
          return this.rateWorkCount(4, 7) + '%'
        } else {
          return numStr + '%'
        }
      }

      // 10 月稼働率仕込含
      if (item.text === this.supporterTableLabels[10].text) {
        if (total) {
          return this.rateWorkCount(9, 7) + '%'
        } else {
          return numStr + '%'
        }
      }

      return numStr
    },
    /**
     * 2つの比率合計時間を百分率計算をする
     * @param keyOne 対象のテーブルラベルのインデックス番号
     * @param keyTwo 対象のテーブルラベルのインデックス番号
     * @return 百分率の計算結果(互いが0で割れない場合、0を返す)
     */
    rateWorkCount(keyOne: number, keyTwo: number) {
      const targetOneTotal = this.totalData()[
        this.supporterTableLabels[keyOne].key
      ] as number

      const targetTwoTotal = this.totalData()[
        this.supporterTableLabels[keyTwo].key
      ] as number

      const result = (targetOneTotal / targetTwoTotal) * 100

      if (!isNaN(result)) {
        return Math.round(result)
      } else {
        return 0
      }
    },
  },
  watch: {
    'response.serviceTypesManHours': {
      deep: true,
      handler(newValue: any) {
        if (newValue.length) {
          this.chart2Id = newValue[0].serviceTypeId
          this.chart3Id = newValue[0].serviceTypeId
        }
      },
    },
  },
})
</script>
