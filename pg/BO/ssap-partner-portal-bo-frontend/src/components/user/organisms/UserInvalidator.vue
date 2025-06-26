<template>
  <v-row justify="center">
    <Sheet
      class="d-flex justify-center py-6 mt-15"
      color="#E3E3E3"
      rounded
      width="1200"
    >
      <Button
        v-if="!isUserInvalid"
        outlined
        style-set="large-tertiary"
        width="200"
        @click="$emit('click:switchValid')"
      >
        {{ $t('common.invalid.button') }}
      </Button>

      <Button
        v-if="isUserInvalid"
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
  name: 'UserInvalidator',
  components: {
    Sheet,
    Button,
  },
  props: {
    /** ユーザーが有効か */
    isUserInvalid: {
      type: Boolean,
      default: false,
    },
  },
})
</script>
