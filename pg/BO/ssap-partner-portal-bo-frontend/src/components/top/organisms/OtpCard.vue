<template>
  <v-container pt-7 pb-0>
    <v-layout class="justify-center">
      <v-card elevation="4" class="pt-10 pb-12" width="900">
        <v-layout class="justify-center">
          <v-card-title class="py-0">
            <h1 class="text-h5 font-weight-bold">
              {{ $t('top.pages.otp.title') }}
            </h1>
          </v-card-title>
        </v-layout>
        <v-layout class="justify-center">
          <v-card-actions class="py-0 px-0">
            <OtpStepper
              :is-sending-mail="isSendingMail"
              :is-authorizing="isAuthorizing"
              @generateOtp="onClickGenerateOtp"
              @next="onClickNext"
            />
          </v-card-actions>
        </v-layout>
      </v-card>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import OtpStepper from '../molecules/OtpStepper.vue'
import BaseComponent from '~/common/BaseComponent'
import { Button, TextField } from '~/components/common/atoms/index'
import { meStore, errorBarStore, redirectStore } from '~/store'
import { GetAdminByMineResponse } from '~/models/Admin'
import { ForbiddenError, UnauthorizedError } from '~/plugins/api'
import { getAllowedRoles } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  components: {
    TextField,
    Button,
    OtpStepper,
  },
  data() {
    return {
      isSendingMail: false,
      isAuthorizing: false,
    }
  },
  /**
   * リダイレクトURLがセットされている場合はパラメータを付加
   */
  beforeCreate() {
    const encodedRedirectUrl = redirectStore.url
    redirectStore.clearUrl()
    if (encodedRedirectUrl) {
      const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
      const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
      const allowedRoles = getAllowedRoles(decodedRedirectPath)
      if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
        this.$router.push(`/otp?redirect_url=${encodedRedirectUrl}`)
      }
    }
  },
  methods: {
    /**
     * エラーメッセージをエラーバーにセットする
     */
    setErrorMessage() {
      // エラーバーにOTP間違いエラーメッセージ設定
      errorBarStore.setMessage(
        this.$t('top.pages.otp.errorMessages.wrongPassword') as string
      )
    },
    /**
     * Sony Acceleration Platform公式サイトから取得したJWTトークンからユーザ情報を取り出し、そのユーザに対してメールでワンタイムパスワードを送信
     */
    async onClickGenerateOtp() {
      this.clearErrorBar()
      this.isSendingMail = true

      // OTPが含まれるメールを発行する
      await this.$api
        .getOtpToken()
        .then((res) => {
          this.$logger.info(res.data)
        })
        .catch((error: Error) => {
          if (
            error instanceof ForbiddenError &&
            error.message === 'Forbidden'
          ) {
            this.showErrorBarWithScrollPageTop(
              this.$t('top.pages.otp.errorMessages.invalidAccount')
            )
          } else if (
            error instanceof UnauthorizedError &&
            error.message === 'Auth user not found.'
          ) {
            this.showErrorBarWithScrollPageTop(
              this.$t('top.pages.otp.errorMessages.userNotFound')
            )
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
        })
        .finally(() => {
          this.isSendingMail = false
        })
    },
    /**
     * ユーザが入力したワンタイムパスワードの妥当性が正しい場合、ユーザに対してトークンを払い出す
     * @param ワンタイムパスワード
     */
    async onClickNext(otp: string) {
      this.clearErrorBar()
      this.isAuthorizing = true

      // OTPトークンを取得する
      await this.$api
        .postOtpToken(otp)
        .then((res) => {
          this.$logger.info(res.data)

          // エラーバーを削除する
          errorBarStore.clear()

          // OTPトークン取得
          meStore.addOTP(res.data)
        })
        .catch((error: Error) => {
          // エラーバーを表示する
          this.setErrorMessage()
          this.isAuthorizing = false
          if (
            error instanceof ForbiddenError &&
            error.message === 'Forbidden'
          ) {
            this.showErrorBarWithScrollPageTop(
              this.$t('top.pages.otp.errorMessages.invalidAccount')
            )
          } else if (
            error instanceof UnauthorizedError &&
            error.message === 'Auth user not found.'
          ) {
            this.showErrorBarWithScrollPageTop(
              this.$t('top.pages.otp.errorMessages.userNotFound')
            )
          } else {
            this.showErrorBarWithScrollPageTop(
              this.$t('top.pages.otp.errorMessages.wrongPassword')
            )
          }
        })
      // OTPトークンの取得に失敗していたらこの時点で終了
      if (!this.isAuthorizing) {
        return !errorBarStore.checked
      }

      // ユーザー情報を取得する
      await this.$api
        .get<GetAdminByMineResponse>(`/admins/me`)
        .then((res) => {
          this.$logger.info(res.data)

          // storeに格納
          meStore.setResponse(res.data)

          // リダイレクトURLがセットされている場合は該当ページにリダイレクト
          if (
            this.$route.query.redirect_url &&
            this.$route.query.redirect_url !== ''
          ) {
            const encodedRedirectUrl = String(this.$route.query.redirect_url)
            const decodedRedirectUrl = decodeURIComponent(encodedRedirectUrl)
            const decodedRedirectPath = decodedRedirectUrl.split('?')[0]
            const allowedRoles = getAllowedRoles(decodedRedirectPath)
            if (
              allowedRoles &&
              allowedRoles.length &&
              allowedRoles.length > 0
            ) {
              this.$router.push(decodedRedirectUrl)
            } else {
              this.$router.push('/home')
            }
          } else {
            this.$router.push('/home')
          }
        })
        .catch((err) => {
          this.showErrorBarWithScrollPageTop(
            this.$t('top.pages.otp.errorMessages.wrongPassword')
          )
          throw err
        })
        .finally(() => {
          this.isAuthorizing = false
        })
    },
  },
})
</script>

<style lang="scss" scoped>
.o-otp__body {
  position: relative;
  padding-top: 40px;
}
.o-otp__unit {
  position: relative;
  width: calc(532px + (95px * 2));
  padding: 0 95px;
  p {
    white-space: nowrap;
  }
  &:nth-child(n + 2) {
    padding-top: 84px;
    &::before {
      top: 60px;
    }
  }
  &::before {
    position: absolute;
    width: 70px;
    height: 70px;
    background-color: $c-primary-dark;
    border-radius: 50%;
    font-weight: bold;
    font-size: 32px;
    color: $c-white;
    display: flex;
    justify-content: center;
    align-items: center;
    top: -16px;
    left: 0;
    z-index: 2;
  }
  &::after {
    content: '';
    display: block;
    width: 2px;
    height: 100%;
    position: absolute;
    top: 0;
    left: 35px;
    background-color: $c-primary-dark;
  }
  @for $i from 1 through 2 {
    &:nth-child(#{$i}) {
      &::before {
        content: '#{$i}';
      }
    }
  }
}
</style>
