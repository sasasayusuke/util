<template>
  <BaseLayout :type="3" class="wide_layout" />
</template>

<script lang="ts">
import BaseLayout from './BaseLayout.vue'

/**
 * 標準レイアウト
 */
export default BaseLayout.extend({
  components: {
    BaseLayout,
  },
})
</script>
<style lang="scss">
.wide_layout {
  .ssap__main {
    width: 1350px;
  }
}
</style>
