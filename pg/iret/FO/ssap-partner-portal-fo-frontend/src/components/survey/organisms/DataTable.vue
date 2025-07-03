<template>
  <v-data-table
    class="o-survey-data-table"
    :class="className"
    :headers="headers"
    :items="items"
    :no-data-text="$t('common.label.noData')"
    :loading-text="$t('common.label.loading')"
    v-bind="attributes"
    v-on="$listeners"
  >
    <template
      v-if="!isHidePagination"
      #top="{ pagination, options, updateOptions }"
    >
      <v-data-footer
        :pagination="pagination"
        :options="options"
        :page-text="
          getPageText(
            pagination.pageStart + 1,
            pagination.pageStop,
            pagination.itemsLength
          )
        "
        items-per-page-text=""
        disable-items-per-page
        @update:options="updateOptions"
      >
      </v-data-footer>
    </template>
    <template v-else-if="$scopedSlots.top" #top>
      <slot name="top" />
    </template>

    <template #default>
      <slot name="default" />
    </template>

    <template #foot>
      <slot name="foot" />
    </template>
    <template #[`footer.page-text`]></template>

    <template #[`item.surveyName`]="{ item }">
      <nuxt-link class="o-data-table__link" :to="'/survey/' + item.id">
        {{ item.surveyName }}
      </nuxt-link>
    </template>

    <template #[`item.status`]="{ item }">
      <template v-if="item.status">
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

    <!-- 案件名 -->
    <template #[`item.projectName`]="{ item }">
      <nuxt-link class="o-data-table__link" :to="`/project/${item.projectId}`">
        <OverflowTooltip :text="item.projectName" :max="28" />
      </nuxt-link>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import { format } from 'date-fns'
import WrapperComponent, {
  AttributeSet,
} from '~/components/common/bases/WrapperComponent'
import { Chip } from '~/components/common/atoms/index'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'

/*
  v-data-tableの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  components: {
    Chip,
    OverflowTooltip,
  },
  props: {
    linkPrefix: {
      type: String,
      default: '',
    },
    /** ページネーションを隠すか */
    isHidePagination: {
      type: Boolean,
      default: false,
    },
    isScroll: {
      type: Boolean,
      default: false,
    },
    /** 案件アンケート一覧 */
    surveys: {
      type: Array,
      required: true,
      default: [],
    },
    type: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
      surveyHeaders: [
        [
          {
            text: this.$t('survey.row.status'),
            align: 'start',
            value: 'status',
            sortable: false,
          },
          {
            text: this.$t('survey.row.surveyName'),
            align: 'start',
            value: 'surveyName',
            sortable: false,
          },
          {
            text: this.$t('survey.row.actualSurveyRequestDatetime'),
            align: 'start',
            value: 'actualSurveyRequestDatetime',
            sortable: false,
          },
          {
            text: this.$t('survey.row.surveyLimitDate'),
            align: 'start',
            value: 'surveyLimitDate',
            sortable: false,
          },
          {
            text: this.$t('survey.row.actualSurveyResponseDatetime'),
            align: 'start',
            value: 'actualSurveyResponseDatetime',
            sortable: false,
          },
          {
            text: this.$t('survey.row.customerName'),
            align: 'start',
            value: 'customerName',
            sortable: false,
          },
          {
            text: this.$t('survey.row.company'),
            align: 'start',
            value: 'company',
            sortable: false,
          },
        ],
        [
          {
            text: this.$t('survey.row.projectName'),
            align: 'start',
            value: 'projectName',
            sortable: false,
          },
          {
            text: this.$t('survey.row.customerName'),
            align: 'start',
            value: 'customerName',
            sortable: false,
          },
          {
            text: this.$t('survey.row.serviceTypeName'),
            align: 'start',
            value: 'serviceTypeName',
            sortable: false,
          },
          {
            text: this.$t('survey.row.supportDateFromTo'),
            align: 'start',
            value: 'supportDateFromTo',
            sortable: false,
          },
        ],
        [
          {
            text: this.$t('survey.row.status'),
            align: 'start',
            value: 'status',
            sortable: false,
          },
          {
            text: this.$t('survey.row.surveyName'),
            align: 'start',
            value: 'surveyName',
            sortable: false,
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
      ],
    }
  },
  computed: {
    /**
     * テーブルの見出しを生成
     */
    headers(): any {
      if (this.surveyHeaders[this.type]) {
        return this.surveyHeaders[this.type]
      } else {
        return []
      }
    },
    /**
     * テーブルのボディを生成
     */
    items(): any {
      if (this.surveys) {
        return this.surveys.map((survey: any) => {
          if (
            survey.actualSurveyResponseDatetime &&
            survey.actualSurveyResponseDatetime !== '-' &&
            survey.actualSurveyResponseDatetime.length
          ) {
            survey.status = 1
            survey.actualSurveyResponseDatetime = format(
              new Date(survey.actualSurveyResponseDatetime),
              'yyyy/M/d'
            )
          } else {
            survey.status = 0
            survey.actualSurveyResponseDatetime = '-'
          }
          survey.actualSurveyRequestDatetime = format(
            new Date(survey.actualSurveyRequestDatetime),
            'yyyy/M/d'
          )
          survey.surveyLimitDate = format(
            new Date(survey.surveyLimitDate),
            'yyyy/M/d'
          )

          let surveyName = ''
          switch (survey.surveyType) {
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

          survey.surveyName =
            format(new Date(survey.actualSurveyRequestDatetime), 'yyyy年M月') +
            ' ' +
            surveyName

          // 支援期間
          survey.supportDateFromTo =
            survey.supportDateFrom + ' ~ ' + survey.supportDateTo

          // 当期満足度平均
          if (Object.keys(survey.points).length) {
            const sum = Object.keys(survey.points).reduce((sum, key) => {
              if (key !== 'continuation') {
                return sum + survey.points[key]
              } else {
                return sum
              }
            }, 0)
            if (
              survey.points.continuation &&
              Object.keys(survey.points).length >= 2
            ) {
              survey.satisfaction =
                sum / (Object.keys(survey.points).length - 1)
            } else if (!survey.points.continuation) {
              survey.satisfaction = sum / Object.keys(survey.points).length
            } else {
              survey.satisfaction = 0
            }
          } else {
            survey.satisfaction = 0
          }
          return survey
        })
      } else {
        return []
      }
    },
    /**
     * タイプによってクラス名を変更
     */
    className(): string {
      return this.type
        ? `o-servey-list-table--${this.type}`
        : 'o-servey-list-table'
    },
  },
  methods: {
    /**
     * ページを表すテキストを生成
     * @param start 最初のページ
     * @param end 最後のページ
     * @param total ページ合計数
     */
    getPageText(start = 0, end = 0, total = 0) {
      return `${start} 〜 ${end}${this.$t(
        'survey.terminology.showItems'
      )} / ${this.$t('survey.terminology.all')} ${total} ${this.$t(
        'survey.terminology.items'
      )}`
    },
  },
})
</script>

