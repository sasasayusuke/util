<template>
  <div class="m-term-picker">
    <p class="m-term-picker__title">表示期間：</p>
    <div class="m-term-picker__body">
      <DateSelect /> <span class="m-term-picker__separator">〜</span>
      <DateSelect />
    </div>
    <v-divider vertical class="mx-7"></v-divider>
    <div class="m-term-picker__button">
      <Button color="primary" width="120px">絞り込み</Button>
      <Button text width="48px" class="ml-4">リセット</Button>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon } from '~/components/common/atoms/index'
import DateSelect from '~/components/common/molecules/DateSelect.vue'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    DateSelect,
  },
  props: {},
  data() {
    return {}
  },
  computed: {},
  methods: {},
})
</script>

<style lang="scss">
.m-term-picker {
  display: flex;
  align-items: center;
  .a-date-select {
    &:nth-of-type(n + 2) {
      .v-input__prepend-outer {
        display: none;
      }
    }
  }
}
.m-term-picker__title {
  margin-bottom: 0 !important;
  margin-right: 12px;
  font-size: 1rem;
  font-weight: bold;
}
.m-term-picker__body {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 300px;
}
.m-term-picker__separator {
  margin: 0 8px;
}
</style>
