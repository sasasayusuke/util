<template>
  <v-container pa-8 fluid>
    <v-row no-gutters>
      <v-col>
        <Title hx="h3" class="type4">{{ title }}</Title>
      </v-col>
    </v-row>
    <v-row no-gutters>
      <v-col class="pt-5">
        <slot name="default" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Title, Sheet } from '~/components/common/atoms'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Title,
    Sheet,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
  },
  data() {
    return {}
  },
  computed: {},
  methods: {},
})
</script>

<style lang="scss" scoped></style>
