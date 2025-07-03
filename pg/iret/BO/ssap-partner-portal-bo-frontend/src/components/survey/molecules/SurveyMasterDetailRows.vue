<template>
  <v-form
    class="o-user-detail-rows pt-4 px-8 pb-0"
    :value="isValid"
    @input="$listeners['input']"
  >
    <!-- アンケート名 -->
    <CommonDetailRow
      :label="$t('survey.pages.masterDetail.setting.row.name.name')"
      required
      :is-editing="isEditing && isEditable"
      :value="formattedSurveyMaster.name"
    >
      <Sheet width="100%">
        <TextField
          v-model="localParam.name"
          required
          :max-length="100"
          style-set="outlined"
          :placeholder="
            $t('survey.pages.masterDetail.setting.row.name.placeholder')
          "
        />
      </Sheet>
    </CommonDetailRow>
    <!-- アンケート種別 -->
    <CommonDetailRow
      v-if="!isCreating"
      :label="$t('survey.pages.masterDetail.setting.row.type')"
      :value="formattedSurveyMaster.type"
      :is-editing="false"
    />
    <CommonDetailRow
      v-else
      :label="$t('survey.pages.masterDetail.setting.row.type')"
      :value="$t('survey.pages.create.row.type.quick')"
      :is-editing="false"
    />
    <!-- 利用タイミング -->
    <CommonDetailRow
      v-if="!isCreating"
      :label="$t('survey.pages.masterDetail.setting.row.timing')"
      :value="formattedSurveyMaster.timing"
      :is-editing="false"
    />
    <CommonDetailRow
      v-else
      :label="$t('survey.pages.masterDetail.setting.row.timing')"
      :value="$t('survey.pages.create.row.timing.anytime')"
      :is-editing="false"
    />
    <!-- 送信日初期設定 -->
    <CommonDetailRow
      :label="$t('survey.pages.masterDetail.setting.row.sending')"
      required
      :is-editing="isEditing"
      :value="initSendSettingDisplay"
    >
      <RadioGroup
        v-model="localParam.initSendDaySettingType"
        required
        horizontal
        hide-details
      >
        <template #unique>
          <!-- 回答期限日からXX営業日前 -->
          <v-radio
            :value="INIT_SENT_DAY_SETTING_TYPE.daysBeforeDeadline"
            :disabled="
              localParam.initAnswerLimitDaySettingType ===
                INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterSent ||
              localParam.initAnswerLimitDaySettingType ===
                INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.none
            "
          >
            <template #label>
              <span class="radio-text">
                {{ $t('survey.pages.masterDetail.setting.row.deadlineDate') }}
                <Sheet width="60" class="mx-2">
                  <TextField
                    v-model="localParam.initSendDayInput.daysBeforeDeadline"
                    type="number"
                    number
                    :min="minSendDayDaysBeforeDeadLineSetting"
                    :max="maxSendDayDaysBeforeDeadLineSetting"
                    :range-number-from="1"
                    :range-number-to="20"
                    :max-digits="2"
                    positive-number
                    style-set="outlined"
                    :placeholder="
                      $t(
                        'survey.pages.masterDetail.setting.row.radio.sendDaySetting.placeholder'
                      )
                    "
                    @focusout="sendDaySettingValidationCorrect"
                  />
                </Sheet>
                {{
                  $t('survey.pages.masterDetail.setting.row.businessDayBefore')
                }}
              </span>
            </template>
          </v-radio>
          <!-- XX日 -->
          <v-radio :value="INIT_SENT_DAY_SETTING_TYPE.dayEveryMonth">
            <template #label>
              <span class="radio-text">
                <Sheet width="60" class="mx-2">
                  <TextField
                    v-model="localParam.initSendDayInput.dayEveryMonth"
                    type="number"
                    number
                    :min="minSendDayEveryMonthSetting"
                    :max="maxSendDayEveryMonthSetting"
                    :range-number-from="1"
                    :range-number-to="31"
                    :max-digits="2"
                    positive-number
                    style-set="outlined"
                    :placeholder="
                      $t(
                        'survey.pages.masterDetail.setting.row.radio.sendDaySetting.placeholder'
                      )
                    "
                    @focusout="sendDaySettingValidationCorrect"
                  />
                </Sheet>
                {{ $t('survey.pages.masterDetail.setting.row.day') }}
              </span>
            </template>
          </v-radio>
          <!-- なし -->
          <v-radio
            :value="INIT_SENT_DAY_SETTING_TYPE.none"
            :disabled="
              localParam.initAnswerLimitDaySettingType ===
              INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterSent
            "
          >
            <template #label>
              <span class="radio-text">
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.sendDaySetting.none'
                  )
                }}
              </span>
            </template>
          </v-radio>
        </template>
      </RadioGroup>
    </CommonDetailRow>
    <!-- 回答期限日初期設定 -->
    <CommonDetailRow
      :label="$t('survey.pages.masterDetail.setting.row.limit')"
      :value="initAnswerLimitDaySettingDisplay"
      required
      :is-editing="isEditing && isEditable"
    >
      <RadioGroup
        v-model="localParam.initAnswerLimitDaySettingType"
        required
        class="mt-0 pt-0"
        horizontal
      >
        <template #unique>
          <!-- 月末最終日 -->
          <v-radio :value="INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.endOfMonth">
            <template #label>
              <span class="radio-text">
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.2'
                  )
                }}
              </span>
            </template>
          </v-radio>
          <!-- 送信日からXX営業日後 -->
          <v-radio
            :value="
              $t(
                'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting'
              ).values[0]
            "
            :disabled="
              localParam.initSendDaySettingType !==
              INIT_SENT_DAY_SETTING_TYPE.dayEveryMonth
            "
          >
            <template #label>
              <span class="radio-text">
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.1[0]'
                  )
                }}
                <Sheet width="60" class="mx-2">
                  <TextField
                    v-model="localParam.initAnswerLimitDayInput.daysAfterSent"
                    type="number"
                    number
                    :min="minAnswerLimitDayAfterSentSetting"
                    :max="maxAnswerLimitDayAfterSentSetting"
                    :max-digits="2"
                    positive-number
                    :range-number-from="1"
                    :range-number-to="15"
                    style-set="outlined"
                    :placeholder="
                      $t(
                        'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.placeholder'
                      )
                    "
                    @focusout="answerLimitDaySettingValidationCorrect"
                  />
                </Sheet>
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.1[1]'
                  )
                }}
              </span>
            </template>
          </v-radio>
          <!-- 翌月月初XX営業日 -->
          <v-radio
            :value="INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterNextMonthStart"
          >
            <template #label>
              <span class="radio-text">
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.4[0]'
                  )
                }}
                <Sheet width="60" class="mx-2">
                  <TextField
                    v-model="
                      localParam.initAnswerLimitDayInput.daysAfterNextMonthStart
                    "
                    type="number"
                    number
                    :min="minAnswerLimitDayAfterNextMonthStartSetting"
                    :max="maxAnswerLimitDayAfterNextMonthStartSetting"
                    :max-digits="2"
                    positive-number
                    :range-number-from="1"
                    :range-number-to="15"
                    style-set="outlined"
                    :placeholder="
                      $t(
                        'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.placeholder'
                      )
                    "
                    @focusout="answerLimitDaySettingValidationCorrect"
                  />
                </Sheet>
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.4[1]'
                  )
                }}
              </span>
            </template>
          </v-radio>
          <!-- なし -->
          <v-radio
            :value="INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.none"
            :disabled="localParam.initSendDaySettingType === 3"
          >
            <template #label>
              <span class="radio-text">
                {{
                  $t(
                    'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.3'
                  )
                }}
              </span>
            </template>
          </v-radio>
        </template>
      </RadioGroup>
    </CommonDetailRow>
    <!-- 開示確認質問有無 -->
    <CommonDetailRow
      :class="{ 'no-border': !isEditing }"
      :label="$t('survey.pages.masterDetail.setting.row.disclosure')"
      :value="
        formattedSurveyMaster.isDisclosure
          ? $t('common.detail.existence')
          : $t('common.detail.nonexistence')
      "
      required
      :is-editing="isEditing"
    >
      <RadioGroup
        v-model="localParam.isDisclosure"
        required
        :labels="
          $t('survey.pages.masterDetail.setting.row.radio.disclosure').labels
        "
        :values="
          $t('survey.pages.masterDetail.setting.row.radio.disclosure').values
        "
        horizontal
      />
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import { cloneDeep } from 'lodash'
import {
  TextField,
  Select,
  ToolTips,
  Sheet,
  Required,
  RadioGroup,
  Chip,
} from '../../common/atoms/index'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'
import { formatDateStr } from '~/utils/common-functions'
import { countBusinessDayBeyondAMonth } from '~/utils/business-day'
import { SurveyMasterListItem } from '~/models/Master'
import { INIT_SENT_DAY_SETTING_TYPE } from '~/types/Master'

