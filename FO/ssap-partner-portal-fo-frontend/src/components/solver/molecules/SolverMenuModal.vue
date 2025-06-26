<template>
  <Modal v-if="isModalOpen">
    <template #head>{{ $t('solver.pages.menu.modal.title') }}</template>
    <p class="text-center mb-0" style="font-weight: bold; margin: 0 50px">
      {{ $t('solver.pages.menu.modal.description1') }}
      <br />
      {{ $t('solver.pages.menu.modal.description2') }}
    </p>
    <template #foot>
      <Button
        v-if="isButtonShow"
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
        width="230"
        :to="`/solver/corporation/${solverCorporationId}`"
      >
        {{ $t('solver.pages.menu.modal.button') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import Modal from '~/components/common/molecules/Modal.vue'
import { meStore, solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  name: 'SolverMenuModal',
  components: {
    Button,
    Modal,
  },
  props: {
    /** モーダルを開いているか否か */
    isModalOpen: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    // キャンセルボタン表示判定
    isButtonShow() {
      if (meStore.role === ENUM_USER_ROLE.SOLVER_STAFF) {
        return false
      } else {
        return true
      }
    },
    // 法人ソルバーID
    solverCorporationId() {
      return solverCorporationStore.id
    },
  },
})
</script>

<style lang="scss" scoped></style>
