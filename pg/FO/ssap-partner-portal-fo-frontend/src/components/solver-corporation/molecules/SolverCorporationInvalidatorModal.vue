<template>
  <Modal>
    <p class="font-size-large font-weight-bold" align="center">
      {{ $t('solver-corporation.invalid.title1') }}<br />
      {{ $t('solver-corporation.invalid.title2') }}
    </p>
    {{ $t('solver-corporation.invalid.text') }}
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:closeModal')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-error"
        width="160"
        :loading="isLoading"
        @click="$emit('click:delete')"
      >
        {{ $t('common.button.delete') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import Modal from '~/components/common/molecules/Modal.vue'
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Modal,
    Sheet,
    Button,
  },
  props: {
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
