<template>
  <RootTemplate>
    <MasterDetailContainer
      :master="master"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import MasterDetailContainer, {
  LocalMaster,
} from '../organisms/MasterDetailContainer.vue'
import { PropType } from '~/common/BaseComponent'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  UpdateMasterById,
  GetMasterByIdResponse,
  UpdateMasterRequest,
} from '~/models/Master'

export default CommonDetail.extend({
  components: {
    MasterDetailContainer,
    RootTemplate,
  },
  props: {
    /**
     * GetMasterById APIから取得したマスターメンテナンス情報
     */
    master: {
      type: Object as PropType<GetMasterByIdResponse>,
      required: true,
    },
  },
  head() {
    const isEditing = this.isEditing as boolean
    const edit = this.$t('master.pages.edit.name') as string
    const detail = this.$t('master.pages.detail.name') as string
    return {
      title: isEditing ? edit : detail,
    }
  },
  data() {
    return {
      headerPageName: this.$t('master.group_info.name'),
      listPagePath: '/master/list',
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * ページ、タブの名称
     */
    pageName() {
      const isEditing = this.isEditing as boolean
      const edit = this.$t('master.pages.edit.name') as string
      const detail = this.$t('master.pages.detail.name') as string
      return isEditing ? edit : detail
    },
    /**
     * UpdateMasterById APIを叩き、マスターを更新
     * @param localMaster 入力中のマスターメンテナンス情報
     */
    update(localMaster: LocalMaster) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new UpdateMasterRequest()
      Object.assign(request, localMaster)

      const id = localMaster.id
      const version = localMaster.version

      UpdateMasterById(id, version, request)
        .then(this.updateThen)
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
  },
})
</script>
