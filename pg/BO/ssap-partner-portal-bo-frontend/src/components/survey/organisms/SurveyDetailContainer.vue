<template>
  <DetailContainer
    :title="titleDate + ' ' + title"
    :is-editing="isEditing"
    note-head="false"
    :is-valid="isValid"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="onClickPositive"
    @click:negative="onClickNegative"
  >
    <SurveyDetailEdit
      v-model="isValid"
      :survey="survey"
      v-on="$listeners"
      @update="update"
    />
    <SurveyDetailSection :title="$t('survey.pages.detail.infomation.name')">
      <SurveyDetailInfomation :survey="survey" />
    </SurveyDetailSection>
    <SurveyDetailSection :title="$t('survey.pages.detail.answers.name')">
      <SurveyDetailQuestion :survey="survey" :survey-master="surveyMaster" />
    </SurveyDetailSection>
  </DetailContainer>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { Sheet } from '~/components/common/atoms'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyDetailEdit from '~/components/survey/molecules/SurveyDetailEdit.vue'
import SurveyDetailRows from '~/components/survey/molecules/SurveyDetailRows.vue'
import SurveyDetailSection from '~/components/survey/molecules/SurveyDetailSection.vue'
import SurveyDetailInfomation from '~/components/survey/molecules/SurveyDetailInfomation.vue'
import SurveyDetailQuestion from '~/components/survey/molecules/SurveyDetailQuestion.vue'
import { GetSurveyByIdResponse } from '~/models/Survey'
import { GetSurveyMasterByIdAndRevisionResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    Sheet,
    DetailContainer,
    SurveyDetailEdit,
    SurveyDetailRows,
    SurveyDetailSection,
    SurveyDetailInfomation,
    SurveyDetailQuestion,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
    /** 編集中か */
    isEditing: {
      type: Boolean,
    },
    /** 作成中か */
    isCreating: {
      type: Boolean,
    },
    /** アンケート情報 */
    survey: {
      type: Object as PropType<GetSurveyByIdResponse>,
      required: true,
    },
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    surveyInfo: {
      type: Object,
    },
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    /**
     * 表示タイトル用の日付
     * @returns フォーマット済み日付文字列
     */
    titleDate() {
      return format(
        new Date(this.survey.summaryMonth),
        this.$t('common.format.date_ym') as string
      )
    },
  },
  data() {
    return {
      isValid: true,
    }
  },
  methods: {
    /** アンケート情報を保存 */
    onClickPositive() {
      this.$emit('save')
    },
    /** アンケート情報の編集をキャンセル */
    onClickNegative() {
      this.$emit('cancel')
    },
    /**
     * 他コンポーネントへアンケート情報の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>
