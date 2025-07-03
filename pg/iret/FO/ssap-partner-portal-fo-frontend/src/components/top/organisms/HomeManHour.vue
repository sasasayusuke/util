<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.manHour') }}
        </h2>
      </v-flex>
    </v-container>
    <v-container class="pa-0">
      <HomeManHourCard
        :this-month="thisMonth"
        :this-month-year="thisMonthYear"
        :last-month="lastMonth"
        :last-month-year="lastMonthYear"
        :this-month-man-hour-statuses="thisMonthManHourStatuses"
        :last-month-man-hour-statuses="lastMonthManHourStatuses"
      />
    </v-container>
  </section>
</template>

<script lang="ts">
import HomeManHourCard from '../molecules/HomeManHourCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { GetManHourByMineResponse } from '~/models/ManHour'

export default BaseComponent.extend({
  components: {
    HomeManHourCard,
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
