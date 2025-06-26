<template>
  <CommonInPageHeader class="o-InPageHeader">
    <slot name="default" />
    <template #button>
      <Button
        outlined
        style-set="large-tertiary"
        class="a-link-month-btn mr-8"
        :to="currentLinkRoute"
      >
        {{ $t('common.button.currentMonth') }}
      </Button>
      <Button style-set="large-primary" :to="prevLinkRoute" class="mr-2">
        <v-layout justify-space-between>
          <Icon class="mr-2" size="14">icon-org-arrow-left</Icon>
          {{ $t('common.button.lastMonth') }}
        </v-layout>
      </Button>
      <Button
        v-if="nextLinkRoute"
        :disabled="nextLinkAppear"
        style-set="large-primary"
        :to="nextLinkRoute"
      >
        <v-layout justify-space-between>
          {{ $t('common.button.nextMonth') }}
          <Icon class="ml-2" size="14">icon-org-arrow-right</Icon>
        </v-layout>
      </Button>
    </template>
  </CommonInPageHeader>
</template>

<script lang="ts">
import { getCurrentDate } from '~/utils/common-functions'
import { Button, Icon } from '~/components/common/atoms/index'
import CommonInPageHeader from '~/components/common/organisms/InPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    CommonInPageHeader,
  },
  props: {},
  methods: {
    /**
     * 日付からクエリ付きルートを作る
     * @param {Date} 日付
     * @returns {Object} ルート
     */
    generateManHourRoute(date: Date): Object {
      return {
        path: '/man-hour',
        query: {
          year: date.getFullYear(),
          month: date.getMonth() + 1,
        },
      }
    },
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
    /**
     * 今月分のデータページへのリンク
     * @returns リンク情報
     */
    currentLinkRoute(): Object {
      const now = getCurrentDate()
      return this.generateManHourRoute(now)
    },
    /**
     * 前月分のデータページへのリンク
     * @returns リンク情報
     */
    prevLinkRoute(): Object {
      const lastMonth = new Date(
        this.queryDate.getFullYear(),
        this.queryDate.getMonth() - 1
      )

      return this.generateManHourRoute(lastMonth)
    },
    /**
     * 次月分のデータページへのリンク
     * @returns リンク情報
     */
    nextLinkRoute(): Object {
      const nextMonth = new Date(
        this.queryDate.getFullYear(),
        this.queryDate.getMonth() + 1
      )
      return this.generateManHourRoute(nextMonth)
    },
    /**
     * 翌月分のデータがあるか否か
     * @returns 翌月分のデータがあるか否か
     */
    nextLinkAppear(): boolean {
      const now = getCurrentDate()
      const queryDate = this.queryDate

      if (now.getFullYear() !== queryDate.getFullYear()) {
        return false
      }

      if (now.getMonth() !== queryDate.getMonth()) {
        return false
      }

      // 年月が両方一致する場合は今月なので"翌月"ボタンを不活性に
      return true
    },
  },
})
</script>

<style lang="scss" scoped>
.a-chip.v-size--large {
  height: 44px;
  color: $c-black-60;
}
.a-link-month-btn {
  width: 96px;
}
.o-InPageHeader {
  .theme--light.v-btn.v-btn--disabled .v-icon {
    color: #fff !important;
  }
}
</style>
