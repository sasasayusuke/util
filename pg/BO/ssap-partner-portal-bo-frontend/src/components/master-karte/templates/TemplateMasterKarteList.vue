<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader>
      {{ $t('karte.pages.index.name') }}
    </ListInPageHeader>
    <MasterKarteSort
      :search-params="searchParams"
      :detail-search-params="detailSearchParams"
      @update-search-params="updateSearchParams"
      @update-detail-search-params="updateDetailSearchParams"
      @handleSearch="handleSearch"
      @handleReset="handleReset"
    />
    <MasterKarteListPagination
      :max-page="totalPages(limit, totalItems)"
      :offset-page="offsetPage"
      :limit="limit"
      :total-items="totalItems"
      :page-text="true"
      @changePage="changePage"
    />
    <MasterKarteListTable
      :master-karte-list="masterKarteList"
      :is-loading="isLoading"
    />
    <div class="d-flex justify-end mt-3">
      <MasterKarteListPagination
        :max-page="totalPages(limit, totalItems)"
        :offset-page="offsetPage"
        @changePage="changePage"
      />
    </div>
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import MasterKarteSort from '~/components/master-karte/organisms/MasterKarteSort.vue'
import MasterKarteListTable from '~/components/master-karte/organisms/MasterKarteListTable.vue'
import MasterKarteListPagination from '~/components/master-karte/organisms/MasterKarteListPagination.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import {
  GetMasterKarten,
  MasterKarteListSearchParams,
  MasterKarteListDetailSearchParams,
} from '~/models/MasterKarte'
import { masterKarteListUrlStore } from '~/store'

export default BaseComponent.extend({
  name: 'TemplateMasterKarteList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    MasterKarteSort,
    MasterKarteListTable,
    MasterKarteListPagination,
  },
  data() {
    return {
      /** マスターカルテ一覧 */
      masterKarteList: [],
      /** 現在のページ */
      offsetPage: 1,
      /** 最大取得件数 */
      limit: 10,
      /** 全件数 */
      totalItems: 0,
      /** ローディングフラグ */
      isLoading: false,
      /** 案件関連の条件（変更用） */
      searchParams: new MasterKarteListSearchParams(),
      /** マスターカルテ関連の条件（変更用） */
      detailSearchParams: new MasterKarteListDetailSearchParams(),
    }
  },
  created() {
    this.getQueryParams()
    this.getMasterKarte(this.searchParams, this.detailSearchParams)
  },
  methods: {
    /** 案件関連の条件の更新 */
    updateSearchParams(newObject: MasterKarteListSearchParams) {
      this.searchParams = newObject
    },
    /** マスターカルテ関連の条件の更新 */
    updateDetailSearchParams(newObject: MasterKarteListDetailSearchParams) {
      this.detailSearchParams = newObject
    },
    /** パラメーターの受け渡し */
    getQueryParams() {
      const params = this.$route.query

      this.offsetPage = params.offsetPage ? Number(params.offsetPage) : 1
      this.searchParams = params.searchParams
        ? JSON.parse(params.searchParams as string)
        : new MasterKarteListSearchParams()
      this.detailSearchParams = params.detailSearchParams
        ? JSON.parse(params.detailSearchParams as string)
        : new MasterKarteListDetailSearchParams()
    },
    /** マスターカルテ一覧 呼び出し */
    async getMasterKarte(
      searchParams: MasterKarteListSearchParams,
      detailSearchParams: MasterKarteListDetailSearchParams
    ): Promise<void> {
      this.isLoading = true

      const params = {
        offsetPage: this.offsetPage,
        limit: this.limit,
        ...searchParams,
        ...detailSearchParams,
      }

      //「当期支援」か「次期支援」か参照
      //「当期支援」であれば以下の項目を無効化
      // 無効化項目「お客様に不足している人的リソース」「お客様に紹介したい企業や産業」「お客様の強み」
      // ------------------------------------
      //「次期支援」であれば以下の項目を無効化
      // 無効化項目「顧客セグメント」「業界セグメント」「部署名」「支援結果」
      if (!this.detailSearchParams.isCurrentProgram) {
        params.category = []
        params.industrySegment = []
        params.departmentName = ''
      } else {
        params.requiredPersonalSkill = ''
        params.requiredPartner = ''
        params.strength = ''
      }

      await GetMasterKarten(params).then((res) => {
        this.masterKarteList = res.data.karten
        this.totalItems = res.data.total
        this.offsetPage = res.data.offsetPage
        this.isLoading = false
      })
    },
    /** ページネーションのページ変更 */
    async changePage(page: number) {
      // クエリパラメータのoffsetPageを文字列からJSONに変換
      this.offsetPage = this.$route.query.offsetPage
        ? parseInt(JSON.parse(this.$route.query.offsetPage as string))
        : 1

      // offsetPageを更新
      this.offsetPage = page

      // パラメーターの受け渡し
      const params = {
        offsetPage: String(this.offsetPage),
        searchParams: JSON.stringify(this.searchParams),
        detailSearchParams: JSON.stringify(this.detailSearchParams),
      }

      // vuexにパラメーターを保存
      masterKarteListUrlStore.setParams(JSON.stringify(params))

      this.$router.push({
        path: this.$router.currentRoute.path,
        query: { ...params },
      })

      await this.getMasterKarte(this.searchParams, this.detailSearchParams)
    },
    /** 絞り込み検索 */
    async handleSearch() {
      this.offsetPage = 1

      // パラメーターの受け渡し
      const params = {
        offsetPage: String(this.offsetPage),
        searchParams: JSON.stringify(this.searchParams),
        detailSearchParams: JSON.stringify(this.detailSearchParams),
      }

      // vuexにパラメーターを保存
      masterKarteListUrlStore.setParams(JSON.stringify(params))

      this.$router.push({
        path: this.$router.currentRoute.path,
        query: { ...params },
      })

      await this.getMasterKarte(this.searchParams, this.detailSearchParams)
    },
    /** リセット */
    handleReset() {
      this.searchParams = new MasterKarteListSearchParams()
      this.detailSearchParams = new MasterKarteListDetailSearchParams()

      // パラメーターをリセット
      this.$router.push({ path: this.$router.currentRoute.path })

      // 再度初期状態の絞り込み内容で再検索
      this.getMasterKarte(this.searchParams, this.detailSearchParams)
    },
    /**
     * 全ページ数を取得
     * @param limit 最大取得件数
     * @param totalItems 全件数
     */
    totalPages(limit: number, totalItems: number): number {
      return Math.ceil(totalItems / limit)
    },
  },
})
</script>
