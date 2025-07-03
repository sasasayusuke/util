<template>
  <v-form
    ref="form"
    :value="isValid"
    class="o-master-detail-rows py-4 px-8"
    @input="$listeners['input']"
  >
    <!-- ID -->
    <CommonDetailRow
      v-if="!isCreating"
      :label="$t('master.row.id.name')"
      :value="master.id"
    />
    <!-- 種別 -->
    <CommonDetailRow
      :label="$t('master.row.dataType.name')"
      :value="formatValues.dataType"
      :is-editing="isEditing"
      required
    >
      <Sheet v-if="isCreating" width="300">
        <Select
          v-model="localParam.dataType"
          :aria-label="$t('master.row.dataType.name')"
          dense
          outlined
          :items="$t('master.row.dataType.select')"
          item-text="label"
          item-value="value"
          :placeholder="$t('master.row.dataType.placeholder')"
          required
        />
      </Sheet>
      <template v-else>
        {{ formatValues.dataType }}
      </template>
    </CommonDetailRow>
    <!-- 名称 -->
    <CommonDetailRow
      :label="$t('master.row.name.name')"
      :value="master.name"
      :is-editing="isEditing"
      required
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.name"
          :aria-label="$t('master.row.name.name')"
          outlined
          dense
          :placeholder="$t('master.row.name.placeholder')"
          required
          max-length="256"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 値 -->
    <CommonDetailRow
      :label="$t('master.row.value.name')"
      :value="master.value"
      :is-editing="isEditing"
      required
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.value"
          :aria-label="$t('master.row.value.name')"
          outlined
          dense
          :placeholder="$t('master.row.value.placeholder')"
          required
          max-length="256"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 予備情報1 -->
    <CommonDetailRow
      :label="$t('master.row.info1.name')"
      :value="master.attributes.info1"
      :is-editing="isEditing"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.attributes.info1"
          :aria-label="$t('master.row.info1.name')"
          outlined
          dense
          :placeholder="$t('master.row.info1.placeholder')"
          max-length="1024"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 予備情報2 -->
    <CommonDetailRow
      :label="$t('master.row.info2.name')"
      :value="master.attributes.info2"
      :is-editing="isEditing"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.attributes.info2"
          :aria-label="$t('master.row.info2.name')"
          outlined
          dense
          :placeholder="$t('master.row.info2.placeholder')"
          max-length="1024"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 予備情報3 -->
    <CommonDetailRow
      :label="$t('master.row.info3.name')"
      :value="master.attributes.info3"
      :is-editing="isEditing"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.attributes.info3"
          :aria-label="$t('master.row.info3.name')"
          outlined
          dense
          :placeholder="$t('master.row.info3.placeholder')"
          max-length="1024"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 予備情報4 -->
    <CommonDetailRow
      :label="$t('master.row.info4.name')"
      :value="master.attributes.info4"
      :is-editing="isEditing"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.attributes.info4"
          :aria-label="$t('master.row.info4.name')"
          outlined
          dense
          :placeholder="$t('master.row.info4.placeholder')"
          max-length="1024"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 予備情報5 -->
    <CommonDetailRow
      :label="$t('master.row.info5.name')"
      :value="master.attributes.info5"
      :is-editing="isEditing"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.attributes.info5"
          :aria-label="$t('master.row.info5.name')"
          outlined
          dense
          :placeholder="$t('master.row.info5.placeholder')"
          max-length="1024"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 登録日時 -->
    <CommonDetailRow
      v-if="!isCreating"
      :label="$t('master.row.createAt.name')"
      :value="formatDate(new Date(master.createAt))"
    />
    <!-- 更新日時 -->
    <CommonDetailRow
      v-if="!isCreating"
      :label="$t('master.row.updateAt.name')"
      :value="formatDate(new Date(master.updateAt))"
    />
    <!-- 利用フラグ -->
    <CommonDetailRow
      :label="$t('master.row.use.name')"
      :value="master.use"
      :is-editing="isEditing"
      required
    >
      <Sheet width="300">
        <RadioGroup
          v-model="localParam.use"
          :labels="$t('master.row.use.radio.label')"
          :values="$t('master.row.use.radio.values')"
          horizontal
          hide-details
        />
      </Sheet>
      <template #isNotEditing>
        {{
          master.use
            ? $t('master.row.use.radio.labels.enabled')
            : $t('master.row.use.radio.labels.disabled')
        }}
      </template>
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import {
  TextField,
  RadioGroup,
  Select,
  ToolTips,
  Sheet,
  Required,
} from '../../common/atoms/index'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import { PropType } from '~/common/BaseComponent'
// LocalMaster型はGetMasterByIdResponse型と同じ
import {
  GetMasterByIdResponse,
  GetMasterByIdResponse as LocalMaster,
} from '~/models/Master'
export { LocalMaster }

// 種別 選択肢
const SELECT_ITEMS = ['master_supporter_organization', 'master_service_type']

export default CommonDetailRows.extend({
  components: {
    TextField,
    RadioGroup,
    Select,
    ToolTips,
    Sheet,
    Required,
    CommonDetailRow,
  },
  props: {
    /**
     * GetMasterById APIから取得したマスターメンテナンス情報
     */
    master: {
      type: Object as PropType<GetMasterByIdResponse>,
      required: true,
    },
  },
  data(): {
    localParam: GetMasterByIdResponse
  } {
    return {
      localParam: cloneDeep(this.master),
    }
  },
  computed: {
    dataTypes(): object[] {
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
    formatValues(): GetMasterByIdResponse {
      const copiedValue = Object.assign(
        new GetMasterByIdResponse(),
        this.master
      )
      copiedValue.dataType = this.$t(
        `master.row.dataType.items.${copiedValue.dataType}`
      ) as string
      return copiedValue
    },
  },
  methods: {
    /**
     * 編集中のマスターメンテナンス情報をリセット
     */
    resetLocalParam() {
      this.localParam = cloneDeep(this.master)
    },
  },
})
</script>

<style lang="scss" scoped>
.o-master-detail-rows__unit {
  border-bottom: 1px solid $c-gray-line;
  margin: 0 !important;
  align-items: flex-start;
}
.o-master-detail-rows__title {
  font-size: 0.875rem;
  font-weight: bold;
  align-items: center;
  padding-top: 18px;
}
.o-master-detail-rows__data {
  align-items: center;
}
.o-master-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-master-detail-rows__select__item {
  list-style: none;
  display: inline-block;
  margin-right: 1em;
  &::after {
    content: ',';
  }
  &:last-child {
    &::after {
      display: none;
    }
  }
}
</style>
