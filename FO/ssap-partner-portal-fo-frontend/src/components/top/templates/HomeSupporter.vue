<template>
  <RootTemplate :class="'fill-height align-start'">
    <div class="home-supporter">
      <!-- 参加中の案件 -->
      <HomeProjects v-if="!isSalesMgr" :projects="projects" />
      <!-- 直近のカルテ -->
      <HomeKarten v-if="!isSalesMgr" :karten="karten" />
      <!-- アンケート集計サマリー -->
      <HomeSurveySummaries
        :survey-summary-all="surveySummaryAll"
        :survey-summary-supporter-organizations="
          surveySummarySupporterOrganizations
        "
        :survey-summary-all-params="surveySummaryAllParams"
        :survey-summary-supporter-organizations-params="
          surveySummarySupporterOrganizationsParams
        "
        :batch-control-kpi="batchControlKpi"
        :batch-control-section="batchControlSection"
        :param-kpi="searchParamKpi"
        :param-section="searchParamSection"
        @sort-kpi="searchKpi"
        @sort-section="searchSection"
        @clear-kpi="clearKpi"
        @clear-section="clearSection"
        @update-kpi="updateParamKpi"
        @update-section="updateParamSection"
      />
      <!-- PartnerPortalに関するアンケート -->
      <HomeQuestionnaire :surveys="surveys" />
      <!-- 支援工数 -->
      <HomeManHour
        v-if="showsHomeManHour"
        :this-month="thisMonth"
        :this-month-year="thisMonthYear"
        :last-month="lastMonth"
        :last-month-year="lastMonthYear"
        :this-month-man-hour-statuses="thisMonthManHourStatuses"
        :last-month-man-hour-statuses="lastMonthManHourStatuses"
      />
    </div>
  </RootTemplate>
</template>

<script lang="ts">
import { format } from 'date-fns'
import HomeProjects from '../organisms/HomeProjects.vue'
import HomeKarten from '../organisms/HomeKarten.vue'
import HomeSurveySummaries, {
  SearchParamKpi,
  SearchParamSection,
} from '../organisms/HomeSurveySummaries.vue'
import HomeQuestionnaire from '../organisms/HomeQuestionnaire.vue'
import HomeManHour from '../organisms/HomeManHour.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { ProjectListItem } from '~/models/Project'
import { KartenLatestListItem } from '~/models/Karte'
import { SurveyByMineItem } from '~/models/Survey'
import { GetManHourByMineResponse } from '~/models/ManHour'
import { meStore } from '~/store'

