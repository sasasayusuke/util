<template>
  <v-list
    class="a-list"
    :no-data-text="$t('common.label.noData')"
    v-bind="attributes"
    v-on="$listeners"
  >
    <template v-for="(value, index) in values">
      <v-list-item :key="index" v-bind="listVBind" :to="links[index]">
        {{ value }}
      </v-list-item>
    </template>
  </v-list>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-listの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/lists/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

// v-listに適用されるATTRIBUTE_SET
const LIST_ATTRIBUTE_SET: AttributeSet = {
  default: {
    ripple: false,
  },
}

export default WrapperComponent.extend({
  // v-model用
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    // 各listをユーザーが指定した時に返す値
    values: {
      type: Array,
      required: true,
    },
    // listに割り振られるラベル(指定がない場合、valuesがそのままlabelとなる)
    links: {
      type: Array,
    },
    listStyleSet: {
      type: String,
      default: '',
    },
    listAttributes: {
      type: Object,
      default() {
        return {}
      },
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
  computed: {
    listVBind() {
      return this.generateAttributes(
        LIST_ATTRIBUTE_SET,
        this.listStyleSet,
        this.listAttributes
      )
    },
  },
})
</script>

<style lang="scss" scoped>
.a-list {
  &.v-list {
    @include list-item;
  }
}
</style>
