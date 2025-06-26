<template>
  <v-row>
    <v-col>
      <v-container>
        <OptionGroup
          v-for="(localOptionGroup, indexGroup) in localOptionGroups"
          :key="indexGroup"
          v-model="isValid"
          :options="localOptionGroup"
          :index="indexGroup"
          :only="localOptionGroups.length === 1"
          :last="localOptionGroups.length === indexGroup + 1"
          :additional-rules="additionalRules"
          :is-removable="localOptionGroups.length > 1"
          :required-count="requiredCount"
          @add-option-group="addOptionGroup"
          @remove-option-group="removeOptionGroup"
          @disable-option-group="disableOptionGroup"
          @enable-option-group="enableOptionGroup"
          @update="update"
        />
      </v-container>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Button } from '../../common/atoms/index'
import OptionGroup from './OptionGroup.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import {
  SurveyMasterChoiceGroupItem,
  SurveyMasterChoiceItem,
} from '~/models/Master'
const { v4: uuidv4 } = require('uuid')

export default BaseComponent.extend({
  components: {
    Button,
    OptionGroup,
  },
  props: {
    /** 回答選択肢配列 */
    optionGroups: {
      type: Array,
      required: true,
    },
    /** 設問インデックス番号 */
    index: {
      type: Number,
    },
    /** 最小選択肢数 */
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
      maxRows: 5,
      localOptionGroups: this.optionGroups, // TODO:現在のリストは多分親側で管理したほうがいいので、親に移植する
      isValid: false,
    }
  },
  methods: {
    /**
     * 他コンポーネントへ回答選択肢の変更を受け渡す
     * @param index 対象設問インデックス番号
     * @param localOptions 回答選択肢配列
     */
    update(index: number, localOptions: any) {
      this.localOptionGroups[index] = localOptions
      this.$emit('update', this.index, this.localOptionGroups)
    },
    /**
     * 回答選択肢を消去
     * @param index 対象設問インデックス番号
     */
    removeOptionGroup(index: number) {
      this.localOptionGroups.splice(index, 1)
      this.$emit('update', this.index, this.localOptionGroups)
    },
    /**
     * 回答選択肢を無効化
     * @param index 対象設問インデックス番号
     */
    disableOptionGroup(index: number): void {
      const targetGroups = (this.localOptionGroups[index] as any).group
      for (let i = 0; i < targetGroups.length; i++) {
        ;(this.localOptionGroups[index] as any).group[i].disabled = true
      }
      this.$emit('update', this.index, this.localOptionGroups)
    },
    /**
     * 回答選択肢を有効化
     * @param index 対象設問インデックス番号
     */
    enableOptionGroup(index: number): void {
      const targetGroups = (this.localOptionGroups[index] as any).group
      for (let i = 0; i < targetGroups.length; i++) {
        ;(this.localOptionGroups[index] as any).group[i].disabled = false
      }
      this.$emit('update', this.index, this.localOptionGroups)
    },
    /** 回答選択肢を追加 */
    addOptionGroup() {
      const newChoiceItem = new SurveyMasterChoiceItem()
      newChoiceItem.isNew = true
      // 回答タイプの最小選択肢数分の選択肢を作成
      for (let i = 0; i < this.requiredCount; i++) {
        const newChoiceGroupItem = new SurveyMasterChoiceGroupItem()
        newChoiceGroupItem.id = uuidv4()
        newChoiceGroupItem.isNew = true
        newChoiceItem.group.push(newChoiceGroupItem)
      }
      this.localOptionGroups.push(newChoiceItem)
      this.$emit('update', this.index, this.localOptionGroups)
    },
  },
})
</script>
