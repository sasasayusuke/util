<template>
  <TemplateCustomerDetail :customer="customer" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerDetail from '~/components/customer/templates/CustomerDetail.vue'
import { GetCustomerByIdResponse, GetCustomerById } from '~/models/Customer'

export default BasePage.extend({
  name: 'CustomerDetail',
  layout: 'noPageHeader',
  components: {
    TemplateCustomerDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('customer.pages.detail.name') as string,
    }
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
    async getCustomerById() {
      const id: string = this.$route.params.customerId

      await GetCustomerById(id)
        .then((res) => {
          if (res.data) {
            this.customer = res.data
          }
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
  },
})
</script>
