<template>
  <Sheet class="d-flex align-center pr-7">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('master.pages.index.sort_input.type') }}
    </p>
    <Sheet width="240" mr-7>
      <Select
        :value="param.dataType"
        :aria-label="$t('master.pages.index.sort_input.type')"
        dense
        outlined
        :items="dataTypes"
        style-set="bgWhite"
        item-text="label"
        item-value="value"
        @change="$emit('update', 'dataType', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Select, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'MasterSortInput',
  components: {
    Select,
    Sheet,
  },
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    /**
     * データ種別
     */
    dataTypes: Array,
    value: {
      type: String,
      default: '',
    },
    /**
     * パラメーター
     */
    param: {
      type: Object,
      required: true,
    },
  },
})
</script>
