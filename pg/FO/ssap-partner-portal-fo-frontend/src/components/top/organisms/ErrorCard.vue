<template>
  <CommonNoticeCard class="o-ErrorCard">
    <v-card-title class="pa-0">
      <h1 class="o-ErrorCard__title">{{ title }}</h1>
    </v-card-title>
    <div class="o-ErrorCard__body">
      <p class="o-ErrorCard__description">
        <span>{{ description }}</span>
      </p>
    </div>
    <v-card-actions v-if="!isHideButtons" class="o-ErrorCard__actions">
      <v-container fluid pa-0>
        <v-row no-gutters justify="center">
          <v-col cols="auto">
            <template v-if="isLogin">
              <Button
                v-if="isSolverStaffOrApt"
                style-set="large-primary"
                width="160"
                :to="path"
              >
                {{ $t('common.button.solverTop') }}
              </Button>
              <Button v-else style-set="large-primary" width="160" :to="path">
                {{ $t('common.button.home') }}
              </Button>
            </template>
            <template v-else>
              <Button style-set="large-primary" width="160" to="/">
                {{ $t('common.button.top') }}
              </Button>
            </template>
          </v-col>
        </v-row>
      </v-container>
    </v-card-actions>
  </CommonNoticeCard>
</template>

<script lang="ts">
import { Button } from '../../common/atoms/index'
import CommonNoticeCard from '../../common/organisms/CommonNoticeCard.vue'
import { meStore } from '~/store'
import BaseComponent from '~/common/BaseComponent'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    Button,
    CommonNoticeCard,
  },
  props: {
    /**
     * エラーページのタイトル
     */
    title: {
      type: String,
      required: true,
    },
    /**
     * エラーページの説明
     */
    description: {
      type: String,
      required: true,
    },
  },
  data(): {
    path: string
  } {
    return {
      path: '/home',
    }
  },
  created() {
    if (
      meStore.role === ENUM_USER_ROLE.APT ||
      meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
    ) {
      this.path = '/solver/menu'
    }
  },
  computed: {
    /**
     * ログインしているか否か
     */
    isLogin() {
      return !!meStore.id
    },
    isSolverStaffOrApt() {
      /**
       * ログインユーザーが法人ソルバー担当またはアライアンス担当であるか
       */
      return (
        meStore.role === ENUM_USER_ROLE.APT ||
        meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
      )
    },
    /**
     * ボタンを表示するか否か
     */
    isHideButtons(): boolean {
      return this.isSourcePath('/anonymous-survey')
    },
  },
  methods: {
    isSourcePath(path: string): boolean {
      let defaultPath = '/'
      if (this.isLogin) {
        // アライアンス担当・法人ソルバー担当の場合はソルバートップ、そうでない場合はHOME画面
        defaultPath = this.path
      }
      const sourcePath = this.backToUrl(defaultPath)
      return sourcePath.startsWith(path)
    },
  },
})
</script>

<style lang="scss" scoped>
.o-ErrorCard {
  padding: 60px 0;
  &.v-card {
    border-color: $c-primary-dark;
    border-width: 2px;
  }
  .o-ErrorCard__title {
    font-size: 1.425rem;
    font-weight: bold;
    color: $c-primary-dark;
    text-align: center;
    width: 100%;
    justify-content: center;
    padding: 0;
  }
}
.o-ErrorCard__body {
  text-align: center;
  p {
    margin-bottom: 0;
  }
  span {
    white-space: pre-wrap;
  }
}
.o-ErrorCard__description {
  @include fontSize('small');
  margin-top: 24px;
}
.o-ErrorCard__actions {
  padding: 40px 0 0;
}
</style>
