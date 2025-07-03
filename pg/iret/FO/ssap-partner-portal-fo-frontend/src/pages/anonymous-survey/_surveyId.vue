<template>
  <component
    :is="survey.isFinished ? 'SurveyDetail' : 'SurveyInput'"
    v-if="isLoad"
    :survey="survey"
    :survey-master="surveyMaster"
    :is-anonymous="true"
    :token="authorized.token"
    @submit="submit"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import SurveyDetail from '~/components/survey/templates/SurveyDetail.vue'
import SurveyInput from '~/components/survey/templates/SurveyInput.vue'
import { AnonymousSurveyAuthorized } from '~/store/anonymous-survey'
import {
  SurveyAnonymousInfo,
  SurveyMasterAnonymousInfo,
  CheckAndGetSurveyAnonymousByIdRequest,
  CheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest,
  CheckAndUpdateSurveyAnonymousByIdRequest,
  CheckAndGetSurveyAnonymousById,
  CheckAndGetSurveyMasterAnonymousByIdAndRevision,
  CheckAndUpdateSurveyAnonymousById,
} from '~/models/Survey'
import { ISurveyInfo, ISurveyMasterInfo } from '~/types/Survey'
import { anonymousSurveyStore, currentPageDataStore } from '~/store'

interface DataType {
  authorized: AnonymousSurveyAuthorized
  survey: ISurveyInfo
  surveyMaster: ISurveyMasterInfo
  isLoad: boolean
}
export default BasePage.extend({
  name: 'AnonymousSurveySurveyId',
  components: {
    SurveyDetail,
    SurveyInput,
  },
  data(): DataType {
    return {
      authorized: new AnonymousSurveyAuthorized(),
      survey: new SurveyAnonymousInfo() as ISurveyInfo,
      surveyMaster: new SurveyMasterAnonymousInfo() as ISurveyMasterInfo,
      isLoad: false,
    }
  },
  methods: {
    /**
     * 匿名アンケートを取得
     * 呼び出し後、getSurveyMasterAnonymousByIdAndRevision関数を実行
     */
    async checkAndGetSurveyAnonymousById() {
      const surveyId = this.$route.params.surveyId
      const request: CheckAndGetSurveyAnonymousByIdRequest =
        new CheckAndGetSurveyAnonymousByIdRequest()
      request.token = this.authorized.token
      request.password = this.authorized.password
      await CheckAndGetSurveyAnonymousById(surveyId, request)
        .then((res) => {
          this.survey = res.data
          currentPageDataStore.setValues({
            projectId: this.survey.projectId,
            projectName: this.survey.projectName,
            customerName: this.survey.customerName,
          })
        })
        .catch(() => {
          this.$router.push('/403')
        })
      this.checkAndGetSurveyMasterAnonymousByIdAndRevision(
        this.survey.surveyMasterId,
        this.survey.surveyRevision
      )
    },
    /**
     * アンケートマスターをIDとバージョンで一意に取得
     * @param id アンケートマスタID
     * @param revision アンケートマスターバージョン番号
     */
    async checkAndGetSurveyMasterAnonymousByIdAndRevision(
      id: string,
      revision: number
    ) {
      const request: CheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest =
        new CheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest()
      request.token = this.authorized.token
      request.password = this.authorized.password
      await CheckAndGetSurveyMasterAnonymousByIdAndRevision(
        id,
        revision,
        request
      )
        .then((res) => {
          this.surveyMaster = res.data
          this.isLoad = true
        })
        .catch(() => {
          this.$router.push('/403')
        })
    },
    /**
     * 匿名アンケートをIDで一意に更新
     * @param answerData リクエストを行う匿名アンケート内容
     */
    async submit(answerData: CheckAndUpdateSurveyAnonymousByIdRequest) {
      this.clearErrorBar()
      const surveyId = this.$route.params.surveyId
      answerData.token = this.authorized.token
      answerData.password = this.authorized.password
      await CheckAndUpdateSurveyAnonymousById(
        surveyId,
        this.survey.version,
        answerData
      )
        .then(() => {})
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
  },
  mounted() {
    if (!anonymousSurveyStore.isAuthorized) {
      this.$router.push(this.forwardToUrl('/403'))
    } else {
      // リロード不可とする為、認証状態を取得後に明示的に初期化
      this.authorized.token = anonymousSurveyStore.token
      this.authorized.password = anonymousSurveyStore.password
      anonymousSurveyStore.resetAuthorized()
      this.displayLoading([this.checkAndGetSurveyAnonymousById()])
    }
  },
})
</script>
