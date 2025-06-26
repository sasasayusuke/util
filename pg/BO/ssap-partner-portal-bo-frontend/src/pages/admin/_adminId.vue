<template>
  <TemplateAdminDetail :admin="admin" @refresh="refresh" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateAdminDetail from '~/components/admin/templates/AdminDetail.vue'
import { GetAdminById, GetAdminByIdResponse } from '@/models/Admin'

export default BasePage.extend({
  name: 'AdminDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateAdminDetail,
  },
  data() {
    return {
      admin: new GetAdminByIdResponse(),
    }
  },
  mounted() {
    this.displayLoading([this.getAdminById()])
  },
  methods: {
    /**
     * GetAdminByIdAPIを叩いて、管理ユーザーをadminIdで一意に取得
     */
    async getAdminById() {
      const id: string = this.$route.params.adminId

      await GetAdminById(id).then((res) => {
        this.admin = res.data
      })
    },
    /**
     * 再度GetAdminByIdAPIを叩いて、管理ユーザーをadminIdで一意に取得
     */
    refresh() {
      this.displayLoading([this.getAdminById()])
    },
  },
})
</script>
