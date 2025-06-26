<template>
  <div class="m-common-anchor">
    <!-- 追加する・キャンセル・確認画面に進む・申請するボタン -->
    <Sheet
      v-if="!save && !forSolverList && !isDelete"
      class="flex-column pt-10"
      width="100%"
    >
      <div v-if="isEditing" class="d-flex mb-7">
        <Button style-set="add" class="ml-auto" @click="$emit('add')">
          <Icon class="mr-1" size="20" color="btn_primary">mdi-plus</Icon>
          {{ $t('common.button.add2') }}
        </Button>
      </div>
      <div class="w-100 d-flex">
        <Button
          v-if="isEditing"
          style-set="xlarge-cancel"
          class="mx-auto mr-8"
          width="300"
          :to="backToUrl(backUrl)"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          v-if="!isEditing"
          style-set="xlarge-cancel"
          class="mx-auto mr-8"
          width="300"
          @click="onClickBackToInput"
        >
          {{ $t('common.button.backToInput') }}
        </Button>
        <Button
          v-if="isEditing"
          :disabled="isDisabled"
          style-set="xlarge-primary"
          class="mx-auto ml-0"
          width="300"
          @click="onClickConfirm"
        >
          {{ $t('common.button.toConfirm') }}
        </Button>
        <Button
          v-if="!isEditing"
          :disabled="isDisabled"
          :loading="isLoading"
          style-set="xlarge-primary"
          class="mx-auto ml-0"
          width="300"
          @click="onClickApply"
        >
          {{ $t('common.button.apply') }}
        </Button>
      </div>
    </Sheet>
    <!-- 保存・キャンセルボタン -->
    <Sheet v-if="save" class="d-flex justify-center pt-16" width="100%">
      <Button style-set="cancel" class="mr-4" :to="backToUrl(backUrl)">
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        :loading="isLoading"
        :disabled="isDisabled"
        style-set="save"
        @click="$emit('save')"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </Sheet>
    <!-- ソルバートップへ戻るボタン -->
    <Sheet
      v-if="forSolverList"
      class="d-flex justify-center pt-16"
      width="100%"
    >
      <Button style-set="cancel" class="mr-4" to="/solver/menu">
        {{ $t('common.button.solverTop') }}
      </Button>
    </Sheet>
    <!-- このソルバーを削除するボタン -->
    <Sheet
      v-if="isDelete && !isEditing"
      class="d-flex justify-center mt-16 pt-6 pb-6"
      width="100%"
      color="#e3e3e3"
    >
      <Button width="220" style-set="cancel" @click="$emit('delete')">
        {{ $t('common.button.deleteSolver') }}
      </Button>
    </Sheet>
  </div>
</template>

<script lang="ts">
import { Button, Sheet, Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'SolverFooter',
  components: {
    Button,
    Sheet,
    Icon,
  },
  computed: {
    backUrl(): string {
      const sourceUrlParam = this.$route.query.source_url
      return sourceUrlParam ? '' : '/solver/menu'
    },
  },
  props: {
    save: {
      type: Boolean,
      default: false,
    },
    forSolverList: {
      type: Boolean,
      default: false,
    },
    isDelete: {
      type: Boolean,
      default: false,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
    isDisabled: {
      type: Boolean,
      default: true,
    },
    isEditing: {
      type: Boolean,
      default: true,
    },
  },
  methods: {
    // 「確認画面に進む」ボタン押下時の処理
    onClickConfirm() {
      this.$emit('click:confirm')
    },

    // 「入力画面に戻る」ボタン押下時の処理
    onClickBackToInput() {
      this.$emit('click:backToInput')
    },

    // 「申請する」ボタン押下時の処理
    onClickApply() {
      this.$emit('click:apply')
    },
  },
})
</script>
