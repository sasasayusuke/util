<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <CustomerSortInput
      :param="param"
      :suggest-customers="suggestCustomers"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import CustomerSortInput from '../molecules/CustomerSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'
import type { PropType } from '~/common/BaseComponent'
import { SuggestCustomer } from '~/models/Customer'

export class CustomerSearchParam {
  name = ''
}

export default BaseComponent.extend({
  name: '',
  components: {
    CommonSort,
    CustomerSortInput,
  },
  props: {
    /** サジェストする取引先 */
    suggestCustomers: {
      type: Array as PropType<SuggestCustomer[]>,
      required: true,
    },
    /** ソートの条件 */
    param: {
      type: Object,
      required: true,
    },
    /** ローディング中か否か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
