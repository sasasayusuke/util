<template>
  <DetailContainer
    :title="$t('survey.pages.index.section.collectionStatus')"
    :is-editing="false"
    is-hide-header-button
    is-hide-footer
    is-hide-button1
    is-hide-button2
    note-head="required"
  >
    <template #date>
      {{ $t('survey.pages.index.lastUpdate') }}：{{ getBatchEndAt() }}
    </template>
    <Sheet class="px-7 pt-1 pb-7">
      <SurveyTopCollectionStatusSort
        :param="searchParam"
        @update="update"
        @sort="search"
        @clear="clear"
      />
      <SurveyTopCollectionStatusTable
        v-if="!isLoading"
        category="service"
        :surveys="surveySummaryAll.surveys"
      />
      <SurveyTopCollectionStatusTable
        v-if="!isLoading"
        class="mt-3"
        category="completion"
        :surveys="surveySummaryAll.surveys"
      />
      <SurveyTopCollectionStatusTable
        v-if="!isLoading"
        class="mt-3"
        category="serviceAndcompletionTotal"
        :surveys="surveySummaryAll.surveys"
      />
      <SurveyTopCollectionStatusTable
        v-if="!isLoading"
        class="mt-3"
        category="karte"
        :surveys="surveySummaryAll.surveys"
      />
    </Sheet>
  </DetailContainer>
</template>

<script lang="ts">
import { format, parse, parseISO } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import { Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyTopCollectionStatusSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyTopCollectionStatusSort.vue'
import SurveyTopCollectionStatusTable from '~/components/survey/molecules/SurveyTopCollectionStatusTable.vue'
import { GetSurveySummaryAllResponse } from '~/models/Survey'
import { GetBatchControlByIdResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    Sheet,
    DetailContainer,
    SurveyTopCollectionStatusSort,
    SurveyTopCollectionStatusTable,
  },
  props: {
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** アンケート全集計結果 */
    surveySummaryAll: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** 最終集計日時 */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
    },
  },
  data() {
    return {
      header: [
        {
          name: 'hoge',
        },
        {
          name: 'fuga',
          emphasize: true,
        },
      ],
      searchParam: new SearchParam(),
      apiName: 'getSurveySummaryAll',
    }
  },
  methods: {
    /**
     * 他コンポーネントから表示期間の変更を受け取る
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      if (item === 'year') {
        const selectedYearObject = parse(event, 'yyyy', getCurrentDate())
        const yearMonthFrom = format(
          new Date(selectedYearObject.getFullYear(), 3, 1),
          'yyyy/MM'
        )
        const yearMonthTo = format(
          new Date(selectedYearObject.getFullYear() + 1, 2, 31),
          'yyyy/MM'
        )
        this.searchParam.yearMonthFrom = String(yearMonthFrom)
        this.searchParam.yearMonthTo = String(yearMonthTo)
      } else {
        // @ts-ignore
        this.searchParam[item] = String(event)
      }
    },
    /** 表示期間および表示をリセット */
    clear() {
      this.searchParam = new SearchParam()
      this.search()
    },
    /** 指定した表示期間の集計結果を取得 */
    search() {
      const strYearMonthFrom = this.searchParam.yearMonthFrom
      const strYearMonthTo = this.searchParam.yearMonthTo
      let intYearMonthFrom = null
      let intYearMonthTo = null
      if (strYearMonthFrom != null && !Number.isInteger(strYearMonthFrom)) {
        intYearMonthFrom = parseInt(
          format(new Date(strYearMonthFrom), 'yyyyMM')
        )
      }
      if (strYearMonthTo != null && !Number.isInteger(strYearMonthTo)) {
        intYearMonthTo = parseInt(format(new Date(strYearMonthTo), 'yyyyMM'))
      }
      if (
        intYearMonthFrom != null &&
        intYearMonthTo != null &&
        intYearMonthFrom > intYearMonthTo
      ) {
        this.searchParam.yearMonthFrom = strYearMonthTo
        this.searchParam.yearMonthTo = strYearMonthFrom
      }
      const request = new SearchParam()
      Object.assign(request, this.searchParam)
      this.$emit(this.apiName, 'CollectionStatus', request)
    },
    /**
     * ISO8601形式の最終集計日時を表示用にフォーマットした文字列を返す
     * @returns フォーマット済み最終集計日時文字列
     */
    getBatchEndAt() {
      return this.batchControl && this.batchControl.batchEndAt
        ? format(parseISO(this.batchControl.batchEndAt), 'yyyy/MM/dd HH:mm')
        : 'ー'
    },
  },
})
</script>

<style lang="scss" scoped></style>
