<template>
  <div>
    <ModalEdit v-if="isModalEdit && isOpen" :title="title" :not-required="true">
      <Paragraph class="mb-0" style-set="modal-text1">
        {{ text }}
      </Paragraph>
      <template #foot>
        <v-container>
          <v-row justify="center">
            <v-col cols="auto">
              <Button
                outlined
                style-set="large-tertiary"
                width="160"
                @click="$emit('click:closeDelete')"
              >
                {{ $t('common.button.cancel') }}
              </Button>
            </v-col>
            <v-col cols="auto">
              <Button
                style-set="large-error"
                width="160"
                @click="$emit('click:deleteSolver')"
              >
                {{ $t('common.button.delete') }}
              </Button>
            </v-col>
          </v-row>
        </v-container>
      </template>
    </ModalEdit>
    <Modal v-if="!isModalEdit && isOpen">
      <Paragraph class="mb-0" style-set="modal-text1">
        {{ text }}
        {{ text2 }}
      </Paragraph>
      <Paragraph v-if="text3" class="mb-0" style-set="modal-text2">
        {{ text3 }}
      </Paragraph>
      <template #foot>
        <v-container>
          <v-row justify="center">
            <v-col cols="auto">
              <Button
                outlined
                style-set="large-tertiary"
                width="160"
                @click="$emit('click:closeDelete')"
              >
                {{ $t('common.button.cancel') }}
              </Button>
            </v-col>
            <v-col cols="auto">
              <Button
                style-set="large-error"
                width="160"
                @click="$emit('click:deleteSolver')"
              >
                {{ $t('common.button.delete') }}
              </Button>
            </v-col>
          </v-row>
        </v-container>
      </template>
    </Modal>
  </div>
</template>

<script lang="ts">
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import { Button, Paragraph } from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import BaseComponent from '~/common/BaseComponent'
import Modal from '~/components/common/molecules/Modal.vue'

export default BaseComponent.extend({
  name: 'SolverDeleteModal',
  components: {
    Modal,
    ModalEdit,
    CommonDetailRow,
    Button,
    Paragraph,
  },
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    text: {
      type: String,
      required: true,
    },
    text2: {
      type: String,
      default: '',
    },
    text3: {
      type: String,
      default: '',
    },
    isModalEdit: {
      type: Boolean,
      default: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.radio-text {
  font-size: 0.875rem !important;
  color: $c-black;
  display: flex;
  align-items: center;
}
.m-survery-modal__unit {
  border-bottom: 1px solid $c-gray-line;
  color: $c-black;
  &:nth-child(n + 2) {
    margin-top: 20px;
  }
}
</style>
