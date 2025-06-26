<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader note-head="required" level="2">{{
      pageName
    }}</ListInPageHeader>
    <SurveySort @sort="search" @clear="clear" @update="updateParam" />
    <SurveyListTable
      :surveys="response"
      :project="project"
      :is-loading="isLoading"
      :role="role"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import type { PropType } from '~/common/BaseComponent'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import SurveySort from '~/components/survey/organisms/SurveySort.vue'
import SurveyListTable from '~/components/survey/organisms/SurveyListTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { SurveyListItem, SurveySearchParam } from '~/models/Survey'
import CommonList from '~/components/common/templates/CommonList'
import { GetProjectByIdResponse } from '~/models/Project'

export default CommonList.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    SurveySort,
    SurveyListTable,
  },
  props: {
    /** アンケート一覧 */
    response: {
      type: Array as PropType<SurveyListItem[]>,
      required: true,
    },
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** ロール(役職) */
    role: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      SearchParamType: SurveySearchParam,
      RequestType: SurveySearchParam,
      apiName: 'getSurveys',
      pageName: this.$t('survey.pages.list.name'),
    }
  },
})
</script>
