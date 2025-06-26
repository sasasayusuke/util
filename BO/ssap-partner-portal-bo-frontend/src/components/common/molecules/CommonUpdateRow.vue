<template>
  <v-row class="o-customer-update-rows__unit">
    <v-col class="o-customer-update-rows__title"> 最終更新者： </v-col>
    <v-col class="o-customer-update-rows__data">
      <slot name="name">
        {{ name }}
      </slot>
    </v-col>
    <v-col class="o-customer-update-rows__title"> 最終更新日時： </v-col>
    <v-col class="o-customer-update-rows__data">
      <slot name="date">
        {{ date }}
      </slot>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {},
  props: {
    name: { type: String },
    date: { type: String },
  },
})
</script>

<style lang="scss" scoped>
.o-customer-update-rows__unit {
  justify-content: flex-end;
}
.o-customer-update-rows__title {
  font-size: 0.75rem;
  font-weight: bold;
  flex-grow: 0;
  white-space: nowrap;
}
.o-customer-update-rows__data {
  font-size: 0.75rem;
  flex-grow: 0;
  white-space: nowrap;
  padding-left: 0;
}
</style>
