<template>
  <div>
    <DetailContainer
      :title="title"
      :is-editing="isEditing"
      :note-head="noteHead"
      :is-valid="true"
      :is-hide-button1="isHideButton1"
      :is-hide-button2="isHideButton2"
      :is-hide-button3="isTemporarySaved"
      :is-project="false"
      :is-loading-button="isLoadingButton"
      :is-loading-button2="isLoadingButton2"
      :is-solver="false"
      :is-solver-detail="isSolverDetail"
      :is-solver-application="isSolverApplication"
      :is-disabled-button1="isDisabled"
      :is-disabled-button3="isTemporaryDisabled"
      :is-temporary-save="isTemporarySave"
      style="margin-top: 30px"
      @click:positive="$emit('click:positive')"
      @click:negative="$emit('click:negative')"
      @buttonAction3="$emit('buttonAction3')"
      @buttonAction4="$emit('buttonAction4')"
      @buttonAction5="$emit('buttonAction5')"
      @buttonAction6="$emit('buttonAction6')"
      @temporarySaveAction="$emit('temporarySaveAction')"
    >
      <SolverApplicationRows
        v-model="isValid"
        :is-editing="isEditing"
        :solver-corporation-id="solverCorporationId"
        :solver="solver"
        :is-new="isNew"
        :issue-map50="issueMap50"
        :tsi-areas="tsiAreas"
        :is-display-application-project="isDisplayApplicationProject"
        :is-temporary-disabled="isTemporaryDisabled"
        :file-refresh="fileRefresh"
        @inputForm="onInputForm"
        @change:isValid="onChangeIsValid"
        @updateTemporaryDisabled="handleTemporaryDisabledUpdate"
      />
      <template #footerButton>
        <slot name="footerButton" />
      </template>
    </DetailContainer>
    <slot name="snackBar" />
  </div>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import SolverApplicationRows from '~/components/solver/molecules/SolverApplicationRows.vue'
import { PropType } from '~/common/BaseVueClass'
import { IGetSelectItemsResponse } from '~/types/Master'
import { IGetSolverByIdResponse } from '~/types/Solver'
import SolverDeleteModal from '~/components/solver/molecules/SolverDeleteModal.vue'

export default CommonDetailContainer.extend({
  name: 'SolverApplicationContainer',
  components: {
    DetailContainer,
    SolverApplicationRows,
    SolverDeleteModal,
  },
  props: {
    // 個人ソルバー情報
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: false,
    },
    title: {
      type: String,
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
    isDisabled: {
      type: Boolean,
      default: true,
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
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    isLoadingButton2: {
      type: Boolean,
      default: false,
    },
    isSolverDetail: {
      type: Boolean,
      default: false,
    },
    isSolverApplication: {
      type: Boolean,
      default: true,
    },
    isTemporarySaved: {
      type: Boolean,
      default: false,
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
    isDisplayApplicationProject: {
      type: Boolean,
      default: true,
    },
    // 一時保存ボタン非活性フラグ
    isTemporaryDisabled: {
      type: Boolean,
      default: true,
    },
    // 一時保存ボタン表示フラグ
    isTemporarySave: {
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
    }
  },
  methods: {
    // formの入力値を保持する処理
    onInputForm(newValue: any) {
      this.$emit('inputForm', newValue)
    },

    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      if (this.isEditing) {
        this.isValid = newValue
      } else {
        this.isValid = true
      }
      this.$emit('change:isValid', newValue)
    },
    // 子コンポーネントから送られた値でをフラグ更新
    handleTemporaryDisabledUpdate(isTemporaryDisabled: boolean) {
      // 子コンポーネントから送られたデータを親で更新
      this.$emit('updateTemporaryDisabled', isTemporaryDisabled)
    },
  },
})
</script>
