<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValid"
    :is-disabled-button1="isDisabled"
    :is-hide-footer="false"
    :is-hide-button2="!isEditing"
    :is-solver-corporation="true"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive', LocalSolverCorporation)"
    @click:negative="$emit('click:negative')"
    @buttonAction3="backToSolverList"
  >
    <SolverCorporationDetailRows
      v-model="isValid"
      :solver-corporation="solverCorporation"
      :issue-map50="issueMap50"
      :is-editing="isEditing"
      :is-loading="isLoading"
      @update="update('LocalSolverCorporation', $event)"
      @inputForm="onInputForm"
      @change:isValid="onChangeIsValid"
    />
    <template #footerButton>
      <slot name="footerButton" />
    </template>
  </DetailContainer>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import SolverCorporationDetailRows, {
  LocalSolverCorporation,
} from '~/components/solver-corporation/molecules/SolverCorporationDetailRows.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import { GetSolverCorporationByIdResponse } from '~/models/SolverCorporation'
import { IGetSelectItemsResponse } from '~/types/Master'
// import { isLoading } from '~/components/customer/templates/CustomerList.vue'
// 更新用データ
export { LocalSolverCorporation }

export interface isLoading {
  selectItem: boolean
  solverCorporation: boolean
}

class LocalCorporateInfoDocument {
  path: string = ''
  fileName: string = ''
}

export default CommonDetailContainer.extend({
  name: 'SolverCorporationDetailContainer',
  components: {
    DetailContainer,
    SolverCorporationDetailRows,
  },
  props: {
    /** 選択した法人ソルバー情報 */
    solverCorporation: {
      type: Object as PropType<GetSolverCorporationByIdResponse>,
      required: true,
    },
    /** 課題マップ50 */
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    isDisabled: {
      type: Boolean,
      default: true,
    },
    noteHead: {
      type: String,
      default: '',
    },
    /** 法人ソルバー情報を保存/更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isValid: false,
      /** 編集用の法人ソルバー内容 */
      LocalSolverCorporation: new LocalSolverCorporation(),
      /** 会社案内資料の内容 */
      localCorporateInfoDocument: new LocalCorporateInfoDocument(),
    }
  },
  methods: {
    // 「ソルバートップへ戻る」ボタン押下時の処理
    backToSolverList() {
      this.$router.push(`/solver/menu`)
    },
    // formの入力値を保持する処理
    onInputForm(newValue: any) {
      this.$emit('inputForm', newValue)
    },
    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      this.$emit('change:isValid', newValue)
    },
  },
  computed: {
    /** ページによって、タイトルを変更 */
    title() {
      return this.$t('solver-corporation.pages.' + this.mode + '.name')
    },
  },
})
</script>
