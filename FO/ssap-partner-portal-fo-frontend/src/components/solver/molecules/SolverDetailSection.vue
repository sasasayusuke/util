<template>
  <div>
    <v-container v-if="isNew" pa-8 fluid>
      <v-row no-gutters>
        <v-col>
          <Title hx="h3" class="type4 a-title--header">
            {{ title }}
          </Title>
        </v-col>
      </v-row>
      <v-row no-gutters>
        <v-col class="pt-5">
          <slot name="default" />
        </v-col>
      </v-row>
    </v-container>
    <v-container v-if="!isNew">
      <v-row>
        <v-col>
          <slot name="default" />
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
import { Title, Sheet, Chip } from '~/components/common/atoms'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Title,
    Sheet,
    Chip,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.master-karte-detail__chip {
  margin-left: 8px;
}
</style>
