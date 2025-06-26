<template>
  <v-footer class="o-footer" height="137">
    <v-layout>
      <p class="o-footer__logo">
        <img
          src="@/assets/img/logo-sap-footer.png"
          alt="Sony Acceleration Platform"
          width="77"
          height="77"
        />
      </p>
      <v-layout class="d-flex align-self-center pl-3">
        <Button
          class="px-3 text-caption font-weight-bold"
          text
          tile
          color="#000"
          href="/guide/index.html"
          target="_blank"
          >ご利用ガイド</Button
        >
      </v-layout>
      <v-layout
        class="d-flex align-self-end justify-end pl-3 text-caption"
        color="#666"
      >
        <p class="o-footer__copryright">
          Copyright 2022 Sony Group Corporation
        </p>
      </v-layout>
    </v-layout>
    <!-- <v-row justify="center" no-gutters>
      <v-col class="primary lighten-2 py-4 text-center white--text" cols="12">
        {{ getCurrentDate().getFullYear() }} — <strong>Vuetify</strong>
      </v-col>
    </v-row> -->
  </v-footer>
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
.o-footer {
  padding: 0 25px;
  .layout {
    max-width: 1200px;
    margin: 0 auto;
  }
}
.o-footer__logo {
  line-height: 0;
  margin: 0;
}
.o-footer__copryright {
  margin: 0;
  color: #666;
}
</style>

<style lang="scss">
.o-footer {
  .a-button {
    font-weight: normal !important;
    &:hover,
    &:focus {
      font-weight: bold !important;
    }
  }
}
</style>
