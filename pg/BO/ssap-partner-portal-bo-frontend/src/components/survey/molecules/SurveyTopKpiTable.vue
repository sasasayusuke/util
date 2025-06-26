<template>
  <Table class="m-SurveyTopKpiTable">
    <template #header>
      <tr>
        <th>
          <span
            ><strong>{{
              $t('survey.pages.index.table.kind.name')
            }}</strong></span
          >
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.satisfaction') }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.continuation') }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.recommended') }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.sales') }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <!-- サービスアンケート -->
      <tr>
        <th>{{ $t('survey.pages.index.table.service.name') }}</th>
        <td>{{ surveys.service.satisfactionAverage.toLocaleString() }}</td>
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>{{ surveys.service.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- 修了アンケート -->
      <tr>
        <th>{{ $t('survey.pages.index.table.completion.name') }}</th>
        <td>{{ surveys.completion.satisfactionAverage.toLocaleString() }}</td>
        <td>
          {{ surveys.completion.continuationPositivePercent.toLocaleString() }}%
        </td>
        <td>{{ surveys.completion.recommendedAverage.toLocaleString() }}</td>
        <td>{{ surveys.completion.salesAverage.toLocaleString() }}</td>
        <td>{{ surveys.completion.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- サービス／修了アンケート合算 -->
      <tr>
        <th>
          {{ $t('survey.pages.index.table.serviceAndcompletionTotal.name') }}
        </th>
        <td>
          {{
            surveys.serviceAndCompletion.satisfactionAverage.toLocaleString()
          }}
        </td>
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>
          {{ surveys.serviceAndCompletion.totalReceive.toLocaleString() }}
        </td>
      </tr>
      <!-- グラフ表示 -->
      <tr class="is-radio">
        <th>
          {{ $t('survey.pages.index.table.label.graph') }}
        </th>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.satisfaction')]"
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
            :labels="[$t('survey.pages.index.table.label.continuation')]"
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
            :labels="[$t('survey.pages.index.table.label.recommended')]"
            :values="['recommended']"
            class="is-no-label"
            horizontal
            hide-details
            is-hide-label
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.sales')]"
            :values="['sales']"
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

export default BaseComponent.extend({
  components: {
    Table,
    RadioGroup,
  },
  props: {
    /** アンケート情報一覧 */
    surveys: {
      type: Object,
      required: true,
    },
    /** 表示するグラフ種別 */
    setType: {
      type: String,
    },
  },
  watch: {
    select() {
      this.$emit('changeChart', this.select)
    },
  },
  data() {
    return {
      select: this.setType,
    }
  },
  methods: {},
})
</script>

<style lang="scss" scoped>
.m-SurveyTopKpiTable {
  td {
    width: 71.2px;
  }
}
</style>
