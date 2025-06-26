<template>
  <DetailContainer
    :title="title"
    :is-hide-button1="isHideButton1"
    hx="h1"
    @click:negative="$emit('click:negative')"
  >
    <CustomerDetailRows
      v-model="isValid"
      :customer="customer"
      :is-loading="isLoading"
    />
    <template #footerButton>
      <Button
        style-set="large-tertiary"
        outlined
        width="160"
        to="/customer/list"
      >
        {{ $t('common.button.backToList') }}
      </Button>
    </template>
  </DetailContainer>
</template>

<script lang="ts">
import type { LocaleMessages } from 'vue-i18n/types'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import CustomerDetailRows from '~/components/customer/molecules/CustomerDetailRows.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { GetCustomerByIdResponse } from '~/models/Customer'
import { Button } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CustomerDetailRows,
    DetailContainer,
    Button,
  },
  props: {
    /** ローディング中か否か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 一意に取得した取引先 */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
    },
  },
  data() {
    return {
      isValid: true,
      isHideButton1: true,
    }
  },
  computed: {
    title(): string | LocaleMessages {
      return this.$t('customer.pages.detail.name')
    },
  },
})
</script>
