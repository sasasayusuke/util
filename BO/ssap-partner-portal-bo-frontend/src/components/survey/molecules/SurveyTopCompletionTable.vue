<template>
  <Table class="m-SurveyTopCompletionTable">
    <template #header>
      <tr>
        <th>
          <span
            ><strong>{{
              $t('survey.pages.index.table.label.manager')
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
          <span>{{ $t('survey.pages.index.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <!-- periodAverage -->
      <tr>
        <th>{{ $t('survey.pages.index.table.label.periodAverage') }}</th>
        <td>{{ surveys.completion.satisfactionAverage.toLocaleString() }}</td>
        <td>
          {{ surveys.completion.continuationPositivePercent.toLocaleString() }}%
        </td>
        <td>{{ surveys.completion.recommendedAverage.toLocaleString() }}</td>
        <td>{{ surveys.completion.totalReceive.toLocaleString() }}</td>
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
.m-SurveyTopCompletionTable {
  width: 490px !important;
  td {
    width: 75px;
  }
}
</style>
