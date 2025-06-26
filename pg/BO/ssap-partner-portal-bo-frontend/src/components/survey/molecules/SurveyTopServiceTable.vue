<template>
  <Table class="m-SurveyTopServiceTable">
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
          <span>{{ $t('survey.pages.index.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <!-- periodAverage -->
      <tr>
        <th>{{ $t('survey.pages.index.table.label.periodAverage') }}</th>
        <td>{{ surveys.service.satisfactionAverage.toLocaleString() }}</td>
        <td>{{ surveys.service.totalReceive.toLocaleString() }}</td>
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
            is-hide-label
            :values="['satisfaction']"
            class="is-no-label"
            horizontal
            hide-details
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
  data() {
    return {
      select: this.setType,
    }
  },
  methods: {},
})
</script>

<style lang="scss" scoped>
.m-SurveyTopServiceTable {
  width: 340px !important;
  td {
    width: 75px;
  }
}
</style>
