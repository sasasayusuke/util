<template>
  <component
    :is="survey.isFinished ? 'SurveyDetail' : 'SurveyInput'"
    v-if="isLoad"
    :survey="survey"
    :survey-master="surveyMaster"
    @submit="submit"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import SurveyDetail from '~/components/survey/templates/SurveyDetail.vue'
import SurveyInput from '~/components/survey/templates/SurveyInput.vue'
import { GetNpfProjectId } from '~/models/MasterKarte'
import {
  SurveyInfo,
  SurveyMasterInfo,
  GetSurveyById,
  GetSurveyMasterByIdAndRevision,
  PutUpdateSurveyByIdRequest,
  UpdateSurveyByIdRequest,
} from '~/models/Survey'
import { currentPageDataStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

interface DataType {
  survey: SurveyInfo
  surveyMaster: SurveyMasterInfo
  isLoad: boolean
}

export default BasePage.extend({
  name: 'SurveySurveyId',
  components: {
    SurveyDetail,
    SurveyInput,
  },
  middleware: ['roleCheck'],
  data(): DataType {
    return {
      survey: new SurveyInfo(),
      surveyMaster: new SurveyMasterInfo(),
      isLoad: false,
    }
  },
  computed: {
    /** 顧客と営業と事業者責任者のみ回答の入力が可能 (営業の場合はURLに直接遷移することで代理の入力を可能) */
    isAllowInput() {
      return hasRole([
        ENUM_USER_ROLE.CUSTOMER,
        ENUM_USER_ROLE.SALES,
        ENUM_USER_ROLE.SALES_MGR,
        ENUM_USER_ROLE.BUSINESS_MGR,
      ])
    },
  },
  methods: {
    /**
     * 案件アンケートの一覧を取得
     * 呼び出し後、getSurveyMasterByIdAndRevision関数を実行
     */
    async getSurveyById() {
      const surveyId = this.$route.params.surveyId
      await GetSurveyById(surveyId)
        .then((res) => {
          if (res.data.isFinished === false && !this.isAllowInput) {
            this.$router.push('/404')
          }
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

      await GetNpfProjectId(this.survey.projectId).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
      this.getSurveyMasterByIdAndRevision(
        this.survey.surveyMasterId,
        this.survey.surveyRevision
      )
    },
    /**
     * アンケートマスターをIDとバージョンで一意に取得
     * @param id アンケートマスタID
     * @param revision アンケートマスターバージョン番号
     */
    async getSurveyMasterByIdAndRevision(id: string, revision: number) {
      await GetSurveyMasterByIdAndRevision(id, revision)
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
     * 案件アンケートをIDで一意に更新
     * @param answerData リクエストを行う案件アンケート内容
     */
    async submit(answerData: UpdateSurveyByIdRequest) {
      this.clearErrorBar()
      const surveyId = this.$route.params.surveyId
      await PutUpdateSurveyByIdRequest(
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
    this.displayLoading([this.getSurveyById()])
  },
})
</script>
