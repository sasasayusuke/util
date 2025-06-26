<template>
  <CommonMenuList
    v-if="menuItems.length"
    v-slot="{ on, attrs }"
    :items="menuItems"
    class="m-button-profile__list"
  >
    <Button
      text
      tile
      color="#fff"
      :class="{ 'is-current': isCurrent }"
      v-bind="attrs"
      v-on="on"
    >
      {{ $t('header.global.menu.other') }}
      <Icon v-bind="attrs" size="20" class="m-button-others__arrow"
        >mdi-menu-down</Icon
      >
    </Button>
  </CommonMenuList>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon } from '~/components/common/atoms/index'
import CommonMenuList from '~/components/common/molecules/CommonMenuList.vue'

interface ButtonOthersMenuItems {
  label: string
  name: string
  to: string
}

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    CommonMenuList,
  },
  props: {
    //現在表示されているページの居場所の有無
    isCurrent: {
      type: Boolean,
      default: false,
    },
    //ヘッダーメニュー一覧
    headerList: {
      type: Array,
    },
  },
  computed: {
    /**
     * @return ドロップダウンメニュー
     */
    menuItems(): ButtonOthersMenuItems[] {
      if (this.headerList.includes('admin')) {
        return [
          {
            label: this.$t('admin.group_info.name') as string,
            name: this.$t('admin.group_info.name') as string,
            to: '/admin/list',
          },
          {
            label: this.$t('user.group_info.name') as string,
            name: this.$t('user.group_info.name') as string,
            to: '/user/list',
          },
          {
            label: this.$t('master.group_info.name') as string,
            name: this.$t('master.group_info.name') as string,
            to: '/master/list',
          },
        ]
      } else if (this.headerList.includes('master')) {
        return [
          {
            label: this.$t('user.group_info.name') as string,
            name: this.$t('user.group_info.name') as string,
            to: '/user/list',
          },
          {
            label: this.$t('master.group_info.name') as string,
            name: this.$t('master.group_info.name') as string,
            to: '/master/list',
          },
        ]
      } else {
        return [
          {
            label: this.$t('user.group_info.name') as string,
            name: this.$t('user.group_info.name') as string,
            to: '/user/list',
          },
        ]
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.a-button {
  &.v-btn {
    &.v-btn--text {
      font-weight: normal;
      &.is-current,
      &:hover,
      &:focus {
        color: $c-white !important;
        font-weight: bold;
      }
    }
  }
}
.m-button-others__arrow {
  position: absolute;
  right: -20px;
  transform-origin: center center;
  transition-duration: 0.2s;
  &[aria-expanded='true'] {
    transform: rotate(-180deg);
  }
}
</style>
