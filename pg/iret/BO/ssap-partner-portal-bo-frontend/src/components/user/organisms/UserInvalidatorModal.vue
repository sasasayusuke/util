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
        @click="$emit('click:patchUser')"
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
  name: 'UserInvalidatorModal',
  components: {
    Modal,
    Sheet,
    Button,
  },
  props: {
    /** ユーザーか有効か */
    isUserInvalid: {
      type: Boolean,
      default: false,
    },
  },

  methods: {
    /** ユーザーが有効か判定し、valid/invalidを返す */
    validWord() {
      if (this.isUserInvalid) {
        return 'valid'
      } else {
        return 'invalid'
      }
    },
  },
})
</script>
