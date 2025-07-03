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
    this.displayLoading([this.getCustomers(), this.suggestCustomers()])
  },
  methods: {
    /**
     * getCustomersApi(/customers)を叩いて取引先の一覧を取得
     */
    async getCustomers(
      params: GetCustomersRequest = new GetCustomersRequest()
    ) {
      this.isLoading.customers = true

      await GetCustomers(params).then((res) => {
        this.getCustomersResponse = res.data
        this.isLoading.customers = false
      })
    },
    /**
     * SuggestCustomersApi(/customers/suggest)を叩いて取引先のサジェスト用情報を取得
     */
    async suggestCustomers() {
      this.isLoading.suggestCustomers = true

      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
        this.isLoading.suggestCustomers = false
      })
    },
  },
})
</script>
