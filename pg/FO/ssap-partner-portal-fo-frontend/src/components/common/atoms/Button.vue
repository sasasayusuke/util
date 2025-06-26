<template>
  <!-- 関数を実行するボタンの場合 -->
  <v-btn class="a-button" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-btn>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-btnの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/buttons/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    elevation: '0',
    ripple: false,
  },
  text: {
    text: true,
    class: 'px-0',
  },
  icon: {
    text: true,
    class: 'pa-0',
    color: 'primary',
    height: 'auto',
  },
  'icon-rotate-45': {
    text: true,
    class: 'pa-0 rotate rotate--45',
    color: 'primary',
    height: 'auto',
  },
  'xlarge-primary': {
    'x-large': true,
    color: 'primary',
  },
  'xlarge-secondary': {
    'x-large': true,
    color: 'secondary',
  },
  'xlarge-tertiary': {
    'x-large': true,
    color: 'tertiary',
  },
  'xlarge-disabled': {
    'x-large': true,
    disabled: true,
  },
  'xlarge-cancel': {
    'x-large': true,
    color: 'tertiary',
    outlined: true,
  },
  'large-primary': {
    large: true,
    color: 'primary',
  },
  'large-secondary': {
    large: true,
    color: 'secondary',
  },
  'large-tertiary': {
    large: true,
    color: 'tertiary',
  },
  'large-error': {
    large: true,
    color: 'error',
  },
  'large-disabled': {
    large: true,
    disabled: true,
  },
  'normal-primary': {
    color: 'primary',
  },
  'normal-primary-text': {
    color: 'primary',
    text: true,
    class: 'pa-0',
  },
  'normal-secondary': {
    color: 'secondary',
  },
  'normal-tertiary': {
    color: 'tertiary',
  },
  'normal-disabled': {
    disabled: true,
  },
  'normal-error': {
    color: 'error',
  },
  'small-primary': {
    small: true,
    color: 'primary',
  },
  'small-secondary': {
    small: true,
    color: 'secondary',
  },
  'small-tertiary': {
    small: true,
    color: 'tertiary',
  },
  'small-disabled': {
    small: true,
    disabled: true,
  },
  'xsmall-primary': {
    'x-small': true,
    color: 'primary',
  },
  'xsmall-secondary': {
    'x-small': true,
    color: 'secondary',
  },
  'xsmall-tertiary': {
    'x-small': true,
    color: 'tertiary',
  },
  'xsmall-disabled': {
    'x-small': true,
    disabled: true,
  },
  add: {
    large: true,
    color: 'primary',
    width: '160px',
    outlined: true,
  },
  sort: {
    small: true,
    color: 'primary',
    class: 'font-size-small',
    width: '120px',
  },
  submit: {
    small: true,
    color: 'primary',
    class: 'px-4',
  },
  count: {
    small: true,
    outlined: true,
    width: '28',
    class: 'px-0',
    color: 'primary',
  },
  cancel: {
    large: true,
    color: 'tertiary',
    width: '160px',
    outlined: true,
  },
  save: {
    large: true,
    color: 'primary',
    width: '160px',
  },
  'group-xlarge': {
    'x-large': true,
    width: '200',
    height: '60',
    color: 'primary',
  },
  'group-large': {
    large: true,
    width: '250',
    height: '48',
    color: 'primary',
  },
  'group-small': {
    small: true,
    width: '150',
    color: 'primary',
  },
  'global-header': {
    text: true,
    tile: true,
    color: '#000',
  },
  'page-header': {
    text: true,
    tile: true,
    color: '#666',
    height: '60',
  },
  pagination: {
    large: true,
    color: '#fff',
    elevation: '3',
    class: 'pa-0 is-pagination',
    width: '44',
  },
  detailHeaderPositive: {
    small: true,
    color: 'primary',
    width: '96',
  },
  detailHeaderPositiveStretch: {
    small: true,
    color: 'primary',
  },
  detailHeaderNegative: {
    small: true,
    color: 'tertiary',
    width: '96',
    outlined: true,
  },
  detailHeaderError: {
    small: true,
    color: 'error',
    width: '96',
    outlined: true,
  },
  'text-button': {
    color: 'primary',
    text: true,
    class: 'd-flex align-center pa-0',
    style: 'height:auto;',
  },
  showAll: {
    color: '#333',
    text: true,
    class: 'd-flex align-center pa-0 font-weight-normal font-size-xsmall',
    width: '100%',
    style: 'height:auto;',
  },
  showContinuation: {
    elevation: '3',
    width: '100%',
    color: '#fff',
    class: 'is-showContinuation',
  },
  'home-tools-1': {
    'x-large': true,
    color: 'primary',
    outlined: true,
    class: 'home-tools--1',
    width: 290,
  },
  'home-tools-2': {
    'x-large': true,
    color: '#00AD19',
    class: 'home-tools--2',
    width: 290,
  },
  'satisfied-survey': {
    class: 'justify-space-between',
  },
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.a-button {
  &.v-btn {
    font-weight: 700;
    min-width: 0px;
    text-transform: none;
    &.font-weight-normal {
      font-weight: normal;
    }
    &::before,
    &::after {
      display: none;
    }
    // background-color: transparent !important;
    transition-property: background-color;
    transition-duration: 0.2s;
    @each $type, $color in $buttonColors {
      @if $type == disabled {
        &.v-btn--#{ $type } {
          color: $c-white !important;
          background-color: map-get($color, 'default') !important;
        }
      } @else {
        &.#{ $type } {
          background-color: map-get($color, 'default') !important;
          color: $c-white !important;
          &:hover,
          &:focus {
            background-color: map-get($color, 'hover') !important;
          }
        }
        &.v-btn--outlined {
          background-color: $c-white !important;
        }
        &.#{ $type }--text {
          background-color: transparent !important;
          color: map-get($color, 'default') !important;
          &:hover,
          &:focus {
            color: map-get($color, 'hover') !important;
          }
        }
        &.v-btn--text {
          background-color: transparent !important;
          transition: 0.2s;
          &:hover,
          &:focus {
            color: $c-primary-dark !important;
          }
        }
      }
    }
  }
  &.v-btn--outlined {
    background-color: $c-white !important;
    @each $type, $color in $buttonColorsOutline {
      @if $type == disabled {
        &.v-btn--#{ $type } {
          background-color: transparent !important;
          color: map-get($color, 'default') !important;
        }
      } @else {
        &.#{ $type }--text {
          color: map-get($color, 'default') !important;
          &:hover,
          &:focus {
            background-color: map-get($color, 'hover') !important;
          }
        }
      }
    }
    &.home-tools--1 {
      color: $c-black-60 !important;
      &:hover {
        color: $c-black-60 !important;
        background-color: $c-primary-8 !important;
      }
    }
  }
  &.v-size--x-large {
    height: 60px;
  }
  &.is-pagination {
    &.v-btn--disabled {
      background-color: #fff !important;
      box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
        0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
    }
    &:hover,
    &:focus {
      background-color: $c-primary-over !important;
      .v-icon {
        color: $c-white !important;
      }
    }
  }
  &.home-tools--2 {
    &:hover {
      background-color: $c-primary-dark !important;
    }
  }
}
</style>
<style lang="scss">
.a-button {
  .is-pre-wrap {
    white-space: pre-wrap;
  }
  &.is-pagination {
    &.v-btn--disabled {
      .v-icon {
        color: #8f8f8f !important;
      }
    }
    &:hover,
    &:focus {
      .v-icon {
        color: $c-white !important;
      }
    }
  }
  &.rotate {
    transform-origin: center center;
    &--45 {
      transform: rotate(45deg);
    }
  }
  &.is-showContinuation {
    &:hover,
    &:focus {
      background-color: $c-primary-8 !important;
      // color: $c-white !important;
      // .v-icon {
      //   color: $c-white !important;
      // }
    }
    .v-icon {
      transform-origin: center center;
    }
    &.is-open {
      .v-icon {
        transform: rotate(180deg);
        transition-duration: 0.3s;
      }
    }
  }
}
.v-btn__content {
  letter-spacing: 0;
}
</style>
