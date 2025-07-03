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
      <Icon size="36" class="m-button-alarm__icon--out" color="#fff"
        >icon-org-bell-outline</Icon
      >
      <Icon size="36" class="m-button-alarm__icon--in" color="#fff"
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
    <Icon size="36" class="m-button-alarm__icon--out" color="#fff"
      >icon-org-bell-outline</Icon
    >
    <Icon size="36" class="m-button-alarm__icon--in" color="#fff"
      >icon-org-bell</Icon
    >
  </Button>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon } from '~/components/common/atoms/index'
import type { PropType } from '~/common/BaseComponent'
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
    //お知らせ一覧
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
    /**
     *未読のお知らせを抽出
     * @return 未読のお知らせ
     */
    unreadNotifications(): Notification[] {
      return this.notificationList.filter((elm: any) => {
        return elm.confirmed === false
      })
    },
    /**
     * アラームボタンの通知数字の表示方法を切り替える
     * 未読数100以上 → 「99+」と表示
     * 未読数0 → 非表示
     * @return アラームボタンの通知数
     */
    content(): string {
      if (!this.showBadgeIcon) {
        return ''
      }
      const unreadNotificationLength = this.unreadNotifications.length
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
    /**
     * 自身の管理ユーザーIDに紐づく現在の未読お知らせ情報を全て既読に更新
     */
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
    }
    .m-button-alarm__icon--out {
      opacity: 0;
    }
  }
}
.m-button-alarm__text {
  visibility: hidden;
}
.m-button-alarm__icon {
  position: absolute;
  &--in {
    @extend .m-button-alarm__icon;
    opacity: 0;
    background-color: $c-primary-over;
  }
  &--out {
    @extend .m-button-alarm__icon;
  }
}
</style>
