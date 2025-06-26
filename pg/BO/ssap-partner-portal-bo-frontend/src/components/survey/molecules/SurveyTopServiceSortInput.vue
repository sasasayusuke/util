<template>
  <Sheet class="d-flex align-center">
    <Sheet width="260" class="d-flex align-center mr-7">
      <MonthSelect
        v-model="param.yearMonthFrom"
        @change="update('yearMonthFrom', $event)"
      />&nbsp;〜&nbsp;
      <MonthSelect
        v-model="param.yearMonthTo"
        is-no-icon
        @change="update('yearMonthTo', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import { TextField, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    TextField,
    Sheet,
    MonthSelect,
  },
  props: {
    /** 表示期間 */
    param: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      from: '',
      to: '',
    }
  },
  methods: {
    /**
     * 他コンポーネントへ表示期間の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>
