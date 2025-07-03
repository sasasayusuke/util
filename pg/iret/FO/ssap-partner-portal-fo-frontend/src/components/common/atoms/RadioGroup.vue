<template>
  <v-radio-group
    class="a-radio"
    :class="setDirection"
    v-bind="attributes"
    v-on="$listeners"
  >
    <template v-for="(value, index) in values">
      <template v-if="isNoEditing">
        <v-radio
          :key="value"
          :value="value"
          :disabled="!value"
          :aria-label="labels[index]"
          class="is-no-edit"
          v-bind="radioVBind"
        >
          <template #label>
            <span class="a-radio__text" :class="{ 'is-hide': isHideLabel }">{{
              labels ? labels[index] : value
            }}</span>
          </template>
        </v-radio>
      </template>
      <template v-else>
        <v-radio
          :key="value"
          :value="value"
          :aria-label="labels[index]"
          :class="{
            'bg-white-radio': bgWhite,
          }"
          v-bind="radioVBind"
        >
          <template #label>
            <span class="a-radio__text" :class="{ 'is-hide': isHideLabel }">{{
              labels ? labels[index] : value
            }}</span>
          </template>
        </v-radio>
      </template>
    </template>
    <slot name="unique" />
  </v-radio-group>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-radio-groupの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/radio-buttons/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    'hide-details': 'auto',
  },
}

// v-radioに適用されるATTRIBUTE_SET
const RADIO_ATTRIBUTE_SET: AttributeSet = {
  default: {},
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
    // 各radioをユーザーが指定した時に返す値
    values: {
      type: Array,
      required: true,
    },
    // radioに割り振られるラベル(指定がない場合、valuesがそのままlabelとなる)
    labels: {
      type: Array,
    },
    radioStyleSet: {
      type: String,
      default: '',
    },
    radioAttributes: {
      type: Object,
      default() {
        return {}
      },
    },
    horizontal: {
      type: Boolean,
      default: false,
    },
    isNoEditing: {
      type: Boolean,
      default: false,
    },
    isHideLabel: {
      type: Boolean,
      default: false,
    },
    bgWhite: {
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
    radioVBind() {
      return this.generateAttributes(
        RADIO_ATTRIBUTE_SET,
        this.radioStyleSet,
        this.radioAttributes
      )
    },
    setDirection() {
      return this.horizontal ? 'horizontal' : ''
    },
  },
})
</script>
<style lang="scss">
.a-radio {
  .bg-white-radio {
    .v-icon {
      border-radius: 50%;
      background-image: radial-gradient(#fff 50%, transparent 50%);
    }
  }
}

.a-radio {
  &.is-no-label {
    display: inline-block;
    .v-input--selection-controls__input {
      margin-right: 0 !important;
    }
  }
  .v-radio {
    &:only-child {
      margin-right: 0 !important;
    }
    &.is-no-edit {
      pointer-events: none;
      touch-action: none;
    }
  }
  &.is-error {
    .v-icon {
      color: $c-red !important;
      &::before {
        position: relative;
        z-index: 2;
      }
      &::after {
        width: 16px;
        height: 16px;
        top: 4px;
        left: 4px;
        border-radius: 50%;
        background-color: $c-red-10;
        opacity: 1;
        transform: scale(1);
      }
    }
  }
  &.horizontal {
    margin: 0 !important;
    padding: 0 !important;
    .v-input--radio-group__input {
      display: flex;
      flex-direction: row;
    }
    .v-radio {
      margin-bottom: 0 !important;
      margin-right: 1.5em !important;
      &:only-child {
        margin-right: 0 !important;
      }
    }
  }
  .theme--light.v-radio--is-disabled .v-icon {
    color: #8f8f8f !important;
  }
  &.v-input--is-readonly {
    pointer-events: none;
    touch-action: none;
  }
  .a-radio__text {
    @include fontSize('small');
  }
}
</style>
<style lang="scss" scoped>
.a-radio__text {
  color: $c-black;
  // font-size: 0.875rem;
  &.is-hide {
    display: none;
  }
}
.a-radio__error-text {
  margin-top: -6px !important;
}
</style>
