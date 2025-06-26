<template>
  <Sheet class="d-flex align-center pr-7">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('customer.pages.index.sort_input.name.name') }}
    </p>
    <Sheet width="240" mr-7>
      <AutoComplete
        :value="param.name"
        outlined
        dense
        :items="autoCompleteItems"
        :placeholder="$t('common.placeholder.autoComplete')"
        hide-details
        :loading="isLoading"
        style-set="bgWhite"
        @input="$emit('update', 'name', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { AutoComplete, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    AutoComplete,
    Sheet,
  },
  props: {
    /**
     * サジェストする顧客情報
     */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /**
     * パラメータ
     */
    param: {
      type: Object,
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
  computed: {
    /**
     * @returns サジェストする顧客名
     */
    autoCompleteItems() {
      return this.suggestCustomers.map((elm: any) => elm.name)
    },
  },
})
</script>
