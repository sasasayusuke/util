<template>
  <div class="m-modal">
    <!-- Modal loading -->
    <div id="modal-loading">
      <div id="nuxt-modal-loading" aria-live="polite" role="status">
        <div>Loading...</div>
      </div>
    </div>
    <div id="modal-loaded">
      <Sheet
        v-if="!smallModal"
        color="#fff"
        rounded
        width="664"
        class="m-modal__wrapper"
      >
        <Sheet
          class="m-modal__header d-flex justify-center align-center font-size-large font-weight-bold px-9"
          color="#F0F0F0"
          height="60"
          rounded
        >
          {{ title }}
          <div v-if="!notRequired" class="m-modal__required">
            <Required />
            <span class="m-modal__required__text">{{
              $t('common.label.required2')
            }}</span>
          </div>
        </Sheet>
        <v-container py-4 pb-6 px-9 ma-0>
          <slot name="default" />
          <v-row class="pt-6 my-0" justify="center">
            <slot name="foot" />
          </v-row>
        </v-container>
      </Sheet>
      <Sheet v-else color="#fff" rounded width="400">
        <Sheet
          class="d-flex justify-center align-center font-size-large font-weight-bold px-9"
          color="#F0F0F0"
          height="60"
          rounded
        >
          {{ title }}
          <Required />
        </Sheet>
        <v-container py-4 pb-8 px-9 ma-0>
          <slot name="default" />
          <v-row class="pt-6 my-0" justify="center">
            <slot name="foot" />
          </v-row>
        </v-container>
      </Sheet>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet, Required } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    Button,
    Sheet,
    Required,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    smallModal: {
      type: Boolean,
      default: false,
    },
    notRequired: {
      type: Boolean,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgb(51 51 51 / 85%);
  z-index: 1000;
}
.m-modal__wrapper {
  z-index: 1500;
}
.m-modal__close--bg {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  z-index: 1;
  opacity: 0;
  cursor: default;
}
.m-modal__header {
  position: relative;
}
.m-modal__required {
  @include fontSize('xsmall');
  font-weight: normal;
  position: absolute;
  top: 0;
  bottom: 0;
  right: 30px;
  margin: auto;
  height: 1em;
}
.m-modal__required__text {
  color: $c-black-60;
}

#modal-loading {
  background: transparent;
  display: none;
}

#modal-loaded {
  display: block;
}

#nuxt-modal-loading {
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

#nuxt-modal-loading > div,
#nuxt-modal-loading > div:after {
  border-radius: 50%;
  width: 5rem;
  height: 5rem;
}

#nuxt-modal-loading > div {
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

#nuxt-modal-loading.error > div {
  border-left: 0.5rem solid #ff4500;
  animation-duration: 5s;
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
