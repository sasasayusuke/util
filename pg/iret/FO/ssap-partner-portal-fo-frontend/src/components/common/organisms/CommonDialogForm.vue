<template>
  <v-dialog :value="isOpen" @input="$emit('update:isOpen', $event)">
    <template #activator="slotData">
      <slot name="activator" v-bind="slotData" />
    </template>

    <v-card>
      <v-card-title>
        {{ title }}
      </v-card-title>
      <v-card-text>
        <slot />
      </v-card-text>
      <v-card-actions>
        <Button @click="close">Close</Button>
        <Button :disabled="!isValid" @click="$emit('click:save')">Save</Button>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Button } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    isValid: {
      type: Boolean,
      required: true,
    },
    title: {
      type: String,
    },
  },
  methods: {
    close() {
      this.$emit('update:isOpen', false)
    },
  },
})
</script>
