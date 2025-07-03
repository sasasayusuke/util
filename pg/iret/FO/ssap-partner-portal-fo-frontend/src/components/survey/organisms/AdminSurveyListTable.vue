<template>
  <CommonDataTable
    :headers="surveyHeaders"
    :items="formattedSurveys"
    :is-loading="isLoading"
    :loads-every-time="false"
    :page="offsetPage"
    :total="total"
    :limit="limit"
    link-prefix="/surveys"
    class="o-AdminSurveyListTable"
    @click:prev="$emit('click:prev')"
    @click:next="$emit('click:next')"
  >
    <!-- header編集 -->
    <template #[`header.isDisclosure`]="{ header }">
      <span class="o-AdminSurveyListTable__line-2">
        {{ header.text.split('\n')[0] }}<br />
        {{ header.text.split('\n')[1] }}
      </span>
    </template>
    <template #[`header.isNotSummary`]="{ header }">
      <span class="o-AdminSurveyListTable__line-2">
        {{ header.text.split('\n')[0] }}<br />
        {{ header.text.split('\n')[1] }}
      </span>
    </template>
    <!-- body編集 -->
    <template #[`item.customerName`]="{ item }">
      <OverflowTooltip :text="item.customerName" :max="18" />
    </template>
    <template #[`item.projectName`]="{ item }">
      <OverflowTooltip :text="item.projectName" :max="12" />
    </template>
    <template #[`item.surveyType`]="{ item }">
      <nuxt-link
        v-if="isAccessibleSurveyDetail(item)"
        class="o-common-data-table__link"
        :to="forwardToUrl(`/survey/${item.id}`)"
      >
        <OverflowTooltip :text="item.surveyType" :max="12" />
      </nuxt-link>
      <OverflowTooltip v-else :text="item.surveyType" :max="12" />
    </template>
    <template #[`item.answerUserName`]="{ item }">
      <OverflowTooltip :text="item.answerUserName" :max="8" />
    </template>
    <template #[`item.actualSurveyResponseDatetime`]="{ item }">
      <template
        v-if="
          item.actualSurveyResponseDatetime &&
          item.actualSurveyResponseDatetime !== 'NaN/NaN/NaN'
        "
      >
        {{ item.actualSurveyResponseDatetime }}
      </template>
      <template v-else>-</template>
    </template>
    <template #[`item.isNotSummary`]="{ item }">
      <template v-if="item.isNotSummary">
        {{ $t('common.button.no') }}
      </template>
      <template v-else>
        {{ $t('common.button.yes') }}
      </template>
    </template>
    <template #[`item.isDisclosure`]="{ item }">
      <template v-if="item.isDisclosure">
        {{ $t('common.button.yes') }}
      </template>
      <template v-else>
        {{ $t('common.button.no') }}
      </template>
    </template>
  </CommonDataTable>
</template>

