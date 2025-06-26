<template>
  <div>
    <CommonDataTable
      :is-solver-list="true"
      :headers="solverHeaders"
      :items="formattedSolvers"
      :total="total"
      :is-loading="isLoading"
      :offset-page="offsetPage"
      :limit="10"
      :custom-sort-func="customSort"
      v-on="$listeners"
      @update="$emit('update')"
    />
  </div>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { UNKNOWN } from '~/const'
import { SolverListItem } from '~/models/Solver'
import {
  ENUM_OPERATING_STATUS,
  ENUM_REGISTRATION_STATUS,
  ENUM_SEX_TYPE,
} from '~/types/Solver'

export default BaseComponent.extend({
  name: 'SolverListTable',
  components: {
    CommonDataTable,
  },
  props: {
    /**
     * 個人ソルバー情報のリスト
     */
    solvers: {
      type: Array as PropType<SolverListItem[]>,
      required: true,
    },
    /**
     * データの合計件数
     */
    total: {
      type: Number,
      required: true,
    },
    /**
     * 開始ページ
     */
    offsetPage: {
      type: Number,
      required: true,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { solverHeaders: IDataTableHeader[] } {
    return {
      solverHeaders: [
        // 個人ソルバー名、ソートなし、個人ソルバー情報詳細（参照）へ遷移
        {
          text: this.$t('solver.pages.list.header.name'),
          sortable: false,
          align: 'start',
          value: 'name',
          width: '230px',
          maxLength: 13,
          link: {
            prefix: '/solver/',
            value: 'id',
          },
        },
        // 性別
        {
          text: this.$t('solver.pages.list.header.sex.name'),
          sortable: true,
          value: 'sex',
          width: '90px',
        },
        // 年齢
        {
          text: this.$t('solver.pages.list.header.age'),
          sortable: true,
          value: 'age',
          width: '90px',
        },
        // 専門テーマ（ソートなし）
        {
          text: this.$t('solver.pages.list.header.specializedThemes'),
          sortable: false,
          value: 'specializedThemes',
          maxLength: 40,
          width: '340px',
        },
        // 稼働状況
        {
          text: this.$t('solver.pages.list.header.operatingStatus'),
          sortable: true,
          value: 'operatingStatus',
          maxLength: 40,
          width: '170px',
        },
        // 稼働率（今月）
        {
          text: this.$t('solver.pages.list.header.providedOperatingRate'),
          sortable: true,
          value: 'providedOperatingRate',
          maxLength: 40,
          width: '170px',
        },
        // 稼働率（来月）
        {
          text: this.$t('solver.pages.list.header.providedOperatingRateNext'),
          sortable: true,
          value: 'providedOperatingRateNext',
          maxLength: 40,
          width: '170px',
        },
        // 人月単価（下限〜上限）
        {
          text: this.$t('solver.pages.list.header.pricePerPersonMonth'),
          sortable: true,
          value: 'pricePerPersonMonth',
          maxLength: 40,
          width: '340px',
        },
        // 登録ステータス
        {
          text: this.$t('solver.pages.list.header.registrationStatus.name'),
          sortable: true,
          button: {
            label: 'registrationStatus',
          },
          value: 'registrationStatus',
          width: '170px',
        },
      ],
    }
  },
  computed: {
    /**
     * 個人ソルバー情報を表示用にフォーマット
     * @returns フォーマット済みのソルバー情報リスト
     */
    formattedSolvers(): SolverListItem[] {
      return this.solvers.map((solver: SolverListItem) => {
        // 直接変更しないようにコピー
        const rtn = {
          ...solver,
        }

        // 性別
        rtn.sex = this.formatSex(
          solver.sex as typeof ENUM_SEX_TYPE[keyof typeof ENUM_SEX_TYPE]
        )
        // 年齢
        rtn.age = rtn.age ? rtn.age : UNKNOWN
        // 稼働状況
        rtn.operatingStatus = this.formatOperatingStatus(
          solver.operatingStatus as typeof ENUM_OPERATING_STATUS[keyof typeof ENUM_OPERATING_STATUS]
        )
        // 登録ステータス
        rtn.buttonInfo = this.formatRegistrationStatus(
          solver.registrationStatus as typeof ENUM_REGISTRATION_STATUS[keyof typeof ENUM_REGISTRATION_STATUS]
        )
        // 稼働率（今月）
        rtn.providedOperatingRate = this.formatProvidedOperatingRate(
          this.$t('solver.pages.list.utilizationRate.month1') as string,
          solver.providedOperatingRate
        )
        // 稼働率（来月）
        rtn.providedOperatingRateNext = this.formatProvidedOperatingRate(
          this.$t('solver.pages.list.utilizationRate.month2') as string,
          solver.providedOperatingRateNext
        )
        // 人月単価（下限〜上限）
        rtn.pricePerPersonMonth = this.formatPricePerPersonMonth(
          solver.pricePerPersonMonthLower,
          solver.pricePerPersonMonth
        )
        return rtn
      })
    },
  },
  methods: {
    /**
     * 性別を表示用の文字列に変換
     * @param sex 性別のENUM値
     * @returns 性別の文字列
     */
    formatSex(sex: typeof ENUM_SEX_TYPE[keyof typeof ENUM_SEX_TYPE]): string {
      const sexKeyMap: Record<
        typeof ENUM_SEX_TYPE[keyof typeof ENUM_SEX_TYPE],
        string
      > = {
        [ENUM_SEX_TYPE.MAN]: 'solver.pages.list.sort_input.sex.items.man',
        [ENUM_SEX_TYPE.WOMAN]: 'solver.pages.list.sort_input.sex.items.woman',
        [ENUM_SEX_TYPE.NOT_SET]:
          'solver.pages.list.sort_input.sex.items.not_set',
        [ENUM_SEX_TYPE.ALL]: 'solver.pages.list.sort_input.sex.items.all',
      }
      const sexKey = sexKeyMap[sex] || ''
      return this.$t(sexKey) as string
    },
    /**
     * 稼働状況を表示用の文字列に変換
     * @param operatingStatus 稼働状況のENUM値
     * @returns 稼働状況の文字列
     */
    formatOperatingStatus(
      operatingStatus: typeof ENUM_OPERATING_STATUS[keyof typeof ENUM_OPERATING_STATUS]
    ): string {
      const operatingStatusKeyMap: Record<
        typeof ENUM_OPERATING_STATUS[keyof typeof ENUM_OPERATING_STATUS],
        string
      > = {
        [ENUM_OPERATING_STATUS.NOT_WORKING]:
          'solver.pages.list.sort_input.operatingStatus.items.not_working',
        [ENUM_OPERATING_STATUS.WORKING]:
          'solver.pages.list.sort_input.operatingStatus.items.working',
        [ENUM_OPERATING_STATUS.INACTIVE]:
          'solver.pages.list.sort_input.operatingStatus.items.inactive',
        [ENUM_OPERATING_STATUS.ALL]:
          'solver.pages.list.sort_input.operatingStatus.items.all',
      }
      const statusKey = operatingStatusKeyMap[operatingStatus] || ''
      return this.$t(statusKey) as string
    },
    /**
     * 登録ステータスに基づいてボタン情報を生成
     * @param registrationStatus 登録ステータスのENUM値
     * @returns ボタン情報またはテキスト情報
     */
    formatRegistrationStatus(
      registrationStatus: typeof ENUM_REGISTRATION_STATUS[keyof typeof ENUM_REGISTRATION_STATUS]
    ) {
      if (registrationStatus === ENUM_REGISTRATION_STATUS.SAVED) {
        // 登録ステータスが "saved" の場合、ボタンを表示
        return {
          label: this.$t(
            'solver.pages.list.header.registrationStatus.items.saved'
          ) as string,
          to: '',
        }
      } else {
        // その他の登録ステータスの場合、テキストを表示
        const statusKeyMap: Record<
          typeof ENUM_REGISTRATION_STATUS[keyof typeof ENUM_REGISTRATION_STATUS],
          string
        > = {
          [ENUM_REGISTRATION_STATUS.NEW]:
            'solver.pages.list.header.registrationStatus.items.before',
          [ENUM_REGISTRATION_STATUS.TEMPORARY_SAVING]:
            'solver.pages.list.header.registrationStatus.items.before',
          [ENUM_REGISTRATION_STATUS.CERTIFICATED]:
            'solver.pages.list.header.registrationStatus.items.certificated',
          saved: '',
        }
        const textKey = statusKeyMap[registrationStatus] || ''
        return {
          label: this.$t(textKey) as string,
          to: '',
        }
      }
    },
    /**
     * 稼働率を表示用フォーマットに変換
     * @param label ラベル（例: "今月", "来月"）
     * @param rate 稼働率の数値
     * @returns フォーマット済みの文字列または空文字
     */
    formatProvidedOperatingRate(label: string, rate?: number): string {
      const percentSymbol = this.$t(
        'solver.pages.list.utilizationRate.unit1'
      ) as string

      return `${label}${rate ?? 0}${percentSymbol}`
    },
    /**
     * 人月単価（下限〜上限）を表示用フォーマットに変換
     * @param lower 下限の単価
     * @param upper 上限の単価
     * @returns フォーマット済みの文字列または空文字
     */
    formatPricePerPersonMonth(lower?: number, upper?: number): string {
      const yenSymbol = this.$t(
        'solver.pages.list.utilizationRate.unit2'
      ) as string

      const lowerPrice = lower
        ? `${(lower ?? 0).toLocaleString()}${yenSymbol}`
        : UNKNOWN
      const upperPrice = upper
        ? `${(upper ?? 0).toLocaleString()}${yenSymbol}`
        : UNKNOWN

      return `${lowerPrice} 〜 ${upperPrice}`
    },
    update(id: string, version: number) {
      this.$emit('update', id, version)
    },
    customSort(items: any[]) {
      return items
    },
  },
})
</script>
