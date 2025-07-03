<template>
  <Table class="m-SurveyTopSectionTable stretch">
    <template #header>
      <tr class="m-SurveyTopSectionTable__head-1">
        <th rowspan="2">
          <span
            ><strong>{{
              $t('top.pages.home.table.label.section')
            }}</strong></span
          >
        </th>
        <th colspan="2">
          <span>{{ $t('top.pages.home.table.label.service') }}</span>
        </th>
        <th colspan="4">
          <span>{{ $t('top.pages.home.table.label.completion') }}</span>
        </th>
        <th colspan="2">
          <span>{{
            $t('top.pages.home.table.label.overallSatisfaction')
          }}</span>
        </th>
      </tr>
      <tr class="m-SurveyTopSectionTable__head-2">
        <th>
          <span>{{
            $t('top.pages.home.table.label.overallSatisfaction')
          }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.receive') }}</span>
        </th>
        <th>
          <span>{{
            $t('top.pages.home.table.label.overallSatisfaction')
          }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.continuation') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.recommended2') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.receive') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.evaluation') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <!-- SMT -->
      <tr v-for="(item, index) in surveys" :key="index">
        <th>{{ item.supporterOrganizationName }}</th>
        <td>
          {{ item.serviceSatisfactionAverage.toLocaleString() }}
        </td>
        <td>
          {{ item.serviceReceive.toLocaleString() }}
        </td>
        <td>
          {{ item.completionSatisfactionAverage.toLocaleString() }}
        </td>
        <td>{{ item.completionContinuation.positivePercent }}%</td>
        <td>
          {{ item.completionRecommendedAverage.toLocaleString() }}
        </td>
        <td>
          {{ item.completionReceive.toLocaleString() }}
        </td>
        <td>
          {{ item.totalSatisfactionAverage.toLocaleString() }}
        </td>
        <td>
          {{ item.totalReceive }}
        </td>
      </tr>
      <!-- 平均・合計 -->
      <tr>
        <th>{{ $t('top.pages.home.table.label.averageAndTotal') }}</th>
        <td>
          {{ total.serviceSatisfactionAverage.toLocaleString() }}
        </td>
        <td>{{ total.serviceReceive.toLocaleString() }}</td>
        <td>
          {{ total.completionSatisfactionAverage.toLocaleString() }}
        </td>
        <td>{{ total.completionContinuationPercent }}%</td>
        <td>
          {{ total.completionRecommendedAverage.toLocaleString() }}
        </td>
        <td>{{ total.completionReceive.toLocaleString() }}</td>
        <td>{{ total.totalSatisfactionAverage.toLocaleString() }}</td>
        <td>{{ total.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- グラフ表示 -->
      <tr class="is-radio">
        <th>
          {{ $t('top.pages.home.table.label.graph') }}
        </th>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.overallSatisfaction')]"
            :values="['serviceOverallSatisfaction']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>&nbsp;</td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.overallSatisfaction')]"
            :values="['completionOverallSatisfaction']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.continuation')]"
            :values="['continuation']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.recommended')]"
            :values="['recommended']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>&nbsp;</td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.evaluation')]"
            :values="['evaluation']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>&nbsp;</td>
      </tr>
    </template>
  </Table>
</template>

<script lang="ts">
import { Table, RadioGroup } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { ENUM_SURVEY_SUMMARY } from '~/types/Survey'

export default BaseComponent.extend({
  components: {
    Table,
    RadioGroup,
  },
  props: {
    /**
     * 課別集計表のデータ
     */
    surveys: {
      type: Array,
      required: true,
    },
    total: {
      type: Object, // TODO: レスポンス型から順番に型を定義していく
      default: {
        supporterOrganizationId: '',
        supporterOrganizationName: '',
        serviceSatisfactionSummary: 0,
        serviceSatisfactionAverage: 0,
        serviceReceive: 0,
        completionSatisfactionSummary: 0,
        completionSatisfactionAverage: 0,
        completionContinuation: {
          positiveCount: 0,
          negativeCount: 0,
          positivePercent: 0,
        },
        completionRecommendedSummary: 0,
        completionRecommendedAverage: 0,
        completionReceive: 0,
        totalSatisfactionSummary: 0,
        totalSatisfactionAverage: 0,
        totalReceive: 0,
      },
    },
  },
  watch: {
    select() {
      this.$emit('changeChart', this.select)
    },
  },
  data() {
    return {
      select: ENUM_SURVEY_SUMMARY.SERVICEOVERALLSATISFACTION,
    }
  },
})
</script>

<style lang="scss" scoped>
.m-SurveyTopSectionTable {
  td {
    width: 58.125px;
  }
  .m-SurveyTopSectionTable__head-1 {
    th {
      &:first-child {
        width: 15% !important;
      }
    }
  }
  .m-SurveyTopSectionTable__head-2 {
    th {
      width: 10% !important;
    }
  }
}
</style>
