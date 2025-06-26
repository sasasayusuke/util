<template>
  <div>
    <CommonDataTable
      :class-name="className"
      :headers="headers"
      :items="items"
      :shows-pagination="showsPagination"
      :is-loading="isLoading"
      v-on="$listeners"
    >
      <template v-if="type === 0 || type === 2" #top>
        <div class="pb-2 o-survey-list-table__total">
          {{ total }}
        </div>
      </template>
      <template #[`item.status`]="{ item }">
        <template v-if="item.status">
          <Chip label small class="width-70 mr-2" color="secondary">
            {{ $t('survey.group_info.status.complete') }}
          </Chip>
        </template>
        <template v-else>
          <Chip label small outlined class="width-70 mr-2" color="secondary">
            {{ $t('survey.group_info.status.incomplete') }}
          </Chip>
        </template>
      </template>
      <template #[`item.surveyName`]="{ item }">
        <!-- リンク有効 -->
        <template v-if="isAccessibleSurvey(item.isFinished)">
          <nuxt-link :to="`/survey/${item.id}`">{{
            item.surveyName
          }}</nuxt-link>
        </template>
        <!-- リンク無効 -->
        <template v-else>{{ item.surveyName }}</template>
      </template>
    </CommonDataTable>
  </div>
</template>

<script lang="ts">
import { format } from 'date-fns'
import type { IDataTableHeader } from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import { Chip } from '~/components/common/atoms/index'
import { meStore } from '~/store'
import { GetProjectByIdResponse } from '~/models/Project'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    CommonDataTable,
    Chip,
  },
  props: {
    /** 案件アンケート一覧 */
    surveys: {
      type: Array,
      required: true,
      default: [],
    },
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    type: {
      type: Number,
      default: 0,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** ロール */
    role: {
      type: String,
      required: false,
    },
  },
  data(): {
    surveyHeaders: IDataTableHeader[][]
  } {
    const surveyHeaders: IDataTableHeader[][] = [
      [
        // type:0 アンケート一覧
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
          maxLength: 26,
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
          value: 'answerUserName',
          sortable: false,
          maxLength: 12,
        },
        {
          text: this.$t('survey.row.company'),
          align: 'start',
          value: 'surveyCompany',
          sortable: false,
          maxLength: 22,
        },
      ],
      [
        // type:1 adminアンケート一覧
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
        // type:2 PPアンケート一覧
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
    ]
    return {
      surveyHeaders,
    }
  },
  computed: {
    /**
     * テーブルの見出しの条件分岐
     */
    headers(): any {
      if (this.surveyHeaders[this.type]) {
        return this.surveyHeaders[this.type]
      } else {
        return []
      }
    },
    /**
     * テーブルのアイテムを作成
     */
    items(): any {
      if (this.surveys) {
        return this.surveys.map((survey: any) => {
          if (
            survey.actualSurveyResponseDatetime &&
            survey.actualSurveyResponseDatetime !== 'ー' &&
            survey.actualSurveyResponseDatetime.length
          ) {
            // 状況を「済み」にする
            survey.status = 1
            survey.actualSurveyResponseDatetime = format(
              new Date(survey.actualSurveyResponseDatetime),
              this.$t('common.format.date_ymd2') as string
            )
          } else {
            // 状況を「未回答」にする
            survey.status = 0
            survey.actualSurveyResponseDatetime = 'ー'
          }
          survey.actualSurveyRequestDatetime = format(
            new Date(survey.actualSurveyRequestDatetime),
            this.$t('common.format.date_ymd2') as string
          )
          // 回答期限なしの場合
          if (survey.planSurveyResponseDatetime === null) {
            survey.surveyLimitDate = 'ー'
          } else {
            survey.surveyLimitDate = format(
              new Date(survey.planSurveyResponseDatetime),
              this.$t('common.format.date_ymd2') as string
            )
          }

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
            format(
              new Date(survey.summaryMonth),
              this.$t('common.format.date_ym') as string
            ) +
            ' ' +
            surveyName

          // 支援期間
          survey.supportDateFromTo =
            survey.supportDateFrom + ' ~ ' + survey.supportDateTo

          // 所属会社(顧客の場合は取引先名,顧客以外の場合は所属会社)
          if (survey.customerName !== null) {
            survey.surveyCompany = survey.customerName
          } else if (survey.company !== null) {
            survey.surveyCompany = survey.company
          } else {
            survey.surveyCompany = null
          }

          return survey
        })
      } else {
        return []
      }
    },
    /**
     * typeによってクラス名を変更
     */
    className(): string {
      return this.type
        ? `o-survey-list-table--${this.type} o-survey-data-table pb-2`
        : 'o-survey-list-table o-survey-data-table pb-2'
    },
    /**
     * @return typeが1であればtrueを返す
     */
    showsPagination(): boolean {
      return this.type === 1
    },
    /**
     * 案件一覧の合計のテキストを作成
     */
    total(): string {
      return (
        this.$t('survey.terminology.all') +
        ' ' +
        this.surveys.length +
        ' ' +
        this.$t('survey.terminology.items')
      )
    },
    /**
     * 顧客のときリンクを有効にする
     * @param {number} type アンケートタイプ
     * @param {string} role ログインユーザロール
     * @returns {boolean} リンクの可否
     */
    addLinkForCustomer(): boolean {
      if (this.type === 0 && this.role === 'customer') {
        return true
      }
      return false
    },
    /**
     * 営業のときリンクを有効にする
     * @param {number} type アンケートタイプ
     * @param {string} role ログインユーザロール
     * @returns {boolean} リンクの可否
     */
    addLinkForSales(): boolean {
      if (this.type === 0 && this.role === 'sales') {
        return true
      }
      return false
    },
  },
  methods: {
    /**
     * ログインユーザーが案件に所属しているか
     * @returns 所属案件か否か
     */
    isRegionalProject(): boolean {
      if (meStore.projectIds && meStore.projectIds.length > 0) {
        return meStore.projectIds.includes(this.project.id)
      }
      return false
    },
    /**
     * ログインユーザーが案件の粗利メイン課に所属しているか
     * @returns 案件の粗利メイン課に所属しているか否か
     */
    isRegionalSupporterOrganization(): boolean {
      if (
        meStore.supporterOrganizations &&
        meStore.supporterOrganizations.length > 0
      ) {
        const regionalSupporterOrganization =
          meStore.supporterOrganizations.find(
            (organization) =>
              this.project.supporterOrganizationId === organization.id
          )
        if (regionalSupporterOrganization) {
          return true
        }
      }
      return false
    },
    /**
     * ログインユーザーがアンケート情報にアクセス出来るか
     */
    isAccessibleSurvey(isFinished: boolean): boolean {
      if (hasRole([ENUM_USER_ROLE.CUSTOMER])) {
        // お客様の場合、案件のお客様代表者であればアクセス可能
        return meStore.id === this.project.mainCustomerUserId
      } else if (hasRole([ENUM_USER_ROLE.SALES])) {
        // 営業の場合
        if (isFinished) {
          //回答済みアンケートは、担当案件/公開案件のみアクセス可
          return this.isRegionalProject() || !this.project.isSecret
        } else {
          // 未回答アンケートは、担当案件のみアクセス可
          return this.isRegionalProject()
        }
      } else if (
        hasRole([ENUM_USER_ROLE.SALES_MGR, ENUM_USER_ROLE.BUSINESS_MGR])
      ) {
        // 営業責任者と事業者責任者の場合
        if (isFinished) {
          //回答済みアンケートは、全てアクセス可
          return true
        } else {
          // 未回答アンケートは、担当案件のみアクセス可
          return this.isRegionalProject()
        }
      } else {
        // それ以外のロールは回答済みアンケートのみアクセス可能
        return isFinished
      }
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
          &:hover {
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
      &:hover {
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
}

.o-survey-list-table.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
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
      &:nth-child(1) {
        width: 112px;
        max-width: 112px;
        padding: 8px 8px 8px 20px;
      }
    }
    & > tbody > tr > td {
      font-size: 0.75rem;
    }
  }
  .o-survey-list-table__total {
    font-size: 0.875rem;
  }
}
.o-survey-list-table--1.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      &:nth-child(1) {
        width: 400px;
        max-width: 400px;
        padding: 8px 8px 8px 20px;
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
.o-survey-list-table__total {
  font-size: 0.875rem;
}
.o-survey-list-table--2.v-data-table {
  & > .v-data-table__wrapper > table {
    & > tbody > tr > td,
    & > thead > tr > th {
      padding: 8px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
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
      &:nth-child(1) {
        width: 112px;
        max-width: 112px;
        padding: 8px 8px 8px 20px;
      }
    }
    & > tbody > tr > td {
      font-size: 0.75rem;
    }
  }
  .o-survey-list-table__total {
    font-size: 0.875rem;
  }
}
</style>
