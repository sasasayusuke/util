<template>
  <ContentHeaderContainer :level="level" :class="'type' + type">
    {{ title }}
    <template #summary>
      <span v-if="isEditing" class="ml-10 pt-1 m-heading__title__required"
        ><Required /><span class="m-heading__title__required__text">{{
          $t('common.label.required2')
        }}</span></span
      >
      <div v-else class="d-flex align-center a-manhour-samary-head pl-9">
        <Chip
          v-if="isConfirm"
          label
          small
          class="width-70 mr-2"
          color="secondary"
        >
          {{ $t('common.button.confirm') }}
        </Chip>
        <Chip
          v-else
          label
          small
          outlined
          class="width-70 mr-2"
          color="secondary"
        >
          {{ $t('common.button.noConfirm') }}
        </Chip>
        <div class="a-manhour-total--head">
          {{ $t('man-hour.group_info.total') }} :
          <span>{{ total }}h</span>
        </div>
      </div>
    </template>
    <template #button>
      <Button
        v-if="isEditing"
        outlined
        style-set="small-tertiary"
        class="mr-2"
        @click="buttonAction(0)"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        v-else
        outlined
        style-set="small-tertiary"
        class="mr-2"
        :disabled="isConfirm || !isInputManHour || isWithinMonthBeforeLast"
        @click="buttonAction(1)"
      >
        {{ $t('common.button.edit') }}
      </Button>
      <Button
        v-if="isEditing"
        style-set="small-primary"
        class="primary"
        :disabled="isValid !== true"
        @click="buttonAction(2)"
      >
        {{ $t('common.button.save2') }}
      </Button>
      <Button
        v-else
        style-set="small-primary"
        class="primary mr-2"
        :disabled="isConfirm || !isInputManHour || isWithinMonthBeforeLast"
        @click="buttonAction(3)"
      >
        {{ $t('common.button.confirm_and_submit') }}
      </Button>
      <!-- isInputManHourがtrue 工数調査が必要なユーザー 不要な場合のみtooltipを出現させる-->
      <ToolTips v-if="!isInputManHour">
        {{ $t('man-hour.tables.is_input_man_hour.tooltip') }}
      </ToolTips>
    </template>
  </ContentHeaderContainer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import ContentHeaderContainer from '~/components/man-hour/organisms/ContentHeaderContainer.vue'
import {
  Button,
  Chip,
  Required,
  ToolTips,
} from '~/components/common/atoms/index'
import { meStore } from '~/store'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    ContentHeaderContainer,
    Button,
    Chip,
    Required,
    ToolTips,
  },
  props: {
    /**
     * 編集中か否か
     */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /**
     * タイトル
     */
    title: {
      type: String,
      default: '',
    },
    /**
     * フォント大きさレベル
     */
    level: {
      type: Number,
      default: 1,
    },
    /**
     * cssクラス名の文字列構成要素
     */
    type: {
      type: Number,
      default: 3,
    },
    /**
     * 確認済みか否か
     */
    isConfirm: {
      type: Boolean,
      default: false,
    },
    /**
     * 工数合計
     */
    total: {
      type: Number,
      default: 0,
    },
    /**
     * 基準の月よりも前か否か
     */
    isWithinMonthBeforeLast: {
      type: Boolean,
      default: false,
    },
    /**
     * データが有効か否か
     */
    isValid: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isInputManHour: meStore.isInputManHour,
    }
  },
  methods: {
    /**
     * ボタン押下時の挙動
     * @param ボタンのアクションタイプ
     */
    buttonAction(actionType: number = 0) {
      this.$emit('buttonAction', actionType)
    },
  },
})
</script>

<style lang="scss" scoped>
.a-manhour-total--head {
  font-weight: bold;
  font-size: 1rem;
  span {
    font-size: 1.15rem;
    color: $c-primary-dark;
  }
}
.m-heading__button {
  .a-button.v-btn {
    min-width: 96px;
  }
}
.m-heading__title__required {
  @include fontSize('xsmall');
  font-weight: normal;
}
.m-heading__title__required__text {
  color: $c-black-60;
}
</style>
