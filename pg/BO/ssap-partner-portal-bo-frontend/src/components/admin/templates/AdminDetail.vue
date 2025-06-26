<template>
  <RootTemplate>
    <AdminDetailContainer
      :admin="admin"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
    <AdminInvalidation
      v-if="!isEditing"
      :is-admin-invalid="admin.disabled"
      @click:switchValid="isModalOpen = true"
    />
    <AdminInvalidationModal
      v-if="isModalOpen"
      :is-admin-invalid="admin.disabled"
      @click:closeModal="isModalOpen = false"
      @click:patch="patch"
    />
  </RootTemplate>
</template>

<script lang="ts">
import AdminDetailContainer, {
  LocalAdmin,
} from '../organisms/AdminDetailContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import AdminInvalidation from '~/components/admin/molecules/AdminInvalidation.vue'
import AdminInvalidationModal from '~/components/admin/molecules/AdminInvalidationModal.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  UpdateAdminByIdRequest,
  GetAdminByIdResponse,
  UpdateAdminById,
  PatchAdminStatusByIdRequest,
  PatchAdminStatusById,
  CreateLocalAdmin,
  getAdminByMine,
} from '@/models/Admin'
import { PropType } from '~/common/BaseComponent'
import { meStore } from '~/store'

class organization {
  id: string = ''
  name: string = ''
}

class UpdateLocalAdmin extends CreateLocalAdmin {
  supporterOrganizations?: organization[]
}

export default CommonDetail.extend({
  name: 'AdminDetail',
  components: {
    AdminDetailContainer,
    AdminInvalidation,
    AdminInvalidationModal,
    RootTemplate,
  },
  props: {
    /**
     * Back Officeにログイン可能な選択中の管理ユーザー
     */
    admin: {
      type: Object as PropType<GetAdminByIdResponse>,
      required: true,
    },
  },
  head() {
    return {
      title: this.isEditing
        ? (this.$t('admin.pages.edit.name') as string)
        : (this.$t('admin.pages.detail.name') as string),
    }
  },
  data() {
    return {
      headerPageName: this.$t('admin.group_info.name'),
      listPagePath: '/admin/list',
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * UpdateAdminByIdAPIを叩き、Back Officeにログイン可能な管理ユーザーを入力した値に更新
     * @param localAdmin 入力中の新規管理者情報
     */
    async update(localAdmin: UpdateLocalAdmin) {
      const request = new UpdateAdminByIdRequest()
      this.isLoading = true
      this.isLoadingButton = true

      if (localAdmin.roles.includes('supporter_mgr')) {
        if (localAdmin.supporterOrganizations) {
          localAdmin.supporterOrganizations.forEach((organization: any) => {
            // organizationがObject型になっている場合はidのみpushする
            if (organization.id) {
              request.supporterOrganizationId.push(organization.id)
            } else {
              request.supporterOrganizationId.push(organization)
            }
          })
        }
      } else {
        request.organizationName = localAdmin.organizationName
      }

      request.name = localAdmin.name
      request.email = localAdmin.email
      request.roles = localAdmin.roles
      request.company = localAdmin.company
      request.job = localAdmin.job
      request.disabled = localAdmin.disabled

      const id = localAdmin.id
      const version = localAdmin.version

      await UpdateAdminById(id, version, request)
        .then(this.updateThen)
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })

      // 自分の管理ユーザーシステムロールを変更する場合は更新された情報をすぐにstoreに反映させる
      if (request.name === meStore.name) {
        await getAdminByMine().then((response) => {
          meStore.setResponse(response.data)
        })
      }
    },
    /**
     * PatchAdminStatusByIdAPIを叩き、管理ユーザーをIDで一意に有効化/無効化。 物理削除は行わずdisableの値を制御
     * @param localAdmin 入力中の新規管理者情報(不要だと思われます)
     */
    patch(localAdmin: LocalAdmin) {
      this.clearErrorBar()
      const request = new PatchAdminStatusByIdRequest()
      this.isLoading = true

      request.id = this.admin.id
      request.version = +this.admin.version
      if (this.admin.disabled === true) {
        request.enable = false
      } else {
        request.enable = true
      }
      Object.assign(request, localAdmin)

      PatchAdminStatusById(request)
        .then(() => {})
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isModalOpen = false
          this.isLoading = false
          this.$emit('refresh')
        })
    },
  },
})
</script>
