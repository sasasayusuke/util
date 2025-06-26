<template>
  <TemplateCustomerCreate :customer="customer" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerCreate from '~/components/customer/templates/CustomerCreate.vue'
import { GetCustomerByIdResponse } from '~/models/Customer'

export default BasePage.extend({
  name: 'CustomerCreate',
  middleware: ['roleCheck'],
  components: {
    TemplateCustomerCreate,
  },
  head() {
    return {
      title: this.$t('customer.pages.create.name') as string,
    }
  },
  data() {
    return {
      customer: new GetCustomerByIdResponse(),
    }
  },
})
</script>
