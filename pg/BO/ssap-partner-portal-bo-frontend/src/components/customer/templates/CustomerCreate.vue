<template>
  <RootTemplate>
    <CustomerDetailContainer
      :customer="customer"
      is-editing
      is-creating
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import CustomerDetailContainer, {
  LocalCustomer,
} from '../organisms/CustomerDetailContainer.vue'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  CreateCustomerRequest,
  CreateCustomer,
  GetCustomerByIdResponse,
} from '~/models/Customer'

export { LocalCustomer }

export default CommonCreate.extend({
  components: {
    CustomerDetailContainer,
    RootTemplate,
  },
  props: {
    /**
     * IDから取得した取引先情報
     */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      listPagePath: this.backToUrl('/customer/list'),
      detailPagePrefix: '/customer/',
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 顧客を作成
     * @param 顧客情報
     */
    create(localCustomer: LocalCustomer) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new CreateCustomerRequest()
      Object.assign(request, localCustomer)

      CreateCustomer(request)
        .then((res) => {
          const id = res.data.id
          this.toDetailPage(id)
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
  },
})
</script>
