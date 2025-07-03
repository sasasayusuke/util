<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <KarteSortInput
      :param="param"
      :suggest-customers="suggestCustomers"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import KarteSortInput from '~/components/karte/molecules/KarteSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class ProjectSearchParam {
  customerId: string = ''
  fromYearMonth: string = format(getCurrentDate(), 'yyyy/MM')
  toYearMonth: string | null = null
  offsetPage: number = 1
}

export default BaseComponent.extend({
  name: 'KarteSort',
  components: {
    CommonSort,
    KarteSortInput,
  },
  props: {
    /**
     * 提案される顧客一覧
     */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /**
     * カルテ検索用のパラメータ
     */
    param: {
      type: Object,
      required: true,
    },
    /**
     * ページがリロード中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
