<template>
  <!-- <header class="m-heading"> -->
  <Sheet
    class="m-heading d-flex justify-space-between align-center"
    height="60"
  >
    <compnent :is="tag" class="m-heading__title">
      <slot name="default" />
    </compnent>
    <div class="m-heading__ui d-flex align-center">
      <p v-if="$scopedSlots.date" class="m-heading__date font-size-xsmall">
        <slot name="date" />
      </p>
      <div class="m-heading__button ml-8">
        <slot name="button" />
        <slot name="button-csv" />
        <slot name="button-back" />
        <slot name="button-month" />
      </div>
    </div>
  </Sheet>
  <!-- </header> -->
</template>

<script lang="ts">
import { Button, Card, Sheet } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Card,
    Sheet,
  },
  computed: {
    tag() {
      return `h${this.level}`
    },
  },
  props: {
    level: {
      type: Number,
      default: 1,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-heading {
  // .m-heading__body {
  //   height: 40px;
  // }
  border-bottom: 2px solid $c-gray-line !important;
  // &.type2 {
  //   .m-heading__title {
  //     font-size: 1.5rem;
  //   }
  //   .m-heading__body {
  //     height: 60px;
  //     border-bottom: 2px solid $c-gray-line !important;
  //   }
  // }
  // &.type3 {
  //   .m-heading__title {
  //     font-size: 1.25rem;
  //   }
  //   .m-heading__body {
  //     height: 60px;
  //     padding: 0 30px;
  //     border-bottom: 4px solid $c-primary-dark !important;
  //   }
  // }
  // &.type4 {
  //   .m-heading__title {
  //     font-size: 1.125rem;
  //     color: $c-primary-dark;
  //   }
  //   .m-heading__body {
  //     height: 45px;
  //     border-bottom: 4px solid $c-primary-dark !important;
  //   }
  // }
  // &.type5 {
  //   .m-heading__title {
  //     font-size: 1rem;
  //   }
  //   .m-heading__body {
  //     padding-left: 10px;
  //     height: auto;
  //     border-left: 6px solid $c-primary-dark !important;
  //   }
  // }
}
.m-heading__title {
  // font-size: 1.125rem;
  font-size: 1.5rem;
  font-weight: bold;
}
.m-heading__date {
  margin: 0;
  color: $c-black-60;
}
.m-heading__button {
}
</style>
