<template>
  <CommonMenuList
    v-slot="{ on, attrs }"
    :items="items"
    is-show-state
    class="m-button-profile"
  >
    <Sheet>
      <Button
        class="m-button-profile__button pr-3"
        icon
        width="48"
        height="36"
        :title="$t('common.button.profile')"
        v-bind="attrs"
        v-on="on"
      >
        <Icon class="m-button-profile__icon--in" size="36" color="#fff"
          >icon-org-account</Icon
        >
        <Icon class="m-button-profile__icon--out" size="36" color="#fff"
          >icon-org-account-outline</Icon
        >
        <Icon
          v-bind="attrs"
          class="m-button-profile__arrow ml-n1"
          size="20"
          color="#fff"
          >mdi-menu-down</Icon
        >
      </Button>
    </Sheet>
  </CommonMenuList>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon, Sheet } from '~/components/common/atoms/index'
import CommonMenuList from '~/components/common/molecules/CommonMenuList.vue'
import { signOut } from '~/utils/common-functions'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    Sheet,
    CommonMenuList,
  },
  data() {
    return {
      items: [
        {
          label: this.$t('header.profile.menu.mailChange') as string,
          name: 'mailChange',
          href: `${process.env.SSAP_SITE_URL}email_reset/new`,
          otherPage: true,
        },
        {
          label: this.$t('header.profile.menu.passwordChange') as string,
          name: 'passwordChange',
          href: `${process.env.SSAP_SITE_URL}password/edit`,
          otherPage: true,
        },
        {
          label: this.$t('header.profile.menu.signOut') as string,
          name: 'signOut',
          onClick() {
            signOut()
          },
        },
        {
          label: this.$t('header.profile.menu.resign') as string,
          name: 'resign',
          href: `${process.env.SSAP_SITE_URL}withdrawal/new`,
          otherPage: true,
        },
      ],
    }
  },
})
</script>

<style lang="scss" scoped>
.m-button-profile__button {
  position: relative;
  &:hover,
  &:focus {
    .m-button-profile__icon--in {
      opacity: 1;
    }
    .m-button-profile__icon--out {
      opacity: 0;
    }
  }
  .v-btn {
    &[aria-expanded='true'] {
      .m-button-profile__icon--in {
        opacity: 1;
      }
      .m-button-profile__icon--out {
        opacity: 0;
      }
    }
  }
}
.m-button-profile__icon {
  position: absolute;
  &--in {
    @extend .m-button-profile__icon;
    opacity: 0;
    &::before {
      background-color: $c-primary-over;
      border-radius: 50%;
    }
  }
  &--out {
    @extend .m-button-profile__icon;
  }
}
.m-button-profile__arrow {
  position: absolute;
  right: -20px;
  transform-origin: center center;
  transition-duration: 0.2s;
  &[aria-expanded='true'] {
    transform: rotate(-180deg);
  }
}
</style>
