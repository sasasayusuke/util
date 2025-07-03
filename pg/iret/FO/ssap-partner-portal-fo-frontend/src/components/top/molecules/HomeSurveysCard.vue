<template>
  <Card elevation="3">
    <SimpleTable>
      <CommonWideClickableCard
        :name="`${survey.projectName}`"
        :content="[
          `${actualSurveyRequestDatetime} ${getSurveyTypeString}`,
          `${$t(
            'top.pages.home.surveys.planSurveyResponseDatetime'
          )}：${planSurveyResponseDatetime}`,
        ]"
        :to="forwardToUrl(`/survey/${survey.id}`)"
      />
    </SimpleTable>
  </Card>
</template>

<script lang="ts">
import { format } from 'date-fns'
import ja from 'date-fns/locale/ja'
import CommonWideClickableCard from '~/components/common/molecules/CommonWideClickableCard.vue'
import BaseComponent from '~/common/BaseComponent'
import { Card, SimpleTable } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonWideClickableCard,
    Card,
    SimpleTable,
  },
  props: {
    /**
     * 未回答アンケート情報
     */
    survey: {
      type: Object,
    },
  },
  computed: {
    /**
     * アンケート名の生成
     */
    getSurveyTypeString() {
      return this.$t(
        'survey.group_info.surveyNameList.' + this.survey.surveyType
      ) as string
    },
    /**
     * アンケート送信日の書式整形
     */
    actualSurveyRequestDatetime() {
      return format(
        new Date(this.survey.actualSurveyRequestDatetime),
        this.$t('common.format.date_ym') as string
      )
    },
    /**
     * アンケート回答期限日の書式整形
     */
    planSurveyResponseDatetime() {
      // @ts-ignore
      if (this.survey.planSurveyResponseDatetime) {
        return format(
          new Date(this.survey.planSurveyResponseDatetime),
          this.$t('common.format.date_ymd3') as string,
          { locale: ja }
        )
      } else {
        return 'ー'
      }
    },
  },
})
</script>
