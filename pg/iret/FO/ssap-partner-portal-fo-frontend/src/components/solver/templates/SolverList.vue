<template>
  <RootTemPlate>
    <ListInPageHeader
      :is-solver-list="true"
      :for-solver-list="true"
      :is-loading="isLoading.solvers"
      >{{ pageName }}</ListInPageHeader
    >
    <SolverSort
      :param="searchParam"
      :is-loading="isLoading.solvers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <SolverListTable
      :solvers="response.solvers"
      :total="total"
      :offset-page="offsetPage"
      :is-loading="isLoading.solvers"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
      @update="update"
    />
    <div class="bordered-component">
      <SolverFooter :for-solver-list="true" />
    </div>
  </RootTemPlate>
</template>

<script lang="ts">
import SolverSort from '../organisms/SolverSort.vue'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import SolverListTable from '~/components/solver/organisms/SolverListTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import type { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'
import SolverFooter from '~/components/solver/organisms/SolverFooter.vue'
import { GetSolversRequest, GetSolversResponse } from '~/models/Solver'
import { isLoading } from '~/pages/solver/list/_solverCorporationId.vue'

export class SolverSearchParam {
  name = ''
  sex = 'all'
  operatingStatus = 'all'
}

export default CommonList.extend({
  name: 'SolverList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    SolverSort,
    SolverListTable,
    SolverFooter,
  },
  props: {
    /** GetUsers APIのレスポンス
     * @from SolverCorporationId
     */
    response: {
      type: Object as PropType<GetSolversResponse>,
      required: true,
    },
    /** ローディング中かどうか
     * @from SolverCorporationId
     */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: SolverSearchParam,
      RequestType: GetSolversRequest,
      apiName: 'getSolvers',
      pageName: this.$t('solver.pages.list.name'),
      lastSearchRequest: new GetSolversRequest(),
    }
  },
  methods: {
    update(id: string, version: number) {
      this.$emit('update', id, version)
    },
  },
})
</script>
