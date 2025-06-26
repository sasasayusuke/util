<template>
  <CommonWideClickableCard
    :name="`${karte.projectName}`"
    :content="[
      karte.customerName,
      `${karte.date}（${karte.day}）${karte.startTime}〜${karte.endTime}`,
    ]"
    :last="`${$t('top.pages.home.karten.lastUpdate')}： ${lastUpdateInfo}`"
    :to="forwardToUrl(`/karte/${karte.karteId}`)"
  />
</template>

<script lang="ts">
import { format } from 'date-fns'
import CommonWideClickableCard from '~/components/common/molecules/CommonWideClickableCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { KartenLatestListItem } from '~/models/Karte'

export default BaseComponent.extend({
  components: {
    CommonWideClickableCard,
  },
  props: {
    /**
     * 最近更新されたカルテの情報
     */
    karte: {
      type: Object as PropType<KartenLatestListItem>,
    },
  },
  computed: {
    /**
     * JST時間から文字列表記yyyy/MM//ddに変換する
     * 表示のために全角スペースを関数内で追加する
     * @returns カルテ最終更新日時
     */
    lastUpdateInfo() {
      const lastUpdate = this.karte.lastUpdateDatetime
        ? format(
            new Date(this.karte.lastUpdateDatetime),
            this.$t('common.format.date_ymd4') as string
          )
        : this.karte.lastUpdateDatetime

      const draftSupporter = this.karte.draftSupporterName

      if (!lastUpdate || !draftSupporter) {
        return 'ー'
      }

      // 両方存在する場合は結合して返す（全角スペースを含む）
      return lastUpdate + '　' + draftSupporter
    },
  },
})
</script>
