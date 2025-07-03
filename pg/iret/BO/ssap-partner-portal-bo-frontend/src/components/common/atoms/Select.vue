<template>
  <v-select
    v-if="!multiple"
    class="a-select"
    :class="{
      'bg-transparent': bgTransparent,
    }"
    v-bind="attributes"
    :rules="validationCheck"
    :items="items"
    v-on="$listeners"
  />
  <!-- 複数選択時 -->
  <v-select
    v-else
    class="a-select"
    :class="{
      'bg-transparent': bgTransparent,
    }"
    v-bind="attributes"
    :rules="validationCheck"
    :items="items"
    multiple
    v-on="$listeners"
  />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-selectの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/selects/
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

    items: {
      type: Array,
    },
    multiple: {
      type: Boolean,
    },
    required: {
      type: Boolean,
      default: false,
    },
    bgTransparent: {
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
      const rules: Function[] = []
      //必須
      if (this.required === true) {
        //複数選択
        if (this.multiple) {
          const rule = (value: string[]) => {
            if (value) {
              return value.length > 0 || this.$t('common.rule.required')
            }
          }
          rules.push(rule)
        } else {
          //単一選択
          const rule = (value: string[]) => {
            return !!value || this.$t('common.rule.required')
          }
          rules.push(rule)
        }
      }
      return rules
    },
  },
})
</script>
<style lang="scss">
.bg-transparent.v-input--is-disabled .v-input__control .v-input__slot fieldset {
  background: transparent !important;
}

.a-select {
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
  &.v-text-field--outlined.v-input--dense {
    .v-select__slot {
      height: 32px !important;
    }
    .v-select__selections {
      flex-wrap: nowrap;
    }
  }
  &.v-text-field--enclosed.v-input--dense:not(.v-text-field--solo).v-text-field--outlined
    .v-input__append-inner {
    margin-top: 4px !important;
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
  &.a-text-field--bg-white {
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
  &.v-input {
    input {
      &::placeholder {
        color: $c-gray-line-dark;
      }
    }
  }
}
.v-menu__content {
  .v-list-item {
    transition-property: background-color;
    transition-duration: 0.2s;
    &:hover,
    &:focus {
      background-color: $c-primary-8;
    }
  }
}
</style>
<style lang="scss" scoped>
.a-select__error-text {
  margin-top: 4px !important;
}
</style>
