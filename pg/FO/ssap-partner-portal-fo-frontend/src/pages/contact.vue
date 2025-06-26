<template>
  <TemplateContact />
</template>

<script lang="ts">
import TemplateContact from '~/components/contact/templates/Contact.vue'
import BasePage from '~/common/BasePage'

export default BasePage.extend({
  layout: 'NoNavigation',
  components: {
    TemplateContact,
  },
  head() {
    return {
      title: this.$t('contact.pages.index.name') as string,
    }
  },
})
</script>
