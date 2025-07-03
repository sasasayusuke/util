<template>
  <TemplateProjectImport />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectImport from '~/components/project/templates/ProjectImport.vue'

export default BasePage.extend({
  name: 'ProjectImport',
  middleware: ['roleCheck'],
  components: {
    TemplateProjectImport,
  },
  head() {
    return {
      title: this.$t('project.pages.import.name') as string,
    }
  },
})
</script>
