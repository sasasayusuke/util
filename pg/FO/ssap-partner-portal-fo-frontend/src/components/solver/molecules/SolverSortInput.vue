<template>
  <v-container pr-7>
    <v-row>
      <!-- 個人ソルバー名 -->
      <p class="font-size-small mb-0 mr-2 mt-2">
        {{ $t('solver.pages.list.sort_input.name') }}
      </p>
      <Sheet width="200" mr-7>
        <TextField
          :value="param.name"
          :max-length="120"
          :aria-label="$t('solver.pages.list.sort_input.name')"
          outlined
          dense
          @input="$emit('update', 'name', $event)"
        />
      </Sheet>
      <!-- 性別 -->
      <p class="font-size-small mb-0 mr-2 ml-7 mt-2">
        {{ $t('solver.pages.list.sort_input.sex.name') }}
      </p>
      <Sheet width="100" mr-7>
        <Select
          v-model="param.sex"
          :aria-label="$t('solver.pages.list.sort_input.sex.name')"
          outlined
          dense
          :items="selectItemsForSex"
          item-text="label"
          item-value="value"
          style-set="bgWhite"
        />
      </Sheet>
      <!-- 稼働状況 -->
      <p class="font-size-small mb-0 mr-2 ml-7 mt-2">
        {{ $t('solver.pages.list.sort_input.operatingStatus.name') }}
      </p>
      <Sheet width="100" mr-7>
        <Select
          v-model="param.operatingStatus"
          :aria-label="$t('solver.pages.list.sort_input.operatingStatus.name')"
          outlined
          dense
          :items="selectItemsForOperatingStatus"
          item-text="label"
          item-value="value"
          style-set="bgWhite"
        />
      </Sheet>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { TextField, Sheet, Select } from '~/components/common/atoms'
// 性別 選択肢
const SELECT_ITEMS_FOR_SEX = ['all', 'man', 'woman', 'not_set']
// 稼働状況 選択肢
const SELECT_ITEMS_FOR_STATUS = ['all', 'not_working', 'working', 'inactive']
export default BaseComponent.extend({
  name: 'SolverSortInput',
  components: {
    TextField,
    Sheet,
    Select,
  },
  props: {
    /** ソルバー候補検索用のパラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** ページがリロード中か否か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    // 性別セレクトの選択肢を生成
    selectItemsForSex() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }
      SELECT_ITEMS_FOR_SEX.forEach((elm) => {
        selectItem.label = this.$t(
          'solver.pages.list.sort_input.sex.items.' + elm
        ) as string
        selectItem.value = elm
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      })
      return selectItems
    },
    // 稼働状況セレクトの選択肢を生成
    selectItemsForOperatingStatus() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }
      SELECT_ITEMS_FOR_STATUS.forEach((elm) => {
        selectItem.label = this.$t(
          'solver.pages.list.sort_input.operatingStatus.items.' + elm
        ) as string
        selectItem.value = elm
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      })
      return selectItems
    },
  },
})
</script>
