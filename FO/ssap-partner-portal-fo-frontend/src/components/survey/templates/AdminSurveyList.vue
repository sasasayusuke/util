<template>
  <RootTemplate>
    <SurveyListButtons :survey-type="surveyType" @click="changeSurveyType" />
    <DetailInPageHeader note-head="required">
      {{ getListName() }}
    </DetailInPageHeader>
    <SurveyListSort
      :param="searchParam"
      :service-types="serviceTypes"
      :supporter-organizations="supporterOrganizations"
      :projects="projects"
      :customers="customers"
      :is-loading="isLoading.inputCandidate"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <SurveyListChart
      v-if="isNotQuickSurvey(surveyType)"
      class="mt-4"
      :type="surveyType"
      :surveys-response="surveysResponse"
    />
    <AdminSurveyListTable
      class="mt-5"
      :surveys="surveysResponse.surveys"
      :is-loading="isLoading.getSurveys"
      :offset-page="localOffsetPage"
      :limit="limit"
      :total="total"
      link-prefix="survey"
      @click:prev="$emit('click:prev')"
      @click:next="$emit('click:next')"
    >
    </AdminSurveyListTable>
  </RootTemplate>
</template>

<script lang="ts">
import { toFiscalYear } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Sheet, Title, Table } from '~/components/common/atoms'
import SurveyListButtons from '~/components/survey/molecules/SurveyListButtons.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import SurveyListChart from '~/components/survey/organisms/SurveyListChart.vue'
import SurveyListSort, {
  SearchParam,
  InputCandidateLoading,
} from '~/components/survey/organisms/AdminSurveyListSort.vue'
import AdminSurveyListTable from '~/components/survey/organisms/AdminSurveyListTable.vue'
import { GetSurveysResponse, GetSurveysRequest } from '~/models/Survey'
import { ENUM_GET_SURVEYS_BY_MINE_TYPE } from '~/types/Survey'

// TODO 定義を移動
export class IsLoading {
  getSurveys: boolean = false
  inputCandidate: InputCandidateLoading = new InputCandidateLoading()
}

export function generateRequest(
  fiscalYear: string = toFiscalYear().toString(),
  surveyType: string = 'service_and_completion',
  customerId?: string,
  projectId?: string,
  serviceTypeId?: string,
  organizationIds: string[] = []
): GetSurveysRequest {
  const request = new GetSurveysRequest()

  request.type = surveyType

  request.summaryMonthFrom = `${fiscalYear}04`
  request.summaryMonthTo = `${parseInt(fiscalYear) + 1}03`

  if (customerId) {
    request.customerId = customerId
  }

  if (projectId) {
    request.projectId = projectId
  }

  if (serviceTypeId) {
    request.serviceTypeId = serviceTypeId
  }

  if (organizationIds.length > 0) {
    request.organizationIds = organizationIds.join(',')
  }
  request.isFinished = true
  return request
}

export default BaseComponent.extend({
  components: {
    RootTemplate,
    Sheet,
    Title,
    Table,
    SurveyListButtons,
    DetailInPageHeader,
    SurveyListChart,
    SurveyListSort,
    AdminSurveyListTable,
  },
  props: {
    /** 案件アンケートの一覧 */
    surveysResponse: {
      type: Object as PropType<GetSurveysResponse>,
      required: true,
    },
    /** サービス種別の一覧 */
    serviceTypes: {
      type: Array,
      required: true,
    },
    /** 支援者組織の一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** お客様一覧 */
    customers: {
      type: Array,
      required: true,
    },
    /** 案件一覧 */
    projects: {
      type: Array,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 現在のページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 最大ページ */
    maxPage: {
      type: Number,
      required: true,
    },
    /** ページ合計 */
    total: {
      type: Number,
      required: true,
    },
    /** 件数合計 */
    limit: {
      type: Number,
      required: true,
    },
  },
  data() {
    const searchParam = new SearchParam()
    const defaultProjectId = this.$route.query.projectId as string
    if (defaultProjectId) {
      searchParam.projectName = defaultProjectId
    }
    return {
      apiName: 'GetSurveys',
      searchParam,
      lastSearchRequest: new GetSurveysRequest(),
      surveyType: 'service_and_completion',
    }
  },
  methods: {
    /**
     * アンケートタイプによってタイトルを生成
     */
    getListName(): string {
      if (this.surveyType === 'completion') {
        return this.$t('survey.pages.admin.list.completion.name') as string
      } else if (this.surveyType === 'service_and_completion') {
        return this.$t(
          'survey.pages.admin.list.serviceAndcompletion.name'
        ) as string
      } else if (this.surveyType === 'quick') {
        return this.$t('survey.pages.admin.list.quick.name') as string
      } else {
        return this.$t('survey.pages.admin.list.service.name') as string
      }
    },
    updateParam(keyName: string, newVal: string): void {
      // @ts-ignore TODO: ここの型の記述をちゃんと型付けする
      this.searchParam[keyName] = newVal
    },
    /**
     * パラメーターをリセット
     * 検索条件を送信
     */
    clear(): void {
      this.searchParam = new SearchParam()
      this.search()
    },
    /**
     * 並び替えを送信
     */
    search(): void {
      const request = generateRequest(
        this.searchParam.year,
        this.surveyType,
        this.searchParam.customerName,
        this.searchParam.projectName,
        this.searchParam.serviceName,
        this.searchParam.organization
      )
      request.type = this.surveyType
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    /**
     * アンケートタイプを更新
     * @param $event アンケートタイプ
     */
    changeSurveyType($event: string): void {
      this.surveyType = $event
      const request = generateRequest(
        this.searchParam.year,
        this.surveyType,
        this.searchParam.customerName,
        this.searchParam.projectName,
        this.searchParam.serviceName,
        this.searchParam.organization
      )
      request.type = this.surveyType
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    /**
     * アンケートタイプがクイックアンケートかチェック
     * @param surveyType アンケートタイプ
     * @return boolean
     */
    isNotQuickSurvey(surveyType: string) {
      return surveyType !== ENUM_GET_SURVEYS_BY_MINE_TYPE.QUICK
    },
  },
})
</script>
