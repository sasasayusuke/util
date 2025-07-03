<template>
  <div>
    <div v-if="selected === 'fundamental'">
      <MasterKarteFundamental
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :show-current-program="showCurrentProgram"
        :local-param="localParam"
        :select-box-items="selectBoxItems"
        @updateFundamentalValue="updateFundamentalValue"
      />
    </div>
    <div v-if="selected === 'result'">
      <MasterKarteResult
        :select-box-items="selectBoxItems"
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :local-param="localParam"
        @updateResultValue="updateResultValue"
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
        :local-param="localParam"
        @updateOthersValue="updateOthersValue"
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
import {
  GetMasterKarteByIdResponse,
  GetSelectBoxResponse,
} from '~/types/MasterKarte'
import type { PropType } from '~/common/BaseComponent'
import {
  GetMasterKarteSelectBox,
  UpdateMasterKarteResponseClass,
} from '~/models/MasterKarte'

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
    /** 当期支援内容のチェックボックスがチェック済か */
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    localParam: {
      type: Object as PropType<UpdateMasterKarteResponseClass>,
      default: () => ({}),
    },
  },
  data() {
    return {
      schedule: '',
      selectBoxItems: [] as GetSelectBoxResponse[],
    }
  },
  created() {
    this.getSelectBoxItems()
  },
  methods: {
    updateResultValue(val: any) {
      this.$emit('updateResultValue', val)
    },
    updateFundamentalValue(val: any) {
      this.$emit('updateFundamentalValue', val)
    },
    updateOthersValue(val: any) {
      this.$emit('updateOthersValue', val)
    },
    /** セレクトボックスのアイテムを取得 */
    async getSelectBoxItems() {
      await GetMasterKarteSelectBox().then((res: any) => {
        this.selectBoxItems = res.data
      })
    },
  },
})
</script>

<style>
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}
</style>
