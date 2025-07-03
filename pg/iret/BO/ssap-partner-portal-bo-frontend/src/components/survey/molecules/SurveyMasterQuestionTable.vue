<!-- 設問テーブル -->
<template>
  <v-form
    class="o-user-detail-rows no-border pt-4 px-8 pb-0"
    :value="isValid"
    @input="$listeners['input']"
  >
    <!-- 設問ID -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.id')"
      :value="localSurveyMaster.questions[index].id"
      :is-editing="false"
    />
    <!-- 表示順番 -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.order')"
      :value="index + 1"
      :is-editing="false"
    />
    <!-- 入力必須 -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.required.name')"
      required
      :is-editing="isEditing"
      :value="
        localSurveyMaster.questions[index].required
          ? $t('common.detail.required')
          : $t('common.detail.elective')
      "
    >
      <RadioGroup
        v-model="localSurveyMaster.questions[index].required"
        required
        :labels="$t('survey.pages.revision.table.survey.row.required').labels"
        :values="$t('survey.pages.revision.table.survey.row.required').values"
        horizontal
        hide-details
        @change="onChange"
      />
    </CommonDetailRow>
    <!-- 設問見出し -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.description.name')"
      required
      :is-editing="isEditing"
      :value="localSurveyMaster.questions[index].description"
    >
      <Sheet width="100%">
        <TextField
          v-model="localSurveyMaster.questions[index].description"
          required
          outlined
          dense
          max-length="255"
          :placeholder="
            $t('survey.pages.revision.table.survey.row.description.placeholder')
          "
          @input="onChange"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 設問表示条件有無 -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.provision.name')"
      required
      :is-editing="isEditing"
    >
      <template #isNotEditing>
        <span v-if="isExistsConditionId">あり</span>
        <span v-else>なし</span>
      </template>
      <RadioGroup
        v-model="isExistsConditionId"
        :labels="$t('survey.pages.revision.table.survey.row.provision').labels"
        :values="$t('survey.pages.revision.table.survey.row.provision').values"
        horizontal
        hide-details
        @change="onChangeProvision($event)"
      />
    </CommonDetailRow>
    <!-- 設問表示条件設問（設問表示条件有無がありの時のみ） -->
    <CommonDetailRow
      v-if="isExistsConditionId"
      :label="
        $t('survey.pages.revision.table.survey.row.provisionQuestion.name')
      "
      required
      :is-editing="isEditing"
      :value="
        questionDescriptions[localSurveyMaster.questionFlow[index].conditionId]
      "
    >
      <Sheet width="100%">
        <Select
          v-model="localSurveyMaster.questionFlow[index].conditionId"
          outlined
          dense
          required
          :items="getProvisionQuestions()"
          item-text="description"
          item-value="id"
          :placeholder="
            $t(
              'survey.pages.revision.table.survey.row.provisionQuestion.placeholder'
            )
          "
          @change="onChange"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 設問表示条件設問選択肢（設問表示条件有無がありの時のみ） -->
    <CommonDetailRow
      v-if="
        isExistsConditionId &&
        localSurveyMaster.questionFlow[index].conditionId !== ''
      "
      :label="
        $t('survey.pages.revision.table.survey.row.provisionChoices.name')
      "
      class="o-survey-master-provision-choice"
      required
      :is-editing="isEditing"
    >
      <template #isNotEditing>
        <span v-for="(choice, index) in conditionChoices" :key="index">
          {{ choice.title }}
          <span v-if="index < conditionChoices.length - 1">、</span>
        </span>
      </template>
      <Checkbox
        v-for="(choice, index) in choices"
        :key="index"
        v-model="conditionChoiceIds"
        class="mr-7"
        :label="choice.title"
        :value="choice.id"
        hide-details
        required
        @change="onChangeConditionChoiceIds"
      />
    </CommonDetailRow>
    <!-- 回答タイプ -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.format.name')"
      required
      :is-editing="isEditing"
      :value="formatLabels[localSurveyMaster.questions[index].format]"
    >
      <RadioGroup
        v-model="localSurveyMaster.questions[index].format"
        :labels="$t('survey.pages.revision.table.survey.row.format').labels"
        :values="$t('survey.pages.revision.table.survey.row.format').values"
        horizontal
        hide-details
        @change="onChange"
      />
    </CommonDetailRow>
    <!-- 集計タイプ -->
    <CommonDetailRow
      v-if="localSurveyMaster.questions[index].format === 'radio'"
      :label="$t('survey.pages.revision.table.survey.row.summaryType.name')"
      required
      :is-editing="isEditing"
      :value="summaryTypeNames[localSurveyMaster.questions[index].summaryType]"
    >
      <RadioGroup
        v-model="localSurveyMaster.questions[index].summaryType"
        class="o-survey-master-summarytype totalling-type"
        :labels="getSummaryTypes.labels"
        :values="getSummaryTypes.values"
        horizontal
        hide-details
        :disabled-radio-indexes="hiddenIndexes"
        @change="onChange"
      />
    </CommonDetailRow>
    <!-- 回答選択肢グループ -->
    <template v-if="localSurveyMaster.questions[index].format !== 'textarea'">
      <template v-if="!isEditing">
        <template v-if="showsChoiceGroup">
          <CommonDetailRow
            v-for="(choice, indexChoices) in localSurveyMaster.questions[index]
              .choices"
            :key="choice.description"
            :label="
              $t('survey.pages.revision.table.survey.row.choiceGroup.title')
            "
            :is-editing="isEditing"
          >
            <template #isNotEditing>
              <table class="o-common-detail-rows__table">
                <tr>
                  <th>
                    {{
                      $t(
                        'survey.pages.revision.table.survey.row.choiceGroup.row.groupTitle'
                      )
                    }}
                  </th>
                  <td>
                    {{ choice.description }}
                  </td>
                </tr>
                <tr
                  v-for="(answerChoice, indexAnswerChoice) in localSurveyMaster
                    .questions[index].choices[indexChoices].group"
                  :key="indexAnswerChoice"
                >
                  <th
                    v-if="
                      !localSurveyMaster.questions[index].choices[indexChoices]
                        .group[indexAnswerChoice].disabled
                    "
                  >
                    {{
                      $t(
                        'survey.pages.revision.table.survey.row.choiceGroup.row.choices'
                      )
                    }}
                    {{ indexAnswerChoice + 1 }}
                  </th>
                  <td
                    v-if="
                      !localSurveyMaster.questions[index].choices[indexChoices]
                        .group[indexAnswerChoice].disabled
                    "
                  >
                    <span>
                      {{
                        localSurveyMaster.questions[index].choices[indexChoices]
                          .group[indexAnswerChoice].title
                      }}
                    </span>
                  </td>
                </tr>
              </table>
            </template>
          </CommonDetailRow>
        </template>
      </template>
      <template v-else>
        <OptionGroups
          v-model="isValid"
          :option-groups="localSurveyMaster.questions[index].choices"
          :index="index"
          :required-count="requiredChoiceCount"
          :additional-rules="
            localSurveyMaster.questions[index].format === 'radio' &&
            localSurveyMaster.questions[index].summaryType !== 'normal'
              ? [scoreFormatCheck]
              : []
          "
          @update="onChangeChoices"
        />
      </template>
    </template>
    <!-- 回答任意入力有無 -->
    <CommonDetailRow
      :label="$t('survey.pages.revision.table.survey.row.optional.name')"
      required
      :is-editing="isEditing"
      :value="
        isNotBlackOtherDescription
          ? $t('common.detail.existence')
          : $t('common.detail.nonexistence')
      "
    >
      <RadioGroup
        v-model="isNotBlackOtherDescription"
        :labels="$t('survey.pages.revision.table.survey.row.optional').labels"
        :values="$t('survey.pages.revision.table.survey.row.optional').values"
        horizontal
        hide-details
        @change="onChange"
      />
    </CommonDetailRow>
    <!-- 回答任意入力見出し（回答任意入力有無がtrueの時のみ） -->
    <CommonDetailRow
      v-if="isNotBlackOtherDescription"
      :label="$t('survey.pages.revision.table.survey.row.otherDescription')"
      required
      :is-editing="isEditing"
      :value="localSurveyMaster.questions[index].otherDescription"
    >
      <Sheet width="100%">
        <Textarea
          v-model="localSurveyMaster.questions[index].otherDescription"
          style-set="outlined"
          rows="2"
          required
          @input="onChange"
        />
      </Sheet>
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import OptionGroups from './OptionGroups.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import {
  RadioGroup,
  Checkbox,
  TextField,
  Textarea,
  Button,
  Select,
  Sheet,
  Chip,
} from '~/components/common/atoms/index'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import Heading from '~/components/common/molecules/Heading.vue'
import {
  SurveyMasterQuestionItem,
  SurveyMasterChoiceGroupItem,
  SurveyMasterChoiceItem,
  GetSurveyMasterByIdAndRevisionResponse,
  UpdateSurveyMasterDraftByIdRequest as LocalSurveyMaster,
} from '~/models/Master'
import {
  ENUM_GET_SURVEY_SERVICE_TYPE_IDX,
  ENUM_GET_SURVEY_COMPLETION_TYPE_IDX,
  ENUM_GET_SURVEY_PP_TYPE_IDX,
} from '@/types/Survey'
const { v4: uuidv4 } = require('uuid')

