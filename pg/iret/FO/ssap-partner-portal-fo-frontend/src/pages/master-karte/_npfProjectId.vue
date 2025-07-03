<template>
  <TemplateMasterKarteDetailVue />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMasterKarteDetailVue from '~/components/master-karte/templates/TemplateMasterKarteDetail.vue'

export default BasePage.extend({
  name: 'MasterKarteListNpfProjectId',
  middleware: ['roleCheck'],
  components: {
    TemplateMasterKarteDetailVue,
  },
  head() {
    return {
      title: this.$t('master-karte.pages.index.name') as string,
    }
  },
})
</script>
