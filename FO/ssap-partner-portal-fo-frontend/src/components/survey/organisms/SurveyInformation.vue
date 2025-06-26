<template>
  <SurveyInformationContainer
    :title="title"
    :text="limitDate"
    :survey-type="survey.surveyType ? survey.surveyType : ''"
    :project-id="survey.projectId ? survey.projectId : ''"
    :token="token"
    :type="type"
    :is-anonymous="isAnonymous"
    :is-satisfaction="isSatisfaction"
    @click:start="$emit('click:start')"
  >
    <SurveyInformationList
      v-if="type === 'input-before-start' || type === 'input'"
      :type="type"
      :survey="filteredSurvey"
      :introduction="introduction"
    />
    <SurveyInputFinished
      v-if="type === 'finished'"
      :finish-text1="finishText1"
      :finish-text2="finishText2"
    />
  </SurveyInformationContainer>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import SurveyInformationContainer from '~/components/survey/organisms/SurveyInformationContainer.vue'
import SurveyInformationList from '~/components/survey/organisms/SurveyInformationList.vue'
import SurveyInputFinished from '~/components/survey/organisms/SurveyInputFinished.vue'
import SurveySummaryHead from '~/components/survey/molecules/SurveySummaryHead.vue'
import { SurveyInfo } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    SurveyInformationContainer,
    SurveyInformationList,
    SurveyInputFinished,
    SurveySummaryHead,
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
    /** 満足度評価のみアンケートか否か */
    isSatisfaction: {
      type: Boolean,
      default: false,
    },
    /** 匿名アンケートの場合に使用する認証トークン */
    token: {
      type: String,
      default: '',
    },
  },
  computed: {
    /**
     * アンケートタイプ別のアンケート名を生成
     * @return アンケート名
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

        // 集計月からスラッシュを取り除く
        const formattedSummaryMonth = this.survey.summaryMonth.replace(
          /\//g,
          ''
        )

        // 年と月を取り出す
        const year = formattedSummaryMonth.slice(0, 4)
        const month = formattedSummaryMonth.slice(4, 6)

        // Dateオブジェクトを正しく作成する（1日は任意に設定）
        // monthは0から始まるため-1
        const date = new Date(Number(year), Number(month) - 1)
        const formatYearAndMonth = this.$t('common.format.date_ym') as string

        const formattedDate = format(
          new Date(date),
          formatYearAndMonth.replace('Y', 'yyyy').replace('M', 'M') // "Y年M月" → "yyyy年M月"
        )

        surveyName = formattedDate + ' ' + surveyName
      }
      return surveyName
    },
    /**
     * @return 回答期限
     */
    limitDate(): string {
      const dateTimeAry = this.survey.planSurveyResponseDatetime
        ? this.survey.planSurveyResponseDatetime.split(' ')
        : 'ー'
      return (
        this.$t('survey.terminology.surveyLimitDate') + ' : ' + dateTimeAry[0]
      )
    },
    /**
     * アンケートタイプ別の説明を生成
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
     * アンケートタイプ別で案件アンケート生成
     */
    filteredSurvey(): any {
      if (this.survey.id) {
        // プロデューサー
        if (this.survey.mainSupporterUser) {
          this.survey.mainSupporterUserName = this.survey.mainSupporterUser.name
        }

        // アクセラレータ―
        if (this.survey.supporterUsers) {
          this.survey.supportersStr = this.survey.supporterUsers
            .map((supporterUser) => {
              return supporterUser.name
            })
            .join(' ／ ')
        }

        // 対象期間
        this.survey.surveyTargetDuration = ''
        // 集計月からスラッシュを取り除く
        const formattedSummaryMonth = this.survey.summaryMonth.replace(
          /\//g,
          ''
        )

        // 年と月を取り出す
        const year = formattedSummaryMonth.slice(0, 4)
        const month = formattedSummaryMonth.slice(4, 6)
        // Dateオブジェクトを正しく作成する（1日は任意に設定）
        // monthは0から始まるため-1
        const date = new Date(Number(year), Number(month) - 1)

        switch (this.survey.surveyType) {
          case 'service':
          case 'quick':
          case 'pp':
            this.survey.surveyTargetDuration = format(
              new Date(date),
              this.$t('common.format.date_ym2') as string
            )
            break
          case 'completion':
            this.survey.surveyTargetDuration =
              this.survey.supportDateFrom + ' ~ ' + this.survey.supportDateTo
            break
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
     * @return アンケート回答後の締めの言葉を生成
     */
    finishText1(): string {
      let text = ''
      if (this.survey.surveyType) {
        switch (this.survey.surveyType) {
          case 'service':
            text = this.$t(
              'survey.group_info.surveyFinishedList.service1'
            ) as string
            break
          case 'completion':
            text = this.$t(
              'survey.group_info.surveyFinishedList.completion1'
            ) as string
            break
          case 'quick':
            text = this.$t(
              'survey.group_info.surveyFinishedList.quick1'
            ) as string
            break
          case 'pp':
            text = this.$t('survey.group_info.surveyFinishedList.pp1') as string
            break
        }
      } else {
        // 満足度評価のみ回答アンケートの場合
        text = this.$t(
          'survey.group_info.surveyFinishedList.service1'
        ) as string
      }
      return text
    },
    /**
     * @return アンケート回答後の締めの言葉を生成
     */
    finishText2(): string {
      let text = ''
      if (this.survey.surveyType) {
        switch (this.survey.surveyType) {
          case 'service':
            text = this.$t(
              'survey.group_info.surveyFinishedList.service2'
            ) as string
            break
          case 'completion':
            text = this.$t(
              'survey.group_info.surveyFinishedList.completion2'
            ) as string
            break
          case 'quick':
            text = this.$t(
              'survey.group_info.surveyFinishedList.quick2'
            ) as string
            break
          case 'pp':
            text = this.$t('survey.group_info.surveyFinishedList.pp2') as string
            break
        }
      } else {
        // 満足度評価のみ回答アンケートの場合
        text = this.$t(
          'survey.group_info.surveyFinishedList.service2'
        ) as string
      }
      return text
    },
  },
})
</script>
