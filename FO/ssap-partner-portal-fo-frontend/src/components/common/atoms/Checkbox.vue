<template>
  <v-checkbox
    class="a-checkbox"
    :class="className"
    v-bind="attributes"
    :rules="validationCheck"
    v-on="$listeners"
  />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-checkboxの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  black: {
    class: 'a-checkbox--black',
  },
}

export default WrapperComponent.extend({
  // v-model用
  model: {
    prop: 'input-value',
    event: 'change',
  },
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    type: {
      type: String,
      default: '',
    },
    required: {
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
    className() {
      return this.type ? 'a-checkbox--' + this.type : ''
    },
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: string[]) => {
          // @ts-ignore
          return value.length > 0 || this.$t('common.rule.required')
        }
        rules.push(rule)
      }
      return rules
    },
  },
})
</script>

<style lang="scss">
.a-checkbox {
  &.is-error {
    .v-icon {
      color: $c-red !important;
      &::before {
        position: relative;
        z-index: 2;
      }
      &::after {
        width: 14px;
        height: 14px;
        top: 5px;
        left: 5px;
        border-radius: 2px;
        background-color: $c-red-10;
        opacity: 1;
        transform: scale(1);
      }
    }
  }
  &--black {
    .v-label {
      color: $c-black;
    }
  }
  &.a-checkbox--gray {
    .v-icon {
      color: rgba(0, 0, 0, 0.6);
    }
  }
  &.v-input--is-readonly {
    pointer-events: none;
    touch-action: none;
  }
  .v-label {
    @include fontSize('small');
  }
}
.a-checkbox--2 {
  .v-input__slot {
    align-items: flex-start;
    .v-label {
      top: 2px;
    }
  }
}
.a-checkbox--noEditing {
  pointer-events: none;
  touch-action: none;
}
</style>
