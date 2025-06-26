<template>
  <SurveyInformationContainer
    :title="title"
    :text="limitDate"
    :survey-type="survey.surveyType ? survey.surveyType : ''"
    :project-id="survey.projectId ? survey.projectId : ''"
    :token="token"
    :type="type"
    :is-anonymous="isAnonymous"
  >
    <template #head>
      <SurveySummaryHead
        :summary-month="survey.summaryMonth"
        :is-target="targetSummary"
        :is-solver-project="isSolverProject"
        :survey-type="survey.surveyType ? survey.surveyType : ''"
      />
    </template>

    <!-- default -->
    <SurveyInformationList
      type="detail"
      :survey="filteredSurvey"
      :introduction="introduction"
      :is-anonymous="isAnonymous"
    />

    <!-- answers -->
    <template #detail>
      <SurveyDetailAnswers
        type="detail"
        :is-disclosure="survey.isDisclosure"
        :questions="questions"
        :disclosure="disclosure"
        :answers="survey.answers"
        :question-index-flow="questionIndexFlow"
        :survey-type="survey.surveyType ? survey.surveyType : ''"
      />
    </template>
  </SurveyInformationContainer>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import SurveyInformationContainer from '~/components/survey/organisms/SurveyInformationContainer.vue'
import SurveySummaryHead from '~/components/survey/molecules/SurveySummaryHead.vue'
import SurveyInformationList from '~/components/survey/organisms/SurveyDetailInformationList.vue'
import SurveyDetailAnswers from '~/components/survey/organisms/SurveyDetailAnswers.vue'
import { SurveyInfo, QuestionInfo } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    SurveyInformationContainer,
    SurveySummaryHead,
    SurveyInformationList,
    SurveyDetailAnswers,
  },
  props: {
    /** 案件アンケート */
    survey: {
      type: Object as PropType<SurveyInfo>,
      default: new SurveyInfo(),
    },
    type: {
      type: String,
      default: '',
    },
    /** 匿名アンケートか否か */
    isAnonymous: {
      type: Boolean,
      default: false,
    },
    /** 匿名アンケートの場合に使用する認証トークン */
    token: {
      type: String,
      default: '',
    },
    /** 設問一覧 */
    questions: {
      type: Array as PropType<Array<QuestionInfo>>,
      default: [],
    },
    /** 開示設問 */
    disclosure: {
      type: Object,
      default: {},
    },
    /** 設問インデックスの配列 */
    questionIndexFlow: {
      type: Array as PropType<Array<number>>,
      default: [],
    },
  },
  computed: {
    /**
     * アンケートタイプによってタイトルを変更
     * @return タイトル名
     */
    title(): string {
      let surveyName = ''
      if (this.survey.surveyType) {
        switch (this.survey.surveyType) {
          case 'service':
            surveyName = this.$t(
              'survey.group_info.surveyNameList.service'
            ) as string
            break
          case 'completion':
            surveyName = this.$t(
              'survey.group_info.surveyNameList.completion'
            ) as string
            break
          case 'quick':
            surveyName = this.$t(
              'survey.group_info.surveyNameList.quick'
            ) as string
            break
          case 'pp':
            surveyName = this.$t(
              'survey.group_info.surveyNameList.pp'
            ) as string
            break
        }

        surveyName =
          format(
            new Date(this.survey.summaryMonth),
            this.$t('common.format.date_ym') as string
          ) +
          ' ' +
          surveyName
      }
      return surveyName
    },
    /**
     * 期限を取得
     * @return 期限日
     */
    limitDate(): string {
      const date = this.survey.planSurveyResponseDatetime

      if (!date) {
        return ''
      }

      return (
        this.$t('survey.terminology.surveyLimitDate') +
        ': ' +
        format(
          new Date(this.survey.planSurveyResponseDatetime),
          this.$t('common.format.date_ymd') as string
        )
      )
    },
    /**
     * アンケートタイプによって説明を更新
     */
    introduction(): string {
      let text = ''
      if (this.survey.surveyType) {
        switch (this.survey.surveyType) {
          case 'service':
            text = this.$t(
              'survey.group_info.surveyIntroductionList.service'
            ) as string
            break
          case 'completion':
            text = this.$t(
              'survey.group_info.surveyIntroductionList.completion'
            ) as string
            break
          case 'quick':
            text = this.$t(
              'survey.group_info.surveyIntroductionList.quick'
            ) as string
            break
          case 'pp':
            text = this.$t(
              'survey.group_info.surveyIntroductionList.pp'
            ) as string
            break
        }
      }
      return text
    },
    /**
     * 案件アンケートをアンケートタイプ別に生成
     */
    filteredSurvey(): SurveyInfo {
      if (this.survey.id) {
        // プロデューサー
        if (this.survey.mainSupporterUser) {
          this.survey.mainSupporterUserName = this.survey.mainSupporterUser.name
        }

        // アクセラレータ―
        // ※注
        // バックエンドから返される値に、現状nullが入っている(想定としては空配列)
        // 空配列が返るようになったら、下記のif文によるネストを削除し、map処理を外に出すこと
        if (this.survey.supporterUsers) {
          this.survey.supportersStr = this.survey.supporterUsers
            .map((supporterUser) => {
              return supporterUser.name
            })
            .join(' ／ ')
        }

        // 対象期間
        this.survey.surveyTargetDuration = ''
        switch (this.survey.surveyType) {
          case 'service':
          case 'quick':
          case 'pp':
            this.survey.surveyTargetDuration = format(
              new Date(this.survey.summaryMonth),
              this.$t('common.format.date_ym2') as string
            )
            break
          case 'completion':
            this.survey.surveyTargetDuration =
              this.survey.supportDateFrom + ' ~ ' + this.survey.supportDateTo
            break
        }
        // 回答日
        if (this.survey.actualSurveyResponseDatetime === null) {
          this.survey.actualSurveyResponseDatetime = '-'
        } else {
          this.survey.actualSurveyResponseDatetime = format(
            new Date(this.survey.actualSurveyResponseDatetime),
            this.$t('common.format.date_ymd5') as string
          )
        }

        // 表示用の所属会社(顧客の場合は取引先名,顧客以外の場合は所属会社)
        if (this.survey.customerName !== null) {
          this.survey.surveyCompany = this.survey.customerName
        } else if (this.survey.company !== null) {
          this.survey.surveyCompany = this.survey.company
        } else {
          this.survey.surveyCompany = ''
        }
      }
      return this.survey
    },
    /**
     * 集計対象の文章を生成
     * @return 集計対象の有無
     */
    targetSummary(): string {
      const targetSummary = this.survey.isNotSummary
        ? this.$t('survey.pages.detail.is_not_summary')
        : this.$t('survey.pages.detail.is_summary')
      return targetSummary as string
    },
    /**
     * ソルバー担当の文章を生成
     * @return ソルバー担当の有無
     */
    isSolverProject(): string {
      const isSolverProject = this.survey.isSolverProject
        ? this.$t('common.detail.yes')
        : this.$t('common.detail.no')
      return isSolverProject as string
    },
  },
})
</script>
