<template>
  <v-row class="o-karte-detail-row" no-gutters :class="classes">
    <v-col>
      <v-row
        no-gutters
        class="o-karte-detail-row__header"
        justify="space-between"
        align="center"
      >
        <v-col v-if="projectId" cols="auto customer-success__cols">
          <v-col cols="auto customer-success__icon">
            <Icon style-set="primary" size="25" class="mr-1">
              icon-org-flag-variant
            </Icon>
          </v-col>
          <component
            :is="hx"
            class="o-karte-detail-row__sub-title customer-success__sub-title"
          >
            {{ title }}
          </component>
        </v-col>
        <v-col v-else cols="auto">
          <component :is="hx" class="o-karte-detail-row__sub-title">
            {{ title }}
          </component>
        </v-col>
        <v-col v-if="projectId" cols="auto">
          <Button
            v-if="isCustomer === false"
            style-set="small-primary"
            outlined
            :to="forwardToUrl(`/project/${projectId}`)"
            >{{ $t('karte.pages.detail.project') }}</Button
          >
        </v-col>
      </v-row>
      <!-- <v-row no-gutters class="o-karte-detail-row__content">
        <v-col>
          <slot name="default" />
        </v-col>
      </v-row> -->
      <v-col class="o-karte-detail-rows__data">
        <v-row v-if="isEditing" class="px-3 pt-3 pb-3">
          <slot name="default"></slot>
        </v-row>
        <template v-else>
          <slot name="isNotEditing">
            <!-- eslint-disable vue/no-v-html -->
            <span
              v-html="
                $sanitize(
                  typeof value === 'string'
                    ? value.replace(/\r?\n/g, '<br />')
                    : value
                )
              "
            ></span>
            <!-- eslint-enable -->
          </slot>
        </template>
      </v-col>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Icon, Button } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: '',
  components: {
    Button,
    Icon,
  },
  props: {
    /**
     * プロジェクトID
     */
    projectId: {
      type: String,
      default: '',
    },
    /**
     * タイトル
     */
    title: {
      type: String,
      required: true,
    },
    /**
     * アンダーラインが必要ないか否か
     */
    isNoUnderLine: {
      type: Boolean,
      default: false,
    },
    /**
     * ショートか否か
     */
    isShort: {
      type: Boolean,
      default: false,
    },
    /**
     * フォントサイズ
     */
    hx: {
      type: String,
      default: 'h2',
    },
    /**
     * 編集か否か
     */
    isEditing: { type: Boolean },
    value: { default: '' },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      classes: {
        'is-no-under-line': this.isNoUnderLine,
        'is-short': this.isShort,
      },
    }
  },
})
</script>

<style lang="scss" scoped>
.o-karte-detail-row {
  padding: 24px 0;
  border-bottom: 1px solid $c-gray-line;
  &:first-child {
    padding-top: 0;
  }
  &.is-no-under-line {
    border-bottom: 0;
    padding-bottom: 32px;
    &.is-short {
      border-bottom: 0;
      padding-bottom: 0;
    }
  }
  &.is-short {
    padding: 0 0 24px;
    border-bottom: 1px solid $c-gray-line;
    &:nth-child(n + 2) {
      margin-top: 24px;
    }
    &:last-child {
      border-bottom: 0;
    }
  }
}
.o-karte-detail-row__header {
  padding-bottom: 12px;
}
.o-karte-detail-row__sub-title {
  @include fontSize($size: 'small');
}
.o-karte-detail-row__content {
  p {
    margin-bottom: 0;
    & + p {
      margin-top: 1em;
    }
  }
}
.o-karte-detail-rows__data {
  padding: 0;
  overflow-wrap: break-word;
}
.customer-success__cols {
  display: flex;
  align-items: center;
}
.customer-success__icon {
  padding: 0 !important;
}
.customer-success__sub-title {
  color: #008a19 !important;
}
</style>
