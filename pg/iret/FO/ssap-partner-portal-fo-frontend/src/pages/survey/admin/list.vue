<template>
  <TemplateSurveyList
    :surveys-response="getSurveysResponse"
    :is-loading="isLoading"
    :service-types="getServiceTypesResponse.serviceTypes"
    :supporter-organizations="
      getSupporterOrganizationsResponse.supporterOrganizations
    "
    :customers="suggestCustomersResponse"
    :projects="suggestProjectsResponse"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :limit="limit"
    :total="total"
    :query-type="queryType"
    @click:prev="onClickPrev"
    @click:next="onClickNext"
    @GetSurveys="getSurveys"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyList, {
  generateRequest,
  IsLoading,
} from '~/components/survey/templates/AdminSurveyList.vue'
import {
  GetSurveys,
  GetSurveysRequest,
  GetSurveysResponse,
} from '~/models/Survey'
import { SuggestCustomersResponse, SuggestCustomers } from '~/models/Customer'
import {
  GetServiceTypes,
  GetServiceTypesResponse,
  GetSupporterOrganizations,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'
import {
  SuggestProjects,
  SuggestProjectsRequest,
  SuggestProjectsResponse,
} from '~/models/Project'
import { meStore } from '~/store'

export default BasePage.extend({
  name: 'AdminSurveyList',
  components: {
    TemplateSurveyList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('survey.pages.admin.list.name') as string,
    }
  },
  data() {
    return {
      getSurveysResponse: new GetSurveysResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
      getSupporterOrganizationsResponse:
        new GetSupporterOrganizationsResponse(),
      getServiceTypesResponse: new GetServiceTypesResponse(),
      suggestProjectsResponse: new SuggestProjectsResponse(),
      role: meStore.role,
      isLoading: new IsLoading(),
      queryType: '',
      limit: 20,
      offsetPage: 1,
      total: 0,
    }
  },
  mounted() {
    const getSurveysRequest = generateRequest()
    const defaultProjectId = this.$route.query.projectId as string
    if (defaultProjectId) {
      getSurveysRequest.projectId = defaultProjectId
    }
    this.displayLoading([
      this.getSurveys(getSurveysRequest),
      this.getSupporterOrganizations(),
      this.getServiceTypes(),
      this.suggestCustomers(),
      this.suggestProjects(),
    ])
  },
  methods: {
    /**
     * 案件アンケートの一覧を取得
     * @param params リクエストパラメーター
     */
    async getSurveys(params: GetSurveysRequest = generateRequest()) {
      this.isLoading.getSurveys = true

      if (this.role === 'supporter') {
        params.isDisclosure = true
      }

      await GetSurveys(params).then((res) => {
        this.getSurveysResponse = res.data
        this.isLoading.getSurveys = false
      })
    },
    /**
     * サービス種別の一覧を取得
     */
    async getServiceTypes() {
      this.isLoading.inputCandidate.serviceTypes = true

      await GetServiceTypes().then((res) => {
        this.getServiceTypesResponse = res.data
        this.isLoading.inputCandidate.serviceTypes = false
      })
    },
    /**
     * 支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      this.isLoading.inputCandidate.supporterOrganizations = true

      await GetSupporterOrganizations().then((res) => {
        this.getSupporterOrganizationsResponse = res.data
        this.isLoading.inputCandidate.supporterOrganizations = false
      })
    },
    /**
     * 取引先のサジェスト用情報を取得
     */
    async suggestCustomers() {
      this.isLoading.inputCandidate.customers = true

      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
        this.isLoading.inputCandidate.customers = false
      })
    },
    /**
     * 案件のサジェスト用情報を取得
     * 顧客の場合は自身のアサイン案件のみ取得
     */
    async suggestProjects() {
      this.isLoading.inputCandidate.projects = true

      await SuggestProjects(new SuggestProjectsRequest()).then((res) => {
        this.suggestProjectsResponse = res.data
        this.isLoading.inputCandidate.projects = false
      })
    },
  },
})
</script>
