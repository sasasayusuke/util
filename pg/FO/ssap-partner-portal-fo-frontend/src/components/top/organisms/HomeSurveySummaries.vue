<template>
  <section class="o-HomeSurveySummaries">
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.surveySummaries') }}
        </h2>
      </v-flex>
    </v-container>
    <v-container class="pa-0">
      <Card elevation="3">
        <Heading :level="3" class="type3">
          <template #title>
            {{ $t('top.pages.home.surveys.summaries.kpi.title')
            }}<span class="o-HomeSurveySummaries__required"
              ><Required /><span
                class="o-HomeSurveySummaries__required__text"
                >{{ $t('common.label.required2') }}</span
              ></span
            ></template
          >
          <template #date>{{
            $t('top.pages.home.surveys.summaries.kpi.update', {
              date: getBatchEndAtKpi(),
            })
          }}</template>
          <template #button>
            <Button outlined color="primary" small :to="`/project/list/me`">
              {{ $t('top.pages.home.button.project.me') }}
            </Button>
          </template>
        </Heading>
        <v-container class="py-5 px-9">
          <TermPicker
            :param="paramKpi"
            v-on="$listeners"
            @sort="$emit('sort-kpi')"
            @clear="$emit('clear-kpi')"
          />
          <div class="d-flex justify-space-between align-center mt-8">
            <HomeKpiChart
              :survey-summary-all="surveySummaryAll"
              :survey-summary-all-params="surveySummaryAllParams"
            />
          </div>
        </v-container>
      </Card>
    </v-container>
    <v-container class="pa-0 mt-6">
      <Card elevation="3">
        <Heading :level="3" class="type3">
          <template #title
            >{{ $t('top.pages.home.surveys.summaries.section.title')
            }}<span class="o-HomeSurveySummaries__required"
              ><Required /><span
                class="o-HomeSurveySummaries__required__text"
                >{{ $t('common.label.required2') }}</span
              ></span
            ></template
          >
          <template #date>{{
            $t('top.pages.home.surveys.summaries.section.update', {
              date: getBatchEndAtSection(),
            })
          }}</template>
          <template #button>
            <Button outlined color="primary" small :to="`/survey/admin/list`">
              {{ $t('top.pages.home.button.survey.admin') }}
            </Button>
          </template>
        </Heading>
        <v-container class="py-5 px-9">
          <TermPicker
            :param="paramSection"
            v-on="$listeners"
            @sort="$emit('sort-section')"
            @clear="$emit('clear-section')"
          />
          <div class="d-flex justify-space-between align-center mt-8">
            <HomeSectionChart
              :survey-summary-supporter-organizations="
                surveySummarySupporterOrganizations
              "
              :survey-summary-supporter-organizations-params="
                surveySummarySupporterOrganizationsParams
              "
            />
          </div>
        </v-container>
      </Card>
    </v-container>
  </section>
</template>

<script lang="ts">
import { format, parseISO } from 'date-fns'
import { getFiscalYearStart, getFiscalYearEnd } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import Heading from '~/components/common/molecules/Heading.vue'
import TermPicker from '~/components/common/molecules/TermPicker.vue'
import HomeKpiChart from '~/components/top/molecules/HomeKpiChart.vue'
import HomeSectionChart from '~/components/top/molecules/HomeSectionChart.vue'

import {
  Button,
  Card,
  Icon,
  SimpleTable,
  DataTable,
  RadioGroup,
  Sheet,
  Chip,
  Required,
} from '~/components/common/atoms/index'

export class SearchParamKpi {
  yearMonthFrom: string
  yearMonthTo: string

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.yearMonthFrom = format(fiscalyearStart, 'yyyy/MM')

    const fiscalyearEnd = getFiscalYearEnd()
    this.yearMonthTo = format(fiscalyearEnd, 'yyyy/MM')
  }
}

export class SearchParamSection {
  yearMonthFrom: string
  yearMonthTo: string

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.yearMonthFrom = format(fiscalyearStart, 'yyyy/MM')

    const fiscalyearEnd = getFiscalYearEnd()
    this.yearMonthTo = format(fiscalyearEnd, 'yyyy/MM')
  }
}

export default BaseComponent.extend({
  components: {
    Button,
    Card,
    Icon,
    SimpleTable,
    DataTable,
    RadioGroup,
    Sheet,
    Chip,
    Required,
    Heading,
    TermPicker,
    HomeKpiChart,
    HomeSectionChart,
  },
  props: {
    /**
     * アンケートの課（粗利メイン課）別の集計結果
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
     * 担当案件集計 絞り込み「表示期間」
     */
    paramKpi: {
      type: Object,
      required: true,
    },
    /**
     * 課別集計 絞り込み「表示期間」
     */
    paramSection: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      radio: '',
      radio2: '',
    }
  },
  methods: {
    /**
     * 担当案件集計 最終集計日時の書式を整形
     */
    getBatchEndAtKpi() {
      return this.batchControlKpi && this.batchControlKpi.batchEndAt
        ? format(
            parseISO(this.batchControlKpi.batchEndAt),
            this.$t('common.format.date_ymd6') as string
          )
        : 'ー'
    },
    /**
     * 課別集計 最終集計日時の書式を整形
     */
    getBatchEndAtSection() {
      return this.batchControlSection && this.batchControlSection.batchEndAt
        ? format(
            parseISO(this.batchControlSection.batchEndAt),
            this.$t('common.format.date_ymd6') as string
          )
        : 'ー'
    },
  },
})
</script>

<style lang="scss" scoped>
// .data-table {
//   border-top: 1px solid #000;
//   border-left: 1px solid #000;
//   border-collapse: collapse;
//   width: 540px;
//   th,
//   td {
//     border-bottom: 1px solid #000;
//     border-right: 1px solid #000;
//     padding: 4px 8px;
//   }
//   th {
//     font-size: 0.75rem;
//   }
//   td {
//     font-size: 0.625rem;
//   }
//   tbody {
//     th {
//       text-align: left;
//     }
//   }
// }
.o-HomeSurveySummaries__required {
  @include fontSize('xsmall');
  font-weight: normal;
  margin-left: 40px;
}
.o-HomeSurveySummaries__required__text {
  color: $c-black-60;
}
</style>
