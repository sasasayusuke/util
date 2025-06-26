<template>
  <CommonSort class="is-tall" @sort="sort" @clear="clear">
    <SurveyListSortInput
      :service-types="serviceTypes"
      :supporter-organizations="supporterOrganizations"
      :projects="projects"
      :customers="customers"
      :param="param"
      :is-loading="isLoading"
      v-on="$listeners"
      @update="update"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getFiscalYearStart, getFiscalYearEnd } from '~/utils/common-functions'
import SurveyListSortInput from '~/components/survey/molecules/SurveyListSortInput.vue'

import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class SearchParam {
  constructor(type: string = 'service_and_completion') {
    this.type = type

    const fiscalyearStart = getFiscalYearStart()
    this.summaryMonthFrom = format(fiscalyearStart, 'yyyy/MM')
    this.planSurveyResponseDateFrom = format(fiscalyearStart, 'yyyy/MM/dd')

    const fiscalyearEnd = getFiscalYearEnd()
    this.summaryMonthTo = format(fiscalyearEnd, 'yyyy/MM')
    this.planSurveyResponseDateTo = format(fiscalyearEnd, 'yyyy/MM/dd')
  }

  type: string = 'service'
  searchType: any = 1

  summaryMonthFrom: string
  summaryMonthTo: string

  planSurveyResponseDateFrom: string
  planSurveyResponseDateTo: string

  projectId = ''
  customerId = ''
  organizationIds = []
  serviceTypeId = ''
}

export default BaseComponent.extend({
  components: {
    CommonSort,
    SurveyListSortInput,
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
    /** サービス種別 */
    serviceTypes: {
      type: Array,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** 案件のサジェスト用情報一覧 */
    projects: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用情報一覧 */
    customers: {
      type: Array,
      required: true,
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
