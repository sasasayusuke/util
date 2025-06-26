<template>
  <v-container class="m-common-anchor" pa-0 ma-0 mt-4 mb-6>
    <v-row justify="end">
      <Button
        v-for="(item, index) in items"
        :key="index"
        :class="index ? 'ml-8' : ''"
        style-set="normal-primary-text"
        :to="item.target"
      >
        <Icon size="24">mdi-menu-down</Icon>
        {{ item.text }}
      </Button>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button, Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
  },
  computed: {},
  props: {
    items: {
      type: Array,
      required: true,
    },
  },
})
</script>
<style lang="scss">
.m-common-anchor {
  .v-btn--text {
    &:hover,
    &:focus {
      .v-btn__content {
        color: $c-primary-over;
      }
    }
  }
}
</style>
