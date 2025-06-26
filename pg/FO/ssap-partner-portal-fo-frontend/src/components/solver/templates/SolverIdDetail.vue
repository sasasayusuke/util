<template>
  <RootTemplate>
    <SolverApplicationContainer
      :title="pageName()"
      :note-head="'true'"
      :is-editing="isEditing"
      :is-disabled="isDisabled"
      :is-loading-button="isLoading"
      :is-solver-delete-modal-open="isSolverDeleteModalOpen"
      :is-temporary-saved="isTemporarySaved"
      :solver-corporation-id="solverCorporationId"
      :is-hide-button1="false"
      :is-hide-button2="false"
      :is-solver-detail="true"
      :is-solver-application="isSolverApplication"
      :solver="solver"
      :is-new="false"
      :issue-map50="issueMap50"
      :tsi-areas="tsiAreas"
      :is-temporary-save="isTemporarySave()"
      :is-temporary-disabled="isTemporaryDisabled"
      :is-loading-button2="isLoadingTemporary"
      :file-refresh="fileRefresh"
      style="margin-top: 30px"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
      @change:isValid="onChangeIsValid"
      @inputForm="onInputForm"
      @updateTemporaryDisabled="handleTemporaryDisabledUpdate"
      @temporarySaveAction="$emit('temporarySaveAction')"
    >
      <template v-if="isEditing" #footerButton>
        <div
          ref="editButtonArea"
          :class="!isEditButtonAreaVisible ? '' : 'no-fixed-edit-button-wrap'"
        >
          <div
            :class="
              !isEditButtonAreaVisible
                ? 'fixed-edit-button-area'
                : 'no-fixed-edit-button-area'
            "
          >
            <Button
              outlined
              style-set="large-tertiary"
              width="150"
              @click="onClickCancel"
            >
              {{ $t('common.button.cancel') }}
            </Button>
            <Button
              v-if="isTemporarySave()"
              outlined
              style-set="large-primary"
              width="150"
              class="ml-2"
              :loading="isLoadingTemporary"
              :disabled="isTemporaryDisabled"
              @click="$emit('temporarySaveAction')"
            >
              {{ $t('common.button.temporarySave') }}
            </Button>
            <Button
              style-set="large-primary"
              class="ml-2"
              width="150"
              :loading="isLoading"
              :disabled="isDisabledButton1"
              @click="onClickSave"
            >
              {{
                isTemporarySave()
                  ? $t('common.button.apply')
                  : $t('common.button.save2')
              }}
            </Button>
          </div>
        </div>
      </template>
      <template v-else #footerButton>
        <Button outlined style-set="large-tertiary" width="150" :to="backUrl">
          {{ $t('common.button.backToList') }}
        </Button>
        <Button
          style-set="large-primary"
          class="ml-2"
          width="150"
          @click="onClickEdit"
        >
          {{ $t('common.button.edit2') }}
        </Button>
      </template>
      <template #snackBar>
        <v-snackbar
          :value="isSnackbarOpen"
          :width="300"
          :vertical="true"
          timeout="3000"
          style="z-index: 1500"
        >
          <div>
            {{ $t('solver.pages.application.detail.snack-bar.complete1') }}
          </div>
          <div>
            {{ $t('solver.pages.application.detail.snack-bar.complete2') }}
          </div>

          <template #action="{ attrs }">
            <v-btn
              color="#008a19"
              text
              v-bind="attrs"
              @click="$emit('updateSnackBarOpen', false)"
            >
              {{ $t('solver.pages.application.detail.snack-bar.close') }}
            </v-btn>
          </template>
        </v-snackbar>
      </template>
    </SolverApplicationContainer>
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
import SolverApplicationContainer from '~/components/solver/organisms/SolverApplicationContainer.vue'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { meStore, solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { GetSolversResponse } from '~/models/Solver'
import { PropType } from '~/common/BaseVueClass'
import {
  IGetSolverByIdResponse,
  ENUM_REGISTRATION_STATUS,
} from '~/types/Solver'
import { IGetSelectItemsResponse } from '~/types/Master'
import SolverDeleteModal from '~/components/solver/molecules/SolverDeleteModal.vue'

export default CommonDetailContainer.extend({
  name: 'TemplateSolverDetail',
  components: {
    SolverDetailContainer,
    DetailInPageHeader,
    RootTemplate,
    Button,
    SolverApplicationContainer,
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
    // ローディング中か否か
    isLoadingTemporary: {
      type: Boolean,
      default: false,
    },
    // スナックバー表示フラグ
    isSnackbarOpen: {
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
      isLoadingButton: false,
      isSolverDeleteModalOpen: false,
      isDisabled: false,
      checkValid: false,
      isTemporarySaved: true,
      isSolverApplication: false,
      solverCorporationId: '',
      isDisabledButton1: true,
      backUrl:
        meStore.role === ENUM_USER_ROLE.APT
          ? `/solver/list/${solverCorporationStore.id}`
          : `/solver/list/${meStore.solverCorporationId}`,
      isTemporaryDisabled: false,
      isEditButtonAreaVisible: false,
    }
  },
  watch: {
    isEditing(newVal) {
      this.isDisabled = newVal
      this.isDisabledButton1 = newVal
    },
  },
  updated() {
    if (this.isEditing) {
      // 追従ボタンの表示非表示設定
      const observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            this.isEditButtonAreaVisible = entry.isIntersecting
          })
        },
        {
          root: null,
          threshold: 0.1,
        }
      )
      observer.observe(this.$refs.editButtonArea as Element)
    }
  },
  methods: {
    pageName() {
      const isEditing = this.isEditing as boolean
      const edit = this.$t('solver.pages.solverEdit.name') as string
      const detail = this.$t('solver.pages.solverDetail.name') as string
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
      // this.checkValid(newValue)
      this.$emit('inputForm', newValue)
    },

    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      if (this.isEditing) {
        this.isDisabled = !newValue
      } else {
        this.isDisabled = false
      }

      this.isDisabledButton1 = !newValue
    },
    // 子コンポーネントから送られた値でをフラグ更新
    handleTemporaryDisabledUpdate(isTemporaryDisabled: boolean) {
      // 子コンポーネントから送られたデータを親で更新
      this.isTemporaryDisabled = isTemporaryDisabled
    },
    // 一時保存
    temporarySaveAction() {
      this.$emit('temporarySaveAction')
    },
    // 一時保存中のデータか判定
    isTemporarySave() {
      return (
        this.solver.registrationStatus ===
        ENUM_REGISTRATION_STATUS.TEMPORARY_SAVING
      )
    },
  },
})
</script>
<style lang="scss" scoped>
.fixed-edit-button-area {
  width: 1200px;
  text-align: center;
  background-color: #fff;
  padding: 20px 10px;
  transform: translateX(-50%);
  position: fixed;
  bottom: 0;
  z-index: 1000;
}
.no-fixed-edit-button-wrap {
  width: 1150px !important;
}
.no-fixed-edit-button-area {
  width: 100%;
  text-align: center;
  background-color: #fff;
  padding: 20px 10px;
}
</style>
