<template>
  <CommonSort
    :title="$t('survey.pages.index.sort_input.name')"
    hx="h3"
    is-required
    @sort="sort"
    @clear="clear"
  >
    <SurveyTopSectionSortInput
      :param="param"
      v-on="$listeners"
      @update="update"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getFiscalYearStart, getFiscalYearEnd } from '~/utils/common-functions'
import SurveyTopSectionSortInput from '~/components/survey/molecules/SurveyTopSectionSortInput.vue'

import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class SearchParam {
  yearMonthFrom: string
  yearMonthTo: string

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.yearMonthFrom = format(fiscalyearStart, 'yyyy/MM')

    const fiscalyearEnd = getFiscalYearEnd()
    this.yearMonthTo = format(fiscalyearEnd, 'yyyy/MM')
  }
}

export default BaseComponent.extend({
  components: {
    CommonSort,
    SurveyTopSectionSortInput,
  },
  props: {
    /** 表示期間 */
    param: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * 他コンポーネントへ表示期間の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
    /** 指定した表示期間の集計結果を表示 */
    sort() {
      this.$emit('sort')
    },
    /** 表示期間および表示をリセット */
    clear() {
      this.$emit('clear')
    },
  },
})
</script>
