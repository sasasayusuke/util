<template>
  <RootTemplate>
    <AdminDetailContainer
      :admin="admin"
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
import AdminDetailContainer, {
  LocalAdmin,
} from '../organisms/AdminDetailContainer.vue'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  GetAdminByIdResponse,
  CreateAdminRequest,
  CreateAdmin,
} from '@/models/Admin'
import { PropType } from '~/common/BaseComponent'
import { errorBarStore } from '~/store'
import { ENUM_CREATE_ADMIN_RESPONSE_ERROR } from '~/types/Admin'

export { LocalAdmin }

export default CommonCreate.extend({
  name: 'AdminCreate',
  components: {
    AdminDetailContainer,
    RootTemplate,
  },
  props: {
    /**
     * Back Officeにログイン可能な管理ユーザーの初期値
     */
    admin: {
      type: Object as PropType<GetAdminByIdResponse>,
      required: true,
    },
  },

  data() {
    return {
      listPagePath: '/admin/list',
      detailPagePrefix: '/admin/',
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * CreateAdminAPIを叩き、Back Officeにログイン可能な管理ユーザーを一件作成
     * @param localAdmin 入力中の新規管理者情報
     */
    create(localAdmin: LocalAdmin) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new CreateAdminRequest()
      Object.assign(request, localAdmin)

      if (localAdmin.roles.includes('supporter_mgr')) {
        request.organizationName = ''
        if (localAdmin.supporterOrganizations) {
          localAdmin.supporterOrganizations.forEach((organization) => {
            // @ts-ignore
            request.supporterOrganizationId.push(organization)
          })
        }
      }

      CreateAdmin(request)
        .then((res) => {
          const id = res.data.id
          errorBarStore.clear()
          this.toDetailPage(id)
        })
        .catch((error) => {
          if (
            error?.response?.data?.detail ===
            ENUM_CREATE_ADMIN_RESPONSE_ERROR.ALREADY_REGISTERED
          ) {
            errorBarStore.setMessage(
              this.$t(
                'admin.pages.create.errors.registered_email_error'
              ) as string
            )
          } else if (error?.status === 422) {
            this.setCreateErrorMessage()
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
    /**
     * 管理者ユーザー作成時のエラー文を表示する
     */
    setCreateErrorMessage() {
      errorBarStore.setMessage(this.$t('admin.error.create') as string)
    },
  },
})
</script>
