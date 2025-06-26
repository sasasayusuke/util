<template>
  <RootTemplate>
    <CustomerImportSelectFile />
  </RootTemplate>
</template>

<script lang="ts">
import CustomerImportSelectFile from '../../customer/organisms/CustomerImportSelectFile.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  created() {
    //一括登録機能は利用不可のため、404画面にリダイレクトする
    this.$router.push('/404')
  },
  components: {
    CustomerImportSelectFile,
    RootTemplate,
  },
})
</script>
