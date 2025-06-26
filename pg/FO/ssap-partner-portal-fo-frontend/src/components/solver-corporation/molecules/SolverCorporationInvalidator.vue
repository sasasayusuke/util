<template>
  <v-row justify="center">
    <Sheet
      class="d-flex justify-center py-6 mt-15"
      color="#E3E3E3"
      rounded
      width="1200"
    >
      <Button
        outlined
        style-set="large-tertiary"
        width="200"
        @click="$emit('click:switchValid')"
      >
        {{ $t('solver-corporation.invalid.button') }}
      </Button>
    </Sheet>
  </v-row>
</template>

<script lang="ts">
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Sheet,
    Button,
  },
  props: {},
  methods: {},
})
</script>
