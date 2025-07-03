<template>
  <TemplateMasterKarteList />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMasterKarteList from '~/components/master-karte/templates/TemplateMasterKarteList.vue'

export default BasePage.extend({
  name: 'MasterKarteListIndex',
  layout: 'noPageHeader',
  middleware: ['roleCheck'],
  components: {
    TemplateMasterKarteList,
  },
  head() {
    return {
      title: this.$t('project.pages.index.name') as string,
    }
  },
})
</script>
