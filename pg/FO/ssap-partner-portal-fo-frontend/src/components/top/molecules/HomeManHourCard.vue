<template>
  <Card elevation="3">
    <SimpleTable>
      <tbody>
        <tr
          @click="
            $router.push({
              path: `/man-hour?year=${thisMonthYear}&month=${thisMonth}`,
            })
          "
        >
          <th class="font-size-normal">
            [{{
              $t('top.pages.home.manHour.currentMonth')
            }}]&nbsp;&nbsp;&nbsp;{{ thisMonthYear }}/{{
              ('00' + thisMonth).slice(-2)
            }}
          </th>
          <td class="font-size-small font-weight-bold">
            <Chip
              v-if="thisMonthManHourStatuses.isConfirm"
              label
              small
              class="width-70 mr-2"
              color="secondary"
            >
              {{ $t('top.pages.home.manHour.confirm') }}
            </Chip>
            <Chip
              v-else
              label
              small
              outlined
              class="width-70 mr-2"
              color="secondary"
            >
              {{ $t('top.pages.home.manHour.noConfirm') }}
            </Chip>
            {{ $t('top.pages.home.manHour.total') }} :
            {{ getThisMonthSummaryManHour('total') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >1. {{ $t('top.pages.home.manHour.direct') }}</span
            ><br />
            {{ getThisMonthSummaryManHour('direct') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >2. {{ $t('top.pages.home.manHour.pre') }}</span
            ><br />
            {{ getThisMonthSummaryManHour('pre') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >3. {{ $t('top.pages.home.manHour.sales') }}</span
            ><br />
            {{ getThisMonthSummaryManHour('sales') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >4. {{ $t('top.pages.home.manHour.ssap') }}</span
            ><br />
            {{ getThisMonthSummaryManHour('ssap') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >5. {{ $t('top.pages.home.manHour.others') }}</span
            ><br />
            {{ getThisMonthSummaryManHour('others') }}h
          </td>
          <td class="text-right">
            <Icon size="16px">icon-org-arrow-right</Icon>
          </td>
        </tr>
        <tr
          @click="
            $router.push(`/man-hour?year=${lastMonthYear}&month=${lastMonth}`)
          "
        >
          <th class="font-size-normal">
            [{{ $t('top.pages.home.manHour.lastMonth') }}]&nbsp;&nbsp;&nbsp;{{
              lastMonthYear
            }}/{{ ('00' + lastMonth).slice(-2) }}
          </th>
          <td class="font-size-small font-weight-bold">
            <Chip
              v-if="lastMonthManHourStatuses.isConfirm"
              label
              small
              class="width-70 mr-2"
              color="secondary"
            >
              {{ $t('top.pages.home.manHour.confirm') }}
            </Chip>
            <Chip
              v-else
              label
              small
              outlined
              class="width-70 mr-2"
              color="secondary"
            >
              {{ $t('top.pages.home.manHour.noConfirm') }}
            </Chip>
            {{ $t('top.pages.home.manHour.total') }} :
            {{ getLastMonthSummaryManHour('total') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >1. {{ $t('top.pages.home.manHour.direct') }}</span
            ><br />
            {{ getLastMonthSummaryManHour('direct') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >2. {{ $t('top.pages.home.manHour.pre') }}</span
            ><br />
            {{ getLastMonthSummaryManHour('pre') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >3. {{ $t('top.pages.home.manHour.sales') }}</span
            ><br />
            {{ getLastMonthSummaryManHour('sales') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >4. {{ $t('top.pages.home.manHour.ssap') }}</span
            ><br />
            {{ getLastMonthSummaryManHour('ssap') }}h
          </td>
          <td class="font-size-small font-weight-bold text-center">
            <span class="font-weight-medium font-size-xsmall"
              >5. {{ $t('top.pages.home.manHour.others') }}</span
            ><br />
            {{ getLastMonthSummaryManHour('others') }}h
          </td>
          <td class="text-right">
            <Icon size="16px">icon-org-arrow-right</Icon>
          </td>
        </tr>
      </tbody>
    </SimpleTable>
  </Card>
</template>

<script lang="ts">
import CommonWideClickableCard from '~/components/common/molecules/CommonWideClickableCard.vue'
import BaseComponent from '~/common/BaseComponent'
import { Card, SimpleTable, Chip, Icon } from '~/components/common/atoms/index'
import type { PropType } from '~/common/BaseComponent'
import { GetManHourByMineResponse } from '~/models/ManHour'

export default BaseComponent.extend({
  components: {
    CommonWideClickableCard,
    Card,
    SimpleTable,
    Chip,
    Icon,
  },
  methods: {
    /**
     * 自身の当月の支援工数情報を取得
     */
    getThisMonthSummaryManHour(this: any, key: string) {
      return this.thisMonthManHourStatuses &&
        this.thisMonthManHourStatuses.summaryManHour &&
        this.thisMonthManHourStatuses.summaryManHour[key]
        ? this.thisMonthManHourStatuses.summaryManHour[key]
        : 0
    },
    /**
     * 自身の先月の支援工数情報を取得
     */
    getLastMonthSummaryManHour(this: any, key: string) {
      return this.lastMonthManHourStatuses &&
        this.lastMonthManHourStatuses.summaryManHour &&
        this.lastMonthManHourStatuses.summaryManHour[key]
        ? this.lastMonthManHourStatuses.summaryManHour[key]
        : 0
    },
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
     * 自身の今月の支援工数情報
     */
    thisMonthManHourStatuses: {
      type: Object as PropType<GetManHourByMineResponse>,
    },
    /**
     * 自身の先月の支援工数情報
     */
    lastMonthManHourStatuses: {
      type: Object as PropType<GetManHourByMineResponse>,
    },
  },
})
</script>
