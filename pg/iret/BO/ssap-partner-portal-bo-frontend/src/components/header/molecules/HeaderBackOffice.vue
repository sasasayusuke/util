<template>
  <div class="m-header-backoffice">
    <v-layout class="m-header-backoffice__layout">
      <v-flex class="d-flex align-center">
        <PartnerPortalBackOffice />
      </v-flex>
      <v-flex class="px-15" align-self-center="true">
        <Button
          style-set="global-header"
          :class="{ 'is-current': checkURL('home') }"
          to="/home"
        >
          {{ $t('header.global.menu.home') }}
        </Button>
        <Button
          style-set="global-header"
          :class="{
            'is-current': checkURL('karte') || checkURL('master-karte'),
          }"
          to="/karte/list"
        >
          {{ $t('header.global.menu.karte') }}
        </Button>
        <Button
          v-if="headerList.includes('man-hour')"
          style-set="global-header"
          :class="{ 'is-current': checkURL('man-hour') }"
          to="/man-hour"
        >
          {{ $t('header.global.menu.man-hours') }}
        </Button>
        <Button
          v-if="headerList.includes('survey')"
          style-set="global-header"
          :class="{ 'is-current': checkURL('survey') }"
          to="/survey"
        >
          {{ $t('header.global.menu.survey') }}
        </Button>
        <Button
          style-set="global-header"
          :class="{ 'is-current': checkURL('customer') }"
          to="/customer/list"
        >
          {{ $t('header.global.menu.customer') }}
        </Button>
        <Button
          style-set="global-header"
          :class="{ 'is-current': checkURL('project') }"
          to="/project/list"
        >
          {{ $t('header.global.menu.matter') }}
        </Button>
        <ButtonOthers
          :is-current="checkURL('other')"
          :header-list="headerList"
        />
      </v-flex>
      <v-flex class="d-flex justify-end" align-self-center="true">
        <ButtonAlarm
          class="mr-5"
          :notification-list="notificationList"
          @click:toggle="toggleNotifications"
          @clickNotificationIcon="clickNotificationIcon"
        />
        <ButtonProfile />
      </v-flex>
    </v-layout>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import PartnerPortalBackOffice from '~/components/header/atoms/PartnerPortalBackOffice.vue'
import ButtonOthers from '~/components/header/molecules/ButtonOthers.vue'
import ButtonAlarm from '~/components/header/molecules/ButtonAlarm.vue'
import ButtonProfile from '~/components/header/molecules/ButtonProfile.vue'
import { meStore } from '~/store'
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

const headerItemRoleAuthority: {
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
  components: {
    Button,
    PartnerPortalBackOffice,
    ButtonOthers,
    ButtonAlarm,
    ButtonProfile,
  },
  data() {
    return {
      other: ['admin', 'user', 'master'],
    }
  },
  props: {
    //お知らせ情報
    notificationList: {
      type: Array,
    },
  },
  methods: {
    /**
     * storeの状態を変更することで、お知らせ一覧を表示する
     */
    toggleNotifications() {
      const state = !this.$store.state.me.showNotifications
      meStore.toggleNotifications(state)
    },
    /**
     * ルーティングから現在表示されているパスを判断
     * @param current 各メニューのパス名
     * @return 現在表示されているパスはtrueを返す
     */
    checkURL(current: string) {
      const path = this.$route.path.replace(/^\//, '').split('/')[0]
      return current === 'other' ? this.other.includes(path) : current === path
    },
    /**
     * お知らせボタンの開閉
     */
    clickNotificationIcon() {
      this.$emit('clickNotificationIcon')
    },
  },
  computed: {
    /**
     * headerItemRoleAuthorityのキーからパスを取得
     * @return ヘッダーメニューのパス
     */
    headerList() {
      const headerList: Array<string> = []
      Object.keys(headerItemRoleAuthority).forEach((itemName) => {
        if (hasRole(headerItemRoleAuthority[itemName])) {
          headerList.push(itemName)
        }
      })
      return headerList
    },
  },
})
</script>

<style lang="scss">
.m-header-backoffice {
  height: 90px;
  background-color: $c-primary-dark;
  display: flex;
  align-items: center;
  padding: 0 25px;
}
.m-header-backoffice__layout {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0;
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
}
</style>
