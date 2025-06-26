<!--
  2022/11/02
  PPアンケート詳細画面でこのコンポーネントが使われておらず、SurveyDetail.vueが使われていた
  また、PPアンケート詳細にて権限によってはアクセスできない問題が発生していたため、
    1. SurveyDetail.vueの内容を全てコピー。
    2. 権限チェックに関係する記述を全て削除。
  という手順でこのコンポーネントを作成した。

  TODO: SurveyDetailコンポーネントとの記述重複を見直すリファクタリング
-->

<template>
  <RootTemplate pt-8 pb-10 mb-10>
    <SurveyDetailContainer
      :survey="survey"
      type="detail"
      :questions="questions"
      :disclosure="disclosure"
      :question-index-flow="questionIndexFlow"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import SurveyDetailContainer from '~/components/survey/organisms/SurveyDetailContainer.vue'
import {
  SurveyInfo,
  SurveyMasterInfo,
  QuestionInfo,
  AnswerInfo,
  QuestionFlowInfo,
  QuestionChoiceGroupInfo,
  QuestionChoiceInfo,
} from '~/models/Survey'

interface DataType {
  questions: Array<QuestionInfo>
  disclosure: Object
}

export default BaseComponent.extend({
  components: {
    RootTemplate,
    SurveyDetailContainer,
  },
  head() {
    return {
      title: this.$t('survey.pages.detail.name') as string,
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
  },
  watch: {
    surveyMaster: {
      handler(newVal, oldVal) {
        if (
          (newVal.questions &&
            oldVal &&
            newVal.questions.length !== oldVal.questions.length) ||
          (newVal.questions && !oldVal)
        ) {
          this.filteredQuestions()
        }
      },
      immediate: true,
    },
  },
  data(): DataType {
    return {
      questions: [] as QuestionInfo[],
      disclosure: {},
    }
  },
  computed: {
    /**
     * アンケートマスタと回答情報を突き合わせて回答情報を作成
     * アンケートマスタの出題フロー順に処理
     */
    questionIndexFlow(): number[] {
      const flowList: number[] = []
      if (this.surveyMaster.questionFlow) {
        this.surveyMaster.questionFlow.forEach(
          (questionFlow: QuestionFlowInfo) => {
            // フローに対応する設問情報参照
            const questionIndex = this.surveyMaster.questions.findIndex(
              (question: QuestionInfo) => question.id === questionFlow.id
            )
            // 設問がない場合は表示対象外
            if (questionIndex === -1) {
              return
            }
            // 設問が無効の場合は表示対象外
            if (this.surveyMaster.questions[questionIndex].disabled === true) {
              return
            }
            if (questionFlow.conditionId === '') {
              // 条件設定がない場合は表示対象
              flowList.push(questionIndex)
            } else {
              // 条件設定がある場合は設定元の回答情報を参照
              const answerIndex = this.survey.answers.findIndex(
                (answer: AnswerInfo) => answer.id === questionFlow.conditionId
              )
              // 回答ががない場合は表示対象外
              if (answerIndex === -1) {
                return
              }
              // 回答選択肢の中に一致する条件の選択肢があれば表示対象
              const isMatch = questionFlow.conditionChoiceIds.map(
                (conditionChoiceId: string) =>
                  this.survey.answers[answerIndex].choiceIds.includes(
                    conditionChoiceId
                  )
              )
              if (isMatch.includes(true)) {
                flowList.push(questionIndex)
              }
            }
          }
        )
      }
      return flowList
    },
  },
  methods: {
    /**
     * 設問のformatによって表示方法を切り替える
     * ラジオボタンの場合は選択肢表示テキストと対応する値を設定
     * チェックボックスの場合は無効選択肢の除外のみ実施
     * またAPIの値には含まれていない支援者への開示の問い追加
     */
    filteredQuestions(): void {
      this.questions = this.surveyMaster.questions
      const _this = this

      // 回答0件の場合はnullとなっている
      // 開示の問いを追加するために初期化する
      if (this.survey.answers === null) {
        this.survey.answers = []
      }

      this.questions.forEach((question: any, index: number) => {
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
      // 支援者への開示の問い追加
      if (this.surveyMaster.isDisclosure) {
        _this.disclosure = {
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
        }
        this.survey.answers.push({
          id: '',
          answer: this.survey.isDisclosure
            ? ''
            : (this.$t('survey.pages.common.disclosure.choice') as string),
          point: 0,
          choiceIds: this.survey.isDisclosure
            ? []
            : [this.$t('survey.pages.common.disclosure.choice') as string],
          summaryType: '',
          otherInput: '',
        })
      }
    },
  },
})
</script>
