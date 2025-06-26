<template>
  <RootTemplate>
    <CustomerDetailContainer
      :customer="customer"
      :is-loading="isLoading"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import CustomerDetailContainer from '../organisms/CustomerDetailContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import { GetCustomerByIdResponse } from '~/models/Customer'

export default CommonDetail.extend({
  components: {
    CustomerDetailContainer,
    RootTemplate,
  },
  props: {
    /** 一意に取得した取引先 */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      listPagePath: '/customer/list',
    }
  },
})
</script>
