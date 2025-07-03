<template>
  <RootTemplate>
    <UserDetailContainer
      :user="user"
      is-editing
      is-creating
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      :suggest-customers="suggestCustomers"
      :suggest-solver-corporations="suggestSolverCorporations"
      :get-supporter-organizations-response="getSupporterOrganizationsResponse"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
      @update="updateParam"
    />
  </RootTemplate>
</template>

<script lang="ts">
import UserDetailContainer, {
  LocalUser,
} from '../organisms/UserDetailContainer.vue'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { CreateUser, CreateUserRequest } from '~/models/User'
import { errorBarStore } from '~/store'
import { ENUM_CREATE_USER_RESPONSE_ERROR } from '~/types/User'

export { LocalUser }

export default CommonCreate.extend({
  name: 'UserCreate',
  components: {
    UserDetailContainer,
    RootTemplate,
  },
  props: {
    /** CreateUser APIの空ユーザーのレスポンス */
    user: {
      type: Object,
      required: true,
    },
    /** SuggestCustomers APIの顧客一覧レスポンス */
    suggestCustomers: {
      type: Array,
      required: false,
    },
    /** SuggestSolverCorporations APIの法人一覧レスポンス */
    suggestSolverCorporations: {
      type: Array,
      required: false,
    },
    /** getSupporterOrganizations支援者一覧のレスポンス */
    getSupporterOrganizationsResponse: {
      type: Object,
      required: false,
    },
  },
  data() {
    return {
      listPagePath: '/user/list',
      detailPagePrefix: '/user/',
      isLoadingButton: false,
    }
  },
  methods: {
    /** 新規作成する一般ユーザーの情報を変更 */
    updateParam(keyName: any, newVal: any) {
      this.user[keyName] = newVal
    },

    /** 一般ユーザーを新規作成 */
    create(localUser: LocalUser) {
      this.isLoading = true
      this.isLoadingButton = true
      if (localUser.role === 'customer') {
        delete localUser.company
        delete localUser.isInputManHour
        delete localUser.supporterOrganizations
      } else if (
        (localUser.role === 'supporter' || localUser.role === 'business_mgr') &&
        // @ts-ignore
        localUser.supporterOrganizations.length === 0
      ) {
        delete localUser.supporterOrganizations
      } else if (
        localUser.role === 'supporter' ||
        localUser.role === 'business_mgr'
      ) {
        delete localUser.customerId
        delete localUser.job
        delete localUser.organizationName
      } else if (localUser.role === 'supporter_mgr') {
        delete localUser.customerId
        delete localUser.job
        delete localUser.organizationName
      } else {
        delete localUser.customerId
        delete localUser.job
        delete localUser.isInputManHour
        delete localUser.supporterOrganizations
      }

      const request = new CreateUserRequest()

      Object.assign(request, localUser)

      if (
        localUser.supporterOrganizations &&
        (localUser.role === 'supporter' ||
          localUser.role === 'supporter_mgr' ||
          localUser.role === 'business_mgr')
      ) {
        if (
          localUser.role === 'supporter' ||
          localUser.role === 'business_mgr'
        ) {
          // @ts-ignore
          localUser.supporterOrganizations = Array(
            localUser.supporterOrganizations
          )
        }
        // @ts-ignore
        request.supporterOrganizations = localUser.supporterOrganizations.map(
          function (obj: any) {
            return { id: obj }
          }
        )
      }

      CreateUser(request)
        .then((res) => {
          const id = res.data.id
          errorBarStore.clear()
          this.toDetailPage(id)
        })
        .catch((error) => {
          if (
            error?.response?.data?.detail ===
            ENUM_CREATE_USER_RESPONSE_ERROR.ALREADY_REGISTERED
          ) {
            errorBarStore.setMessage(
              this.$t(
                'user.pages.create.errors.registered_email_error'
              ) as string
            )
          } else if (error?.status === 422) {
            this.setCreateErrorMessage()
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('user.errors.create'))
            //throw error
          }
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
    /** 422エラーの場合にエラー文を表示 */
    setCreateErrorMessage() {
      errorBarStore.setMessage(this.$t('user.errors.create') as string)
    },
  },
})
</script>
