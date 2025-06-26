<template>
  <BaseLayout
    v-if="isLoading === false"
    :type="1"
    :style="{
      'background-image': 'url(' + require(`@/assets/img/top-bg.jpg`) + ')',
      'background-size': 'cover',
      'background-position': 'center top',
      'background-repeat': 'no-repeat',
    }"
  />
</template>

<script lang="ts">
import { Auth } from 'aws-amplify'
import BaseLayout from './BaseLayout.vue'
import { signOut } from '~/utils/common-functions'
import { fullScreenLoadingStore, persistedErrorBarStore } from '~/store'

/**
 * 標準レイアウト
 */
export default BaseLayout.extend({
  components: {
    BaseLayout,
  },
  created() {
    // 保持されたエラー情報が存在する場合はエラーバーに表示した後にクリア
    if (persistedErrorBarStore.message !== '') {
      this.showErrorBarWithScrollPageTop(persistedErrorBarStore.message)
      persistedErrorBarStore.clear()
    }
    fullScreenLoadingStore.setHold()
    Auth.currentAuthenticatedUser()
      .then(() => {
        signOut()
      })
      .catch(() => {
        this.isLoading = false
      })
  },
  data(): {
    isLoading: boolean
  } {
    return {
      isLoading: true,
    }
  },
})
</script>
