<template>
  <Modal v-if="isModalOpen">
    <template #head>{{ $t('man-hour.modal.title') }}</template>
    <p class="text-center mb-0">
      {{
        $t('man-hour.modal.confirm1', {
          year: manHour.yearMonth.split('/')[0],
          month: manHour.yearMonth.split('/')[1].replace(/^0/, ''),
        })
      }}<br />
      {{ $t('man-hour.modal.confirm2') }}
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
        width="160"
        @click="$emit('update', true), $emit('closeModal')"
      >
        {{ $t('common.button.confirm_and_submit') }}
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
     * 支援工数データ
     */
    manHour: {
      type: Object,
    },
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
