<template>
  <div>
    <v-combobox
      v-if="isCombobox"
      class="a-autocomplete"
      v-bind="attributes"
      :rules="validationCheck"
      :no-data-text="$t('common.label.noData')"
      v-on="$listeners"
    />
    <v-autocomplete
      v-else
      :append-icon="showIcon ? 'mdi-menu-down' : ''"
      class="a-autocomplete"
      v-bind="attributes"
      :rules="validationCheck"
      :no-data-text="$t('common.label.noData')"
      v-on="$listeners"
    />
    <span v-if="warning" class="error--text warning-style">{{ warning }}</span>
  </div>
</template>

<script lang="ts">
import validator from 'validator'
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import type { PropType } from '~/common/BaseComponent'

/*
  v-autocompleteの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/autocompletes/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    'hide-details': 'auto',
    outlined: true,
    dense: true,
  },
  menuIcon: {
    'hide-details': 'auto',
    outlined: false,
    dense: true,
  },
  bgWhite: {
    outlined: true,
    dense: true,
    class: 'a-autocomplete--bg-white',
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
    isCombobox: {
      type: Boolean,
      default: false,
    },
    //バリデーション
    required: {
      type: Boolean,
      default: false,
    },
    maxLength: {
      type: Number,
      default: 0,
    },
    email: {
      type: Boolean,
      default: false,
    },
    additionalRules: {
      type: Array as PropType<Array<Function>>,
      required: false,
    },
    warning: {
      type: String,
    },
    showIcon: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
  computed: {
    validationCheck(): any {
      let rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: string | number) =>
          !!value || this.$t('common.rule.required')
        rules.push(rule)
      }
      //最大入力文字数
      if (this.maxLength) {
        const maxLength = Number(this.maxLength)
        // @ts-ignore
        this.attributes.maxlength = maxLength
        const rule = (value: any) => {
          const targetValue: string = value && value === 'string' ? value : ''
          // @ts-ignore
          return (
            targetValue.length <= maxLength ||
            this.$t('common.rule.maxLength', { maxLength })
          )
        }

        rules.push(rule)
      }
      //メール形式
      if (this.email) {
        const rule = (value: string) =>
          validator.isEmail(value) || this.$t('common.rule.email')
        rules.push(rule)
      }
      if (
        this.additionalRules &&
        this.additionalRules.length &&
        this.additionalRules.length > 0
      ) {
        rules = rules.concat(this.additionalRules)
      }
      return rules
    },
  },
})
</script>

<style lang="scss">
.v-autocomplete__content {
  .v-list-item {
    &::before {
      mix-blend-mode: darken !important;
    }
    &:hover {
      &::before {
        opacity: 1 !important;
      }
    }
  }
}
.a-autocomplete {
  &.v-text-field--outlined.v-input--dense.v-text-field--outlined
    > .v-input__control
    > .v-input__slot {
    min-height: 32px !important;
    font-size: 0.875rem !important;
  }
  &.v-text-field--outlined {
    background-color: $c-black-page-bg;
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
.a-autocomplete--bg-white {
  &.v-text-field--outlined {
    &:not(.v-input--is-focused) {
      &:not(.v-input--has-state) {
        .v-input__control {
          .v-input__slot {
            fieldset {
              background-color: $c-white;
            }
          }
        }
      }
    }
  }
  &.v-input--is-disabled {
    &.v-text-field--outlined {
      &:not(.v-input--is-focused) {
        &:not(.v-input--has-state) {
          .v-input__control {
            .v-input__slot {
              fieldset {
                background-color: $c-black-page-bg;
              }
            }
          }
        }
      }
    }
  }
}
</style>

<style>
.v-application .warning-style {
  color: #dc5c5d;
  margin-left: 14px;
  font-size: 12px;
}
</style>
