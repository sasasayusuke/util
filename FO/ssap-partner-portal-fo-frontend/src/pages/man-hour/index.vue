<template>
  <TemplateManHour
    :man-hour="manHour"
    :suggest-customers="suggestCustomersResponse"
    @refresh="refresh"
  />
</template>

<script lang="ts">
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateManHour from '~/components/man-hour/templates/ManHour.vue'
import {
  GetManHourByMineRequest,
  GetManHourByMineResponse,
  GetManHourByMine,
} from '~/models/ManHour'
import { SuggestCustomersResponse, SuggestCustomers } from '~/models/Customer'

export default BasePage.extend({
  name: 'MonHour',
  middleware: ['roleCheck'],
  components: {
    TemplateManHour,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.list.name') as string,
    }
  },
  created() {
    this.denyFuture()
  },
  mounted() {
    this.displayLoading([this.getManHourByMine(), this.SuggestCustomers()])
  },
  data(): {
    manHour: GetManHourByMineResponse
    suggestCustomersResponse: SuggestCustomersResponse
  } {
    return {
      manHour: new GetManHourByMineResponse(),
      suggestCustomersResponse: new SuggestCustomersResponse(),
    }
  },
  computed: {
    /**
     * クエリで指定された年月を取得する
     * 指定されていない場合は今月を返す
     * @returns {Date} 指定の日付、または現在の日付
     */
    queryDate(): Date {
      if (!this.$route.query.year || !this.$route.query.month) {
        return getCurrentDate()
      }

      const queryYear = parseInt(this.$route.query.year as string)
      const queryMonth = parseInt(this.$route.query.month as string)

      return new Date(queryYear, queryMonth - 1)
    },
  },
  methods: {
    /**
     * クエリパラメーターに翌月以降が指定されている場合は404エラーページに遷移
     */
    denyFuture() {
      const now = getCurrentDate()
      const isFuture = this.queryDate.getTime() > now.getTime()

      if (isFuture) {
        this.$router.push(`/404`)
      }
    },
    /**
     * GetManHourByMineAPIから支援工数データを取得する
     */
    async getManHourByMine() {
      // APIからの取得処理
      const request = new GetManHourByMineRequest()

      // 年月をリクエストに格納
      const year = this.queryDate.getFullYear()
      request.year = year
      const month = this.queryDate.getMonth() + 1
      request.month = month

      await GetManHourByMine(request)
        .then((res) => {
          this.manHour = res.data
          // データの存在しない月は数値にnullが返るためnullには0を入れる
          if (this.manHour.summaryManHour === null) {
            this.manHour.summaryManHour = {
              direct: 0,
              pre: 0,
              sales: 0,
              ssap: 0,
              others: 0,
              total: 0,
            }
          }
          if (this.manHour.summaryManHour.direct === null) {
            this.manHour.summaryManHour.direct = 0
          }
          if (this.manHour.summaryManHour.pre === null) {
            this.manHour.summaryManHour.pre = 0
          }
          if (this.manHour.summaryManHour.sales === null) {
            this.manHour.summaryManHour.sales = 0
          }
          if (this.manHour.summaryManHour.ssap === null) {
            this.manHour.summaryManHour.ssap = 0
          }
          if (this.manHour.summaryManHour.others === null) {
            this.manHour.summaryManHour.others = 0
          }
          if (this.manHour.summaryManHour.total === null) {
            this.manHour.summaryManHour.total = 0
          }

          if (this.manHour.salesSupportManHours.items === null) {
            this.manHour.salesSupportManHours.items = [
              {
                // @ts-ignore
                projectName: '',
                // @ts-ignore
                customerId: '',
                // @ts-ignore
                customerName: '',
                // @ts-ignore
                type: 'new',
                // @ts-ignore
                inputManHour: 0,
              },
            ]
          }

          // 工数が存在しない箇所
          // 数値がnullのものは0を入れないとupdateの時にエラーになる
          if (this.manHour.ssapManHours.meeting === null) {
            this.manHour.ssapManHours.meeting = 0
          }
          if (this.manHour.ssapManHours.study === null) {
            this.manHour.ssapManHours.study = 0
          }
          if (this.manHour.ssapManHours.learning === null) {
            this.manHour.ssapManHours.learning = 0
          }
          if (this.manHour.ssapManHours.newService === null) {
            this.manHour.ssapManHours.newService = 0
          }
          if (this.manHour.ssapManHours.startdash === null) {
            this.manHour.ssapManHours.startdash = 0
          }
          if (this.manHour.ssapManHours.improvement === null) {
            this.manHour.ssapManHours.improvement = 0
          }
          if (this.manHour.ssapManHours.ssap === null) {
            this.manHour.ssapManHours.ssap = 0
          }
          if (this.manHour.ssapManHours.qc === null) {
            this.manHour.ssapManHours.qc = 0
          }
          if (this.manHour.ssapManHours.accounting === null) {
            this.manHour.ssapManHours.accounting = 0
          }
          if (this.manHour.ssapManHours.management === null) {
            this.manHour.ssapManHours.management = 0
          }
          if (this.manHour.ssapManHours.officeWork === null) {
            this.manHour.ssapManHours.officeWork = 0
          }
          if (this.manHour.ssapManHours.others === null) {
            this.manHour.ssapManHours.others = 0
          }
          if (this.manHour.holidaysManHours.paidHoliday === null) {
            this.manHour.holidaysManHours.paidHoliday = 0
          }
          if (this.manHour.holidaysManHours.holiday === null) {
            this.manHour.holidaysManHours.holiday = 0
          }
          if (this.manHour.holidaysManHours.private === null) {
            this.manHour.holidaysManHours.private = 0
          }
          if (this.manHour.holidaysManHours.others === null) {
            this.manHour.holidaysManHours.others = 0
          }
          if (this.manHour.holidaysManHours.departmentOthers === null) {
            this.manHour.holidaysManHours.departmentOthers = 0
          }
          if (this.manHour.summaryManHour.direct === null) {
            this.manHour.summaryManHour.direct = 0
          }
          if (this.manHour.summaryManHour.pre === null) {
            this.manHour.summaryManHour.pre = 0
          }
          if (this.manHour.summaryManHour.sales === null) {
            this.manHour.summaryManHour.sales = 0
          }
          if (this.manHour.summaryManHour.ssap === null) {
            this.manHour.summaryManHour.ssap = 0
          }
          if (this.manHour.summaryManHour.others === null) {
            this.manHour.summaryManHour.others = 0
          }
          if (this.manHour.summaryManHour.total === null) {
            this.manHour.summaryManHour.total = 0
          }
        })
        .catch((error) => {
          throw error
        })
    },
    /**
     * GetManHourByMineAPIから支援工数データを取得する
     */
    refresh() {
      this.displayLoading([this.getManHourByMine()])
    },
    /**
     * サジェストする顧客情報を取得する
     */
    async SuggestCustomers() {
      await SuggestCustomers().then((res) => {
        this.suggestCustomersResponse = res.data
      })
    },
  },
  watch: {
    $route() {
      this.getManHourByMine()
    },
  },
})
</script>
