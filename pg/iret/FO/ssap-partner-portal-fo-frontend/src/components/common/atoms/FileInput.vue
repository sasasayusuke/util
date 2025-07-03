<template>
  <v-file-input
    class="a-file-input"
    v-bind="attributes"
    :rules="rules"
    v-on="$listeners"
    @change="validationCheck($event)"
  />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import type { PropType } from '~/common/BaseComponent'
/*
  v-file-inputの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
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
    allowExtensions: {
      type: Array as PropType<Array<String>>,
      required: false,
      default: [],
    },
    maxFileSize: {
      type: Number,
      required: false,
      default: 0,
    },
    additionalRules: {
      type: Array as PropType<Array<string | boolean>>,
      required: false,
    },
  },
  data(): {
    attributeSet: AttributeSet
    rules: (string | boolean)[]
  } {
    return {
      attributeSet: ATTRIBUTE_SET,
      rules: [],
    }
  },
  watch: {
    additionalRules() {
      if (this.additionalRules.length > 0) {
        this.additionalRules.forEach((rule) => {
          this.rules.push(rule)
        })
      }
    },
  },
  methods: {
    validationCheck(event: File): void {
      const rules: (string | boolean)[] = []
      // 許可された拡張子
      if (this.allowExtensions.length > 0) {
        if (event && event.name) {
          const extension = String(event.name.split('.').pop()).toLowerCase()
          const rule: boolean | string =
            this.allowExtensions.includes(extension) ||
            String(this.$t('karte.pages.detail.errorMessages.allowExtensions'))
          rules.push(rule)
        }
      }
      // ファイルサイズ制限
      if (this.maxFileSize > 0) {
        if (event && event.size) {
          let sizeString = ''
          if (this.maxFileSize < 1024) {
            // byte表記
            sizeString = String(Math.floor(this.maxFileSize)) + 'byte'
          } else if (this.maxFileSize < 1024 * 1024) {
            // Kbyte表記
            sizeString = String(Math.floor(this.maxFileSize / 1024)) + 'KB'
          } else if (this.maxFileSize < 1024 * 1024 * 1024) {
            // Mbyte表記
            sizeString =
              String(Math.floor(this.maxFileSize / 1024 / 1024)) + 'MB'
          } else if (this.maxFileSize < 1024 * 1024 * 1024 * 1024) {
            // Gbyte表記
            sizeString =
              String(Math.floor(this.maxFileSize / 1024 / 1024 / 1024)) + 'GB'
          } else if (this.maxFileSize < 1024 * 1024 * 1024 * 1024 * 1024) {
            // Tbyte表記
            sizeString =
              String(Math.floor(this.maxFileSize / 1024 / 1024 / 1024 / 1024)) +
              'TB'
          }
          const rule: boolean | string =
            this.maxFileSize >= event.size ||
            String(
              this.$t('karte.pages.detail.errorMessages.maxFileSize', {
                size: sizeString,
              })
            )
          rules.push(rule)
        }
      }
      this.rules = rules
    },
  },
})
</script>

<style lang="scss">
.a-file-input {
  @include fontSize('small');
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
              color: $c-gray-line-dark;
              background-color: $c-black-page-bg;
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
