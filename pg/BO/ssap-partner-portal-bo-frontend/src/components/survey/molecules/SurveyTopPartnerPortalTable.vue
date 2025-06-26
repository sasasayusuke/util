<template>
  <Table class="m-SurveyTopPartnerPortalTable">
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
          <span>{{ $t('survey.pages.index.table.label.surveyFunction') }}</span>
        </th>
        <th>
          <span>{{
            $t('survey.pages.index.table.label.manHourFunction')
          }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.karteFunction') }}</span>
        </th>
        <th>
          <span>{{
            $t('survey.pages.index.table.label.masterKarteFunction')
          }}</span>
        </th>
        <th>
          <span>{{ $t('survey.pages.index.table.label.receive') }}</span>
        </th>
      </tr>
    </template>
    <template #body>
      <tr>
        <th>{{ $t('survey.pages.index.table.label.partnerPortal') }}</th>
        <td>{{ surveys.pp.surveySatisfactionAverage.toLocaleString() }}</td>
        <td>{{ surveys.pp.manHourSatisfactionAverage.toLocaleString() }}</td>
        <td>{{ surveys.pp.karteSatisfactionAverage.toLocaleString() }}</td>
        <td>
          {{ surveys.pp.masterKarteSatisfactionAverage.toLocaleString() }}
        </td>
        <td>{{ surveys.pp.totalReceive.toLocaleString() }}</td>
      </tr>
      <!-- グラフ表示 -->
      <tr class="is-radio">
        <th>
          {{ $t('survey.pages.index.table.label.graph') }}
        </th>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.surveyFunction')]"
            is-hide-label
            :values="['survey']"
            class="is-no-label"
            horizontal
            hide-details
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.manHourFunction')]"
            is-hide-label
            :values="['manHour']"
            class="is-no-label"
            horizontal
            hide-details
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.karteFunction')]"
            is-hide-label
            :values="['karte']"
            class="is-no-label"
            horizontal
            hide-details
          />
        </td>
        <td>
          <RadioGroup
            v-model="select"
            :labels="[$t('survey.pages.index.table.label.masterKarteFunction')]"
            is-hide-label
            :values="['masterKarte']"
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
.m-SurveyTopPartnerPortalTable {
  width: 490px !important;
  td {
    width: 75px;
  }
}
</style>
