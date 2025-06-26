<template>
  <v-container pr-7>
    <v-row align="center">
      <p class="font-size-small mb-0 mr-2">
        {{ $t('karte.pages.index.sort_input.name') }}
      </p>
      <Sheet width="240" mr-7>
        <AutoComplete
          :value="param.customerId"
          outlined
          dense
          style-set="bgWhite"
          :items="autoCompleteItems"
          item-text="name"
          item-value="id"
          :placeholder="$t('common.placeholder.autoComplete')"
          hide-details
          :loading="isLoading"
          :max-length="255"
          @input="$emit('update', 'customerId', $event)"
        />
      </Sheet>
      <p class="font-size-small mb-0 mr-2 ml-7">
        {{ $t('karte.pages.index.sort_input.support') }}
      </p>
      <Sheet width="260" class="d-flex align-center">
        <MonthSelect
          v-model="param.fromYearMonth"
          :allowed-dates="allowedDates"
          style-set="bgWhite"
        />
        <span class="mx-2">〜</span>
        <MonthSelect
          v-model="param.toYearMonth"
          :allowed-dates="allowedDates"
          is-no-icon
          style-set="bgWhite"
        />
      </Sheet>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import { AutoComplete, TextField, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'KarteSortInput',
  components: {
    AutoComplete,
    TextField,
    Sheet,
    MonthSelect,
  },
  props: {
    /** 提案される顧客一覧 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** カルテ検索用のパラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** ページがリロード中か否か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    /** autoCompleteに表示するための提案する顧客の一覧を返す */
    autoCompleteItems() {
      return this.suggestCustomers
    },
  },
  methods: {
    /** 前後一年のみを選択可能とする */
    allowedDates(val: Date) {
      const beforeDate = getCurrentDate()
      const afterDate = getCurrentDate()
      beforeDate.setDate(beforeDate.getDate() - 365)
      afterDate.setDate(afterDate.getDate() + 365)

      const yearMonth = Number(format(new Date(val), 'yyyyMM'))
      const minYearMonth = Number(format(beforeDate, 'yyyyMM'))
      const maxYearMonth = Number(format(afterDate, 'yyyyMM'))

      return maxYearMonth >= yearMonth && minYearMonth <= yearMonth
    },
  },
})
</script>
