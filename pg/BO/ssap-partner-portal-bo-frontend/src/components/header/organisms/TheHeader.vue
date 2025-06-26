<template>
  <header role="banner" class="o-header">
    <SonyHeader />
    <HeaderBackOffice v-if="type >= 2" :notification-list="notificationList" />
    <PageHeader v-if="type >= 3" />
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
import HeaderBackOffice from '~/components/header/molecules/HeaderBackOffice.vue'
import PageHeader from '~/components/header/molecules/PageHeader.vue'
import Notification from '~/components/notification/organisms/NotificationContainer.vue'
import {
  GetNotifications,
  GetNotificationsResponse,
} from '@/models/Notification'

export default BaseComponent.extend({
  components: {
    SonyHeader,
    HeaderBackOffice,
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
    this.GetNotifications()
  },
  watch: {
    $route() {
      this.GetNotifications()
    },
  },
  methods: {
    /**
     * 自身の管理ユーザーIDに紐づくお知らせ情報を取得
     */
    async GetNotifications() {
      await GetNotifications().then((res) => {
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
