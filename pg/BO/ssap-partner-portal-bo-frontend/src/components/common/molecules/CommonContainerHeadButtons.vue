<template>
  <v-container>
    <v-row>
      <template v-for="(button, index) in buttons">
        <v-col :key="index">
          <Button :style-set="button.styleSet" @click="button.onClick()">
            {{ button.label }}
          </Button>
        </v-col>
      </template>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    buttons: {
      type: Array,
      default() {
        return []
      },
    },
  },
})
</script>
