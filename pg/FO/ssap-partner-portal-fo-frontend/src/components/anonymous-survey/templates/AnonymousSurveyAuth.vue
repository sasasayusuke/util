<template>
  <RootTemplate>
    <AnonymousSurveyAuthCard
      :param="param"
      :is-loading="isLoading"
      @auth="auth"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import AnonymousSurveyAuthCard from '~/components/anonymous-survey/organisms/AnonymousSurveyAuthCard.vue'
import type { PropType } from '~/common/BaseComponent'
import { AnonymousSurveyAuthorized } from '~/store/anonymous-survey'
import {
  CheckSurveyByIdPassword,
  CheckSurveyByIdPasswordRequest,
} from '~/models/Survey'
import { anonymousSurveyStore } from '~/store'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    AnonymousSurveyAuthCard,
  },
  props: {
    /**
     * 匿名アンケートの認証情報
     */
    param: {
      type: Object as PropType<CheckSurveyByIdPasswordRequest>,
      required: true,
    },
    /**
     * GET値に含まれるJWT情報
     */
    token: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      isLoading: false,
    }
  },
  methods: {
    /**
     * パスワードの送信
     */
    auth(localParam: CheckSurveyByIdPasswordRequest) {
      this.clearErrorBar()
      this.isLoading = true

      const request = new CheckSurveyByIdPasswordRequest()
      Object.assign(request, localParam)
      request.token = this.token
      CheckSurveyByIdPassword(request)
        .then((res) => {
          const authorized = new AnonymousSurveyAuthorized()
          authorized.token = request.token
          authorized.password = request.password
          // 認証状態を認証中に変更
          anonymousSurveyStore.setAuthorized(authorized)
          const id = res.data.id
          this.isLoading = false
          // 取得したIDを用いて匿名アンケート入力/詳細画面に遷移
          this.$router.push(`/anonymous-survey/${id}`)
        })
        .catch(() => {
          this.isLoading = false
          const errorText = String(
            this.$t(
              'anonymous-survey.pages.password.errorMessages.invalidPassword'
            )
          )
            .replaceAll(/\n/g, '<br />')
            .replace(
              /(https?:\/\/[\w/:%#$&?()~.=+-]+)/gi,
              "<a href='$1' target='_blank'>$1</a>"
            )
          this.showErrorBarWithScrollPageTop(errorText)
        })
    },
  },
})
</script>

<style lang="scss" scoped></style>
