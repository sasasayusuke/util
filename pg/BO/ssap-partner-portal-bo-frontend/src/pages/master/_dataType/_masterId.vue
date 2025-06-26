<template>
  <TemplateMasterDetail :master="master" @refresh="refresh" />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMasterDetail from '~/components/master/templates/MasterDetail.vue'
import { GetMasterById, GetMasterByIdResponse } from '@/models/Master'

export default BasePage.extend({
  name: 'MasterDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateMasterDetail,
  },
  data() {
    return {
      master: new GetMasterByIdResponse(),
    }
  },
  mounted() {
    this.displayLoading([this.getMasterById()])
  },
  methods: {
    /**
     * getMasterByIdAPIを叩き、一意のマスター情報を取得して代入
     */
    async getMasterById() {
      const masterId: string = this.$route.params.masterId

      await GetMasterById(masterId).then((res) => {
        this.master = res.data
      })
    },
    /**
     * マスタ情報をリフレッシュ
     */
    refresh() {
      this.displayLoading([this.getMasterById()])
    },
  },
})
</script>
