<template>
  <span v-if="isTooLong" class="a-overflow-tooltip">
    <v-tooltip v-model="isShow" top>
      <template #activator="{ on, attrs }">
        <span v-if="isClick" v-bind="attrs" @click="isShow = !isShow">
          {{ `${text.slice(0, max)}..` }}
        </span>
        <span v-else v-bind="attrs" v-on="on">
          {{ `${text.slice(0, max)}...` }}
        </span>
      </template>
      <span>{{ text }}</span>
    </v-tooltip>
  </span>
  <span v-else>{{ text }}</span>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  data() {
    return {
      isShow: false,
    }
  },
  props: {
    text: {
      type: String,
      required: false,
      default: '',
    },
    max: {
      type: Number,
      required: false,
    },
    isClick: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    isTooLong(): boolean {
      if (!this.text) {
        return false
      }
      if (!this.text.length) {
        return false
      }

      return this.text.length > this.max
    },
  },
})
</script>
