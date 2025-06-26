<template>
  <Sheet elevation="3" rounded width="1200" color="white">
    <!-- TODO: このコンポーネントはコンテナ型の共通コンポーネントなので、詳細/新規画面専用の記述は別コンポーネントを作る -->
    <InDetailHeader
      v-if="!isManHour"
      :is-editing="isEditing"
      :is-karte="isKarte"
      :is-project="isProject"
      :is-solver-application="isSolverApplication"
      :is-valid="isValid"
      :is-invalid="isInvalid"
      :is-attendees-valid="isAttendeesValid"
      :is-hide-button1="isHideButton1"
      :is-hide-button2="isHideButton2"
      :is-hide-button3="isHideButton3"
      :is-disabled-button1="isDisabledButton1"
      :is-disabled-button2="isDisabledButton2"
      :is-disabled-button3="isDisabledButton3"
      :is-updating="isUpdating"
      :is-solver="isSolver"
      :is-solver-detail="isSolverDetail"
      :is-solver-corporation="isSolverCorporation"
      :is-detail="isDetail"
      :hx="hx"
      :is-loading-button="isLoadingButton"
      :is-loading-button2="isLoadingButton2"
      :is-temporary-save="isTemporarySave"
      @buttonAction1="buttonAction1"
      @buttonAction2="buttonAction2"
      @buttonAction3="buttonAction3"
      @buttonAction4="buttonAction4"
      @buttonAction5="buttonAction5"
      @buttonAction6="buttonAction6"
      @temporarySaveAction="temporarySaveAction"
    >
      <slot name="label" />
      <template #term>
        <slot name="term" />
      </template>
      {{ title }}
      <span
        v-if="(isEditing && !isHideRequired) || noteHead === 'required'"
        class="ml-10 m-heading__title__required"
        ><Required /><span class="m-heading__title__required__text">{{
          $t('common.label.required2')
        }}</span></span
      >
      <template #button>
        <slot name="button" />
      </template>
    </InDetailHeader>
    <slot name="default" />
    <InDetailFooter
      v-if="!isHideFooter"
      :is-editing="isEditing"
      :is-karte="isKarte"
      :is-man-hour="isManHour"
      :is-project="isProject"
      :is-solver-corporation="isSolverCorporation"
      :is-valid="isValid"
      :is-invalid="isInvalid"
      :is-hide-button1="isHideButton1"
      :is-hide-button2="isHideButton2"
      :is-disabled-button1="isDisabledButton1"
      :is-disabled-button2="isDisabledButton2"
      :is-loading-button="isLoadingButton"
      @buttonAction1="buttonAction1"
      @buttonAction2="buttonAction2"
      @buttonAction3="buttonAction3"
    >
      <template #footerButton>
        <slot name="footerButton" />
      </template>
    </InDetailFooter>
  </Sheet>
</template>

<script lang="ts">
// import HeadButtons from '../molecules/CommonContainerHeadButtons.vue'
// import FootButtons from '../molecules/CommonContainerFootButtons.vue'
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Button, Required } from '~/components/common/atoms/index'
import InDetailHeader from '~/components/common/organisms/InDetailHeader.vue'
import InDetailFooter from '~/components/common/organisms/InDetailFooter.vue'

export default BaseComponent.extend({
  components: {
    // HeadButtons,
    // FootButtons,
    Sheet,
    Button,
    InDetailHeader,
    InDetailFooter,
    Required,
  },
  props: {
    title: {
      type: String,
      // required: true,
      default: '',
    },
    isEditing: {
      type: Boolean,
      // required: true,
      default: false,
    },
    isUpdating: {
      type: Boolean,
      default: false,
    },
    headButtons: {
      type: Array,
      default() {
        return []
      },
    },
    footButtons: {
      type: Array,
      default() {
        return []
      },
    },
    isKarte: {
      type: Boolean,
    },
    isManHour: {
      type: Boolean,
      default: false,
    },
    isProject: {
      type: Boolean,
      default: false,
    },
    isSolverApplication: {
      type: Boolean,
      default: false,
    },
    isInvalid: {
      type: Boolean,
      default: false,
    },
    isValid: {
      type: Boolean,
      default: false,
    },
    isAttendeesValid: {
      type: Boolean,
      default: true,
    },
    isHideFooter: {
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
    isHideRequired: {
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
    isTemporarySave: {
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

<style lang="scss" scoped>
.m-heading__title__required {
  @include fontSize('xsmall');
  font-weight: normal;
}
.m-heading__title__required__text {
  color: $c-black-60;
}
</style>
