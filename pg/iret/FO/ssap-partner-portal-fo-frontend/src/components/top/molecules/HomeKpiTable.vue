<template>
  <Table class="m-SurveyTopKpiTable">
    <template #header>
      <tr>
        <th>
          <span
            ><strong>{{ $t('top.pages.home.table.kind.name') }}</strong></span
          >
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.satisfaction') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.continuation') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.recommended') }}</span>
        </th>
        <th v-if="!isSupporter">
          <span>{{ $t('top.pages.home.table.label.sales') }}</span>
        </th>
        <th>
          <span>{{ $t('top.pages.home.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <!-- サービスアンケート -->
      <tr>
        <th>{{ $t('top.pages.home.table.service.name') }}</th>
        <td>{{ surveys.service.satisfactionAverage.toLocaleString() }}</td>
        <td>-</td>
        <td v-if="!isSupporter">-</td>
        <td>-</td>
        <td>{{ surveys.service.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- 修了アンケート -->
      <tr>
        <th>{{ $t('top.pages.home.table.completion.name') }}</th>
        <td>{{ surveys.completion.satisfactionAverage.toLocaleString() }}</td>
        <td>{{ surveys.completion.continuationPositivePercent }}%</td>
        <td>{{ surveys.completion.recommendedAverage.toLocaleString() }}</td>
        <td v-if="!isSupporter">
          {{ surveys.completion.salesAverage.toLocaleString() }}
        </td>
        <td>{{ surveys.completion.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- サービス／修了アンケート合算 -->
      <tr>
        <th>
          {{ $t('top.pages.home.table.serviceAndcompletionTotal.name') }}
        </th>
        <td>
          {{
            surveys.serviceAndCompletion.satisfactionAverage.toLocaleString()
          }}
        </td>
        <td>-</td>
        <td>-</td>
        <td v-if="!isSupporter">-</td>
        <td>
          {{ surveys.serviceAndCompletion.totalReceive.toLocaleString() }}
        </td>
      </tr>
      <!-- グラフ表示 -->
      <tr class="is-radio">
        <th>
          {{ $t('top.pages.home.table.label.graph') }}
        </th>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('top.pages.home.table.label.satisfaction')]"
            :values="['satisfaction']"
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
        <td>
          <RadioGroup
            v-if="!isSupporter"
            v-model="select"
            :labels="[$t('top.pages.home.table.label.sales')]"
            :values="['sales']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td v-if="!isSupporter">&nbsp;</td>
      </tr>
    </template>
  </Table>
</template>

<script lang="ts">
import { Table, RadioGroup } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { ENUM_SURVEY_SUMMARY } from '~/types/Survey'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    Table,
    RadioGroup,
  },
  props: {
    /**
     * アンケート集計結果
     */
    surveys: {
      type: Object,
      required: true,
    },
  },
  watch: {
    select() {
      this.$emit('changeChart', this.select)
    },
  },
  data() {
    return {
      select: ENUM_SURVEY_SUMMARY.SATISFACTION,
    }
  },
  computed: {
    /**
     *  支援者・支援者責任者の場合、「営業評価」を非表示にする
     */
    isSupporter() {
      if (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR
      ) {
        return true
      } else {
        return false
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.m-SurveyTopKpiTable {
  td {
    width: 71.2px;
  }
}
</style>
