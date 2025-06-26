<template>
  <RootTemPlate :class="'fill-height align-start'">
    <v-container>
      <v-layout>
        <v-flex align-self-center>
          <h1 class="text-h5 font-weight-bold text-center">
            {{ $t('top.pages.home.title') }}
          </h1>
        </v-flex>
      </v-layout>
      <v-layout class="pt-7">
        <v-flex align-self-center>
          <HomeMenu :menu-items="menuItems" />
        </v-flex>
      </v-layout>
    </v-container>
  </RootTemPlate>
</template>

<script lang="ts">
import type { MenuItem } from '../organisms/HomeMenu.vue'
import HomeMenu from '../organisms/HomeMenu.vue'
import { Button } from '~/components/common/atoms/index'
import TopCard from '~/components/top/organisms/TopCard.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { hasRole, ADMIN_ROLE } from '~/utils/role-authorizer'

const allRoles = [
  ADMIN_ROLE.SALES,
  ADMIN_ROLE.SUPPORTER_MGR,
  ADMIN_ROLE.SALES_MGR,
  ADMIN_ROLE.SURVEY_OPS,
  ADMIN_ROLE.MAN_HOUR_OPS,
  ADMIN_ROLE.SYSTEM_ADMIN,
  ADMIN_ROLE.BUSINESS_MGR,
]

const menuItemRoleAuthority: {
  [key: string]: Array<string>
} = {
  karte: allRoles,
  'man-hour': [
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.MAN_HOUR_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  survey: [
    ADMIN_ROLE.SALES,
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SURVEY_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  customer: allRoles,
  project: allRoles,
  admin: [ADMIN_ROLE.SYSTEM_ADMIN],
  user: allRoles,
  master: [
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SURVEY_OPS,
    ADMIN_ROLE.MAN_HOUR_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
}

export default BaseComponent.extend({
  name: 'HomeTemplate',
  components: {
    HomeMenu,
    Button,
    TopCard,
    RootTemPlate,
  },
  computed: {
    /**
     * メニュー情報
     * @returns メニュー情報
     */
    menuItems(): MenuItem[] {
      const rtn: MenuItem[] = []
      Object.keys(menuItemRoleAuthority).forEach((itemName) => {
        if (hasRole(menuItemRoleAuthority[itemName])) {
          rtn.push(this.createMenuItem(itemName))
        }
      })

      return rtn
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * メニュー情報を生成する
     * @param メニューを出し分けするためのグループ名
     * @returns メニュー情報
     *
     */
    createMenuItem(groupName: string): MenuItem {
      // HACK: toとiconはlang.jsonに格納すべきでない
      const rtn: MenuItem = {
        icon: this.$t(`${groupName}.group_info.icon`).toString(),
        title: this.$t(`${groupName}.group_info.name`).toString(),
        text: this.$t(`${groupName}.group_info.description`).toString(),
        to: this.$t(`${groupName}.group_info.index`).toString(),
      }

      return rtn
    },
  },
})
</script>
