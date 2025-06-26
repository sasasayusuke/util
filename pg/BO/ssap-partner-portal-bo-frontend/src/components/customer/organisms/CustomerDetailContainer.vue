<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive', localCustomer)"
    @click:negative="$emit('click:negative')"
  >
    <CustomerDetailRows
      v-model="isValid"
      :customer="customer"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @isChanged="update('isChanged', $event)"
      @update="update('localCustomer', $event)"
    />
    <template #button>
      <slot name="button" />
    </template>
  </DetailContainer>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import CustomerDetailRows, {
  LocalCustomer,
} from '~/components/customer/molecules/CustomerDetailRows.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { GetCustomerByIdResponse } from '~/models/Customer'

export { LocalCustomer }

export default CommonDetailContainer.extend({
  components: {
    CustomerDetailRows,
    DetailContainer,
  },
  props: {
    /**
     * IDから取得した取引先情報
     */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
    },
    /**
     * お客様情報を登録/更新中か
     */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localCustomer: new LocalCustomer(),
    }
  },
  /**
   * @returns タイトル名
   */
  computed: {
    title() {
      return this.$t('customer.pages.' + this.mode + '.name')
    },
  },
})
</script>
