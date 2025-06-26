<template>
  <CommonSort is-required :disabled="disabled" @sort="sort" @clear="clear">
    <SurveyForecastSortInput
      :param="param"
      :is-loading="isLoading"
      :disabled="disabled"
      v-on="$listeners"
      @update="update"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import {
  getCurrentDate,
  getFiscalYearEnd,
  getFiscalYearStart,
} from '~/utils/common-functions'
import SurveyForecastSortInput from '~/components/survey/molecules/SurveyForecastSortInput.vue'

import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class SearchParam {
  searchType: any = 1

  summaryMonthFrom: string
  summaryMonthTo: string

  planSurveyResponseDateFrom: string
  planSurveyResponseDateTo: string

  constructor() {
    const CurrentDateFrom = getCurrentDate()
    const FiscalYearStart = getFiscalYearStart()
    this.summaryMonthFrom = format(CurrentDateFrom, 'yyyy/MM')
    this.planSurveyResponseDateFrom = format(FiscalYearStart, 'yyyy/MM/dd')

    const CurrentDateTo = getCurrentDate()
    const FiscalYearEnd = getFiscalYearEnd()
    this.summaryMonthTo = format(CurrentDateTo, 'yyyy/MM')
    this.planSurveyResponseDateTo = format(FiscalYearEnd, 'yyyy/MM/dd')
  }
}

export default BaseComponent.extend({
  components: {
    CommonSort,
    SurveyForecastSortInput,
  },
  props: {
    /** 検索条件 */
    param: {
      type: Object,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {}
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
    /** 検索を実行 */
    sort() {
      this.$emit('sort')
    },
    /** 検索条件をクリアし表示をリセット */
    clear() {
      this.$emit('clear')
    },
  },
})
</script>
