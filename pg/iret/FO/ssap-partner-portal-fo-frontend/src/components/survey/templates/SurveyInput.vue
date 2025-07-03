<template>
  <RootTemplate class="t-SurveyInput" pb-10 pt-8 mb-10>
    <SurveyInformation
      :survey="survey"
      :type="type"
      :is-anonymous="isAnonymous"
      :token="anonymousToken"
      @click:start="start"
    />
    <SurveyInputAnswer
      v-if="!isFinished && isStarted"
      :question="currentQuestion"
      :answer="currentAnswer"
      :other-answer="currentOtherAnswer"
      :total-pages="totalPages"
      :current-page="currentPage + skipPages"
      :survey-type="survey.surveyType ? survey.surveyType : ''"
      @click:next="next"
      @click:prev="prev"
      @click:submit="submit"
      @change="setAnswer"
      @change-other="setOtherInput"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import SurveyInformation from '~/components/survey/organisms/SurveyInformation.vue'
import SurveyInputAnswer from '~/components/survey/organisms/SurveyInputAnswer.vue'
import {
  SurveyInfo,
  SurveyMasterInfo,
  QuestionInfo,
  QuestionFlowInfo,
  QuestionChoiceGroupInfo,
  QuestionChoiceInfo,
  InputAnswerInfo,
  UpdateSurveyByIdRequest,
} from '~/models/Survey'

