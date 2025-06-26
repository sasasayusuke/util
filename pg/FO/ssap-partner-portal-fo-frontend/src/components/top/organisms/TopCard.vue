<template>
  <v-layout class="o-top-card" justify-center>
    <v-card elevation="4" class="px-12 py-12" width="500">
      <h1 class="text-center">
        <TopCardLogo />
      </h1>
      <p class="text-h6 font-weight-bold pt-12 mb-0">
        {{ $t('top.pages.index.lead1') }}<br />
        {{ $t('top.pages.index.lead2') }}
      </p>
      <v-layout class="pt-8 justify-center">
        <Button
          class="white--text font-weight-bold font-size-large"
          block
          x-large
          color="primary"
          @click="onClickSignIn"
        >
          {{ $t('common.button.signin') }}
        </Button>
      </v-layout>
      <v-layout class="pt-4 justify-center">
        <Button class="font-weight-bold" text :href="signupUrl">
          {{ $t('common.button.signup') }}
        </Button>
      </v-layout>
    </v-card>
  </v-layout>
</template>

<script lang="ts">
import { Auth } from 'aws-amplify'
import { Button } from '~/components/common/atoms/index'
import TopCardLogo from '~/components/top/atoms/TopCardLogo.vue'
import BaseComponent from '~/common/BaseComponent'
import { redirectStore } from '~/store'
import { getAllowedRoles } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  components: {
    Button,
    TopCardLogo,
  },
  data() {
    return {
      loading: false,
      signupUrl: process.env.SSAP_SITE_URL + 'users/new',
    }
  },
  beforeCreate() {
    // Get値が無いが一時保存のリダイレクトURLがセットされている場合はパラメータを付加
    if (!this.$route.query.redirect_url) {
      const encodedRedirectUrl = redirectStore.bufferUrl
      redirectStore.clearBufferUrl()
      if (encodedRedirectUrl) {
        const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
        const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
        const allowedRoles = getAllowedRoles(decodedRedirectPath)
        if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
          this.$router.push(`/?redirect_url=${encodedRedirectUrl}`)
        }
      }
    } else {
      redirectStore.clearBufferUrl()
    }
  },
  methods: {
    onClickSignIn() {
      this.loading = true
      // Cognitoにアクセス
      const provider = process.env.AWS_FEDERATION_PROVIDER
      if (provider != null) {
        // リダイレクト先の指定がある場合はセッションストレージに保存
        if (
          this.$route.query.redirect_url &&
          this.$route.query.redirect_url !== ''
        ) {
          const encodedRedirectUrl = String(this.$route.query.redirect_url)
          const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
          const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
          const allowedRoles = getAllowedRoles(decodedRedirectPath)
          if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
            redirectStore.setUrl(encodedRedirectUrl)
          }
        } else {
          redirectStore.clearUrl()
        }
        Auth.federatedSignIn({ customProvider: provider })
      } else {
        this.$logger.error('AWS_FEDERATION_PROVIDER is null.')
        this.loading = false
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.o-top-card {
  padding: 28px 0 26px;
}
</style>
