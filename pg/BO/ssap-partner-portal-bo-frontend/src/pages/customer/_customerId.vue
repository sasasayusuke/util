<template>
  <TemplateCustomerDetail :customer="customer" @refresh="refresh" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerDetail from '~/components/customer/templates/CustomerDetail.vue'
import { GetCustomerByIdResponse, GetCustomerById } from '~/models/Customer'

export default BasePage.extend({
  name: 'CustomerDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateCustomerDetail,
  },
  mounted() {
    this.displayLoading([this.getCustomerById()])
  },
  data() {
    return {
      customer: new GetCustomerByIdResponse(),
    }
  },
  methods: {
    /**
     * getCustomerByIdApi(/customers/${id})を叩いて取引先をIDで一意に取得
     */
    async getCustomerById() {
      const id: string = this.$route.params.customerId

      await GetCustomerById(id).then((res) => {
        this.customer = res.data
      })
    },
    refresh() {
      this.displayLoading([this.getCustomerById()])
    },
  },
})
</script>
