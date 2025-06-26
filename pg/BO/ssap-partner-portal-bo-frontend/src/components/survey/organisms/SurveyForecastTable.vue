<template>
  <CommonDataTable
    ref="table"
    type="forecast"
    :value="selectedSurveys"
    :is-loading="isLoading"
    :headers="surveyHeaders"
    :items="formattedSurveys"
    :offset-page="offsetPage"
    :max-page="maxPage"
    :total="total"
    :is-hide-buttons="true"
    :shows-pagination="false"
    :sort-by="sortBy"
    :sort-desc="sortDesc"
    :show-select="showSelect"
    :is-single-month="isSingleMonth"
    :is-select-rows="true"
    item-key="index"
    short-text
    link-prefix="/surveys"
    class="o-survey-forecast-table"
    @click:prev="$emit('click:prev')"
    @click:next="$emit('click:next')"
    @update:sort-items="emitSortItems"
    @update:sort-by="emitSortBy"
    @update:sort-desc="emitSortDesc"
    @click:toggle-show-select="$emit('click:toggle-show-select')"
    @change:selected-rows="$listeners['change:selected-rows']"
    @click:open-modal-bulk-edit="$emit('click:open-modal-bulk-edit')"
    @click:open-modal-bulk-delete="$emit('click:open-modal-bulk-delete')"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import { format } from 'date-fns'
import CommonDataTable, {
  IDataTableHeader,
} from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import { SurveyPlanListItem } from '~/models/Survey'
import type { PropType } from '~/common/BaseComponent'

