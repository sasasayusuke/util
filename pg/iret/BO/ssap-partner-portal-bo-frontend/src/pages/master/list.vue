<template>
  <TemplateMasterList
    :response="getMastersResponse"
    :is-loading="isLoading"
    @getMasters="getMasters"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMasterList from '~/components/master/templates/MasterList.vue'
import {
  GetMasters,
  GetMastersRequest,
  GetMastersResponse,
} from '@/models/Master'

export default BasePage.extend({
  name: 'MasterList',
  components: {
    TemplateMasterList,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('master.pages.index.name') as string,
    }
  },
  data(): {
    getMastersResponse: GetMastersResponse
    isLoading: boolean
  } {
    return {
      getMastersResponse: new GetMastersResponse(),
      isLoading: true,
    }
  },
  mounted() {
    this.displayLoading([this.getMasters()])
  },
  methods: {
    /**
     * getMastersAPIを叩き、マスター情報一覧を取得して代入
     * @param params GetMastersAPIのリクエストパラメータを指定
     */
    async getMasters(params: GetMastersRequest = new GetMastersRequest()) {
      this.isLoading = true

      await GetMasters(params).then((res) => {
        this.getMastersResponse = res.data
        this.isLoading = false
      })
    },
  },
})
</script>
