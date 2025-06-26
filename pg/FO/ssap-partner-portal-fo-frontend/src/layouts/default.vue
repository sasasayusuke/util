<template>
  <BaseLayout :type="3" />
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
