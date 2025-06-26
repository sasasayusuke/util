<template>
  <Modal @click-bg="$emit('click:closeModal')">
    {{ $t('common.' + validWord() + '.text') }}
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
        @click="$emit('click:patch')"
      >
        {{ $t('common.button.' + validWord()) }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import Modal from '~/components/common/molecules/Modal.vue'
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'AdminInvalidationModal',
  components: {
    Modal,
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

  methods: {
    /**
     * 選択中のユーザーが無効化されているか否かを明示
     * @returns validかinvalidのString
     */
    validWord() {
      if (this.isAdminInvalid) {
        return 'valid'
      } else {
        return 'invalid'
      }
    },
  },
})
</script>
