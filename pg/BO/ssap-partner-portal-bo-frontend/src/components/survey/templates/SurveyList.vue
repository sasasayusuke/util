<template>
  <RootTemPlate>
    <SurveyListButtons :selected="searchParam.type" @type="type" />
    <InPageHeader
      is-output-csv
      :csv-button-disabled="parseInt(total) <= 0"
      @outputCsv="exportSurveys"
    >
      {{ getListName() }}
    </InPageHeader>
    <SurveyListSort
      :param="searchParam"
      :is-loading="isLoading.surveys"
      :service-types="serviceTypesResponse.serviceTypes"
      :supporter-organizations="
        supporterOrganizationsResponse.supporterOrganizations
      "
      :projects="projects"
      :customers="customers"
      @update="update"
      @sort="search"
      @clear="clear"
    />
    <SurveyListChart
      v-if="isNotQuickSurvey(searchParam.type)"
      class="mt-4"
      :type="searchParam.type"
      :surveys-response="surveysResponse"
    />
    <SurveyListTable
      class="mt-5"
      :is-loading="isLoading.surveys"
      :offset-page="localOffsetPage"
      :max-page="maxPage"
      :limit="limit"
      :total="total"
      :surveys="surveysResponse.surveys"
      @click:prev="prev()"
      @click:next="next()"
    >
    </SurveyListTable>
  </RootTemPlate>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { Sheet, Title, Table } from '~/components/common/atoms'
import SurveyListButtons from '~/components/survey/molecules/SurveyListButtons.vue'
import InPageHeader from '~/components/common/organisms/InPageHeader.vue'
import SurveyListChart from '~/components/survey/organisms/SurveyListChart.vue'
import SurveyListSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyListSort.vue'
import SurveyListTable from '~/components/survey/organisms/SurveyListTable.vue'
import { GetSurveysResponse } from '~/models/Survey'
import {
  IGetSurveysRequest,
  IExportSurveysRequest,
  ENUM_GET_SURVEY_TYPE,
} from '~/types/Survey'
import { SuggestCustomersResponse } from '~/models/Customer'
import { SuggestProjectsResponse } from '~/models/Project'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

