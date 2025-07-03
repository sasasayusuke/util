<template>
  <v-container pt-2 pb-10>
    <template v-if="$scopedSlots.button">
      <slot name="button" />
    </template>
    <v-row v-if="buttonDefault" class="pa-0 ma-0" justify="center">
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="buttonAction2"
      >
        {{
          isEditing
            ? $t('common.button.cancel')
            : $t('common.button.backToList')
        }}
      </Button>
      <Button
        class="ml-4"
        style-set="large-primary"
        width="160"
        @click="buttonAction1"
      >
        {{ isEditing ? $t('common.button.save') : $t('common.button.edit') }}
      </Button>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {
    isEditing: {
      type: Boolean,
      default: false,
    },
    buttonDefault: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    buttonAction1() {
      this.$emit('buttonAction1')
    },
    buttonAction2() {
      this.$emit('buttonAction2')
    },
  },
})
</script>
