<template>
  <table class="m-addable-rows">
    <slot name="staticRows" />
    <template v-if="rowObjects.length">
      <AddableRow
        v-for="(rowObject, index) in rowObjects"
        :key="index"
        :is-last-row="index === rowObjects.length - 1"
        :is-addable="isAddable"
        :category-name="categoryName"
        :row-length="rowObjects.length"
        @click:remove="remove(index)"
        @click:add="add()"
      >
        <template v-if="!optionGroup">
          <th>
            {{ !index ? categoryName : '&nbsp;' }}
          </th>
          <td>{{ rowObject.name }}</td>
        </template>
        <slot name="default" :index="index" />
      </AddableRow>
    </template>
    <template v-else>
      <AddableRow
        :is-last-row="true"
        :is-addable="isAddable"
        :category-name="categoryName"
        :row-length="rowObjects.length"
        @click:remove="remove(index)"
        @click:add="add()"
      >
        <template v-if="!optionGroup">
          <th>{{ categoryName }}</th>
          <td>&nbsp;</td>
        </template>
        <slot name="default" />
      </AddableRow>
    </template>
  </table>
</template>

<script lang="ts">
import AddableRow from './AddableRow.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    AddableRow,
  },
  props: {
    categoryName: {
      type: String,
      default: '',
    },
    rowObjects: {
      type: Array,
    },
    maxRows: {
      type: Number,
    },
    optionGroup: {
      type: Boolean,
    },
  },
  computed: {
    isAddable() {
      return this.rowObjects.length < this.maxRows
    },
  },
  methods: {
    remove(index: Number) {
      this.$emit('remove', index) // TODO: 受け取った配列から特定オブジェクトを削除処理をこっちに移行できる？要検討
    },
    add() {
      this.$emit('add')
    },
  },
})
</script>

<style lang="scss">
.m-addable-rows {
  border-spacing: 0;
  border-collapse: collapse;
  th,
  td {
    padding-bottom: 12px;
  }
  tr {
    &:last-child {
      th,
      td {
        padding-bottom: 0;
      }
    }
  }
  th {
    text-align: left;
    padding-right: 24px;
    padding-left: 0;
    width: 150px;
  }
}
.m-addable-rows__edit {
  padding-left: 24px;
  text-align: left;
}
</style>
