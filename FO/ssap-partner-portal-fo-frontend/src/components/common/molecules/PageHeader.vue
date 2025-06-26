<template>
  <v-app-bar class="m-page-header" elevation="4" height="60" color="#fff">
    <v-layout>
      <v-flex
        align-item-center="true"
        class="d-flex flex-column justify-center"
      >
        <h1 class="m-page-header__title">
          {{ $t('header.global.menu.survey') }}
        </h1>
        <p class="m-page-header__name font-size-normal mb-0">
          中村 太郎（◯◯◯◯◯株式会社）
        </p>
      </v-flex>
      <v-flex class="d-flex justify-end" align-self-center="true">
        <Button
          class="m-page-header__button"
          text
          tile
          color="#666"
          height="60"
          >{{ $t('header.page.menu.answer') }}</Button
        >
        <Button
          class="m-page-header__button"
          text
          tile
          color="#666"
          height="60"
          >{{ $t('header.page.menu.projects') }}</Button
        >
        <Button
          class="m-page-header__button"
          text
          tile
          color="#666"
          height="60"
          >{{ $t('header.page.menu.results') }}</Button
        >
      </v-flex>
    </v-layout>
  </v-app-bar>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Button,
  },
})
</script>

<style lang="scss" scoped>
.m-page-header__title {
  font-size: 1.125rem;
  font-weight: bold;
}
.m-page-header__button {
  position: relative;
  &::before {
    display: none !important;
  }
  &:hover,
  &:focus {
    color: $c-primary-dark !important;
    font-weight: bold;
    &::after {
      background-color: $c-primary-dark;
    }
  }
  &::after {
    content: '';
    display: block !important;
    width: 100%;
    height: 6px;
    background-color: $c-white;
    position: absolute;
    left: 0;
    bottom: 0;
    transition-duration: 0.2s;
  }
}
</style>
