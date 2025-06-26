<template>
  <v-row justify="center">
    <Sheet
      class="d-flex justify-center py-6 mt-15"
      color="#E3E3E3"
      rounded
      width="1200"
    >
      <Button
        v-if="!isAdminInvalid"
        outlined
        style-set="large-tertiary"
        width="200"
        @click="$emit('click:switchValid')"
      >
        {{ $t('common.invalid.button') }}
      </Button>

      <Button
        v-if="isAdminInvalid"
        outlined
        style-set="large-tertiary"
        width="200"
        @click="$emit('click:switchValid')"
      >
        {{ $t('common.valid.button') }}
      </Button>
    </Sheet>
  </v-row>
</template>

<script lang="ts">
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'AdminInvalidation',
  components: {
    Sheet,
    Button,
  },
  props: {
    /**
     * 選択中のユーザーが無効化されているか否かを明示
     */
    isAdminInvalid: {
      type: Boolean,
      default: false,
    },
  },
})
</script>
