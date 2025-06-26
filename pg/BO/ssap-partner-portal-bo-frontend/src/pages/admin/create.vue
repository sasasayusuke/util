<template>
  <TemplateAdminCreate :admin="admin" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateAdminCreate from '~/components/admin/templates/AdminCreate.vue'
import { GetAdminByIdResponse } from '@/models/Admin'

export default BasePage.extend({
  name: 'AdminCreate',
  middleware: ['roleCheck'],
  components: {
    TemplateAdminCreate,
  },
  head() {
    return {
      title: this.$t('admin.pages.create.name') as string,
    }
  },
  data() {
    return {
      admin: new GetAdminByIdResponse(),
    }
  },
})
</script>
