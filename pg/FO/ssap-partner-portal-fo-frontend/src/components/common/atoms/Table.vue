<template>
  <table class="a-table" v-bind="attributes" v-on="$listeners">
    <thead class="a-table__header">
      <slot name="header" />
    </thead>
    <tbody class="a-table__body">
      <slot name="body" />
    </tbody>
  </table>
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
  default: {
    class: 'full',
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
.a-table {
  border-collapse: collapse;
  border-spacing: 0;
  border-top: 1px solid $c-gray-line;
  border-left: 1px solid $c-gray-line;
  @include fontSize('xsmall');
  thead {
    th {
      &.is-emphasize {
        background-color: $c-primary-dark;
        color: $c-white;
        font-weight: bold;
      }
    }
  }
  tbody {
    th {
      &.is-emphasize {
        background-color: $c-primary-light;
      }
    }
  }
  th,
  td {
    border-bottom: 1px solid $c-gray-line;
    border-right: 1px solid $c-gray-line;
    padding: 4px;
    &.is-limit-width {
      width: 70px;
    }
  }
  th {
    text-align: left;
    background-color: #ebf7ed;
    font-weight: normal;
    &.is-emphasize {
      background-color: $c-primary-dark;
      color: $c-white;
      font-weight: bold;
    }
    &:first-child {
      width: 190px;
    }
  }
  td {
    &.is-emphasize {
      background-color: $c-primary-light;
    }
  }
  &.full {
    width: 100%;
  }
  &.stretch {
    th {
      width: auto !important;
    }
  }
  span {
    white-space: pre-wrap;
  }
  &.report {
    th,
    td {
      border: 0;
      font-weight: bold;
      padding-left: 18px;
    }
    th {
      background-color: #333;
      color: $c-white;
    }
    .a-table__header {
      @include fontSize('xsmall');
      th {
        padding-top: 8px;
        padding-bottom: 8px;
      }
    }
    .a-table__body {
      @include fontSize('large');
      th {
        width: 300px;
        padding-bottom: 35px;
      }
      td {
        text-align: left;
        padding-top: 8px;
        padding-bottom: 8px;
        &:last-child {
          width: 400px;
        }
      }
      tr {
        &:nth-child(even) {
          td {
            background-color: #f7f7f7;
          }
        }
      }
    }
  }
}
.a-table__header {
}
.a-table__body {
  td {
    text-align: right;
  }
  th {
  }
  .is-radio {
    th,
    td {
      background-color: $c-black-table-bg;
    }
    td {
      text-align: center;
    }
  }
}
</style>
