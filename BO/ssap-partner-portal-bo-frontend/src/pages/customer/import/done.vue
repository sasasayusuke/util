<template>
  <TemplateCustomerImportDone
    :response="response"
    :is-loading="isLoading"
    :error-status="errorStatus"
    :error-detail="errorDetail"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateCustomerImportDone from '~/components/customer/templates/CustomerImportDoneTemplate.vue'
import {
  ImportCustomersRequest,
  ImportCustomerResponse,
  ImportCustomers,
  ENUM_IMPORT_CUSTOMER_MODE,
} from '~/models/Customer'

export default BasePage.extend({
  name: 'CustomerImportDone',
  components: {
    TemplateCustomerImportDone,
  },
  middleware: ['roleCheck'],
  data() {
    return {
      response: new ImportCustomerResponse(),
      errorStatus: 0,
      errorDetail: '',
      isLoading: true,
    }
  },
  computed: {
    /**
     * @returns クエリパラメータのobjectKeyを返却
     */
    objectKey() {
      return this.$route.query.objectKey as string
    },
  },
  methods: {
    /**
     * ImportCustomersApi(/customers/import)を叩いて取引先を作成
     */
    async importCustomers() {
      this.isLoading = true

      const request = new ImportCustomersRequest()
      request.mode = ENUM_IMPORT_CUSTOMER_MODE.EXECUTE
      request.file = this.objectKey

      await ImportCustomers(request)
        .then((res) => {
          this.response = res.data
        })
        .catch((error) => {
          if (error && error.response && error.response.status) {
            this.errorStatus = error.response.status
          } else {
            // 不明なエラー類は一旦全てこちらに設定 ※番号はとりあえずで999としてます。
            this.errorStatus = 999
          }
          if (error?.response?.data?.detail) {
            this.errorDetail = error?.response?.data?.detail
          }
        })
        .finally(() => {
          this.isLoading = false
        })
    },
  },
  mounted() {
    this.displayLoading([this.importCustomers()])
  },
})
</script>