<style lang="scss">
.o-survey-data-table {
  &.v-data-table {
    background-color: $c-black-page-bg;
    tr {
      background-color: $c-white;
      td {
        border-bottom: 0 !important;
      }
      &:nth-child(even) {
        background-color: $c-black-table-border;
      }
    }
  }
  .v-data-table-header {
    th,
    td {
      background-color: $c-black-80;
      color: $c-white !important;
    }
    .v-icon {
      color: $c-gray-line-dark !important;
      margin-left: 5px !important;
      opacity: 1;
    }
    th {
      &.active {
        .v-icon {
          color: $c-primary !important;
        }
      }
    }
  }
  .v-data-table__wrapper {
    border-radius: 4px;
    box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
      0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
    table {
      tbody {
        tr {
          transition-property: background-color;
          transition-duration: 0.2s;
          &:hover,
          &:focus {
            background: $c-primary-8 !important;
          }
        }
      }
    }
  }
  .v-data-footer {
    &:first-child {
      margin-bottom: 16px;
    }
    &:last-child {
      margin-top: 16px;
    }
  }
  .v-data-footer {
    width: 100%;
    padding: 0 !important;
    justify-content: flex-end;
    border: 0 !important;
    position: relative;
  }
  .v-data-footer__select {
    display: none !important;
  }
  .v-data-table__wrapper {
    .v-data-footer {
      width: 100%;
    }
  }
  .v-data-footer__pagination {
    font-size: 0.875rem;
    position: absolute;
    left: 0;
    margin: 0;
  }
  .v-data-footer__icons-before,
  .v-data-footer__icons-after {
    .v-btn {
      width: 42px;
      height: 42px;
      box-shadow: 0 3px 6px rgba(#000000, 0.16);
      border-radius: 4px;
      transition-property: background-color;
      transition-duration: 0.2s;
      .v-ripple__container,
      &::before {
        display: none !important;
      }
      &:hover,
      &:focus {
        background-color: $c-primary-8;
      }
    }
    .v-icon {
      width: 30px;
      height: 30px;
      font-size: 30px;
      color: $c-primary-dark !important;
    }
    .v-btn--disabled {
      .v-icon {
        color: #8f8f8f !important;
      }
    }
  }
}
.o-data-table__link {
  color: $c-primary-dark !important;
  font-weight: bold;
  &:hover,
  &:focus {
    color: $c-primary-over !important;
  }
}

.o-servey-list-table.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 14px 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      &:nth-child(1),
      &:nth-child(3),
      &:nth-child(4),
      &:nth-child(5) {
        width: 96px;
        max-width: 96px;
      }
      &:nth-child(2) {
        width: 320px;
        max-width: 320px;
      }
      &:nth-child(6) {
        width: 176px;
        max-width: 176px;
      }
      &:nth-child(7) {
        max-width: 320px;
      }
    }
    & > tbody > tr > td {
      font-size: 0.75rem;
    }
  }
  .o-servey-list-table__total {
    font-size: 0.875rem;
  }
}
.o-servey-list-table--1.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 14px 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      &:nth-child(1) {
        width: 400px;
        max-width: 400px;
      }
      &:nth-child(3) {
        width: 176px;
      }
      &:nth-child(4) {
        width: 180px;
      }
      &:nth-child(5) {
        width: 104px;
      }
    }
    & > tbody > tr > td {
      font-size: 0.75rem;
    }
  }
}
.o-servey-list-table__total {
  font-size: 0.875rem;
}
.o-servey-list-table--2.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 14px 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      &:nth-child(1),
      &:nth-child(3),
      &:nth-child(4) {
        width: 96px;
        max-width: 96px;
      }
      &:nth-child(5) {
        width: 176px;
        max-width: 176px;
      }
      &:nth-child(6) {
        max-width: 320px;
        width: 320px;
      }
    }
    & > tbody > tr > td {
      font-size: 0.75rem;
    }
  }
  .o-servey-list-table__total {
    font-size: 0.875rem;
  }
}
</style>
