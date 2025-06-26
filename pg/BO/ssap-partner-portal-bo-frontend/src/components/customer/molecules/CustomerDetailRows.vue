<template>
  <v-form
    class="o-user-detail-rows pt-4 px-8 pb-0"
    :value="isValid"
    @input="$listeners['input']"
  >
    <CommonUpdateRow
      v-if="!isCreating"
      :name="customer.updateUserName"
      :date="formatDate(new Date(customer.updateAt), 'Y/MM/DD hh:mm')"
    />
    <!-- 取引先識別ID -->
    <CommonDetailRow
      v-if="!isCreating"
      chip="void"
      :label="$t('customer.row.id.name')"
      :value="customer.id"
    >
    </CommonDetailRow>
    <!-- SF最終更新日 -->
    <CommonDetailRow
      v-if="!isCreating"
      chip="sf"
      :label="$t('customer.row.SFupdateAt.name')"
      :value="
        customer.salesforceUpdateAt
          ? formatDate(new Date(customer.salesforceUpdateAt), 'Y/MM/DD hh:mm')
          : ''
      "
    >
    </CommonDetailRow>
    <!-- 取引先ID -->
    <CommonDetailRow
      v-if="!isCreating"
      chip="sf"
      :label="$t('customer.row.shortId.name')"
      :value="customer.salesforceCustomerId"
    >
    </CommonDetailRow>
    <!-- 取引先名 -->
    <CommonDetailRow
      chip="sf"
      :label="$t('customer.row.name.name')"
      required
      :is-editing="isEditing"
      :value="customer.name"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.name"
          outlined
          dense
          required
          :max-length="255"
          :placeholder="$t('common.placeholder.input')"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- カテゴリ -->
    <CommonDetailRow
      chip="sf"
      :label="$t('customer.row.category.name')"
      :is-editing="isEditing"
      :value="customer.category"
      required
    >
      <Sheet width="300">
        <Select
          v-model="localParam.category"
          :items="categoryItems"
          item-text="label"
          item-value="value"
          required
          :placeholder="$t('common.placeholder.input')"
          style-set="outlined bgWhite bgTransparent"
          :bg-transparent="true"
          :multiple="false"
        />
      </Sheet>
      <template #isNotEditing>
        {{ customer.category }}
      </template>
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import {
  TextField,
  Select,
  ToolTips,
  Sheet,
  Required,
  Chip,
} from '../../common/atoms/index'
import CommonUpdateRow from '../../common/molecules/CommonUpdateRow.vue'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'

// LocalCustomer型はGetCustomerByIdResponse型と同じ
import {
  GetCustomerByIdResponse,
  GetCustomerByIdResponse as LocalCustomer,
} from '~/models/Customer'
import { GetMasterKartenSelectBox } from '~/models/MasterKarte'
export { LocalCustomer }

export default CommonDetailRows.extend({
  components: {
    TextField,
    Select,
    ToolTips,
    Sheet,
    Required,
    Chip,
    CommonUpdateRow,
    CommonDetailRow,
  },
  props: {
    /**
     * IDから取得した取引先情報
     */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      localParam: Object.assign(new LocalCustomer(), this.customer),
      /** セレクターアイテム取得 ローディングフラグ */
      isSelectBoxLoading: false,
      /** 顧客セグメントのセレクターアイテム一覧 */
      categoryItems: [],
    }
  },
  created() {
    this.getMasterKartenSelectBox()
  },
  methods: {
    /**
     * パラメータをリセットする
     */
    resetLocalParam() {
      this.localParam = Object.assign(new LocalCustomer(), this.customer)
    },
    /**
     * カテゴリーセレクトボックスのアイテムを取得
     */
    async getMasterKartenSelectBox(): Promise<void> {
      this.isSelectBoxLoading = true

      await GetMasterKartenSelectBox().then((res) => {
        this.categoryItems = this.findSelectBox(res.data, 'category')
        this.isSelectBoxLoading = false
      })
    },
    /** 取得したセレクトボックスから該当のアイテムを取得 */
    findSelectBox(object: any, name: string) {
      const foundObject = object.find((obj: any) => obj.name === name)
      return foundObject.items
    },
  },
  // 取引先名が入力されたらisChanged=true を親コンポーネントに送る
  watch: {
    'localParam.name'(newValue, oldValue) {
      if (newValue !== oldValue) {
        this.$emit('isChanged', true)
      }
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
