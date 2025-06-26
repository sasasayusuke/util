<template>
  <TemplateSurveyMasterCreate
    :survey-master="emptySurveyMaster"
    :is-loading="isLoading"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateSurveyMasterCreate from '~/components/survey/templates/SurveyMasterCreate.vue'
import { CreateSurveyMastersRequest } from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyMasterCreate',
  components: {
    TemplateSurveyMasterCreate,
  },
  head() {
    return {
      title: this.$t('survey.pages.create.name') as string,
    }
  },
  middleware: ['roleCheck'],
  data() {
    return {
      isLoading: true,
    }
  },
  computed: {
    /**
     * 空のcreateSurveyMastersAPIのリクエストパラメータを返す。
     * @returns 空のcreateSurveyMastersAPIのリクエストパラメータ
     */
    emptySurveyMaster(): CreateSurveyMastersRequest {
      const rtn = new CreateSurveyMastersRequest()
      rtn.name = ''
      rtn.type = 'quick'
      rtn.timing = 'anytime'
      rtn.initSendDaySetting = 24
      rtn.initAnswerLimitDaySetting = 102
      rtn.isDisclosure = true
      return rtn
    },
  },
})
</script>
