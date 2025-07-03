<template>
  <Sheet class="m-project-sort-input">
    <Sheet class="m-project-sort-input__unit d-flex align-center pr-7">
      <!-- お客様名 -->
      <p class="font-size-small mb-0 mr-2">
        {{ $t('project.pages.index.sortInput.customer') }}
      </p>
      <Sheet width="240" mr-7>
        <AutoComplete
          v-model="param.customerId"
          :aria-label="$t('project.pages.index.sortInput.customer')"
          outlined
          item-text="name"
          item-value="id"
          style-set="bgWhite"
          :items="suggestCustomers"
          :loading="isLoading.suggestCustomers"
          :placeholder="$t('common.placeholder.autoComplete')"
          @input="$emit('update', 'customerName', $event)"
        />
      </Sheet>
      <!-- サービス名 -->
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('project.pages.index.sortInput.service') }}
        <Required />
      </p>
      <Sheet width="240">
        <Select
          v-model="param.serviceTypeId"
          :aria-label="$t('project.pages.index.sortInput.service')"
          outlined
          dense
          :items="serviceTypes.serviceTypes"
          item-text="name"
          item-value="id"
          :loading="isLoading.serviceTypes"
          style-set="bgWhite"
        />
      </Sheet>
      <!-- 課 -->
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('project.pages.index.sortInput.division') }}
        <Required />
      </p>
      <Sheet width="100">
        <Select
          v-model="param.supporterOrganizationId"
          :aria-label="$t('project.pages.index.sortInput.division')"
          outlined
          dense
          :items="supporterOrganizations.supporterOrganizations"
          item-text="shortName"
          item-value="id"
          :loading="isLoading.supporterOrganizations"
          style-set="bgWhite"
        />
      </Sheet>
    </Sheet>
    <Sheet class="m-project-sort-input__unit d-flex align-center pr-7 mt-5">
      <!-- 支援期間 -->
      <p class="font-size-small mb-0 mr-2">
        {{ $t('project.pages.index.sortInput.supportPeriod') }}
      </p>
      <Sheet class="mr-2" width="150">
        <MonthSelect
          v-model="param.fromYearMonth"
          :allowed-dates="allowedDatesFrom"
          style-set="bgWhite"
          @change="$emit('update', 'fromYearMonth', $event)"
        />
      </Sheet>
      〜
      <Sheet class="ml-2" width="117">
        <MonthSelect
          v-model="param.toYearMonth"
          :allowed-dates="allowedDatesTo"
          is-no-icon
          style-set="bgWhite"
          @change="$emit('update', 'toYearMonth', $event)"
        />
      </Sheet>
      <!-- ステータス -->
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('project.pages.index.sortInput.status.name') }}
        <Required />
      </p>
      <Sheet width="160">
        <Select
          v-model="param.status"
          :aria-label="$t('project.pages.index.sortInput.status.name')"
          outlined
          dense
          :items="statuses"
          item-text="label"
          item-value="value"
          style-set="bgWhite"
        />
      </Sheet>
      <!-- 営業担当者 -->
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('project.pages.index.sortInput.salesUsers') }}
      </p>
      <Sheet width="240">
        <AutoComplete
          v-model="param.mainSalesUserId"
          :aria-label="$t('project.pages.index.sortInput.salesUsers')"
          outlined
          :items="suggestSalesUsers"
          item-text="name"
          item-value="id"
          :loading="isLoading.suggestSalesUsers"
          style-set="bgWhite"
          :placeholder="$t('common.placeholder.autoComplete')"
          @input="$emit('update', 'mainSalesUserName', $event)"
        />
      </Sheet>
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import {
  TextField,
  Select,
  AutoComplete,
  Sheet,
  Required,
} from '~/components/common/atoms'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

// ステータス 選択肢
const SELECT_ITEMS = ['before', 'during', 'after']

export default BaseComponent.extend({
  components: {
    TextField,
    Select,
    AutoComplete,
    MonthSelect,
    Sheet,
    Required,
  },
  props: {
    /** 絞り込みパラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** お客様名のサジェスト用情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** ユーザー名のサジェスト用情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** サービス名の一覧情報 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 課の一覧情報 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Object,
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
        this.localFrom = newValue.fromYearMonth
        this.allowedDatesFrom(newValue.fromYearMonth)
        this.localTo = newValue.toYearMonth
        this.allowedDatesTo(newValue.toYearMonth)
      },
    },
  },
  computed: {
    statuses() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }

      SELECT_ITEMS.forEach((elm) => {
        selectItem.label = this.$t(
          'project.pages.index.sortInput.status.items.' + elm
        ) as string
        selectItem.value = elm
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      })
      return selectItems
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
