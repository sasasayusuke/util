<template>
  <Modal>
    <p class="font-size-large font-weight-bold" align="center">
      {{ $t('customer.disable.title1') }}<br />
      {{ $t('customer.disable.title2') }}
    </p>
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:closeModal')"
      >
        {{ $t('common.button.close') }}
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
  props: {},
  methods: {},
})
</script>
