<template>
  <TemplateSurveyTop
    :is-loading="isLoading"
    :params="params"
    :survey-summary-all-collection-status="surveySummaryAllCollectionStatus"
    :survey-summary-all-kpi="surveySummaryAllKpi"
    :survey-summary-all-service="surveySummaryAllService"
    :survey-summary-all-completion="surveySummaryAllCompletion"
    :survey-summary-all-service-and-completion="
      surveySummaryAllServiceAndCompletion
    "
    :survey-summary-all-partner-portal="surveySummaryAllPartnerPortal"
    :survey-summary-supporter-organizations="
      surveySummarySupporterOrganizations
    "
    :batch-control-collection-status="batchControlCollectionStatus"
    :batch-control-kpi="batchControlKpi"
    :batch-control-service="batchControlService"
    :batch-control-completion="batchControlCompletion"
    :batch-control-service-and-completion="batchControlServiceAndCompletion"
    :batch-control-section="batchControlSection"
    :batch-control-partner-portal="batchControlPartnerPortal"
    @getSurveySummarySupporterOrganizations="
      getSurveySummarySupporterOrganizations
    "
    @getSurveySummaryAll="getSurveySummaryAll"
    @exportSurveys="exportSurveys"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateSurveyTop from '~/components/survey/templates/SurveyTop.vue'
import {
  GetSurveySummaryAllRequest,
  GetSurveySummaryAllResponse,
  GetSurveySummaryAll,
  GetSurveySummarySupporterOrganizationsRequest,
  GetSurveySummarySupporterOrganizationsResponse,
  GetSurveySummarySupporterOrganizations,
  ExportSurveys,
} from '~/models/Survey'
import { IExportSurveysRequest } from '@/types/Survey'

import {
  GetBatchControlByIdResponse,
  GetBatchControlById,
} from '~/models/Master'

