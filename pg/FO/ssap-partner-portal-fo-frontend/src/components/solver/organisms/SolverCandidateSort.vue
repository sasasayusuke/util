<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <SolverCandidateSortInput
      :param="param"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import SolverCandidateSortInput from '../molecules/SolverCandidateSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class SolverCandidateSearchParam {
  name = ''
  sex = 'all'
  certificationStatus = 'all'
}

export default BaseComponent.extend({
  name: 'SolverCandidateSort',
  components: {
    CommonSort,
    SolverCandidateSortInput,
  },
  props: {
    /** ソートの条件 */
    param: {
      type: Object,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
