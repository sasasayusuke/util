<template>
  <TemplateUserList
    :response="getUsersResponse"
    :suggest-users="suggestUsersResponse"
    :is-loading="isLoading"
    @getUsers="getUsers"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateUserList, {
  isLoading,
} from '~/components/user/templates/UserList.vue'
import {
  GetUsersRequest,
  GetUsersResponse,
  GetUsers,
  SuggestUsersResponse,
  SuggestUsers,
} from '~/models/User'

export default BasePage.extend({
  name: 'UserList',
  components: {
    TemplateUserList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('user.pages.index.name') as string,
    }
  },
  data(): {
    getUsersResponse: GetUsersResponse
    suggestUsersResponse: SuggestUsersResponse
    isLoading: isLoading
  } {
    return {
      getUsersResponse: new GetUsersResponse(),
      suggestUsersResponse: new SuggestUsersResponse(),
      isLoading: {
        users: true,
        suggestUsers: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getUsers(), this.suggestUsers()])
  },
  methods: {
    /**
     * GetUsersAPIを叩き、Front Officeにサインインできる一般ユーザーの検索・一覧を取得
     * @returns GetUsersAPIのリクエストパラメータ
     */
    async getUsers(params: GetUsersRequest = new GetUsersRequest()) {
      this.isLoading.users = true

      await GetUsers(params).then((res) => {
        this.getUsersResponse = res.data
        this.isLoading.users = false
      })
    },
    /**
     * SuggestUsersAPIを叩き、一般ユーザーのサジェスト用情報を取得
     */
    async suggestUsers() {
      this.isLoading.suggestUsers = true

      await SuggestUsers().then((res) => {
        this.suggestUsersResponse = res.data
        this.isLoading.suggestUsers = false
      })
    },
  },
})
</script>
