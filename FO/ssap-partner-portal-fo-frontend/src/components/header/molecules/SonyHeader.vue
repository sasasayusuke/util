<template>
  <Sheet class="m-header-container">
    <v-system-bar class="m-header-container__inner" color="#000">
      <Sheet width="100" height="18">
        <a href="https://www.sony.com/ja/" target="_blank">
          <Img
            :src="require('~/assets/img/logo.png')"
            width="100"
            height="18"
            alt="SONY"
          />
        </a>
      </Sheet>
    </v-system-bar>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Img } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Sheet,
    Img,
  },
})
</script>

<style lang="scss">
.v-application {
  .m-header-container {
    padding: 0 25px;
    &.transparent {
      background-color: $c-black !important;
    }
  }
  .m-header-container__inner {
    width: 100%;
    padding: 25px 0 !important;
    height: auto !important;
  }
}
.v-system-bar {
  &.theme--light {
    max-width: 1200px;
    margin: 0 auto;
  }
}
</style>
