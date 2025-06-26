<template>
  <Sheet class="o-common-sort d-flex align-start py-4">
    <compnent :is="hx" class="o-common-sort__title font-size-normal mr-5">
      {{ title ? title : $t('common.sort') }} ：&nbsp;
      <Required v-if="isRequired" />
    </compnent>
    <div class="o-common-sort__main">
      <slot />
    </div>
    <div class="o-common-sort__buttons pl-7">
      <CommonSortButtons
        :disabled="disabled"
        @sort="$emit('sort')"
        @clear="$emit('clear')"
      />
    </div>
  </Sheet>
</template>

<script lang="ts">
import CommonSortButtons from '~/components/common/molecules/CommonSortButtons.vue'
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Required } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonSortButtons,
    Sheet,
    Required,
  },
  props: {
    title: {
      type: String,
      default: '',
    },
    isRequired: {
      type: Boolean,
      default: false,
    },
    hx: {
      type: String,
      default: 'h2',
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss">
.o-common-sort {
  &.is-tall {
    .o-common-sort__buttons {
      align-self: center;
      .col {
        display: flex;
        flex-direction: column;
        align-items: center;
      }
      .a-button {
        margin: 0 !important;
        &:nth-child(n + 2) {
          margin-top: 8px !important;
        }
      }
    }
  }
}
.o-common-sort__title {
  height: 32px;
  display: flex;
  align-items: center;
}
.o-common-sort__main {
  border-right: 1px solid $c-gray-line;
}
</style>
