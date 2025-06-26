<template>
  <v-chip class="a-chip" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-chip>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-chipの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/chips/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  primary: {
    label: true,
    color: 'primary',
  },
  secondary: {
    label: true,
    color: 'secondary',
  },
  tertiary: {
    label: true,
    color: 'tertiary',
  },
  required: {
    label: true,
    color: 'required',
  },
  any: {
    label: true,
    color: 'any',
  },
  'primary-60': {
    label: true,
    color: 'primary',
    class: 'width-60',
  },
  'primary-70': {
    label: true,
    color: 'primary',
    class: 'width-70',
  },
  'secondary-70': {
    label: true,
    color: 'secondary',
    class: 'width-70',
  },
  'tertiary-70': {
    label: true,
    color: 'tertiary',
    class: 'width-70',
  },
  'required-70': {
    label: true,
    color: 'required',
    class: 'width-70',
  },
  'any-70': {
    label: true,
    color: 'any',
    class: 'width-70',
  },
  'secondary-60': {
    label: true,
    color: 'secondary',
    class: 'width-60',
  },
  sf: {
    'x-small': true,
    outlined: true,
    label: true,
    color: 'primary',
  },
  void: {
    'x-small': true,
    label: true,
    class: 'is-void',
  },
  confirm: {
    color: 'secondary',
    class: 'is-confirm',
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
$labelColors: (
  primary: $c-secondary,
  secondary: #0d3296,
  tertiary: #8f8f8f,
  required: #d53030,
  any: #707070,
);
@each $type, $color in $labelColors {
  .a-chip {
    padding: 0 10px !important;
    display: inline-flex;
    justify-content: center;
    border-radius: 2px !important;
    &.#{$type} {
      color: $c-white !important;
      background: $color !important;
      &.#{$type}--text {
        color: $color !important;
        background: transparent !important;
        border-color: $color !important;
        &.v-chip--outlined {
          background-color: $c-white !important;
        }
      }
    }
    &.v-size--x-small {
      height: 20px;
      font-size: 0.75rem !important;
    }
    &.v-size--small {
      height: 24px;
      font-size: 0.75rem !important;
    }
    &.v-size--default {
      height: 26px;
      font-size: 0.875rem !important;
    }
    &.v-size--large {
      height: 28px;
      font-size: 0.875rem !important;
    }
    &.v-size--x-large {
      height: 30px;
    }
    @for $i from 1 through 20 {
      &.width-#{$i*5} {
        padding: 0 !important;
        display: inline-flex;
        justify-content: center;
        width: #{$i * 5}px;
      }
    }
    &.is-void {
      opacity: 0;
    }
    &.is-confirm {
      @include fontSize('xxsmall');
      min-width: 18px;
      height: 18px;
      padding: 0 !important;
      border-radius: 0 !important;
    }
  }
}
</style>
