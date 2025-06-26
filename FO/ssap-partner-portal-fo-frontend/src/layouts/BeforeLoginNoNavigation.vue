<template>
  <BaseLayout :type="5" />
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
