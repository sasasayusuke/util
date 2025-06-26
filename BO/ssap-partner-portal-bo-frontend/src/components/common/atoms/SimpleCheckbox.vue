<template>
  <v-simple-checkbox
    class="a-checkbox"
    :class="classes"
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

  VuetifyLink
  https://vuetifyjs.com/ja/components/checkboxes/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
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
    isNoEditing: {
      type: Boolean,
      default: false,
    },
    required: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
      classes: {
        'is-no-editing': this.isNoEditing,
      },
    }
  },
  computed: {
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: string[]) => {
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
  .v-label {
    @include fontSize('small');
  }
  .theme--light.v-label {
    color: $c-black;
  }
  .theme--light.v-label--is-disabled {
    color: rgba(0, 0, 0, 0.38);
  }
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
  &.is-no-editing {
    pointer-events: none;
    touch-action: none;
    &.type2 {
      &.v-input--is-label-active {
        &.theme--light {
          .v-label {
            color: $c-secondary;
            font-weight: bold;
            @include fontSize('small');
          }
        }
      }
    }
  }
}
</style>