<script lang="ts">
import { format } from 'date-fns'
import CommonDataTable, {
  IDataTableHeader,
} from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import { SurveyListItem } from '~/models/Survey'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  components: {
    CommonDataTable,
    OverflowTooltip,
  },
  props: {
    /** 案件アンケート一覧 */
    surveys: {
      type: Array,
      required: true,
    },
    /** 現在ページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 最大ページ数 */
    maxPage: {
      type: Number,
      required: true,
    },
    /** ページ合計 */
    total: {
      type: Number,
      required: true,
    },
    /** 件数合計 */
    limit: {
      type: Number,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { surveyHeaders: IDataTableHeader[] } {
    return {
      surveyHeaders: [
        {
          text: this.$t('survey.pages.admin.list.tableHeader.serviceTypeName'),
          align: 'start',
          value: 'serviceTypeName',
          sortable: true,
          cellClass: 'td-serviceTypeName',
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.customerName'),
          value: 'customerName',
          sortable: true,
          maxLength: 19,
          cellClass: 'td-customerName',
          sort: (a, b) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.projectName'),
          value: 'projectName',
          sortable: true,
          maxLength: 13,
          cellClass: 'td-projectName',
          sort: (a, b) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.surveyType'),
          value: 'surveyType',
          sortable: true,
          maxLength: 13,
          cellClass: 'td-surveyType',
          sort: (a, b) => {
            return a.localeCompare(b, 'ja', { numeric: true })
          },
        },
        {
          text: this.$t(
            'survey.pages.admin.list.tableHeader.actualSurveyResponseDatetime'
          ),
          value: 'actualSurveyResponseDatetime',
          sortable: true,
          width: '96px',
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.answerUserName'),
          value: 'answerUserName',
          sortable: true,
          maxLength: 9,
          cellClass: 'td-answerUserName',
          sort: (a, b) => {
            return a.localeCompare(b, 'ja')
          },
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.isDisclosure'),
          value: 'isDisclosure',
          sortable: true,
          width: '88px',
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.summaryMonth'),
          value: 'summaryMonth',
          sortable: true,
          width: '96px',
        },
        {
          text: this.$t('survey.pages.admin.list.tableHeader.isNotSummary'),
          value: 'isNotSummary',
          sortable: true,
          width: '84px',
        },
      ],
    }
  },
  computed: {
    /**
     * 表の表示形式を揃えるための変換は、ListTable organismで行う
     * (上流で元データを加工しないように)
     */
    formattedSurveys(): SurveyListItem[] {
      return this.surveys.map((survey: any) => {
        if (survey.actualSurveyResponseDatetime) {
          survey.actualSurveyResponseDatetime =
            survey.actualSurveyResponseDatetime.split(' ')[0]
        }
        survey.summaryMonth = survey.summaryMonth.replace('/', '.')

        // アンケート種別と集計月に合わせて、案件名を変更
        if (survey.surveyType === 'service') {
          survey.surveyType = (format(
            new Date(survey.summaryMonth),
            this.$t('common.format.date_ym') as string
          ) +
            ' ' +
            this.$t('survey.group_info.surveyNameList.service')) as string
        } else if (survey.surveyType === 'completion') {
          survey.surveyType = (format(
            new Date(survey.summaryMonth),
            this.$t('common.format.date_ym') as string
          ) +
            ' ' +
            this.$t('survey.group_info.surveyNameList.completion')) as string
        } else if (survey.surveyType === 'quick') {
          survey.surveyType = (format(
            new Date(survey.summaryMonth),
            this.$t('common.format.date_ym') as string
          ) +
            ' ' +
            this.$t('survey.group_info.surveyNameList.quick')) as string
        } else if (survey.surveyType === 'pp') {
          survey.surveyType = (format(
            new Date(survey.summaryMonth),
            this.$t('common.format.date_ym') as string
          ) +
            ' ' +
            this.$t('survey.group_info.surveyNameList.pp')) as string
        }
        return survey
      })
    },
  },
  methods: {
    /**
     * アクセス可能なアンケートかどうか確認する
     * @param survey アンケート
     * @returns boolean アクセス可能なアンケートかどうか
     */
    isAccessibleSurveyDetail(survey: SurveyListItem): boolean {
      if (hasRole([ENUM_USER_ROLE.SUPPORTER])) {
        // 支援者の場合、内部開示NGのアンケートはテキストリンクを非活性にする
        if (survey.isDisclosure === false) {
          return false
        }
        return true
      }
      return true
    },
  },
})
</script>

<style lang="scss">
.o-AdminSurveyListTable {
  td {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }
  .td-serviceTypeName {
    max-width: 200px;
    width: 200px;
  }
  .td-customerName {
    width: 180px;
    max-width: 180px;
  }
  .td-projectName {
    width: 150px;
    max-width: 150px;
  }
  .td-surveyType {
    width: 168px;
    max-width: 168px;
  }
  .td-answerUserName {
    width: 144px;
    max-width: 144px;
  }
}
.o-AdminSurveyListTable__line-2 {
  & + .v-icon {
    margin-top: -1em;
  }
}
</style>
