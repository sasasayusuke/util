<template>
  <RootTemplate>
    <SolverDetailContainer
      :title="pageName()"
      :index="0"
      :note-head="'required'"
      :is-editing="isEditing"
      :is-loading-button="isLoading"
      :is-solver-delete-modal-open="isSolverDeleteModalOpen"
      :is-disabled="false"
      :solver-candidate="solver"
      :solvers="solvers"
      :solver="solver"
      :solver-corporation-id="''"
      :issue-map50="issueMap50"
      :tsi-areas="tsiAreas"
      :is-new="false"
      :is-hide-button1="false"
      :is-hide-button2="false"
      :is-solver="false"
      :is-detail="true"
      :is-disabled-button1="isDisabledButton1"
      :file-refresh="fileRefresh"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
      @inputForm="onInputForm"
      @change:isValid="onChangeIsValid"
    >
      <template v-if="isEditing" #footerButton>
        <Button
          outlined
          style-set="large-tertiary"
          width="150"
          @click="onClickCancel"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="large-primary"
          class="ml-2"
          width="150"
          :loading="isLoading"
          :disabled="isDisabledButton1"
          @click="onClickSave"
        >
          {{ $t('common.button.save2') }}
        </Button>
      </template>
      <template v-else #footerButton>
        <Button outlined style-set="large-tertiary" width="150" :to="backUrl">
          {{ $t('common.button.backToList') }}
        </Button>
        <Button
          class="ml-2"
          style-set="large-primary"
          width="150"
          @click="onClickEdit"
        >
          {{ $t('common.button.edit2') }}
        </Button>
      </template>
    </SolverDetailContainer>
    <SolverFooter
      :is-editing="isEditing"
      :is-delete="true"
      @delete="$emit('click:delete')"
    />
    <!-- 削除モーダル -->
    <SolverDeleteModal
      :is-open="isDeleteModalOpen"
      :title="''"
      :text="$t('solver.pages.candidate.modal.text')"
      :text2="$t('solver.pages.candidate.modal.text2')"
      :text3="$t('solver.pages.candidate.modal.text3')"
      :is-modal-edit="false"
      @click:deleteSolver="$emit('click:deleteSolver')"
      @click:closeDelete="$emit('click:closeDelete')"
    />
  </RootTemplate>
</template>

<script lang="ts">
import SolverDetailContainer from '../organisms/SolverDetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button } from '~/components/common/atoms'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { meStore, solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { GetSolversResponse } from '~/models/Solver'
import { PropType } from '~/common/BaseVueClass'
import {
  ENUM_REGISTRATION_STATUS,
  IGetSolverByIdResponse,
} from '~/types/Solver'
import { IGetSelectItemsResponse } from '~/types/Master'
import SolverDeleteModal from '~/components/solver/molecules/SolverDeleteModal.vue'

export default CommonDetailContainer.extend({
  name: 'TemplateSolverCandidateDetail',
  components: {
    SolverDetailContainer,
    DetailInPageHeader,
    RootTemplate,
    Button,
    SolverFooter,
    SolverDeleteModal,
  },
  props: {
    // 個人ソルバー情報
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
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
      default: false,
    },
    // ローディング中か否か
    isLoading: {
      type: Boolean,
      default: false,
    },
    // ソルバー削除モーダルが開いているか否か
    isDeleteModalOpen: {
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
      solvers: new GetSolversResponse(),
      isSolverDeleteModalOpen: false,
      isDisabledButton1: false,
      isChanged: false,
      backUrl:
        meStore.role === ENUM_USER_ROLE.APT
          ? `/solver/candidate/list/${solverCorporationStore.id}`
          : `/solver/candidate/list/${meStore.solverCorporationId}`,
    }
  },
  watch: {
    isEditing(newVal) {
      this.isDisabledButton1 = newVal
    },
  },
  methods: {
    pageName() {
      const isEditing = this.isEditing as boolean
      const edit = this.$t('solver.pages.candidate.edit.name') as string
      const detail = this.$t('solver.pages.candidate.name') as string
      return isEditing ? edit : detail
    },
    // 「変更する」ボタン押下時の処理
    onClickEdit() {
      this.$emit('click:edit')
    },

    // 「保存する」ボタン押下時の処理
    onClickSave() {
      this.$emit('click:save')
    },

    // 「キャンセル」ボタン押下時の処理
    onClickCancel() {
      this.$emit('click:cancel')
    },

    // コンテナヘッダー「変更する」・「保存する」ボタン押下時の処理
    onClickPositive() {
      this.$emit('click:positive')
    },

    // コンテナヘッダー「一覧へ戻る」・「キャンセル」ボタン押下時の処理
    onClickNegative() {
      this.$emit('click:negative')
    },

    // フォームに入力した値を親コンポーネントに渡す処理（更新用データ）
    onInputForm(newValue: any) {
      this.$emit('inputForm', newValue)
      this.isChanged = true
    },

    // formのバリデーションが変化した時の処理
    // 「保存する」ボタン活性非活性判定
    onChangeIsValid(newValue: { index: number; isValid: boolean }) {
      if (this.isChanged) {
        this.isDisabledButton1 = this.isEditing ? !newValue.isValid : false
      }
    },
    // 個人ソルバー登録申請済みか判定
    isSolverSaved() {
      return this.solver.registrationStatus === ENUM_REGISTRATION_STATUS.SAVED
    },
  },
})
</script>
