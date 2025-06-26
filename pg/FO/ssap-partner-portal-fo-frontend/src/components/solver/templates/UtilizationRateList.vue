<template>
  <RootTemPlate>
    <ListInPageHeader
      :save="true"
      :date="priceAndOperatingRateUpdateAt"
      :is-loading="isLoading.solverUtilizationRate"
      :is-disabled="buttonDisabled"
      @save="save"
      >{{ pageName }}</ListInPageHeader
    >
    <v-form ref="tableForm">
      <UtilizationRateListTable
        :solvers="response.solvers"
        :total="response.total"
        :is-loading="isLoading.solver"
        @sort="sort($event)"
        @changeInput="changeInput"
        @handle-error="handleError"
      />
    </v-form>
    <SolverFooter
      :save="true"
      :is-loading="isLoading.solverUtilizationRate"
      :is-disabled="buttonDisabled"
      @save="save"
    />
    <v-snackbar v-model="snackbar" :vertical="true" timeout="3000">
      {{ $t('solver.pages.utilizationRate.snack-bar.complete') }}

      <template #action="{ attrs }">
        <v-btn color="#008a19" text v-bind="attrs" @click="snackbar = false">
          {{ $t('solver.pages.utilizationRate.snack-bar.close') }}
        </v-btn>
      </template>
    </v-snackbar>
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import ProjectSort from '~/components/project/organisms/ProjectSort.vue'
import UtilizationRateListTable from '~/components/solver/organisms/UtilizationRateListTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import type { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { GetSolversResponse } from '~/models/Solver'
import { IFormattedSolver } from '~/types/Solver'

export interface isLoading {
  solverCorporation: boolean
  solver: boolean
  solverUtilizationRate: boolean
}

export default CommonList.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    ProjectSort,
    UtilizationRateListTable,
    SolverFooter,
  },
  props: {
    /** ソルバー一覧 */
    response: {
      type: Object as PropType<GetSolversResponse>,
      required: true,
    },
    /** 稼働率・単価最終更新日時 */
    priceAndOperatingRateUpdateAt: {
      type: String,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    snackbar: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      pageName: this.$t('solver.pages.utilizationRate.name'),
      changeInputData: [] as IFormattedSolver[],
      buttonDisabled: true,
      // 初回変更されたか
      isFirstChange: false,
      // 各テキストボックスのバリテーションエラーを管理用
      errors: {} as { [key: string]: boolean },
    }
  },
  methods: {
    // 保存するボタン押下時の処理
    save(): void {
      // ソルバー一覧のデータを整形
      const formattedSolvers = this.changeInputData.map(
        (solver: IFormattedSolver) => {
          return {
            id: solver.id,
            name: solver.name,
            providedOperatingRate: Number(solver.providedOperatingRate) || 0,
            providedOperatingRateNext:
              Number(solver.providedOperatingRateNext) || 0,
            operationProspectsMonthAfterNext:
              solver.operationProspectsMonthAfterNext,
            pricePerPersonMonth:
              Number(solver.pricePerPersonMonth?.value2) || 0,
            pricePerPersonMonthLower:
              Number(solver.pricePerPersonMonth?.value1) || 0,
            hourlyRate: Number(solver.hourlyRate?.value2) || 0,
            hourlyRateLower: Number(solver.hourlyRate?.value1) || 0,
          }
        }
      )

      const saveUtilizationRateData = {
        utilizationRate: formattedSolvers,
      }

      this.$emit('updateSolverUtilizationRate', saveUtilizationRateData)
    },
    // ソルバー一覧のテキストボックスの値が変更された時の処理
    changeInput(newValue: IFormattedSolver[]): void {
      const form: any = this.$refs.tableForm
      const hasError = form && !form.validate()
      if (hasError) {
        this.buttonDisabled = true
        return
      }
      this.changeInputData = newValue
      // 初回変更されていない場合は保存ボタンを活性化
      if (!this.isFirstChange) {
        this.buttonDisabled = false
        this.isFirstChange = true
      }
    },
    // バリデーションエラーが出ている場合、保存ボタンを非活性にする
    handleError(
      item: any, // 個人ソルバーID
      headerValue: string, // 項目名
      error: boolean, // エラーの有無
      valueKey = null // 人月単価・時間単価(Value1,Value2)
    ) {
      // キー：個人ソルバーID-項目名-（人月単価・時間単価はValue）
      const errorKey = `${item.id}-${headerValue}${
        valueKey ? `-${valueKey}` : ''
      }`

      // エラー情報をバリテーションエラーを管理用に格納
      this.$set(this.errors, errorKey, error)

      // エラーがあるかどうかを判定
      // errorsに1つでもtrueある場合trueを返す
      let hasError = Object.values(this.errors).some((e) => e)
      const form: any = this.$refs.tableForm
      hasError = form && !form.validate()
      this.buttonDisabled = hasError
    },
  },
})
</script>
