<template>
  <RootTemplate>
    <UserDetailContainer
      :user="user"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      :suggest-solver-corporations="suggestSolverCorporations"
      :get-supporter-organizations-response="getSupporterOrganizationsResponse"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
    <UserInvalidator
      v-if="!isEditing && isSystemAdmin"
      :is-user-invalid="isUserInvalid()"
      @click:switchValid="isModalOpen = true"
    />

    <!-- モーダル -->
    <template v-if="isModalOpen">
      <UserInvalidatorModal
        :is-loading="isLoading"
        :is-user-invalid="isUserInvalid()"
        @click:closeModal="isModalOpen = false"
        @click:patchUser="patchUser"
      />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import UserDetailContainer, {
  LocalUser,
} from '../organisms/UserDetailContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import UserInvalidator from '~/components/user/organisms/UserInvalidator.vue'
import UserInvalidatorModal from '~/components/user/organisms/UserInvalidatorModal.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { meStore } from '~/store'

import {
  UpdateUserByIdRequest,
  UpdateUserById,
  GetUserByIdResponse,
  PatchUser,
  PatchUserStatusRequest,
} from '~/models/User'

import { ISupporterOrganization } from '~/types/User'

export default CommonDetail.extend({
  name: 'UserDetail',
  components: {
    UserDetailContainer,
    UserInvalidator,
    UserInvalidatorModal,
    RootTemplate,
  },
  props: {
    /** 選択したユーザー情報 */
    user: {
      type: Object as PropType<GetUserByIdResponse>,
      required: true,
    },
    /** SuggestSolverCorporations APIの法人一覧レスポンス */
    suggestSolverCorporations: {
      type: Array,
      required: false,
    },
    /** 支援者組織一覧 */
    getSupporterOrganizationsResponse: {
      type: Object,
      required: false,
    },
  },
  /** 編集中かどうかでタイトルを変更 */
  head() {
    return {
      title: this.isEditing
        ? (this.$t('user.pages.edit.name') as string)
        : (this.$t('user.pages.detail.name') as string),
    }
  },
  data() {
    return {
      headerPageName: this.$t('user.group_info.name'),
      listPagePath: '/user/list',
      isLoadingButton: false,
    }
  },
  methods: {
    /** ユーザー情報を更新 */
    update(localUser: LocalUser) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new UpdateUserByIdRequest()

      // @ts-ignore
      request.name = localUser.name
      // @ts-ignore
      request.job = localUser.job
      // @ts-ignore
      request.company = localUser.company
      // @ts-ignore
      request.solverCorporationId = localUser.solverCorporationId
      // @ts-ignore
      request.organizationName = localUser.organizationName
      // @ts-ignore
      request.isInputManHour = localUser.isInputManHour

      // 支援組織を持っているかどうかの判定
      if (localUser.supporterOrganizations !== null) {
        // 支援者組織名の処理 まずFLGを作る idが存在するか
        let isIdExist = false
        // @ts-ignore
        const value = localUser.supporterOrganizations[0]
        if (!(typeof value === 'string')) {
          // idオブジェクトが存在する場合
          isIdExist = true
        } else {
          // idオブジェクトが存在しない場合
          isIdExist = false
        }

        // idが存在する場合は元の状態のまま配列にpush
        if (isIdExist) {
          // @ts-ignore
          localUser.supporterOrganizations.forEach(function (value) {
            // @ts-ignore
            request.supporterOrganizations.push(value)
          })
        } else if (localUser.supporterOrganizations) {
          // idが存在しない場合はidというkeyを付与して配列にpush
          localUser.supporterOrganizations.forEach(
            (value: ISupporterOrganization) => {
              request.supporterOrganizations.push({ id: String(value) })
            }
          )
        }
      }

      if (request.supporterOrganizations === null) {
        request.supporterOrganizations = []
      }
      const id = this.user.id
      const version = this.user.version

      UpdateUserById(id, version, request)
        .then(this.updateThen)
        .catch((error) => {
          if (error.status && parseInt(error.status) === 409) {
            this.$emit('refresh')
            this.isEditing = false
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.conflict'))
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },

    pageName() {
      return this.isEditing
        ? this.$t('user.pages.edit.name')
        : this.$t('user.pages.detail.name')
    },

    /** 編集中であれば、編集中を終了する。編集中でなければ一覧画面に戻る */
    onClickNegative() {
      if (this.isEditing) {
        this.$emit('refresh')
        this.isEditing = false
      } else {
        this.$router.push('/user/list')
      }
    },

    /** 一般ユーザーをIDで一意に有効化/無効化します */
    patchUser(localUser: LocalUser) {
      this.clearErrorBar()
      const request = new PatchUserStatusRequest()
      this.isLoading = true

      request.id = this.user.id
      request.version = +this.user.version
      if (this.user.disabled === true) {
        request.enable = false
      } else {
        request.enable = true
      }

      Object.assign(request, localUser)

      PatchUser(request)
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
    /** ユーザーが有効か判定  */
    isUserInvalid() {
      if (this.user.disabled === true) {
        return true
      } else {
        return false
      }
    },
  },
  computed: {
    /** SystemAdminロールが含まれているアカウントか判定 */
    isSystemAdmin() {
      return meStore.roles.includes('system_admin')
    },
  },
})
</script>
