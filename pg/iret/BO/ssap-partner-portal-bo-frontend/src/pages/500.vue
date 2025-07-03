<template>
  <TemplateError
    :title="$t('top.pages.notice.500.title')"
    :description="$t('top.pages.notice.500.description')"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateError from '~/components/top/templates/Error.vue'

export default BasePage.extend({
  name: 'Error500',
  layout: 'ErrorPage',
  components: {
    TemplateError,
  },
  head() {
    return {
      title: this.$t('top.pages.notice.500.title') as string,
    }
  },
})
</script>
