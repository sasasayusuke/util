<template>
  <RootTemplate>
    <MasterDetailContainer
      :master="master"
      is-editing
      is-creating
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
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  CreateMaster,
  CreateMasterRequest,
  GetMasterByIdResponse,
} from '~/models/Master'
import { ENUM_CREATE_MASTER_RESPONSE_ERROR } from '~/types/Master'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import { errorBarStore } from '~/store'

export { LocalMaster }

export default CommonCreate.extend({
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
  data() {
    return {
      listPagePath: '/master/list',
      detailPagePrefix: ``,
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * CreateMaster APIを叩き、マスターメンテナンス一件作成
     * @param localMaster 入力中のマスターメンテナンス情報
     */
    create(localMaster: LocalMaster) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new CreateMasterRequest()
      Object.assign(request, localMaster)
      const dataType = request.dataType
      this.detailPagePrefix = `/master/${dataType}/`

      CreateMaster(request)
        .then((res) => {
          const id = res.data.id
          this.toDetailPage(id)
        })
        /**
         * マスターメンテナンス作成時のエラー文を表示する
         */
        .catch((error) => {
          if (
            error?.response?.data?.detail ===
            ENUM_CREATE_MASTER_RESPONSE_ERROR.ALREADY_EXIST
          ) {
            errorBarStore.setMessage(
              this.$t(
                'master.pages.create.errors.registered_data_error'
              ) as string
            )
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
  },
})
</script>
