<template>
  <Sheet elevation="3" rounded width="1200" class="mt-6 pa-8" color="white">
    <SurveyAnswer
      :question="question"
      :answer="answer"
      :other-answer="otherAnswer"
      :current-page="currentPage"
      :survey-type="surveyType"
      :is-satisfaction-survey="isSatisfactionSurvey"
      @change="onChange"
      @change-other="$emit('change-other', $event)"
    />
    <v-row
      v-if="!isSatisfactionSurvey"
      class="pb-0 px-0 ma-0 pt-12 justify-center"
    >
      <!-- 前に戻るボタン -->
      <Button
        outlined
        style-set="large-primary"
        width="300px"
        :disabled="currentPage === 1"
        @click="$emit('click:prev')"
      >
        <v-layout justify-space-between>
          <Icon class="mr-2" size="14">icon-org-arrow-left</Icon>
          {{ $t('common.button.prevQuestion') }}
          <Icon size="14" style-set="transparent">icon-org-arrow-right</Icon>
        </v-layout>
      </Button>

      <div class="a-page-text">
        <span class="a-page-text__current">{{ currentPage }}</span> /
        {{ totalPages }} ページ
      </div>

      <!-- 提出するボタン(PP利用アンケート以外) -->
      <Button
        v-if="currentPage === totalPages && surveyType !== 'pp'"
        style-set="large-primary"
        width="300px"
        @click="next"
      >
        <v-layout justify-space-between>
          <Icon class="mr-2" size="14" style-set="transparent"
            >icon-org-arrow-left</Icon
          >
          {{ $t('common.button.submitSurvey') }}
          <Icon class="ml-2" size="14" color="white">icon-org-arrow-right</Icon>
        </v-layout>
      </Button>
      <!-- 提出するボタン(PP利用アンケート) -->
      <Button
        v-else-if="currentPage === totalPages && surveyType === 'pp'"
        style-set="large-primary"
        :disabled="question.required && !(answer.length > 0 || answeredFlg)"
        width="300px"
        @click="next"
      >
        <v-layout justify-space-between>
          <Icon class="mr-2" size="14" style-set="transparent"
            >icon-org-arrow-left</Icon
          >
          {{ $t('common.button.submitSurvey') }}
          <Icon class="ml-2" size="14" color="white">icon-org-arrow-right</Icon>
        </v-layout>
      </Button>
      <!-- 次の質問へボタン-->
      <Button
        v-else
        style-set="large-primary"
        width="300px"
        :disabled="question.required && !(answer.length > 0 || answeredFlg)"
        @click="next"
      >
        <v-layout justify-space-between>
          <Icon class="mr-2" size="14" style-set="transparent"
            >icon-org-arrow-left</Icon
          >
          {{ $t('common.button.nextQuestion') }}
          <Icon class="ml-2" size="14" color="white">icon-org-arrow-right</Icon>
        </v-layout>
      </Button>
    </v-row>
    <v-row v-else class="pb-0 px-0 ma-0 pt-12 justify-center">
      <Button
        style-set="large-primary satisfied-survey"
        width="300"
        :disabled="question.required && !(answer.length > 0 || answeredFlg)"
        @click="next"
      >
        <Icon class="ml-2" size="14" color="white"></Icon>
        {{ $t('common.button.submitSurvey') }}
        <Icon class="ml-2" size="14" color="white">icon-org-arrow-right</Icon>
      </Button>
    </v-row>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Button, Icon } from '~/components/common/atoms/index'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import SurveyAnswer from '~/components/survey/organisms/SurveyAnswer.vue'

export default BaseComponent.extend({
  components: {
    Sheet,
    Button,
    Icon,
    CommonContainer,
    SurveyAnswer,
  },
  props: {
    /** 設問 */
    question: {
      type: Object,
      default: {},
    },
    /** 回答 */
    answer: {
      type: String,
      default: '',
    },
    otherAnswer: {
      type: String,
      default: '',
    },
    /** 合計ページ */
    totalPages: {
      type: Number,
      default: 1,
    },
    /** 現在のページ */
    currentPage: {
      type: Number,
      default: 1,
    },
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
    /** 満足度評価のみ回答アンケートか */
    isSatisfactionSurvey: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    currentPage() {
      this.answeredFlg = false
    },
  },
  data() {
    return {
      answeredFlg: false,
    }
  },
  methods: {
    /**
     * 次のページ又は送信ボタン
     */
    next() {
      if (this.currentPage === this.totalPages) {
        this.$emit('click:submit')
      } else {
        this.$emit('click:next')
      }
    },
    /**
     * 回答を更新
     */
    onChange($event: any) {
      this.answeredFlg = !!$event.length
      this.$emit('change', $event)
    },
  },
})
</script>

<style lang="scss" scoped>
.a-page-text {
  width: 300px;
  font-size: 1rem;
  color: $c-black;
  text-align: center;
  &__current {
    font-size: 1.5rem;
    font-weight: bold;
  }
}
</style>
