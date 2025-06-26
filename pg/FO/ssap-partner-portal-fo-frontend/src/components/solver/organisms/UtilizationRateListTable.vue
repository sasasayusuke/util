<template>
  <div style="padding-top: 20px">
    <CommonDataTable
      :headers="utilizationRateHeaders"
      :items="formattedSolvers"
      :total="total"
      :is-loading="isLoading"
      :shows-pagination="false"
      :short-text="true"
      @sort="sort"
      @changeInput="changeInput"
      @handle-error="handleError"
    />
  </div>
</template>

<script lang="ts">
import { cloneDeep, isEqual } from 'lodash'
import CommonDataTable, {
  IDataTableHeader,
} from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { TextLink } from '~/components/common/atoms/index'
import { IFormattedSolver, ISolver } from '~/types/Solver'

export default BaseComponent.extend({
  name: 'UtilizationRateListTable',
  components: {
    CommonDataTable,
    TextLink,
  },
  props: {
    /** ソルバー一覧 */
    solvers: {
      type: Array as PropType<ISolver[]>,
      required: true,
    },
    /** ソルバー一覧総件数 */
    total: {
      type: Number,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): {
    utilizationRateHeaders: IDataTableHeader[]
    // 変更前の一覧データ保存用
    originalFormattedSolvers: IFormattedSolver[]
  } {
    return {
      utilizationRateHeaders: [
        {
          text: this.$t('solver.pages.utilizationRate.table.header.name'),
          value: 'name',
          maxLength: 10,
          sortable: false,
          link: {
            prefix: '/solver/',
            value: 'id',
            target: '_blank',
          },
          width: '165px',
        },
        {
          text: this.$t(
            'solver.pages.utilizationRate.table.header.providedOperatingRate'
          ),
          value: 'providedOperatingRate',
          sortable: false,
          maxLength: 3,
          textBox: {
            type: 'single',
            unit: this.$t(
              'solver.pages.utilizationRate.table.value.unit1'
            ) as string,
            positiveNumber: true,
            rangeNumberTo: 100,
          },
          width: '140px',
        },
        {
          text: this.$t(
            'solver.pages.utilizationRate.table.header.providedOperatingRateNext'
          ),
          value: 'providedOperatingRateNext',
          sortable: false,
          maxLength: 3,
          textBox: {
            type: 'single',
            unit: this.$t(
              'solver.pages.utilizationRate.table.value.unit1'
            ) as string,
            positiveNumber: true,
            rangeNumberTo: 100,
          },
          width: '140px',
        },
        {
          text: this.$t(
            'solver.pages.utilizationRate.table.header.operationProspectsMonthAfterNext'
          ),
          value: 'operationProspectsMonthAfterNext',
          sortable: false,
          maxLength: 20,
          textBox: {
            type: 'text',
            positiveNumber: false,
          },
          width: '280px',
        },
        {
          text: this.$t(
            'solver.pages.utilizationRate.table.header.pricePerPersonMonth'
          ),
          value: 'pricePerPersonMonth',
          sortable: false,
          maxLength: 7,
          textBox: {
            type: 'double',
            unit: this.$t(
              'solver.pages.utilizationRate.table.value.unit2'
            ) as string,
            positiveNumber: true,
          },
          width: '265px',
        },
        {
          text: this.$t('solver.pages.utilizationRate.table.header.hourlyRate'),
          value: 'hourlyRate',
          sortable: false,
          maxLength: 5,
          textBox: {
            type: 'double',
            unit: this.$t(
              'solver.pages.utilizationRate.table.value.unit2'
            ) as string,
            positiveNumber: true,
          },
          width: '265px',
        },
        {
          text: this.$t(
            'solver.pages.utilizationRate.table.header.priceAndOperatingRateUpdateAt'
          ),
          value: 'priceAndOperatingRateUpdateAt',
          sortable: true,
          width: '140px',
        },
      ],
      originalFormattedSolvers: [],
    }
  },
  computed: {
    /** ソルバー一覧をフォーマット */
    formattedSolvers(): IFormattedSolver[] {
      const formattedSolvers = this.solvers.map((solver) => {
        const { pricePerPersonMonthLower, hourlyRateLower, ...solvers } = solver
        const formattedSolver = {
          ...solvers,
          // 稼働率（下限〜上限）
          pricePerPersonMonth: {
            value1: pricePerPersonMonthLower || 0,
            value2: solvers.pricePerPersonMonth || 0,
          },
          // 時間単価（下限〜上限）
          hourlyRate: {
            value1: hourlyRateLower || 0,
            value2: solvers.hourlyRate || 0,
          },
          // 最終更新日時
          priceAndOperatingRateUpdateAt: this.formatDate(
            new Date(solvers.priceAndOperatingRateUpdateAt || ''),
            'Y/MM/DD hh:mm'
          ),
        }
        return formattedSolver
      })

      this.copyFormattedData(formattedSolvers)
      return formattedSolvers
    },
  },
  methods: {
    // ソートイベントを親コンポーネントに送る
    sort(event: any): void {
      this.$emit('sort', event)
    },
    // 変更前の一覧の情報をコピー
    copyFormattedData(formattedSolver: any) {
      this.originalFormattedSolvers = cloneDeep(formattedSolver)
    },
    // ソルバー一覧のテキストボックスの値が変更された時の処理
    changeInput(newValue: IFormattedSolver[]): void {
      const changedItems: IFormattedSolver[] = []

      // 変更されたソルバーのみを抽出
      newValue.forEach((newItem) => {
        const originalItem = this.originalFormattedSolvers.find(
          (original) => original.id === newItem.id
        )
        // 変更フラグ
        let isChange = false

        if (originalItem) {
          // 提供稼働率（今月）
          if (
            !isEqual(
              newItem.providedOperatingRate,
              originalItem.providedOperatingRate
            )
          ) {
            isChange = true
          }
          // 提供稼働率（来月）
          else if (
            !isEqual(
              newItem.providedOperatingRateNext,
              originalItem.providedOperatingRateNext
            )
          ) {
            isChange = true
          }
          // 再来月以降の稼働見込み
          else if (
            !isEqual(
              newItem.operationProspectsMonthAfterNext,
              originalItem.operationProspectsMonthAfterNext
            )
          ) {
            isChange = true
          }
          // 人月単価（上限・下限）
          else if (
            !isEqual(
              newItem.pricePerPersonMonth,
              originalItem.pricePerPersonMonth
            )
          ) {
            isChange = true
          }
          // 時間単価（上限・下限）
          else if (!isEqual(newItem.hourlyRate, originalItem.hourlyRate)) {
            isChange = true
          }

          // 変更があった場合は対象のソルバーのデータを格納
          if (isChange) {
            changedItems.push(newItem)
          }
        }
        // }
      })

      this.$emit('changeInput', changedItems)
    },
    handleError(
      item: any,
      headerValue: string,
      error: boolean,
      valueKey = null
    ) {
      this.$emit('handle-error', item, headerValue, error, valueKey)
    },
  },
})
</script>
