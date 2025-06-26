<template>
  <v-container fluid pa-0 mx-0>
    <v-row no-gutters :class="{ 'align-right': !pageText }">
      <!-- テキスト -->
      <v-col v-if="pageText" class="page-text">
        <p class="font-size-small">
          {{ paginationText(offsetPage, limit, totalItems) }}
        </p>
      </v-col>
      <!-- ページネーション -->
      <v-col cols="auto">
        <CommonNumberingPagination
          :page="offsetPage"
          :length="maxPage"
          :total-visible="7"
          class-name="master-karte-numbering-pagination-block"
          @input="changePage"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import CommonNumberingPagination from '~/components/common/molecules/NumberingPagination.vue'

export default BaseComponent.extend({
  name: 'MasterKarteListPagination',
  components: {
    CommonNumberingPagination,
  },
  props: {
    /** 現在のペーシ */
    offsetPage: {
      type: Number,
    },
    /** 最大ページ数 */
    maxPage: {
      type: Number,
    },
    /** 最大取得件数 */
    limit: {
      type: Number,
    },
    /** 全件数 */
    totalItems: {
      type: Number,
    },
    /** ページネーションのテキスト */
    pageText: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    /**
     * ページネーションのテキストを取得
     * @param offsetPage 現在のページ番号
     * @param limit 最大取得件数
     * @param totalItems 全件数
     */
    paginationText(offsetPage: number, limit: number, totalItems: number) {
      let start = (offsetPage - 1) * limit + 1
      const end =
        offsetPage * limit > totalItems ? totalItems : offsetPage * limit

      // totalItemsが0の場合、startも0に設定
      if (totalItems === 0) start = 0

      return `${start}〜${end} ${this.$t(
        'master-karte.pages.list.table.headerText[0]'
      )}${totalItems}${this.$t('master-karte.pages.list.table.headerText[1]')}`
    },
    /**
     * ページ変更時の処理
     * @param page
     */
    changePage(page: number) {
      this.$emit('changePage', page)
    },
  },
})
</script>

<style>
.master-karte-numbering-pagination-block {
  width: 407px !important;
}
</style>

<style lang="scss" scoped>
.align-right {
  display: flex;
  justify-content: flex-end;
}

.page-text {
  display: flex;
  align-items: center;

  p {
    line-height: 1;
    margin-bottom: 0;
  }
}
</style>
