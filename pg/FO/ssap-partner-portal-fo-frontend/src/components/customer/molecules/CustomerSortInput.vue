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
        style-set="bgWhite"
        :loading="isLoading"
        @input="$emit('update', 'name', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { AutoComplete, Sheet } from '~/components/common/atoms'
import { SuggestCustomer } from '~/models/Customer'

export default BaseComponent.extend({
  components: {
    AutoComplete,
    Sheet,
  },
  props: {
    /** サジェストする取引先 */
    suggestCustomers: {
      type: Array as PropType<SuggestCustomer[]>,
      required: true,
    },
    /** ソートの条件 */
    param: {
      type: Object,
      required: true,
    },
    /** ローディング中か否か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    /** 取引先をサジェストする */
    autoCompleteItems(): string[] {
      return this.suggestCustomers.map(
        (suggestCustomer: SuggestCustomer) => suggestCustomer.name
      )
    },
  },
})
</script>

<style lang="scss" scoped>
.v-autocomplete {
  .v-input__append-inner {
    display: none;
  }
}
</style>
