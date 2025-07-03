<template>
  <div class="px-8 pt-8 pb-0 ma-0">
    <div class="pb-2 d-flex align-center">
      <Title slot="title" hx="h3" style-set="large">{{
        $t('man-hour.tables.' + type + '.title')
      }}</Title>
      <ToolTips v-if="$t('man-hour.tables.' + type + '.tooltip')">
        {{ $t('man-hour.tables.' + type + '.tooltip') }}
      </ToolTips>
    </div>
    <!-- 各要素一覧テーブル -->
    <ManHourSimpleTable
      v-if="type"
      :man-hour-data="manHourBySupporterUserId[type]"
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :is-confirm="manHourBySupporterUserId.isConfirm"
      :type="type"
      @change="onChange"
    />
    <!-- 実績工数合計時間 -->
    <ManHourSum>
      <template v-if="type === 'salesSupportManHours'" #new>{{
        calcNewSum()
      }}</template>
      <template v-if="type === 'salesSupportManHours'" #continuation>{{
        calcContinuationSum()
      }}</template>
      <template #sum>{{ calcSum() }}</template>
    </ManHourSum>
  </div>
</template>

<script lang="ts">
import { ToolTips, Title } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import ManHourSimpleTable from '~/components/man-hour/molecules/ManHourSimpleTable.vue'
import ManHourSum from '~/components/man-hour/molecules/ManHourSum.vue'
import Modal from '~/components/common/molecules/Modal.vue'
export default BaseComponent.extend({
  name: 'ManHourContainerBlock',
  components: {
    ManHourSimpleTable,
    ManHourSum,
    ToolTips,
    Title,
    Modal,
  },
  props: {
    /** 支援者単位での支援工数情報 */
    manHourBySupporterUserId: {
      type: Object,
      default: {},
    },
    /** 支援者単位での支援工数情報の表示項目
     * type : 項目名
     * directSupportManHours : 1．対面支援：お客様に対する支援（直接）
     * preSupportManHours :  2．支援仕込：お客様に対する支援（仕込）
     * salesSupportManHours : 3．商談/提案準備：新規・継続案件獲得に向けた資料作成・調査
     * ssapManHours : 4．内部業務：お客様を特定せずSony Acceleration Platformの業務で発生した工数
     * holidaysManHours : 5．休憩・その他：有給休暇・休憩・雑務・その他Sony Acceleration Platformの業務以外で使用した工数
     */
    type: {
      type: String,
      default: '',
      required: true,
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
     * 分類が新規の合計を計算
     * @return 分類が新規の合計
     */
    calcNewSum() {
      const result = this.manHourBySupporterUserId[this.type].items.reduce(
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
    },
    /**
     * 分類が継続の合計を計算
     * @return 分類が継続の合計
     */
    calcContinuationSum() {
      const result = this.manHourBySupporterUserId[this.type].items.reduce(
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
    },
    /**
     * 項目に対応した合計を選択
     * @return 項目に対応した合計
     */
    calcSum() {
      if (this.type === 'directSupportManHours') {
        return this.manHourBySupporterUserId.summaryManHour.direct
      } else if (this.type === 'preSupportManHours') {
        return this.manHourBySupporterUserId.summaryManHour.pre
      } else if (this.type === 'salesSupportManHours') {
        return this.manHourBySupporterUserId.summaryManHour.sales
      } else if (this.type === 'ssapManHours') {
        return this.manHourBySupporterUserId.summaryManHour.ssap
      } else if (this.type === 'holidaysManHours') {
        return this.manHourBySupporterUserId.summaryManHour.others
      } else {
        return 0
      }
    },
    /**
     * localDataに合計を計算
     * @return 計算された合計
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
     * localDataに項目と合計を入れ返す
     * @return 項目と合計が入ったlocalData
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
})
</script>
