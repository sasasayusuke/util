<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-karte="isKarte"
    :is-project="isProject"
    :is-solver-application="isSolverApplication"
    :is-valid="isValid"
    :is-invalid="isInvalid"
    :is-attendees-valid="isAttendeesValid"
    :is-updating="isUpdating"
    :note-head="noteHead"
    :hx="hx"
    :is-hide-required="isHideRequired"
    :is-hide-button1="isHideButton1"
    :is-hide-button2="isHideButton2"
    :is-hide-button3="isHideButton3"
    :is-disabled-button1="isDisabledButton1"
    :is-disabled-button2="isDisabledButton2"
    :is-disabled-button3="isDisabledButton3"
    :is-loading-button="isLoadingButton"
    :is-loading-button2="isLoadingButton2"
    :is-solver="isSolver"
    :is-solver-detail="isSolverDetail"
    :is-solver-corporation="isSolverCorporation"
    :is-detail="isDetail"
    :is-hide-footer="isHideFooter"
    :is-temporary-save="isTemporarySave"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
    @buttonAction3="buttonAction3"
    @buttonAction4="buttonAction4"
    @buttonAction5="buttonAction5"
    @buttonAction6="buttonAction6"
    @temporarySaveAction="temporarySaveAction"
  >
    <template #label>
      <slot name="label" />
    </template>
    <template #term>
      <slot name="term" />
    </template>
    <template #button>
      <slot name="button" />
    </template>
    <template #footerButton>
      <slot name="footerButton" />
    </template>
    <slot />
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
    isKarte: {
      type: Boolean,
    },
    isProject: {
      type: Boolean,
      default: false,
    },
    isSolverApplication: {
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
    isAttendeesValid: {
      type: Boolean,
      default: true,
    },
    isUpdating: {
      type: Boolean,
      default: false,
    },
    noteHead: {
      type: String,
      default: '',
    },
    hx: {
      type: String,
    },
    isHideRequired: {
      type: Boolean,
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
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    isLoadingButton2: {
      type: Boolean,
      default: false,
    },
    isSolver: {
      type: Boolean,
      default: false,
    },
    isSolverDetail: {
      type: Boolean,
      default: false,
    },
    isSolverCorporation: {
      type: Boolean,
      default: false,
    },
    isDetail: {
      type: Boolean,
      default: false,
    },
    isHideFooter: {
      type: Boolean,
      default: false,
    },
    isTemporarySave: {
      type: Boolean,
      default: false,
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
    buttonAction5() {
      this.$emit('buttonAction5')
    },
    buttonAction6() {
      this.$emit('buttonAction6')
    },
    temporarySaveAction() {
      this.$emit('temporarySaveAction')
    },
  },
})
</script>
