<template>
  <header role="banner" class="o-header">
    <SonyHeader />
    <HeaderFrontOffice
      v-if="type >= 2 && type !== 4 && type !== 5"
      :notification-list="notificationList"
    />
    <PageHeader v-if="type >= 3 && type !== 5" />
    <Notification
      :notification-list="notificationList"
      :type="type"
      @clickNotificationIcon="GetNotifications"
    />
  </header>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import SonyHeader from '~/components/header/molecules/SonyHeader.vue'
import HeaderFrontOffice from '~/components/header/molecules/HeaderFrontOffice.vue'
import PageHeader from '~/components/header/molecules/PageHeader.vue'
import Notification from '~/components/notification/organisms/NotificationContainer.vue'
import {
  GetNotifications,
  GetNotificationsResponse,
} from '@/models/Notification'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    SonyHeader,
    HeaderFrontOffice,
    PageHeader,
    Notification,
  },
  props: {
    type: {
      type: Number,
      default: 1,
    },
  },
  data() {
    return {
      notificationList: new GetNotificationsResponse(),
    }
  },
  created() {
    // 自身の管理ユーザーIDに紐づくお知らせ情報を取得
    if (!hasRole([ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF])) {
      this.GetNotifications()
    }
  },
  watch: {
    $route() {
      // 自身の管理ユーザーIDに紐づくお知らせ情報を取得
      this.GetNotifications()
    },
  },
  methods: {
    async GetNotifications() {
      await GetNotifications().then((res) => {
        this.$logger.info(res.data)
        this.notificationList = res.data
      })
    },
  },
})
</script>

<style lang="scss" scoped>
.o-header {
  position: relative;
  z-index: 2;
}
</style>
