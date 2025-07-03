<template>
  <div>
    <CommonDataTable
      :headers="solverHeaders"
      :items="formattedSolvers"
      :total="total"
      :offset-page="offsetPage"
      :limit="10"
      :is-loading="isLoading"
      :custom-sort-func="customSort"
      class="o-customer-list-table"
      @update="openModal"
      v-on="$listeners"
    >
    </CommonDataTable>
    <SolverConfirmSaveModal
      :is-modal-open="isModalOpen"
      @closeModal="closeModal"
      @update="patchSolver"
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
import { solverCorporationStore } from '~/store'
import { ENUM_REGISTRATION_STATUS } from '~/types/Solver'

export default BaseComponent.extend({
  name: '',
  components: {
    CommonDataTable,
  },
  props: {
    /**
     * ソルバー候補情報
     */
    solvers: {
      type: Array as PropType<SolverListItem[]>,
      required: true,
    },
    /**
     * データの合計
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
  data(): {
    solverHeaders: IDataTableHeader[]
    isModalOpen: Boolean
    solverId: string
    version: number | undefined
  } {
    return {
      solverHeaders: [
        // 個人ソルバー名
        {
          text: this.$t('solver.pages.candidateList.header.name'),
          sortable: false,
          align: 'start',
          value: 'name',
          width: '230px',
          maxLength: 13,
          link: {
            prefix: '/solver/candidate/',
            value: 'id',
          },
        },
        // 性別
        {
          text: this.$t('solver.pages.candidateList.header.sex.name'),
          sortable: true,
          value: 'sex',
          width: '90px',
        },
        // 年齢
        {
          text: this.$t('solver.pages.candidateList.header.age'),
          sortable: true,
          value: 'age',
          width: '90px',
        },
        // 専門テーマ
        {
          text: this.$t('solver.pages.candidateList.header.specializedThemes'),
          sortable: false,
          value: 'specializedThemes',
          maxLength: 40,
          width: '340px',
        },
        // 個人ソルバー登録ステータス
        {
          text: this.$t(
            'solver.pages.candidateList.header.certificationStatus.name'
          ),
          sortable: false,
          button: {
            label: 'certificationStatus',
          },
          value: 'certificationStatus',
          width: '170px',
        },
      ],
      isModalOpen: false,
      solverId: '',
      version: undefined,
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    /**
     * ソルバー候補情報をフォーマット化
     * @returns フォーマット化されたソルバー候補情報
     */
    formattedSolvers() {
      return this.solvers.map((solver: SolverListItem) => {
        const rtn = Object.assign(new SolverListItem(), solver)

        // 性別の整形
        if (rtn.sex === 'woman') {
          rtn.sex = (this.$t as Function)(
            'solver.pages.candidateList.header.sex.items.woman'
          ) as string
        } else if (rtn.sex === 'man') {
          rtn.sex = (this.$t as Function)(
            'solver.pages.candidateList.header.sex.items.man'
          ) as string
        } else {
          // 未設定
          rtn.sex = (this.$t as Function)(
            'solver.pages.candidateList.header.sex.items.not_set'
          ) as string
        }

        // 年齢
        rtn.age = rtn.age ? rtn.age : UNKNOWN

        // buttonInfoが存在しない場合に初期化
        if (!rtn.buttonInfo) {
          rtn.buttonInfo = { label: '', to: '' }
        }

        // 個人ソルバー登録ステータスの整形（登録申請/申請を再開/個人ソルバー登録）
        // label: ボタン名 to: 遷移先
        if (solver.registrationStatus === ENUM_REGISTRATION_STATUS.NEW) {
          // 登録申請
          rtn.buttonInfo.label = (this.$t as Function)(
            'solver.pages.candidateList.header.certificationStatus.items.before'
          ) as string

          rtn.buttonInfo.to =
            '/solver/candidate/certification/' +
            solver.id +
            `?source_url=/solver/candidate/list/${solverCorporationStore.id}`
        } else if (
          solver.registrationStatus ===
          ENUM_REGISTRATION_STATUS.TEMPORARY_SAVING
        ) {
          // 申請を再開
          rtn.buttonInfo.label = (this.$t as Function)(
            'solver.pages.candidateList.header.certificationStatus.items.editing'
          ) as string

          rtn.buttonInfo.to =
            '/solver/candidate/certification/' +
            solver.id +
            `?source_url=/solver/candidate/list/${solverCorporationStore.id}`
        } else if (
          solver.registrationStatus === ENUM_REGISTRATION_STATUS.SAVED
        ) {
          // 個人ソルバー登録
          rtn.buttonInfo.label = (this.$t as Function)(
            'solver.pages.candidateList.header.certificationStatus.items.during'
          ) as string
        }

        return rtn
      })
    },
  },
  methods: {
    patchSolver() {
      this.$emit('patchSolver', this.solverId, this.version)
    },
    openModal(id: string, version: number) {
      this.isModalOpen = true
      this.solverId = id
      this.version = version
    },
    closeModal() {
      this.isModalOpen = false
      this.solverId = ''
      this.version = undefined
    },
    customSort(items: any[]) {
      return items
    },
  },
})
</script>

<style lang="scss">
.o-customer-list-table {
  td {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    &:nth-of-type(1) {
      max-width: 590px;
    }
    &:nth-of-type(2) {
      max-width: 230px;
    }
    &:nth-of-type(3),
    &:nth-of-type(4) {
      max-width: 151px;
    }
  }
}
</style>
