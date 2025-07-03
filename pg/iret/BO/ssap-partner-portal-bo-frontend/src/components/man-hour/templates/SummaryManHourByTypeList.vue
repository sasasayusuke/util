<template>
  <RootTemPlate>
    <!-- ヘッダ -->
    <ListInPageHeader
      month-change
      :next-month-disabled="nextMonthDisabled"
      @buttonAction1="thisMonth"
      @buttonAction2="lastMonth"
      @buttonAction3="nextMonth"
    >
      {{ $t('man-hour.pages.report.title') }}&nbsp;&nbsp; {{ `${year()}`
      }}{{ $t('man-hour.unit.year') }}{{ `${month()}`
      }}{{ $t('man-hour.unit.month') }}
    </ListInPageHeader>
    <!-- 一覧リスト -->
    <SummaryManHourListTable
      :man-hours="response.manHours"
      :header="response.header"
      :is-loading="isLoading.manHours"
      class="mt-6"
      @csvOutput="$emit('csvOutput')"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import { format, parse } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import SummaryManHourListTable from '~/components/man-hour/organisms/SummaryManHourListTable.vue'
import { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetSummaryManHourTypeResponse } from '@/models/ManHour'

export interface isLoading {
  manHours: boolean
}

export default CommonList.extend({
  name: 'SummaryManHourByTypeList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    SummaryManHourListTable,
  },
  props: {
    /** 月次工数分類別工数一覧 */
    response: {
      type: Object as PropType<GetSummaryManHourTypeResponse>,
      required: true,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year(this: any) {
      return parseInt(this.$route.params.year)
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month(this: any) {
      return parseInt(this.$route.params.month)
    },
    /**
     * 今月ボタンが押下された際、今月に遷移
     * @param {number} year 現在の年
     * @param {number} month 現在の月
     */
    thisMonth() {
      const year = format(getCurrentDate(), 'yyyy')
      const month = format(getCurrentDate(), 'MM')
      this.$router.push(`/man-hour/report/${year}/${month}`)
    },
    /**
     * 前月ボタンが押下された際、前月に遷移
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @param {number} year 現在表示されている年
     * @param {number} month 現在表示されている月の前月
     */
    lastMonth() {
      const yearObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const monthObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const currentYear = parseInt(format(yearObject, 'yyyy'))
      const currentMonth = parseInt(format(monthObject, 'MM')) - 1
      const year = format(new Date(currentYear, currentMonth - 1), 'yyyy')
      const month = format(new Date(currentYear, currentMonth - 1), 'MM')
      this.$router.push(`/man-hour/report/${year}/${month}`)
    },
    /**
     * 翌月ボタンが押下された際、翌月に遷移
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @param {number} year 現在表示されている年
     * @param {number} month 現在表示されている月の翌月
     */
    nextMonth() {
      const yearObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const monthObject = parse(
        String(this.year()) + '/' + String(this.month()),
        'yyyy/MM',
        getCurrentDate()
      )
      const currentYear = parseInt(format(yearObject, 'yyyy'))
      const currentMonth = parseInt(format(monthObject, 'MM')) - 1
      const year = format(new Date(currentYear, currentMonth + 1), 'yyyy')
      const month = format(new Date(currentYear, currentMonth + 1), 'MM')
      this.$router.push(`/man-hour/report/${year}/${month}`)
    },
  },
  computed: {
    /**
     * 翌月ボタン無効判定
     * (表示年月と現在年月が同じ場合無効)
     * @param now  現在日時
     * @param {number} currentYear 現在表示されている年
     * @param {number} currentMonth 現在表示されている月
     * @returns 翌月ボタン無効か否か
     */
    nextMonthDisabled() {
      const now = getCurrentDate()
      const currentYear = now.getFullYear().toString()
      const currentMonth = (now.getMonth() + 1).toString()
      if (
        this.$route.params.year === currentYear &&
        this.$route.params.month === currentMonth
      ) {
        return true
      } else {
        return false
      }
    },
  },
})
</script>
