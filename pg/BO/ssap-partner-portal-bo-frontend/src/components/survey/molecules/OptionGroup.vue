<template>
  <div class="m-survey-master-option-group-table">
    <v-row class="m-survey-master-option-group-wrapper">
      <v-col class="m-survey-master-option-group-title" cols="3"
        >{{ $t('survey.pages.list_item') }}
        <div>
          <ErrorText v-if="localOptions.isNew" class="font-weight-normal">{{
            errorText
          }}</ErrorText>
        </div></v-col
      >
      <table>
        <AddableRows
          :row-objects="localOptions.group"
          :max-rows="maxRows"
          :is-addable="true"
          option-group
          @add="addRow"
          @remove="removeRow"
        >
          <template #staticRows>
            <tr>
              <th class="m-survey-master-option-group-title">
                {{ $t('survey.pages.sub_list_item.title') }}
              </th>
              <td class="pb-1">
                <Sheet width="550px">
                  <TextField
                    v-model="localOptions.description"
                    max-length="255"
                    style-set="outlined"
                    @change="onChange"
                  />
                </Sheet>
              </td>
            </tr>
          </template>

          <template #default="slotProps">
            <th class="m-survey-master-option-group-title">
              {{ $t('survey.pages.sub_list_item.option') }}
              {{ slotProps.index + 1 }}
              <Required />
            </th>
            <td>
              <Sheet width="550px">
                <TextField
                  v-model="localOptions.group[slotProps.index].title"
                  required
                  max-length="255"
                  style-set="outlined"
                  :additional-rules="additionalRules"
                  @change="onChange"
                />
              </Sheet>
            </td>
            <td>
              <Checkbox
                :id="'choice' + (slotProps.index + 1)"
                v-model="localOptions.group[slotProps.index].disabled"
                :label="$t('survey.pages.sub_list_item.check')"
                :for="'choice' + (slotProps.index + 1)"
                class="m-survey-master-option-group-table__checkbox"
                :true-value="false"
                :false-value="true"
                @change="onChange"
              />
            </td>
          </template>
        </AddableRows>
        <tr>
          <td class="m-survey-master-option-group-button pt-6">
            <template v-if="!localOptions.isNew">
              <Button v-if="disabled" style-set="enable" @click="enableGroup">
                {{ $t('common.button.enableGroup') }}
              </Button>
              <Button v-else style-set="disable" @click="disableGroup">
                {{ $t('common.button.disableGroup') }}
              </Button>
            </template>
            <template v-else>
              <template v-if="isDisabledCount">
                <Button v-if="disabled" style-set="enable" @click="enableGroup">
                  {{ $t('common.button.enableGroup') }}
                </Button>
                <Button v-else style-set="disable" @click="disableGroup">
                  {{ $t('common.button.disableGroup') }}
                </Button>
              </template>
              <template v-if="isRemovable">
                <Button style-set="remove" @click="removeGroup">
                  <Icon class="mr-1" size="20" color="error">mdi-minus</Icon>
                  {{ $t('common.button.delGroup') }}
                </Button>
              </template>
            </template>
            <Button v-if="last" style-set="add" @click="addGroup">
              <Icon class="mr-1" size="20" color="btn_primary">mdi-plus</Icon>
              {{ $t('common.button.addGroup') }}
            </Button>
          </td>
        </tr>
      </table>
    </v-row>
  </div>
</template>

<script lang="ts">
import {
  Button,
  TextField,
  Checkbox,
  Required,
  Sheet,
  Icon,
  ErrorText,
} from '../../common/atoms/index'
import AddableRows from '~/components/common/molecules/AddableRows.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { SurveyMasterChoiceGroupItem } from '~/models/Master'
const { v4: uuidv4 } = require('uuid')

export interface ILocalOptionsGroup {
  disabled: boolean
}

