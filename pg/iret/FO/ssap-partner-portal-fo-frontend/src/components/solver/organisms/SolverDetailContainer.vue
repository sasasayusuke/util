<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :note-head="noteHead"
    :is-valid="isValid"
    :is-hide-button1="isHideButton1"
    :is-hide-button2="isHideButton2"
    :is-disabled-button1="isDisabledButton1"
    :is-project="false"
    :is-loading-button="isLoadingButton"
    :is-solver="isSolver"
    :is-detail="isDetail"
    :is-solver-detail="!isNew"
    style="margin-top: 30px"
    @click:positive="$emit('click:positive')"
    @click:negative="$emit('click:negative', index)"
  >
    <SolverDetailRows
      v-show="!isSolverSaved"
      v-model="isCandidateValid"
      :is-editing="isEditing"
      :is-disabled="isDisabled"
      :is-registered-solver-candidate="isRegisteredSolverCandidate"
      :solver-candidate="solverCandidate"
      :solvers="solvers"
      :solver="solver"
      :solver-corporation-id="solverCorporationId"
      :issue-map50="issueMap50"
      :tsi-areas="tsiAreas"
      :is-new="isNew"
      @change:solverName="onChangeSolverName"
      @inputForm="onInputForm"
      @change:isValid="onChangeIsValidByCandidate"
    />
    <SolverApplicationRows
      v-show="isSolverSaved"
      v-model="isSolverValid"
      :is-editing="isEditing"
      :solver-corporation-id="solverCorporationId"
      :solver="solver"
      :is-new="isNew"
      :issue-map50="issueMap50"
      :tsi-areas="tsiAreas"
      :is-display-application-project="true"
      :file-refresh="fileRefresh"
      @inputForm="onInputForm"
      @change:isValid="onChangeIsValidBySolver"
    />
    <template #footerButton>
      <slot name="footerButton" />
    </template>
    <!-- 削除モーダル -->
    <SolverDeleteModal
      :is-open="isSolverDeleteModalOpen"
      :text="$t('solver.pages.edit.text', { text: `${index + 1}人目` })"
      :title="$t('solver.pages.edit.delete')"
      @click:deleteSolver="$emit('click:deleteSolver', index)"
      @click:closeDelete="$emit('click:closeDelete', index)"
      @refresh="$emit('refresh')"
    />
  </DetailContainer>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import SolverDetailRows from '~/components/solver/molecules/SolverDetailRows.vue'
import {
  ENUM_REGISTRATION_STATUS,
  IGetSolverByIdResponse,
  IGetSolversResponse,
} from '~/types/Solver'
import { PropType } from '~/common/BaseVueClass'
import { IGetSelectItemsResponse } from '~/types/Master'
import SolverDeleteModal from '~/components/solver/molecules/SolverDeleteModal.vue'

export default CommonDetailContainer.extend({
  name: 'SolverDetailContainer',
  components: {
    DetailContainer,
    SolverDetailRows,
    SolverDeleteModal,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    index: {
      type: Number,
      required: true,
    },
    noteHead: {
      type: String,
      default: '',
    },
    isEditing: {
      type: Boolean,
      default: true,
    },
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    isSolverDeleteModalOpen: {
      type: Boolean,
      required: false,
    },
    isDisabled: {
      type: Boolean,
      default: false,
    },
    isRegisteredSolverCandidate: {
      type: Boolean,
      default: false,
    },
    solverCandidate: {
      type: Object as PropType<any>,
      required: true,
    },
    solvers: {
      type: Object as PropType<IGetSolversResponse>,
      required: true,
    },
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: true,
    },
    solverCorporationId: {
      type: String,
      required: true,
    },
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    tsiAreas: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
    isHideButton1: {
      type: Boolean,
      default: true,
    },
    isHideButton2: {
      type: Boolean,
      default: true,
    },
    isSolver: {
      type: Boolean,
      default: true,
    },
    isDetail: {
      type: Boolean,
      default: false,
    },
    isDisabledButton1: {
      type: Boolean,
      default: true,
    },
    newCandidateApplication: {
      type: Boolean,
      default: false,
    },
    // 添付ファイルのリフレッシュ
    fileRefresh: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      isValid: false,
      isDeleteModalOpen: false,
      isSolverSaved: false,
      // SolverApplicationRowsのバリデーションフラグ
      isSolverValid: false,
      // SolverDetailRowsのバリデーションフラグ
      isCandidateValid: false,
    }
  },
  watch: {
    // 個人ソルバー名を変更した時の処理
    solver: {
      handler(newValue) {
        this.isSolverSaved =
          newValue.registrationStatus === ENUM_REGISTRATION_STATUS.SAVED &&
          !newValue.isSolver &&
          !this.isNew
      },
    },
    isSolverValid(newValue) {
      this.isValid = this.isSolverSaved ? newValue : true
    },
    isCandidateValid(newValue) {
      this.isValid = !this.isSolverSaved ? newValue : true
    },
  },
  methods: {
    // formの入力値を保持する処理
    onInputForm(newValue: any) {
      const solverCandidateInputData = {
        index: this.index,
        ...newValue,
      }
      this.$emit('inputForm', solverCandidateInputData)
    },

    // 個人ソルバー名を変更した時の処理
    onChangeSolverName(newValue: { solverId: string; solverName: string }) {
      const solverNameData = {
        index: this.solverCandidate.index,
        solverId: newValue.solverId,
        solverName: newValue.solverName,
      }
      this.$emit('change:solverName', solverNameData)
    },

    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      const validInfo = { index: this.index, isValid: newValue }
      this.$emit('change:isValid', validInfo)
    },

    // SolverApplicationRowsのバリデーションの変化時処理
    onChangeIsValidBySolver(newValue: boolean) {
      // 新規ソルバー候補申請の場合は不要のためバリデーション処理をスキップ
      if (this.newCandidateApplication) {
        return
      }

      this.onChangeIsValid(this.isSolverSaved ? newValue : true)
    },

    // SolverDetailRowsのバリデーションの変化時処理
    onChangeIsValidByCandidate(newValue: boolean) {
      this.onChangeIsValid(!this.isSolverSaved ? newValue : true)
    },
  },
})
</script>
