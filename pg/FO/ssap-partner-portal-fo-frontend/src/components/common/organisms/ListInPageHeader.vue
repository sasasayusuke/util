<template>
  <CommonInPageHeader :level="level">
    <slot name="default" />
    <span
      v-if="noteHead === 'required'"
      class="ml-10 m-heading__title__required"
      ><Required /><span class="m-heading__title__required__text">{{
        $t('common.label.required2')
      }}</span></span
    >
    <!-- dateが与えられていれば、最終更新日時を表示 -->
    <template v-if="date" #date>
      <span style="padding-right: 12px">{{
        $t('common.lastUpdate.date')
      }}</span>
      <span>{{
        date ? formatDate(new Date(date), 'Y/MM/DD hh:mm') : 'ー'
      }}</span>
    </template>
    <!-- createPageRouteが与えられていれば、新規作成ボタンを作成する -->
    <!-- saveがtrueであれば、保存ボタン、キャンセルボタンを作成する -->
    <!-- isSolverListがtrueであれば、新規個人ソルバー申請、個人ソルバー稼働率・単価更新へのリンクを作成する -->
    <template #button>
      <Button
        v-if="createPageRoute"
        style-set="small-primary"
        width="96"
        :to="createPageRoute"
      >
        {{ $t('common.button.register') }}
      </Button>
      <Button
        v-if="save"
        style-set="detailHeaderNegative"
        width="96"
        :to="backToUrl('')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        v-if="save"
        style-set="small-primary"
        width="96"
        :loading="isLoading"
        :disabled="isDisabled"
        :style="{ marginLeft: '8px' }"
        @click="$emit('save')"
      >
        {{ $t('common.button.save2') }}
      </Button>
      <div v-for="(item, index) in labels" :key="index">
        <Button
          v-if="forSolverList && index === 0"
          style-set="small-primary"
          width="160"
          :loading="isLoading"
          :disabled="isDisabled"
          :style="{ marginLeft: '8px' }"
          :to="item.to"
        >
          {{ item.label }}
        </Button>
        <Button
          v-if="forSolverList && index === 1"
          outlined
          style-set="small-primary"
          width="96"
          :loading="isLoading"
          :disabled="isDisabled"
          :style="{ marginLeft: '8px' }"
        >
          {{ item.label }}
        </Button>
      </div>
      <!-- 新規個人ソルバー申請ボタン -->
      <Button
        v-if="isSolverList"
        style-set="small-primary"
        :style="{ marginLeft: '8px' }"
        :to="`/solver/application?source_url=/solver/list/${solverCorporationIdStore}`"
      >
        {{ $t('solver.pages.list.button.toApplication') }}
      </Button>
      <!-- 個人ソルバー稼働率・単価更新ボタン -->
      <Button
        v-if="isSolverList && solverCorporationId"
        width="192"
        style-set="small-primary"
        :style="{ marginLeft: '8px' }"
        :to="`/solver/utilization-rate/${solverCorporationId}?source_url=/solver/list/${solverCorporationIdStore}`"
      >
        {{ $t('solver.pages.list.button.toUtilizationRate') }}
      </Button>
    </template>
  </CommonInPageHeader>
</template>

<script lang="ts">
import { Button, Required } from '../atoms/index'
import CommonInPageHeader from './CommonInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'
import { solverCorporationStore } from '~/store'

export default BaseComponent.extend({
  components: {
    Button,
    Required,
    CommonInPageHeader,
  },
  props: {
    createPageRoute: {
      type: [String, Object],
      default: '',
    },
    noteHead: {
      type: String,
      default: '',
    },
    level: {
      type: Number,
      default: 1,
    },
    save: {
      type: Boolean,
      default: false,
    },
    // 個人ソルバー一覧かどうか
    isSolverList: {
      type: Boolean,
      default: false,
    },
    date: {
      type: String,
      default: '',
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
    isDisabled: {
      type: Boolean,
      default: false,
    },
    // ソルバー候補一覧または個人ソルバー一覧か
    forSolverList: {
      type: Boolean,
      default: false,
    },
    // ボタン名
    labels: {
      type: Array,
      default: () => [],
    },
  },
  computed: {
    solverCorporationId(): string {
      // ルートパラメータから取得
      const SolverCorporationId = this.$route.params.solverCorporationId || ''
      return SolverCorporationId
    },
    solverCorporationIdStore(): string {
      const SolverCorporationId = solverCorporationStore.id
      return SolverCorporationId
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
