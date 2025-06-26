<template>
  <BaseLayout v-if="isLoading === false" :type="1" />
</template>

<script lang="ts">
import { Auth } from 'aws-amplify'
import BaseLayout from './BaseLayout.vue'
import { signOut } from '~/utils/common-functions'
import { fullScreenLoadingStore } from '~/store'

/**
 * 標準レイアウト
 */
export default BaseLayout.extend({
  components: {
    BaseLayout,
  },
  created() {
    // ルートパスにアクセスした場合は場合は強制ログアウト
    if (this.$route.path === '/') {
      fullScreenLoadingStore.setHold()
      Auth.currentAuthenticatedUser()
        .then(() => {
          signOut()
        })
        .catch(() => {
          this.isLoading = false
        })
    } else {
      this.isLoading = false
    }
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
