<template>
  <TemplateMaintenance />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMaintenance from '~/components/top/templates/UnderMaintenance.vue'

export default BasePage.extend({
  name: 'Maintenance',
  layout: 'ErrorPage',
  components: {
    TemplateMaintenance,
  },
  head() {
    return {
      title: this.$t('top.pages.notice.maintenance.title') as string,
    }
  },
})
</script>
