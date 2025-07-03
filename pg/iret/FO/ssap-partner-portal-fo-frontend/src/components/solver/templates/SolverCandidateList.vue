<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader
      :for-solver-list="true"
      :labels="labels"
      :is-loading="isLoading.solvers"
      >{{ pageName }}</ListInPageHeader
    >
    <SolverCandidateSort
      :param="searchParam"
      :is-loading="isLoading.solvers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <SolverCandidateListTable
      :solvers="response.solvers"
      :total="total"
      :offset-page="offsetPage"
      :is-loading="isLoading.solvers"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
      @patchSolver="patchSolver"
    />
    <SolverFooter :for-solver-list="true" />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import SolverCandidateSort, {
  SolverCandidateSearchParam,
} from '~/components/solver/organisms/SolverCandidateSort.vue'
import { GetSolversRequest, GetSolversResponse } from '~/models/Solver'
import SolverCandidateListTable from '~/components/solver/organisms/SolverCandidateListTable.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import CommonList from '~/components/common/templates/CommonList'
import { solverCorporationStore } from '~/store'

export interface isLoading {
  solvers: boolean
}

export default CommonList.extend({
  name: 'SolverCandidateList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    SolverCandidateSort,
    SolverCandidateListTable,
  },
  props: {
    /** GetUsers APIのレスポンス */
    response: {
      type: Object as PropType<GetSolversResponse>,
      required: true,
    },
    /** 画面を読み込んでいるかどうか */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: SolverCandidateSearchParam,
      RequestType: GetSolversRequest,
      apiName: 'getSolvers',
      pageName: this.$t('solver.pages.candidateList.name'),
      lastSearchRequest: new GetSolversRequest(),
      labels: [
        {
          label: this.$t(
            'solver.pages.candidateList.buttons.solverCandidateApplication'
          ),
          to: `/solver/candidate/application?source_url=/solver/candidate/list/${solverCorporationStore.id}`,
        },
      ],
    }
  },
  methods: {
    patchSolver(id: string, version: number) {
      this.$emit('patchSolver', id, version)
    },
  },
})
</script>
