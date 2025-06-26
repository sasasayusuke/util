<template>
  <v-form
    class="o-user-detail-rows pt-4 px-8 pb-0"
    :value="isValid"
    @input="$listeners['input']"
  >
    <CommonUpdateRow
      :name="customer.updateUserName"
      :date="
        customer.updateAt != null
          ? formatDate(new Date(customer.updateAt), 'Y/MM/DD hh:mm')
          : null
      "
    />
    <!-- 取引先識別ID -->
    <CommonDetailRow
      badge
      chip="void"
      sheet
      :label="$t('customer.row.id.name')"
      :value="customer.id"
    >
    </CommonDetailRow>
    <!-- SF最終更新日 -->
    <CommonDetailRow
      badge
      chip="sf"
      :label="$t('customer.row.SFupdateAt.name')"
      :value="
        customer.salesforceUpdateAt != null
          ? formatDate(new Date(customer.salesforceUpdateAt), 'Y/MM/DD hh:mm')
          : null
      "
    >
    </CommonDetailRow>
    <!-- 取引先ID -->
    <CommonDetailRow
      badge
      chip="sf"
      :label="$t('customer.row.shortId.name')"
      :value="customer.salesforceCustomerId"
    >
    </CommonDetailRow>
    <!-- 取引先名 -->
    <CommonDetailRow
      badge
      chip="sf"
      :label="$t('customer.row.name.name')"
      :value="customer.name"
    >
    </CommonDetailRow>
    <!-- カテゴリ -->
    <CommonDetailRow
      badge
      chip="sf"
      :label="$t('customer.row.category.name')"
      :value="customer.category"
    >
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import {
  TextField,
  Select,
  ToolTips,
  Sheet,
  Chip,
} from '../../common/atoms/index'
import CommonUpdateRow from '../../common/molecules/CommonUpdateRow.vue'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'

import { GetCustomerByIdResponse } from '~/models/Customer'

export default CommonDetailRows.extend({
  components: {
    TextField,
    Select,
    ToolTips,
    Sheet,
    Chip,
    CommonUpdateRow,
    CommonDetailRow,
  },
  props: {
    /** 一意に取得した取引先 */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.o-user-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-user-detail-rows__select__item {
  list-style: none;
  display: inline-block;
  margin-right: 1em;
  &::after {
    content: ',';
  }
  &:last-child {
    &::after {
      display: none;
    }
  }
}
</style>
