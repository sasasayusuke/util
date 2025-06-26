<template>
  <div class="m-supporter-man-hours-table">
    <CommonDataTable
      :total="supporterManHours.length"
      :is-loading="isLoading"
      :headers="manHourHeaders"
      :items="formattedSupporterManHours"
      :shows-pagination="false"
      short-text
      is-csv-output
      :csv-button-disabled="parseInt(supporterManHours.length) <= 0"
      link-prefix="/project"
      @csvOutput="$emit('csvOutput')"
    >
      <!-- 支援者名(担当別工数詳細へリンク) -->
      <template #[`item.supporterName`]="{ item }">
        <nuxt-link
          class="o-common-data-table__link"
          :to="`/man-hour/supporter/${$route.params.year}/${$route.params.month}/${item.supporterId}`"
        >
          {{ item.supporterName }}
        </nuxt-link>
      </template>
      <!-- ステータス -->
      <template #[`item.isConfirm`]="{ item }">
        <Chip v-if="!item.isConfirm" small style-set="secondary-70" outlined>
          {{ $t('common.label.unsettled') }}
        </Chip>
      </template>
    </CommonDataTable>
  </div>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import { Chip } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'ManHourSupporterListTable',
  components: {
    CommonDataTable,
    Chip,
  },
  props: {
    /** 月次担当者別工数一覧 */
    supporterManHours: {
      type: Array,
      required: true,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
  },
  data() {
    return {
      manHourHeaders: [
        {
          text: this.$t(
            'man-hour.pages.supporter.header.supporterOrganizationName'
          ),
          value: 'supporterOrganizationName',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.supporterName'),
          value: 'supporterName',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.isConfirm'),
          value: 'isConfirm',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourDirect'),
          value: 'summaryManHourDirect',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourPre'),
          value: 'summaryManHourPre',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourSales'),
          value: 'summaryManHourSales',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourSsap'),
          value: 'summaryManHourSsap',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourOthers'),
          value: 'summaryManHourOthers',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.summaryManHourTotal'),
          value: 'summaryManHourTotal',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.contractTimeMain'),
          value: 'contractTimeMain',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.contractTimeOther'),
          value: 'contractTimeOther',
          sortable: false,
        },
        {
          text: this.$t('man-hour.pages.supporter.header.contractTimeTotal'),
          value: 'contractTimeTotal',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedSupporterManHours() {
      const supporterManHours = cloneDeep(this.supporterManHours)
      return supporterManHours.map((supporter: any) => {
        Object.keys(supporter.summaryManHour).map((key) => {
          return (supporter.summaryManHour[key] += 'h')
        })

        Object.keys(supporter.contractTime).map((key) => {
          return (supporter.contractTime[key] += 'h')
        })
        supporter.summaryManHourDirect = supporter.summaryManHour.direct
        supporter.summaryManHourPre = supporter.summaryManHour.pre
        supporter.summaryManHourSales = supporter.summaryManHour.sales
        supporter.summaryManHourSsap = supporter.summaryManHour.ssap
        supporter.summaryManHourOthers = supporter.summaryManHour.others
        supporter.summaryManHourTotal = supporter.summaryManHour.total
        supporter.contractTimeMain = supporter.contractTime.producer
        supporter.contractTimeOther = supporter.contractTime.accelerator
        supporter.contractTimeTotal = supporter.contractTime.total
        return supporter
      })
    },
  },
})
</script>

<style lang="scss">
.m-supporter-man-hours-table {
  .v-data-table {
    .v-data-table__wrapper {
      table {
        thead {
          tr {
            th {
              padding: 0 10px;
              &:first-child {
                padding: 0 10px 0 20px;
              }
            }
          }
        }
        tbody {
          tr {
            td {
              padding: 16px 10px;
              font-size: 0.75rem;
              &:first-child {
                padding: 16px 10px 16px 20px;
              }
              &:nth-of-type(2) {
                width: 155px;
              }
              &:nth-of-type(9),
              &:nth-of-type(12) {
                font-size: 0.875rem;
                font-weight: bold;
              }
              &:nth-of-type(10),
              &:nth-of-type(11) {
                width: 120px;
              }
            }
          }
        }
      }
    }
  }
}
</style>
