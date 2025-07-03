<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ title }}
        </h2>
      </v-flex>
    </v-container>
    <v-container v-if="filteredKarten.length" class="pa-0">
      <Card elevation="3" class="m-HomeKarten__card">
        <SimpleTable>
          <HomeKartenCard
            v-for="(karte, n) in filteredKarten"
            :key="n"
            :karte="karte"
          />
        </SimpleTable>
      </Card>
    </v-container>
    <Alert v-else style-set="no_data">
      {{ $t('common.alert.no_data') }}
    </Alert>
  </section>
</template>

<script lang="ts">
import HomeKartenCard from '../molecules/HomeKartenCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { KartenLatestListItem } from '~/models/Karte'
import { meStore } from '~/store'
import { Alert, Card, SimpleTable } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    HomeKartenCard,
    Alert,
    Card,
    SimpleTable,
  },
  data(): {
    localKarten: KartenLatestListItem[]
  } {
    return {
      localKarten: this.karten,
    }
  },
  computed: {
    /**
     * 最大表示件数の切り替え
     * - 顧客: 最大5件
     * - 支援者: 最大10件
     */
    filteredKarten(this: any) {
      const maxCount = meStore.role === 'customer' ? 5 : 10
      return this.localKarten.slice(0, maxCount)
    },
    /**
     * タイトル
     * 顧客：最近更新されたカルテ
     * 支援者：直近のカルテ
     */
    title() {
      return meStore.role === 'customer'
        ? this.$t('top.pages.home.header.karten')
        : this.$t('top.pages.home.header.kartenRecently')
    },
  },
  props: {
    /**
     *カルテ情報
     */
    karten: {
      type: Array as PropType<KartenLatestListItem[]>,
    },
  },
})
</script>

<style lang="scss">
.m-HomeKarten__card {
  tbody {
    &:nth-child(n + 2) {
      th,
      td {
        border-top: 1px solid $c-gray-line;
      }
    }
  }
}
</style>
