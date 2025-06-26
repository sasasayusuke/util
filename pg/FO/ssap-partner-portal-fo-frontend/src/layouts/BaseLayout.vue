<template>
  <v-app class="ssap">
    <!-- Full screen loading -->
    <div v-if="isFullScreenLoading" id="full-screen-loading">
      <div id="nuxt-full-screen-loading" aria-live="polite" role="status">
        <div>Loading...</div>
      </div>
    </div>
    <!-- Header start -->
    <TheHeader :type="type" />
    <!-- Header end -->

    <!-- Content start -->
    <v-main class="ssap__main">
      <!-- Components loading -->
      <div id="component-loading">
        <div id="nuxt-component-loading" aria-live="polite" role="status">
          <div>Loading...</div>
        </div>
      </div>
      <v-container
        id="component-loaded"
        fluid
        pt-0
        px-0
        :fill-height="type === 4"
      >
        <nuxt></nuxt>
      </v-container>
    </v-main>
    <!-- Content end -->

    <!-- Footer start -->
    <TheFooter :type="type" />
    <!-- Footer end -->
  </v-app>
</template>

<script lang="ts">
import BaseVueClass from '~/common/BaseVueClass'
import TheHeader from '~/components/header/organisms/TheHeader.vue'
import TheFooter from '~/components/footer/organisms/TheFooter.vue'
import {
  meStore,
  errorBarStore,
  redirectStore,
  fullScreenLoadingStore,
} from '~/store'
import { getAllowedRoles } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseVueClass.extend({
  name: 'Base',
  components: {
    TheHeader,
    TheFooter,
  },
  props: {
    type: {
      type: Number,
      default: 1,
    },
  },
  watch: {
    // 別ページに移動した際にエラーバーのメッセージをクリアする
    $route(to, from) {
      if (to.path !== from.path) {
        errorBarStore.clear()
      }
    },
  },
  beforeCreate() {
    // リダイレクトURLがセットされている場合はローディングをホールドする
    let redirectBasePath = '/home'
    if (
      meStore.role === ENUM_USER_ROLE.APT ||
      meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
    ) {
      redirectBasePath = '/solver/menu'
    }

    const encodedRedirectUrl = redirectStore.url
    if (this.$route.path === redirectBasePath && encodedRedirectUrl) {
      const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
      const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
      if (decodedRedirectPath === redirectBasePath) {
        fullScreenLoadingStore.clearHold()
      } else {
        const allowedRoles = getAllowedRoles(decodedRedirectPath)
        if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
          fullScreenLoadingStore.setHold()
        }
      }
    }
  },
  created() {
    this.$nuxt.$on('componentLoading', () => {
      const componentLoaded: HTMLElement = document.getElementById(
        'component-loaded'
      ) as HTMLElement
      const componentLoading: HTMLElement = document.getElementById(
        'component-loading'
      ) as HTMLElement
      componentLoaded.style.display = 'none'
      componentLoading.style.display = 'block'
    })
    this.$nuxt.$on('componentLoaded', () => {
      const componentLoaded: HTMLElement = document.getElementById(
        'component-loaded'
      ) as HTMLElement
      const componentLoading: HTMLElement = document.getElementById(
        'component-loading'
      ) as HTMLElement
      componentLoading.style.display = 'none'
      componentLoaded.style.display = 'block'
    })
    this.$nuxt.$on('modalLoading', () => {
      const modalLoaded: HTMLElement = document.getElementById(
        'modal-loaded'
      ) as HTMLElement
      const modalLoading: HTMLElement = document.getElementById(
        'modal-loading'
      ) as HTMLElement
      modalLoaded.style.display = 'none'
      modalLoading.style.display = 'block'
    })
    this.$nuxt.$on('modalLoaded', () => {
      const modalLoaded: HTMLElement = document.getElementById(
        'modal-loaded'
      ) as HTMLElement
      const modalLoading: HTMLElement = document.getElementById(
        'modal-loading'
      ) as HTMLElement
      modalLoading.style.display = 'none'
      modalLoaded.style.display = 'block'
    })
  },
  mounted() {
    if (fullScreenLoadingStore.isHold === false) {
      this.isFullScreenLoading = false
    }
    // 読み込みホールド状態は都度開放する。
    fullScreenLoadingStore.clearHold()
  },
  destroyed() {
    this.clearErrorBar()
  },
  data(): {
    isFullScreenLoading: boolean
  } {
    return {
      isFullScreenLoading: true,
    }
  },
})
</script>

<style lang="scss" scoped>
.ssap {
  background-color: $c-black-page-bg;
}
.ssap__main {
  width: 1200px;
  margin: 0 auto;
  color: $c-black;
}

#component-loading {
  background: transparent;
  display: none;
}

#component-loaded {
  display: block;
}

#nuxt-component-loading {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  overflow: hidden;
}

#nuxt-component-loading > div,
#nuxt-component-loading > div:after {
  border-radius: 50%;
  width: 5rem;
  height: 5rem;
}

#nuxt-component-loading > div {
  font-size: 10px;
  position: relative;
  text-indent: -9999em;
  border: 0.5rem solid #f5f5f5;
  border-left: 0.5rem solid silver;
  -webkit-transform: translateZ(0);
  -ms-transform: translateZ(0);
  transform: translateZ(0);
  -webkit-animation: nuxtLoading 1.1s infinite linear;
  animation: nuxtLoading 1.1s infinite linear;
}

#nuxt-component-loading.error > div {
  border-left: 0.5rem solid #ff4500;
  animation-duration: 5s;
}

#full-screen-loading {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100%;
  z-index: 9999;
  position: fixed;
  background: white;
}

#nuxt-full-screen-loading {
  background: white;
  visibility: hidden;
  opacity: 0;
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  animation: nuxtLoadingIn 10s ease;
  -webkit-animation: nuxtLoadingIn 10s ease;
  animation-fill-mode: forwards;
  overflow: hidden;
}

@keyframes nuxtLoadingIn {
  0% {
    visibility: hidden;
    opacity: 0;
  }
  20% {
    visibility: visible;
    opacity: 0;
  }
  100% {
    visibility: visible;
    opacity: 1;
  }
}

@-webkit-keyframes nuxtLoadingIn {
  0% {
    visibility: hidden;
    opacity: 0;
  }
  20% {
    visibility: visible;
    opacity: 0;
  }
  100% {
    visibility: visible;
    opacity: 1;
  }
}

#nuxt-full-screen-loading > div,
#nuxt-full-screen-loading > div:after {
  border-radius: 50%;
  width: 5rem;
  height: 5rem;
}

#nuxt-full-screen-loading > div {
  font-size: 10px;
  position: relative;
  text-indent: -9999em;
  border: 0.5rem solid #f5f5f5;
  border-left: 0.5rem solid black;
  -webkit-transform: translateZ(0);
  -ms-transform: translateZ(0);
  transform: translateZ(0);
  -webkit-animation: nuxtLoading 1.1s infinite linear;
  animation: nuxtLoading 1.1s infinite linear;
}

#nuxt-full-screen-loading.error > div {
  border-left: 0.5rem solid #ff4500;
  animation-duration: 5s;
}

@-webkit-keyframes nuxtLoading {
  0% {
    -webkit-transform: rotate(0deg);
    transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
    transform: rotate(360deg);
  }
}

@keyframes nuxtLoading {
  0% {
    -webkit-transform: rotate(0deg);
    transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
    transform: rotate(360deg);
  }
}
</style>
