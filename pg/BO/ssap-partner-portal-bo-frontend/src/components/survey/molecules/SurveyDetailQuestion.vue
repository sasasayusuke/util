<template>
  <v-container pa-0 fluid>
    <v-row
      v-for="(item, index) in getConditionalQuestions()"
      :key="index"
      no-gutters
      :class="{ 'mt-4': index }"
    >
      <v-col>
        <Sheet style-set="text-box">
          <Title style-set="normal">
            {{ $t('survey.pages.detail.answers.question') }}{{ index + 1 }} :
            {{ item.description }}
            <Chip
              v-if="item.required"
              x-small
              style-set="required"
              class="ml-3"
              >{{ $t('common.detail.required') }}</Chip
            >
            <Chip v-else x-small style-set="any" class="ml-3">{{
              $t('common.detail.elective')
            }}</Chip>
          </Title>
        </Sheet>
        <template v-if="item.format === 'textarea' || item.format === 'text'">
          <div class="pa-4">
            <Sheet style-set="no-editing">
              <template v-if="isOtherInput">
                <Paragraph
                  v-for="(text, p_index) in brText(getAnswers(item.id).answer)"
                  :key="p_index"
                  style-set="small"
                  class="mb-0"
                >
                  {{ text }}
                </Paragraph>
              </template>
              <Paragraph v-else style-set="small" class="mb-0"></Paragraph>
            </Sheet>
          </div>
        </template>

        <div
          v-for="(choice, choice_index) in item.choices"
          :key="choice_index"
          class="pa-4"
        >
          <h4 v-if="choice.description">{{ choice.description }}</h4>
          <template v-if="item.format === 'radio'">
            <RadioGroup
              v-model="values[index][choice_index]"
              :labels="
                choice.group
                  .filter((elm) => {
                    return !elm.disabled
                  })
                  .map((elm) => {
                    return trimPointString(String(elm.title))
                  })
              "
              :values="
                choice.group
                  .filter((elm) => {
                    return !elm.disabled
                  })
                  .map((elm) => {
                    return elm.id
                  })
              "
              readonly
              horizontal
              is-no-editing
              hide-details
            />
          </template>
          <template v-if="item.format === 'checkbox'">
            <v-container fluid pa-0>
              <v-row no-gutters>
                <template v-for="(checkbox, checkbox_index) in choice.group">
                  <v-col :key="checkbox_index" cols="auto" class="mr-4">
                    <Checkbox
                      :id="checkbox.id + checkbox_index"
                      v-model="values[index][choice_index][checkbox_index]"
                      :label="checkbox.title"
                      :value="
                        isChoiceId(item.id, checkbox.id) ? checkbox.id : ''
                      "
                      readonly
                      :disabled="!isChoiceId(item.id, checkbox.id)"
                      :for="checkbox.id + checkbox_index"
                      is-no-editing
                      class="pt-0 mt-0 type2 a-checkbox--survey"
                      hide-details
                    />
                  </v-col>
                </template>
              </v-row>
            </v-container>
          </template>
          <template v-if="item.format === 'text'">
            <Sheet style-set="no-editing">
              <Paragraph
                v-for="(text, p_index) in brText(item.otherDescription)"
                :key="p_index"
                style-set="small"
                class="mb-0"
              >
                {{ text }}
              </Paragraph>
            </Sheet>
          </template>
          <template
            v-else-if="
              choice_index === item.choices.length - 1 &&
              item.otherDescription !== null &&
              item.otherDescription !== ''
            "
          >
            <Title hx="h5" style-set="small" class="mt-3">{{
              item.otherDescription
            }}</Title>
            <Sheet style-set="no-editing" class="mt-3">
              <Paragraph
                v-for="(text, p_index) in brText(getOtherInput(item.id))"
                :key="p_index"
                style-set="small"
                class="mb-0"
              >
                {{ text }}
              </Paragraph>
            </Sheet>
          </template>
        </div>
      </v-col>
    </v-row>
    <v-row v-if="surveyMaster.isDisclosure" no-gutters class="mt-4">
      <v-col>
        <Sheet style-set="text-box">
          <Title style-set="normal">
            {{ $t('survey.pages.detail.answers.disclosure.description') }}
          </Title>
        </Sheet>
        <div class="pa-4">
          <Checkbox
            v-model="isDisclosure"
            :value="isDisclosure"
            :label="$t('survey.pages.detail.answers.disclosure.choice')"
            :disabled="!isDisclosure"
            readonly
            is-no-editing
            class="pt-0 mt-0 type2 a-checkbox--survey"
            hide-details
          />
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { AnswerInfo, GetSurveyByIdResponse } from '~/models/Survey'
import {
  SurveyMasterQuestionItem,
  SurveyMasterQuestionFlowItem,
  SurveyMasterChoiceItem,
} from '~/models/Master'
import {
  Sheet,
  Title,
  Chip,
  RadioGroup,
  Checkbox,
  Textarea,
  Paragraph,
} from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    Sheet,
    Title,
    Chip,
    RadioGroup,
    Checkbox,
    Textarea,
    Paragraph,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object,
      required: true,
    },
    /** アンケート情報 */
    survey: {
      type: Object as PropType<GetSurveyByIdResponse>,
    },
  },
  created() {
    this.buildValues()
  },
  data() {
    return {
      values: [] as any,
      isDisclosure: !this.survey.isDisclosure,
    }
  },
  computed: {
    localSurveyMaster() {
      return cloneDeep(this.surveyMaster)
    },
  },
  methods: {
    /**
     * テキストを改行で分割し配列化
     * @param text 元テキスト
     * @returns 分割テキスト配列
     */
    brText(text: string) {
      return text.split('\n')
    },
    /**
     * 指定した設問IDの回答結果を返す
     * @param questionsId 設問ID
     * @returns 回答結果
     */
    getAnswers(questionsId: string): AnswerInfo {
      const answers = this.survey.answers
      for (const i in answers) {
        if (answers[i].id === questionsId) {
          return answers[i] as AnswerInfo
        }
      }
      return new AnswerInfo()
    },
    /**
     * 指定した設問IDのその他記載欄が入力されているかチェック
     * @param questionsId 設問ID
     * @returns 入力有無判定真偽値
     */
    isOtherInput(questionsId: string): boolean {
      const answer: AnswerInfo = this.getAnswers(questionsId)
      return answer.otherInput !== ''
    },
    /**
     * 指定した設問IDの設問を返す
     * @param questionsId 設問ID
     * @returns 設問
     */
    getQuestion(questionsId: string): SurveyMasterQuestionItem {
      const questions = this.localSurveyMaster.questions
      for (const i in questions) {
        if (questions[i].id === questionsId) {
          return questions[i] as SurveyMasterQuestionItem
        }
      }
      return new SurveyMasterQuestionItem()
    },
    /**
     * 指定した設問IDの出題フローを返す
     * @param questionsId 設問ID
     * @returns 出題フロー
     */
    getQuestionFlow(questionsId: string): SurveyMasterQuestionFlowItem {
      const questionFlow = this.localSurveyMaster.questionFlow
      for (const i in questionFlow) {
        if (questionFlow[i].id === questionsId) {
          return questionFlow[i] as SurveyMasterQuestionFlowItem
        }
      }
      return new SurveyMasterQuestionFlowItem()
    },
    /**
     * 表示対象の設問配列を返す
     * @returns 設問配列
     */
    getConditionalQuestions(): SurveyMasterQuestionItem[] {
      const questions: SurveyMasterQuestionItem[] = []
      for (const i in this.localSurveyMaster.questions) {
        // 設問が無効の場合は表示対象外
        if (this.localSurveyMaster.questions[i].disabled === true) {
          continue
        }
        const question: SurveyMasterQuestionItem =
          this.localSurveyMaster.questions[i]
        const questionFlow: SurveyMasterQuestionFlowItem = this.getQuestionFlow(
          question.id
        )

        // 有効な選択肢が存在しない選択肢グループは表示させない。
        question.choices = question.choices.filter(
          (elm: SurveyMasterChoiceItem) => {
            let existsChoice = false
            for (const i2 in elm.group) {
              if (elm.group[i2].disabled === false) {
                existsChoice = true
              }
            }
            return existsChoice
          }
        )

        if (questionFlow.conditionId === '') {
          questions.push(question)
        } else {
          const answers: AnswerInfo = this.getAnswers(questionFlow.conditionId)
          if (
            answers.choiceIds !== null &&
            questionFlow.conditionChoiceIds !== null
          ) {
            for (const i2 in answers.choiceIds) {
              if (
                questionFlow.conditionChoiceIds.includes(answers.choiceIds[i2])
              ) {
                questions.push(question)
                break
              }
            }
          }
        }
      }
      return questions
    },
    /**
     * 指定した設問IDのその他記載欄の内容を返す
     * @param questionsId 設問ID
     * @returns その他記載欄文字列
     */
    getOtherInput(questionsId: string): string {
      const answer = this.getAnswers(questionsId)
      return answer.otherInput !== null ? answer.otherInput : ''
    },
    /**
     * 指定した設問IDの設問の回答選択肢IDが存在するかチェック
     * @param questionsId 設問ID
     * @param choiceId 回答選択肢ID
     * @returns 回答選択肢判定真偽値
     */
    isChoiceId(questionsId: string, choiceId: string): boolean {
      return (
        this.getAnswers(questionsId).choiceIds &&
        this.getAnswers(questionsId).choiceIds.includes &&
        this.getAnswers(questionsId).choiceIds.includes(choiceId)
      )
    },
    /**
     * 回答選択肢項目名から評点プレフィックスを消去した文字列を返す
     * @param value 回答選択肢項目名
     * @returns フォーマット済み回答選択肢項目名
     */
    trimPointString(value: string): string {
      return value.replace(/^\[\[\d+\]\]/, '')
    },
    /**
     * created用に、valuesをセットアップする
     */
    buildValues() {
      const questions = this.getConditionalQuestions()
      this.values = []
      for (let i = 0; i < questions.length; i++) {
        if (!this.values[i]) {
          this.values[i] = []
        }
        const questionsId = questions[i].id
        const format = questions[i].format
        const choices =
          questions[i].choices && questions[i].choices.length
            ? questions[i].choices
            : []
        for (let i2 = 0; i2 < choices.length; i2++) {
          const choicesGroup =
            choices[i2].group && choices[i2].group.length
              ? choices[i2].group
              : []
          if (format === 'radio') {
            if (!this.values[i][i2]) {
              this.values[i][i2] = ''
            }
            for (let i3 = 0; i3 < choicesGroup.length; i3++) {
              const choiceGroupId = choicesGroup[i3].id
              if (this.isChoiceId(questionsId, choiceGroupId)) {
                this.values[i][i2] = choiceGroupId
                break
              }
            }
          } else {
            if (!this.values[i][i2]) {
              this.values[i][i2] = []
            }
            for (let i3 = 0; i3 < choicesGroup.length; i3++) {
              if (!this.values[i][i2][i3]) {
                this.values[i][i2][i3] = ''
              }
              const choiceGroupId = choicesGroup[i3].id
              this.values[i][i2][i3] = this.isChoiceId(
                questionsId,
                choiceGroupId
              )
                ? choiceGroupId
                : ''
            }
          }
        }
      }
    },
  },
})
</script>

<style lang="scss" scoped>
h4 {
  font-size: 1rem;
  padding-left: 16px;
  margin-bottom: 16px;
  position: relative;
  &:before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    height: 100%;
    width: 6px;
    background-color: $c-primary-dark;
  }
}
.a-paragraph {
  & + .a-paragraph {
    margin-top: 0;
  }
}
</style>
