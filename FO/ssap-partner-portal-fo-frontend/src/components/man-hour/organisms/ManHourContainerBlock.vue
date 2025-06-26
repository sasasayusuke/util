<template>
  <div class="px-8 pt-8 pb-0 ma-0">
    <div class="pb-2 d-flex align-center">
      <h3 slot="title" class="a-table-title">
        {{ $t('man-hour.tables.' + type + '.title') }}
      </h3>
      <ToolTips v-if="$t('man-hour.tables.' + type + '.tooltip')">
        {{ $t('man-hour.tables.' + type + '.tooltip') }}
      </ToolTips>
    </div>
    <MonHourSimpleTable
      v-if="type"
      v-model="isValid"
      :man-hour-data="manHour[type]"
      :suggest-customers="suggestCustomers"
      :type="type"
      :is-editing="isEditing"
      @change="onChange"
      @input="$listeners['input']"
    />
    <ManHourSum>
      <template v-if="type === 'salesSupportManHours'" #new>{{
        calcNewSum()
      }}</template>
      <template v-if="type === 'salesSupportManHours'" #continuation>{{
        calcContinuationSum()
      }}</template>
      <template #sum="{}">{{ calcSum }}</template>
    </ManHourSum>
  </div>
</template>

<script lang="ts">
import { ToolTips } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import MonHourSimpleTable from '~/components/man-hour/molecules/ManHourSimpleTable.vue'
import ManHourSum from '~/components/man-hour/molecules/ManHourSum.vue'
export default BaseComponent.extend({
  components: {
    MonHourSimpleTable,
    ManHourSum,
    ToolTips,
  },
  model: {
    prop: 'isValid',
    event: 'input',
  },
  props: {
    /**
     * 支援工数データ
     */
    manHour: {
      type: Object,
      default: {},
    },
    /**
     * 支援工数データの種類
     */
    type: {
      type: String,
      default: '',
      required: true,
    },
    /**
     * 編集中か否か
     */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /**
     * サジェストする顧客情報
     */
    suggestCustomers: {
      type: Array,
    },
    /**
     * データが有効か否か
     */
    isValid: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localSum: 0,
      localNewSum: 0,
      localContinuationSum: 0,
    }
  },
  methods: {
    /**
     * 新規合計値を計算する
     * @returns 新規合計値
     */
    calcNewSum(): number {
      if (this.isEditing) {
        return this.localNewSum
      } else {
        const result = this.manHour[this.type].items.reduce(
          (sum: number, ele: any) => {
            if (ele.type === 'new') {
              return sum + ele.inputManHour
            } else {
              return sum
            }
          },
          0
        )
        return result
      }
    },
    /**
     * 継続合計値を計算する
     * @returns 継続合計値
     */
    calcContinuationSum(): number {
      if (this.isEditing) {
        return this.localContinuationSum
      } else {
        const result = this.manHour[this.type].items.reduce(
          (sum: number, ele: any) => {
            if (ele.type !== 'new') {
              return sum + ele.inputManHour
            } else {
              return sum
            }
          },
          0
        )
        return result
      }
    },
    /**
     * それぞれの合計値を計算する
     * @param 支援工数データ
     * @returns それぞれの合計値
     */
    localCalcSum(localManHourData: any) {
      let resultSum = 0
      let resultNewSum = 0
      let resultContinuationSum = 0
      switch (this.type) {
        case 'salesSupportManHours':
          resultNewSum = localManHourData.items.reduce(
            (sum: number, ele: any) => {
              return ele.type === 'new' ? sum + ele.inputManHour : sum
            },
            0
          )
          resultContinuationSum = localManHourData.items.reduce(
            (sum: number, ele: any) => {
              return ele.type !== 'new' ? sum + ele.inputManHour : sum
            },
            0
          )
        /* falls through */
        case 'directSupportManHours':
        case 'preSupportManHours':
          if (localManHourData.items) {
            resultSum = localManHourData.items.reduce(
              (sum: number, ele: any) => {
                return ele.inputManHour ? sum + ele.inputManHour : sum
              },
              0
            )
          }
          break
        case 'ssapManHours':
        case 'holidaysManHours':
          Object.keys(localManHourData).forEach((key) => {
            if (key !== 'memo' && localManHourData[key]) {
              resultSum += localManHourData[key]
            }
          })
          break
      }
      this.localSum = resultSum
      this.localNewSum = resultNewSum
      this.localContinuationSum = resultContinuationSum
      return this.localSum
    },
    /**
     * 入力値に変更があった場合の動作
     */
    onChange(localManHourData: any) {
      const localManHourDataWithSum = {
        type: this.type,
        localManHourData,
        sum: this.localCalcSum(localManHourData),
      }
      this.$emit('change', localManHourDataWithSum)
    },
  },
  computed: {
    /**
     * 時間の合計値を計算する
     * @returns 時間の合計値
     */
    calcSum(): number {
      if (this.isEditing) {
        return this.localSum
      } else if (this.type === 'directSupportManHours') {
        return this.manHour.summaryManHour.direct
      } else if (this.type === 'preSupportManHours') {
        return this.manHour.summaryManHour.pre
      } else if (this.type === 'salesSupportManHours') {
        return this.manHour.summaryManHour.sales
      } else if (this.type === 'ssapManHours') {
        return this.manHour.summaryManHour.ssap
      } else if (this.type === 'holidaysManHours') {
        return this.manHour.summaryManHour.others
      } else {
        return 0
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.a-table-title {
  font-size: 1.125rem;
}
</style>
