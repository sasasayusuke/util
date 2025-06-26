<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive', localMaster)"
    @click:negative="$emit('click:negative')"
  >
    <MasterDetailRows
      v-model="isValid"
      :master="master"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @update="update('localMaster', $event)"
    />
  </DetailContainer>
</template>

<script lang="ts">
import type { LocaleMessages } from 'vue-i18n/types'
import { PropType } from '~/common/BaseComponent'
import MasterDetailRows, {
  LocalMaster,
} from '~/components/master/molecules/MasterDetailRows.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { GetMasterByIdResponse } from '~/models/Master'

export { LocalMaster }

export default CommonDetailContainer.extend({
  components: {
    MasterDetailRows,
    DetailContainer,
  },
  props: {
    /**
     * GetMasterById APIから取得したマスターメンテナンス情報
     */
    master: {
      type: Object as PropType<GetMasterByIdResponse>,
    },
    /** マスターメンテナンス登録/更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localMaster: new LocalMaster(),
    }
  },
  computed: {
    title(): string | LocaleMessages {
      return this.$t('master.pages.' + this.mode + '.name')
    },
  },
})
</script>
