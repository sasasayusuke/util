<template>
  <v-sheet class="a-sheet" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-sheet>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-sheet
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/sheets/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    color: 'transparent',
  },
  outlined: {
    color: undefined,
    outlined: true,
  },
  infomation: {
    color: '',
    class: 'a-sheet--infomation',
  },
  'text-box': {
    color: '#f0f0f0',
    rounded: true,
    class: 'pa-3',
  },
  'text-box2': {
    color: '#f0f0f0',
    rounded: true,
    class: 'pa-5',
  },
  'no-editing': {
    color: '#fafafa',
    class: 'a-sheet--no-editing pa-3',
    rounded: true,
  },
  error: {
    class: 'pa-5',
    color: '#faeded',
    rounded: true,
  },
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.a-sheet--infomation {
  border: 1px solid $c-gray-line;
  border-left: 3px solid $c-primary-dark;
  border-radius: 4px;
  padding: 20px 24px;
}
.a-sheet--no-editing {
  border: 1px solid #8f8f8f !important;
}
</style>
