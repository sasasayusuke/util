<template>
  <RootTemplate>
    <template v-if="!isFinished">
      <SolverApplicationContainer
        :title="title"
        :note-head="'required'"
        :is-editing="isEditing"
        :is-disabled="isDisabled"
        :is-loading-button="isLoading"
        :is-loading-button2="isLoadingTemporary"
        :is-solver-delete-modal-open="isSolverDeleteModalOpen"
        :is-temporary-saved="isTemporarySaved"
        :solver-corporation-id="solverCorporationId"
        :issue-map50="issueMap50"
        :tsi-areas="tsiAreas"
        :solver="solver"
        :is-display-application-project="isDisplayApplicationProject"
        :is-temporary-disabled="isTemporaryDisabled"
        :file-refresh="fileRefresh"
        style="margin-top: 30px"
        @click:positive="onClickBackToList"
        @click:negative="onClickTemporarySave"
        @buttonAction3="onClickConfirm"
        @buttonAction4="onClickBackToInput"
        @buttonAction5="onClickApply"
        @buttonAction6="onClickCancel"
        @click:closeDelete="onClickTemporarySave"
        @change:isValid="onChangeIsValid"
        @inputForm="onInputForm"
        @updateTemporaryDisabled="handleTemporaryDisabledUpdate"
      >
        <template v-if="isEditing" #footerButton>
          <div ref="targetButton">
            <Button
              v-if="!isTemporarySaved"
              outlined
              style-set="large-tertiary"
              width="160"
              class="mr-3"
              @click="onClickCancel"
            >
              {{ $t('common.button.cancel') }}
            </Button>
            <Button
              v-if="isTemporarySaved"
              outlined
              style-set="large-tertiary"
              width="160"
              class="mr-3"
              @click="onClickBackToList"
            >
              {{ $t('common.button.backToList') }}
            </Button>
            <Button
              outlined
              style-set="large-primary"
              width="160"
              class="mr-3"
              :loading="isLoadingTemporary"
              :disabled="isTemporaryDisabled"
              @click="onClickTemporarySave"
            >
              {{ $t('common.button.temporarySave') }}
            </Button>
            <Button
              style-set="large-primary"
              width="160"
              :disabled="isDisabled"
              :loading="isLoadingTemporary"
              @click="onClickConfirm"
            >
              {{ $t('common.button.toConfirm') }}
            </Button>
          </div>
        </template>
        <template v-else #footerButton>
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            class="mr-3"
            @click="onClickBackToInput"
          >
            {{ $t('common.button.backToInput') }}
          </Button>
          <Button
            style-set="large-primary"
            width="160"
            class="mr-3"
            :loading="isLoading"
            @click="onClickApply"
          >
            {{ $t('common.button.apply') }}
          </Button>
        </template>
        <template #snackBar>
          <v-snackbar
            :value="snackbar"
            :width="300"
            :vertical="true"
            timeout="3000"
            style="z-index: 1500"
          >
            <div>
              {{ $t('solver.pages.application.detail.snack-bar.complete1') }}
            </div>
            <!-- 個人ソルバー登録申請画面の場合 -->
            <div v-if="isRegistrationApplication">
              {{ $t('solver.pages.application.detail.snack-bar.complete3') }}
            </div>
            <!-- 新規個人ソルバー申請画面の場合 -->
            <div v-else>
              {{ $t('solver.pages.application.detail.snack-bar.complete4') }}
            </div>

            <template #action="{ attrs }">
              <v-btn
                color="#008a19"
                text
                v-bind="attrs"
                @click="snackbar = false"
              >
                {{ $t('solver.pages.application.detail.snack-bar.close') }}
              </v-btn>
            </template>
          </v-snackbar>
        </template>
      </SolverApplicationContainer>
      <Sheet
        v-if="!isVisible && isEditing"
        class="fixed-nav-button d-flex justify-center pt-6 pb-6"
        width="100%"
        color="#ffffff"
      >
        <Button
          v-if="!isTemporarySaved"
          outlined
          style-set="large-tertiary"
          width="160"
          class="mr-3"
          @click="onClickCancel"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          v-if="isTemporarySaved"
          outlined
          style-set="large-tertiary"
          width="160"
          class="mr-3"
          @click="onClickBackToList"
        >
          {{ $t('common.button.backToList') }}
        </Button>
        <Button
          outlined
          style-set="large-primary"
          width="160"
          class="mr-3"
          :loading="isLoadingTemporary"
          :disabled="isTemporaryDisabled"
          @click="onClickTemporarySave"
        >
          {{ $t('common.button.temporarySave') }}
        </Button>
        <Button
          style-set="large-primary"
          width="160"
          :disabled="isDisabled"
          :loading="isLoadingTemporary"
          @click="onClickConfirm"
        >
          {{ $t('common.button.toConfirm') }}
        </Button>
      </Sheet>
    </template>
    <template v-if="isFinished">
      <SolverInformation :back-url="backUrl" />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet } from '~/components/common/atoms'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { PropType } from '~/common/BaseVueClass'
