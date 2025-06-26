<template>
  <Sheet class="m-project-sort-input">
    <Sheet class="m-project-sort-input__unit d-flex align-center pr-7">
      <Sheet width="270" class="mt-1">
        <RadioGroup
          v-if="!isSalesMgr"
          v-model="param.allAssigned"
          :labels="[
            $t('project.pages.index.sort_input.types.main2'),
            $t('project.pages.index.sort_input.types.all2'),
          ]"
          :values="[false, true]"
          horizontal
          hide-details
        />
      </Sheet>
    </Sheet>
    <Sheet class="m-project-sort-input__unit d-flex align-center pr-7 mt-5">
      <!-- お客様名 -->
      <p class="font-size-small mb-0 mr-2">
        {{ $t('project.pages.index.sort_input.customer') }}
      </p>
      <Sheet width="340">
        <AutoComplete
          v-model="param.customerId"
          item-text="name"
          item-value="id"
          :items="suggestCustomers"
          :loading="isLoading"
          style-set="outlined bgWhite"
          :placeholder="$t('common.placeholder.autoComplete')"
          @input="$emit('update', 'customerName', $event)"
        />
      </Sheet>
      <!-- 期間指定 -->
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('project.pages.index.sort_input.period') }}
      </p>
      <Sheet class="mr-2" width="150">
        <DateSelect
          v-model="param.dateFrom"
          :allowed-dates="allowedDatesFrom"
          style-set="bgWhite"
          @change="$emit('update', 'dateFrom', $event)"
        />
      </Sheet>
      〜
      <Sheet class="ml-2" width="117">
        <DateSelect
          v-model="param.dateTo"
          :allowed-dates="allowedDatesTo"
          style-set="bgWhite"
          class="is-no-icon"
          @change="$emit('update', 'dateTo', $event)"
        />
      </Sheet>
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { AutoComplete, Sheet, RadioGroup } from '~/components/common/atoms'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import { meStore } from '~/store'

export default BaseComponent.extend({
  name: 'ProjectSortInput',
  components: {
    AutoComplete,
    DateSelect,
    Sheet,
    RadioGroup,
  },
  props: {
    /** 絞り込みパラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** 一般ユーザーのサジェスト用情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      /** 「期間指定」開始日時 */
      localFrom: '',
      /** 「期間指定」修了日時 */
      localTo: '',
    }
  },
  watch: {
    param: {
      deep: true,
      handler(newValue: any) {
        this.localFrom = newValue.dateFrom
        this.allowedDatesFrom(newValue.dateFrom)
        this.localTo = newValue.dateTo
        this.allowedDatesTo(newValue.dateTo)
      },
    },
  },
  computed: {
    /** 営業責任者 */
    isSalesMgr(): boolean {
      if (meStore.role === 'sales_mgr') {
        return true
      } else {
        return false
      }
    },
  },
  methods: {
    /**
     * 「期間指定」開始日時が「期間指定」修了日時よりも前ならtrueを返す
     * @param val 絞り込みパラメータの「期間指定」開始日時
     */
    allowedDatesFrom(val: string) {
      if (this.localTo !== '') {
        const inputDate = new Date(val)
        const toDate = new Date(this.localTo)
        return inputDate < toDate
      } else {
        return true
      }
    },
    /**
     * 「期間指定」修了日時が「期間指定」開始日時よりも後ならtrueを返す
     * @param val 絞り込みパラメータの「期間指定」修了日時
     */
    allowedDatesTo(val: string) {
      if (this.localFrom !== '') {
        const inputDate = new Date(val)
        const fromDate = new Date(this.localFrom)
        return inputDate > fromDate
      } else {
        return true
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.m-project-sort-input__unit {
  &:nth-child(2) {
    margin-left: -100px;
  }
}
</style>
