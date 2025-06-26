<template>
  <Sheet class="d-flex align-center m-survey-forcast-sort-input__sheet">
    <v-container pa-0>
      <v-row no-gutters>
        <v-col class="d-flex align-center" cols="auto">
          <RadioGroup
            v-model="param.searchType"
            :disabled="disabled"
            :labels="[$t('survey.pages.forecast.sort_input.summaryMonth')]"
            :values="[1]"
            horizontal
            hide-details
            :class="{ 'is-disabled': disabled }"
            @change="update('searchType', $event)"
          />
          <Sheet width="260" class="d-flex align-center mr-7 ml-2">
            <MonthSelect
              v-model="param.summaryMonthFrom"
              :disabled="disabled"
              style-set="bgWhite"
              @change="update('summaryMonthFrom', $event)"
            />&nbsp;〜&nbsp;
            <MonthSelect
              v-model="param.summaryMonthTo"
              :disabled="disabled"
              style-set="bgWhite"
              is-no-icon
              @change="update('summaryMonthTo', $event)"
            />
          </Sheet>
        </v-col>
        <v-col class="d-flex align-center" cols="auto">
          <RadioGroup
            v-model="param.searchType"
            :disabled="disabled"
            :labels="[$t('survey.pages.forecast.sort_input.terms')]"
            :values="[2]"
            horizontal
            hide-details
            :class="{ 'is-disabled': disabled }"
            @change="update('searchType', $event)"
          />
          <Sheet width="280" class="d-flex align-center mr-7 ml-2">
            <DateSelect
              v-model="param.planSurveyResponseDateFrom"
              :disabled="disabled"
              style-set="bgWhite"
              @change="update('planSurveyResponseDateFrom', $event)"
            />&nbsp;〜&nbsp;
            <DateSelect
              v-model="param.planSurveyResponseDateTo"
              :disabled="disabled"
              style-set="bgWhite"
              is-no-icon
              @change="update('planSurveyResponseDateTo', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import { Sheet, RadioGroup } from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    Sheet,
    RadioGroup,
    DateSelect,
    MonthSelect,
  },
  props: {
    /** 検索条件 */
    param: {
      type: Object,
      required: true,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      searchType: 1,
      monthFrom: '',
      monthTo: '',
      dateFrom: '',
      dateTo: '',
    }
  },
  methods: {
    /**
     * 他コンポーネントへ検索条件の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>

<style lang="scss">
.m-survey-forcast-sort-input__sheet .v-input.a-radio.is-disabled {
  .a-radio__text {
    color: #8f8f8f;
  }
}
</style>
