<template>
  <v-row justify="center" class="px-8 py-0 ma-0">
    <Sheet class="pa-6" color="#EBF7ED" rounded width="100%">
      <p class="text-center a-manhour-total--foot pb-0 mb-0">
        {{ title }} : <span>{{ total }}h</span>
      </p>
      <v-row
        v-if="!isEditing"
        class="pt-6 py-0 pb-0 ma-0"
        justify="center"
        no-gutters
      >
        <v-col cols="auto" class="d-flex justify-content-center align-center">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            :disabled="isConfirm || !isInputManHour || isWithinMonthBeforeLast"
            @click="buttonAction(1)"
          >
            {{ $t('common.button.edit') }}
          </Button>
          <Button
            class="ml-4 mr-2"
            style-set="large-primary"
            width="160"
            :disabled="isConfirm || !isInputManHour || isWithinMonthBeforeLast"
            @click="buttonAction(3)"
          >
            {{ $t('common.button.confirm_and_submit') }}
          </Button>
          <!-- isInputManHourがtrue 工数調査が必要なユーザー 不要な場合のみtooltipを出現させる-->
          <ToolTips v-if="!isInputManHour">
            {{ $t('man-hour.tables.is_input_man_hour.tooltip') }}
          </ToolTips>
        </v-col>
      </v-row>
    </Sheet>
    <v-row class="pt-10 py-0 pb-0 ma-0" justify="center">
      <Button
        v-if="isEditing"
        outlined
        style-set="large-tertiary"
        width="160"
        @click="buttonAction(0)"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        v-if="isEditing"
        class="ml-4"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true"
        @click="buttonAction(2)"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </v-row>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Sheet, ToolTips } from '~/components/common/atoms/index'
import { meStore } from '~/store'

export default BaseComponent.extend({
  components: {
    Button,
    Sheet,
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
     * 確認済み否か
     */
    isConfirm: {
      type: Boolean,
      default: false,
    },
    /**
     * 合計時間
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
.a-manhour-total--foot {
  font-weight: bold;
  font-size: 1.25rem;
  span {
    font-size: 1.5rem;
    color: $c-primary-dark;
  }
}
</style>
