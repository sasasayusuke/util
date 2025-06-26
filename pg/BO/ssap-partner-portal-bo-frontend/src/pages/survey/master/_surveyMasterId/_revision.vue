<template>
  <TemplateSurveyMasterQuestionsDetail
    v-if="isLoading === false"
    :survey-master="surveyMaster"
    :question-indexes="questionIndexes"
    :is-loading="isLoading"
    @refresh="getSurveyMasterByIdAndRevision"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyMasterQuestionsDetail from '~/components/survey/templates/SurveyMasterQuestionsDetail.vue'
import {
  GetSurveyMasterByIdAndRevisionResponse,
  GetSurveyMasterByIdAndRevision,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyMasterDetailRevision',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyMasterQuestionsDetail,
  },
  head() {
    return {
      title: this.$t('survey.pages.revision.name') as string,
    }
  },
  data(): {
    surveyMaster: GetSurveyMasterByIdAndRevisionResponse
    questionIndexes: boolean[]
    isLoading: boolean
  } {
    return {
      surveyMaster: new GetSurveyMasterByIdAndRevisionResponse(),
      questionIndexes: [] as boolean[],
      isLoading: true,
    }
  },
  mounted() {
    this.displayLoading([this.getSurveyMasterByIdAndRevision()])
  },
  methods: {
    /**
     * getSurveyMasterByIdAndRevisionAPIを叩き、アンケートマスターをIDとバージョンで一意に取得し代入。
     */
    async getSurveyMasterByIdAndRevision() {
      this.isLoading = true
      const id: string = String(this.$route.params.surveyMasterId)
      const revision: number = Number(this.$route.params.revision)
      await GetSurveyMasterByIdAndRevision(id, revision)
        .then((res) => {
          this.questionIndexes = [] as boolean[]
          for (const i in res.data.questions) {
            this.questionIndexes[i] = false
          }
          this.surveyMaster = res.data
          this.isLoading = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
  },
})
</script>
