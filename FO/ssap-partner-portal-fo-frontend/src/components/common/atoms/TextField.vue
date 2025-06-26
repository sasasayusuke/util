<template>
  <v-text-field
    class="a-text-field"
    v-bind="attributes"
    :type="type"
    :rules="validationCheck"
    v-on="$listeners"
  />
</template>

<script lang="ts">
import validator from 'validator'
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import type { PropType } from '~/common/BaseComponent'
/*
  v-text-fieldの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/text-fields/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    'hide-details': 'auto',
  },
  outlined: {
    outlined: true,
    dense: true,
  },
  bgWhite: {
    outlined: true,
    dense: true,
    class: 'a-text-field--bg-white',
  },
  bgTransparent: {
    outlined: true,
    dense: true,
    class: 'a-text-field--bg-transparent',
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
    minLength: {
      type: Number,
      default: 0,
    },
    email: {
      type: Boolean,
      default: false,
    },
    number: {
      type: Boolean,
      default: false,
    },
    phoneNumber: {
      type: Boolean,
      default: false,
    },
    positiveNumber: {
      type: Boolean,
      default: false,
    },
    maxDigits: {
      type: Number,
      default: 0,
    },
    positiveNumberDigits: {
      type: Number,
      default: 0,
    },
    decimalNumberDigits: {
      type: Number,
      default: 0,
    },
    additionalRules: {
      type: Array as PropType<Array<Function>>,
      required: false,
    },
    type: {
      type: String,
      required: false,
    },
    rangeNumberFrom: {
      type: [Number, Boolean],
      default: false,
    },
    rangeNumberTo: {
      type: [Number, Boolean],
      default: false,
    },
    validateDateFormat: {
      type: Boolean,
      default: false,
    },
    halfWidthCharAsHalf: {
      type: Boolean,
      default: false,
    },
    postalCode: {
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
    validationCheck(): Function[] {
      let rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: string | number) =>
          !!value || value === 0 || this.$t('common.rule.required')
        rules.push(rule)
      }
      //最大入力文字数
      if (this.maxLength) {
        // @ts-ignore
        this.attributes.maxlength = Number(this.maxLength)
        const rule = (value: any) => {
          let count: number = 0
          // halfWidthCharAsHalfフラグがtrueの場合は0.5文字換算
          for (const v of value || '') {
            if (
              this.halfWidthCharAsHalf &&
              (v.match(/[ -~]/) || v.match(/[ｦ-ﾟ]/))
            ) {
              count += 0.5
            } else {
              count += 1
            }
          }
          const messageType: string = this.halfWidthCharAsHalf
            ? 'common.rule.maxLengthWithHalfWidthChars'
            : 'common.rule.maxLength'
          const length: number = this.halfWidthCharAsHalf
            ? this.maxLength / 2
            : this.maxLength
          return count <= length || this.$t(messageType, { length })
        }
        rules.push(rule)
      }
      // 最小入力文字数
      if (this.minLength) {
        const length = Number(this.minLength)
        // @ts-ignore
        this.attributes.minLength = length
        const rule = (value: string) => {
          // 入力が空の場合、バリデーションをパスさせる
          if (!value) return true

          // 最小文字数以上であるかどうかをチェック
          return (
            value.length >= length ||
            this.$t('common.rule.minLength', { length })
          )
        }
        rules.push(rule)
      }
      //メール形式
      if (this.email) {
        const rule = (value: string) => {
          // 入力が空の場合、バリデーションをパスさせる
          if (!value) return true
          return validator.isEmail(value) || this.$t('common.rule.email')
        }
        rules.push(rule)
      }
      //数値
      if (this.number) {
        const rule = (value: any) =>
          !isNaN(value) || this.$t('common.rule.number')
        rules.push(rule)
      }
      //電話番号
      if (this.phoneNumber) {
        if (this.required) {
          const rule = (value: any) =>
            (value && /^[0-9-]+$/.test(value)) ||
            this.$t('common.rule.phoneNumber')
          rules.push(rule)
        } else {
          const rule = (value: any) =>
            !value ||
            /^[0-9-]+$/.test(value) ||
            this.$t('common.rule.phoneNumber')
          rules.push(rule)
        }
      }
      // 正の整数または0
      if (this.positiveNumber) {
        const rule = (value: string | number) => {
          if (typeof value === 'string') {
            if (value) {
              if (!/^(0|[1-9]\d*)$/.test(value)) {
                return this.$t('common.rule.positiveNumber')
              }
            }
            value = Number(value)
          }

          // 数値かつ整数であり、0以上であることをチェック
          if (Number.isInteger(value) && value >= 0) {
            return true
          }

          return this.$t('common.rule.positiveNumber')
        }
        rules.push(rule)
      }
      //最大入力桁数
      if (this.maxDigits) {
        const length = this.maxDigits
        const rule = (value: number) =>
          this.exceptDecimalPoint(value) <= Number(this.maxDigits) ||
          this.$t('common.rule.numberDigits', { length })

        rules.push(rule)
      }
      //整数部分の最大入力桁数
      if (this.positiveNumberDigits) {
        const length = this.positiveNumberDigits
        const rule = (value: number) =>
          this.exceptDecimalPoint(value) <= Number(this.positiveNumberDigits) ||
          this.$t('common.rule.positiveNumberDigits', { length })

        rules.push(rule)
      }
      //小数点以下の最大入力桁数
      if (this.decimalNumberDigits) {
        const length = this.decimalNumberDigits
        const rule = (value: number) =>
          this.getDecimalPointLength(value, Number(this.decimalNumberDigits)) <=
            Number(this.decimalNumberDigits) ||
          this.$t('common.rule.decimalNumberDigits', { length })

        rules.push(rule)
      }
      //数値の範囲
      if (this.rangeNumberFrom !== false || this.rangeNumberTo !== false) {
        if (this.rangeNumberFrom !== false && this.rangeNumberTo === false) {
          const rule = (value: any) =>
            value >= this.rangeNumberFrom ||
            this.$t('common.rule.rangeNumberFrom', {
              from: this.rangeNumberFrom,
            })
          rules.push(rule)
        } else if (
          this.rangeNumberFrom === false &&
          this.rangeNumberTo !== false
        ) {
          const rule = (value: any) =>
            value <= this.rangeNumberTo ||
            this.$t('common.rule.rangeNumberTo', {
              to: this.rangeNumberTo,
            })
          rules.push(rule)
        } else {
          const rule = (value: any) =>
            (value >= this.rangeNumberFrom && value <= this.rangeNumberTo) ||
            this.$t('common.rule.rangeNumberFromTo', {
              from: this.rangeNumberFrom,
              to: this.rangeNumberTo,
            })
          rules.push(rule)
        }
      }
      // yyyy/MM/dd形式チェック
      if (this.validateDateFormat) {
        const dateFormatRule = (value: string) => {
          if (!value) return true // 空の値はバリデーションを通過
          const dateRegex = /^\d{4}\/\d{2}\/\d{2}$/
          return (
            dateRegex.test(value) || this.$t('common.rule.invalidDateFormat')
          )
        }
        rules.push(dateFormatRule)
      }
      if (this.postalCode) {
        rules.push((value: string) => {
          if (!value) return true // 空の値はバリデーションを通過
          return /^\d{7}$/.test(value) || this.$t('common.rule.postalCode')
        })
      }
      // 親コンポーネントからルール関数配列を流し込む
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
  methods: {
    //小数点以下の桁数
    getDecimalPointLength(elm: number, maxLength: number): number {
      if (String(elm).includes('.')) {
        const numbers = String(elm).split('.')
        return numbers[1].length
      } else {
        return maxLength
      }
    },
    //整数部分の桁数
    exceptDecimalPoint(elm: number) {
      if (String(elm).includes('.')) {
        const numbers = String(elm).split('.')
        return numbers[0].length
      } else {
        return String(elm).length
      }
    },
  },
})
</script>

<style lang="scss">
.a-text-field--bg-transparent {
  &.v-input--is-disabled > .v-input__control > .v-input__slot fieldset {
    background: #e3e3e3 !important;
  }
}

.a-text-field {
  &.is-error {
    fieldset {
      color: $c-red !important;
    }
    .v-input__slot {
      background-color: $c-red-10 !important;
    }
  }
  &.v-text-field--outlined.v-input--dense.v-text-field--outlined
    > .v-input__control
    > .v-input__slot {
    min-height: 32px !important;
    font-size: 0.875rem !important;
  }
  &.v-text-field--outlined {
    &:not(.v-input--is-focused) {
      &:not(.v-input--has-state) {
        .v-input__control {
          .v-input__slot {
            fieldset {
              color: $c-gray-line-dark;
              background-color: $c-black-page-bg;
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
.a-text-field--bg-white {
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
  &.v-input--is-readonly {
    pointer-events: none;
    touch-action: none;
  }
}
</style>
<style lang="scss" scoped>
.a-text-field__error-text {
  margin-top: 4px !important;
}
</style>
