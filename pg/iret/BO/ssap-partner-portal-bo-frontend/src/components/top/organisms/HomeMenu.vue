<template>
  <v-container class="o-home-menu">
    <v-row>
      <v-col v-for="(menuItem, index) in menuItems" :key="index" cols="4">
        <HomeMenuItem :menu-item="menuItem" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import HomeMenuItem, { MenuItem } from '../molecules/HomeMenuItem.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'HomeMenu',
  components: {
    HomeMenuItem,
  },
  props: {
    /**
     * メニュー情報
     */
    menuItems: {
      type: Array,
      required: true,
    },
  },
})

export type { MenuItem }
</script>

<style lang="scss" scoped></style>
