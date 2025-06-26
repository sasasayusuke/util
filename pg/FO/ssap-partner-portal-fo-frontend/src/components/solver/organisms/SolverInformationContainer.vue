<template>
  <Sheet elevation="3" rounded width="1200">
    <v-expansion-panels accordion>
      <v-expansion-panel class="o-expansion-panel">
        <slot />
      </v-expansion-panel>
    </v-expansion-panels>

    <InContentFooter class="pt-0">
      <template #button>
        <div class="d-flex justify-center">
          <Button style-set="xlarge-primary" width="400" :to="backUrl">
            {{ $t('common.button.solverTop3') }}
          </Button>
        </div>
      </template>
    </InContentFooter>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Button } from '~/components/common/atoms/index'
import InContentFooter from '~/components/common/organisms/InContentFooter.vue'

export default BaseComponent.extend({
  components: {
    Sheet,
    Button,
    InContentFooter,
  },
  props: {
    backUrl: {
      type: String,
      required: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.o-expansion-panel {
  &:before {
    box-shadow: none;
  }
}
.container {
  background: $c-white;
  max-width: 1200px;
}
</style>