export { LocalSurveyMaster }

export default BaseComponent.extend({
  // v-model用
  model: {
    prop: 'isValid',
    event: 'input',
  },
  components: {
    TextField,
    Textarea,
    RadioGroup,
    Checkbox,
    Button,
    Select,
    Sheet,
    Chip,
    CommonDetailRow,
    DetailContainer,
    Heading,
    OptionGroups,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    /** 編集開始時点の状態のアンケートマスター */
    snapSurveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    /** 読み込み中か */
    isEditing: {
      type: Boolean,
      required: true,
    },
    /** 表示中の設問インデックス番号 */
    index: {
      type: Number,
    },
    /** バリデーション状態 */
    isValid: {
      type: Boolean,
    },
    /** 回答選択肢の最小数 */
    requiredChoiceCount: {
      type: Number,
      required: true,
    },
  },
  data(): {
    value: string
    isInvalid: boolean
    isExistsConditionId: boolean
    isNotBlackOtherDescription: boolean
    conditionChoiceIds: string[]
  } {
    return {
      value: '',
      isInvalid: false,
      isExistsConditionId:
        this.surveyMaster.questionFlow[this.index].conditionId !== '',
      isNotBlackOtherDescription:
        this.surveyMaster.questions[this.index].otherDescription !== '' &&
        this.surveyMaster.questions[this.index].otherDescription !== null,
      conditionChoiceIds: this.surveyMaster.questionFlow[this.index]
        .conditionChoiceIds
        ? this.surveyMaster.questionFlow[this.index].conditionChoiceIds
        : [],
    }
  },
  computed: {
    /**
     * 集計タイプのRadioGroupで非活性にするラジオボタンを返す
     * @returns hiddenIndexes 非活性にするラジオボタン値配列
     */
    hiddenIndexes() {
      const hiddenIndexes = []

      // サービスアンケート
      if (this.localSurveyMaster.type === 'service') {
        // 他の設問で「総合満足度」が選択されており、設問編集開始時に「総合満足度」が選択されていない場合
        if (this.isSatisfaction && this.initialSummaryType !== 'satisfaction') {
          hiddenIndexes.push(ENUM_GET_SURVEY_SERVICE_TYPE_IDX.SATISFACTION)
        }
      }

      // 終了アンケート
      if (this.localSurveyMaster.type === 'completion') {
        if (this.isSatisfaction && this.initialSummaryType !== 'satisfaction') {
          hiddenIndexes.push(ENUM_GET_SURVEY_COMPLETION_TYPE_IDX.SATISFACTION)
        }
        if (this.isContinuation && this.initialSummaryType !== 'continuation') {
          hiddenIndexes.push(ENUM_GET_SURVEY_COMPLETION_TYPE_IDX.CONTINUATION)
        }
        if (this.isRecommended && this.initialSummaryType !== 'recommended') {
          hiddenIndexes.push(ENUM_GET_SURVEY_COMPLETION_TYPE_IDX.RECOMMENDED)
        }
        if (this.isSales && this.initialSummaryType !== 'sales') {
          hiddenIndexes.push(ENUM_GET_SURVEY_COMPLETION_TYPE_IDX.SALES)
        }
      }

      // PartnerPortalアンケート
      if (this.localSurveyMaster.type === 'pp') {
        if (
          this.isSurveySatisfaction &&
          this.initialSummaryType !== 'survey_satisfaction'
        ) {
          hiddenIndexes.push(ENUM_GET_SURVEY_PP_TYPE_IDX.SURVEY_SATISFACTION)
        }
        if (
          this.isManHourSatisfaction &&
          this.initialSummaryType !== 'man_hour_satisfaction'
        ) {
          hiddenIndexes.push(ENUM_GET_SURVEY_PP_TYPE_IDX.MAN_HOUR_SATISFACTION)
        }
        if (
          this.isKarteSatisfaction &&
          this.initialSummaryType !== 'karte_satisfaction'
        ) {
          hiddenIndexes.push(ENUM_GET_SURVEY_PP_TYPE_IDX.KARTE_SATISFACTION)
        }
        if (
          this.isMasterKarteSatisfaction &&
          this.initialSummaryType !== 'master_karte_satisfaction'
        ) {
          hiddenIndexes.push(
            ENUM_GET_SURVEY_PP_TYPE_IDX.MASTER_KARTE_SATISFACTION
          )
        }
      }

      return hiddenIndexes
    },
    /**
     * 変種時に選択肢の非表示チェックボックスが外れていたら、グループごと非表示にする
     * @returns {boolean} 回答選択肢グループを表示するか否か
     */
    showsChoiceGroup(): boolean {
      // 各設問の回答選択肢グループ
      const group =
        this.localSurveyMaster.questions[this.index].choices[0].group

      for (const i in group) {
        // 変種時に選択肢の非表示チェックボックスがチェックであればtrue
        if (!group[i].disabled) {
          return true
        }
      }
      return false
    },
    /**
     * 集計タイプを返す
     * @returns 集計タイプ値配列
     */
    getSummaryTypes(this: any) {
      const result = {
        labels: [],
        values: [],
      }
      const values = this.$t(
        'survey.pages.revision.table.survey.row.summaryType'
      ).values
      const labels = this.$t(
        'survey.pages.revision.table.survey.row.summaryType'
      ).labels
      for (const i in values) {
        const value = values[i]
        const label = labels[i]
        let boolRegister = false
        if (this.localSurveyMaster.type === 'service') {
          if (
            value === 'normal' ||
            value === 'point' ||
            value === 'satisfaction'
          ) {
            boolRegister = true
          }
        } else if (this.localSurveyMaster.type === 'completion') {
          if (
            value === 'normal' ||
            value === 'point' ||
            value === 'satisfaction' ||
            value === 'continuation' ||
            value === 'recommended' ||
            value === 'sales'
          ) {
            boolRegister = true
          }
        } else if (this.localSurveyMaster.type === 'quick') {
          if (value === 'normal' || value === 'point') {
            boolRegister = true
          }
        } else if (this.localSurveyMaster.type === 'pp') {
          if (
            value === 'normal' ||
            value === 'point' ||
            value === 'survey_satisfaction' ||
            value === 'man_hour_satisfaction' ||
            value === 'karte_satisfaction' ||
            value === 'master_karte_satisfaction'
          ) {
            boolRegister = true
          }
        }
        if (boolRegister === true) {
          // @ts-ignore
          result.values.push(value)
          // @ts-ignore
          result.labels.push(label)
        }
      }
      return result
    },
    getConditionChoiceIdsLength(this: any) {
      if (
        this.localSurveyMaster.questionFlow[this.index].conditionChoiceIds &&
        this.localSurveyMaster.questionFlow[this.index].conditionChoiceIds
          .length
      ) {
        return this.localSurveyMaster.questionFlow[this.index]
          .conditionChoiceIds.length
      }
      return 0
    },
    conditionChoiceIdsLabels() {
      const results = []
      const provisionChoicesList: any = this.$t(
        'survey.pages.revision.table.survey.row.provisionChoices.list'
      )
      for (const i in provisionChoicesList) {
        results[Number(provisionChoicesList[i].value)] = String(
          provisionChoicesList[i].name
        )
      }
      return results
    },
    conditionChoiceIdsLabelsById() {
      // conditionIdからconditionChoiceIdsLabelsを成形する
      const _this = this
      const choiceQuestion = this.localSurveyMaster.questions.find(
        (question: any) => {
          return (
            question.id ===
            _this.localSurveyMaster.questionFlow[_this.index].conditionId
          )
        }
      )
      const choiceLabels: string[] = []
      choiceQuestion.choices.forEach((choiceGroup: any) => {
        choiceGroup.group.forEach((choice: any) => {
          const result = _this.localSurveyMaster.questionFlow[
            _this.index
          ].conditionChoiceIds.find((conditionId: string) => {
            return conditionId === choice.id
          })
          if (result) {
            choiceLabels.push(choice.title)
          }
        })
      })
      return choiceLabels
    },
    /**
     * 集計タイプをオブジェクトで返す
     * @returns 集計タイプオブジェクト
     */
    summaryTypeNames(this: any) {
      const results: any = {}
      const labels = this.$t(
        'survey.pages.revision.table.survey.row.summaryType.labels'
      )
      const values = this.$t(
        'survey.pages.revision.table.survey.row.summaryType.values'
      )
      for (const i in values) {
        results[values[i]] = labels[i]
      }
      return results
    },
    /**
     * 回答タイプをオブジェクトで返す
     * @returns 回答タイプオブジェクト
     */
    formatLabels(this: any) {
      const results: any = {}
      const labels = this.$t(
        'survey.pages.revision.table.survey.row.format.labels'
      )
      const values = this.$t(
        'survey.pages.revision.table.survey.row.format.values'
      )
      for (const i in values) {
        results[values[i]] = labels[i]
      }
      return results
    },
    /**
     * 設問表示条件設問をオブジェクトで返す
     * @returns 設問表示条件設問オブジェクト
     */
    questionDescriptions(this: any) {
      const results: any = {}
      for (const i in this.localSurveyMaster.questions) {
        results[this.localSurveyMaster.questions[i].id] =
          this.localSurveyMaster.questions[i].description
      }
      return results
    },
    /**
     * 編集対象のアンケートマスターを初期化し返す
     * @returns アンケートマスター
     */
    localSurveyMaster(this: any) {
      const localSurveyMaster = cloneDeep(this.surveyMaster)

      // 回答任意入力有無をなしにした場合は回答任意入力見出しデータを削除する
      if (!this.isNotBlackOtherDescription) {
        localSurveyMaster.questions[this.index].otherDescription = ''
      }
      /*
      ※各種値の補完はこちらで行う
      **/
      // 回答選択肢グループが空の場合は追加する
      if (localSurveyMaster.questions[this.index].choices.length === 0) {
        const choiceItem = new SurveyMasterChoiceItem()
        choiceItem.isNew = true
        const choiceGroupItem = new SurveyMasterChoiceGroupItem()
        choiceGroupItem.id = uuidv4()
        choiceGroupItem.isNew = true
        choiceItem.group.push(choiceGroupItem)
        localSurveyMaster.questions[this.index].choices.push(choiceItem)
      }
      // 回答タイプがラジオボタンかつ集計タイプが空の場合はデフォルト値をセットする
      // 回答タイプがラジオボタン以外の場合は明示的に集計タイプにnullを設定する
      if (localSurveyMaster.questions[this.index].format === 'radio') {
        if (
          localSurveyMaster.questions[this.index].summaryType === null ||
          localSurveyMaster.questions[this.index].summaryType === ''
        ) {
          localSurveyMaster.questions[this.index].summaryType = 'normal'
        }
      } else {
        // @ts-ignore
        localSurveyMaster.questions[this.index].summaryType = null
      }
      return Object.assign(new LocalSurveyMaster(), localSurveyMaster)
    },
    /**
     * 表示用にフォーマットした回答選択肢配列を返す
     * @returns フォーマット済み回答選択肢配列
     */
    choices(): SurveyMasterChoiceGroupItem[] {
      const choices: SurveyMasterChoiceGroupItem[] = []
      const conditionId =
        this.localSurveyMaster.questionFlow[this.index].conditionId
      let questionChoices: SurveyMasterChoiceItem[] = []
      let format = ''
      let summaryType = ''
      for (const i in this.localSurveyMaster.questions) {
        if (this.localSurveyMaster.questions[i].id === conditionId) {
          questionChoices = this.localSurveyMaster.questions[i].choices
          format = this.localSurveyMaster.questions[i].format
          summaryType = this.localSurveyMaster.questions[i].summaryType
          break
        }
      }
      if (questionChoices.length > 0) {
        for (const i in questionChoices) {
          for (const i2 in questionChoices[i].group) {
            const choice: SurveyMasterChoiceGroupItem = cloneDeep(
              questionChoices[i].group[i2]
            )
            if (format === 'radio' && summaryType !== 'normal') {
              choice.title = choice.title
                ? String(choice.title).replace(/^\[\[\d+\]\]/, '')
                : choice.title
            }
            choices.push(choice)
          }
        }
      }
      return choices
    },
    /**
     * 条件分岐選択肢IDに設定されている回答選択肢配列を返す
     * @returns 回答選択肢配列
     */
    conditionChoices(): SurveyMasterChoiceGroupItem[] {
      const conditionChoices: SurveyMasterChoiceGroupItem[] = []
      for (const i in this.choices) {
        const choice: SurveyMasterChoiceGroupItem = cloneDeep(this.choices[i])
        if (this.conditionChoiceIds.includes(choice.id)) {
          conditionChoices.push(choice)
        }
      }
      return conditionChoices
    },
    /**
     * 集計タイプ[満足度]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isSatisfaction(): boolean {
      return this.checkQuestionsState('satisfaction')
    },
    /**
     * 集計タイプ[継続意思]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isContinuation(): boolean {
      return this.checkQuestionsState('continuation')
    },
    /**
     * 集計タイプ[紹介可能性]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isRecommended(): boolean {
      return this.checkQuestionsState('recommended')
    },
    /**
     * 集計タイプ[営業評価]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isSales(): boolean {
      return this.checkQuestionsState('sales')
    },
    /**
     * 集計タイプ[工数機能満足度]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isManHourSatisfaction(): boolean {
      return this.checkQuestionsState('man_hour_satisfaction')
    },
    /**
     * 集計タイプ[アンケート機能満足度]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isSurveySatisfaction(): boolean {
      return this.checkQuestionsState('survey_satisfaction')
    },
    /**
     * 集計タイプ[カルテ機能満足度]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isKarteSatisfaction(): boolean {
      return this.checkQuestionsState('karte_satisfaction')
    },
    /**
     * 集計タイプ[マスターカルテ満足度]が他の有効化されている設問で使われているか
     * @returns 判定真偽値
     */
    isMasterKarteSatisfaction(): boolean {
      return this.checkQuestionsState('master_karte_satisfaction')
    },
    /**
     * 編集対象に設定中の集計タイプを返す
     * @return 集計タイプ
     */
    initialSummaryType(): string {
      return this.surveyMaster.questions[this.index].summaryType
    },
  },
  methods: {
    /**
     * 指定の集計タイプが他の有効化されている設問で使われているか否かを導き出す
     * @param 集計タイプ
     * @returns 判定真偽値
     */
    checkQuestionsState(summaryType: string) {
      for (const i in this.surveyMaster.questions) {
        if (parseInt(i) !== this.index) {
          const question: SurveyMasterQuestionItem =
            this.surveyMaster.questions[i]
          if (question.disabled === false && question.format === 'radio') {
            const type = question.summaryType
            if (type === summaryType) {
              return true
            }
          }
        }
      }
      return false
    },
    /**
     * 現在の各設問の区分(通常設問/条件元設問/条件設問)を全て返す
     * @returns 設問区分文字列配列
     */
    getIndexAttributes() {
      const results: any = []
      for (const i in this.localSurveyMaster.questions) {
        const id = String(this.localSurveyMaster.questions[i].id)
        const parentConditionId = String(
          this.localSurveyMaster.questionFlow[i].conditionId
        )
        let isChild = false
        if (parentConditionId !== '') {
          for (const i2 in this.localSurveyMaster.questions) {
            if (this.localSurveyMaster.questions[i2].id === parentConditionId) {
              isChild = true
              break
            }
          }
        }
        let isParent = false
        if (isChild === false) {
          for (const i2 in this.localSurveyMaster.questionFlow) {
            if (
              String(this.localSurveyMaster.questionFlow[i2].conditionId) === id
            ) {
              isParent = true
              break
            }
          }
        }
        if (isChild === false && isParent === false) {
          results[i] = 'neutral'
        } else if (isParent === true) {
          results[i] = 'parent'
        } else if (isChild === true) {
          results[i] = 'child'
        }
      }
      return results
    },
    /**
     * 設問編集画面を開いた時点での各設問の区分(通常設問/条件元設問/条件設問)を全て返す
     * @returns 設問区分文字列配列
     */
    getIndexAttributesBySnap() {
      const results: any = []
      for (const i in this.snapSurveyMaster.questions) {
        const id = String(this.snapSurveyMaster.questions[i].id)
        const parentConditionId = String(
          this.snapSurveyMaster.questionFlow[i].conditionId
        )
        let isChild = false
        if (parentConditionId !== '') {
          for (const i2 in this.snapSurveyMaster.questions) {
            if (this.snapSurveyMaster.questions[i2].id === parentConditionId) {
              isChild = true
              break
            }
          }
        }
        let isParent = false
        if (isChild === false) {
          for (const i2 in this.snapSurveyMaster.questionFlow) {
            if (
              String(this.snapSurveyMaster.questionFlow[i2].conditionId) === id
            ) {
              isParent = true
              break
            }
          }
        }
        if (isChild === false && isParent === false) {
          results[i] = 'neutral'
        } else if (isParent === true) {
          results[i] = 'parent'
        } else if (isChild === true) {
          results[i] = 'child'
        }
      }
      return results
    },
    /**
     * 条件元設問のインデックス番号から条件設問のインデックス番号配列を取得
     * @param parentIndex 条件元設問のインデックス番号
     * @param indexAttributes 設問区分文字列配列
     * @returns 条件設問のインデックス番号配列
     */
    getIndexGroupByParent(parentIndex: number, indexAttributes: any) {
      const indexGroup: number[] = []
      indexGroup.push(parentIndex)
      let nextIndex: number = Number(parentIndex) + 1
      while (true) {
        if (indexAttributes[nextIndex]) {
          if (indexAttributes[nextIndex] === 'child') {
            indexGroup.push(nextIndex)
            nextIndex++
          } else {
            break
          }
        } else {
          break
        }
      }
      return indexGroup
    },
    /**
     * 条件設問のインデックス番号から条件元設問および条件設問のインデックス番号配列を取得
     * @param childIndex 条件設問のインデックス番号
     * @param indexAttributes 設問区分文字列配列
     * @returns 条件元設問および条件設問のインデックス番号配列
     */
    getIndexGroupByChild(childIndex: number, indexAttributes: any) {
      let indexGroup: number[] = []
      let nextIndex: number = Number(childIndex) - 1
      let parentIndex = -1
      while (true) {
        if (indexAttributes[nextIndex]) {
          if (indexAttributes[nextIndex] === 'parent') {
            parentIndex = nextIndex
            break
          } else {
            nextIndex--
          }
        } else {
          break
        }
      }
      if (parentIndex !== -1) {
        indexGroup = this.getIndexGroupByParent(parentIndex, indexAttributes)
      }
      return indexGroup
    },
    /**
     * 条件元設問に設定可能な設問インデックス番号配列を返す
     * @returns 設問インデックス番号配列
     */
    getProvisionQuestions(this: any) {
      // 自身が他設問の条件元の場合は何も返さない
      // 自身に条件を設定している場合は条件元のみ返す
      // 条件未指定かつ他設問の条件元でもない場合は直前の設定可能な設問のみを返す
      const provisionQuestions: number[] = []
      const indexAttributes = this.getIndexAttributesBySnap()
      const indexAttribute = indexAttributes[this.index]

      if (indexAttribute === 'parent') {
        return provisionQuestions
      } else if (indexAttribute === 'child') {
        const indexGroupByChild = this.getIndexGroupByChild(
          this.index,
          indexAttributes
        )
        for (const i in indexGroupByChild) {
          const question = JSON.parse(
            JSON.stringify(
              this.localSurveyMaster.questions[indexGroupByChild[i]]
            )
          )
          if (question.disabled === false) {
            question.description = `【ID：${
              Number(indexGroupByChild[0]) + 1
            }】${question.description}`
            provisionQuestions.push(question)
            break
          }
        }
        return provisionQuestions
      } else if (indexAttribute === 'neutral') {
        const beforeIndexes: number[] = []
        for (const i in indexAttributes) {
          if (Number(i) < Number(this.index)) {
            beforeIndexes.push(Number(i))
          }
        }
        const reversedBeforeIndexes = beforeIndexes.reverse()
        for (const i in reversedBeforeIndexes) {
          const backIndex = reversedBeforeIndexes[i]
          const backAttribute = indexAttributes[backIndex]
          if (backAttribute === 'neutral' || backAttribute === 'parent') {
            const question = JSON.parse(
              JSON.stringify(this.localSurveyMaster.questions[backIndex])
            )
            if (question.disabled === false) {
              question.description = `【ID：${Number(backIndex) + 1}】${
                question.description
              }`
              provisionQuestions.push(question)
              break
            }
          }
        }
        return provisionQuestions
      }
    },
    /**
     * 他コンポーネントへ設問表示条件有無の変更を受け渡す
     * @param event 変更値
     */
    onChangeProvision(this: any, event: any) {
      const provision = Boolean(event)
      this.localSurveyMaster.questionFlow[this.index].id =
        this.localSurveyMaster.questions[this.index].id
      if (provision === true) {
        this.localSurveyMaster.questionFlow[this.index].conditionChoiceIds = []
        this.localSurveyMaster.questionFlow[this.index].conditionId = ''
      } else {
        this.localSurveyMaster.questionFlow[this.index].conditionChoiceIds =
          null
        this.localSurveyMaster.questionFlow[this.index].conditionId = ''
      }
      this.$emit('update', this.localSurveyMaster, this.index)
    },
    /** 他コンポーネントへアンケートマスターの変更を受け渡す */
    onChange(this: any) {
      this.$emit('update', this.localSurveyMaster, this.index)
    },
    /**
     * 他コンポーネントへ回答選択肢の変更を受け渡す
     * @param index 対象設問インデックス番号
     * @param choices 回答選択肢配列
     */
    onChangeChoices(index: number, choices: SurveyMasterChoiceItem[]) {
      this.localSurveyMaster.questions[index].choices = choices
      this.$emit('update', this.localSurveyMaster, this.index)
    },
    /** 他コンポーネントへ設問表示条件設問選択肢の変更を受け渡す */
    onChangeConditionChoiceIds(this: any) {
      this.localSurveyMaster.questionFlow[this.index].conditionChoiceIds =
        this.conditionChoiceIds.sort()
      this.$emit('update', this.localSurveyMaster, this.index)
    },
    /**
     * 回答選択肢の評点プレフィックスの有無を判定
     * @returns 判定真偽値
     */
    scoreFormatCheck(value: string) {
      const regex = /^\[\[\d+\]\]/
      if (!regex.test(value)) {
        return this.$t(
          'survey.pages.revision.table.survey.row.choiceGroup.errorMessages.noPrefixScoreFormat'
        ) as string
      }
      return true
    },
  },
})
</script>

<style lang="scss">
.o-survey-master-summarytype {
  .v-input--radio-group__input {
    flex-wrap: wrap;
  }
}
.o-survey-master-provision-choice {
  .v-input--selection-controls {
    margin-top: 0;
  }
  .v-label {
    font-size: 0.875rem;
    color: $c-black;
  }
}

.totalling-type .v-radio {
  padding: 3px 0;
}
</style>
