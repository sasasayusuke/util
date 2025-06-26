<template>
  <TemplateError
    :title="$t('top.pages.notice.404.title')"
    :description="$t('top.pages.notice.404.description')"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateError from '~/components/top/templates/Error.vue'

export default BasePage.extend({
  name: 'Error404',
  layout: 'ErrorPage',
  components: {
    TemplateError,
  },
  head() {
    return {
      title: this.$t('top.pages.notice.404.title') as string,
    }
  },
})
</script>
