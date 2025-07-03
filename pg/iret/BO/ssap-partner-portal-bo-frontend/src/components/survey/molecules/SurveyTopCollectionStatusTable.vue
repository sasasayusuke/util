<template>
  <Table>
    <template #header>
      <tr>
        <th>
          <strong>{{ $t(`survey.pages.index.table.${category}.name`) }}</strong>
        </th>
        <th v-for="n of fyMonth" :key="n">
          {{ n + $t('survey.pages.index.table.label.month') }}
        </th>
        <th class="is-emphasize">
          {{ $t('survey.pages.index.table.label.monthlyAverage') }}
        </th>
        <th class="is-emphasize">
          {{ $t('survey.pages.index.table.label.annualTotal') }}
        </th>
      </tr>
    </template>
    <template #body>
      <tr>
        <th>
          {{ $t('survey.pages.index.table.label.projectCount') }}
        </th>
        <td v-for="n of fyMonth" :key="n">
          <div v-for="(item, index) in surveys" :key="index">
            <div v-if="dataExist(item, n)">
              {{
                category === 'serviceAndcompletionTotal'
                  ? (
                      item['service'].projectCount +
                      item['completion'].projectCount
                    ).toLocaleString()
                  : item[category].projectCount.toLocaleString()
              }}
            </div>
          </div>
        </td>
        <td class="is-emphasize is-limit-width">
          {{
            category === 'serviceAndcompletionTotal'
              ? totalMonthAverage('projectCount').toLocaleString()
              : monthlyAverage('projectCount').toLocaleString()
          }}
        </td>
        <td class="is-emphasize is-limit-width">
          {{
            category === 'serviceAndcompletionTotal'
              ? totalServiceAnnual('projectCount').toLocaleString()
              : annualTotal('projectCount').toLocaleString()
          }}
        </td>
      </tr>
      <tr>
        <th>{{ $t('survey.pages.index.table.label.receiveCount') }}</th>
        <td v-for="n of fyMonth" :key="n">
          <div v-for="(item, index) in surveys" :key="index">
            <div v-if="dataExist(item, n)">
              {{
                category === 'serviceAndcompletionTotal'
                  ? (
                      item['completion'].receiveCount +
                      item['service'].receiveCount
                    ).toLocaleString()
                  : category === 'karte'
                  ? item[category].karteCount.toLocaleString()
                  : item[category].receiveCount.toLocaleString()
              }}
            </div>
          </div>
        </td>
        <td class="is-emphasize">
          {{
            category === 'serviceAndcompletionTotal'
              ? totalMonthAverage('receiveCount')
              : monthlyAverage(
                  category === 'karte' ? 'karteCount' : 'receiveCount'
                )
          }}
        </td>
        <td class="is-emphasize">
          {{
            category === 'serviceAndcompletionTotal'
              ? totalServiceAnnual('receiveCount')
              : annualTotal(
                  category === 'karte' ? 'karteCount' : 'receiveCount'
                )
          }}
        </td>
      </tr>
      <tr>
        <th>{{ $t('survey.pages.index.table.label.receivePercent') }}</th>
        <td v-for="n of fyMonth" :key="n">
          <div v-for="(item, index) in surveys" :key="index">
            <div v-if="dataExist(item, n)">
              {{
                category === 'serviceAndcompletionTotal'
                  ? serviceAndcompletionTotalReceivePercent(
                      parseInt(item['completion'].receiveCount),
                      parseInt(item['service'].receiveCount),
                      parseInt(item['completion'].projectCount),
                      parseInt(item['service'].projectCount)
                    )
                  : category === 'karte'
                  ? item[category].usePercent
                  : item[category].receivePercent
              }}%
            </div>
          </div>
        </td>
        <td class="is-emphasize">{{ monthlyAverageRecoveryRate() }}%</td>
        <td class="is-emphasize">{{ annualTotalRecoveryRate() }}%</td>
      </tr>
    </template>
  </Table>
</template>

