<template>
  <RootTemplate :class="'t-home'">
    <TopCard />
  </RootTemplate>
</template>

<script lang="ts">
import TopCard from '../organisms/TopCard.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    TopCard,
  },
})
</script>

<style lang="scss" scoped>
.t-home {
  &.b-RootTemplate {
    padding: 60px 0 137px;
  }
}
</style>
