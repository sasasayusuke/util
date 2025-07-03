<template>
  <v-footer class="o-footer" :height="type === 5 ? 77 : 137">
    <v-container class="o-footer__inner">
      <v-row v-if="type !== 5 && !isHideLinks" no-gutters>
        <v-col cols="auto" class="o-footer__logo pr-3">
          <img
            src="@/assets/img/logo-sap-footer.png"
            alt="Sony Acceleration Platform"
            width="77"
            height="77"
          />
        </v-col>
        <v-col cols="auto" class="o-footer__link">
          <a
            class="o-footer__link__text"
            href="https://www.sony.com/ja/privacy/"
            target="_blank"
            >{{ $t('footer.link.privacy') }}</a
          >
        </v-col>
        <v-col cols="auto" class="o-footer__link">
          <a
            class="o-footer__link__text"
            href="https://sony-startup-acceleration-program.com/privacy"
            target="_blank"
            >{{ $t('footer.link.privacySSAP') }}</a
          >
        </v-col>
        <v-col cols="auto" class="o-footer__link">
          <a class="o-footer__link__text" href="/terms" target="_blank">{{
            $t('footer.link.terms')
          }}</a>
        </v-col>
        <v-col cols="auto" class="o-footer__link">
          <a
            class="o-footer__link__text"
            href="/guide/index.html"
            target="_blank"
            >{{ $t('footer.link.guide') }}</a
          >
        </v-col>
        <!-- TODO ↓未ログイン時とログイン後顧客の場合非表示 -->
        <v-col v-if="showsSsapGuide" cols="auto" class="o-footer__link">
          <a
            class="o-footer__link__text"
            href="/guide/ssap/index.html"
            target="_blank"
            >{{ $t('footer.link.guideSSAP') }}</a
          >
        </v-col>
        <v-col cols="auto" class="o-footer__link">
          <a class="o-footer__link__text" href="/contact" target="_blank">{{
            $t('footer.link.contact')
          }}</a>
        </v-col>
      </v-row>
      <v-row no-gutters>
        <v-col class="o-footer__copyright" :class="type === 5 ? 'mt-0' : ''">
          <p class="o-footer__copyright__text">
            {{ $t('footer.copyright') }}
          </p>
        </v-col>
      </v-row>
    </v-container>
  </v-footer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {
    type: {
      type: Number,
      default: 1,
    },
  },
  computed: {
    showsSsapGuide() {
      return meStore.role && meStore.role !== ENUM_USER_ROLE.CUSTOMER
    },
    isHideLinks(): boolean {
      return (
        this.isCurrent('/anonymous-survey') ||
        this.isSourcePath('/anonymous-survey')
      )
    },
  },
  methods: {
    isCurrent(path: string): boolean {
      const currentPath = this.$route.path
      return currentPath.startsWith(path)
    },
    isSourcePath(path: string): boolean {
      const defaultPath = '/'
      const sourcePath = this.backToUrl(defaultPath)
      return sourcePath.startsWith(path)
    },
  },
})
</script>

<style lang="scss" scoped>
.o-footer {
  padding: 0 25px;
}
.o-footer__link {
  align-self: center;
  padding: 0 10px !important;
}
.o-footer__link__text {
  color: $c-black;
  text-decoration: none;
  @include fontSize('xsmall');
  &:hover {
    color: $c-primary-dark;
    font-weight: bold;
    text-decoration: underline;
  }
}
.o-footer__inner {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0;
}
.o-footer__logo {
  line-height: 0;
  margin: 0;
}
// .o-footer__button {
//   align-self: center;
//   .v-btn {
//     padding: 0 11px !important;
//   }
//   .v-btn__content {
//     white-space: nowrap;
//   }
// }
.o-footer__copyright {
  text-align: right;
  margin-top: -1em;
}
.o-footer__copyright__text {
  color: $c-black-60;
  margin: 0;
  font-weight: 400;
  @include fontSize('xsmall');
  line-height: 1.25;
  font-family: 'Roboto', sans-serif;
}
</style>
<style lang="scss">
.o-footer__inner {
  .a-button {
    font-weight: normal !important;
    &:hover,
    &:focus {
      font-weight: bold !important;
    }
  }
}
</style>