export default BaseComponent.extend({
  components: {
    RootTemPlate,
    Sheet,
    Title,
    Table,
    SurveyListButtons,
    InPageHeader,
    SurveyListChart,
    SurveyListSort,
    SurveyListTable,
  },
  props: {
    /** アンケート一覧 */
    surveysResponse: {
      type: Object as PropType<GetSurveysResponse>,
      required: true,
    },
    /** サービス種別 */
    serviceTypesResponse: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizationsResponse: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** 取引先のサジェスト用情報一覧 */
    customers: {
      type: Array as PropType<SuggestCustomersResponse>,
      required: true,
    },
    /** 案件のサジェスト用情報一覧 */
    projects: {
      type: Array as PropType<SuggestProjectsResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Object,
      required: true,
    },
    /** 現在のページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 最終ページ */
    maxPage: {
      type: Number,
      required: true,
    },
    /** 全件数 */
    total: {
      type: Number,
      required: true,
    },
    /** 1ページの表示上限数 */
    limit: {
      type: Number,
      required: true,
    },
    /** アンケート種別 */
    queryType: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      searchParam: new SearchParam(
        this.queryType !== '' ? this.queryType : 'service_and_completion'
      ),
      localOffsetPage: this.offsetPage,
    }
  },
  methods: {
    /**
     * 表示中のアンケート種別文字列取得
     * @returns アンケート種別文字列
     */
    getListName() {
      if (this.searchParam.type === 'completion') {
        return this.$t('survey.pages.list.completion.name')
      } else if (this.searchParam.type === 'service_and_completion') {
        return this.$t('survey.pages.list.serviceAndcompletion.name')
      } else if (this.searchParam.type === 'quick') {
        return this.$t('survey.pages.list.quick.name')
      } else {
        return this.$t('survey.pages.list.service.name')
      }
    },
    /** 前のページを表示 */
    prev() {
      this.localOffsetPage--
    },
    /** 次のページを表示 */
    next() {
      this.localOffsetPage++
    },
    /** 表示するアンケート種別を切り替え */
    type(type: string) {
      this.searchParam.type = type
      this.localOffsetPage = 1
      this.search()
    },
    /**
     * 他コンポーネントから検索条件の変更を受け取る
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      // @ts-ignore
      this.searchParam[item] = event
    },
    /** 検索条件をクリアし表示をリセット */
    clear() {
      this.searchParam = new SearchParam()
      this.search()
    },
    /** 検索を実行 */
    search() {
      if (parseInt(this.searchParam.searchType) === 1) {
        const strFromYearMonth = this.searchParam.summaryMonthFrom
        const strToYearMonth = this.searchParam.summaryMonthTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMM')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(format(new Date(strToYearMonth), 'yyyyMM'))
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.summaryMonthFrom = strToYearMonth
          this.searchParam.summaryMonthTo = strFromYearMonth
        }
        const request: IGetSurveysRequest = {
          type: this.searchParam.type,
          summaryMonthFrom: parseInt(
            format(new Date(this.searchParam.summaryMonthFrom), 'yyyyMM')
          ),
          summaryMonthTo: parseInt(
            format(new Date(this.searchParam.summaryMonthTo), 'yyyyMM')
          ),
          projectId: this.searchParam.projectId,
          customerId: this.searchParam.customerId,
          organizationIds: this.searchParam.organizationIds.join(','),
          serviceTypeId: this.searchParam.serviceTypeId,
          isFinished: true,
        }
        this.$emit('getSurveysBySummaryMonth', request)
      } else if (parseInt(this.searchParam.searchType) === 2) {
        const strFromYearMonth = this.searchParam.planSurveyResponseDateFrom
        const strToYearMonth = this.searchParam.planSurveyResponseDateTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMMdd')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(
            format(new Date(strToYearMonth), 'yyyyMMdd')
          )
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.planSurveyResponseDateFrom = strToYearMonth
          this.searchParam.planSurveyResponseDateTo = strFromYearMonth
        }
        const request: IGetSurveysRequest = {
          type: this.searchParam.type,
          planSurveyResponseDateFrom: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateFrom),
              'yyyyMMdd'
            )
          ),
          planSurveyResponseDateTo: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateTo),
              'yyyyMMdd'
            )
          ),
          projectId: this.searchParam.projectId,
          customerId: this.searchParam.customerId,
          organizationIds: this.searchParam.organizationIds.join(','),
          serviceTypeId: this.searchParam.serviceTypeId,
          isFinished: true,
        }
        this.$emit('getSurveysByPlanSurveyResponseDate', request)
      }
    },
    /** 表示されている情報のCSVファイルをダウンロード実行 */
    exportSurveys() {
      if (parseInt(this.searchParam.searchType) === 1) {
        const strFromYearMonth = this.searchParam.summaryMonthFrom
        const strToYearMonth = this.searchParam.summaryMonthTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMM')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(format(new Date(strToYearMonth), 'yyyyMM'))
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.summaryMonthFrom = strToYearMonth
          this.searchParam.summaryMonthTo = strFromYearMonth
        }
        const request: IExportSurveysRequest = {
          type: this.searchParam.type,
          summaryMonthFrom: parseInt(
            format(new Date(this.searchParam.summaryMonthFrom), 'yyyyMM')
          ),
          summaryMonthTo: parseInt(
            format(new Date(this.searchParam.summaryMonthTo), 'yyyyMM')
          ),
          projectId: this.searchParam.projectId,
          organizationIds: this.searchParam.organizationIds.join(','),
          mode: 'raw',
        }
        this.$emit('exportSurveys', request)
      } else if (parseInt(this.searchParam.searchType) === 2) {
        const strFromYearMonth = this.searchParam.planSurveyResponseDateFrom
        const strToYearMonth = this.searchParam.planSurveyResponseDateTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMMdd')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(
            format(new Date(strToYearMonth), 'yyyyMMdd')
          )
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.planSurveyResponseDateFrom = strToYearMonth
          this.searchParam.planSurveyResponseDateTo = strFromYearMonth
        }
        const request: IExportSurveysRequest = {
          type: this.searchParam.type,
          summaryMonthFrom: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateFrom),
              'yyyyMM'
            )
          ),
          summaryMonthTo: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateTo),
              'yyyyMM'
            )
          ),
          projectId: this.searchParam.projectId,
          organizationIds: this.searchParam.organizationIds.join(','),
          mode: 'raw',
        }
        this.$emit('exportSurveys', request)
      }
    },
    /**
     * 表示対象がクイックアンケートが判定する
     * @param surveyType アンケート種別
     * @returns アンケート種別判定真偽値
     */
    isNotQuickSurvey(surveyType: string) {
      return surveyType !== ENUM_GET_SURVEY_TYPE.QUICK
    },
  },
})
</script>
