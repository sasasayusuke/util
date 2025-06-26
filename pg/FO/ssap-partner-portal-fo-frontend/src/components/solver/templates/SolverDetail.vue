<template>
  <RootTemplate>
    <template v-if="!isFinished">
      <DetailInPageHeader
        :is-solver="true"
        :is-disabled="isDisabledFooter"
        :back-url="backUrlCancel"
        :is-editing="isEditing"
        :is-loading="isLoading"
        @buttonAction1="onClickConfirm"
        @buttonAction2="onClickBackToInput"
        @buttonAction3="onClickApply"
      >
        {{ $t('solver.pages.index.name') }}
      </DetailInPageHeader>
      <SolverDetailCommonContainer
        :title="$t('solver.pages.index.commonDetailTitle')"
        :note-head="'required'"
        :is-editing="isEditing"
        :solver-corporations="solverCorporations"
        style="margin-top: 30px"
        @change:solverCorporation="keepCorporateId"
        @click:displayProjectName="displayProjectName"
        @change:isValidCommon="onChangeIsValidCommon"
      />
      <div v-for="(solverCandidate, index) in solverCandidates" :key="index">
        <SolverDetailContainer
          :title="`${index + 1}人目`"
          :index="index"
          :note-head="'required'"
          :is-editing="isEditing"
          :is-loading-button="isLoadingButton"
          :is-solver-delete-modal-open="isSolverDeleteModalOpen[index]"
          :is-disabled="isDisabled[index]"
          :is-registered-solver-candidate="isRegisteredSolverCandidate"
          :solver-candidate="solverCandidate"
          :solvers="filteredSolvers"
          :solver="solver"
          :new-candidate-application="newCandidateApplication"
          :solver-corporation-id="solverCorporationId"
          :issue-map50="issueMap50"
          :tsi-areas="tsiAreas"
          style="margin-top: 30px"
          @click:negative="openSolverDeleteModal"
          @click:deleteSolver="deleteSolver"
          @click:closeDelete="closeSolverDeleteModal"
          @inputForm="onInputForm"
          @change:solverName="onChangeSolverName"
          @change:isValid="onChangeIsValid"
        >
          <template #footerButton>
            <Button
              style-set="large-error"
              outlined
              width="160"
              @click="openSolverDeleteModal(index)"
            >
              {{ $t('common.button.delete2') }}
            </Button>
          </template>
        </SolverDetailContainer>
      </div>
      <SolverFooter
        :is-editing="isEditing"
        :is-disabled="isDisabledFooter"
        :is-loading="isLoading"
        @add="addSolver"
        @click:confirm="onClickConfirm"
        @click:backToInput="onClickBackToInput"
        @click:apply="onClickApply"
      />
    </template>
    <template v-if="isFinished">
      <SolverInformation :back-url="backUrl" />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import SolverDetailCommonContainer from '../organisms/SolverCommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button } from '~/components/common/atoms'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { IGetSolverCorporationsResponse } from '~/types/SolverCorporation'
