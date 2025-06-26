<template>
  <SurveyInput
    v-if="isLoad"
    :survey="survey"
    :survey-master="surveyMaster"
    :token="authorized.token"
    @submit="submit"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import SurveyDetail from '~/components/survey/templates/SurveyDetail.vue'
import SurveyInput from '~/components/satisfaction-survey/templates/SatisfactionSurveyInput.vue'
import { SatisfactionSurveyAuthorized } from '~/store/satisfaction-survey'
import {
  GetSurveyOfSatisfactionByIdResponse,
  SurveyMasterSatisfactionInfo,
  GetSurveyOfSatisfactionByIdRequest,
  GetSurveyMasterSatisfactionByIdAndRevisionRequest,
  UpdateSurveyOfSatisfactionByIdRequest,
  GetSurveyOfSatisfactionById,
  GetSurveyMasterSatisfactionByIdAndRevision,
  UpdateSurveyOfSatisfactionById,
} from '~/models/Survey'
import {
  IGetSurveyOfSatisfactionByIdResponse,
  ISurveyMasterSatisfactionInfo,
} from '~/types/Survey'
import { satisfactionSurveyStore, currentPageDataStore } from '~/store'

interface DataType {
  authorized: SatisfactionSurveyAuthorized
  survey: IGetSurveyOfSatisfactionByIdResponse
  surveyMaster: ISurveyMasterSatisfactionInfo
  isLoad: boolean
}

export default BasePage.extend({
  name: 'SatisfactionSurveyId',
  components: {
    SurveyDetail,
    SurveyInput,
  },
  data(): DataType {
    return {
      authorized: new SatisfactionSurveyAuthorized(),
      survey:
        new GetSurveyOfSatisfactionByIdResponse() as IGetSurveyOfSatisfactionByIdResponse,
      surveyMaster:
        new SurveyMasterSatisfactionInfo() as ISurveyMasterSatisfactionInfo,
      isLoad: false,
    }
  },
  methods: {
    /**
     * 満足度評価のみ回答アンケートを取得
     * 呼び出し後、getSurveyMasterSatisfactionByIdAndRevision関数を実行
     */
    async getSurveyOfSatisfactionById() {
      const request: GetSurveyOfSatisfactionByIdRequest =
        new GetSurveyOfSatisfactionByIdRequest()
      request.token = satisfactionSurveyStore.token
      await GetSurveyOfSatisfactionById(request)
        .then((res) => {
          this.survey = res.data
          currentPageDataStore.setValues({
            projectId: this.survey.projectId,
            projectName: this.survey.projectName,
            customerName: this.survey.customerName,
          })
        })
        .catch((error) => {
          if (error?.response?.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
      if (this.survey.isFinished === false) {
        this.getSurveyMasterSatisfactionByIdAndRevision(
          this.survey.surveyMasterId,
          this.survey.surveyRevision
        )
      } else {
        this.isLoad = true
      }
    },
    /**
     * アンケートマスターをIDとバージョンで一意に取得
     * @param id アンケートマスタID
     * @param revision アンケートマスターバージョン番号
     */
    async getSurveyMasterSatisfactionByIdAndRevision(
      id: string,
      revision: number
    ) {
      const request: GetSurveyMasterSatisfactionByIdAndRevisionRequest =
        new GetSurveyMasterSatisfactionByIdAndRevisionRequest()
      request.token = satisfactionSurveyStore.token
      await GetSurveyMasterSatisfactionByIdAndRevision(id, revision, request)
        .then((res) => {
          this.surveyMaster = res.data
          this.isLoad = true
        })
        .catch((error) => {
          if (error?.response?.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /**
     * 満足度評価のみ回答アンケートをIDで一意に更新
     * @param answerData リクエストを行う満足度評価のみ回答アンケート内容
     */
    async submit(answerData: UpdateSurveyOfSatisfactionByIdRequest) {
      this.clearErrorBar()
      const id = this.survey.surveyId
      answerData.token = satisfactionSurveyStore.token
      await UpdateSurveyOfSatisfactionById(id, this.survey.version, answerData)
        .then(() => {})
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
  },
  mounted() {
    // クエリパラメータが存在しない場合は403エラーとする
    if (!this.$route.query.s) {
      this.$router.push(this.forwardToUrl('/403'))
    }
    const authorized = new SatisfactionSurveyAuthorized()
    authorized.token = this.$route.query.s as string
    // 認証状態を認証中に変更
    satisfactionSurveyStore.setAuthorized(authorized)
    this.displayLoading([this.getSurveyOfSatisfactionById()])
  },
})
</script>
