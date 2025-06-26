<template>
  <div class="m-term-picker">
    <p class="m-term-picker__title">
      {{ $t('top.pages.home.label.displayPeriod') }}：<Required />
    </p>
    <div class="m-term-picker__body">
      <MonthSelect v-model="param.yearMonthFrom" style-set="bgWhite" />
      <span class="m-term-picker__separator">〜</span
      ><MonthSelect v-model="param.yearMonthTo" style-set="bgWhite" />
    </div>
    <v-divider vertical class="mx-7"></v-divider>
    <div class="m-term-picker__button">
      <Button color="primary" width="120px" @click="$emit('sort')">
        {{ $t('common.button.sort') }}
      </Button>
      <Button text width="48px" class="ml-4" @click="$emit('clear')">
        {{ $t('common.button.reset') }}
      </Button>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon, Required } from '~/components/common/atoms/index'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    Required,
    MonthSelect,
  },
  props: {
    param: {
      type: Object,
      required: true,
    },
  },
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
