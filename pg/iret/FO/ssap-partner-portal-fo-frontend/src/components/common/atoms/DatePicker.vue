<template>
  <v-date-picker
    v-if="!month"
    class="a-date-picker"
    :day-format="(date) => new Date(date).getDate()"
    :allowed-dates="allowedDates"
    :min="minDate"
    :max="maxDate"
    :active-picker.sync="localActivePicker"
    v-bind="attributes"
    v-on="$listeners"
  />
  <!-- 月選択の時 -->
  <v-date-picker
    v-else
    class="a-date-picker"
    :day-format="(date) => new Date(date).getDate()"
    :allowed-dates="allowedDates"
    v-bind="attributes"
    :min="minDate"
    :max="maxDate"
    :active-picker.sync="localActivePicker"
    type="month"
    v-on="$listeners"
  />
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-date-pickerの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    locale: 'ja',
    color: 'btn_primary',
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
    allowedDates: {
      type: Function,
      required: false,
    },
    month: {
      type: Boolean,
      default: false,
    },
    minDate: {
      type: String,
      default: null,
    },
    maxDate: {
      type: String,
      default: null,
    },
    activePicker: {
      // 追加
      type: String,
      default: 'YEAR',
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
      localActivePicker: 'YEAR',
    }
  },
  watch: {
    // 親からの変更を反映
    activePicker(newVal) {
      this.localActivePicker = newVal
    },
  },
})
</script>

<style lang="scss">
.a-date-picker {
  .v-btn {
    &:hover,
    &:focus {
      &::before {
        color: $c-primary-8;
        opacity: 1;
      }
    }
  }
}
</style>
