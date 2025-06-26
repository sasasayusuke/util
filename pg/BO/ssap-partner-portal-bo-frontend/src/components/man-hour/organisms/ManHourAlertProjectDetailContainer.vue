<template>
  <DetailContainer
    :title="title"
    :is-editing="false"
    hx="h1"
    is-hide-button1
    @click:negative="$emit('click:negative')"
  >
    <!-- 最終集計日時 -->
    <template #date>
      {{ setLastUpdate }}
    </template>
    <div class="pt-6 px-6">
      <Title style-set="detail2">{{
        summaryProjectManHourAlerts.projectName
      }}</Title>
      <!-- 案件別工数詳細テーブル -->
      <ManHourSimpleTable
        :man-hour-data="summaryProjectManHourAlerts"
        :type="'alertProjectDetail'"
      ></ManHourSimpleTable>
    </div>
  </DetailContainer>
</template>

<script lang="ts">
import { format, parseISO } from 'date-fns'
import { Title } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import ManHourSimpleTable from '~/components/man-hour/molecules/ManHourSimpleTable.vue'
import { GetBatchControlById } from '~/models/Master'

export default BaseComponent.extend({
  name: 'ManHourAlertProjectDetailContainer',
  components: {
    Title,
    DetailContainer,
    ManHourSimpleTable,
  },
  computed: {
    /**
     * 最終集計日時をセット
     * @param {string} dateString GetBatchControlByIdAPIから得られた処理終了日時
     */
    setLastUpdate(): string {
      this.getDate()
      let dateString = 'ー'
      // JST時刻を変換する
      if (this.date) {
        dateString = format(
          parseISO(this.date),
          this.$t('common.format.date_ymd_hm').toString()
        )
      }
      return this.$t('man-hour.pages.alertDetail.lastUpdate', {
        date: dateString,
      }) as string
    },
  },
  props: {
    /** ページタイトル */
    title: {
      type: String,
      required: true,
    },
    /** 案件指定の工数状況とアラート */
    summaryProjectManHourAlerts: {
      type: Object,
      required: true,
    },
    /** 工数アラート設定 */
    alertSettings: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      date: '',
      batchControlId: encodeURIComponent(
        `partnerportal-backoffice-${process.env.LANDSCAPE}-batch_summary_man_hour#project`
      ),
    }
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
        })
    },
  },
})
</script>

<style lang="scss" scoped></style>
