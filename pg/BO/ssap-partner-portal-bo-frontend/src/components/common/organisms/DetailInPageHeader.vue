<template>
  <CommonInPageHeader>
    <slot name="default" />
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
          :loading="isLoadingButton"
          @click="buttonAction1"
        >
          {{ $t('common.button.save') }}
        </Button>
      </template>
    </template>
  </CommonInPageHeader>
</template>

<script lang="ts">
import { Button } from '../atoms/index'
import CommonInPageHeader from './CommonInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
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
    isValid: {
      type: Boolean,
    },
    isLoadingButton: {
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
