<template>
  <TemplateOtp />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateOtp from '~/components/top/templates/Otp.vue'

export default BasePage.extend({
  name: 'Otp',
  layout: 'BeforeLogin',
  middleware: ['beforeLogin'],
  components: {
    TemplateOtp,
  },
  head() {
    return {
      title: this.$t('top.pages.otp.title') as string,
    }
  },
})
</script>
