<template>
  <CommonDataTable
    :headers="customerHeaders"
    :items="formattedCustomer"
    :total="total"
    :offset-page="offsetPage"
    :limit="limit"
    :is-loading="isLoading"
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
    /** 取引先 */
    customers: {
      type: Array as PropType<CustomerListItem[]>,
      required: true,
    },
    /** 全ての管理者ユーザーの件数 */
    total: {
      type: Number,
      required: true,
    },
    /** 開始ページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 一ページに表示される管理者ユーザーの件数 */
    limit: {
      type: Number,
      required: true,
    },
    /** ローディング中か否か */
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
          width: '558px',
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
          width: '200px',
        },
        {
          text: this.$t('customer.pages.index.header.salesforceUpdateAt'),
          value: 'salesforceUpdateAt',
          sortable: false,
          width: '160px',
        },
        {
          text: this.$t('customer.pages.index.header.updateAt'),
          value: 'updateAt',
          sortable: false,
          width: '160px',
        },
      ],
    }
  },
  methods: {},
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedCustomer(): CustomerListItem[] {
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

<style lang="scss"></style>
