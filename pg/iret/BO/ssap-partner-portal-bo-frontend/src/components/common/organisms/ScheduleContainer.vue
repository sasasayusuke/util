<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-schecule="isSchecule"
    :is-hide-button1="isAllEdit === false"
    :is-hide-button2="isHideButton2"
    :is-hide-button3="isAllCreate === false"
    is-hide-footer
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
    @buttonAction3="buttonAction3"
  >
    <slot />
    <template #uniqueButtons>
      <Button
        v-if="!isHideButton2"
        style-set="detailHeaderPositive"
        @click="buttonAction2"
      >
        {{ $t('common.button.add') }}
      </Button>
      <!-- 一括変更 -->
      <Button
        v-if="isAllEdit"
        style-set="detailHeaderPositive"
        class="ml-2"
        @click="buttonAction1"
      >
        {{ $t('common.button.editAll') }}
      </Button>
      <!-- 一括追加 -->
      <Button
        v-else-if="isAllCreate"
        style-set="detailHeaderPositive"
        class="ml-2"
        @click="buttonAction3"
      >
        {{ $t('common.button.createAll') }}
      </Button>
    </template>
  </CommonContainer>
</template>

<script lang="ts">
import CommonContainer from './CommonContainer.vue'
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Button,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    isEditing: {
      type: Boolean,
      default: false,
    },
    isAllEdit: {
      type: Boolean,
      default: false,
    },
    isAllCreate: {
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
  },
  data() {
    return {
      isSchecule: true,
    }
  },
  methods: {
    buttonAction1() {
      this.$emit('click:bulkEdit')
    },
    buttonAction2() {
      this.$emit('click:add')
    },
    buttonAction3() {
      this.$emit('click:bulkCreate')
    },
  },
})
</script>
