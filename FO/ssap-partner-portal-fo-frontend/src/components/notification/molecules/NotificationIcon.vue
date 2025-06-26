<template>
  <div class="m-NotificationIcon">
    <Icon :size="size" :color="color">
      <slot />
    </Icon>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Icon } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'NotificationIcon',
  components: { Icon },
  props: {
    /** アイコンのサイズ */
    size: {
      type: String,
      default: '24',
    },
    /** アイコンのカラー */
    color: {
      type: String,
      default: '#fff',
    },
  },
})
</script>

<style lang="scss" scoped>
.m-NotificationIcon {
  width: 36px;
  height: 36px;
  background-color: $c-primary-dark;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
