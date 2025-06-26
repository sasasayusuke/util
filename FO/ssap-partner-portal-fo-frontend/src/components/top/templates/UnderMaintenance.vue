<template>
  <RootTemplate>
    <v-row no-gutters justify="center" class="pt-16 mt-1">
      <v-col cols="auto">
        <UnderMaintenanceCard />
      </v-col>
    </v-row>
  </RootTemplate>
</template>

<script lang="ts">
import UnderMaintenanceCard from '../../top/organisms/UnderMaintenanceCard.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    UnderMaintenanceCard,
  },
})
</script>