export type SurveyForecastTableType = {
  clearSelectRows(): void
}
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
    /** 現在のページ */
    offsetPage: {
      type: Number,
    },
    /** 最終ページ */
    maxPage: {
      type: Number,
    },
    /** 全件数 */
    total: {
      type: Number,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    sortBy: {
      type: Array as PropType<String[]> | String as PropType<String>,
    },
    sortDesc: {
      type: Array as PropType<Boolean[]> | Boolean as PropType<Boolean>,
    },
    showSelect: {
      type: Boolean,
    },
    isSingleMonth: {
      type: Boolean,
    },
    selectedSurveys: {
      type: Array as PropType<SurveyPlanListItem[]>,
    },
  },
  data(): { surveyHeaders: IDataTableHeader[] } {
    if (this.$route.path.includes('survey-ops')) {
      // URLに「survey-ops」が含まれている場合のヘッダー
      return {
        surveyHeaders: [
          {
            text: this.$t('survey.pages.forecast.header.salesUserName'),
            value: 'salesUserName',
            maxLength: 6,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.serviceTypeName'),
            align: 'start',
            value: 'serviceTypeName',
            maxLength: 5,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.customerName'),
            value: 'customerName',
            maxLength: 11,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.projectName'),
            value: 'projectName',
            maxLength: 12,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
            link: {
              prefix: '/project/',
              value: 'projectId',
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyType'),
            value: 'surveyType',
            maxLength: 10,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.thisMonthType'),
            value: 'thisMonthType',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.noSendReason'),
            value: 'noSendReason',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.planSurveyRequestDatetime'
            ),
            value: 'planSurveyRequestDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.planSurveyResponseDatetime'
            ),
            value: 'planSurveyResponseDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.unansweredSurveysNumber'
            ),
            value: 'unansweredSurveysNumber',
            class: 'th-ymd',
            sort: (a: string | number, b: string | number) => {
              const convert = (value: string | number) => {
                // 数値に変換できない場合は -Infinity を返す
                const num = Number(value)
                return isNaN(num) ? -Infinity : num
              }
              a = convert(a)
              b = convert(b)
              return a - b
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.mainSupporterUser'),
            value: 'mainSupporterUser',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.supporterUsers'),
            value: 'supporterUsers',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserType'),
            value: 'surveyUserType',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserName'),
            value: 'surveyUserName',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserEmail'),
            value: 'surveyUserEmail',
            maxLength: 18,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.customerSuccess'),
            value: 'customerSuccess',
            maxLength: 16,
            sortable: false,
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.actualSurveyRequestDatetime'
            ),
            value: 'actualSurveyRequestDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.actualSurveyResponseDatetime'
            ),
            value: 'actualSurveyResponseDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.summaryMonth'),
            value: 'summaryMonth',
            class: 'th-ym',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.supporterOrganizationName'
            ),
            value: 'supporterOrganizationName',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.supportDateFrom'),
            value: 'supportDateFrom',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.supportDateTo'),
            value: 'supportDateTo',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.phase'),
            value: 'phase',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.isCountManHour'),
            value: 'isCountManHour',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
        ],
      }
    } else {
      // 通常のヘッダー
      return {
        surveyHeaders: [
          {
            text: this.$t('survey.pages.forecast.header.salesUserName'),
            value: 'salesUserName',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.serviceTypeName'),
            align: 'start',
            value: 'serviceTypeName',
            maxLength: 12,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.customerName'),
            value: 'customerName',
            maxLength: 16,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.projectName'),
            value: 'projectName',
            maxLength: 12,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
            link: {
              prefix: '/project/',
              value: 'projectId',
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyType'),
            value: 'surveyType',
            maxLength: 12,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.mainSupporterUser'),
            value: 'mainSupporterUser',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.supporterUsers'),
            value: 'supporterUsers',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserType'),
            value: 'surveyUserType',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserName'),
            value: 'surveyUserName',
            maxLength: 7,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.surveyUserEmail'),
            value: 'surveyUserEmail',
            maxLength: 18,
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.customerSuccess'),
            value: 'customerSuccess',
            maxLength: 16,
            sortable: false,
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.planSurveyRequestDatetime'
            ),
            value: 'planSurveyRequestDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.actualSurveyRequestDatetime'
            ),
            value: 'actualSurveyRequestDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.planSurveyResponseDatetime'
            ),
            value: 'planSurveyResponseDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.actualSurveyResponseDatetime'
            ),
            value: 'actualSurveyResponseDatetime',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.summaryMonth'),
            value: 'summaryMonth',
            class: 'th-ym',
          },
          {
            text: this.$t(
              'survey.pages.forecast.header.supporterOrganizationName'
            ),
            value: 'supporterOrganizationName',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.supportDateFrom'),
            value: 'supportDateFrom',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.supportDateTo'),
            value: 'supportDateTo',
            class: 'th-ymd',
          },
          {
            text: this.$t('survey.pages.forecast.header.phase'),
            value: 'phase',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
          {
            text: this.$t('survey.pages.forecast.header.isCountManHour'),
            value: 'isCountManHour',
            sort: (a: string, b: string) => {
              a = a ?? ''
              b = b ?? ''
              return a.localeCompare(b, 'ja')
            },
          },
        ],
      }
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換したアンケート情報を返す
     * @returns フォーマット済みアンケート情報配列
     */
    formattedSurveys(): SurveyPlanListItem[] {
      return this.surveys.map((survey: any, index: number) => {
        survey.index = index
        survey.surveyType = survey.surveyType
          ? ((format(
              new Date(survey.summaryMonth),
              this.$t('common.format.date_ym') as string
            ) +
              ' ' +
              this.$t(
                'survey.pages.masterList.row.type.' + survey.surveyType
              )) as string)
          : (this.$t('survey.pages.forecast.row.survey_type.none') as string)
        survey.unansweredSurveysNumber =
          survey.unansweredSurveysNumber !== null
            ? survey.unansweredSurveysNumber !== 0
              ? survey.unansweredSurveysNumber
              : '0'
            : '-'

        survey.mainSupporterUser = survey.mainSupporterUser?.name || ''

        const supporterUserNames: string[] = []
        for (const i in survey.supporterUsers) {
          if (survey.supporterUsers[i] && survey.supporterUsers[i].name) {
            supporterUserNames.push(survey.supporterUsers[i].name)
          }
        }
        survey.supporterUsers = supporterUserNames.join('、')

        survey.isCountManHour = survey.isCountManHour ? 'する' : 'しない'

        survey.planSurveyRequestDatetime = survey.planSurveyRequestDatetime
          ? format(
              new Date(survey.planSurveyRequestDatetime),
              this.$t('common.format.date_ymd2') as string
            )
          : ''

        survey.actualSurveyRequestDatetime = survey.actualSurveyRequestDatetime
          ? format(
              new Date(survey.actualSurveyRequestDatetime),
              this.$t('common.format.date_ymd2') as string
            )
          : ''

        survey.planSurveyResponseDatetime = survey.planSurveyResponseDatetime
          ? format(
              new Date(survey.planSurveyResponseDatetime),
              this.$t('common.format.date_ymd2') as string
            )
          : ''
        survey.actualSurveyResponseDatetime =
          survey.actualSurveyResponseDatetime
            ? format(
                new Date(survey.actualSurveyResponseDatetime),
                this.$t('common.format.date_ymd2') as string
              )
            : ''
        survey.summaryMonth = survey.summaryMonth
          ? format(
              new Date(survey.summaryMonth),
              this.$t('common.format.date_ym2') as string
            )
          : ''
        survey.supportDateFrom = survey.supportDateFrom
          ? format(
              new Date(survey.supportDateFrom),
              this.$t('common.format.date_ymd2') as string
            )
          : ''
        survey.supportDateTo = survey.supportDateTo
          ? format(
              new Date(survey.supportDateTo),
              this.$t('common.format.date_ymd2') as string
            )
          : ''
        return survey
      })
    },
  },
  methods: {
    // 子コンポーネントから受け取ったソート済みの値配列を親コンポーネントに渡す
    emitSortItems(event: SurveyPlanListItem[]) {
      this.$emit('update:sort-items', event)
    },
    // 子コンポーネントから受け取ったソート対象変更イベントを親コンポーネントに渡す
    emitSortBy(event: string[]) {
      this.$emit('update:sort-by', event)
    },
    // 子コンポーネントから受け取ったソート並び順変更イベントを親コンポーネントに渡す
    emitSortDesc(event: boolean[]) {
      this.$emit('update:sort-desc', event)
    },
  },
})
</script>
<style lang="scss">
.o-survey-forecast-table {
  td {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    &:nth-child(13),
    &:nth-child(15) {
      font-weight: bold;
    }
    &:nth-child(2) {
      max-width: 232px;
    }
    &:nth-child(3),
    &:nth-child(4) {
      max-width: 200px;
    }
  }
  th {
    white-space: nowrap;
  }
  th.th-ymd {
    width: 96px;
  }
  th.th-ym {
    width: 80px;
  }
  .v-data-table > .v-data-table__wrapper > table > thead > tr > th.sortable {
    padding: 0 24px 0 16px;
  }
}
</style>
