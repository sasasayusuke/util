<template>
  <RootTemPlate>
    <!-- ヘッダ -->
    <ListInPageHeader
      csv-output
      :latest-date="setLatestDate"
      note-head="required"
      :csv-button-disabled="parseInt(length) <= 0"
      @buttonAction0="buttonAction0"
      >{{ pageName }}</ListInPageHeader
    >
    <!-- ソート -->
    <ManHourSort
      :param="param"
      :supporter-organizations="supporterOrganizations"
      :service-types="serviceTypes"
      :is-loading="isLoading.supporterOrganizations && isLoading.serviceTypes"
      @update="update"
      @sort="sort"
      @clear="clear"
      @click:prev="$emit('click:prev')"
      @click:next="$emit('click:next')"
    />
    <!-- 一覧リスト -->
    <ManHourListTable
      :summary-project-man-hour-alerts="response"
      :is-loading="isLoading.summaryProjectManHourAlerts"
      :total="length"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import ManHourSort, {
  SearchParam,
} from '~/components/man-hour/organisms/ManHourSort.vue'
import ManHourListTable from '~/components/man-hour/organisms/ManHourListTable.vue'
import CommonList from '~/components/common/templates/CommonList'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetBatchControlById } from '~/models/Master'
import type { PropType } from '~/common/BaseComponent'

export { SearchParam }

export interface isLoading {
  supporterOrganizations: boolean
  serviceTypes: boolean
  summaryProjectManHourAlerts: boolean
}
export default CommonList.extend({
  name: 'ManHourList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    ManHourSort,
    ManHourListTable,
  },
  props: {
    /** 検索パラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** 案件毎工数状況とアラート一覧 */
    response: {
      type: Object,
      required: true,
    },
    /** 支援者組織一覧情報 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** サービス種別一覧情報 */
    serviceTypes: {
      type: Array,
      required: true,
    },
    /** 一覧件数 */
    length: {
      type: Number,
      required: true,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      pageName: this.$t('man-hour.pages.index.name'),
      batchControlId: encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      ),
      date: '',
      searchParam: new SearchParam(),
    }
  },
  computed: {
    /**
     * 最終集計日時をセット
     * @param {string} formattedDate GetBatchControlByIdAPIから得られた処理終了日時
     */
    setLatestDate() {
      this.getDate()
      const formattedDate = this.formatDate(new Date(this.date)) as string
      return formattedDate
    },
  },

  methods: {
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
    sort() {
      this.$emit('sort')
    },
    clear() {
      this.$emit('clear')
    },
    /** CSV出力 */
    buttonAction0() {
      this.$emit('exportCsv')
    },
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
  },
  created() {
    this.getDate()
  },
})
</script>
