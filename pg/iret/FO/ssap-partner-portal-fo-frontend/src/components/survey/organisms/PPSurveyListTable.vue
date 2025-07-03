<template>
  <CommonDataTable
    :headers="headers"
    :total="total"
    :items="formattedItems"
    :is-loading="isLoading"
    :loads-every-time="false"
    :short-text="true"
    :shows-pagination="false"
    v-on="$listeners"
  >
    <template #[`item.isFinished`]="{ item }">
      <template v-if="item.isFinished">
        <Chip label small class="width-70" color="secondary">
          {{ $t('survey.group_info.status.complete') }}
        </Chip>
      </template>
      <template v-else>
        <Chip label small outlined class="width-70" color="secondary">
          {{ $t('survey.group_info.status.incomplete') }}
        </Chip>
      </template>
    </template>
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import { Survey } from '~/types/Survey'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { Chip } from '~/components/common/atoms/index'
import { formatDateStr } from '~/utils/common-functions'

class SurveyListTableItem extends Survey {
  surveyName = ''
}

export default BaseComponent.extend({
  components: {
    CommonDataTable,
    Chip,
  },
  props: {
    /** 案件アンケート一覧 */
    surveys: {
      type: Array as PropType<Survey[]>,
      required: true,
    },
    /** 最大ページ数 */
    total: {
      type: Number,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { headers: IDataTableHeader[] } {
    return {
      headers: [
        {
          text: this.$t('survey.row.status'),
          align: 'start',
          value: 'isFinished',
          sortable: false,
        },
        {
          text: this.$t('survey.row.surveyName'),
          align: 'start',
          value: 'surveyName',
          sortable: false,
          link: {
            prefix: '/survey/pp/',
            value: 'id',
          },
        },
        {
          text: this.$t('survey.row.actualSurveyRequestDatetime'),
          align: 'start',
          value: 'actualSurveyRequestDatetime',
          sortable: false,
        },
        {
          text: this.$t('survey.row.actualSurveyResponseDatetime'),
          align: 'start',
          value: 'actualSurveyResponseDatetime',
          sortable: false,
        },
        {
          text: this.$t('survey.row.answerUserName'),
          align: 'start',
          value: 'answerUserName',
          sortable: false,
        },
        {
          text: this.$t('survey.row.company'),
          align: 'start',
          value: 'company',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    /**
     * 表の表示形式を揃えるための変換は、ListTable organismで行う
     * (上流で元データを加工しないように)
     */
    formattedItems(): SurveyListTableItem[] {
      return this.surveys.map((survey) => {
        const rtn = Object.assign(new SurveyListTableItem(), survey)

        const surveyNameYearMonth = formatDateStr(
          survey.summaryMonth,
          this.$t('common.format.date_ym') as string
        )
        const surveyNamePP = this.$t('survey.group_info.surveyNameList.pp')

        rtn.surveyName = `${surveyNameYearMonth} ${surveyNamePP}`

        if (survey.actualSurveyRequestDatetime) {
          rtn.actualSurveyRequestDatetime = formatDateStr(
            survey.actualSurveyRequestDatetime,
            this.$t('common.format.date_ymd5') as string
          )
        } else {
          rtn.actualSurveyRequestDatetime = '-'
        }

        if (survey.actualSurveyResponseDatetime) {
          rtn.actualSurveyResponseDatetime = formatDateStr(
            survey.actualSurveyResponseDatetime,
            this.$t('common.format.date_ymd5') as string
          )
        } else {
          rtn.actualSurveyResponseDatetime = '-'
        }

        return rtn
      })
    },
  },
})
</script>
