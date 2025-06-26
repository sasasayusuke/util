<template>
  <p :class="{ disabled: disabled }">
    {{ label }}
  </p>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'TextFieldLabel',
  props: {
    /** 入力フォームラベル */
    label: {
      type: String,
      required: true,
    },
    /** 無効化フラグ */
    disabled: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style scoped lang="scss">
p {
  font-size: 0.85rem;
  font-weight: bold;
  margin-bottom: 0.25rem;

  &.disabled {
    opacity: 0.5;
  }
}
</style>
