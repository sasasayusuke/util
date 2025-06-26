<template>
  <TemplatePartnerPortalSurveysList
    :response="response"
    :is-loading="isLoading"
    @GetSurveysByMine="getSurveysByMine"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplatePartnerPortalSurveysList, {
  isLoading,
} from '~/components/survey/templates/PPSurveyList.vue'

import {
  getSurveysByMine,
  PPSurveysListRequest,
  GetSurveysByMineResponse,
} from '~/models/Survey'

export default BasePage.extend({
  name: 'SurveyPpList',
  components: {
    TemplatePartnerPortalSurveysList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('survey.pages.pp.list.name') as string,
    }
  },
  data(): { response: GetSurveysByMineResponse; isLoading: isLoading } {
    return {
      response: new GetSurveysByMineResponse(),
      isLoading: { surveys: false },
    }
  },
  mounted() {
    this.displayLoading([this.getSurveysByMine(new PPSurveysListRequest())])
  },
  methods: {
    /**
     * 自身の回答対象の案件アンケートの一覧を取得
     */
    async getSurveysByMine(request: PPSurveysListRequest) {
      this.isLoading.surveys = true

      await getSurveysByMine(request)
        .then((res) => {
          this.response = res.data
          this.isLoading.surveys = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
  },
})
</script>
