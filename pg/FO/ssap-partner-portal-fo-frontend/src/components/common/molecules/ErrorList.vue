<template>
  <Sheet class="m-error-list mt-4" style-set="error">
    <ul class="m-error-list__list">
      <li
        v-for="(error, index) in errors"
        :key="index"
        class="m-error-list__item font-size-small"
      >
        {{ error }}
      </li>
    </ul>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Sheet,
  },
  props: {
    errors: {
      type: Array,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-error-list__list {
  padding: 0;
  margin: 0;
}
.m-error-list__item {
  list-style: none;
  line-height: 1.7;
}
</style>
