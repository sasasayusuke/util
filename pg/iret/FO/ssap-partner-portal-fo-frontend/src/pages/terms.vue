<template>
  <TemplateUserPolicyText />
</template>

<script lang="ts">
import TemplateUserPolicyText from '~/components/top/templates/UserPolicyText.vue'
import BasePage from '~/common/BasePage'

export default BasePage.extend({
  layout: 'BeforeLoginFillHeight',
  components: {
    TemplateUserPolicyText,
  },
  head() {
    return {
      title: this.$t('top.pages.terms.title') as string,
    }
  },
})
</script>
