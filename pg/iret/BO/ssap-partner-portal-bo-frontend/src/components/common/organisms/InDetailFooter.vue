<template>
  <v-container v-if="$scopedSlots.footerButton" py-10>
    <v-row class="pa-0 ma-0" justify="center">
      <v-col cols="auto">
        <slot name="footerButton" />
      </v-col>
    </v-row>
  </v-container>
  <v-container v-else py-10>
    <v-row v-if="isDraft && isEditing" class="pa-0 ma-0" justify="center">
      <Button
        v-if="!isHideButton2"
        outlined
        style-set="large-tertiary"
        width="160"
        @click="buttonAction2"
      >
        {{
          isEditing
            ? $t('common.button.cancel')
            : $t('common.button.backToList')
        }}
      </Button>
      <Button
        v-if="!isHideButton1"
        class="ml-4"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true"
        :loading="isLoadingButton"
        @click="buttonAction1"
      >
        {{
          isEditing
            ? $t('common.button.save')
            : isRegister
            ? $t('common.button.register')
            : $t('common.button.edit')
        }}
      </Button>
    </v-row>
    <v-row v-else class="pa-0 ma-0" justify="center">
      <Button
        v-if="!isHideButton2"
        outlined
        style-set="large-tertiary"
        width="160"
        @click="buttonAction2"
      >
        {{
          isEditing || isRegister
            ? $t('common.button.cancel')
            : buttonText2 || $t('common.button.backToList')
        }}
      </Button>
      <Button
        v-if="!isHideButton1"
        class="ml-4"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true || isDisabledEditButton"
        :loading="isLoadingButton"
        @click="buttonAction1"
      >
        {{
          isEditing
            ? $t('common.button.save')
            : isRegister
            ? $t('common.button.register')
            : $t('common.button.edit')
        }}
      </Button>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {
    isEditing: {
      type: Boolean,
      required: true,
    },
    isRegister: {
      type: Boolean,
      default: false,
    },
    isDraft: {
      type: Boolean,
      default: false,
    },
    isValid: {
      type: Boolean,
      default: false,
    },
    isHideButton1: {
      type: Boolean,
    },
    isHideButton2: {
      type: Boolean,
    },
    isDisabledButton1: {
      type: Boolean,
      default: false,
    },
    isDisabledButton2: {
      type: Boolean,
      default: false,
    },
    buttonText2: {
      type: String,
      default: '',
    },
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    isDisabledEditButton: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    buttonAction1() {
      this.$emit('buttonAction1')
    },
    buttonAction2() {
      this.$emit('buttonAction2')
    },
  },
})
</script>
