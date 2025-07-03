<template>
  <RootTemPlate>
    <ListInPageHeader>
      {{ pageName }}
    </ListInPageHeader>
    <AdminSort
      :params="searchParam"
      :is-loading="isLoading.admins"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <AdminListTable
      :admins="response.admins"
      :total="total"
      :limit="limit"
      :is-loading="isLoading.admins"
      @sort="sort($event)"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import AdminSort, {
  AdminSearchParams,
} from '~/components/admin/organisms/AdminSort.vue'
import AdminListTable from '~/components/admin/organisms/AdminListTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetAdminsRequest, GetAdminsResponse } from '@/models/Admin'
import type { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'

export interface isLoading {
  admins: boolean
}

export default CommonList.extend({
  name: 'AdminList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    AdminSort,
    AdminListTable,
  },
  props: {
    /**
     * GetAdminsAPIのレスポンス
     */
    response: {
      type: Object as PropType<GetAdminsResponse>,
      required: true,
    },
    /**
     * ローディング中か否かを示す
     */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: AdminSearchParams,
      RequestType: GetAdminsRequest,
      apiName: 'getAdmins',
      headerPageName: this.$t('admin.group_info.name'),
      pageName: this.$t('admin.pages.index.name'),
      lastSearchRequest: new GetAdminsRequest(),
      buttons: [
        { name: this.$t('admin.pages.index.name'), link: '/admin/list' },
        { name: this.$t('admin.pages.create.name'), link: '/admin/create' },
      ],
    }
  },
  methods: {
    /**
     * ソート条件を元にGetAdminsAPIを叩く。ソート条件を明示していない場合はログイン日時順に表示
     * @param options ソート条件
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
      this.$emit('getAdmins', request)
    },
  },
})
</script>
