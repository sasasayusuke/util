<template>
  <v-data-table
    :headers="customerHeaders"
    :items="formattedCustomer"
    :loading="isLoading"
    :no-data-text="$t('common.label.noData')"
    :loading-text="$t('common.label.loading')"
    disable-pagination
  >
    <template #top="{ pagination }">
      <Paragraph style-set="confirm-text">
        {{ $t('customer.pages.import.confirm.processed') }}
        <strong>
          <strong>{{ pagination.itemsLength || '-' }} </strong>
          {{ $t('customer.pages.import.confirm.number') }}</strong
        >
      </Paragraph>
    </template>
    <template #[`footer.page-text`]></template>

    <!-- (アンケート)アンケート記載時のプロジェクト名 -->
    <template #[`header.name`]="{ header }">
      <OverflowTooltip :text="header.text" :max="16" />
    </template>
  </v-data-table>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import { ImportedCustomer } from '@/types/Customer'
import { formatDateStr } from '~/utils/common-functions'

import { Paragraph } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'CustomerImportSelectFile',
  components: {
    CommonContainer,
    Paragraph,
    OverflowTooltip,
  },
  props: {
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /**
     * 顧客情報
     */
    customers: {
      type: Array as PropType<ImportedCustomer[]>,
      required: true,
    },
  },
  data() {
    return {
      customerHeaders: [
        {
          text: this.$t('customer.pages.import.tableHeader.customerId'),
          value: 'salesforceCustomerId',
          sortable: false,
        },
        {
          text: this.$t('customer.pages.import.tableHeader.salesforceUpdateAt'),
          value: 'salesforceUpdateAt',
          sortable: false,
        },
        {
          text: this.$t('customer.pages.import.tableHeader.name'),
          value: 'name',
          sortable: false,
        },
        {
          text: this.$t('customer.pages.import.tableHeader.salesforceTarget'),
          value: 'salesforceTarget',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCreditLimit'
          ),
          value: 'salesforceCreditLimit',
          sortable: false,
        },
        {
          text: this.$t('customer.pages.import.tableHeader.category'),
          value: 'category',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCreditGetMonth'
          ),
          value: 'salesforceCreditGetMonth',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCreditManager'
          ),
          value: 'salesforceCreditManager',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCreditNoRetry'
          ),
          value: 'salesforceCreditNoRetry',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforcePawsCreditNumber'
          ),
          value: 'salesforcePawsCreditNumber',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCustomerOwner'
          ),
          value: 'salesforceCustomerOwner',
          sortable: false,
        },
        {
          text: this.$t(
            'customer.pages.import.tableHeader.salesforceCustomerSegment'
          ),
          value: 'salesforceCustomerSegment',
          sortable: false,
        },
        {
          text: this.$t('customer.pages.import.tableHeader.errorMessage'),
          value: 'errorMessage',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    /**
     * 顧客情報をフォーマット化
     * @returns フォーマット化された顧客情報
     */
    formattedCustomer() {
      return this.customers.map((customer) => {
        const rtn = Object.assign(new ImportedCustomer(), customer)

        rtn.salesforceUpdateAt = formatDateStr(
          customer.salesforceUpdateAt,
          'yyyy/MM/dd HH:mm'
        )

        if (rtn.salesforceCreditGetMonth !== '') {
          rtn.salesforceCreditGetMonth = formatDateStr(
            customer.salesforceCreditGetMonth,
            'yyyy/MM'
          )
        }

        rtn.salesforceCreditLimit = Number(
          rtn.salesforceCreditLimit
        ).toLocaleString()
        return rtn
      })
    },
  },
})
</script>
