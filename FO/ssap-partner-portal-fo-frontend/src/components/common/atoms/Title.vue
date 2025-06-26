<template>
  <compnent :is="hx" class="a-title" v-bind="attributes" v-on="$listeners">
    <slot />
  </compnent>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  titleの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  detail: {
    class: 'font-size-large font-weight-bold mb-7',
  },
  aside: {
    class: 'a-title--aside',
  },
  contactSub: {
    class: 'font-size-normal',
  },
  subTitle: {
    class: 'a-title--sub-title',
  },
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    hx: {
      type: String,
      default: 'h2',
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.a-title--header {
  @include fontSize($size: 'large');
  color: $c-primary-dark;
  font-weight: bold;
  padding-bottom: 12px;
  border-bottom: 3px solid $c-primary-dark;
}
.a-title--aside {
  @include fontSize($size: 'small');
  font-weight: bold;
  padding-bottom: 12px;
  border-bottom: 3px solid $c-primary-dark;
}
.a-title--sub-title {
  @include fontSize($size: 'small');
  font-weight: bold;
}
</style>
<style lang="scss">
.a-title {
  &.type5 {
    font-size: 1rem;
    padding-left: 10px;
    height: auto;
    border-left: 6px solid $c-primary-dark !important;
  }
}
</style>
