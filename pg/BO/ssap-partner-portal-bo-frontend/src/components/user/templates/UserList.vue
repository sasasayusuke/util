<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader>
      {{ pageName }}
    </ListInPageHeader>
    <UserSort
      :param="searchParam"
      :suggest-users="suggestUsers"
      :is-loading="isLoading.suggestUsers"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <UserListTable
      :users="response.users"
      :total="total"
      :offset-page="offsetPage"
      :limit="limit"
      :is-loading="isLoading.users"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import UserSort, {
  UserSearchParam,
} from '~/components/user/organisms/UserSort.vue'
import { GetUsersRequest, GetUsersResponse } from '~/models/User'
import UserListTable from '~/components/user/organisms/UserListTable.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import CommonList from '~/components/common/templates/CommonList'

export interface isLoading {
  users: boolean
  suggestUsers: boolean
}

export default CommonList.extend({
  name: 'UserList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    UserSort,
    UserListTable,
  },
  props: {
    /** GetUsers APIのレスポンス */
    response: {
      type: Object as PropType<GetUsersResponse>,
      required: true,
    },
    /** 画面を読み込んでいるかどうか */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 提案されるユーザー一覧 */
    suggestUsers: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: UserSearchParam,
      RequestType: GetUsersRequest,
      apiName: 'getUsers',
      limit: new GetUsersRequest().limit,
      pageName: this.$t('user.pages.index.name'),
      lastSearchRequest: new GetUsersRequest(),
    }
  },
  methods: {
    /**
     * optionsのプロパティをキーとしてソートを行う
     * @param options
     */
    sort(options: any) {
      const request = this.lastSearchRequest
      let sortKey: string = options.sortBy[0]
      if (options.sortBy[0] === 'lastLoginAt') {
        sortKey = 'last_login_at'
      }

      const sortType: string = options.sortDesc[0] ? 'desc' : 'asc'
      if (sortKey) {
        request.sort = `${sortKey}:${sortType}`
      }
      this.lastSearchRequest = request
      this.$emit('getUsers', request)
    },
    /** 次のページに遷移 */
    nextPage() {
      const request = this.lastSearchRequest
      request.offsetPage++
      this.$emit('getUsers', request)
    },
    /** 前のページに遷移 */
    prevPage() {
      const request = this.lastSearchRequest
      request.offsetPage--
      this.$emit('getUsers', request)
    },
    /** 新規ユーザー新規作成画面に遷移 */
    linkCreate() {
      this.$router.push('/user/create')
    },
  },
})
</script>
