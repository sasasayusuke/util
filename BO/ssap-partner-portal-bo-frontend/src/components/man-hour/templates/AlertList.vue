<template>
  <RootTemplate pt-8 pb-10 mb-10>
    <!-- タイトル上に数字が表示されるようになっていたのでコメントアウトしました -->
    <!-- {{ totalContractTime }} -->
    <!-- ヘッダー -->
    <ListInPageHeader
      :date="setLastUpdate"
      :csv-button-disabled="parseInt(total) <= 0"
      @buttonAction="buttonAction"
    >
      {{ pageName }}
    </ListInPageHeader>
    <!-- ソート -->
    <AlertSort
      :param="param"
      :service-types="serviceTypes"
      :supporter-organizations="supporterOrganizations"
      @update="update"
      @sort="sort"
      @clear="clear"
      @click:prev="$emit('click:prev')"
      @click:next="$emit('click:next')"
    />
    <!-- 一覧テーブル -->
    <AlertListTable
      :year="year"
      :month="month"
      :offset-page="offsetPage"
      :max-page="maxPage"
      :total="total"
      :total-contract-time="totalContractTime"
      :summary-project-man-hour-alerts="summaryProjectManHourAlerts"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import ListInPageHeader from '~/components/man-hour/organisms/ListInPageHeader.vue'
import AlertSort, {
  SearchParam,
} from '~/components/man-hour/organisms/AlertSort.vue'
import AlertListTable from '~/components/man-hour/organisms/AlertListTable.vue'
import { GetBatchControlById } from '~/models/Master'

export { SearchParam }

export default BaseComponent.extend({
  name: 'AlertList',
  components: {
    RootTemplate,
    ListInPageHeader,
    AlertSort,
    AlertListTable,
  },
  props: {
    /** 検索パラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** 現在の年 || 選択された年 */
    year: {
      type: Number,
    },
    /** 現在の月 || 選択された月 */
    month: {
      type: Number,
    },
    /** オフセットページ */
    offsetPage: {
      type: Number,
    },
    /** 最大ページ */
    maxPage: {
      type: Number,
    },
    /** 合計数 */
    total: {
      type: Number,
    },
    /** 延べ契約時間の合計 */
    totalContractTime: {
      type: Number,
    },
    /** 案件毎の工数状況とアラート一覧情報 */
    summaryProjectManHourAlerts: {
      type: Object,
    },
    /** サービス種別の一覧情報 */
    serviceTypes: {
      type: Array,
      default: [],
    },
    /** 支援者組織一覧情報 */
    supporterOrganizations: {
      type: Array,
      default: [],
    },
  },
  data() {
    return {
      pageName: this.$t('man-hour.pages.alert.list.name').toString(),
      batchControlId: encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      ),
      date: '',
      searchParam: new SearchParam(),
    }
  },
  created() {
    this.getDate()
  },
  computed: {
    /**
     * 最終集計日時をセット
     * @param {string} lastUpdate GetBatchControlByIdAPIから得られた処理終了日時
     */
    setLastUpdate() {
      this.getDate()
      const lastUpdate = this.formatDate(
        new Date(this.date),
        'Y/MM/DD hh:mm'
      ) as string
      return lastUpdate
    },
  },
  methods: {
    /**
     * GetBatchControlByIdAPIを叩いて、各種集計バッチ処理の最終完了日時を取得
     * @param {string} batchControlId バッチ処理関数ID
     */
    getDate() {
      GetBatchControlById(this.batchControlId)
        .then((res) => {
          this.date = res.data.batchEndAt
        })
        .catch(() => {
          this.$logger.info('cannot get batchEndAt')
          this.date = '-'
        })
    },
    buttonAction() {
      // csv出力処理
      this.$emit('exportCsv')
    },
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
    sort() {
      this.$emit('sort')
    },
    clear() {
      this.$emit('clear')
    },
  },
})
</script>
