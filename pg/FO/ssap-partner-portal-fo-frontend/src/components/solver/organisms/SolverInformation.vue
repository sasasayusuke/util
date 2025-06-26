<template>
  <SolverInformationContainer :back-url="backUrl">
    <SolverInputFinished
      :finish-text1="$t('solver.pages.edit.finishText1')"
      :finish-text2="$t('solver.pages.edit.finishText2')"
    />
  </SolverInformationContainer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import SolverInformationContainer from '~/components/solver/organisms/SolverInformationContainer.vue'
import SolverInputFinished from '~/components/solver/organisms/SolverInputFinished.vue'

export default BaseComponent.extend({
  components: {
    SolverInformationContainer,
    SolverInputFinished,
  },
  props: {
    backUrl: {
      type: String,
      required: true,
    },
  },
})
</script>
