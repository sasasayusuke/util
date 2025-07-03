<template>
  <Modal v-if="isModalOpen">
    <template #head>{{
      $t('solver.pages.candidateList.modal.title')
    }}</template>
    <p class="text-center mb-0">
      {{ $t('solver.pages.candidateList.modal.text') }}<br />
      {{ $t('solver.pages.candidateList.modal.text2') }}
    </p>
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('closeModal')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-primary"
        width="180"
        @click="$emit('update'), $emit('closeModal')"
      >
        {{ $t('solver.pages.candidateList.modal.buttons.save') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import Modal from '~/components/common/molecules/Modal.vue'

export default BaseComponent.extend({
  components: {
    Button,
    Modal,
  },
  props: {
    /**
     * モーダルを開いているか否か
     */
    isModalOpen: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss" scoped></style>