interface DataType {
  currentQuestionFlowIndex: number
  isFinished: boolean
  answers: Array<InputAnswerInfo>
  currentPage: number
  questions: Array<QuestionInfo>
  questionFlow: Array<QuestionFlowInfo>
  skipPages: number
}
export default BaseComponent.extend({
  components: {
    RootTemplate,
    SurveyInformation,
    SurveyInputAnswer,
  },
  head() {
    return {
      title: this.$t('survey.pages.input.name') as string,
    }
  },
  props: {
    /** 案件アンケート */
    survey: {
      type: Object as PropType<SurveyInfo>,
      default: new SurveyInfo(),
    },
    /** 最新バージョンのアンケートひな形 */
    surveyMaster: {
      type: Object as PropType<SurveyMasterInfo>,
      default: new SurveyMasterInfo(),
    },
    /** 匿名アンケートか否か */
    isAnonymous: {
      type: Boolean,
      default: false,
    },
    /** 匿名アンケートの場合に使用する認証トークン */
    token: {
      type: String,
      default: '',
    },
  },
  data(): DataType {
    return {
      currentQuestionFlowIndex: -1, // -1の時、未開始
      isFinished: false,
      answers: [],
      currentPage: 1,
      questions: [],
      questionFlow: [],
      skipPages: 0,
    }
  },
  computed: {
    /**
     * アンケート開始チェック
     * @returns 開始であればtrue
     */
    isStarted(): boolean {
      return this.currentQuestionFlowIndex > -1
    },
    /**
     * @returns アンケート設問
     */
    currentQuestion(): QuestionInfo {
      return this.questions[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ]
    },
    /**
     * @returns アンケート回答
     */
    currentAnswer(): string {
      return this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].answer
    },
    /**
     * @returns アンケート回答
     */
    currentOtherAnswer(): string {
      return this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].otherAnswer
    },
    /**
     * 案件アンケートの回答状況のチェック
     */
    type() {
      if (!this.isFinished) {
        if (!this.isStarted) {
          // 回答開始前
          return 'input-before-start'
        } else if (!this.isFinished && this.isStarted) {
          // 回答入力中
          return 'input'
        }
      } else {
        // 回答完了
        return 'finished'
      }
    },

    totalPages(): number {
      // 設問フローを基準にカウントする
      // 条件参照元が同じ設問は同一の設問としてカウントする

      // 条件なしの設問数カウント
      const noConditionCount = this.questionFlow.filter(
        (flow: QuestionFlowInfo) => {
          return flow.conditionId === ''
        }
      ).length

      // 条件ありの設問の条件参照元抽出
      const conditionIds = this.questionFlow
        .filter((flow: QuestionFlowInfo) => {
          return flow.conditionId !== ''
        })
        .map((flow: QuestionFlowInfo) => {
          return flow.conditionId
        })

      // 条件参照元の重複しない設問数
      const conditionCount = Array.from(new Set(conditionIds)).length

      return noConditionCount + conditionCount
    },
    /**
     * 匿名アンケートの場合にトークンを返す
     */
    anonymousToken(): string {
      return this.isAnonymous ? this.token : ''
    },
  },
  methods: {
    /**
     * 回答開始時の初期設定
     */
    start() {
      this.currentQuestionFlowIndex = 0
      this.currentPage = 1

      // 1問目を回答送信対象に設定
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].isAnswered = true
    },
    /**
     * 次の設問を設定
     */
    next() {
      const tmp = this.currentQuestionFlowIndex

      // 現在の設問フロー+1以降のフローを参照して表示対象か確認する
      let questionFlowIndex = this.currentQuestionFlowIndex + 1
      while (questionFlowIndex < this.questionFlow.length) {
        // 条件参照して表示対象か確認
        if (this.checkFlowCondition(this.questionFlow[questionFlowIndex])) {
          // 次ページ情報設定
          this.currentQuestionFlowIndex = questionFlowIndex
          this.currentPage++

          // prev先は現answerに依らないため、next時に現indexを履歴としてprev先を設置
          this.questionFlow[this.currentQuestionFlowIndex].prevIndex = tmp

          // 次の設問を回答送信対象に設定
          this.answers[
            this.questionFlow[this.currentQuestionFlowIndex].questionIndex
          ].isAnswered = true

          // 回答によってスキップされた設問があるかチェック
          this.calcSkipPages()

          return
        } else {
          questionFlowIndex++
        }
      }
      // 該当する設問がない場合は回答送信
      // 設問の回答によって残りの設問の表示条件をすべて満たさなくなる場合
      this.submit()
    },
    /**
     * 設問が表示対象か確認
     * @param flow 出題フロー
     */
    checkFlowCondition(flow: QuestionFlowInfo): boolean {
      // 設問が表示対象か確認する
      if (flow.conditionId === '') {
        // 条件設定がない場合は表示対象
        return true
      } else {
        // 条件設定がある場合は回答情報を参照
        // 回答選択肢の中に一致する条件の選択肢があれば表示対象
        const answerIndex = this.questions.findIndex(
          (question: QuestionInfo) => {
            return question.id === flow.conditionId
          }
        )
        const isMatch = flow.conditionChoiceIds.map(
          (conditionChoiceId: string) =>
            this.answers[answerIndex].answer
              .split(',')
              .includes(conditionChoiceId)
        )
        if (isMatch.includes(true)) {
          return true
        }
        return false
      }
    },
    /**
     * 条件ありの設問で選択肢によって表示がスキップされた設問数をカウントする
     * 現在の回答ページ数の計算に使用する
     * 現在の回答ページ数=回答済みの設問数+選択肢によってスキップされた設問数
     */
    calcSkipPages() {
      let skipCount = 0

      // 条件ありの設問の重複しない参照元抽出
      const conditionIds = Array.from(
        new Set(
          this.questionFlow
            .filter((flow: QuestionFlowInfo) => {
              return flow.conditionId !== ''
            })
            .map((flow: QuestionFlowInfo) => {
              return flow.conditionId
            })
        )
      )

      // 参照元の設問ごとにカウント
      conditionIds.forEach((id: string) => {
        // 対応する設問の出題フロー順取得
        const questionFlowIndex = this.questionFlow.findIndex(
          (flow: QuestionFlowInfo) => {
            return flow.id === id
          }
        )
        // 出題フローの位置が現在位置より後の場合はスキップ対象外
        if (questionFlowIndex >= this.currentQuestionFlowIndex) {
          return
        }

        // 対応する設問位置から回答を取得
        const answerIndex = this.questions.findIndex(
          (question: QuestionInfo) => {
            return question.id === id
          }
        )

        // 未回答の場合はスキップ対象外
        if (this.answers[answerIndex].answer === '') {
          return
        }

        // 回答済みの場合は選択肢に当てはまるか確認
        let isMatchAnswer: boolean = false
        const choiceIds = this.questionFlow.filter((flow: QuestionFlowInfo) => {
          return flow.conditionId === id
        })
        for (const choiceId of choiceIds) {
          const isMatch = choiceId.conditionChoiceIds.map(
            (conditionChoiceId: string) =>
              this.answers[answerIndex].answer
                .split(',')
                .includes(conditionChoiceId)
          )
          if (isMatch.includes(true)) {
            isMatchAnswer = true
            break
          }
        }
        // 当てはまる場合はスキップ数カウント
        if (!isMatchAnswer) {
          skipCount++
        }
      })

      this.skipPages = skipCount
    },
    /**
     * 前の設問に戻る
     */
    prev() {
      // 現在の回答情報を初期化
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].answer = ''
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].otherAnswer = ''
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].isAnswered = false

      if (
        this.questionFlow[this.currentQuestionFlowIndex].prevIndex !== undefined
      ) {
        // 2問目以降はprevIndexに格納されている設問フローに戻る
        this.currentQuestionFlowIndex =
          this.questionFlow[this.currentQuestionFlowIndex].prevIndex
      } else {
        // 1問目の場合は回答情報を初期化する
        if (this.currentQuestionFlowIndex === 0) {
          this.answers = Array(this.questions.length)
            .fill('')
            .map(() => {
              return {
                answer: '',
                otherAnswer: '',
                isAnswered: false,
              }
            })
        }
        this.currentQuestionFlowIndex--
      }
      this.currentPage--

      // 回答を戻ることでスキップされた設問が再び回答対象となることがあるためチェック
      this.calcSkipPages()
    },
    /**
     * 回答送信
     */
    submit() {
      const answers: any = []
      let isDisclosure = true
      const isFinished = true

      // 入力情報をリクエストの形式に変換
      this.answers.forEach((answer: InputAnswerInfo, index: number) => {
        // 回答対象外の設問はスキップ
        if (!answer.isAnswered) {
          return
        }

        // 支援者への開示の問いはisDisclosureに設定
        if (this.questions[index].isEnd) {
          if (answer.answer) {
            // 入力がある場合は開示しない
            isDisclosure = false
          }
          return
        }

        if (this.questions[index].format === 'radio') {
          // ラジオボタンの場合は選択idから選択肢情報を作成
          let choiceItem = new QuestionChoiceGroupInfo()
          this.questions[index].choices.forEach(
            (choice: QuestionChoiceInfo) => {
              const tmp = choice.group.find((item: QuestionChoiceGroupInfo) => {
                return item.id === answer.answer
              })
              if (tmp) {
                choiceItem = tmp
              }
            }
          )

          // 回答の文字列は選択肢の文字列から[[.*]]を除く
          // [[]]の中の値をpointとする
          const pointValue = choiceItem.title.match(/\[\[(.*)\]\]/)
          answers.push({
            id: this.questions[index].id,
            answer: choiceItem.title.replace(/\[\[.*\]\]/, ''),
            point: pointValue == null ? 0 : Number(pointValue[1]),
            choiceIds: answer.answer.split(','),
            summaryType: this.questions[index].summaryType,
            otherInput: answer.otherAnswer ? answer.otherAnswer : '',
          })
        } else if (this.questions[index].format === 'checkbox') {
          // チェックボックスの場合はidから選択肢情報を作成(idは複数あり)
          const choiceItems = answer.answer
            .split(',')
            .map((choiceId: string) => {
              let matchTitle = ''
              this.questions[index].choices.forEach(
                (choice: QuestionChoiceInfo) => {
                  const matchGroup = choice.group.find(
                    (item: QuestionChoiceGroupInfo) => {
                      return item.id === choiceId
                    }
                  )
                  if (matchGroup) {
                    matchTitle = matchGroup.title
                  }
                }
              )
              return matchTitle
            })
          // 回答の文字列は選択肢をカンマ区切りで結合する
          answers.push({
            id: this.questions[index].id,
            answer: choiceItems.join(','),
            choiceIds: answer.answer.split(','),
            otherInput: answer.otherAnswer ? answer.otherAnswer : '',
          })
        } else if (this.questions[index].format === 'textarea') {
          // テキストエリアの場合は入力されたテキストをそのまま設定
          answers.push({
            id: this.questions[index].id,
            answer: answer.answer,
            otherInput: answer.otherAnswer ? answer.otherAnswer : '',
          })
        }
      })

      // 親コンポーネントで回答送信
      const answerData: UpdateSurveyByIdRequest = {
        answers,
        isFinished,
        isDisclosure,
      }
      this.$emit('submit', answerData)
      this.isFinished = true
    },
    /**
     * 設問の特定のインデックス番号に回答を入力する
     * @param $event 回答
     */
    setAnswer($event: any) {
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].answer = Array.isArray($event) ? $event.join(',') : $event
    },
    /**
     * otherAnswerの回答を入力
     * @param $event 回答
     */
    setOtherInput($event: any) {
      this.answers[
        this.questionFlow[this.currentQuestionFlowIndex].questionIndex
      ].otherAnswer = $event
    },
    /**
     * 設問フィルター
     * 支援者への開示の問い追加
     */
    filteredQuestions() {
      this.questions = this.surveyMaster.questions
      this.questionFlow = this.surveyMaster.questionFlow
      const _this = this

      // 設問フローと設問の紐づけ
      this.questionFlow.forEach((flow: QuestionFlowInfo, index: number) => {
        _this.questionFlow[index].questionIndex = _this.questions.findIndex(
          (question: QuestionInfo) => {
            return question.id === flow.id
          }
        )
      })
      // 無効設問は除外
      _this.questionFlow = _this.questionFlow.filter(
        (flow: QuestionFlowInfo) => {
          return (
            _this.questions[flow.questionIndex] &&
            _this.questions[flow.questionIndex].disabled === false
          )
        }
      )
      // 支援者への開示の問い追加
      if (this.surveyMaster.isDisclosure) {
        _this.questions.push({
          id: '',
          required: true,
          description: this.$t(
            'survey.pages.common.disclosure.description'
          ) as string,
          format: 'checkbox',
          summaryType: '',
          choices: [
            {
              description: '',
              group: [
                {
                  id: this.$t(
                    'survey.pages.common.disclosure.choice'
                  ) as string,
                  title: this.$t(
                    'survey.pages.common.disclosure.choice'
                  ) as string,
                  disabled: false,
                  isNew: false,
                },
              ],
              isNew: false,
              options: [],
              optionValues: [],
            },
          ],
          otherDescription: '',
          disabled: false,
          isNew: false,
          isEnd: true,
        })
        _this.questionFlow.push({
          id: '',
          conditionId: '',
          conditionChoiceIds: [],
          questionIndex: _this.questions.length - 1,
          prevIndex: 0,
        })
        this.answers.push({
          answer: '',
          otherAnswer: '',
          isAnswered: false,
        })
      }

      this.questions.forEach((question: QuestionInfo, index: number) => {
        // ラジオボタンの場合の選択肢表示テキストと対応する値を設定する
        if (question.format === 'radio') {
          question.choices.forEach((choice: any, choiceIndex: number) => {
            // [[]]で囲われた部分は表示しない, 無効選択肢は除外
            _this.questions[index].choices[choiceIndex].options = choice.group
              .filter((item: QuestionChoiceGroupInfo) => {
                return !item.disabled
              })
              .map((item: QuestionChoiceGroupInfo) => {
                return item.title.replace(/\[\[.*\]\]/, '')
              })
            // 値は選択肢のidを設定する
            _this.questions[index].choices[choiceIndex].optionValues =
              choice.group
                .filter((item: QuestionChoiceGroupInfo) => {
                  return !item.disabled
                })
                .map((item: any) => {
                  return item.id
                })
          })
          // グループ内の全選択肢が無効の場合は表示対象外
          _this.questions[index].choices = _this.questions[
            index
          ].choices.filter((item: QuestionChoiceInfo) => {
            return item.options.length !== 0
          })
        } else if (question.format === 'checkbox') {
          // チェックボックスの場合は無効選択肢の除外のみ実施
          question.choices.forEach((choice: any, choiceIndex: number) => {
            _this.questions[index].choices[choiceIndex].group =
              choice.group.filter((item: QuestionChoiceGroupInfo) => {
                return !item.disabled
              })
          })
          // グループ内の全選択肢が無効の場合は表示対象外
          _this.questions[index].choices = _this.questions[
            index
          ].choices.filter((item: QuestionChoiceInfo) => {
            return item.group.length !== 0
          })
        }
      })
    },
  },
  mounted() {
    /** 回答情報初期化 */
    this.answers = Array(this.surveyMaster.questions.length)
      .fill('')
      .map(() => {
        return {
          answer: '',
          otherAnswer: '',
          isAnswered: false,
        }
      })
    /** 設問情報初期化 */
    this.filteredQuestions()
  },
})
</script>

<style lang="scss">
.t-SurveyInput {
  .a-checkbox {
    .v-label {
      @include fontSize('normal');
    }
  }
  .a-radio {
    .a-radio__text {
      @include fontSize('normal');
    }
  }
}
</style>
