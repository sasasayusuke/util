<template>
  <v-card class="a-card" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-card>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-cardの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/cards/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  hover: {
    class: 'a-card--hover',
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
.a-card {
  overflow: hidden;
  .v-ripple__container,
  &::before {
    display: none !important;
  }
}
.a-card--hover {
  &:hover,
  &:focus {
    background-color: $c-primary-8;
  }
}
</style>
<style lang="scss">
.a-card {
  .v-ripple__container,
  &::before {
    display: none !important;
  }
}
</style>
