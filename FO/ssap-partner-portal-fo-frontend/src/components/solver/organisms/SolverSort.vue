<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <SolverSortInput :param="param" :is-loading="isLoading" v-on="$listeners" />
  </CommonSort>
</template>

<script lang="ts">
import SolverSortInput from '../molecules/SolverSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export default BaseComponent.extend({
  name: 'SolverSort',
  components: {
    CommonSort,
    SolverSortInput,
  },
  props: {
    /** ソートの条件
     * @from CommonList
     */
    param: {
      type: Object,
      required: true,
    },
    /** ローディング中かどうか
     * @from SolverCorporationId
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
