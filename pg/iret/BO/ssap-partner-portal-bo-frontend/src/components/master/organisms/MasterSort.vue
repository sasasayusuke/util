<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <MasterSortInput :param="param" :data-types="dataTypes" v-on="$listeners" />
  </CommonSort>
</template>

<script lang="ts">
import MasterSortInput from '~/components/master/molecules/MasterSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

// 種別 選択肢
const SELECT_ITEMS = [
  'all',
  'master_supporter_organization',
  'master_service_type',
]

export class SelectedDataTypeParam {
  dataType = 'all'
}

export default BaseComponent.extend({
  name: 'MasterSort',
  components: {
    CommonSort,
    MasterSortInput,
  },
  props: {
    /**
     * パラメータ
     */
    param: {
      type: Object,
      required: true,
    },
  },
  computed: {
    /**
     * データ種別のリストを返す
     */
    dataTypes() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }

      SELECT_ITEMS.forEach((elm) => {
        selectItem.label = this.$t('master.row.dataType.items.' + elm) as string
        selectItem.value = elm
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      })
      return selectItems
    },
  },
})
</script>
