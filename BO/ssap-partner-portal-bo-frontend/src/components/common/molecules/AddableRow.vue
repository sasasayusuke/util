<template>
  <tr>
    <slot />
    <td>
      <div class="ml-2">
        <Button
          v-if="rowLength >= 1 && isRemovable"
          :aria-label="$t('common.button.delete')"
          style-set="icon-addable"
          class="mr-1"
          @click="$emit('click:remove', $event)"
        >
          <Icon size="28">icon-org-button-minus-outline</Icon>
        </Button>
        <Button
          v-if="isLastRow && isAddable"
          :aria-label="$t('common.button.add')"
          style-set="icon-addable"
          @click="$emit('click:add', $event)"
        >
          <Icon size="28">icon-org-button-plus-outline</Icon>
        </Button>
      </div>
    </td>
  </tr>
</template>

<script lang="ts">
import { Button, Icon } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
  },
  props: {
    isLastRow: {
      type: Boolean,
      default: false,
    },
    isAddable: {
      type: Boolean,
      default: false,
    },
    categoryName: {
      type: String,
      default: '',
    },
    rowLength: {
      type: Number,
      default: 0,
    },
    isRemovable: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-addable-rows__edit {
  padding-left: 24px;
  text-align: left;
}
</style>
