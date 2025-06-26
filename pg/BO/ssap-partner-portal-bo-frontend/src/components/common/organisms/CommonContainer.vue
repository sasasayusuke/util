<template>
  <Sheet elevation="3" rounded width="1200" color="white">
    <!-- TODO: このコンポーネントはコンテナ型の共通コンポーネントなので、詳細/新規画面専用の記述は別コンポーネントを作る -->
    <InDetailHeader
      :is-editing="isEditing"
      :is-register="isRegister"
      :is-schecule="isSchecule"
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
      :is-loading-button="isLoadingButton"
      :is-disabled-edit-button="isDisabledEditButton"
      :hx="hx"
      :show-current-program="showCurrentProgram"
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
      {{ title }}
      <span
        v-if="noteHead !== 'false' && (isEditing || noteHead === 'required')"
        class="ml-10 m-heading__title__required"
        ><Required /><span class="m-heading__title__required__text">{{
          $t('common.label.required2')
        }}</span></span
      >
      <template #showStatus>
        <slot name="showStatus" />
      </template>
      <template #button>
        <slot name="button" />
      </template>
      <template #uniqueButtons>
        <slot name="uniqueButtons" />
      </template>
    </InDetailHeader>
    <slot name="default" />
    <InDetailFooter
      v-if="!isHideFooter || isEditing"
      :is-editing="isEditing"
      :is-register="isRegister"
      :is-schecule="isSchecule"
      :is-draft="isDraft"
      :is-valid="isValid"
      :is-invalid="isInvalid"
      :is-hide-button1="isHideButton1"
      :is-hide-button2="isHideButton2"
      :is-disabled-button1="isDisabledButton1"
      :is-disabled-button2="isDisabledButton2"
      :button-text2="buttonText2"
      :is-loading-button="isLoadingButton"
      :is-disabled-edit-button="isDisabledEditButton"
      @buttonAction1="buttonAction1"
      @buttonAction2="buttonAction2"
      @buttonAction3="buttonAction3"
      @buttonAction4="buttonAction4"
    >
      <template v-if="$scopedSlots.footerButton" #footerButton>
        <slot name="footerButton" />
      </template>
    </InDetailFooter>
  </Sheet>
</template>

<script lang="ts">
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
    Required,
    InDetailHeader,
    InDetailFooter,
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
    isRegister: {
      type: Boolean,
    },
    isSchecule: {
      type: Boolean,
    },
    isSurvey: {
      type: Boolean,
    },
    isDraft: {
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
    buttonText2: {
      type: String,
      default: '',
    },
    isHideHeaderButton: {
      type: Boolean,
      default: false,
    },
    isHideFooter: {
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
    clickCheckbox(showCurrentProgram: boolean) {
      this.$emit('clickCheckbox', showCurrentProgram)
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
