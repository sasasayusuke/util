<template>
  <v-badge v-if="content" overlap :content="content" color="#D53030">
    <Button
      class="m-button-alarm"
      icon
      width="36"
      height="36"
      :title="$t('common.button.notification')"
      @click="
        $emit('click:toggle')
        clickNotificationIcon()
      "
    >
      <Icon size="36" class="m-button-alarm__icon--out" color="btn_primary"
        >icon-org-bell-outline</Icon
      >
      <Icon size="36" class="m-button-alarm__icon--in" color="btn_primary"
        >icon-org-bell</Icon
      >
    </Button>
  </v-badge>
  <Button
    v-else
    class="m-button-alarm"
    icon
    width="36"
    height="36"
    :title="$t('common.button.notification')"
    @click="
      $emit('click:toggle')
      clickNotificationIcon()
    "
  >
    <Icon size="36" class="m-button-alarm__icon--out" color="btn_primary"
      >icon-org-bell-outline</Icon
    >
    <Icon size="36" class="m-button-alarm__icon--in" color="btn_primary"
      >icon-org-bell</Icon
    >
  </Button>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Button, Icon } from '~/components/common/atoms/index'
import {
  GetNotificationsResponse,
  ConfirmNotifications,
} from '@/models/Notification'
import { Notification } from '@/types/Notification'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
  },
  props: {
    notificationList: {
      type: Array as PropType<GetNotificationsResponse>,
    },
  },
  data() {
    return {
      showBadgeIcon: true,
    }
  },
  computed: {
    unreadNotifications(): Notification[] {
      return this.notificationList.filter((Notification: Notification) => {
        return Notification.confirmed === false
      })
    },
    content(): string {
      if (!this.showBadgeIcon) {
        return ''
      }

      const unreadNotificationLength = this.unreadNotifications.length

      // 未読数が100以上の場合は99+と表示する
      // 未読数が0の場合はバッジが非表示になる
      if (unreadNotificationLength >= 100) {
        return '99+'
      } else if (unreadNotificationLength === 0) {
        return ''
      } else {
        return unreadNotificationLength.toString()
      }
    },
  },
  methods: {
    // 自身の管理ユーザーIDに紐づく現在の未読お知らせ情報を全て既読に更新
    async clickNotificationIcon() {
      await ConfirmNotifications()
        .then(() => {
          this.$logger.info('confirmed notifications')
        })
        .catch((err) => {
          this.$logger.error(err)
        })
      this.showBadgeIcon = false

      this.$emit('clickNotificationIcon')
    },
  },
})
</script>

<style lang="scss" scoped>
.m-button-alarm {
  position: relative;
  &:hover,
  &:focus {
    .m-button-alarm__icon--in {
      opacity: 1;
      background-color: white;
    }
    .m-button-alarm__icon--out {
      opacity: 0;
    }
  }
}
.m-button-alarm__icon {
  position: absolute;
  &--in {
    @extend .m-button-alarm__icon;
    opacity: 0;
  }
  &--out {
    @extend .m-button-alarm__icon;
  }
}
</style>