export default BaseComponent.extend({
  components: {
    Button,
    TextField,
    Checkbox,
    Required,
    Sheet,
    AddableRows,
    Icon,
    ErrorText,
  },
  props: {
    /** 回答選択肢配列 */
    options: {
      type: Object,
      required: true,
    },
    indexGroup: {
      type: Number,
    },
    hoge: {
      type: String,
    },
    /** 回答選択肢インデックス番号 */
    index: {
      type: Number,
    },
    /** 最後の回答選択肢か */
    last: {
      type: Boolean,
      default: false,
    },
    /** 回答選択肢が消去可能か */
    isRemovable: {
      type: Boolean,
      default: false,
    },
    /** 回答選択肢の最小数 */
    requiredCount: {
      type: Number,
      default: 1,
    },
    /** 回答選択肢の評点プレフィックスの有無を判定 */
    additionalRules: {
      type: Array as PropType<Array<Function>>,
      required: false,
      default: [],
    },
  },
  data() {
    return {
      maxRows: 20,
      localOptions: this.options, // TODO:現在のリストは多分親側で管理したほうがいいので、親に移植する
      indexChoice: '',
      multiChecked: ['choice1'],
      disabled: false,
    }
  },
  watch: {
    localOptions: {
      handler(newValue) {
        const newGroup = newValue.group
        const result = newGroup.every((group: any) => group.disabled === true)
        if (result) {
          this.disabled = true
        } else {
          this.disabled = false
        }
      },
      deep: true,
    },
  },
  computed: {
    /**
     * 回答選択肢グループの無効化が可能か判定
     * @returns 判定真偽値
     */
    isDisabledCount(): boolean {
      const group = this.localOptions.group
      const groupLength = group.length
      let disabled: number = 0
      group.forEach((elm: ILocalOptionsGroup) => {
        if (elm.disabled) {
          disabled++
        }
      })
      return disabled === groupLength
    },
    /**
     * 回答選択肢の有効数が最小数を満たしているか
     * @returns 判定真偽値
     */
    isRequiredCount(): boolean {
      const minRequiredCount = this.requiredCount
      let currentRequiredCount = 0
      if (
        this.localOptions.group &&
        this.localOptions.group.length &&
        this.localOptions.group.length > 0
      ) {
        for (const i in this.localOptions.group) {
          if (this.localOptions.group[i].disabled === false) {
            currentRequiredCount++
          }
        }
      }
      return currentRequiredCount >= minRequiredCount
    },
    /**
     * 回答選択肢の有効数が最小数を満たしていない場合にエラーを表示
     * @returns エラー文字列
     */
    errorText(): string {
      let errorText = ''
      if (!this.isRequiredCount) {
        errorText = this.$t(
          'survey.pages.revision.table.survey.row.choiceGroup.errorMessages.requiredCount',
          { count: this.requiredCount }
        ) as string
      }
      return errorText
    },
  },
  methods: {
    /**
     * 指定した回答選択肢インデックス番号の行を削除
     * @param index 回答選択肢インデックス番号
     */
    removeRow(index: number) {
      this.localOptions.group.splice(index, 1)
      this.$emit('update', this.index, this.localOptions)
    },
    /**
     * 回答選択肢の行を追加
     * @param index 回答選択肢インデックス番号
     */
    addRow(this: any) {
      const choiceGroupItem = new SurveyMasterChoiceGroupItem()
      choiceGroupItem.id = uuidv4()
      choiceGroupItem.isNew = true
      this.localOptions.group.push(choiceGroupItem)
      this.$emit('update', this.index, this.localOptions)
    },
    edit() {
      //
    },
    /** 回答選択肢グループの追加を他のコンポーネントに通知 */
    addGroup() {
      this.$emit('add-option-group')
    },
    /** 回答選択肢グループの削除を他のコンポーネントに通知 */
    removeGroup() {
      this.$emit('remove-option-group', this.index)
    },
    /** 回答選択肢グループの無効化を他のコンポーネントに通知 */
    disableGroup() {
      this.disabled = true
      this.$emit('disable-option-group', this.index)
    },
    /** 回答選択肢グループの有効化を他のコンポーネントに通知 */
    enableGroup() {
      this.disabled = false
      this.$emit('enable-option-group', this.index)
    },
    /** 回答選択肢の変更内容を他のコンポーネントに受け渡す */
    onChange(this: any) {
      this.$emit('update', this.index, this.localOptions)
    },
  },
})
</script>
<style lang="scss">
.m-survey-master-option-group-wrapper {
  border-bottom: 1px solid $c-gray-line;
  padding-bottom: 15px;
}
.m-survey-master-option-group-table {
  table {
    padding-left: 12px;
  }
  th {
    width: 130px;
  }
  td {
    padding-bottom: 0;
  }
  .v-input--selection-controls {
    margin-top: 3px;
    margin-left: 5px;
    .v-label {
      font-size: 0.875rem;
      color: $c-black;
    }
  }
  + .m-survey-master-option-group-table {
    padding-top: 36px;
  }
}
.m-survey-master-option-group-title {
  font-size: 0.875rem;
  font-weight: bold;
  padding-top: 0;
}
.m-survey-master-option-group-button {
  padding-left: 130px;
}
.m-survey-master-option-group-table__checkbox {
  position: relative;
  top: 4px;
}
.font-weight-normal {
  font-weight: normal;
}
</style>
