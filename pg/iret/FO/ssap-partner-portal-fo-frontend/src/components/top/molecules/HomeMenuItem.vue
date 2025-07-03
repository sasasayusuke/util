<template>
  <v-card class="m-home-menu-item py-7" :to="menuItem.to" :ripple="false">
    <v-layout class="justify-center">
      <Icon size="55" color="#000">
        {{ menuItem.icon }}
      </Icon>
    </v-layout>
    <v-layout class="justify-center">
      <v-card-title
        class="py-5 pb-0 text-primary-dark font-weight-bold font-size-large"
      >
        {{ menuItem.title }}
      </v-card-title>
    </v-layout>
    <v-layout class="justify-center">
      <v-card-text class="text-center pt-1 pb-0 font-size-xsmall">
        {{ menuItem.text }}
      </v-card-text>
    </v-layout>
  </v-card>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Icon } from '~/components/common/atoms/index'

export interface MenuItem {
  icon: string
  title: string
  text: string
  to: string
}

export default BaseComponent.extend({
  components: {
    Icon,
  },
  props: {
    menuItem: {
      type: Object as PropType<MenuItem>,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-home-menu-item {
  &::before {
    display: none;
  }
  &:hover,
  &:focus {
    background-color: $c-primary-8;
  }
}
</style>
