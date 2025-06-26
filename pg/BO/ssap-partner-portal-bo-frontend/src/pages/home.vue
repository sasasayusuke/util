<template>
  <TemplateHome />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateHome from '~/components/top/templates/Home.vue'

export default BasePage.extend({
  name: 'Home',
  layout: 'home',
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('top.pages.home.title') as string,
    }
  },
  components: {
    TemplateHome,
  },
})
</script>
