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
  'icon-addable': {
    text: true,
    class: 'pa-0 hover-fill--light',
    color: 'primary',
    height: 'auto',
  },
  'icon-rotate-45': {
    text: true,
    class: 'pa-0 rotate rotate--45 hover-fill--light',
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
  'small-error': {
    small: true,
    color: 'error',
  },
  'small-disabled': {
    small: true,
    disabled: true,
  },
  'small-primary-96': {
    small: true,
    color: 'primary',
    width: '96px',
  },
  'small-secondary-96': {
    small: true,
    color: 'secondary',
    width: '96px',
  },
  'small-tertiary-96': {
    small: true,
    color: 'tertiary',
    width: '96px',
  },
  'small-disabled-96': {
    small: true,
    disabled: true,
    width: '96px',
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
  remove: {
    large: true,
    color: 'error',
    width: '160px',
    outlined: true,
  },
  disable: {
    large: true,
    color: 'tertiary',
    width: '175px',
    outlined: true,
  },
  enable: {
    large: true,
    color: 'primary',
    width: '175px',
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
  'group-small-auto': {
    small: true,
    color: 'primary',
  },
  'global-header': {
    text: true,
    tile: true,
    color: '#fff',
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
  detailHeaderNegativeNew: {
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
  'text-underline': {
    class: 'text-underline',
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
          &.text--disabled {
            color: $c-disabled !important;
          }
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
            color: map-get($color, 'hover') !important;
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
  &.text-underline {
    text-decoration: underline;
    &:hover,
    &:focus {
      span {
        color: $c-primary-over !important;
      }
    }
  }
  &.hover-fill--light {
    &:hover,
    &:focus {
      .v-btn__content {
        &::after {
          opacity: 1;
        }
      }
    }
    .v-btn__content {
      display: block;
      position: relative;
      &::after {
        display: block;
        content: '';
        width: 99%;
        height: 99%;
        background-color: $c-primary-8;
        position: absolute;
        top: 0;
        left: 1px;
        border-radius: 50%;
        opacity: 0;
        transition-duration: 0.3s;
      }
      .v-icon {
        position: relative;
        z-index: 1;
      }
    }
  }
  &.theme--light.v-btn.v-btn--disabled .v-icon {
    // color: $c-white !important;
    &.transparent--text {
      opacity: 0;
    }
  }
}
.v-btn__content {
  letter-spacing: 0;
}
</style>
