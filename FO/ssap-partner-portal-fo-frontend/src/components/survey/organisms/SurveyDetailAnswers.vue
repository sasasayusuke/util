<template>
  <div class="o-survey-detail-answers pt-5">
    <div v-for="(questionIndex, index) in questionIndexFlow" :key="index">
      <SurveyAnswer
        v-if="questions[questionIndex]"
        :question="questions[questionIndex]"
        :answer="getAnswer(questionIndex)"
        :other-answer="getOtherAnswer(questionIndex)"
        :current-page="index + 1"
        :readonly="true"
        :type="1"
        :survey-type="surveyType"
      />
    </div>
    <SurveyAnswer
      v-if="Object.keys(disclosure).length > 0"
      :question="disclosure"
      :answer="getDisclosureAnswer()"
      :is-disclosure="true"
      :readonly="true"
      :type="1"
      :survey-type="surveyType"
    />
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import SurveyAnswer from '~/components/survey/organisms/SurveyAnswer.vue'
import { AnswerInfo, QuestionInfo } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    SurveyAnswer,
  },
  props: {
    type: {
      type: String,
      default: '',
    },
    /** 開示の有無 */
    isDisclosure: {
      type: Boolean,
      default: false,
    },
    /** 設問一覧 */
    questions: {
      type: Array as PropType<Array<QuestionInfo>>,
      default: [],
    },
    /** 開示設問 */
    disclosure: {
      type: Object,
      default: {},
    },
    /** 回答一覧 */
    answers: {
      type: Array as PropType<Array<AnswerInfo>>,
      default: [],
    },
    /** 設問インデックスの配列 */
    questionIndexFlow: {
      type: Array as PropType<Array<number>>,
      default: [],
    },
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
  },
  methods: {
    /**
     * インデックス番号の回答を取得
     * @param index インデックス番号
     */
    getAnswer(index: any) {
      const _this = this
      if (this.answers.length) {
        const result = this.answers.find(
          (answer: any) => answer.id === _this.questions[index].id
        )
        if (result) {
          if (
            _this.questions[index].format === 'radio' ||
            _this.questions[index].format === 'checkbox'
          ) {
            return result.choiceIds.join(',')
          } else {
            return result.answer
          }
        }
      }
      return ''
    },
    /**
     * インデックス番号の回答を取得
     * @param index インデックス番号
     */
    getOtherAnswer(index: any) {
      const _this = this
      if (this.answers.length) {
        const result = this.answers.find(
          (answer: any) => answer.id === _this.questions[index].id
        )
        if (result) {
          return result.otherInput
        }
      }
      return ''
    },
    /**
     * 開示の回答の取得
     */
    getDisclosureAnswer() {
      return this.isDisclosure
        ? ''
        : (this.$t('survey.pages.common.disclosure.choice') as string)
    },
  },
})
</script>
