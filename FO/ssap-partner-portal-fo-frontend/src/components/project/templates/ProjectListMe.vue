<template>
  <RootTemPlate>
    <DetailInPageHeader>
      {{ $t('project.pages.me.name') }}
    </DetailInPageHeader>
    <ProjectListMeSort
      :param="searchParam"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <ProjectListMeTable
      :projects="response.projects"
      :total="total"
      :offset-page="offsetPage"
      :limit="limit"
      :is-loading="isLoading.projects"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ProjectListMeSort, {
  ProjectMeSearchParam,
} from '../organisms/ProjectListMeSort.vue'
import ProjectListMeTable from '../../project/organisms/ProjectListMeTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import CommonList from '~/components/common/templates/CommonList'
import {
  GetProjectsYearMonthAllAssignedRequest,
  GetProjectsResponse,
} from '~/models/Project'
import { PropType } from '~/common/BaseComponent'
import { yearMonthToNum } from '~/utils/common-functions'

export { ProjectMeSearchParam }
export class isLoading {
  projects: boolean = true
}

/** 期間指定等を修正し、リクエストを作成 */
export function paramToRequest(searchParam: ProjectMeSearchParam) {
  const request = new GetProjectsYearMonthAllAssignedRequest()
  //期間指定（From）
  if (searchParam.fromYearMonth) {
    const fromYearMonth = yearMonthToNum(searchParam.fromYearMonth)
    request.fromYearMonth = fromYearMonth
  }
  //期間指定（To）
  if (searchParam.toYearMonth) {
    const toYearMonth = yearMonthToNum(searchParam.toYearMonth)
    request.toYearMonth = toYearMonth
  }
  //案件タイプ
  if (searchParam.allAssigned) {
    request.allAssigned = searchParam.allAssigned
  }

  return request
}

export default CommonList.extend({
  name: 'ProjectListMe',
  components: {
    RootTemPlate,
    DetailInPageHeader,
    ProjectListMeSort,
    ProjectListMeTable,
  },
  props: {
    /** 案件一覧 */
    response: {
      type: Object as PropType<GetProjectsResponse>,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    role: {
      type: String,
      default: 'supporter',
    },
  },
  data() {
    return {
      /** 利用されていない */
      SearchParamType: ProjectMeSearchParam,
      /** 利用されていない */
      RequestType: GetProjectsYearMonthAllAssignedRequest,
      /** 親コンポーネントで実行するAPI名 */
      apiName: 'getProjects',
      /** 利用されていない */
      headerPageName: this.$t('top.pages.project_list_me.name'),
      /** 画面タイトル */
      pageName: this.$t('project.pages.index.name'),
      /** 利用されていない */
      showSort: true,
      /** 「絞り込み」パラメータ */
      searchParam: new ProjectMeSearchParam(),
      /** getProjects APIリクエスト */
      lastSearchRequest: new GetProjectsYearMonthAllAssignedRequest(),
    }
  },
  methods: {
    /** 「絞り込み」パラメータを更新 */
    updateParam(keyName: string, newVal: any): void {
      this.searchParam[keyName] = newVal
    },
    /** 「絞り込み」パラメータをクリア */
    clear(): void {
      this.searchParam = new ProjectMeSearchParam() as any
      this.search()
    },
    /** 「絞り込み」パラメータを利用して、リクエストを作成 */
    search(): void {
      const request = paramToRequest(this.searchParam)
      this.lastSearchRequest = request

      this.$emit(this.apiName, request)
    },
  },
})
</script>
