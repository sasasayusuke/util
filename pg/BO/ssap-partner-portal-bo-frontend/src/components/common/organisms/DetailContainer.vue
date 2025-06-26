<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-survey="isSurvey"
    :is-draft="isDraft"
    :is-valid="isValid"
    :is-invalid="isInvalid"
    :is-new="isNew"
    :is-hide-header-button="isHideHeaderButton"
    :is-hide-button1="isHideButton1"
    :is-hide-button2="isHideButton2"
    :is-hide-button3="isHideButton3"
    :is-hide-button4="isHideButton4"
    :is-disabled-button1="isDisabledButton1"
    :is-disabled-button2="isDisabledButton2"
    :is-disabled-button3="isDisabledButton3"
    :is-disabled-button4="isDisabledButton4"
    :is-hide-footer="isHideFooter"
    :note-head="noteHead"
    :is-loading-button="isLoadingButton"
    :is-disabled-edit-button="isDisabledEditButton"
    :hx="hx"
    :is-current-program="isCurrentProgram"
    @clickCheckbox="clickCheckbox"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
    @buttonAction3="buttonAction3"
    @buttonAction4="buttonAction4"
  >
    <template #label>
      <slot name="label" />
    </template>
    <template #term>
      <slot name="term" />
    </template>
    <template #date>
      <slot name="date" />
    </template>

    <template #showStatus>
      <slot name="showStatus" />
    </template>

    <template #button>
      <slot name="button" />
    </template>
    <template #uniqueButtons>
      <slot name="uniqueButtons" />
    </template>
    <template v-if="$scopedSlots.footerButton" #footerButton>
      <slot name="footerButton" />
    </template>
    <slot name="default" />
  </CommonContainer>
</template>

<script lang="ts">
import CommonContainer from './CommonContainer.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    CommonContainer,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
    isSurvey: {
      type: Boolean,
    },
    isDraft: {
      type: Boolean,
      default: false,
    },
    isValid: {
      type: Boolean,
      default: false,
    },
    isInvalid: {
      type: Boolean,
      default: false,
    },
    isNew: {
      type: Boolean,
      default: false,
    },
    isHideButton1: {
      type: Boolean,
      default: false,
    },
    isHideButton2: {
      type: Boolean,
      default: false,
    },
    isHideButton3: {
      type: Boolean,
      default: false,
    },
    isHideButton4: {
      type: Boolean,
      default: false,
    },
    isDisabledButton1: {
      type: Boolean,
      default: false,
    },
    isDisabledButton2: {
      type: Boolean,
      default: false,
    },
    isDisabledButton3: {
      type: Boolean,
      default: false,
    },
    isDisabledButton4: {
      type: Boolean,
      default: false,
    },
    isHideHeaderButton: {
      type: Boolean,
      default: false,
    },
    isHideFooter: {
      type: Boolean,
      default: false,
    },
    noteHead: {
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
    hx: {
      type: String,
      default: 'h2',
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      headButtons: [],
      footButtons: [],
    }
  },
  methods: {
    buttonAction1() {
      this.$emit('click:positive')
    },
    buttonAction2() {
      this.$emit('click:negative')
    },
    buttonAction3() {
      this.$emit('buttonAction3')
    },
    buttonAction4() {
      this.$emit('buttonAction4')
    },
    clickCheckbox(showCurrentProgram: boolean) {
      this.$emit('clickCheckbox', showCurrentProgram)
    },
  },
})
</script>
