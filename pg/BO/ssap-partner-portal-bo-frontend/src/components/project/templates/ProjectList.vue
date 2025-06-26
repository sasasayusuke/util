<template>
  <RootTemPlate>
    <ListInPageHeader note-head="required">{{ pageName }}</ListInPageHeader>
    <ProjectSort
      :param="searchParam"
      :suggest-customers="suggestCustomers"
      :suggest-sales-users="suggestSalesUsers"
      :service-types="serviceTypes"
      :supporter-organizations="supporterOrganizations"
      :is-loading="isLoading"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <ProjectListTable
      :projects="response.projects"
      :total="total"
      :offset-page="offsetPage"
      :limit="limit"
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

import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetProjectsRequest, GetProjectsResponse } from '~/models/Project'
import type { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

export interface isLoading {
  projects: boolean
  suggestCustomers: boolean
  suggestSalesUsers: boolean
  serviceTypes: boolean
  supporterOrganizations: boolean
}

export default CommonList.extend({
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
    /** 取引先のサジェスト用情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** サービス種別 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: ProjectSearchParam,
      RequestType: GetProjectsRequest,
      apiName: 'getProjects',
      headerPageName: this.$t('project.group_info.name'),
      pageName: this.$t('project.pages.index.name'),
      buttons: [
        { name: this.$t('project.pages.index.name'), link: '/project/list' },
        {
          name: this.$t('project.pages.create.name'),
          link: '/project/create',
        },
      ],
    }
  },
  methods: {
    /** 絞り込み */
    search(): void {
      const request = new this.RequestType()

      function dateStrToNum(date: string): number {
        const strDate = date.replace('/', '')
        return parseInt(strDate)
      }

      const projectSearchParam: { [key: string]: number | string } = {}

      if (this.searchParam.status) {
        projectSearchParam.status = this.searchParam.status
      }
      if (this.searchParam.fromYearMonth) {
        const fromYearMonth = dateStrToNum(this.searchParam.fromYearMonth)
        projectSearchParam.fromYearMonth = fromYearMonth
      }
      if (this.searchParam.toYearMonth) {
        const toYearMonth = dateStrToNum(this.searchParam.toYearMonth)
        projectSearchParam.toYearMonth = toYearMonth
      }
      if (this.searchParam.customerId) {
        projectSearchParam.customerId = this.searchParam.customerId
      }
      if (this.searchParam.mainSalesUserId) {
        projectSearchParam.mainSalesUserId = this.searchParam.mainSalesUserId
      }
      if (this.searchParam.supporterOrganizationId) {
        projectSearchParam.supporterOrganizationId =
          this.searchParam.supporterOrganizationId
      }
      if (this.searchParam.serviceTypeId) {
        projectSearchParam.serviceTypeId = this.searchParam.serviceTypeId
      }
      Object.assign(request, projectSearchParam)

      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
  },
})
</script>