import { IGetSelectItemsResponse } from '~/types/Master'
import SolverInformation from '~/components/solver/organisms/SolverInformation.vue'
import { solverCorporationStore } from '~/store'
import { IGetSolverByIdResponse } from '~/types/Solver'

export default CommonDetailContainer.extend({
  name: 'TemplateSolverApplication',
  components: {
    DetailContainer,
    DetailInPageHeader,
    RootTemplate,
    Button,
    Sheet,
    SolverFooter,
    SolverInformation,
  },
  props: {
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
    // 個人ソルバー情報
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: false,
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
    // ローディング中か否か（一時保存）
    isLoadingTemporary: {
      type: Boolean,
      default: false,
    },
    // スナックバーを表示するか否か
    isSnackbarOpen: {
      type: Boolean,
      default: false,
    },
    // 応募案件を表示するか否か
    isDisplayApplicationProject: {
      type: Boolean,
      default: true,
    },
    // 個人ソルバー登録申請か否か
    isRegistrationApplication: {
      type: Boolean,
      default: false,
    },
    /** 添付ファイルのリフレッシュ */
    fileRefresh: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      title: this.isDisplayApplicationProject
        ? this.$t(`solver.pages.application.name`)
        : this.$t(`solver.pages.candidate.certification.name`),
      isSolverDeleteModalOpen: false,
      isDisabled: true,
      isChanged: false,
      isTemporarySaved: false,
      solverCorporationId: '',
      snackbar: false,
      isVisible: false,
      backUrl: this.isRegistrationApplication
        ? `/solver/candidate/list/${solverCorporationStore.id}`
        : `/solver/list/${solverCorporationStore.id}`,
      backUrlCancel: this.$route.query.source_url
        ? (this.$route.query.source_url as string)
        : '/solver/menu',
      isTemporaryDisabled: false,
    }
  },
  updated() {
    if (this.isEditing) {
      const target = this.$refs.targetButton

      // 追従ボタンの表示非表示設定
      const observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            this.isVisible = entry.isIntersecting
          })
        },
        {
          root: null,
          threshold: 0.1,
        }
      )
      observer.observe(target as Element)
    }
  },
  watch: {
    isSnackbarOpen(newValue) {
      this.snackbar = newValue
    },
  },
  methods: {
    // 「キャンセル」ボタン押下時の処理
    onClickCancel() {
      this.$router.push(this.backUrlCancel)
    },

    // 「一覧へ戻る」ボタン押下時の処理
    onClickBackToList() {
      this.$router.push(this.backUrl)
    },

    // 「一時保存」ボタン押下時の処理
    onClickTemporarySave() {
      this.isTemporarySaved = true
      this.$emit('click:saveTemporary')
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
      this.$emit('inputForm', newValue)
      this.isChanged = true
    },

    // 「申請する」ボタン押下時の処理
    onClickApply() {
      this.$emit('click:apply')
    },

    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      this.isDisabled = !newValue
    },
    // 子コンポーネントから送られた値でフラグ更新
    handleTemporaryDisabledUpdate(isTemporaryDisabled: boolean) {
      // 子コンポーネントから送られたデータを親で更新
      this.isTemporaryDisabled = isTemporaryDisabled
    },
  },
})
</script>

<style lang="scss" scoped>
.fixed-nav-button {
  position: fixed;
  bottom: 0px;
  left: 50%;
  transform: translateX(-50%);
  border: none;
  z-index: 1000;
}
</style>
