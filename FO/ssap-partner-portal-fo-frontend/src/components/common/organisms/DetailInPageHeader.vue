<template>
  <CommonInPageHeader>
    <slot name="default" />
    <span
      v-if="noteHead === 'required'"
      class="ml-10 m-heading__title__required"
      ><Required /><span class="m-heading__title__required__text">{{
        $t('common.label.required2')
      }}</span></span
    >
    <!-- backToListが与えられていれば、一覧へ戻るボタンを作成する -->
    <template #button>
      <Button
        v-if="backToList"
        style-set="small-tertiary"
        outlined
        width="96"
        :to="backToList"
      >
        {{ $t('common.button.backToList') }}
      </Button>
      <Button
        v-else-if="backToDetail"
        style-set="small-tertiary"
        outlined
        width="96"
        :to="backToDetail"
      >
        {{ $t('common.button.back') }}
      </Button>
      <template v-else-if="headerButtons">
        <Button style-set="detailHeaderNegative" @click="buttonAction2">
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="detailHeaderPositive"
          :disabled="isValid !== true"
          @click="buttonAction1"
        >
          {{ $t('common.button.save') }}
        </Button>
      </template>
      <template v-else-if="isSolver && isEditing">
        <Button style-set="detailHeaderNegative" class="mr-2" :to="backUrl">
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="detailHeaderPositive"
          width="120"
          :disabled="isDisabled"
          @click="buttonAction1"
        >
          {{ $t('common.button.toConfirm') }}
        </Button>
      </template>
      <template v-else-if="isSolver && !isEditing">
        <Button
          style-set="detailHeaderNegative"
          class="mr-2"
          width="120"
          @click="buttonAction2"
        >
          {{ $t('common.button.backToInput') }}
        </Button>
        <Button
          :disabled="isDisabled"
          :loading="isLoading"
          style-set="detailHeaderPositive"
          @click="buttonAction3"
        >
          {{ $t('common.button.apply') }}
        </Button>
      </template>
    </template>
  </CommonInPageHeader>
</template>

<script lang="ts">
import { Button, Required } from '../atoms/index'
import CommonInPageHeader from './CommonInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Required,
    CommonInPageHeader,
  },
  props: {
    backToList: {
      type: [String, Object],
      default: '',
    },
    backToDetail: {
      type: [String, Object],
      default: '',
    },
    headerButtons: {
      type: Boolean,
    },
    noteHead: {
      type: String,
      default: '',
    },
    isValid: {
      type: Boolean,
    },
    isSolver: {
      type: Boolean,
    },
    isDisabled: {
      type: Boolean,
      default: true,
    },
    backUrl: {
      type: String,
      default: '',
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
    isEditing: {
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
  },
})
</script>

<style lang="scss" scoped>
.m-heading__title__required {
  @include fontSize('xsmall');
  font-weight: normal;
  vertical-align: middle;
}
.m-heading__title__required__text {
  color: $c-black-60;
}
</style>
