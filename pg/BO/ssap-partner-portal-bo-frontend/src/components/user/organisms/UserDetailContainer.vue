<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-hide-button1="isModeDetail && !isSystemAdmin"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive', localUser)"
    @click:negative="$emit('click:negative')"
  >
    <UserDetailRows
      v-model="isValid"
      :user="user"
      :suggest-customers="suggestCustomers"
      :suggest-solver-corporations="suggestSolverCorporations"
      :get-supporter-organizations-response="getSupporterOrganizationsResponse"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @update="update('localUser', $event)"
      v-on="$listeners"
    />
  </DetailContainer>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import UserDetailRows, {
  LocalUser,
} from '~/components/user/molecules/UserDetailRows.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { CreateUserRequest } from '~/models/User'
import { meStore } from '~/store'

export { LocalUser }

export default CommonDetailContainer.extend({
  name: 'UserDetailContainer',
  components: {
    UserDetailRows,
    DetailContainer,
  },
  props: {
    /** CreateUser APIの空ユーザーのレスポンス */
    user: {
      type: Object as PropType<CreateUserRequest>,
    },
    isLoading: {
      type: Boolean,
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
    /** 一般ユーザー情報を保存/更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      /** 入力中の一般ユーザー情報 */
      localUser: new LocalUser(),
    }
  },
  computed: {
    /** ページによって、タイトルを変更 */
    title() {
      return this.$t('user.pages.' + this.mode + '.name')
    },
    /** SystemAdminロールが含まれているアカウントか判定 */
    isSystemAdmin() {
      return meStore.roles.includes('system_admin')
    },
    /** modeが詳細画面か判定 */
    isModeDetail() {
      return this.mode === 'detail'
    },
  },
})
</script>