<script lang="ts">
import { Table } from '~/components/common/atoms/index'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SurveySummaryItem } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    Table,
  },
  data() {
    return {
      fyMonth: [4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3],
    }
  },
  props: {
    /** 表示カテゴリ */
    category: {
      type: String,
      required: true,
    },
    /** アンケート全集計結果 */
    surveys: {
      type: Array as PropType<SurveySummaryItem[]>,
      required: true,
    },
  },
  methods: {
    /**
     * 各アンケートの月平均回収率を求める
     * @return 月平均の回収率
     */
    monthlyAverageRecoveryRate() {
      const totalSupportCases =
        this.category === 'serviceAndcompletionTotal'
          ? this.totalMonthAverage('projectCount')
          : this.monthlyAverage('projectCount')
      const totalCollectedCases =
        this.category === 'serviceAndcompletionTotal'
          ? this.totalMonthAverage('receiveCount')
          : this.monthlyAverage(
              this.category === 'karte' ? 'karteCount' : 'receiveCount'
            )

      const result = Math.round((totalCollectedCases / totalSupportCases) * 100)
      if (isNaN(result)) {
        return 0
      }
      return result
    },
    /**
     * 各アンケートの年合計回収率を求める
     * @return 年合計の回収率
     */
    annualTotalRecoveryRate() {
      const totalSupportCases =
        this.category === 'serviceAndcompletionTotal'
          ? this.totalServiceAnnual('projectCount')
          : this.annualTotal('projectCount')
      const totalCollectedCases =
        this.category === 'serviceAndcompletionTotal'
          ? this.totalServiceAnnual('receiveCount')
          : this.annualTotal(
              this.category === 'karte' ? 'karteCount' : 'receiveCount'
            )

      const result = Math.round((totalCollectedCases / totalSupportCases) * 100)
      if (isNaN(result)) {
        return 0
      }
      return result
    },
    /**
     * 指定した項目の月平均を返す
     * @param type 算出対象項目
     * @returns 月平均値
     */
    monthlyAverage(type: string): number {
      const length = this.surveys.length
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm[this.category][type]
      })
      if (!total) {
        return 0
      }
      return Math.round(total / length)
    },
    /**
     * 指定した項目の年合計を返す
     * @param type 算出対象項目
     * @returns 年合計値
     */
    annualTotal(type: string): number {
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm[this.category][type]
      })
      return total
    },
    /**
     * 修了アンケートの指定項目の月平均を返す
     * @param type 算出対象項目
     * @returns 月平均値
     */
    completionMonthlyAverage(type: string): number {
      const length = this.surveys.length
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm.completion[type]
      })
      if (!total) {
        return 0
      }
      return Math.round(total / length)
    },
    /**
     * サービスアンケートの指定項目の月平均を返す
     * @param type 算出対象項目
     * @returns 月平均値
     */
    serviceMonthlyAverage(type: string): number {
      const length = this.surveys.length
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm.service[type]
      })
      if (!total) {
        return 0
      }
      return Math.round(total / length)
    },
    /**
     * 修了アンケートの指定項目の年合計を返す
     * @param type 算出対象項目
     * @returns 年合計値
     */
    completionAnnualTotal(type: string): number {
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm.completion[type]
      })
      return total
    },
    /**
     * サービスアンケートの指定項目の年合計を返す
     * @param type 算出対象項目
     * @returns 年合計値
     */
    serviceAnnualTotal(type: string): number {
      let total = 0
      this.surveys.forEach((elm: SurveySummaryItem) => {
        total = total + elm.service[type]
      })
      return total
    },
    /**
     * サービスアンケート/修了アンケート合算の指定項目の月平均を返す
     * @param type 算出対象項目
     * @returns 月平均値
     */
    totalMonthAverage(type: string): number {
      return (
        this.completionMonthlyAverage(type) + this.serviceMonthlyAverage(type)
      )
    },
    /**
     * サービスアンケート/修了アンケート合算の指定項目の年合計を返す
     * @param type 算出対象項目
     * @returns 年合計値
     */
    totalServiceAnnual(type: string): number {
      return this.completionAnnualTotal(type) + this.serviceAnnualTotal(type)
    },
    /**
     * アンケートサマリ内に指定月の情報が存在するか確認
     * @param item アンケートサマリ
     * @param n 月
     * @returns 判定真偽値
     */
    dataExist(item: SurveySummaryItem, n: number): boolean {
      const itemMonth = Number(
        item.summaryMonth.split('/')[1].replace(/^0/, '')
      )
      if (itemMonth === n) {
        return true
      }
      return false
    },
    /**
     * サービスアンケート/修了アンケート合算の回収率を返す
     * @param completionReceiveCount 修了アンケート回収案件数（実績）
     * @param serviceReceiveCount サービスアンケート回収案件数（実績）
     * @param completionProjectCount 修了アンケート回収案件数（計画）
     * @param serviceProjectCount サービスアンケート回収案件数（計画）
     * @returns 回収率
     */
    serviceAndcompletionTotalReceivePercent(
      completionReceiveCount: number,
      serviceReceiveCount: number,
      completionProjectCount: number,
      serviceProjectCount: number
    ) {
      if (isNaN(completionReceiveCount)) {
        completionReceiveCount = 0
      }
      if (isNaN(serviceReceiveCount)) {
        serviceReceiveCount = 0
      }
      if (isNaN(completionProjectCount)) {
        completionProjectCount = 0
      }
      if (isNaN(serviceProjectCount)) {
        serviceProjectCount = 0
      }
      let receivePercent = Math.floor(
        ((completionReceiveCount + serviceReceiveCount) /
          (serviceProjectCount + completionProjectCount)) *
          100
      )
      if (isNaN(receivePercent)) {
        receivePercent = 0
      }
      return receivePercent.toLocaleString()
    },
  },
})
</script>
<style lang="scss" scoped>
.a-table {
  th {
    &:not(:first-child) {
      width: 68px;
    }
  }
}
</style>
