<!-- templatesのルート要素(=Pages直下)にはこのコンポーネントを使用する -->

<template>
  <v-container class="b-RootTemplate" fluid :class="containerClass">
    <ErrorBar />

    <slot />
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import ErrorBar from '~/components/common/organisms/ErrorBar.vue'

export default BaseComponent.extend({
  components: {
    ErrorBar,
  },
  props: {
    containerClass: {
      type: String,
      default: 'pa-0', // サイト規定のデフォルトスタイルを指定
    },
  },
})
</script>

<style lang="scss" scoped>
.b-RootTemplate {
  padding: 32px 0 68px !important;
  width: 1200px;
  margin: 0 auto;
}
</style>
