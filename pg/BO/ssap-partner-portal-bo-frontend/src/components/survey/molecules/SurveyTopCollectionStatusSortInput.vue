<template>
  <Sheet class="d-flex align-center">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('survey.pages.index.sort_input.year') }}
    </p>
    <Sheet width="165" class="d-flex align-center mr-7">
      <YearSelect
        v-model="selectedYear"
        :init-date="initDate"
        @change="update('year', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import YearSelect from '~/components/common/molecules/YearSelect.vue'
import { Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    Sheet,
    YearSelect,
  },
  props: {
    /** 表示年 */
    selectedYear: {
      type: String,
      required: true,
    },
  },
  computed: {
    initDate() {
      return this.selectedYear
    },
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
