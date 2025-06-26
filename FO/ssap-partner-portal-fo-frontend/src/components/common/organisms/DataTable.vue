<template>
  <!-- TODO slotsを貫通してv-data-tableに適用できるようにする -->
  <v-data-table
    class="o-data-table elevation-1"
    :headers="headers"
    :items="items"
    :no-data-text="$t('common.label.noData')"
    :loading-text="$t('common.label.loading')"
    v-bind="attributes"
    v-on="$listeners"
  >
    <template #top>
      <slot name="top" />
    </template>

    <template #default>
      <slot name="default" />
    </template>

    <template #foot>
      <slot name="foot" />
    </template>

    <template #[`item.name`]="{ item }">
      <a class="o-data-table__link" target="_blank" :href="item.link">
        {{ item.name }}
      </a>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  v-data-tableの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    headers: {
      type: Array,
      required: true,
    },
    items: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss">
.o-data-table {
  // $c-black-80
  overflow: hidden;
  .v-data-table-header {
    th,
    td {
      background-color: $c-black-80;
      color: $c-white !important;
    }
    .v-icon {
      color: $c-gray-line-dark !important;
      margin-left: 5px !important;
      opacity: 1;
    }
    th {
      &.active {
        .v-icon {
          color: $c-primary !important;
        }
      }
    }
  }
}
.o-data-table__link {
  color: $c-primary-dark;
  font-weight: bold;
  &:hover,
  &:focus {
    color: $c-primary-over;
  }
}
</style>
