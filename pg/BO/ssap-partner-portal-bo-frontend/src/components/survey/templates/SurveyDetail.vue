<template>
  <RootTemPlate>
    <SurveyDetailContainer
      v-if="!isError"
      :title="switchSurveyType(survey.surveyType)"
      :is-editing="isEditing"
      :is-loading-button="isLoadingButton"
      :survey="survey"
      :survey-master="surveyMaster"
      :survey-info="surveyInfo"
      @save="save"
      @cancel="cancel"
      @update="emitSurvey"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { Sheet, Title, Table } from '~/components/common/atoms'
import SurveyDetailContainer from '~/components/survey/organisms/SurveyDetailContainer.vue'
import {
  GetSurveyByIdResponse,
  UpdateSurveyByIdRequest,
  UpdateSurveyById,
} from '~/models/Survey'
import { GetSurveyMasterByIdAndRevisionResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    RootTemPlate,
    Sheet,
    Title,
    Table,
    SurveyDetailContainer,
  },
  props: {
    /** アンケート情報 */
    survey: {
      type: Object as PropType<GetSurveyByIdResponse>,
      required: true,
    },
    /** アンケートマスター情報 */
    surveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    surveyInfo: {
      type: Object,
    },
    /** 取得時にエラーが出ているか */
    isError: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isEditing: true,
      localSurvey: this.survey,
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 指定したアンケート種別の表示タイトルを返す
     * @param type アンケート種別
     * @returns 表示タイトルと文字列
     */
    switchSurveyType(type: string): string {
      switch (type) {
        case 'service':
          return this.$t('survey.pages.index.table.service.name') as string
        case 'completion':
          return this.$t('survey.pages.index.table.completion.name') as string
        case 'quick':
          return this.$t('survey.pages.index.table.quick.name') as string
        case 'pp':
          return this.$t('survey.pages.index.table.pp.name') as string
        default:
          return ''
      }
    },
    /**
     * 他コンポーネントからアンケート情報の変更を受け取る
     * @param item 変更項目
     * @param event 変更値
     */
    emitSurvey(item: string, event: any) {
      this.$set(this.localSurvey, item, event)
    },
    /** アンケート情報を変更しアンケート一覧を表示 */
    save() {
      this.isLoadingButton = true
      this.update()
        .then(() => {
          this.$router.push(`/survey/list`)
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    /** アンケート情報の編集をキャンセル */
    cancel() {
      this.$router.push(this.backToUrl(`/survey/list`))
    },
    /**
     * アンケート情報を変更
     * @return UpdateSurveyByIdAPI実行時のPromiseオブジェクト
     */
    update() {
      const request = new UpdateSurveyByIdRequest()
      request.summaryMonth = this.localSurvey.summaryMonth
      request.isNotSummary = this.localSurvey.isNotSummary
      request.isSolverProject = this.localSurvey.isSolverProject
      return UpdateSurveyById(
        this.localSurvey.id,
        this.localSurvey.version,
        request
      )
    },
  },
})
</script>
