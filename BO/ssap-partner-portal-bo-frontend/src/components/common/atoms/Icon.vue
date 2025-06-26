<template>
  <v-icon v-bind="attributes" v-on="$listeners">
    <slot />
  </v-icon>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/icons/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  primary: {
    color: '#008a19',
  },
  secondary: {
    color: '#1867c0',
  },
  tertiary: {
    color: '#666',
  },
  error: {
    color: '#d53030',
  },
  disabled: {
    color: '#ccc',
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
.is-rotate-180 {
  transform: rotate(180deg);
  transition-duration: 0.3s;
}
</style>
