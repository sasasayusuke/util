<template>
  <CommonSort class="is-tall" @sort="$emit('sort')" @clear="$emit('clear')">
    <SurveyListSortInput
      :service-types="serviceTypes"
      :supporter-organizations="supporterOrganizations"
      :projects="projects"
      :customers="customers"
      :param="param"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import SurveyListSortInput, {
  SearchParam,
  InputCandidateLoading,
} from '~/components/survey/molecules/AdminSurveyListSortInput.vue'

import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export { SearchParam, InputCandidateLoading }

export default BaseComponent.extend({
  components: {
    CommonSort,
    SurveyListSortInput,
  },
  props: {
    /** リクエストパラメーター */
    param: {
      type: SearchParam,
    },
    /** アンケートタイプ */
    serviceTypes: {
      type: Array,
      required: true,
    },
    /** 支援者組織の一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** 案件一覧 */
    projects: {
      type: Array,
      required: true,
    },
    /** お客様一覧 */
    customers: {
      type: Array,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: InputCandidateLoading,
    },
  },
})
</script>
