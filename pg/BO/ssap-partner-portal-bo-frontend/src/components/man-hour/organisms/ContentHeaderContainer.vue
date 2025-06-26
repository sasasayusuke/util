<template>
  <header class="m-heading">
    <Sheet
      class="m-heading__body d-flex justify-space-between align-center pl-8"
      width="100%"
    >
      <div class="d-flex align-left">
        <compnent :is="tag" class="m-heading__title">
          <slot name="default" />
        </compnent>
        <slot name="summary" />
      </div>
      <div
        v-if="$scopedSlots.date || $scopedSlots.button"
        class="m-heading__ui d-flex align-center"
      >
        <p v-if="$scopedSlots.date" class="m-heading__date font-size-xsmall">
          <slot name="date" />
        </p>
        <div v-if="$scopedSlots.button" class="m-heading__button ml-8">
          <slot name="button" />
        </div>
      </div>
    </Sheet>
  </header>
</template>

<script lang="ts">
import { Button, Card, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'ContentHeaderContainer',
  components: {
    Button,
    Card,
    Sheet,
  },
  computed: {
    /** 受け取った数字(level)をHeadingタグに反映
     * @returns 反映されたHeadingタグ
     */
    tag(): string {
      return `h${this.level}`
    },
  },
  props: {
    /** Headingタグの数字 */
    level: {
      type: Number,
      default: 1,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-heading {
  .m-heading__body {
    height: 40px;
  }
  &.type2 {
    .m-heading__title {
      font-size: 1.5rem;
    }
    .m-heading__body {
      height: 60px;
      border-bottom: 2px solid $c-gray-line !important;
    }
  }
  &.type3 {
    width: 100%;
    .m-heading__title {
      font-size: 1.25rem;
    }
    .m-heading__body {
      height: 64px;
      border-bottom: 4px solid $c-primary-dark !important;
    }
  }
  &.type4 {
    .m-heading__title {
      font-size: 1.125rem;
      color: $c-primary-dark;
    }
    .m-heading__body {
      height: 45px;
      border-bottom: 4px solid $c-primary-dark !important;
    }
  }
  &.type5 {
    .m-heading__title {
      font-size: 1rem;
    }
    .m-heading__body {
      padding-left: 10px;
      height: auto;
      border-left: 6px solid $c-primary-dark !important;
    }
  }
}
.m-heading__title {
  font-size: 1.125rem;
  font-weight: bold;
}
.m-heading__date {
  margin: 0;
  color: $c-black-60;
}
</style>
