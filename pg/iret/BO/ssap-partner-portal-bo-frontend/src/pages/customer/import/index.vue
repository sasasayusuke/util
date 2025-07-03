<template>
  <TemplateCustomerImport />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerImport from '~/components/customer/templates/CustomerImport.vue'

export default BasePage.extend({
  name: 'CustomerImport',
  middleware: ['roleCheck'],
  components: {
    TemplateCustomerImport,
  },
  head() {
    return {
      title: this.$t('customer.pages.import.name') as string,
    }
  },
  data() {
    return {
      // このコンポーネントでは、api通信を行わない
    }
  },
})
</script>
