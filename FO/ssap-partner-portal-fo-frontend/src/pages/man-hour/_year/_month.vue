<template>
  <TemplateManHour :man-hour="manHour" @refresh="refresh" />
</template>

<script lang="ts">
import { getCurrentDate } from '~/utils/common-functions'
import BasePage from '~/common/BasePage'
import TemplateManHour from '~/components/man-hour/templates/ManHour.vue'
import {
  GetManHourByMine,
  GetManHourByMineRequest,
  GetManHourByMineResponse,
} from '~/models/ManHour'

export default BasePage.extend({
  name: 'MonHour',
  components: {
    TemplateManHour,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('man-hour.pages.list.name') as string,
    }
  },
  mounted() {
    this.displayLoading([this.getManHourByMine()])
  },
  data(): {
    manHour: GetManHourByMineResponse
  } {
    return {
      manHour: new GetManHourByMineResponse(),
    }
  },
  computed: {
    /**
     * クエリパラメーターから取得する年
     * @returns クエリパラメーターから取得する年
     */
    year() {
      return this.$route.params.year ? this.$route.params.year : ''
    },
    /**
     * クエリパラメーターから取得する月
     * @returns クエリパラメーターから取得する月
     */
    month() {
      return this.$route.params.month ? this.$route.params.month : ''
    },
  },
  methods: {
    /**
     * GetManHourByMineAPIから支援工数データを取得する
     */
    async getManHourByMine() {
      // APIからの取得処理
      const request = new GetManHourByMineRequest()
      const now = getCurrentDate()
      const queryYear = this.$route.query.year
      const queryMonth = this.$route.query.month
      if (queryYear && queryMonth) {
        // @ts-ignore
        request.year = parseInt(queryYear)
        // @ts-ignore
        request.month = parseInt(queryMonth)
      } else {
        //今月を入れる
        request.year = now.getFullYear()
        request.month = parseInt((now.getMonth() + 1).toString())
        this.$route.query.year = request.year.toString()
        this.$route.query.month = request.month.toString()
      }

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

          // role名を日本語に差し替え
          this.manHour.directSupportManHours.items.forEach((item: object) => {
            // @ts-ignore
            item.role = this.rolesToString(item.role)
          })
          this.manHour.preSupportManHours.items.forEach((item: object) => {
            // @ts-ignore
            item.role = this.rolesToString(item.role)
          })
        })
        .catch((error) => {
          throw error
        })
    },
    /**
     * 役職名を日本語に変えるだけのメソッド
     * @param 英語役職名
     * @returns 日本語役職名
     */
    rolesToString(roleName: string) {
      const rolesLabels = this.$t('man-hour.tables.role.labels')
      const rolesList = this.$t('man-hour.tables.role.values')
      for (let i = 0; i < rolesList.length; i++) {
        // @ts-ignore
        if (rolesList[i] === roleName) {
          // @ts-ignore
          return rolesLabels[i]
        }
      }
    },
    /**
     * 支援工数データを新しく更新する
     */
    refresh() {
      this.displayLoading([this.getManHourByMine()])
    },
  },
  watch: {
    $route() {
      this.getManHourByMine()
    },
  },
})
</script>
