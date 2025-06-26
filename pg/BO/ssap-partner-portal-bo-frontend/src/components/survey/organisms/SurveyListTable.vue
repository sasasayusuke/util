<template>
  <CommonDataTable
    :headers="surveyHeaders"
    :items="formattedSurveys"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :page="offsetPage"
    :total="total"
    :limit="limit"
    :is-loading="isLoading"
    :loads-every-time="false"
    class="o-survey-list-table"
    @click:prev="$emit('click:prev')"
    @click:next="$emit('click:next')"
  >
    <template #[`item.surveyType`]="{ item }">
      <nuxt-link :to="`/survey/${item.id}`">{{ item.surveyType }}</nuxt-link>
    </template>
  </CommonDataTable>
</template>

<script lang="ts">
import { format } from 'date-fns'
import CommonDataTable, {
  IDataTableHeader,
} from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    CommonDataTable,
  },
  props: {
    /** アンケート情報一覧 */
    surveys: {
      type: Array,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 現在のページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 最終ページ */
    maxPage: {
      type: Number,
      required: true,
    },
    /** 全件数 */
    total: {
      type: Number,
      required: true,
    },
    /** 1ページの表示上限数 */
    limit: {
      type: Number,
      required: true,
    },
  },
  data(): { surveyHeaders: IDataTableHeader[] } {
    return {
      surveyHeaders: [
        {
          text: this.$t('survey.pages.index.header.serviceTypeName'),
          align: 'start',
          value: 'serviceTypeName',
          width: 112,
          maxLength: 12,
          sort: (a: string, b: string) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.index.header.customerName'),
          value: 'customerName',
          width: 216,
          maxLength: 18,
          sort: (a: string, b: string) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.index.header.projectName'),
          value: 'projectName',
          width: 196,
          maxLength: 12,
          sort: (a: string, b: string) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.index.header.surveyType'),
          value: 'surveyType',
          width: 196,
          maxLength: 12,
        },
        {
          text: this.$t(
            'survey.pages.index.header.actualSurveyResponseDatetime'
          ),
          value: 'actualSurveyResponseDatetime',
          width: 80,
        },
        {
          text: this.$t('survey.pages.index.header.answerUserName'),
          value: 'answerUserName',
          width: 120,
          maxLength: 8,
          sort: (a: string, b: string) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.index.header.isDisclosure'),
          value: 'isDisclosure',
          width: 84,
        },
        {
          text: this.$t('survey.pages.index.header.summaryMonth'),
          value: 'summaryMonth',
          width: 73,
        },
        {
          text: this.$t('survey.pages.index.header.isNotSummary'),
          value: 'isNotSummary',
          width: 84,
        },
      ],
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換したアンケート情報を返す
     * @returns フォーマット済みアンケート情報配列
     */
    formattedSurveys(this: any) {
      const formatDate = this.formatDate

      return this.surveys.map((survey: any) => {
        //アンケート名
        const surveyType = (this.$t(
          `survey.pages.index.row.type.${survey.surveyType}`
        ) + this.$t('survey.pages.index.surveyName')) as string

        const formattedSummaryMonth = format(
          new Date(survey.summaryMonth),
          this.$t('common.format.date_ym') as string
        )
        survey.surveyType = formattedSummaryMonth + surveyType
        //開示承認
        if (survey.isDisclosure === true) {
          survey.isDisclosure = this.$t('survey.pages.index.label.yes')
        } else {
          survey.isDisclosure = this.$t('survey.pages.index.label.no')
        }
        //集計対象
        if (survey.isNotSummary === true) {
          survey.isNotSummary = this.$t('survey.pages.index.label.no')
        } else {
          survey.isNotSummary = this.$t('survey.pages.index.label.yes')
        }
        //回答日
        if (survey.actualSurveyResponseDatetime) {
          survey.actualSurveyResponseDatetime = formatDate(
            new Date(survey.actualSurveyResponseDatetime)
          ).split(' ')[0]
        }
        //集計月
        survey.summaryMonth = survey.summaryMonth.replace('/', '.')
        return survey
      })
    },
  },
})
</script>

<style lang="scss">
.o-survey-list-table {
  td {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    &:nth-child(1) {
      max-width: 115px;
    }
    &:nth-child(2) {
      max-width: 260px;
    }
    &:nth-child(3),
    &:nth-child(4) {
      max-width: 190px;
    }
  }
}
</style>
