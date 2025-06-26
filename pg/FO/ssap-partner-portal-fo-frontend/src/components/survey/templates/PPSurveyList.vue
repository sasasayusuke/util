<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader>{{ pageName }}</ListInPageHeader>
    <PPSurveySort
      :param="searchParam"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <PPSurveyListTable
      :surveys="response.surveys"
      :total="response.total"
      :offset-page="response.offsetPage"
      :limit="limit"
      :is-loading="isLoading.surveys"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import CommonList from '~/components/common/templates/CommonList'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import PPSurveySort, {
  SurveySearchParam,
} from '~/components/survey/organisms/PPSurveySort.vue'
import PPSurveyListTable from '~/components/survey/organisms/PPSurveyListTable.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'

import { PPSurveysListRequest, GetSurveysByMineResponse } from '~/models/Survey'

export { SurveySearchParam }

export interface isLoading {
  surveys: boolean
}

export default CommonList.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    PPSurveySort,
    PPSurveyListTable,
  },
  props: {
    /** 回答対象の案件アンケートの一覧 */
    response: {
      type: Object as PropType<GetSurveysByMineResponse>,
      required: true,
    },
    /** API呼び出し中の有無 */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: SurveySearchParam,
      RequestType: PPSurveysListRequest,
      apiName: 'GetSurveysByMine',
      pageName: this.$t('survey.pages.pp.list.name'),
    }
  },
  methods: {
    /**
     * 回答対象の案件アンケート一覧の並び替え
     * GetSurveysByMineを再度呼び出し
     */
    search(): void {
      const request = new this.RequestType()

      function dateStrToNum(date: string): number {
        return parseInt(date.replace(/\//g, ''))
      }

      if (this.searchParam.isRefined) {
        const timeParam: { [key: string]: number } = {}
        if (this.searchParam.from) {
          const dateFrom = dateStrToNum(this.searchParam.from)
          timeParam.dateFrom = dateFrom
        }
        if (this.searchParam.to) {
          const dateTo = dateStrToNum(this.searchParam.to)
          timeParam.dateTo = dateTo
        }

        Object.assign(request, timeParam)
      }
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
  },
})
</script>
