<template>
  <Sheet class="m-heading d-flex align-center" height="60" width="100%">
    <ContentHeaderContainer :level="level" :class="'type' + type">
      {{ title }}
      <template #summary>
        <div class="d-flex align-center a-manhour-samary-head pl-9">
          <!-- 確定アイコン -->
          <Chip
            v-if="isConfirm"
            label
            small
            class="width-70 mr-2"
            color="secondary"
          >
            {{ $t('common.button.determine') }}
          </Chip>
          <!-- 未確定アイコン -->
          <Chip
            v-else
            label
            small
            outlined
            class="width-70 mr-2"
            color="secondary"
          >
            {{ $t('common.label.unsettled') }}
          </Chip>
          <!-- 支援工数合計時間 -->
          <div class="a-manhour-total--head">
            {{ $t('man-hour.pages.supporter.detail.total') }} :
            <span>{{ total }}h</span>
          </div>
        </div>
      </template>
    </ContentHeaderContainer>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import ContentHeaderContainer from '~/components/man-hour/organisms/ContentHeaderContainer.vue'
import { Button, Chip, Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'InContentHeader',
  components: {
    CommonContainer,
    ContentHeaderContainer,
    Button,
    Chip,
    Sheet,
  },
  props: {
    /** 編集中判定 */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /** タイトル */
    title: {
      type: String,
      default: '',
    },
    /** Headingタグの数字 */
    level: {
      type: Number,
      default: 1,
    },
    type: {
      type: Number,
      default: 3,
    },
    /** 確定or未確定判定 */
    isConfirm: {
      type: Boolean,
      default: false,
    },
    /** 支援工数合計時間 */
    total: {
      type: Number,
      default: 0,
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
</style>
