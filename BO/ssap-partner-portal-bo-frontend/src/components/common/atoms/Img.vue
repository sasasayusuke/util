<template>
  <v-img v-bind="attributes" v-on="$listeners">
    <template #placeholder>
      <slot name="placeholder" />
    </template>
  </v-img>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-imgの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/images/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はBaseAtomComponent参照
    */
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>
