<template>
  <Sheet
    class="m-heading d-flex justify-space-between align-center px-8 pt-1"
    height="60"
  >
    <compnent :is="hx" class="m-heading__title">
      <template v-if="$scopedSlots.label">
        <slot name="label" />
      </template>
      <slot name="default" />
      <span class="m-heading__title__term"><slot name="term" /></span>
    </compnent>
    <div class="m-heading__ui d-flex align-center">
      <v-checkbox
        v-show="!isCurrentProgram"
        v-model="showCurrentProgram"
        style="font-size: 14px"
        @click="clickCheckbox"
      >
        <template #label>
          <span id="checkboxLabel">{{
            $t('master-karte.pages.detail.container.showCurrentProgram')
          }}</span>
        </template>
      </v-checkbox>
      <p v-if="$scopedSlots.date" class="m-heading__date font-size-xsmall">
        <slot name="date" />
      </p>
      <div
        v-if="!isHideHeaderButton && !$slots.uniqueButtons"
        class="m-heading__button ml-8"
      >
        <template v-if="isSchecule">
          <Button
            v-if="!isHideButton2"
            style-set="detailHeaderPositive"
            @click="buttonAction2"
          >
            {{ $t('common.button.add') }}
          </Button>
          <Button
            v-if="!isHideButton1"
            style-set="detailHeaderPositive"
            class="ml-2"
            @click="buttonAction1"
          >
            {{ $t('common.button.editAll') }}
          </Button>
          <Button
            v-if="!isHideButton3"
            style-set="detailHeaderPositive"
            class="ml-2"
            @click="buttonAction3"
          >
            {{ $t('common.button.createAll') }}
          </Button>
        </template>
        <template v-else-if="isSurvey">
          <Button
            v-if="isEditing"
            style-set="detailHeaderNegative"
            @click="buttonAction2"
          >
            {{ $t('common.button.cancel') }}
          </Button>
          <Button
            v-if="!isHideButton1"
            style-set="detailHeaderPositive"
            class="ml-2"
            :disabled="isValid !== true"
            :loading="isLoadingButton"
            @click="buttonAction1"
          >
            {{
              isEditing ? $t('common.button.save') : $t('common.button.edit')
            }}
          </Button>
        </template>
        <template v-else-if="isDraft">
          <Button
            v-if="!isInvalid && !isNew"
            style-set="detailHeaderNegative"
            @click="buttonAction2"
          >
            {{
              isEditing
                ? $t('common.button.cancel')
                : $t('common.button.invalid')
            }}
          </Button>
          <Button
            v-if="!isInvalid && isNew"
            :style-set="
              isEditing ? 'detailHeaderNegative' : 'detailHeaderNegativeNew'
            "
            @click="buttonAction2"
          >
            {{
              isEditing
                ? $t('common.button.cancel')
                : $t('common.button.delete2')
            }}
          </Button>
          <Button
            v-if="!isInvalid"
            :disabled="isEditing === true && isValid !== true"
            :loading="isLoadingButton"
            style-set="detailHeaderPositive"
            class="ml-2"
            @click="buttonAction1"
          >
            {{
              isEditing
                ? $t('common.button.save')
                : $t('common.button.updating')
            }}
          </Button>
          <template v-if="!isEditing">
            <Button
              v-if="!isInvalid"
              style-set="count"
              class="ml-2"
              @click="buttonAction3"
            >
              <Icon size="27">mdi-chevron-up</Icon>
            </Button>
            <Button
              v-if="!isInvalid"
              style-set="count"
              class="ml-2"
              @click="buttonAction4"
            >
              <Icon size="27">mdi-chevron-down</Icon>
            </Button>
          </template>
          <Button
            v-if="isInvalid"
            style-set="detailHeaderPositive"
            outlined
            :disabled="isDisabledButton3"
            @click="buttonAction3"
          >
            {{ $t('common.button.valid') }}
          </Button>
        </template>
        <template v-else>
          <Button
            v-if="!isHideButton2"
            style-set="detailHeaderNegative"
            @click="buttonAction2"
          >
            {{
              isEditing || isRegister
                ? $t('common.button.cancel')
                : $t('common.button.backToList')
            }}
          </Button>
          <Button
            v-if="!isHideButton1"
            style-set="detailHeaderPositive"
            class="ml-2"
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
        </template>
      </div>
      <div v-if="$slots.showStatus">
        <slot name="showStatus" />
      </div>
      <div v-if="$slots.uniqueButtons" class="m-heading__button ml-8">
        <slot name="uniqueButtons" />
      </div>
    </div>
  </Sheet>
</template>

<script lang="ts">
import { Button, Card, Sheet, Icon } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Card,
    Sheet,
    Icon,
  },
  props: {
    hx: {
      type: String,
      default: 'h2',
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
    isRegister: {
      type: Boolean,
      default: false,
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
    isHideHeaderButton: {
      type: Boolean,
    },
    isHideButton1: {
      type: Boolean,
    },
    isHideButton2: {
      type: Boolean,
    },
    isHideButton3: {
      type: Boolean,
    },
    isHideButton4: {
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
    isDisabledButton4: {
      type: Boolean,
      default: false,
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
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
    buttonAction3() {
      this.$emit('buttonAction3')
    },
    buttonAction4() {
      this.$emit('buttonAction4')
    },
    clickCheckbox() {
      this.showCurrentProgram
        ? this.$emit('clickCheckbox', true)
        : this.$emit('clickCheckbox', false)
    },
  },
})
</script>

<style lang="scss" scoped>
.m-heading {
  border-bottom: 4px solid $c-primary-dark !important;
}
.m-heading__title {
  @include fontSize('xlarge');
  font-weight: bold;
}
.m-heading__title__term {
  @include fontSize('normal');
}
.m-heading__date {
  margin: 0;
  color: $c-black-60;
}
.m-heading__button {
  display: flex;
  align-items: center;
}
#checkboxLabel {
  color: black;
  font-size: 14px;
}
</style>