export default BasePage.extend({
  name: 'SurveyTop',
  middleware: ['roleCheck'],
  components: {
    TemplateSurveyTop,
  },
  head() {
    return {
      title: this.$t('survey.pages.index.name') as string,
    }
  },
  data() {
    return {
      surveySummaryAllItems: [
        'CollectionStatus',
        'Kpi',
        'Service',
        'Completion',
        'ServiceAndCompletion',
        'PartnerPortal',
      ],
      surveySummaryAllCollectionStatus: new GetSurveySummaryAllResponse(),
      surveySummaryAllKpi: new GetSurveySummaryAllResponse(),
      surveySummaryAllService: new GetSurveySummaryAllResponse(),
      surveySummaryAllCompletion: new GetSurveySummaryAllResponse(),
      surveySummaryAllServiceAndCompletion: new GetSurveySummaryAllResponse(),
      surveySummaryAllPartnerPortal: new GetSurveySummaryAllResponse(),
      surveySummarySupporterOrganizations:
        new GetSurveySummarySupporterOrganizationsResponse(),
      batchControlIds: {
        CollectionStatus: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
        Kpi: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
        Service: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
        Completion: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
        ServiceAndCompletion: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
        Section: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#supporter_organization`
        ),
        PartnerPortal: encodeURIComponent(
          `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_survey#all`
        ),
      },
      batchControlCollectionStatus: new GetBatchControlByIdResponse(),
      batchControlKpi: new GetBatchControlByIdResponse(),
      batchControlService: new GetBatchControlByIdResponse(),
      batchControlCompletion: new GetBatchControlByIdResponse(),
      batchControlServiceAndCompletion: new GetBatchControlByIdResponse(),
      batchControlSection: new GetBatchControlByIdResponse(),
      batchControlPartnerPortal: new GetBatchControlByIdResponse(),
      isLoading: {
        surveySummaryAllCollectionStatus: true,
        surveySummaryAllKpi: true,
        surveySummaryAllService: true,
        surveySummaryAllCompletion: true,
        surveySummaryAllServiceAndCompletion: true,
        surveySummaryAllPartnerPortal: true,
        surveySummarySupporterOrganizations: true,
      },
      params: {
        surveySummaryAllCollectionStatus: new GetSurveySummaryAllRequest(),
        surveySummaryAllKpi: new GetSurveySummaryAllRequest(),
        surveySummaryAllService: new GetSurveySummaryAllRequest(),
        surveySummaryAllCompletion: new GetSurveySummaryAllRequest(),
        surveySummaryAllServiceAndCompletion: new GetSurveySummaryAllRequest(),
        surveySummaryAllPartnerPortal: new GetSurveySummaryAllRequest(),
        surveySummarySupporterOrganizations:
          new GetSurveySummarySupporterOrganizationsRequest(),
      },
    }
  },
  mounted() {
    const promises = []
    promises.push(this.getSurveySummaryAll('all'))
    for (const i in this.surveySummaryAllItems) {
      promises.push(this.getBatchControlById(this.surveySummaryAllItems[i]))
    }
    promises.push(this.getSurveySummarySupporterOrganizations())
    promises.push(this.getBatchControlById('Section'))
    this.displayLoading(promises)
  },
  methods: {
    /**
     * getSurveySummaryAllAPIを叩き、アンケートの全集計結果を取得して各項目に代入。
     * @param item 取得項目を指定
     * @param params getSurveySummaryAllAPIのリクエストパラメータを指定
     */
    async getSurveySummaryAll(
      item: String,
      params: GetSurveySummaryAllRequest = new GetSurveySummaryAllRequest()
    ) {
      if (
        params.yearMonthFrom != null &&
        !Number.isInteger(params.yearMonthFrom)
      ) {
        params.yearMonthFrom = parseInt(
          format(new Date(params.yearMonthFrom), 'yyyyMM')
        )
      }
      if (params.yearMonthTo != null && !Number.isInteger(params.yearMonthTo)) {
        params.yearMonthTo = parseInt(
          format(new Date(params.yearMonthTo), 'yyyyMM')
        )
      }
      if (item === 'all') {
        this.isLoading.surveySummaryAllCollectionStatus = true
        this.isLoading.surveySummaryAllKpi = true
        this.isLoading.surveySummaryAllService = true
        this.isLoading.surveySummaryAllCompletion = true
        this.isLoading.surveySummaryAllServiceAndCompletion = true
        this.isLoading.surveySummaryAllPartnerPortal = true
        this.params.surveySummaryAllCollectionStatus = params
        this.params.surveySummaryAllKpi = params
        this.params.surveySummaryAllService = params
        this.params.surveySummaryAllCompletion = params
        this.params.surveySummaryAllServiceAndCompletion = params
        this.params.surveySummaryAllPartnerPortal = params
      } else if (item === 'CollectionStatus') {
        this.isLoading.surveySummaryAllCollectionStatus = true
        this.params.surveySummaryAllCollectionStatus = params
      } else if (item === 'Kpi') {
        this.isLoading.surveySummaryAllKpi = true
        this.params.surveySummaryAllKpi = params
      } else if (item === 'Service') {
        this.isLoading.surveySummaryAllService = true
        this.params.surveySummaryAllService = params
      } else if (item === 'Completion') {
        this.isLoading.surveySummaryAllCompletion = true
        this.params.surveySummaryAllCompletion = params
      } else if (item === 'ServiceAndCompletion') {
        this.isLoading.surveySummaryAllServiceAndCompletion = true
        this.params.surveySummaryAllServiceAndCompletion = params
      } else if (item === 'PartnerPortal') {
        this.isLoading.surveySummaryAllPartnerPortal = true
        this.params.surveySummaryAllPartnerPortal = params
      }
      await GetSurveySummaryAll(params).then((res) => {
        if (item === 'all') {
          this.surveySummaryAllCollectionStatus = res.data
          this.isLoading.surveySummaryAllCollectionStatus = false
          this.surveySummaryAllKpi = res.data
          this.isLoading.surveySummaryAllKpi = false
          this.surveySummaryAllService = res.data
          this.isLoading.surveySummaryAllService = false
          this.surveySummaryAllCompletion = res.data
          this.isLoading.surveySummaryAllCompletion = false
          this.surveySummaryAllServiceAndCompletion = res.data
          this.isLoading.surveySummaryAllServiceAndCompletion = false
          this.surveySummaryAllPartnerPortal = res.data
          this.isLoading.surveySummaryAllPartnerPortal = false
        } else if (item === 'CollectionStatus') {
          this.surveySummaryAllCollectionStatus = res.data
          this.isLoading.surveySummaryAllCollectionStatus = false
        } else if (item === 'Kpi') {
          this.surveySummaryAllKpi = res.data
          this.isLoading.surveySummaryAllKpi = false
        } else if (item === 'Service') {
          this.surveySummaryAllService = res.data
          this.isLoading.surveySummaryAllService = false
        } else if (item === 'Completion') {
          this.surveySummaryAllCompletion = res.data
          this.isLoading.surveySummaryAllCompletion = false
        } else if (item === 'ServiceAndCompletion') {
          this.surveySummaryAllServiceAndCompletion = res.data
          this.isLoading.surveySummaryAllServiceAndCompletion = false
        } else if (item === 'PartnerPortal') {
          this.surveySummaryAllPartnerPortal = res.data
          this.isLoading.surveySummaryAllPartnerPortal = false
        }
      })
    },
    /**
     * getSurveySummarySupporterOrganizationsAPIを叩き、アンケートの課別の集計結果を取得して代入。
     * @param params getSurveySummarySupporterOrganizationsAPIのリクエストパラメータを指定
     */
    async getSurveySummarySupporterOrganizations(
      params: GetSurveySummarySupporterOrganizationsRequest = new GetSurveySummarySupporterOrganizationsRequest()
    ) {
      this.isLoading.surveySummarySupporterOrganizations = true
      if (
        params.yearMonthFrom != null &&
        !Number.isInteger(params.yearMonthFrom)
      ) {
        params.yearMonthFrom = parseInt(
          format(new Date(params.yearMonthFrom), 'yyyyMM')
        )
      }
      if (params.yearMonthTo != null && !Number.isInteger(params.yearMonthTo)) {
        params.yearMonthTo = parseInt(
          format(new Date(params.yearMonthTo), 'yyyyMM')
        )
      }
      this.params.surveySummarySupporterOrganizations = params
      await GetSurveySummarySupporterOrganizations(params).then((res) => {
        this.surveySummarySupporterOrganizations = res.data
        this.isLoading.surveySummarySupporterOrganizations = false
      })
    },
    /**
     * getBatchControlByIdAPIを叩き、各種集計バッチ処理の最終完了日時を取得して代入。
     * @param item 取得項目を指定
     */
    async getBatchControlById(item: String) {
      // @ts-ignore
      await GetBatchControlById(String(this.batchControlIds[item])).then(
        (res) => {
          if (item === 'CollectionStatus') {
            this.batchControlCollectionStatus = res.data
          } else if (item === 'Kpi') {
            this.batchControlKpi = res.data
          } else if (item === 'Service') {
            this.batchControlService = res.data
          } else if (item === 'Completion') {
            this.batchControlCompletion = res.data
          } else if (item === 'ServiceAndCompletion') {
            this.batchControlServiceAndCompletion = res.data
          } else if (item === 'Section') {
            this.batchControlSection = res.data
          } else if (item === 'PartnerPortal') {
            this.batchControlPartnerPortal = res.data
          }
        }
      )
    },
    /**
     * exportSurveysAPIを叩き、支援者別アンケート集計CSVまたは課別アンケート集計CSVを出力。
     * @param params exportSurveysAPIのリクエストパラメータを指定
     */
    exportSurveys(
      params: IExportSurveysRequest = {
        summaryMonthFrom: 0,
        summaryMonthTo: 0,
        mode: '',
      }
    ) {
      const fileName =
        this.$t(`survey.pages.index.formatName.${params.mode}`) +
        '_' +
        String(format(getCurrentDate(), 'yyyyMMdd_HHmmss')) +
        '.csv'
      ExportSurveys(params)
        .then((res) => {
          if (res.data && res.data.url) {
            const url = res.data.url
            const link = document.createElement('a')
            link.download = fileName
            link.href = url
            document.body.appendChild(link)
            link.click()
            document.body.removeChild(link)
          }
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
  },
})
</script>
