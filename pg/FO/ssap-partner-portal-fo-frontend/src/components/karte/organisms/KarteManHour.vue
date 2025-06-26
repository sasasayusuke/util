<template>
  <Sheet class="o-karte-man-hour" style-set="information">
    <v-container fluid pa-0>
      <v-row no-gutters justify="space-between" align="center">
        <v-col class="o-karte-man-hour__title" cols="auto">
          <Title style-set="subTitle">{{
            $t('karte.pages.list.manHour.title')
          }}</Title>
        </v-col>
        <v-col cols="auto">
          <p class="o-karte-man-hour__update">
            {{
              $t('karte.pages.list.manHour.update', {
                date: getBatchEndAt(),
              })
            }}
          </p>
        </v-col>
      </v-row>
      <v-row no-gutters class="pt-5 pb-2" justify="center">
        <v-col cols="auto">
          <Table class="o-karte-man-hour__table man-hour">
            <template #header>
              <tr>
                <th colspan="2">{{ $t('karte.pages.list.manHour.costs') }}</th>
                <th>{{ $t('karte.pages.list.manHour.total') }}</th>
                <th>{{ $t('karte.pages.list.manHour.thisMonth') }}</th>
              </tr>
            </template>
            <template #body>
              <tr>
                <th class="is-emphasize text-center" colspan="2">
                  {{ $t('karte.pages.list.manHour.contractTime') }}
                </th>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.totalContractTime,
                    })
                  }}
                </td>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.thisMonthContractTime,
                    })
                  }}
                </td>
              </tr>
              <tr>
                <th class="is-emphasize" rowspan="2">
                  {{ $t('karte.pages.list.manHour.directSupport') }}
                </th>
                <th>{{ $t('karte.pages.list.manHour.supporter') }}</th>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.summarySupporterDirectSupportManHour,
                    })
                  }}
                </td>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.thisMonthSupporterDirectSupportManHour,
                    })
                  }}
                </td>
              </tr>
              <tr>
                <th>{{ $t('karte.pages.list.manHour.all') }}</th>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.summaryDirectSupportManHour,
                    })
                  }}
                </td>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.thisMonthDirectSupportManHour,
                    })
                  }}
                </td>
              </tr>
              <tr>
                <th class="is-emphasize" rowspan="2">
                  {{ $t('karte.pages.list.manHour.preSupport') }}
                </th>
                <th>{{ $t('karte.pages.list.manHour.supporter') }}</th>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.summarySupporterPreSupportManHour,
                    })
                  }}
                </td>
                <td class="text-center">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.thisMonthSupporterPreSupportManHour,
                    })
                  }}
                </td>
              </tr>
              <tr>
                <th>{{ $t('karte.pages.list.manHour.all') }}</th>
                <td class="text-center is-excess">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.summaryPreSupportManHour,
                    })
                  }}<br />
                  <span class="o-karte-man-hour__message">
                    <Icon color="error" size="18" class="mr-2"
                      >icon-org-alert</Icon
                    >{{ $t('karte.pages.list.manHour.excess') }}</span
                  >
                </td>
                <td class="text-center is-forecast">
                  {{
                    $t('karte.pages.list.manHour.time', {
                      time: manHourStatus.thisMonthPreSupportManHour,
                    })
                  }}<br />
                  <span class="o-karte-man-hour__message">
                    <Icon color="primary" size="18" class="mr-2"
                      >icon-org-alert</Icon
                    >{{ $t('karte.pages.list.manHour.forecast') }}</span
                  >
                </td>
              </tr>
            </template>
          </Table>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import { format, parseISO } from 'date-fns'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { Sheet, Title, Table, Icon } from '~/components/common/atoms'
import { GetSummaryProjectSupporterManHourStatusResponse } from '~/models/ManHour'
import { GetBatchControlByIdResponse } from '~/models/Master'

export default BaseComponent.extend({
  name: '',
  components: {
    Sheet,
    Title,
    Table,
    Icon,
  },
  props: {
    /**
     * 支援工数状況
     */
    manHourStatus: {
      type: Object as PropType<GetSummaryProjectSupporterManHourStatusResponse>,
      required: true,
    },
    /**
     * 各種集計バッチ処理の最終完了日時
     */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
    },
  },
  methods: {
    /**
     * 各種集計バッチ処理の最終完了日時を取得する
     * @returns フォーマット化した各種集計バッチ処理の最終完了日時
     */
    getBatchEndAt() {
      return this.batchControl && this.batchControl.batchEndAt
        ? format(
            parseISO(this.batchControl.batchEndAt),
            this.$t('common.format.date_ymd6') as string
          )
        : 'ー'
    },
  },
  computed: {},
  data() {
    return {}
  },
})
</script>

<style lang="scss" scoped>
.o-karte-man-hour__update {
  @include fontSize('xsmall');
  color: $c-black-60;
  margin: 0;
}
.o-karte-man-hour__table {
  table-layout: fixed;
  width: 740px !important;
  thead {
    th {
      background: $c-black-80;
      color: $c-white;
      text-align: center;
      font-weight: bold;
      width: 33.333%;
    }
  }
  tbody {
    th,
    td {
      color: $c-black;
      &.is-emphasize {
        color: $c-black;
        font-weight: normal;
      }
    }
  }
  td {
    vertical-align: middle;
  }
  .is-excess {
    background-color: $c-red-8;
    .o-karte-man-hour__message {
      color: $c-red;
    }
  }
  .is-forecast {
    background-color: $c-secondary-8;
    .o-karte-man-hour__message {
      color: $c-secondary;
    }
  }
}
.o-karte-man-hour__message {
  display: inline-flex;
  align-items: center;
}
</style>
