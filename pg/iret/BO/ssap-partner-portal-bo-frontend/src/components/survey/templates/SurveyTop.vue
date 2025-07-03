<template>
  <RootTemplate>
    <!-- 年度別アンケート回収状況 -->
    <SurveyTopCollectionStatusContainer
      :is-loading="isLoading.surveySummaryAllCollectionStatus"
      :params="params.surveySummaryAllCollectionStatus"
      :survey-summary-all="surveySummaryAllCollectionStatus"
      :batch-control="batchControlCollectionStatus"
      @getSurveySummaryAll="getSurveySummaryAll"
    />
    <!-- KPI -->
    <SurveyTopKpiContainer
      class="mt-8"
      :is-loading="isLoading.surveySummaryAllKpi"
      :params="params.surveySummaryAllKpi"
      :survey-summary-all="surveySummaryAllKpi"
      :batch-control="batchControlKpi"
      @getSurveySummaryAll="getSurveySummaryAll"
    />
    <!-- サービスアンケート -->
    <SurveyTopServiceContainer
      class="mt-8"
      :is-loading="isLoading.surveySummaryAllService"
      :params="params.surveySummaryAllService"
      :survey-summary-all="surveySummaryAllService"
      :batch-control="batchControlService"
      @getSurveySummaryAll="getSurveySummaryAll"
    />
    <!-- 修了アンケート -->
    <SurveyTopCompletionContainer
      class="mt-8"
      :is-loading="isLoading.surveySummaryAllCompletion"
      :params="params.surveySummaryAllCompletion"
      :survey-summary-all="surveySummaryAllCompletion"
      :batch-control="batchControlCompletion"
      @getSurveySummaryAll="getSurveySummaryAll"
    />
    <!-- サービス／修了アンケート合算 -->
    <SurveyTopServiceAndCompletionContainer
      class="mt-8"
      :is-loading="isLoading.surveySummaryAllServiceAndCompletion"
      :params="params.surveySummaryAllServiceAndCompletion"
      :survey-summary-all="surveySummaryAllServiceAndCompletion"
      :batch-control="batchControlServiceAndCompletion"
      @getSurveySummaryAll="getSurveySummaryAll"
    />
    <!-- 課別集計 -->
    <SurveyTopSectionContainer
      class="mt-8"
      :is-loading="isLoading.surveySummarySupporterOrganizations"
      :params="params.surveySummarySupporterOrganizations"
      :survey-summary-supporter-organizations="
        surveySummarySupporterOrganizations
      "
      :batch-control="batchControlSection"
      @getSurveySummarySupporterOrganizations="
        getSurveySummarySupporterOrganizations
      "
      @exportSurveys="exportSurveys"
    />
    <!-- Partner Portal利用アンケート -->
    <SurveyTopPartnerPortalContainer
      class="mt-8"
      :is-loading="isLoading.surveySummaryAllPartnerPortal"
      :params="params.surveySummaryAllPartnerPortal"
      :survey-summary-all="surveySummaryAllPartnerPortal"
      :batch-control="batchControlPartnerPortal"
      @getSurveySummaryAll="getSurveySummaryAll"
      @exportSurveys="exportSurveys"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import SurveyTopCollectionStatusContainer from '~/components/survey/organisms/SurveyTopCollectionStatusContainer.vue'
import SurveyTopKpiContainer from '~/components/survey/organisms/SurveyTopKpiContainer.vue'
import SurveyTopServiceContainer from '~/components/survey/organisms/SurveyTopServiceContainer.vue'
import SurveyTopCompletionContainer from '~/components/survey/organisms/SurveyTopCompletionContainer.vue'
import SurveyTopServiceAndCompletionContainer from '~/components/survey/organisms/SurveyTopServiceAndCompletionContainer.vue'
import SurveyTopSectionContainer from '~/components/survey/organisms/SurveyTopSectionContainer.vue'
import SurveyTopPartnerPortalContainer from '~/components/survey/organisms/SurveyTopPartnerPortalContainer.vue'

import {
  GetSurveySummaryAllRequest,
  GetSurveySummaryAllResponse,
  GetSurveySummarySupporterOrganizationsRequest,
  GetSurveySummarySupporterOrganizationsResponse,
} from '~/models/Survey'
import { IExportSurveysRequest } from '@/types/Survey'

import { GetBatchControlByIdResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    SurveyTopCollectionStatusContainer,
    SurveyTopKpiContainer,
    SurveyTopServiceContainer,
    SurveyTopCompletionContainer,
    SurveyTopServiceAndCompletionContainer,
    SurveyTopSectionContainer,
    SurveyTopPartnerPortalContainer,
  },
  props: {
    /** 読み込み中か */
    isLoading: {
      type: Object,
      required: true,
    },
    /** 各表示対象の表示期間 */
    params: {
      type: Object,
      required: true,
    },
    /** 年度別アンケート回収状況に表示するアンケート全集計結果 */
    surveySummaryAllCollectionStatus: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** KPIに表示するアンケート全集計結果 */
    surveySummaryAllKpi: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** サービスアンケートに表示するアンケート全集計結果 */
    surveySummaryAllService: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** 修了アンケートに表示するアンケート全集計結果 */
    surveySummaryAllCompletion: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** サービスアンケート／修了アンケート合算に表示するアンケート全集計結果 */
    surveySummaryAllServiceAndCompletion: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** PartnerPortal利用アンケートに表示するアンケート全集計結果 */
    surveySummaryAllPartnerPortal: {
      type: Object as PropType<GetSurveySummaryAllResponse>,
      required: true,
    },
    /** 課別集計結果 */
    surveySummarySupporterOrganizations: {
      type: Object as PropType<GetSurveySummarySupporterOrganizationsResponse>,
      required: true,
    },
    /** 年度別アンケート回収状況に表示する最終集計日時 */
    batchControlCollectionStatus: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** KPIに表示する最終集計日時 */
    batchControlKpi: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** サービスアンケートに表示する最終集計日時 */
    batchControlService: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** 修了アンケートに表示する最終集計日時 */
    batchControlCompletion: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** サービスアンケート／修了アンケート合算に最終集計日時 */
    batchControlServiceAndCompletion: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** 課別集計に表示する最終集計日時 */
    batchControlSection: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
    /** PartnerPortal利用アンケートに表示する最終集計日時 */
    batchControlPartnerPortal: {
      type: Object as PropType<GetBatchControlByIdResponse>,
      required: true,
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * アンケートの全集計結果を取得。
     * @param item 取得項目
     * @param params getSurveySummaryAllAPIのリクエストパラメータ
     */
    getSurveySummaryAll(
      item: String,
      params: GetSurveySummaryAllRequest = new GetSurveySummaryAllRequest()
    ) {
      this.$emit('getSurveySummaryAll', item, params)
    },
    /**
     * アンケートの課別集計結果を取得。
     * @param params getSurveySummarySupporterOrganizationsAPIのリクエストパラメータ
     */
    getSurveySummarySupporterOrganizations(
      params: GetSurveySummarySupporterOrganizationsRequest = new GetSurveySummarySupporterOrganizationsRequest()
    ) {
      this.$emit('getSurveySummarySupporterOrganizations', params)
    },
    /**
     * 支援者別アンケート集計CSVまたは課別アンケート集計CSVを出力。
     * @param params exportSurveysAPIのリクエストパラメータを指定
     */
    exportSurveys(
      params: IExportSurveysRequest = {
        summaryMonthFrom: 0,
        summaryMonthTo: 0,
        mode: '',
      }
    ) {
      this.$emit('exportSurveys', params)
    },
  },
})
</script>
