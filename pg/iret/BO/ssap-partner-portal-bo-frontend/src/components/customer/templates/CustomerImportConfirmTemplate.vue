<template>
  <RootTemplate>
    <template v-if="!isError">
      <CustomerImportConfirm
        :customers="response.customers"
        :is-loading="isLoading"
      />
    </template>
    <template v-else>
      <CustomerImportFailed
        :customers="response.customers"
        :error-status="errorStatus"
        :error-detail="errorDetail"
        :is-loading="isLoading"
      />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import CustomerImportConfirm from '~/components/customer/organisms/CustomerImportConfirm.vue'
import CustomerImportFailed from '~/components/customer/organisms/CustomerImportFailed.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  ImportCustomerResponse,
  ENUM_IMPORT_CUSTOMER_RESULT,
} from '~/models/Customer'

export default BaseComponent.extend({
  name: 'TemplateCustomerImportConfirm',
  created() {
    //一括登録機能は利用不可のため、404画面にリダイレクトする
    this.$router.push('/404')
  },
  components: {
    RootTemplate,
    CustomerImportConfirm,
    CustomerImportFailed,
  },
  props: {
    /**
     * 顧客情報
     */
    response: {
      type: Object as PropType<ImportCustomerResponse>,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /**
     * エラーステータス
     */
    errorStatus: {
      type: Number,
    },
    /**
     * エラー情報詳細
     */
    errorDetail: {
      type: String,
      default: '',
    },
  },
  computed: {
    /**
     * エラーかどうかを判定する
     * @returns エラーか否か
     */
    isError(): boolean {
      if (this.response.result === ENUM_IMPORT_CUSTOMER_RESULT.NG) {
        return true
      }
      if (this.errorStatus) {
        return true
      }
      return false
    },
  },
})
</script>
