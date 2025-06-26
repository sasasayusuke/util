<template>
  <v-tooltip class="a-tooltip" v-bind="attributes" v-on="$listeners">
    <template #activator="{ on, attrs }">
      <div v-bind="attrs" v-on="on">
        <slot name="button">
          <v-icon color="#333"> mdi-help-circle </v-icon>
        </slot>
      </div>
    </template>
    <span>
      <slot />
    </span>
  </v-tooltip>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-tooltipの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    top: true,
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

<style lang="scss" scoped></style>
