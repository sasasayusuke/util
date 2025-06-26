<template>
  <TemplateSurveyMasterList
    :response="getSurveyMastersResponse"
    :is-loading="isLoading"
    @getSurveyMasters="getSurveyMasters"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyMasterList from '~/components/survey/templates/SurveyMasterList.vue'
import {
  GetSurveyMastersRequest,
  GetSurveyMastersResponse,
  GetSurveyMasters,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyMasterList',
  components: {
    TemplateSurveyMasterList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('survey.pages.masterList.name') as string,
    }
  },
  data(): {
    getSurveyMastersResponse: GetSurveyMastersResponse
    isLoading: Boolean
  } {
    return {
      getSurveyMastersResponse: new GetSurveyMastersResponse(),
      isLoading: true,
    }
  },
  mounted() {
    this.displayLoading([this.getSurveyMasters()])
  },
  methods: {
    /**
     * getSurveyMasterAPIを叩き、アンケートマスター一覧を取得して代入。
     * @param params getSurveyMasterAPIのリクエストパラメータを指定
     */
    async getSurveyMasters(
      params: GetSurveyMastersRequest = new GetSurveyMastersRequest()
    ) {
      this.isLoading = true

      await GetSurveyMasters(params).then((res) => {
        this.getSurveyMastersResponse = res.data
        this.isLoading = false
      })
    },
  },
})
</script>
