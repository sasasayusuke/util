<template>
  <RootTemPlate>
    <!-- ヘッダ -->
    <ListInPageHeader
      month-change
      @buttonAction1="buttonAction1"
      @buttonAction2="buttonAction2"
      @buttonAction3="buttonAction3"
    >
      {{ $t('man-hour.pages.project.name') }}&nbsp;&nbsp; {{ `${year()}`
      }}{{ $t('man-hour.unit.year') }}{{ `${month()}`
      }}{{ $t('man-hour.unit.month') }}
    </ListInPageHeader>
    <!-- 一覧リスト -->
    <MonthlyProjectsListTable
      :is-loading="isLoading.monthlyProjects"
      :monthly-projects="response.details"
      :header="response.header"
      class="mt-6"
      :last-count-date="setLastCountDate"
      @csvOutput="csvOutput"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import { getCurrentDate } from '~/utils/common-functions'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import MonthlyProjectsListTable from '~/components/man-hour/organisms/MonthlyProjectsListTable.vue'
import CommonList from '~/components/common/templates/CommonList'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetBatchControlById } from '~/models/Master'
import {
  GetMonthlyProjectsResponse,
  GetMonthlyProjectsRequest,
} from '@/models/Project'

export interface isLoading {
  monthlyProjects: boolean
}

export default CommonList.extend({
  name: 'ManHourMonthlyProjectsList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    MonthlyProjectsListTable,
  },
  props: {
    /** 読み込み中判定 */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 月次案件情報 */
    response: {
      type: Object as PropType<GetMonthlyProjectsResponse>,
      required: true,
    },
  },
  data() {
    return {
      RequestType: GetMonthlyProjectsRequest,
      apiName: 'getMonthlyProjects',
      pageName: this.$t('man-hour.pages.report.project.name'),
      date: '',
      batchControlId: encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      ),
    }
  },
  created() {
    this.getLastCountDate()
  },
  computed: {
    setLastCountDate() {
      this.getLastCountDate()
      const formattedDate = this.formatDate(new Date(this.date)) as string
      return formattedDate
    },
  },
  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year() {
      return this.$route.params.year ? this.$route.params.year : ''
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month() {
      if (this.$route.params.month !== '10') {
        return this.$route.params.month.replace('0', '')
      }
      return this.$route.params.month
    },
    /**
     * 今月ボタンが押下された際、今月に遷移
     * @param {object} now 現在の年月日時(JST)
     * @param {number} currentYear 現在の年
     * @param {number} currentMonth 現在の月
     * @return 今月に遷移
     */
    buttonAction1() {
      const now = getCurrentDate()
      const currentYear = now.getFullYear()
      const currentMonth = now.getMonth() + 1
      return this.$router.push(
        '/man-hour/report/' + currentYear + '/' + currentMonth + '/project'
      )
    },
    /**
     * 前月ボタンが押下された際、前月に遷移
     * @param {number} prevYear 現在表示されている年
     * @param {number} prevYear 現在表示されている月->現在表示されている前月
     * @return 前月に遷移
     */
    buttonAction2() {
      let prevYear = +this.$route.params.year
      let prevMonth = +this.$route.params.month
      if (prevMonth === 1) {
        prevMonth = 12
        prevYear = prevYear - 1
      } else {
        prevMonth = prevMonth - 1
      }
      return this.$router.push(
        '/man-hour/report/' + prevYear + '/' + prevMonth + '/project'
      )
    },
    /**
     * 翌月ボタンが押下された際、翌月に遷移
     * @param {number} nextYear 現在表示されている年
     * @param {number} nextMonth 現在表示されている月->現在表示されている翌月
     * @return 翌月に遷移
     */
    buttonAction3() {
      let nextYear = +this.$route.params.year
      let nextMonth = +this.$route.params.month
      if (nextMonth === 12) {
        nextMonth = 1
        nextYear = nextYear + 1
      } else {
        nextMonth = nextMonth + 1
      }
      return this.$router.push(
        '/man-hour/report/' + nextYear + '/' + nextMonth + '/project'
      )
    },
    /** CSV出力 */
    csvOutput() {
      this.$emit('csvOutput')
    },
    /**
     * @param {string} batchControlId バッチ処理関数ID
     */
    getLastCountDate() {
      GetBatchControlById(this.batchControlId)
        .then((res) => {
          this.date = res.data.batchEndAt
        })
        .catch(() => {
          this.$logger.info('cannot get batchEndAt')
          this.date = 'ー'
        })
    },
  },
})
</script>
