<template>
  <RootTemPlate>
    <ListInPageHeader>{{ pageName }}</ListInPageHeader>
    <ProjectSort
      v-if="showSort"
      :param="searchParam"
      :suggest-customers="suggestCustomers"
      :is-loading="isLoading.suggestCustomers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <ProjectListTable
      :projects="response.projects"
      :total="total"
      :offset-page="offsetPage"
      :link-to-project-detail="linkToProjectDetail"
      :is-loading="isLoading.projects"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import ProjectSort, {
  ProjectSearchParam,
} from '~/components/project/organisms/ProjectSort.vue'
import ProjectListTable from '~/components/project/organisms/ProjectListTable.vue'

import { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import {
  GetProjectsDateNoLimitRequest,
  GetProjectsResponse,
} from '~/models/Project'
import CommonList from '~/components/common/templates/CommonList'
import { meStore } from '~/store'

export interface isLoading {
  projects: boolean
  suggestCustomers: boolean
}

export default CommonList.extend({
  name: 'ProjectListTemplate',
  components: {
    RootTemPlate,
    ListInPageHeader,
    ProjectSort,
    ProjectListTable,
  },
  props: {
    /** 案件一覧 */
    response: {
      type: Object as PropType<GetProjectsResponse>,
      required: true,
    },
    /** 取引先のサジェスト用情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  created() {
    this.lastSearchRequest = new this.RequestType()
    this.searchParam = new this.SearchParamType()

    //顧客の場合、絞り込み・案件詳細へのリンクを非表示・案件名の昇順
    if (meStore.role === 'customer') {
      this.showSort = false
      this.linkToProjectDetail = false
      this.searchParam.customerId = meStore.customerId
    }
  },
  data() {
    return {
      SearchParamType: ProjectSearchParam,
      RequestType: GetProjectsDateNoLimitRequest,
      apiName: 'getProjects',
      headerPageName: this.$t('project.group_info.name'),
      pageName: this.$t('project.pages.index.name'),
      showSort: true,
      linkToProjectDetail: true,
    }
  },
  methods: {
    /** 絞り込み */
    search(): void {
      const request = new this.RequestType()

      function dateStrToNum(date: string): number {
        const strDate = date.split('/').join('')
        return parseInt(strDate)
      }
      const projectSearchParam: { [key: string]: number | string } = {}
      //期間指定（From）
      if (this.searchParam.dateFrom) {
        const fromDate = dateStrToNum(this.searchParam.dateFrom)
        projectSearchParam.fromDate = fromDate
      }
      //期間指定（To）
      if (this.searchParam.dateTo) {
        const toDate = dateStrToNum(this.searchParam.dateTo)
        projectSearchParam.toDate = toDate
      }
      //お客様ID
      if (this.searchParam.customerId) {
        projectSearchParam.customerId = this.searchParam.customerId
      }
      //案件タイプ
      if (this.searchParam.allAssigned) {
        projectSearchParam.allAssigned = this.searchParam.allAssigned
      }

      Object.assign(request, projectSearchParam)
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
  },
})
</script>