class InitSendDayInput {
  daysBeforeDeadline: number = 7 // todo: 設定
  dayEveryMonth: number = 1 // todo: 設定
}

class InitAnswerLimitDayInput {
  daysAfterSent: number = 1
  daysAfterNextMonthStart: number = 2
}

export const INIT_ANSWER_LIMIT_DAY_SETTING_TYPE = {
  daysAfterSent: 1,
  daysAfterNextMonthStart: 2,
  endOfMonth: 3,
  none: 4,
} as const

// eslint-disable-next-line no-redeclare
export type INIT_ANSWER_LIMIT_DAY_SETTING_TYPE =
  typeof INIT_ANSWER_LIMIT_DAY_SETTING_TYPE[keyof typeof INIT_ANSWER_LIMIT_DAY_SETTING_TYPE]

export class LocalSurveyMaster extends SurveyMasterListItem {
  initSendDaySettingType: number = INIT_SENT_DAY_SETTING_TYPE.daysBeforeDeadline
  initSendDayInput = new InitSendDayInput()
  initAnswerLimitDaySettingType: number =
    INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterNextMonthStart

  initAnswerLimitDayInput = new InitAnswerLimitDayInput()
}

export default CommonDetailRows.extend({
  // v-model用
  model: {
    prop: 'isValid',
    event: 'input',
  },
  components: {
    TextField,
    Select,
    ToolTips,
    Sheet,
    Required,
    RadioGroup,
    Chip,
    CommonDetailRow,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<SurveyMasterListItem>,
      required: true,
    },
    /** バリデーション判定結果 */
    isValid: {
      type: Boolean,
    },
  },
  data(): {
    localParam: LocalSurveyMaster
    lastLocalParam: LocalSurveyMaster
    minSendDayEveryMonthSetting: number
    maxSendDayEveryMonthSetting: number
    minSendDayDaysBeforeDeadLineSetting: number
    maxSendDayDaysBeforeDeadLineSetting: number
    minAnswerLimitDayAfterSentSetting: number
    maxAnswerLimitDayAfterSentSetting: number
    minAnswerLimitDayAfterNextMonthStartSetting: number
    maxAnswerLimitDayAfterNextMonthStartSetting: number
    defaultSendDay: number
    defaultAnswerLimitDay: number
    endOfMonth: number
    noAnswer: number
    initSendDaySetting: number
    initAnswerLimitDaySetting: number
    INIT_SENT_DAY_SETTING_TYPE: object
    INIT_ANSWER_LIMIT_DAY_SETTING_TYPE: object
  } {
    return {
      localParam: Object.assign(new LocalSurveyMaster(), this.surveyMaster),
      lastLocalParam: Object.assign(new LocalSurveyMaster(), this.surveyMaster),
      minSendDayEveryMonthSetting: 1,
      maxSendDayEveryMonthSetting: 31,
      minSendDayDaysBeforeDeadLineSetting: 1,
      maxSendDayDaysBeforeDeadLineSetting: 20,
      minAnswerLimitDayAfterSentSetting: 1,
      maxAnswerLimitDayAfterSentSetting: 15,
      minAnswerLimitDayAfterNextMonthStartSetting: 1,
      maxAnswerLimitDayAfterNextMonthStartSetting: 15,
      defaultSendDay: 25,
      defaultAnswerLimitDay: 7,
      endOfMonth: 99,
      noAnswer: 0,
      initSendDaySetting: 25,
      initAnswerLimitDaySetting: 7,
      INIT_SENT_DAY_SETTING_TYPE,
      INIT_ANSWER_LIMIT_DAY_SETTING_TYPE,
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換したアンケートマスターを返す
     * @returns フォーマット済みアンケートマスター
     */
    formattedSurveyMaster(): SurveyMasterListItem {
      const rtn = Object.assign(new SurveyMasterListItem(), this.surveyMaster)
      rtn.type = this.$t(
        'survey.pages.masterDetail.row.type.' + rtn.type
      ) as string
      rtn.timing = this.$t(
        'survey.pages.masterDetail.row.timing.' + rtn.timing
      ) as string
      if (this.surveyMaster.updateAt) {
        rtn.updateAt = formatDateStr(this.surveyMaster.updateAt, 'Y/M/d HH:mm')
      }
      return rtn
    },
    /**
     * 詳細画面の送信日初期設定表示
     * @returns なしまたは送信日初期設定
     */
    initSendSettingDisplay(): string {
      if (this.formattedSurveyMaster.initSendDaySetting === this.noAnswer) {
        // なしの場合
        return this.$t(
          'survey.pages.masterDetail.setting.row.radio.sendDaySetting.none'
        ) as string
      } else if (
        Math.sign(this.formattedSurveyMaster.initSendDaySetting) === -1
      ) {
        const initSendDay = -this.formattedSurveyMaster.initSendDaySetting
        // 送信日初期設定が「回答期限日から〇営業日前(-30～-1)」(負の数である場合)
        //値を正の整数に変換
        return (this.$t('survey.pages.masterDetail.setting.row.deadlineDate') +
          initSendDay.toString() +
          this.$t(
            'survey.pages.masterDetail.setting.row.businessDayBefore'
          )) as string
      } else {
        // 送信日初期設定が「毎月〇日(1～31)」(正の数である場合)
        return (this.$t('survey.pages.masterList.row.timing.monthly') +
          this.formattedSurveyMaster.initSendDaySetting.toString() +
          this.$t('survey.pages.masterDetail.setting.row.day')) as string
      }
    },
    /**
     * 詳細画面の回答期限日初期設定
     * @returns 月末最終日またはなしまたは回答期限日初期設定
     */
    initAnswerLimitDaySettingDisplay(): string {
      if (this.localParam.initAnswerLimitDaySetting === this.endOfMonth) {
        // 月末最終日
        return this.$t(
          'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.2'
        ) as string
      } else if (this.localParam.initAnswerLimitDaySetting === this.noAnswer) {
        // なし
        return this.$t(
          'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.3'
        ) as string
      } else if (this.formattedSurveyMaster.initAnswerLimitDaySetting > 100) {
        const initAnswerLimitDay =
          this.formattedSurveyMaster.initAnswerLimitDaySetting - 100
        // 翌月月初〇営業日(101～130)
        return (this.$t(
          'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.4[0]'
        ) +
          String(initAnswerLimitDay) +
          this.$t(
            'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.4[1]'
          )) as string
      } else {
        // 送信日から〇営業日後(1～30)
        return (this.$t(
          'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.1[0]'
        ) +
          String(this.formattedSurveyMaster.initAnswerLimitDaySetting) +
          this.$t(
            'survey.pages.masterDetail.setting.row.radio.answerLimitDaySetting.labels.1[1]'
          )) as string
      }
    },
    /**
     * アンケートタイプがクイックの時編集可
     * @param surveyMaster.type アンケートタイプ
     * @returns 編集可能か否か
     */
    isEditable() {
      if (this.surveyMaster.type === 'quick') {
        return true
      }
      return false
    },
  },
  created() {
    if (!this.isCreating) {
      this.initLocalParam()
    }
  },
  methods: {
    /** 編集情報を初期化 */
    initLocalParam() {
      //localParamを整形
      const surveyMaster = Object.assign(
        new LocalSurveyMaster(),
        this.surveyMaster
      )
      //前回登録「送信日初期設定」を設定
      if (Math.sign(this.surveyMaster.initSendDaySetting) === -1) {
        // 回答期限日から〇営業日前の場合
        surveyMaster.initSendDayInput.daysBeforeDeadline =
          -this.surveyMaster.initSendDaySetting
      } else if (
        this.surveyMaster.initSendDaySetting >=
          this.minSendDayEveryMonthSetting &&
        this.surveyMaster.initSendDaySetting <= this.maxSendDayEveryMonthSetting
      ) {
        // 〇日
        surveyMaster.initSendDayInput.dayEveryMonth =
          this.surveyMaster.initSendDaySetting
      }

      //前回登録「回答期限日初期設定」を設定
      if (this.surveyMaster.initAnswerLimitDaySetting > 100) {
        //翌月月初〇営業日の場合
        surveyMaster.initAnswerLimitDayInput.daysAfterNextMonthStart =
          this.surveyMaster.initAnswerLimitDaySetting - 100
      } else if (
        this.surveyMaster.initAnswerLimitDaySetting >=
          this.minAnswerLimitDayAfterSentSetting &&
        this.surveyMaster.initAnswerLimitDaySetting <=
          this.maxAnswerLimitDayAfterSentSetting
      ) {
        //送信日から〇営業日後の場合
        surveyMaster.initAnswerLimitDayInput.daysAfterSent =
          this.surveyMaster.initAnswerLimitDaySetting
      }
      this.localParam = surveyMaster

      this.setInitSendDaySettingType()
      this.setInitAnswerLimitDaySettingType()
      this.setInitSendDaySetting()
      this.setInitAnswerLimitDaySetting()
    },
    /** 送信日初期設定の入力値を補正 */
    sendDaySettingValidationCorrect() {
      // 毎月◯日： 上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.initSendDaySettingType ===
        INIT_SENT_DAY_SETTING_TYPE.dayEveryMonth
      ) {
        if (
          this.localParam.initSendDayInput.dayEveryMonth <
          this.minSendDayEveryMonthSetting
        ) {
          this.localParam.initSendDayInput.dayEveryMonth = 1
        }

        if (
          this.localParam.initSendDayInput.dayEveryMonth >
          this.maxSendDayEveryMonthSetting
        ) {
          this.localParam.initSendDayInput.dayEveryMonth =
            this.maxSendDayEveryMonthSetting
        }
      }

      // 回答期限から◯営業日前：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.initSendDaySettingType ===
        INIT_SENT_DAY_SETTING_TYPE.daysBeforeDeadline
      ) {
        if (
          this.localParam.initSendDayInput.daysBeforeDeadline <
          this.minSendDayDaysBeforeDeadLineSetting
        ) {
          this.localParam.initSendDayInput.daysBeforeDeadline =
            this.minSendDayDaysBeforeDeadLineSetting
        }
        if (
          this.localParam.initSendDayInput.daysBeforeDeadline >
          this.maxSendDayDaysBeforeDeadLineSetting
        ) {
          this.localParam.initSendDayInput.daysBeforeDeadline =
            this.maxSendDayDaysBeforeDeadLineSetting
        }
      }
    },
    /** 回答期限日初期設定の入力値を補正 */
    answerLimitDaySettingValidationCorrect() {
      // 送信日から◯営業日後：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterSent
      ) {
        if (
          this.localParam.initAnswerLimitDayInput.daysAfterSent <
          this.minAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.initAnswerLimitDayInput.daysAfterSent =
            this.minAnswerLimitDayAfterSentSetting
        }
        if (
          this.localParam.initAnswerLimitDayInput.daysAfterSent >
          this.maxAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.initAnswerLimitDayInput.daysAfterSent =
            this.maxAnswerLimitDayAfterSentSetting
        }
      }

      // 送信日の翌月月初◯営業日：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterNextMonthStart
      ) {
        if (
          this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart <
          this.minAnswerLimitDayAfterNextMonthStartSetting
        ) {
          this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart =
            this.minAnswerLimitDayAfterNextMonthStartSetting
        }
        if (
          this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart >
          this.maxAnswerLimitDayAfterNextMonthStartSetting
        ) {
          this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart =
            this.maxAnswerLimitDayAfterNextMonthStartSetting
        }
      }
    },
    // 送信日の選択されたラジオボタンの値をローカルの送信日に設定
    setInitSendDaySetting() {
      // 「なし」の場合
      if (
        this.localParam.initSendDaySettingType ===
        INIT_SENT_DAY_SETTING_TYPE.none
      ) {
        this.localParam.initSendDaySetting = this.noAnswer
      }
      // 毎月〇日(1～31)
      if (
        this.localParam.initSendDaySettingType ===
        INIT_SENT_DAY_SETTING_TYPE.dayEveryMonth
      ) {
        const dayEveryMonth = this.localParam.initSendDayInput.dayEveryMonth
        this.localParam.initSendDaySetting = Number(dayEveryMonth)
      }
      // 回答期限日から〇営業日前(-30～-1)
      if (
        this.localParam.initSendDaySettingType ===
        INIT_SENT_DAY_SETTING_TYPE.daysBeforeDeadline
      ) {
        const daysBeforeDeadline =
          this.localParam.initSendDayInput.daysBeforeDeadline
        this.localParam.initSendDaySetting = -daysBeforeDeadline
      }
    },
    /** 変更画面に遷移した際の送信日初期設定のラジオボタンを制御 */
    setInitSendDaySettingType() {
      if (this.localParam.initSendDaySetting === this.noAnswer) {
        // 送信日初期設定が「なし」の場合
        this.localParam.initSendDaySettingType = INIT_SENT_DAY_SETTING_TYPE.none
      } else if (Math.sign(this.localParam.initSendDaySetting) === -1) {
        // 送信日初期設定が「回答期限日から〇営業日前(-30～-1)」(負の数である場合)
        this.localParam.initSendDaySettingType =
          INIT_SENT_DAY_SETTING_TYPE.daysBeforeDeadline
      } else {
        // 送信日初期設定が「毎月〇日(1～31)」(正の数である場合)
        this.localParam.initSendDaySettingType =
          INIT_SENT_DAY_SETTING_TYPE.dayEveryMonth
      }
    },
    /** 回答期限日の選択されたラジオボタンの値をローカルの回答期限日に設定 */
    setInitAnswerLimitDaySetting() {
      // 「なし」の場合
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.none
      ) {
        this.localParam.initAnswerLimitDaySetting = this.noAnswer
      }
      // 「月末最終日」の場合
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.endOfMonth
      ) {
        this.localParam.initAnswerLimitDaySetting = this.endOfMonth
      }
      // 「送信日から〇営業日後」の場合
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterSent
      ) {
        const daysAfterSent =
          this.localParam.initAnswerLimitDayInput.daysAfterSent

        this.localParam.initAnswerLimitDaySetting = Number(daysAfterSent)
        this.localParam.initAnswerLimitDayInput.daysAfterSent =
          Number(daysAfterSent)
      }
      // 「翌月月初〇営業日」の場合
      if (
        this.localParam.initAnswerLimitDaySettingType ===
        INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterNextMonthStart
      ) {
        const daysAfterNextMonthStart =
          this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart

        this.localParam.initAnswerLimitDaySetting =
          Number(daysAfterNextMonthStart) + 100
        this.localParam.initAnswerLimitDayInput.daysAfterNextMonthStart =
          Number(daysAfterNextMonthStart)
      }
    },
    /** 変更画面に遷移した際の回答期限日初期設定のラジオボタンを制御 */
    setInitAnswerLimitDaySettingType() {
      if (this.localParam.initAnswerLimitDaySetting === this.noAnswer) {
        this.localParam.initAnswerLimitDaySettingType =
          INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.none
      } else if (
        this.localParam.initAnswerLimitDaySetting === this.endOfMonth
      ) {
        this.localParam.initAnswerLimitDaySettingType =
          INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.endOfMonth
      } else if (this.localParam.initAnswerLimitDaySetting < 100) {
        //送信日から〇営業日後の場合
        this.localParam.initAnswerLimitDaySettingType =
          INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterSent
      } else if (this.localParam.initAnswerLimitDaySetting >= 100) {
        //翌月月初〇営業日
        this.localParam.initAnswerLimitDaySettingType =
          INIT_ANSWER_LIMIT_DAY_SETTING_TYPE.daysAfterNextMonthStart
      }
    },
    setDaysBeforeDeadline() {
      const fromDay = this.localParam.initSendDaySetting
      const toDay = this.localParam.initAnswerLimitDaySetting

      const day = countBusinessDayBeyondAMonth(fromDay, toDay)

      this.localParam.initSendDayInput.daysBeforeDeadline = day
    },
  },
  watch: {
    localParam: {
      deep: true,
      handler() {
        this.setInitSendDaySetting()
        this.setInitAnswerLimitDaySetting()
      },
    },
    isEditing(newVal: boolean, oldVal: boolean) {
      if (newVal === true && oldVal === false) {
        this.initLocalParam()
        this.lastLocalParam = cloneDeep(this.localParam)
      } else {
        this.localParam = cloneDeep(this.lastLocalParam)
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.o-user-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-user-detail-rows__select__item {
  list-style: none;
  display: inline-block;
  margin-right: 1em;
  &::after {
    content: ',';
  }
  &:last-child {
    &::after {
      display: none;
    }
  }
}
.radio-text {
  font-size: 0.875rem !important;
  color: $c-black;
  display: flex;
  align-items: center;
}
.no-border {
  border-bottom: 0;
}
</style>
