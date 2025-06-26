<template>
  <RootTemPlate>
    <InPageHeader
      is-output-csv
      :csv-button-disabled="parseInt(total) <= 0 || showSelect"
      @outputCsv="checkExportCsvRole"
    >
      {{ $t('survey.pages.forecast.name') }}
    </InPageHeader>
    <SurveyForecastSort
      :param="searchParam"
      :is-loading="isLoading"
      :disabled="showSelect"
      @update="update"
      @sort="search"
      @clear="clear"
    />
    <SurveyForecastStatus
      class="mt-4"
      :is-loading="isLoading"
      :terms="getTerms()"
      :send="getSend()"
      :receive="getReceive()"
    />
    <SurveyForecastTable
      ref="table"
      class="mt-10"
      :is-loading="isLoading"
      :offset-page="response.offsetPage"
      :max-page="0"
      :total="total"
      :sort-by="sortBy"
      :sort-desc="sortDesc"
      :surveys="response.surveys"
      :show-select="showSelect"
      :selected-surveys="selectedSurveys"
      :is-single-month="isSingleMonth"
      @update:sort-items="emitSortItems"
      @update:sort-by="emitSortBy"
      @update:sort-by-survey-ops="emitSortBy"
      @update:sort-desc="emitSortDesc"
      @update:sort-desc-survey-ops="emitSortDesc"
      @click:toggle-show-select="toggleShowSelect"
      @change:selected-rows="changeSelectingRows"
      @click:open-modal-bulk-edit="isSurveyBulkEditModalOpen = true"
      @click:open-modal-bulk-delete="isSurveyBulkDeleteModalOpen = true"
    >
    </SurveyForecastTable>
    <!-- 一括編集モーダル -->
    <template v-if="isSurveyBulkEditModalOpen">
      <SurveyBulkEditModal
        :selected-surveys="selectedSurveys"
        :title="$t('project.pages.edit.survey.title.bulkEdit')"
        @click:save="bulkUpdate"
        @click:closeBulkEdit="isSurveyBulkEditModalOpen = false"
      />
    </template>
    <!-- 一括削除モーダル -->
    <template v-if="isSurveyBulkDeleteModalOpen">
      <SurveyBulkDeleteModal
        :selected-surveys="selectedSurveys"
        :title="$t('survey.pages.forecast.scheduleModal.bulkDeleteModal.title')"
        @click:closeDelete="isSurveyBulkDeleteModalOpen = false"
        @click:delete="bulkDelete"
      />
    </template>
  </RootTemPlate>
</template>

<script lang="ts">
import { format } from 'date-fns'
import cloneDeep from 'lodash/cloneDeep'
import {
  getCurrentDate,
  dataToCsv,
  downloadFile,
} from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { Sheet, Title, Table } from '~/components/common/atoms'
import InPageHeader from '~/components/common/organisms/InPageHeader.vue'
import { errorBarStore } from '~/store'

import SurveyForecastSort, {
  SearchParam,
} from '~/components/survey/organisms/SurveyForecastSort.vue'
import SurveyForecastStatus from '~/components/survey/molecules/SurveyForecastStatus.vue'
import SurveyForecastTable from '~/components/survey/organisms/SurveyForecastTable.vue'
import { GetSurveyPlansResponse, SurveyPlanListItem } from '~/models/Survey'
import { IGetSurveyPlansRequest } from '~/types/Survey'
import SurveyBulkEditModal from '~/components/survey/molecules/SurveyBulkEditModal.vue'
import SurveyBulkDeleteModal from '~/components/survey/molecules/SurveyBulkDeleteModal.vue'

export class SurveyPlanListItemWithProjectUrl extends SurveyPlanListItem {
  public projectUrl = ''
}

export class BulkUpdateParams {
  sendDays: number = 0
  sendDate: string = ''
  sendDayTiming: string = ''
  limitDays: number = 0
  limitDayTiming: string = ''
}

