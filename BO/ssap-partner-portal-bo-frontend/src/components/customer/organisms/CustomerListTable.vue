<template>
  <CommonDataTable
    :headers="customerHeaders"
    :items="formattedCustomer"
    :total="total"
    :offset-page="offsetPage"
    :limit="limit"
    :is-loading="isLoading"
    class="o-customer-list-table"
    v-on="$listeners"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import { CustomerListItem } from '~/models/Customer'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { formatDateStr } from '~/utils/common-functions'

export default BaseComponent.extend({
  name: '',
  components: {
    CommonDataTable,
  },
  props: {
    /**
     * 顧客情報
     */
    customers: {
      type: Array as PropType<CustomerListItem[]>,
      required: true,
    },
    /**
     * データの合計
     */
    total: {
      type: Number,
      required: true,
    },
    /**
     * 開始ページ
     */
    offsetPage: {
      type: Number,
      required: true,
    },
    /**
     * 1ページに表示される制限数
     */
    limit: {
      type: Number,
      required: true,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { customerHeaders: IDataTableHeader[] } {
    return {
      customerHeaders: [
        {
          text: this.$t('customer.pages.index.header.name'),
          align: 'start',
          sortable: false,
          value: 'name',
          // 幅が45だと狭すぎるので590に変更しました
          width: '590px',
          maxLength: 45,
          link: {
            prefix: '/customer/',
            value: 'id',
          },
        },
        {
          text: this.$t('customer.pages.index.header.category'),
          sortable: false,
          value: 'category',
          width: '230px',
        },
        {
          text: this.$t('customer.pages.index.header.salesforceUpdateAt'),
          value: 'salesforceUpdateAt',
          width: '151px',
        },
        {
          text: this.$t('customer.pages.index.header.updateAt'),
          value: 'updateAt',
          width: '151px',
        },
      ],
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    /**
     * 顧客情報をフォーマット化
     * @returns フォーマット化された顧客情報
     */
    formattedCustomer() {
      return this.customers.map((customer) => {
        const rtn = Object.assign(new CustomerListItem(), customer)
        if (customer.salesforceUpdateAt) {
          rtn.salesforceUpdateAt = formatDateStr(
            customer.salesforceUpdateAt,
            'Y/MM/dd HH:mm'
          )
        }

        rtn.updateAt = formatDateStr(customer.updateAt, 'Y/MM/dd HH:mm')

        return rtn
      })
    },
  },
})
</script>

<style lang="scss">
.o-customer-list-table {
  td {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    &:nth-of-type(1) {
      max-width: 590px;
    }
    &:nth-of-type(2) {
      max-width: 230px;
    }
    &:nth-of-type(3),
    &:nth-of-type(4) {
      max-width: 151px;
    }
  }
}
</style>
