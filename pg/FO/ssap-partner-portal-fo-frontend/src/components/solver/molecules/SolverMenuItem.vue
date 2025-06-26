<template>
  <div>
    <v-card
      v-if="isCompany"
      class="m-solver-menu-item"
      :to="menuItem.to"
      :ripple="false"
    >
      <v-layout class="justify-space-between">
        <v-card-title class="py-5 pl-6 font-weight-bold font-size-xxlarge">
          {{ menuItem.title }}
        </v-card-title>
        <v-layout class="justify-end align-center">
          <div class="pr-2 font-size-small">
            {{ $t('solver.pages.menu.menuItem.solverCorporationInfo') }}
          </div>
          <Icon class="pr-6" size="14">icon-org-arrow-right</Icon>
        </v-layout>
      </v-layout>
    </v-card>
    <v-card
      v-if="!isCompany && isCategory"
      class="m-solver-menu-item-category"
      :ripple="false"
    >
      <v-layout class="justify-center">
        <v-card-title class="py-3 font-weight-bold font-size-xlarge">
          {{ menuItem.title }}
        </v-card-title>
      </v-layout>
    </v-card>
    <v-card
      v-if="!isCompany && !isCategory"
      class="m-solver-menu-item"
      :to="menuItem.to"
      :ripple="false"
    >
      <v-layout class="justify-space-between">
        <v-card-title
          class="py-6 pl-6 text-primary-dark font-weight-bold font-size-large"
        >
          {{ menuItem.title }}
        </v-card-title>
        <v-layout class="justify-end align-center">
          <Icon class="pr-2" size="14">icon-org-arrow-right</Icon>
        </v-layout>
      </v-layout>
    </v-card>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Icon } from '~/components/common/atoms/index'

export interface MenuItem {
  title: string
  to: string
}

export default BaseComponent.extend({
  name: 'SolverMenuItem',
  components: {
    Icon,
  },
  props: {
    /** メニュー情報 */
    menuItem: {
      type: Object,
      required: true,
    },
    /** 企業名か否か */
    isCompany: {
      type: Boolean,
      required: true,
    },
    /** カテゴリ名か否か */
    isCategory: {
      type: Boolean,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-solver-menu-item {
  &::before {
    display: none;
  }
  &:hover,
  &:focus {
    background-color: $c-primary-8;
    cursor: pointer;
  }
}
.m-solver-menu-item-category {
  position: relative;
  &::before {
    display: none;
  }
  &::after {
    content: '';
    position: absolute;
    bottom: 0;
    width: 100%;
    height: 2px;
    background-color: black;
  }
}
</style>