import { PropType } from '~/common/BaseVueClass'
import { IGetSolverByIdResponse, IGetSolversResponse } from '~/types/Solver'
import { IGetSelectItemsResponse } from '~/types/Master'
import SolverInformation from '~/components/solver/organisms/SolverInformation.vue'
import { meStore, solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { SolverInfo } from '~/models/Solver'

export default CommonDetailContainer.extend({
  name: 'TemplateSolverDetail',
  components: {
    DetailContainer,
    DetailInPageHeader,
    RootTemplate,
    SolverDetailCommonContainer,
    Button,
    SolverFooter,
    SolverInformation,
  },
  props: {
    // 個人ソルバー一覧
    solvers: {
      type: Object as PropType<IGetSolversResponse>,
      required: true,
    },
    // 個人ソルバー情報
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: true,
    },
    // 法人ソルバー一覧
    solverCorporations: {
      type: Object as PropType<IGetSolverCorporationsResponse>,
      required: true,
    },
    // 課題マップ50プルダウンアイテム
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    // 東証33業種経験/対応可能領域プルダウンアイテム
    tsiAreas: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    // 編集中か否か
    isEditing: {
      type: Boolean,
      default: true,
    },
    // 申請完了か否か
    isFinished: {
      type: Boolean,
      default: false,
    },
    // ローディング中か否か
    isLoading: {
      type: Boolean,
      default: false,
    },
    // 個人ソルバー名がサジェストから選択されたか否か
    isSuggest: {
      type: Boolean,
      default: false,
    },
    // 新規ソルバー候補申請か否か
    newCandidateApplication: {
      type: Boolean,
      default: false,
    },
    // 個人ソルバー名がサジェストから選択されたか否か
    onChangeSolverNameFunc: {
      type: Function,
      required: true,
    },
    currentIndex: {
      type: Number,
      default: 0,
    },
  },
  watch: {
    // 個人ソルバー名変更時の処理
    solver: {
      handler(newValue) {
        this.solverCandidates[newValue.index] = newValue
        this.isDisabled[newValue.index] = true
        if (newValue.id) {
          this.isRegisteredSolverCandidate = true
          this.solverCandidates[newValue.index].isRegisteredSolver = true
        } else {
          this.isRegisteredSolverCandidate = false
          this.solverCandidates[newValue.index].isRegisteredSolver = false
        }
      },
      deep: true,
    },
    solverCorporationId(newValue) {
      // 法人ID変更時の処理
      this.solverCorporationId = newValue
    },
  },
  data() {
    return {
      // コンテナ用ソルバー候補情報管理
      solverCandidates: [new SolverInfo()],
      // コンテナ用ソルバー候補情報活性非活性管理
      isDisabled: [false],
      isRegisteredSolverCandidate: false,
      isLoadingButton: false,
      isSolverDeleteModalOpen: [false],
      solverCorporationId: '',
      projectId: '',
      individualSolverId: [''],
      validInfos: [false],
      isValidCommon: false,
      backUrlCancel: this.$route.query.source_url
        ? this.$route.query.source_url
        : '/solver/menu',
      deleteIndex: -1,
    }
  },
  created() {
    this.solverCorporationId = this.getInitialCorporateId()
  },
  computed: {
    // 個人ソルバー名サジェストアイテムフィルター処理
    filteredSolvers() {
      if (this.solverCorporationId) {
        const corporateId = this.solverCorporationId as string
        const filteredSolvers = this.solvers.solvers.filter(
          (solver) => solver.corporateId === corporateId
        )
        return {
          ...this.solvers,
          solvers: filteredSolvers,
        }
      } else {
        return this.solvers
      }
    },
    isDisabledFooter() {
      if (this.solverCandidates.length === 0) {
        // 申請するソルバー候補がいない場合は非活性
        return true
      } else if (this.validInfos.includes(false) || !this.isValidCommon) {
        return true
      } else {
        return false
      }
    },
    // 一覧ページへ戻るリンクの制御
    backUrl(): string {
      return `/solver/candidate/list/${this.solverCorporationId}`
    },
  },
  methods: {
    // ソルバー候補追加処理
    addSolver() {
      const newSolverCandidate = {
        ...new SolverInfo(),
        index: this.currentIndex,
      }
      this.solverCandidates = [...this.solverCandidates, newSolverCandidate]
      this.isDisabled = [...this.isDisabled, false]
      this.isSolverDeleteModalOpen = [...this.isSolverDeleteModalOpen, false]
      this.validInfos = [...this.validInfos, false]
    },

    // ソルバー候補削除処理
    deleteSolver(newValue: number) {
      this.deleteIndex = this.solverCandidates[newValue].index
      this.solverCandidates.splice(newValue, 1)
      this.isDisabled.splice(newValue, 1)
      this.isSolverDeleteModalOpen.splice(newValue, 1)
      this.validInfos.splice(newValue, 1)
      this.$emit('click:delete', this.solverCandidates)
    },

    // ソルバー候補削除モーダルを開く処理
    openSolverDeleteModal(newValue: number) {
      this.$set(this.isSolverDeleteModalOpen, newValue, true)
    },

    // ソルバー削除モーダルを閉じる処理
    closeSolverDeleteModal(newValue: number) {
      this.$set(this.isSolverDeleteModalOpen, newValue, false)
    },

    // 「確認画面に進む」ボタン押下時の処理
    onClickConfirm() {
      this.$emit('click:confirm')
    },

    // 「入力画面に戻る」ボタン押下時の処理
    onClickBackToInput() {
      this.$emit('click:backToInput')
    },

    // フォームに入力した時の処理
    onInputForm(newValue: any) {
      if (!this.isSuggest) {
        delete newValue.id
        delete newValue.name
      }
      // 削除時にフォームのインプットイベントが発火してしまうため、削除対象のデータをチェックする
      const targetIndex = this.solverCandidates.findIndex(
        (d) => d.index === newValue.index
      )
      if (targetIndex >= 0 && newValue.index !== this.deleteIndex) {
        this.solverCandidates[targetIndex] = {
          ...this.solverCandidates[targetIndex],
          ...newValue,
        }
        this.$emit('inputForm', newValue)
      }
    },

    // ソルバー候補の法人IDの初期値をセットする処理
    getInitialCorporateId() {
      if (meStore.role === ENUM_USER_ROLE.SOLVER_STAFF) {
        return meStore.solverCorporationId
      } else if (meStore.role === ENUM_USER_ROLE.APT) {
        // アライアンス担当で公式サイトからの遷移ではない（クエリパラメータが指定されてない）
        if (!this.$route.query.id) {
          return solverCorporationStore.id
        }
      }
      return ''
    },

    // ソルバー候補の入力値（法人ID）を保持する処理
    keepCorporateId(newValue: string) {
      this.solverCorporationId = newValue
      this.$emit('change:solverCorporation', newValue)
    },

    // 「申請する」ボタン押下時の処理
    onClickApply() {
      this.$emit('click:apply')
    },

    // 案件IDと案件名を表示する処理
    displayProjectName(newValue: { projectId: string; projectName: string }) {
      this.projectId = newValue.projectId
      this.$emit('click:displayProjectName', newValue)
    },

    // 個人ソルバー名を変更した時の処理
    async onChangeSolverName(newValue: {
      index: number
      solverId: string
      solverName: string
    }) {
      this.individualSolverId[newValue.index] = newValue.solverId
      await this.onChangeSolverNameFunc(newValue)
    },

    // formのバリデーションが変化した時の処理
    // 「確認画面に進む」ボタン活性非活性判定
    onChangeIsValid(newValue: { index: number; isValid: boolean }) {
      this.$set(this.validInfos, newValue.index, newValue.isValid)
    },

    // formのバリデーションが変化した時の処理（共通項目）
    // 「確認画面に進む」ボタン活性非活性判定
    onChangeIsValidCommon(newValue: boolean) {
      this.isValidCommon = newValue
    },
  },
})
</script>
