<template>
  <v-container class="m-common-anchor" fluid pa-0 ma-0 mt-1 mb-3>
    <v-row justify="end">
      <v-col v-for="(item, index) in items" :key="index" cols="auto">
        <Button
          :class="index ? 'ml-8' : ''"
          style-set="normal-primary-text"
          :to="item.target"
        >
          <Icon size="24">mdi-menu-down</Icon>
          {{ item.text }}
        </Button>
      </v-col>
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
