<template>
  <TemplateSurveyDetail
    v-if="!isLoading"
    :is-error="isError"
    :survey="survey"
    :survey-master="surveyMaster"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyDetail from '~/components/survey/templates/SurveyDetail.vue'
import { GetSurveyByIdResponse, GetSurveyById } from '~/models/Survey'
import {
  GetSurveyMasterByIdAndRevisionResponse,
  GetSurveyMasterByIdAndRevision,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyDetail,
  },
  head() {
    return {
      title: this.$t('survey.pages.detail.name') as string,
    }
  },
  data(): {
    survey: GetSurveyByIdResponse
    surveyMaster: GetSurveyMasterByIdAndRevisionResponse
    isLoading: boolean
    isError: boolean
  } {
    return {
      survey: new GetSurveyByIdResponse(),
      surveyMaster: new GetSurveyMasterByIdAndRevisionResponse(),
      isLoading: true,
      isError: false,
    }
  },
  mounted() {
    this.displayLoading([
      this.getSurveyById().then(() => {
        this.getSurveyMasterByIdAndRevision().then(() => {
          this.isLoading = false
        })
      }),
    ])
  },
  methods: {
    /**
     * getSurveyByIdAPIを叩き、アンケートを一意に取得して代入。
     */
    async getSurveyById() {
      this.clearErrorBar()
      const id: string = String(this.$route.params.surveyId)
      await GetSurveyById(id)
        .then((res) => {
          this.survey = res.data
          this.isError = false
        })
        .catch((error) => {
          if (error && error.response && error.response.status) {
            if (error.response.status === 400) {
              this.showErrorBarWithScrollPageTop(
                this.$t('survey.pages.detail.errorMessages.notImplemented')
              )
            } else if (error.response.status === 403) {
              this.showErrorBarWithScrollPageTop(
                this.$t('survey.pages.detail.errorMessages.forbidden')
              )
            } else {
              this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
            }
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
          this.isError = true
          this.isLoading = false
        })
    },
    /**
     * getSurveyMasterByIdAndRevisionAPIを叩き、アンケートマスターをIDとバージョンで一意に取得して代入。
     */
    async getSurveyMasterByIdAndRevision() {
      if (!this.survey.surveyMasterId) {
        return
      }

      const id: string = String(this.survey.surveyMasterId)
      const revision: number = Number(this.survey.surveyRevision)
      await GetSurveyMasterByIdAndRevision(id, revision)
        .then((res) => {
          this.surveyMaster = res.data
        })
        .catch((error) => {
          if (error.response && error.response.status === 400) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
  },
})
</script>
