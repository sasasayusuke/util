<template>
  <v-row no-gutters justify="center">
    <v-col cols="auto">
      <Title hx="h1" class="m-contact__title">{{
        $t('contact.pages.index.name')
      }}</Title>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Title } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: '',
  components: {
    Title,
  },
})
</script>
<style lang="scss" scoped>
.m-contact__title {
  font-size: 1.5rem;
}
</style>
