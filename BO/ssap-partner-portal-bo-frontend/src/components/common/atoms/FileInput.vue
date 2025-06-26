<template>
  <v-file-input class="a-file-input" v-bind="attributes" v-on="$listeners" />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
/*
  v-file-inputの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/file-inputs/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    'hide-details': 'auto',
    dense: true,
    outlined: true,
  },
}

export default WrapperComponent.extend({
  // v-model用
  model: {
    prop: 'value',
    event: 'change',
  },
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

<style lang="scss">
.a-file-input {
  &.is-error {
    fieldset {
      color: $c-red !important;
    }
    .v-input__slot {
      background-color: $c-red-10 !important;
    }
  }
  &.v-text-field--outlined {
    &:not(.v-input--is-focused) {
      &:not(.v-input--has-state) {
        .v-input__control {
          .v-input__slot {
            fieldset {
              background-color: $c-black-page-bg;
              color: $c-gray-line-dark;
            }
          }
        }
      }
    }
  }
  &.v-file-input {
    .v-file-input__text--placeholder {
      color: $c-gray-line-dark;
    }
  }
}
</style>
