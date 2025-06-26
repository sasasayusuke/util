<template>
  <TemplateError />
</template>

<script lang="ts">
import TemplateError from '~/components/top/templates/Error.vue'
import BasePage from '~/common/BasePage'

export default BasePage.extend({
  components: {
    TemplateError,
  },
})
</script>
