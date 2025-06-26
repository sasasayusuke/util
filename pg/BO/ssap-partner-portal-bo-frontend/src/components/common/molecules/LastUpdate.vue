<template>
  <v-container pa-0 fluid>
    <v-row class="m-last-update" justify="end">
      <v-col v-if="showTerm">
        <v-row>
          <v-col class="m-last-update__term">
            {{ $t('common.lastUpdate.term') }}
          </v-col>
          <v-col class="m-last-update__data">{{ term }}</v-col>
        </v-row>
      </v-col>
      <v-col class="m-last-update__title">
        {{ $t('common.lastUpdate.user') }}
      </v-col>
      <v-col class="m-last-update__data">
        {{ date && user ? user : 'ー' }}
      </v-col>
      <v-col class="m-last-update__title">
        {{ $t('common.lastUpdate.date') }}
      </v-col>
      <v-col class="m-last-update__data">
        {{ date && user ? formatDate(new Date(date)) : 'ー' }}
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    Button,
    Sheet,
  },
  props: {
    term: {
      type: String,
    },
    showTerm: {
      type: Boolean,
      default: true,
    },
    user: {
      type: String,
      required: true,
    },
    date: {
      type: String,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-last-update__unit {
  justify-content: flex-end;
}
.m-last-update__term {
  font-size: 0.75rem;
  font-weight: bold;
  flex-grow: 0;
  white-space: nowrap;
}
.m-last-update__title {
  font-size: 0.75rem;
  font-weight: bold;
  flex-grow: 0;
  white-space: nowrap;
}
.m-last-update__data {
  font-size: 0.75rem;
  flex-grow: 0;
  white-space: nowrap;
  padding-left: 0;
}
</style>
