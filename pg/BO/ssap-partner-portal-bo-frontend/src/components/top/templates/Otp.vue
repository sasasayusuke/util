<template>
  <RootTemplate :class="'align-start'">
    <OtpCard />
  </RootTemplate>
</template>

<script lang="ts">
import OtpCard from '../organisms/OtpCard.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    OtpCard,
  },
})
</script>

<style lang="scss" scoped></style>
