<template>
  <transition name="slidein">
    <Sheet
      v-if="$store.state.me.showNotifications"
      class="o-news"
      :class="setClass"
      color="#fff"
    >
      <v-container class="o-news__container" pa-0>
        <v-row class="o-news__header" no-gutters>
          <v-col cols="auto">
            <h2 class="o-news__title">お知らせ</h2>
          </v-col>
          <v-col cols="auto">
            <Icon size="24" color="#fff" @click="toggleNotifications"
              >mdi-window-close</Icon
            >
          </v-col>
        </v-row>
        <v-row class="o-news__body" no-gutters>
          <v-col>
            <ul class="o-news__list">
              <li
                v-for="item in notificationList"
                :key="item.id"
                class="o-news__list-item"
                :class="{ 'is-confirmed': item.confirmed }"
              >
                <component
                  :is="setListBlok(item.url)"
                  class="o-news__list-wrap"
                  :to="setURL(item.url)"
                  :href="setURL(item.url)"
                  :target="item.url.match(baseUrl) ? '_self' : '_blank'"
                >
                  <p class="o-news__date">
                    {{ formatDate(item.noticedAt) }}
                  </p>
                  <h3 class="o-news__summary">{{ item.summary }}</h3>
                  <p class="o-news__message">{{ item.message }}</p>
                  <NotificationIcon
                    v-if="item.url"
                    class="o-news__icon"
                    size="16"
                  >
                    {{
                      item.url.match(baseUrl)
                        ? 'icon-org-arrow-right'
                        : 'icon-org-blank'
                    }}
                  </NotificationIcon>
                </component>
              </li>
            </ul>
          </v-col>
        </v-row>
      </v-container>
    </Sheet>
  </transition>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Sheet, Icon } from '~/components/common/atoms/index'
import NotificationIcon from '~/components/notification/molecules/NotificationIcon.vue'
import { meStore } from '~/store'
import { GetNotificationsResponse } from '@/models/Notification'

export default BaseComponent.extend({
  name: 'NotificationContainer',
  components: { Sheet, Icon, NotificationIcon },
  computed: {
    setClass() {
      return this.type === 2 ? 'o-news--2' : 'o-news--3'
    },
  },
  props: {
    /** 画面の種類 */
    type: {
      type: Number,
      default: 1,
    },
    /** 通知一覧 */
    notificationList: {
      type: Array as PropType<GetNotificationsResponse>,
    },
  },
  data() {
    return {
      baseUrl: process.env.APP_URL as string,
    }
  },
  methods: {
    /**
     * 選択した通知のURLによって、タグを切り替える
     * @arg url 選択した通知のURL
     */
    setListBlok(url: string) {
      let tag = 'div' as string
      if (url) {
        tag = url.match(this.baseUrl) ? 'nuxt-link' : 'a'
      }
      return tag
    },
    /** 通知のON/OFFを切り替える */
    toggleNotifications() {
      const state = !this.$store.state.me.showNotifications
      meStore.toggleNotifications(state)
    },
    /** 日付のフォーマットを変更 */
    formatDate(noticedAt: string) {
      return format(new Date(noticedAt), 'yyyy/MM/dd HH:mm')
    },
    setURL(url: string) {
      return url.replace(this.baseUrl, '')
    },
  },
})
</script>

<style lang="scss" scoped>
.slidein-enter-active,
.slidein-leave-active {
  margin-right: 0;
  transition: 0.22s;
}
.slidein-enter,
.slidein-leave-to {
  margin-right: -500px;
}
.o-news {
  background-color: #eee;
  position: fixed;
  width: 500px;
  height: 100vh;
  right: 0;
  top: 0;
  z-index: 1000;
  box-shadow: 0 10px 16px rgba(#000000, 32%);
}
.o-news__container {
  height: 100%;
}
.o-news__header {
  background-color: $c-primary-over;
  justify-content: space-between;
  align-items: center;
  height: 48px;
  padding: 0 20px;
}
.o-news__title {
  color: $c-white;
  @include fontSize('large');
}
.o-news__body {
  height: calc(100% - 48px);
  overflow-y: auto;
}
.o-news__list {
  margin: 0;
  padding: 0;
}
.o-news__list-item {
  list-style: none;
  &:nth-child(n + 1) {
    border-top: 1px solid $c-gray-line;
  }
  &.is-confirmed {
    background-color: #e3e3e3;
  }
}
.o-news__list-wrap {
  display: block;
  position: relative;
  padding: 16px 32px 16px 20px;
  text-decoration: none;
}
a {
  &.o-news__list-wrap {
    transition-duration: 0.3s;
    &:hover {
      background-color: $c-primary-8;
    }
  }
}
.o-news__date {
  margin: 0;
  color: $c-black-60;
  @include fontSize('small');
  font-weight: bold;
}
.o-news__summary {
  margin: 4px 0 0 0;
  color: $c-black-80;
  @include fontSize('normal');
}
.o-news__message {
  white-space: pre-line;
  margin: 8px 0 0 0;
  color: $c-black-80;
  @include fontSize('small');
}
.o-news__icon {
  position: absolute;
  right: 0;
  bottom: 0;
}
</style>
