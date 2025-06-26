<template>
  <TemplateAnonymousSurveyAuth :param="param" :token="$route.query.s" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateAnonymousSurveyAuth from '~/components/anonymous-survey/templates/AnonymousSurveyAuth.vue'
import { CheckSurveyByIdPasswordRequest } from '~/models/Survey'
import { anonymousSurveyStore } from '~/store'

export default BasePage.extend({
  name: 'anonymousSurveyPassword',
  layout: 'BeforeLoginNoNavigation',
  components: {
    TemplateAnonymousSurveyAuth,
  },
  data(): {
    param: CheckSurveyByIdPasswordRequest
  } {
    return {
      param: new CheckSurveyByIdPasswordRequest(),
    }
  },
  created() {
    // 認証状態を初期化
    anonymousSurveyStore.resetAuthorized()
    // クエリパラメータが存在しない場合は403エラーとする
    if (!this.$route.query.s) {
      this.$router.push(this.forwardToUrl('/403'))
    }
  },
})
</script>
