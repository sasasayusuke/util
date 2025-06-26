<template>
  <v-container class="o-home-menu">
    <v-row v-if="isCompany">
      <v-col v-for="(menuItem, index) in menuItems" :key="index">
        <SolverHomeMenuItem
          :menu-item="menuItem"
          :is-company="true"
          :is-category="false"
        />
      </v-col>
    </v-row>
    <v-row v-if="!isCompany">
      <v-col v-for="(menuItemAll, index) in menuItems" :key="index">
        <v-row>
          <v-col class="py-0">
            <SolverHomeMenuItem
              :menu-item="menuItemAll.category"
              :is-company="false"
              :is-category="true"
            />
          </v-col>
        </v-row>
        <v-row
          v-for="(menuItem, indexItem) in menuItemAll.items"
          :key="indexItem"
        >
          <v-col class="py-0">
            <SolverHomeMenuItem
              :menu-item="menuItem"
              :is-company="false"
              :is-category="false"
            />
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import SolverHomeMenuItem, { MenuItem } from '../molecules/SolverMenuItem.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'

interface MenuItemAll {
  category?: Object
  items: MenuItem[]
}

export default BaseComponent.extend({
  name: 'SolverMenu',
  components: {
    SolverHomeMenuItem,
  },
  props: {
    /** メニュー情報 */
    menuItems: {
      type: Array as PropType<MenuItemAll[]>,
      required: true,
    },
    /** 企業名か否か */
    isCompany: {
      type: Boolean,
      required: true,
    },
  },
})

export type { MenuItem }
</script>

<style lang="scss" scoped></style>