export default BaseComponent.extend({
  components: {
    RootTemPlate,
    Sheet,
    Title,
    Table,
    InPageHeader,
    SurveyForecastSort,
    SurveyForecastStatus,
    SurveyForecastTable,
    SurveyBulkEditModal,
    SurveyBulkDeleteModal,
  },
  props: {
    /** アンケート一覧情報 */
    response: {
      type: Object as PropType<GetSurveyPlansResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 表示用始点年月 */
    termsFrom: {
      type: String,
      required: true,
      default: '',
    },
    /** 表示用終点年月 */
    termsTo: {
      type: String,
      required: true,
      default: '',
    },
    /** 最終ページ */
    maxPage: {
      type: Number,
    },
    /** 全件数 */
    total: {
      type: Number,
      required: true,
    },
    sortBy: {
      type: Array as PropType<String[]> | String as PropType<String>,
    },
    sortDesc: {
      type: Array as PropType<Boolean[]> | Boolean as PropType<Boolean>,
    },
  },
  computed: {
    isSingleMonth() {
      if (this.termsFrom.length === 0) {
        return true
      }

      if (this.termsFrom.length === 7) {
        if (this.termsFrom === this.termsTo) {
          return true
        }
      }

      if (this.termsFrom.slice(0, 7) === this.termsTo.slice(0, 7)) {
        return true
      }

      return false
    },
  },
  data() {
    return {
      send: {
        plan: 25,
        achievement: 10,
      },
      receive: {
        plan: 25,
        achievement: 5,
      },
      searchParam: new SearchParam(),
      sortItems: [] as SurveyPlanListItem[],
      showSelect: false,
      isSurveyBulkEditModalOpen: false,
      isSurveyBulkDeleteModalOpen: false,
      selectedSurveys: [] as SurveyPlanListItem[],
    }
  },
  methods: {
    /**
     * 表示用の年月期間文字列を返す
     * @returns フォーマット済み年月期間文字列
     */
    getTerms() {
      let result = 'ー'
      if (this.termsFrom !== '' && this.termsTo !== '') {
        result = `${this.termsFrom} 〜 ${this.termsTo}`
      }
      return result
    },
    /**
     * 送信実績文字列を返す
     * @returns 送信実績文字列
     */
    getSend() {
      const result = {
        plan: 0,
        achievement: 0,
      }
      if (this.response && this.response.surveys) {
        for (const i in this.response.surveys) {
          if (this.response.surveys[i].planSurveyRequestDatetime) {
            result.plan++
          }
          if (this.response.surveys[i].actualSurveyRequestDatetime) {
            result.achievement++
          }
        }
      }
      return result
    },
    /**
     * 受信実績文字列を返す
     * @returns 受信実績文字列
     */
    getReceive() {
      const result = {
        plan: 0,
        achievement: 0,
      }
      if (this.response && this.response.surveys) {
        for (const i in this.response.surveys) {
          if (this.response.surveys[i].planSurveyResponseDatetime) {
            result.plan++
          }
          if (this.response.surveys[i].actualSurveyResponseDatetime) {
            result.achievement++
          }
        }
      }
      return result
    },
    /**
     * 他コンポーネントから検索条件の変更を受け取る
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      // @ts-ignore
      this.searchParam[item] = event
    },
    /** 検索条件をクリアし表示をリセット */
    clear() {
      this.searchParam = new SearchParam()
      this.search()
    },
    /** 検索を実行 */
    search() {
      if (parseInt(this.searchParam.searchType) === 1) {
        const strFromYearMonth = this.searchParam.summaryMonthFrom
        const strToYearMonth = this.searchParam.summaryMonthTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMM')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(format(new Date(strToYearMonth), 'yyyyMM'))
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.summaryMonthFrom = strToYearMonth
          this.searchParam.summaryMonthTo = strFromYearMonth
        }
        const request: IGetSurveyPlansRequest = {
          summaryMonthFrom: parseInt(
            format(new Date(this.searchParam.summaryMonthFrom), 'yyyyMM')
          ),
          summaryMonthTo: parseInt(
            format(new Date(this.searchParam.summaryMonthTo), 'yyyyMM')
          ),
        }
        this.$emit('getSurveysBySummaryMonth', request)
      } else if (parseInt(this.searchParam.searchType) === 2) {
        const strFromYearMonth = this.searchParam.planSurveyResponseDateFrom
        const strToYearMonth = this.searchParam.planSurveyResponseDateTo
        let intFromYearMonth = null
        let intToYearMonth = null
        if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
          intFromYearMonth = parseInt(
            format(new Date(strFromYearMonth), 'yyyyMMdd')
          )
        }
        if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
          intToYearMonth = parseInt(
            format(new Date(strToYearMonth), 'yyyyMMdd')
          )
        }
        if (
          intFromYearMonth != null &&
          intToYearMonth != null &&
          intFromYearMonth > intToYearMonth
        ) {
          this.searchParam.planSurveyResponseDateFrom = strToYearMonth
          this.searchParam.planSurveyResponseDateTo = strFromYearMonth
        }
        const request: IGetSurveyPlansRequest = {
          planSurveyResponseDateFrom: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateFrom),
              'yyyyMMdd'
            )
          ),
          planSurveyResponseDateTo: parseInt(
            format(
              new Date(this.searchParam.planSurveyResponseDateTo),
              'yyyyMMdd'
            )
          ),
        }
        this.$emit('getSurveysByPlanSurveyResponseDate', request)
      }
    },
    /** 表示されている情報をCSVに変換しダウンロード実行 */
    exportCsv() {
      const headers = {
        id: this.$t('survey.pages.forecast.header.id'),
        salesUserName: this.$t('survey.pages.forecast.header.salesUserName'),
        serviceTypeName: this.$t(
          'survey.pages.forecast.header.serviceTypeName'
        ),
        customerName: this.$t('survey.pages.forecast.header.customerName'),
        projectName: this.$t('survey.pages.forecast.header.projectName'),
        projectUrl: this.$t('survey.pages.forecast.header.projectId'),
        surveyType: this.$t('survey.pages.forecast.header.surveyType'),
        mainSupporterUser: this.$t(
          'survey.pages.forecast.header.mainSupporterUser'
        ),
        supporterUsers: this.$t('survey.pages.forecast.header.supporterUsers'),
        surveyUserType: this.$t('survey.pages.forecast.header.surveyUserType'),
        surveyUserName: this.$t('survey.pages.forecast.header.surveyUserName'),
        surveyUserEmail: this.$t(
          'survey.pages.forecast.header.surveyUserEmail'
        ),
        customerSuccess: this.$t(
          'survey.pages.forecast.header.customerSuccess'
        ),
        planSurveyRequestDatetime: this.$t(
          'survey.pages.forecast.header.planSurveyRequestDatetime'
        ),
        actualSurveyRequestDatetime: this.$t(
          'survey.pages.forecast.header.actualSurveyRequestDatetime'
        ),
        planSurveyResponseDatetime: this.$t(
          'survey.pages.forecast.header.planSurveyResponseDatetime'
        ),
        actualSurveyResponseDatetime: this.$t(
          'survey.pages.forecast.header.actualSurveyResponseDatetime'
        ),
        summaryMonth: this.$t('survey.pages.forecast.header.summaryMonth'),
        supporterOrganizationName: this.$t(
          'survey.pages.forecast.header.supporterOrganizationName'
        ),
        supportDateFrom: this.$t(
          'survey.pages.forecast.header.supportDateFrom'
        ),
        supportDateTo: this.$t('survey.pages.forecast.header.supportDateTo'),
        phase: this.$t('survey.pages.forecast.header.phase'),
        isCountManHour: this.$t('survey.pages.forecast.header.isCountManHour'),
      }
      const items: SurveyPlanListItemWithProjectUrl[] = cloneDeep(
        this.sortItems
      ) as SurveyPlanListItemWithProjectUrl[]

      // 案件URLを付加
      for (const i in items) {
        items[
          i
        ].projectUrl = `${process.env.APP_URL}/project/${items[i].projectId}`
      }
      const content = dataToCsv(items, headers)
      const fileName =
        this.$t('survey.pages.forecast.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
    exportCsvSurveyOps() {
      const headers = {
        id: this.$t('survey.pages.forecast.header.id'),
        salesUserName: this.$t('survey.pages.forecast.header.salesUserName'),
        serviceTypeName: this.$t(
          'survey.pages.forecast.header.serviceTypeName'
        ),
        customerName: this.$t('survey.pages.forecast.header.customerName'),
        projectName: this.$t('survey.pages.forecast.header.projectName'),
        projectUrl: this.$t('survey.pages.forecast.header.projectId'),
        surveyType: this.$t('survey.pages.forecast.header.surveyType'),
        unansweredSurveysNumber: this.$t(
          'survey.pages.forecast.header.unansweredSurveysNumber'
        ),
        planSurveyRequestDatetime: this.$t(
          'survey.pages.forecast.header.planSurveyRequestDatetime'
        ),
        planSurveyResponseDatetime: this.$t(
          'survey.pages.forecast.header.planSurveyResponseDatetime'
        ),
        mainSupporterUser: this.$t(
          'survey.pages.forecast.header.mainSupporterUser'
        ),
        supporterUsers: this.$t('survey.pages.forecast.header.supporterUsers'),
        surveyUserType: this.$t('survey.pages.forecast.header.surveyUserType'),
        surveyUserName: this.$t('survey.pages.forecast.header.surveyUserName'),
        surveyUserEmail: this.$t(
          'survey.pages.forecast.header.surveyUserEmail'
        ),
        customerSuccess: this.$t(
          'survey.pages.forecast.header.customerSuccess'
        ),
        actualSurveyRequestDatetime: this.$t(
          'survey.pages.forecast.header.actualSurveyRequestDatetime'
        ),
        actualSurveyResponseDatetime: this.$t(
          'survey.pages.forecast.header.actualSurveyResponseDatetime'
        ),
        summaryMonth: this.$t('survey.pages.forecast.header.summaryMonth'),
        supporterOrganizationName: this.$t(
          'survey.pages.forecast.header.supporterOrganizationName'
        ),
        supportDateFrom: this.$t(
          'survey.pages.forecast.header.supportDateFrom'
        ),
        supportDateTo: this.$t('survey.pages.forecast.header.supportDateTo'),
        phase: this.$t('survey.pages.forecast.header.phase'),
        isCountManHour: this.$t('survey.pages.forecast.header.isCountManHour'),
      }
      const items: SurveyPlanListItemWithProjectUrl[] = cloneDeep(
        this.sortItems
      ) as SurveyPlanListItemWithProjectUrl[]

      // 案件URLを付加
      for (const i in items) {
        items[
          i
        ].projectUrl = `${process.env.APP_URL}/project/${items[i].projectId}`
      }
      const content = dataToCsv(items, headers)
      const fileName =
        this.$t('survey.pages.forecast.formatName') +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      downloadFile(fileName, content, 'text/csv', true)
    },
    checkExportCsvRole() {
      // アンケート事務局ロールの場合
      if (window.location.href.includes('survey-ops')) {
        this.exportCsvSurveyOps()
      } else {
        this.exportCsv()
      }
    },
    // 子コンポーネントから受け取ったソート済みの値配列を保持する
    emitSortItems(event: SurveyPlanListItem[]) {
      this.sortItems = event
    },
    // 子コンポーネントから受け取ったソート対象変更イベントを親コンポーネントに渡す
    emitSortBy(event: string[]) {
      if (this.$route.path.includes('survey-ops')) {
        this.$emit('update:sort-by-survey-ops', event)
      } else {
        this.$emit('update:sort-by', event)
      }
    },
    // 子コンポーネントから受け取ったソート並び順変更イベントを親コンポーネントに渡す
    emitSortDesc(event: boolean[]) {
      if (this.$route.path.includes('survey-ops')) {
        this.$emit('update:sort-desc-survey-ops', event)
      } else {
        this.$emit('update:sort-desc', event)
      }
    },
    toggleShowSelect() {
      this.showSelect = !this.showSelect
      this.selectedSurveys = []
    },
    changeSelectingRows(values: SurveyPlanListItem[]) {
      this.selectedSurveys = values
    },
    /** アンケート編集モーダルを開く */
    onClickSurveyEdit() {
      this.isSurveyBulkEditModalOpen = true
    },
    /** アンケート削除モーダルを開く */
    onClickSurveyDelete() {
      this.isSurveyBulkDeleteModalOpen = true
    },
    /** 選択済アンケートの一括更新 */
    bulkUpdate() {
      // エラーの場合モーダルを閉じる
      if (errorBarStore.message) {
        this.isSurveyBulkEditModalOpen = false
      } else {
        this.clearErrorBar()
        this.isSurveyBulkEditModalOpen = false
        this.toggleShowSelect()
        this.search()
      }
    },
    /** 選択済アンケートの一括削除 */
    bulkDelete() {
      this.clearErrorBar()
      this.isSurveyBulkDeleteModalOpen = false
      this.toggleShowSelect()
      this.search()
    },
  },
})
</script>
