<template>
  <v-container pa-0>
    <v-row>
      <v-col>
        <Button style-set="sort" @click="$emit('sort')">
          {{ $t('common.button.sort') }}
        </Button>
        <Button class="ml-4" style-set="text" small @click="$emit('clear')">
          {{ $t('common.button.reset') }}
        </Button>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {},
})
</script>
