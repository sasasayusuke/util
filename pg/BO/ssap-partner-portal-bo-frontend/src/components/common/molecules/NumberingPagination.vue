<template>
  <div :class="'common-numbering-pagination ' + className">
    <v-pagination
      v-model="page"
      :length="length"
      color="#018A19"
      @input="$emit('input', $event)"
    ></v-pagination>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    /**
     * ページネーションのクラス名
     */
    className: {
      type: String,
      required: false,
    },
    /**
     * ページネーションの現在のページ
     */
    page: {
      type: Number,
      required: true,
    },
    /**
     * ページネーションの最大ページ
     */
    length: {
      type: Number,
      required: true,
    },
  },
})
</script>

<style lang="scss">
.common-numbering-pagination {
  display: flex;
  justify-content: flex-end;

  .v-pagination__item {
    box-shadow: none;
    background-color: transparent !important;
  }

  .theme--light.v-pagination .v-pagination__item--active {
    background-color: #018a19 !important;
  }
}
</style>
