<template>
  <TemplateMasterCreate :master="master" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMasterCreate from '~/components/master/templates/MasterCreate.vue'
import { GetMasterByIdResponse } from '@/models/Master'

export default BasePage.extend({
  name: 'MasterCreate',
  middleware: ['roleCheck'],
  components: {
    TemplateMasterCreate,
  },
  head() {
    return {
      title: this.$t('master.pages.create.name') as string,
    }
  },
  data() {
    return {
      master: new GetMasterByIdResponse(),
    }
  },
})
</script>
