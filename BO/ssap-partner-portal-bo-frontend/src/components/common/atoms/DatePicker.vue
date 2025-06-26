<template>
  <v-date-picker
    v-if="!month"
    class="a-date-picker"
    :day-format="(date) => new Date(date).getDate()"
    v-bind="attributes"
    v-on="$listeners"
  />
  <!-- 月選択の時 -->
  <v-date-picker
    v-else
    class="a-date-picker"
    :day-format="(date) => new Date(date).getDate()"
    v-bind="attributes"
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

  VuetifyLink
  https://vuetifyjs.com/ja/components/date-pickers/
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
    month: {
      type: Boolean,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
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
