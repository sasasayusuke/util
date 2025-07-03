<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader>
      {{ pageName }}
    </ListInPageHeader>
    <CustomerSort
      :param="searchParam"
      :suggest-customers="suggestCustomers"
      :is-loading="isLoading.suggestCustomers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <CustomerListTable
      :customers="response.customers"
      :total="total"
      :offset-page="offsetPage"
      :limit="limit"
      :is-loading="isLoading.customers"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import CustomerSort, {
  CustomerSearchParam,
} from '~/components/customer/organisms/CustomerSort.vue'
import {
  GetCustomersRequest,
  GetCustomersResponse,
  SuggestCustomer,
} from '~/models/Customer'
import CustomerListTable from '~/components/customer/organisms/CustomerListTable.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import CommonList from '~/components/common/templates/CommonList'

export interface isLoading {
  customers: boolean
  suggestCustomers: boolean
}

export default CommonList.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    CustomerSort,
    CustomerListTable,
  },
  props: {
    /** GetCustomers API のレスポンス */
    response: {
      type: Object as PropType<GetCustomersResponse>,
      required: true,
    },
    /** ローディング中か否か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 取引先をサジェストする */
    suggestCustomers: {
      type: Array as PropType<SuggestCustomer[]>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: CustomerSearchParam,
      RequestType: GetCustomersRequest,
      apiName: 'getCustomers',
      headerPageName: this.$t('customer.group_info.name'),
      pageName: this.$t('customer.pages.index.name'),
    }
  },
})
</script>
