<template>
  <div>
    <!-- 各要素をselectedを基準にv-showで表示する -->
    <div v-if="selected === 'fundamental'">
      <MasterKarteFundamental
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :show-current-program="showCurrentProgram"
      />
    </div>
    <div v-if="selected === 'result'">
      <MasterKarteResult
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
      />
    </div>
    <div v-if="selected === 'client'">
      <MasterKarteClient
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
      />
    </div>
    <div v-if="selected === 'others'">
      <MasterKarteOthers
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :show-current-program="showCurrentProgram"
      />
    </div>
  </div>
</template>

<script lang="ts">
import MasterKarteClient from './MasterKarteClient.vue'
import MasterKarteOthersVue from './MasterKarteOthers.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import { GetMasterKarteByIdResponse } from '~/types/MasterKarte'
import type { PropType } from '~/common/BaseComponent'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    LastUpdate,
    MasterKarteClient,
    MasterKarteOthersVue,
  },
  props: {
    selected: {
      type: String,
      default: 'fundamental',
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      schedule: '',
    }
  },
})
</script>

<style>
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}
</style>