import {
  Button,
  Card,
  Icon,
  SimpleTable,
  DataTable,
  RadioGroup,
  Sheet,
  Chip,
} from '~/components/common/atoms/index'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    HomeProjects,
    HomeKarten,
    HomeSurveySummaries,
    HomeQuestionnaire,
    HomeManHour,
    Button,
    Card,
    Icon,
    SimpleTable,
    DataTable,
    RadioGroup,
    Sheet,
    Chip,
  },
  props: {
    /**
     * 当月
     */
    thisMonth: {
      type: Number,
    },
    /**
     * 当年度
     */
    thisMonthYear: {
      type: Number,
    },
    /**
     * 前月
     */
    lastMonth: {
      type: Number,
    },
    /**
     * 前年度
     */
    lastMonthYear: {
      type: Number,
    },
    /**
     * 案件情報
     */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
    },
    /**
     * カルテ情報
     */
    karten: {
      type: Array as PropType<KartenLatestListItem[]>,
    },
    /**
     * 前年度
     */
    surveySummarySupporterOrganizations: {
      type: Object,
      required: true,
    },
    /**
     * 自身の回答対象の案件アンケートの一覧
     */
    surveySummaryAll: {
      type: Object,
      required: true,
    },
    /**
     * 課別集計 絞り込み「表示期間」
     */
    surveySummarySupporterOrganizationsParams: {
      type: Object,
      required: true,
    },
    /**
     * 課別集計 絞り込み「表示期間」
     */
    surveySummaryAllParams: {
      type: Object,
      required: true,
    },
    /**
     * 担当案件集計 最終集計日時
     */
    batchControlKpi: {
      type: Object,
      required: true,
    },
    /**
     * 課別集計 最終集計日時
     */
    batchControlSection: {
      type: Object,
      required: true,
    },
    /**
     * アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyByMineItem[]>,
    },
    /**
     * 当月の支援工数情報
     */
    thisMonthManHourStatuses: {
      type: Object as PropType<GetManHourByMineResponse>,
    },
    /**
     * 先月の支援工数情報
     */
    lastMonthManHourStatuses: {
      type: Object as PropType<GetManHourByMineResponse>,
    },
  },
  data() {
    return {
      searchParamKpi: new SearchParamKpi(),
      searchParamSection: new SearchParamSection(),
      apiNameKpi: 'getSurveySummaryAll',
      apiNameSection: 'getSurveySummarySupporterOrganizations',
    }
  },
  computed: {
    isSales() {
      return meStore.role === ENUM_USER_ROLE.SALES
    },
    isSalesMgr() {
      return meStore.role === ENUM_USER_ROLE.SALES_MGR
    },
    /**
     * 営業担当者、営業責任者および工数調査不要のユーザーの場合はHOME画面の「支援工数」を非表示にする
     */
    showsHomeManHour() {
      return (
        meStore.role !== ENUM_USER_ROLE.SALES &&
        meStore.role !== ENUM_USER_ROLE.SALES_MGR &&
        meStore.role !== ENUM_USER_ROLE.BUSINESS_MGR &&
        meStore.isInputManHour === true
      )
    },
  },
  methods: {
    /**
     * 担当案件集計 絞り込み「表示期間」フォームの値の変更
     */
    updateParamKpi(keyName: string, newVal: any) {
      // @ts-ignore
      this.searchParamKpi[keyName] = newVal
    },
    /**
     * 担当案件集計 絞り込み「表示期間」のリセット
     */
    clearKpi() {
      this.searchParamKpi = new SearchParamKpi()
      this.searchKpi()
    },
    /**
     * 担当案件集計 絞り込み「表示期間」
     */
    searchKpi() {
      const strYearMonthFrom = this.searchParamKpi.yearMonthFrom
      const strYearMonthTo = this.searchParamKpi.yearMonthTo
      let intYearMonthFrom = null
      let intYearMonthTo = null
      if (strYearMonthFrom != null && !Number.isInteger(strYearMonthFrom)) {
        // @ts-ignore
        intYearMonthFrom = parseInt(
          format(
            new Date(strYearMonthFrom),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (strYearMonthTo != null && !Number.isInteger(strYearMonthTo)) {
        // @ts-ignore
        intYearMonthTo = parseInt(
          format(
            new Date(strYearMonthTo),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (
        intYearMonthFrom != null &&
        intYearMonthTo != null &&
        intYearMonthFrom > intYearMonthTo
      ) {
        this.searchParamKpi.yearMonthFrom = strYearMonthTo
        this.searchParamKpi.yearMonthTo = strYearMonthFrom
      }
      const request = new SearchParamKpi()
      Object.assign(request, this.searchParamKpi)
      this.$emit(this.apiNameKpi, request)
    },
    /**
     * 課別集計 絞り込み「表示期間」フォームの値の変更
     */
    updateParamSection(keyName: string, newVal: any) {
      // @ts-ignore
      this.searchParamSection[keyName] = newVal
    },
    /**
     * 課別集計 絞り込み「表示期間」のリセット
     */
    clearSection() {
      this.searchParamSection = new SearchParamSection()
      this.searchSection()
    },
    /**
     * 課別集計 絞り込み「表示期間」
     */
    searchSection() {
      const strYearMonthFrom = this.searchParamSection.yearMonthFrom
      const strYearMonthTo = this.searchParamSection.yearMonthTo
      let intYearMonthFrom = null
      let intYearMonthTo = null
      if (strYearMonthFrom != null && !Number.isInteger(strYearMonthFrom)) {
        intYearMonthFrom = parseInt(
          format(
            new Date(strYearMonthFrom),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (strYearMonthTo != null && !Number.isInteger(strYearMonthTo)) {
        intYearMonthTo = parseInt(
          format(
            new Date(strYearMonthTo),
            this.$t('common.format.date_ym3') as string
          )
        )
      }
      if (
        intYearMonthFrom != null &&
        intYearMonthTo != null &&
        intYearMonthFrom > intYearMonthTo
      ) {
        this.searchParamSection.yearMonthFrom = strYearMonthTo
        this.searchParamSection.yearMonthTo = strYearMonthFrom
      }
      const request = new SearchParamSection()
      Object.assign(request, this.searchParamSection)
      this.$emit(this.apiNameSection, request)
    },
  },
})
</script>

<style lang="scss" scoped>
.home-supporter {
  width: 100%;
  padding: 0 0 80px;
  > section {
    &:nth-of-type(n + 2) {
      margin-top: 36px;
    }
  }
}
</style>
