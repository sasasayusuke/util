<template>
  <v-col :cols="cols" class="px-3">
    <dl :class="{ 'is-boarder-bottom': borderBottom }">
      <dt class="font-size-xsmall font-weight-bold">{{ title }} :</dt>
      <dd class="font-size-small py-2">
        <nuxt-link v-if="link" :to="link" class="link">
          {{ text }}
        </nuxt-link>
        <template v-else>
          {{ text }}
        </template>
      </dd>
    </dl>
  </v-col>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    /** カラムのデフォルト数 */
    cols: {
      type: String,
      default: '1',
    },
    /** タイトル */
    title: {
      type: String,
      default: '',
    },
    /** テキスト */
    text: {
      type: String,
      default: '',
    },
    /** 下部ボーダーのクラス */
    borderBottom: {
      type: Boolean,
      default: true,
    },
    /** リンク */
    link: {
      type: String,
      default: '',
    },
  },
})
</script>

<style lang="scss" scoped>
dl {
  &.is-boarder-bottom {
    border-bottom: 1px solid $c-gray-line-dark;
    height: 100%;
  }
  dd {
    min-height: 37px;
  }
}
.link {
  font-weight: bold;
  color: $c-primary-dark;
}
</style>
