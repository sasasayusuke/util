<template>
  <tr>
    <slot />
    <td>
      <div class="ml-2">
        <Button
          v-if="rowLength >= 1"
          style-set="icon"
          class="mr-1"
          @click="$emit('click:remove', $event)"
        >
          <Icon size="28">icon-org-button-minus-outline</Icon>
        </Button>
        <Button
          v-if="isLastRow && isAddable"
          style-set="icon"
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
  },
})
</script>

<style lang="scss" scoped>
.m-addable-rows__edit {
  padding-left: 24px;
  text-align: left;
}
</style>
