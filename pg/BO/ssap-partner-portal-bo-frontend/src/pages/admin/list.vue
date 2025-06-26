<template>
  <TemplateAdminList
    :response="getAdminsResponse"
    :is-loading="isLoading"
    @sort="getAdmins"
    @getAdmins="getAdmins"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateAdminList, {
  isLoading,
} from '~/components/admin/templates/AdminList.vue'
import { GetAdminsRequest, GetAdminsResponse, GetAdmins } from '@/models/Admin'

export default BasePage.extend({
  name: 'AdminList',
  components: {
    TemplateAdminList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('admin.pages.index.name') as string,
    }
  },
  data(): {
    getAdminsResponse: GetAdminsResponse
    isLoading: isLoading
  } {
    return {
      getAdminsResponse: new GetAdminsResponse(),
      isLoading: {
        admins: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getAdmins()])
  },
  methods: {
    /**
     * GetAdminsAPIを叩いて、Back Officeにログイン可能な管理ユーザーの検索・一覧を取得
     * @param params getAdminAPIのリクエストパラメータを指定
     */
    async getAdmins(params: GetAdminsRequest = new GetAdminsRequest()) {
      this.isLoading.admins = true
      await GetAdmins(params).then((res) => {
        this.getAdminsResponse = res.data
        this.isLoading.admins = false
      })
    },
  },
})
</script>
