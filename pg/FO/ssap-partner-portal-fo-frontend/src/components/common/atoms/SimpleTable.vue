<template>
  <v-simple-table :class="className" v-bind="attributes" v-on="$listeners">
    <slot />
  </v-simple-table>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-simple-tableの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  props: {
    type: {
      type: String,
      required: false,
    },
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
  computed: {
    className() {
      return this.type ? `a-simple-table--${this.type}` : 'a-simple-table'
    },
  },
})
</script>

<style lang="scss">
.a-simple-table {
  th,
  td {
    height: 62px !important;
  }
  &--manHour {
    th {
      height: 32px !important;
      background-color: $c-black-80;
      color: $c-white !important;
      font-weight: bold;
      span {
        color: #fc0f1a !important;
      }
    }
  }
  &.v-data-table {
    .v-data-table__wrapper {
      table {
        tbody {
          tr {
            transition-property: background-color;
            transition-duration: 0.2s;
            &:hover,
            &:focus {
              &:not(.v-data-table__expanded__content) {
                &:not(.v-data-table__empty-wrapper) {
                  background-color: $c-primary-8 !important;
                  cursor: pointer;
                }
              }
            }
          }
        }
      }
    }
  }
}
</style>
