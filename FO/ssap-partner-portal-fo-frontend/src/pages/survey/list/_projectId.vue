<template>
  <TemplateSurveyList
    :response="surveys"
    :project="getProjectByIdResponse"
    :is-loading="isLoading"
    :role="role"
    @getSurveys="getSurveys"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyList from '~/components/survey/templates/SurveyList.vue'
import {
  IGetSurveysByMineRequest,
  IGetSurveysRequest,
  ENUM_GET_SURVEYS_BY_MINE_TYPE,
  GetSurveysByMineType,
} from '@/types/Survey'
import {
  SurveyListItem,
  SurveySearchParam,
  GetSurveysByMine,
  GetSurveys,
} from '~/models/Survey'
import { GetProjectById, GetProjectByIdResponse } from '~/models/Project'
import { meStore, currentPageDataStore } from '~/store'
import { GetNpfProjectId } from '~/models/MasterKarte'

export default BasePage.extend({
  name: 'SurveyList',
  components: {
    TemplateSurveyList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('survey.pages.list.name') as string,
    }
  },
  data(): {
    surveys: Array<SurveyListItem>
    getProjectByIdResponse: GetProjectByIdResponse
    role: string
    isLoading: boolean
  } {
    return {
      surveys: [] as Array<SurveyListItem>,
      /** getProjectById APIのレスポンス */
      getProjectByIdResponse: new GetProjectByIdResponse(),
      role: meStore.role,
      isLoading: false,
    }
  },
  mounted() {
    this.displayLoading([this.getSurveys(), this.makeHeaderTitle()])
  },
  methods: {
    /**
     * ロール別に異なるAPIを呼び出し
     * ロールがお客様であれば、自身の回答対象の案件アンケートの一覧を取得
     * それ以外のロールは、
     * @param params リクエストパラメーター
     */
    async getSurveys(params: SurveySearchParam = new SurveySearchParam()) {
      const request = new SurveySearchParam()
      request.type = params.type
      request.enbFromTo = params.enbFromTo
      request.dateFrom = request.enbFromTo ? params.dateFrom : ''
      request.dateTo = request.enbFromTo ? params.dateTo : ''
      if (!this.checkInputs(request)) {
        return
      }

      if (this.role === 'customer') {
        await this.getSurveysCustomer(request)
      } else {
        await this.getSurveysSalesSupporter(request)
      }
    },
    /**
     * 自身の回答対象の案件アンケートの一覧を取得
     * @param params リクエストパラメーター
     */
    async getSurveysCustomer(
      params: SurveySearchParam = new SurveySearchParam()
    ) {
      const projectId: string = this.$route.params.projectId as string
      const request: IGetSurveysByMineRequest = {
        sort: 'actual_survey_request_datetime:desc',
      }
      if (params.type !== '') {
        request.type = params.type as GetSurveysByMineType
      }
      if (params.dateFrom !== '') {
        request.dateFrom = parseInt(params.dateFrom.replaceAll('/', ''))
      }
      if (params.dateTo !== '') {
        request.dateTo = parseInt(params.dateTo.replaceAll('/', ''))
      }
      request.projectId = projectId
      this.isLoading = true
      await GetSurveysByMine(request).then((res) => {
        this.surveys = this.processSurveyListItem(res.data.surveys)
        this.isLoading = false
      })
    },
    /**
     * 案件アンケートの一覧を取得
     * @param params リクエストパラメーター
     */
    async getSurveysSalesSupporter(
      params: SurveySearchParam = new SurveySearchParam()
    ) {
      const projectId: string = this.$route.params.projectId as string
      const request: IGetSurveysRequest = {}
      // non_ppパラメータを指定しPPアンケートを除外(種別：全ての場合もPPアンケートは表示しない)
      request.type = params.type ? params.type : 'non_pp'
      if (params.dateFrom !== '') {
        request.planSurveyResponseDateFrom = params.dateFrom.replaceAll('/', '')
      }
      if (params.dateTo !== '') {
        request.planSurveyResponseDateTo = params.dateTo.replaceAll('/', '')
      }
      request.projectId = projectId
      this.isLoading = true
      await GetSurveys(request)
        .then((res) => {
          this.surveys = res.data.surveys
          this.isLoading = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /**
     * 各パラメーターの入力チェック
     * @param params リクエストパラメーター
     */
    checkInputs(params: SurveySearchParam = new SurveySearchParam()) {
      let dateValFrom: any = null
      let dateValTo: any = null

      // 種別が全て/サービス/修了/クイック
      if (
        params.type !== '' &&
        !Object.values(ENUM_GET_SURVEYS_BY_MINE_TYPE).includes(
          params.type as GetSurveysByMineType
        )
      ) {
        return false
      }

      if (params.enbFromTo) {
        // From,Toは10文字以下
        if (params.dateFrom.length > 10) {
          return false
        }
        if (params.dateTo.length > 10) {
          return false
        }

        // From,Toが日付フォーマットであること
        if (params.dateFrom !== '') {
          dateValFrom = Date.parse(params.dateFrom)
          if (isNaN(dateValFrom)) {
            return false
          }
        }
        if (params.dateTo !== '') {
          dateValTo = Date.parse(params.dateTo)
          if (isNaN(dateValTo)) {
            return false
          }
        }

        // From,Toが逆転しないこと
        if (
          dateValFrom !== null &&
          dateValTo !== null &&
          dateValFrom > dateValTo
        ) {
          return false
        }
      }
      return true
    },
    /**
     * ヘッダーに表示するタイトルの値をstoreに保存
     */
    async makeHeaderTitle() {
      currentPageDataStore.clear()
      await GetProjectById(this.$route.params.projectId).then((res) => {
        this.getProjectByIdResponse = res.data
        currentPageDataStore.setValues({
          projectId: res.data.id,
          projectName: res.data.name,
          customerName: res.data.customerName,
        })
      })
      await GetNpfProjectId(currentPageDataStore.projectId).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
    },
    processSurveyListItem(data: SurveyListItem[]): SurveyListItem[] {
      const surveyListItem = data.map((item: SurveyListItem) => {
        item.isAnswerUserName = item.answerUserName === meStore.name
        return item
      })
      return surveyListItem
    },
  },
})
</script>
