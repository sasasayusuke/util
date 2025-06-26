<template>
  <TemplateCustomerList
    :response="getCustomersResponse"
    :suggest-customers="suggestCustomersResponse"
    :is-loading="isLoading"
    @getCustomers="getCustomers"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerList, {
  isLoading,
} from '~/components/customer/templates/CustomerList.vue'
import {
  GetCustomersRequest,
  GetCustomersResponse,
  GetCustomers,
  SuggestCustomersResponse,
  SuggestCustomers,
} from '~/models/Customer'

export default BasePage.extend({
  name: 'CustomerList',
  layout: 'noPageHeader',
  components: {
    TemplateCustomerList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('customer.pages.index.name') as string,
    }
  },
  data(): {
    getCustomersResponse: GetCustomersResponse
    suggestCustomersResponse: SuggestCustomersResponse
    isLoading: isLoading
  } {
    return {
      getCustomersResponse: new GetCustomersResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
      isLoading: {
        customers: true,
        suggestCustomers: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getCustomers(), this.SuggestCustomers()])
  },
  methods: {
    async getCustomers(
      params: GetCustomersRequest = new GetCustomersRequest()
    ) {
      this.isLoading.customers = true

      await GetCustomers(params).then((res) => {
        this.getCustomersResponse = res.data
        this.isLoading.customers = false
      })
    },
    async SuggestCustomers() {
      this.isLoading.suggestCustomers = true

      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
        this.isLoading.suggestCustomers = false
      })
    },
  },
})
</script>
