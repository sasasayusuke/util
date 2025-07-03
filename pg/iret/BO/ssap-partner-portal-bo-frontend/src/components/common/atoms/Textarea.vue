<template>
  <v-textarea
    class="a-textarea"
    v-bind="attributes"
    :rules="validationCheck"
    v-on="$listeners"
  />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-textareaの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/textareas/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    'hide-details': 'auto',
  },
  outlined: {
    outlined: true,
    dense: true,
  },
}

export default WrapperComponent.extend({
  // v-model用
  model: {
    prop: 'value',
    event: 'input',
  },
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    required: {
      type: Boolean,
      default: false,
    },
    maxLength: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
  computed: {
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: string | number) =>
          !!value || this.$t('common.rule.required')
        rules.push(rule)
      }
      //最大入力文字数
      if (this.maxLength) {
        const length = Number(this.maxLength)
        // @ts-ignore
        this.attributes.maxlength = length
        const rule = (value: number) =>
          // @ts-ignore
          (value || '').length <= length ||
          this.$t('common.rule.maxLength', { length })
        rules.push(rule)
      }
      return rules
    },
  },
})
</script>

<style lang="scss">
.a-textarea {
  font-size: 0.875rem !important;
  .v-input__slot {
    background-color: #fafafa !important;
  }
  &.font-small {
    font-size: 0.75rem !important;
  }
  &.is-error {
    .v-input__slot {
      background-color: $c-red-10 !important;
    }
    fieldset {
      color: $c-red !important;
    }
  }
  &.is-no-editing {
    pointer-events: none;
    touch-action: none;
  }
  &.v-text-field--outlined {
    &:not(.v-input--is-focused) {
      &:not(.v-input--has-state) {
        .v-input__control {
          .v-input__slot {
            fieldset {
              color: $c-gray-line-dark;
            }
          }
        }
      }
    }
  }
  &.v-input {
    input {
      &::placeholder {
        color: $c-gray-line-dark;
      }
    }
  }
}
</style>
