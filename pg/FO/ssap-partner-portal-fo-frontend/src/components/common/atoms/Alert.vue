<template>
  <v-alert class="a-alert" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-alert>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-alertの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される

  VuetifyLink
  https://vuetifyjs.com/ja/components/alerts/
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  error: {
    icon: 'icon-org-alert',
    type: 'error',
    dense: true,
    outlined: true,
    dismissible: true,
  },
  no_data: {
    color: '#f0f0f0',
    tile: true,
    class: 'is-no-data',
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

<style lang="scss">
$alert-icon-size: 18px;
.a-alert {
  .v-alert__icon {
    &.v-icon {
      width: $alert-icon-size;
      min-width: 0;
      font-size: $alert-icon-size;
      margin-right: 10px;
    }
  }
  &.v-alert--outlined {
    background-color: $c-white !important;
  }
  &.is-no-data {
    .v-alert__content {
      text-align: center;
      color: #8a8a8a;
    }
  }
}
</style>
