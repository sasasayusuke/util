<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.tools') }}
        </h2>
      </v-flex>
    </v-container>
    <v-container class="pa-0">
      <div class="d-flex">
        <Button
          style-set="home-tools-1"
          :href="startDashSiteUrl"
          target="_blank"
        >
          <img
            src="~/assets/img/banner-startdash-text.png"
            alt="StartDash"
            width="124"
            height="21"
          />
        </Button>
        <!-- 初回リリースでは下記ボタンを表示しないため、コメントアウト -->
        <!-- <Button
          style-set="home-tools-2"
          :href="startDashOfficeSiteUrl"
          class="ml-5"
          target="_blank"
        >
          <img
            src="~/assets/img/banner-startdash-office-text.png"
            alt="StartDash Office"
            width="182"
            height="19"
          />
        </Button> -->
      </div>
    </v-container>
  </section>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Img, Button } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Img,
    Button,
  },
  data() {
    return {
      startDashSiteUrl: process.env.STARTDASH_SITE_URL,
      startDashOfficeSiteUrl: process.env.STARTDASH_OFFICE_SITE_URL,
    }
  },
})
</script>
