<template>
  <TemplateSurveyMasterDetail
    :response="getSurveyMastersByIdResponse"
    :is-loading="isLoading"
    @getSurveyMastersById="getSurveyMastersById"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyMasterDetail from '~/components/survey/templates/SurveyMasterDetail.vue'
import {
  GetSurveyMastersByIdResponse,
  GetSurveyMastersById,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyMasterDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyMasterDetail,
  },
  head() {
    return {
      title: this.$t('survey.pages.masterDetail.name') as string,
    }
  },
  data(): {
    getSurveyMastersByIdResponse: GetSurveyMastersByIdResponse
    isLoading: boolean
  } {
    return {
      getSurveyMastersByIdResponse: new GetSurveyMastersByIdResponse(),
      isLoading: true,
    }
  },
  mounted() {
    this.displayLoading([this.getSurveyMastersById()])
  },
  methods: {
    /**
     * getSurveyMastersByIdAPIを叩き、アンケートマスターをIDでバージョン順に一覧取得し代入。
     */
    async getSurveyMastersById() {
      this.isLoading = true
      const id: string = this.$route.params.surveyMasterId
      await GetSurveyMastersById(id).then((res) => {
        this.getSurveyMastersByIdResponse = res.data
        this.isLoading = false
      })
    },
    /**
     * アンケートマスターをリフレッシュ
     */
    refresh() {
      this.displayLoading([this.getSurveyMastersById()])
    },
  },
})
</script>
