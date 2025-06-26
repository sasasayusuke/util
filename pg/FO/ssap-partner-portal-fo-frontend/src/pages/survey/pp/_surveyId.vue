<template>
  <component
    :is="survey.isFinished === true ? 'PPSurveyDetail' : 'SurveyInput'"
    v-if="!isLoading"
    :survey="survey"
    :survey-master="surveyMaster"
    @submit="submit"
  />
</template>
<script lang="ts">
import { parse } from 'date-fns'
import ja from 'date-fns/locale/ja'
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import PPSurveyDetail from '~/components/survey/templates/PPSurveyDetail.vue'
import SurveyInput from '~/components/survey/templates/SurveyInput.vue'
import {
  SurveyInfo,
  SurveyMasterInfo,
  GetSurveyById,
  GetSurveyMasterByIdAndRevision,
  PutUpdateSurveyByIdRequest,
  UpdateSurveyByIdRequest,
} from '~/models/Survey'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

interface DataType {
  survey: SurveyInfo
  surveyMaster: SurveyMasterInfo
  isLoading: Boolean
}

export default BasePage.extend({
  name: 'SurveyPpSurveyId',
  components: {
    PPSurveyDetail,
    SurveyInput,
  },
  middleware: ['roleCheck'],
  data(): DataType {
    return {
      survey: new SurveyInfo(),
      surveyMaster: new SurveyMasterInfo(),
      isLoading: false,
    }
  },
  computed: {
    /** 支援者、支援者責任者、営業担当者、営業責任者、事業者責任者のみ回答の入力が可能 */
    isAllowInput() {
      return hasRole([
        ENUM_USER_ROLE.SUPPORTER,
        ENUM_USER_ROLE.SALES,
        ENUM_USER_ROLE.SUPPORTER_MGR,
        ENUM_USER_ROLE.SALES_MGR,
        ENUM_USER_ROLE.BUSINESS_MGR,
      ])
    },
  },
  methods: {
    /**
     * 案件アンケートをIDで一意に取得
     */
    async getSurveyById() {
      this.isLoading = true
      const surveyId = this.$route.params.surveyId
      await GetSurveyById(surveyId)
        .then((res) => {
          if (res.data.isFinished === false && !this.isAllowInput) {
            this.$router.push('/404')
          }
          this.survey = res.data
        })
        .catch((error) => {
          if (error?.response?.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
      await this.getSurveyMasterByIdAndRevision(
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
          this.isLoading = false
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
     * @param answerData 新たに更新したローカルの案件アンケートオブジェクト
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
    /**
     * 表示するコンポーネントの条件分岐
     * @return 案件アンケートが終了又は回答日が現在の日付よりも以前の場合trueを返す
     */
    judgeComponent(): boolean {
      if (this.survey.isFinished) {
        return true
      }

      const planSurveyResponseDate = parse(
        this.survey.planSurveyResponseDatetime,
        this.$t('common.format.date_ymd6') as string,
        getCurrentDate(),
        { locale: ja }
      )
      if (getCurrentDate() > planSurveyResponseDate) {
        return true
      } else {
        return false
      }
    },
  },
  mounted() {
    this.displayLoading([this.getSurveyById()])
  },
})
</script>
