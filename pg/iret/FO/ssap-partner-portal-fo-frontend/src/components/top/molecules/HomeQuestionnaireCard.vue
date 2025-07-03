<template>
  <CommonWideClickableCard
    v-if="!survey.isFinished"
    class="m-HomeQuestionnaireCard"
    :name="
      createSurveyName(survey.actualSurveyRequestDatetime, survey.surveyType)
    "
    :content="[
      `${$t(
        'top.pages.home.surveys.planSurveyResponseDatetime'
      )}：${getLimitDate}`,
    ]"
    :to="forwardToUrl(`/survey/pp/${survey.id}`)"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import ja from 'date-fns/locale/ja'
import { getCurrentDate } from '~/utils/common-functions'
import CommonWideClickableCard from '~/components/common/molecules/CommonWideClickableCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { SurveyByMineItem } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    CommonWideClickableCard,
  },
  props: {
    /**
     * アンケート情報
     */
    survey: {
      type: Object as PropType<SurveyByMineItem>,
    },
  },
  computed: {
    /**
     * 回答期限日の書式整形
     */
    getLimitDate() {
      // @ts-ignore
      return this.survey.planSurveyResponseDatetime
        ? format(
            new Date(this.survey.planSurveyResponseDatetime),
            'yyyy/MM/dd（E）',
            { locale: ja }
          )
        : ''
    },
  },
  methods: {
    /**
     * アンケート名の生成
     */
    createSurveyName(dateTime: string, surveyType: string): string {
      return (format(
        new Date(dateTime),
        this.$t('common.format.date_ym') as string
      ) +
        '　' +
        this.$t('survey.group_info.surveyNameList.' + surveyType)) as string
    },
    /**
     * アンケートの回答期限日を過ぎていないかの確認
     */
    checkDate() {
      const now = getCurrentDate()
      const planSurveyResponseDatetime = new Date(
        this.survey.planSurveyResponseDatetime
      )
      const checkDateResult = now <= planSurveyResponseDatetime
      return checkDateResult
    },
  },
})
</script>
