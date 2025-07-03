<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :note-head="noteHead"
    :is-valid="isValidWithChange"
    :is-hide-button1="true"
    :is-hide-button2="true"
    :is-project="true"
    :is-loading-button="isLoadingButton"
    :is-solver="false"
    :is-hide-footer="true"
    style="margin-top: 30px"
    @click:positive="$emit('click:positive')"
    @click:negative="$emit('click:negative')"
  >
    <SolverCommonDetailRows
      :solver-corporations="solverCorporations"
      :is-editing="isEditing"
      @change:solverCorporation="keepCorporateId"
      @click:displayProjectName="displayProjectName"
      @change:isValid="onChangeIsValid"
    />
  </DetailContainer>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import SolverCommonDetailRows from '~/components/solver/molecules/SolverCommonDetailRows.vue'
import { PropType } from '~/common/BaseVueClass'
import { IGetSolverCorporationsResponse } from '~/types/SolverCorporation'

export default CommonDetailContainer.extend({
  name: 'SolverDetailCommonContainer',
  components: {
    DetailContainer,
    SolverCommonDetailRows,
  },
  props: {
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
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    solverCorporations: {
      type: Object as PropType<IGetSolverCorporationsResponse>,
      required: true,
    },
  },
  data() {
    return {
      isValid: false,
      isDeleteModalOpen: false,
    }
  },
  methods: {
    // formの入力値（法人ID）を保持する処理
    keepCorporateId(newValue: string) {
      this.$emit('change:solverCorporation', newValue)
    },

    // 案件名を表示する処理
    displayProjectName(newValue: { projectId: string; projectName: string }) {
      this.$emit('click:displayProjectName', newValue)
    },

    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      this.$emit('change:isValidCommon', newValue)
    },
  },
})
</script>
