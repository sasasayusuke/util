<template>
  <v-container fluid pa-0 mt-8>
    <v-row no-gutters align="center">
      <v-col cols="auto">
        <h2 class="font-size-large">{{ $t('karte.pages.detail.admin') }}</h2>
      </v-col>
      <v-col cols="auto">
        <p class="font-size-xsmall mb-0 pl-3">
          {{ $t('karte.pages.detail.adminSub') }}
        </p>
      </v-col>
    </v-row>
    <v-row no-gutters>
      <v-col>
        <Sheet class="mt-3" style-set="text-box">
          <slot name="default" />
        </Sheet>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'KarteAdminContainer',
  components: {
    Sheet,
  },
})
</script>
