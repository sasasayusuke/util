<template>
  <div class="m-header-front-office">
    <v-container pa-0 class="m-header-front-office__layout">
      <v-row no-gutters>
        <v-col cols="3">
          <h1 v-if="isCurrent('/home')">
            <PartnerPortalFrontOffice />
          </h1>
          <PartnerPortalFrontOffice v-else />
        </v-col>
        <v-col cols="7" align-self="center" class="pl-6">
          <template v-for="button in buttons">
            <Button
              v-if="isAccessible(button.to)"
              :key="button.pathPrefix"
              style-set="global-header"
              :class="{ 'is-current': isCurrent(button.pathPrefix) }"
              :to="button.to"
            >
              {{ $t(`header.global.menu.${button.pathPrefix.slice(1)}`) }}
            </Button>
          </template>
        </v-col>
        <v-col cols="2" class="d-flex justify-end" align-self="center">
          <ButtonAlarm
            v-if="
              !(
                isCurrent('/anonymous-survey') ||
                isCurrent('/satisfaction-survey') ||
                isCurrent('/solver')
              )
            "
            class="mr-5"
            :notification-list="notificationList"
            @click:toggle="toggleNotifications"
            @clickNotificationIcon="clickNotificationIcon"
          />
          <ButtonProfile
            v-if="
              !(
                isCurrent('/anonymous-survey') ||
                isCurrent('/satisfaction-survey')
              )
            "
          />
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import PartnerPortalFrontOffice from '~/components/header/atoms/PartnerPortalFrontOffice.vue'
import ButtonAlarm from '~/components/header/molecules/ButtonAlarm.vue'
import ButtonProfile from '~/components/header/molecules/ButtonProfile.vue'
import { meStore } from '~/store'
import { hasRole, getAllowedRoles } from '~/utils/role-authorizer'
import { GetNotificationsResponse } from '~/models/Notification'
import { getCurrentDate } from '~/utils/common-functions'

interface IButton {
  pathPrefix: string
  to: string
}
// 現在の年月を取得
const thisYear = getCurrentDate().getFullYear()
const thisMonth = getCurrentDate().getMonth() + 1

const buttons: IButton[] = [
  { pathPrefix: '/home', to: '/home' },
  { pathPrefix: '/project', to: '/project/list' },
  {
    pathPrefix: '/man-hour',
    to: `/man-hour?year=${thisYear}&month=${thisMonth}`,
  },
  { pathPrefix: '/customer', to: '/customer/list' },
  { pathPrefix: '/survey', to: '/survey/admin/list' },
]

export default BaseComponent.extend({
  components: {
    Button,
    PartnerPortalFrontOffice,
    ButtonAlarm,
    ButtonProfile,
  },
  props: {
    notificationList: {
      type: Array as PropType<GetNotificationsResponse>,
    },
  },
  data() {
    return {
      buttons,
    }
  },
  created() {
    meStore.toggleNotifications(false)
  },
  methods: {
    // storeの状態を変更することで、お知らせ一覧を表示する
    toggleNotifications() {
      const state = !this.$store.state.me.showNotifications
      meStore.toggleNotifications(state)
    },
    clickNotificationIcon() {
      this.$emit('clickNotificationIcon')
    },
    isCurrent(path: string): boolean {
      let currentPath = this.$route.path

      if (currentPath.startsWith('/survey')) {
        if (
          !currentPath.startsWith('/survey/pp') &&
          !currentPath.startsWith('/survey/admin')
        ) {
          currentPath = '/project'
        }
      }
      if (currentPath === '/project/list/me') {
        // /project/list/meは例外として/survey下として扱う？
        currentPath = '/survey'
      }
      if (
        currentPath.startsWith('/karte') ||
        currentPath.startsWith('/master-karte')
      ) {
        currentPath = '/project'
      }

      if (currentPath.startsWith('/solver')) {
        currentPath = '/solver'
      }

      return currentPath.startsWith(path)
    },
    isAccessible(path: string): boolean {
      // ログインユーザーがアライアンス担当 or 法人ソルバー担当 且つ pathが"/home"の場合はfalseを返す
      if (
        (meStore.role === 'apt' || meStore.role === 'solver_staff') &&
        path === '/home'
      ) {
        return false
      }
      const allowedRoles = getAllowedRoles(path)
      return hasRole(allowedRoles)
    },
  },
})
</script>

<style lang="scss" scoped>
.m-header-front-office {
  padding: 0 25px;
  height: 90px;
  background-color: $c-white;
  display: flex;
  align-items: center;
  box-shadow: 0px 2px 4px -1px rgb(0 0 0 / 20%),
    0px 4px 5px 0px rgb(0 0 0 / 14%), 0px 1px 10px -10px rgb(0 0 0 / 12%) !important;
}
.m-header-front-office__layout {
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
          font-weight: bold;
          color: $c-primary-dark !important;
        }
      }
    }
  }
}
</style>
