<!-- TODO textfieldやdatepickerの属性を流し込めるようにする -->

<template>
  <v-menu min-width="auto" v-bind="attributes" :value="menu">
    <template #activator="{ on, attrs }">
      <TextField
        :value="value"
        label="Birthday date"
        prepend-icon="mdi-calendar"
        readonly
        v-bind="attrs"
        v-on="on"
      />
    </template>
    <DatePicker :value="value" v-on="$listeners" />
  </v-menu>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import { DatePicker, TextField } from '../atoms/index'

/*
  v-btnの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  // v-model用
  model: {
    prop: 'value',
    event: 'input',
  },
  components: {
    TextField,
    DatePicker,
  },
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
    value: {
      type: String,
    },
    menu: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style></style>
