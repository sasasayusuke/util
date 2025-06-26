<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader>
      {{ pageName }}
    </ListInPageHeader>
    <KarteSort
      :param="searchParam"
      :suggest-customers="suggestCustomers"
      :is-loading="isLoading.suggestCustomers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <KarteListTable
      :projects="response.projects"
      :total="response.total"
      :offset-page="response.offsetPage"
      :max-page="Math.ceil(response.total / limit)"
      :limit="limit"
      :is-loading="isLoading.projects"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import { format } from 'date-fns'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import KarteSort, {
  ProjectSearchParam,
} from '~/components/karte/organisms/KarteSort.vue'
import { GetProjectsRequest, GetProjectsResponse } from '~/models/Project'
import KarteListTable from '~/components/karte/organisms/KarteListTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'

export interface isLoading {
  projects: boolean
  suggestCustomers: boolean
}

export default BaseComponent.extend({
  name: 'KarteIndex',
  components: {
    RootTemPlate,
    ListInPageHeader,
    KarteSort,
    KarteListTable,
  },
  props: {
    /**
     * GetProjects APIのレスポンス
     */
    response: {
      type: Object as PropType<GetProjectsResponse>,
      required: true,
    },
    /**
     * ページがリロード中か否か
     */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /**
     * 提案される顧客一覧
     */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /**
     * items-per-pageで利用される、ページ毎のアイテム数
     */
    limit: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: ProjectSearchParam,
      RequestType: GetProjectsRequest,
      apiName: 'getProjects',
      headerPageName: this.$t('karte.group_info.name'),
      pageName: this.$t('karte.pages.index.name'),
      searchParam: new ProjectSearchParam(),
      lastSearchRequest: new ProjectSearchParam(),
    }
  },
  methods: {
    /**
     * 再検索時のパラメータを更新
     * @param keyName 更新するパラメーターのキー
     * @param newVal 更新する値
     */
    updateParam(keyName: string, newVal: any) {
      // @ts-ignore
      this.searchParam[keyName] = newVal
    },
    /**
     * searchParamsの値を利用し、検索を行う
     */
    search() {
      const strFromYearMonth = this.searchParam.fromYearMonth
      const strToYearMonth = this.searchParam.toYearMonth
      let intFromYearMonth = null
      let intToYearMonth = null
      if (strFromYearMonth != null && !Number.isInteger(strFromYearMonth)) {
        intFromYearMonth = parseInt(
          format(new Date(strFromYearMonth), 'yyyyMM')
        )
      }
      if (strToYearMonth != null && !Number.isInteger(strToYearMonth)) {
        intToYearMonth = parseInt(format(new Date(strToYearMonth), 'yyyyMM'))
      }
      if (
        intFromYearMonth != null &&
        intToYearMonth != null &&
        intFromYearMonth > intToYearMonth
      ) {
        // @ts-ignore
        this.searchParam.fromYearMonth = strToYearMonth
        // @ts-ignore
        this.searchParam.toYearMonth = strFromYearMonth
      }
      const request = new this.RequestType()
      Object.assign(request, this.searchParam)
      // @ts-ignore
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    // 検索結果をソート
    sort(options: any) {
      const request = this.lastSearchRequest

      const sortKey: string = options.sortBy[0]
      const sortType: string = options.sortDesc[0] ? 'desc' : 'asc'
      if (sortKey) {
        // @ts-ignore
        request.sort = `${sortKey}:${sortType}`
      }

      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    /**
     * 検索用パラメータを全てクリア
     */
    clear() {
      this.searchParam = new this.SearchParamType()
      this.search()
    },
    /**
     * 次のページにページネーション
     */
    nextPage() {
      const request = this.lastSearchRequest
      request.offsetPage++
      this.$emit(this.apiName, request)
    },
    /**
     * 前のページにページネーション
     */
    prevPage() {
      const request = this.lastSearchRequest
      request.offsetPage--
      this.$emit(this.apiName, request)
    },
  },
})
</script>
