<template>
  <Modal>
    <Paragraph style-set="modal-text1">{{
      $t('project.pages.detail.delete.text1')
    }}</Paragraph>
    <Paragraph style-set="modal-text2">
      {{ $t('project.pages.detail.delete.text2') }}
    </Paragraph>
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
        @click="$emit('delete')"
      >
        {{ $t('common.button.delete') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import Modal from '~/components/common/molecules/Modal.vue'
import { Button, Sheet, Paragraph } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Modal,
    Sheet,
    Button,
    Paragraph,
  },
  props: {},
  methods: {},
})
</script>
