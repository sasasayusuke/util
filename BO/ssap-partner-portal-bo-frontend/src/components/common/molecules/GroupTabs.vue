<template>
  <v-layout class="m-group-button">
    <slot />
  </v-layout>
</template>

<style lang="scss" scoped>
.m-group-button {
  box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
    0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
  .v-btn {
    &:first-of-type {
      border-start-end-radius: 0;
      border-end-end-radius: 0;
    }
    &:last-of-type {
      border-start-start-radius: 0;
      border-end-start-radius: 0;
    }
    &:nth-child(n + 2):nth-last-child(n + 2) {
      border-radius: 0;
    }
  }
}
</style>
