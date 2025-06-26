<template>
  <ModalEdit :title="title">
    <v-container py-2 px-0 pb-6 class="font-size-small">
      <v-form v-model="isValid" @input="$listeners['input']">
        <!-- アンケートタイプ -->
        <v-row class="m-survery-modal__unit">
          <CommonDetailRow
            :label="$t('project.pages.edit.survey.row.type.label')"
            :is-editing="true"
            :value="localAddSurveySchedule.surveyType"
            cols="4"
            required
            class="is-noborder"
          >
            <Sheet width="300">
              <Select
                v-model="localAddSurveySchedule.surveyType"
                required
                style-set="outlined"
                :items="surveyTypeSelect"
                item-text="label"
                item-value="value"
                :placeholder="
                  $t('project.pages.edit.survey.row.type.placeholder')
                "
                @change="getSurveyMasterInfo"
              />
            </Sheet>
          </CommonDetailRow>
        </v-row>
        <v-row class="m-survery-modal__unit">
          <v-col cols="4" class="font-weight-bold">{{
            $t('project.pages.edit.survey.row.schedule.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- タイミング -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.timing.label')"
              :is-editing="true"
              :value="localParam.timing"
              cols="3"
              required
              class="is-noborder"
            >
              <RadioGroup
                v-model="localParam.timing"
                required
                :labels="
                  $t('project.pages.edit.survey.row.sub.timing.radio.labels')
                "
                :values="
                  $t('project.pages.edit.survey.row.sub.timing.radio.values')
                "
                horizontal
                :disabled="disabledTiming"
              />
            </CommonDetailRow>
            <!-- 送信日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.send.label')"
              :is-editing="true"
              :value="localAddSurveySchedule.requestDate"
              cols="3"
              required
              class="is-noborder"
            >
              <RadioGroup
                v-model="localParam.sendDayRadio"
                class="mt-0 pt-0"
                @change="changeSendDaysRadio"
              >
                <template #unique>
                  <v-radio
                    :value="sendDayRadioItems.manual"
                    :disabled="disabledLimitDaysBefore"
                  >
                    <template #label>
                      <!-- 回答期限日から〇〇営業日前に自動設定 -->
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.send.deadlineDate'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="localParam.sendDays.limitDaysBefore"
                            :required="
                              localParam.timing === 'monthly' ? true : false
                            "
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :range-number-from="1"
                            :range-number-to="20"
                            min="1"
                            max="20"
                            :max-digits="2"
                            positive-number
                            :disabled="disabledTextFieldLimitDaysBefore"
                            @focusout="sendDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.send.businessDayBefore'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <v-radio :value="sendDayRadioItems.direct">
                    <template #label>
                      <!-- 毎月 -->
                      <template v-if="localParam.timing === 'monthly'">
                        <span class="radio-text">
                          {{
                            $t('project.pages.edit.survey.row.sub.send.monthly')
                          }}
                          <Sheet width="60" class="mx-2">
                            <TextField
                              v-if="isUpdatingRequestDay === false"
                              v-model="localParam.sendDays.everyMonth"
                              :required="
                                localParam.timing === 'monthly' ? true : false
                              "
                              type="number"
                              number
                              style-set="outlined"
                              :placeholder="
                                $t(
                                  'project.pages.edit.survey.row.sub.send.placeholder'
                                )
                              "
                              :range-number-from="1"
                              :range-number-to="31"
                              :max-digits="2"
                              positive-number
                              min="1"
                              max="31"
                              :disabled="disabledTextFieldEveryMonth"
                              @focusout="sendDaySettingValidationCorrect"
                            />
                          </Sheet>
                          {{
                            $t('project.pages.edit.survey.row.sub.send.date')
                          }}
                        </span>
                      </template>
                      <!-- 毎週 -->
                      <template v-if="localParam.timing === 'weekly'">
                        <span class="radio-text">
                          {{
                            $t('project.pages.edit.survey.row.sub.send.weekly')
                          }}
                          <Sheet width="70" class="mx-2">
                            <Select
                              v-model="localParam.sendDays.dayOfWeek"
                              style-set="outlined"
                              :items="dayOfTheWeek"
                              item-text="label"
                              item-value="value"
                              :placeholder="
                                $t(
                                  'project.pages.edit.survey.row.sub.send.placeholder'
                                )
                              "
                              :disabled="disabledTextFieldEveryWeek"
                            />
                          </Sheet>
                          {{
                            $t(
                              'project.pages.edit.survey.row.sub.send.select.sub'
                            )
                          }}
                        </span>
                      </template>
                      <!-- 1回（サービスアンケート・修了アンケート） -->
                      <template v-if="localParam.timing === 'once'">
                        <DateSelect
                          v-model="localParam.sendDays.direct"
                          :required="
                            localParam.timing === 'once' ? true : false
                          "
                          :allowed-dates="allowedDates"
                          :disabled="disabledTextFieldDirect"
                        />
                      </template>
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
            <!-- 回答期限日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.deadline.label')"
              :is-editing="true"
              :value="localAddSurveySchedule.limitDate"
              cols="3"
              required
              tall
              class="is-noborder"
            >
              <RadioGroup
                v-model="localParam.limitAnswerDayRadio"
                class="mt-0 pt-0"
                @change="getAnswerLimitDay"
              >
                <template #unique>
                  <!-- 月末最終営業日 -->
                  <v-radio
                    :value="limitDayRadioItems.endOfMonth"
                    :disabled="disabledEndOfMonth"
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.1'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- 送信日から○営業日後 -->
                  <v-radio
                    :value="limitDayRadioItems.sendDaysAfter"
                    :disabled="disabledSendDaysAfter"
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.2[0]'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-if="isUpdatingAnswerLimitDay === false"
                            v-model="localParam.limitAnswerDays.sendDaysAfter"
                            type="number"
                            number
                            :required="
                              answerLimitDayTiming === 'daysAfter'
                                ? true
                                : false
                            "
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="15"
                            :disabled="disabledTextFieldSendDaysAfter"
                            @focusout="answerLimitDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.2[1]'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- 翌月月初〇〇営業日 -->
                  <v-radio
                    :value="limitDayRadioItems.nextMonth"
                    :disabled="disabledNextMonth"
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.3[0]'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="localParam.limitAnswerDays.nextMonth"
                            type="number"
                            number
                            :required="
                              answerLimitDayTiming === 'daysAfter'
                                ? true
                                : false
                            "
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledTextFieldNextMonth"
                            :range-number-from="1"
                            :range-number-to="15"
                            min="1"
                            max="15"
                            :max-digits="2"
                            positive-number
                            @focusout="answerLimitDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.3[1]'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- なし -->
                  <v-radio
                    :value="limitDayRadioItems.none"
                    :disabled="disabledNone"
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.4'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
          </v-col>
        </v-row>
      </v-form>
    </v-container>
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:closeAdd')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true"
        @click="$emit('click:save')"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import {
  Button,
  Sheet,
  Paragraph,
  AutoComplete,
  TextField,
  ErrorText,
  RadioGroup,
  Required,
  Select,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SurveyMasterListItem } from '~/models/Master'

// 翌日を取得
const now = new Date()
const nextDay = format(
  new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1),
  'yyyy/MM/dd'
)

class LocalParam {
  timing = 'monthly'
  sendDayRadio = 1
  limitAnswerDayRadio = 3
  sendDays = {
    limitDaysBefore: 7,
    everyMonth: 1,
    dayOfWeek: 0,
    direct: nextDay,
  }

  limitAnswerDays = {
    endOfMonth: 99,
    sendDaysAfter: 1,
    nextMonth: 2,
    none: 0,
  }
}

//タイミング ラジオボタン
export const TIMING_RADIO_ITEMS = {
  monthly: 'monthly',
  weekly: 'weekly',
  once: 'once',
} as const

//送信日 ラジオボタン
export const SEND_DAY_RADIO_ITEMS = {
  manual: 1,
  direct: 2,
} as const

//回答期限日ラジオボタン
export const LIMIT_ANSWER_DAY_RADIO_ITEMS = {
  endOfMonth: 1,
  sendDaysAfter: 2,
  nextMonth: 3,
  none: 4,
} as const

export default BaseComponent.extend({
  components: {
    ModalEdit,
    CommonDetailRow,
    Button,
    Sheet,
    Paragraph,
    AutoComplete,
    TextField,
    ErrorText,
    RadioGroup,
    Required,
    Select,
    DateSelect,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    /** 最新バージョンのアンケートひな形の一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterListItem[]>,
      required: true,
    },
    /** 追加アンケートスケジュール
     *  {
        surveyMasterId: '',
        surveyType: '',
        timing: 'monthly',
        requestDate: 1,
        limitDate: 5,
      },
    */
    localAddSurveySchedule: {
      type: Object,
      required: true,
    },
    /** 支援期間終了日 */
    supportDateTo: {
      type: String,
      required: true,
    },
    /** 支援期間開始日 */
    supportDateFrom: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      isValid: false,
      isUpdatingRequestDay: false,
      isUpdatingAnswerLimitDay: false,
      sendDay: 'auto',
      sendDayType: ['manual', 'auto'],
      answerLimitDay: {
        endOfMonth: 99,
        none: 0,
      },
      answerLimitDayTiming: 'nextMonth',
      disabled: false,
      disabledTiming: false,
      surveyAnswerLimitDay: 1,

      // ラジオボタン選択肢
      timing: TIMING_RADIO_ITEMS,
      sendDayRadioItems: SEND_DAY_RADIO_ITEMS,
      limitDayRadioItems: LIMIT_ANSWER_DAY_RADIO_ITEMS,

      localParam: new LocalParam(),

      //選択中のラジオボタン
      sendDayRadio: 1,
      limitAnswerDayRadio: 3,

      //各ラジオボタンの活性/非活性
      //送信日：回答期限日から〇営業日前
      disabledLimitDaysBefore: false,
      //回答期限日：月末最終営業日
      disabledEndOfMonth: false,
      //回答期限日：送信日から〇営業日後
      disabledSendDaysAfter: false,
      //回答期限日：翌月月初〇営業日
      disabledNextMonth: false,
      //回答期限日：なし
      disabledNone: false,

      //各テキストボックスの活性/非活性
      //送信日：回答期限日から〇営業日前
      disabledTextFieldLimitDaysBefore: false,
      //送信日：毎月〇日
      disabledTextFieldEveryMonth: true,
      //送信日：毎週〇曜日
      disabledTextFieldEveryWeek: false,
      //送信日：特定日指定（YYYY/MM/DD）
      disabledTextFieldDirect: false,
      //回答期限日：送信日から〇営業日後
      disabledTextFieldSendDaysAfter: true,
      //回答期限日：翌月月初〇営業日
      disabledTextFieldNextMonth: false,

      //送信日(毎月〇日)の入力範囲
      minSendDayEveryMonthSetting: 1,
      maxSendDayEveryMonthSetting: 31,
      //送信日(回答期限日から〇営業日前)の入力範囲
      minSendDayDaysBeforeDeadLineSetting: 1,
      maxSendDayDaysBeforeDeadLineSetting: 20,
      //回答期限日の入力範囲
      minAnswerLimitDayAfterSentSetting: 1,
      maxAnswerLimitDayAfterSentSetting: 15,
    }
  },
  computed: {
    /**
     * 選択可能なアンケートタイプ一覧を返す
     * @returns 選択可能なアンケートタイプ一覧配列
     */
    surveyTypeSelect() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }

      this.surveyMasters.forEach((elm: any) => {
        //最新のアンケート
        if (elm.isLatest === 1 && elm.type !== 'pp') {
          selectItem.label = elm.name
          selectItem.value = elm.id
          const newElm = Object.assign({}, selectItem)
          selectItems.push(newElm)
        }
      })

      return selectItems
    },
    /**
     * 曜日セレクトボックス
     * @returns 曜日一覧配列
     */
    dayOfTheWeek() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: 0 }

      for (let index = 0; index < 5; index++) {
        // @ts-ignore
        selectItem.label = this.$t(
          'project.pages.edit.survey.row.sub.send.select.weekDay'
        )[index]
        selectItem.value = index
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      }
      return selectItems
    },
    // 月末最終営業日チェックボックスを無効化する
    isEndOfMonthDisabled(): boolean {
      if (this.localParam.timing === 'monthly') {
        return false
      }

      if (this.sendDay === 'manual') {
        return false
      }

      return true
    },
    // 翌月月初〇〇営業日チェックボックスを無効化する
    isDaysAfterDisabled(): boolean {
      if (this.localParam.timing !== 'weekly') {
        return false
      }

      if (this.sendDay === 'manual') {
        return false
      }

      return true
    },
  },
  methods: {
    //送信日の各テキストボックスの活性状態を制御
    changeSendDaysRadio(elm: number) {
      if (elm === this.sendDayRadioItems.manual) {
        // 回答期限日から〇営業日前の場合
        this.disabledTextFieldLimitDaysBefore = false
        this.disabledTextFieldEveryMonth = true
        this.disabledTextFieldEveryWeek = true
        this.disabledTextFieldDirect = true
      } else {
        // 毎月〇日、毎週〇曜日、特定日指定（YYYY/MM/DD）の場合
        this.disabledTextFieldLimitDaysBefore = true
        this.disabledTextFieldEveryMonth = false
        this.disabledTextFieldEveryWeek = false
        this.disabledTextFieldDirect = false
      }
    },
    /**
     * date-pickerで選択できる日付を制限
     * @param val 日付文字列
     * @returns 選択可否判定真偽値
     */
    allowedDates(val: any) {
      // 土日は非活性
      const weekDay = new Date(val).getDay()
      if (weekDay === 6) {
        return false
      }
      if (weekDay === 0) {
        return false
      }
      // タイミングが1回の場合、送信日は現在日時より先かつ支援終了日までしか選択できない
      const maxAllowedDay = this.getMaxAllowedDay()
      let minimumAllowedDay = this.getMinimumAllowedDay()

      //デバッグモードならば、送信日に本日より前の日時の設定が可能
      if (process.env.DEBUG) {
        minimumAllowedDay = new Date(1970, 0)
      }

      return (
        minimumAllowedDay <= new Date(val) && new Date(val) <= maxAllowedDay
      )
    },
    /**
     * date-pickerで選択できる日付の上限値を返す
     * @returns 選択可能上限値Dateオブジェクト
     */
    getMaxAllowedDay() {
      // 支援終了日を配列に格納し、Dateオブジェクトの引数とする
      const supportDateTo = this.supportDateTo.split('/')

      // 配列内のString要素をNumberに変更
      const numSupportDateTo = supportDateTo.map(Number)

      // 支援終了日のDateオブジェクトを作成
      return new Date(
        numSupportDateTo[0],
        numSupportDateTo[1] - 1,
        numSupportDateTo[2] + 1
      )
    },
    /**
     * date-pickerで選択できる日付の下限値を返す
     * @returns 選択可能下限値Dateオブジェクト
     */
    getMinimumAllowedDay() {
      const today = getCurrentDate()

      // 支援開始日を配列に格納し、Dateオブジェクトの引数とする
      const supportDateFrom = this.supportDateFrom.split('/')

      // 配列内のString要素をNumberに変更
      const numSupportDateFrom = supportDateFrom.map(Number)

      // 支援開始日のDateオブジェクトを作成
      const filteredSupportDateFrom = new Date(
        numSupportDateFrom[0],
        numSupportDateFrom[1] - 1,
        numSupportDateFrom[2] + 1
      )

      // 支援開始日をまだ迎えていない場合は、支援開始日からのスケジュールを返却
      if (filteredSupportDateFrom > today) {
        return filteredSupportDateFrom
      } else {
        return today
      }
    },
    /**
     * 設定された回答期限日に合わせて初期化
     * @param elm 回答期限日文字列
     */
    getAnswerLimitDay(elm: string) {
      if (elm === 'endOfMonth') {
        this.localAddSurveySchedule.limitDate = 99
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = true
        this.disabled = true
      } else if (elm === 'none') {
        this.localAddSurveySchedule.limitDate = 0
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = true
        this.disabled = true
      } else if (elm === 'sendDaysAfter') {
        this.localAddSurveySchedule.limitDate = this.surveyAnswerLimitDay
        this.disabled = false
        this.disabledTextFieldSendDaysAfter = false
        this.disabledTextFieldNextMonth = true
      } else if (elm === 'nextMonth') {
        this.localAddSurveySchedule.limitDate = this.surveyAnswerLimitDay
        this.disabled = false
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = false
      }
    },
    /** アンケートタイプが変更された場合、そのアンケートのマスタ情報を取得 */
    getSurveyMasterInfo(masterSurveyId: string) {
      const findSurvey = this.surveyMasters.find(
        (val: SurveyMasterListItem) => val.id === masterSurveyId
      )
      const surveyType = findSurvey!.type

      //サービスアンケート・修正アンケートの場合、1回のみ
      if (surveyType !== 'quick') {
        this.localParam.timing = this.timing.once
        this.localParam.limitAnswerDayRadio = this.limitDayRadioItems.nextMonth
        this.disabledTiming = true
      } else {
        this.localParam.timing = this.localAddSurveySchedule.timing
        this.disabledTiming = false
      }

      this.setRadiosConditions()

      // 送信日・回答期限日各フォームの初期値を取得
      const initLocalParam = new LocalParam()

      for (const i in this.surveyMasters) {
        const elm: SurveyMasterListItem = this.surveyMasters[i]
        this.localAddSurveySchedule.surveyMasterId = elm.id

        // 現在の日時を取得
        const now = new Date()
        const today = format(new Date(), 'yyyy/MM/dd')
        // 翌月を取得
        const nextMonth = new Date(now.getFullYear(), now.getMonth() + 1, 1)

        if (elm.id === masterSurveyId) {
          // 送信日
          if (elm.initSendDaySetting === 0) {
            // 送信日なしの場合
            this.localParam.sendDays.everyMonth = 1
            this.localAddSurveySchedule.requestDate = '1'
            this.localParam.sendDayRadio = this.sendDayRadioItems.direct
            this.localParam.sendDays.direct = this.getMondayOfNextWeek(
              initLocalParam.sendDays.direct,
              'nextDay'
            )

            //回答期限日から〇営業日前の値を初期化
            this.localParam.sendDays.limitDaysBefore =
              initLocalParam.sendDays.limitDaysBefore
            //送信日「毎月〇日」の値を初期化
            this.localParam.sendDays.everyMonth =
              initLocalParam.sendDays.everyMonth
            //送信日「毎週〇曜日」の値を初期化
            this.localParam.sendDays.dayOfWeek =
              initLocalParam.sendDays.dayOfWeek
          } else if (
            elm.initSendDaySetting >= 1 &&
            elm.initSendDaySetting <= 31
          ) {
            // 送信日が毎月〇日（1～31）の場合
            this.localParam.sendDays.everyMonth = elm.initSendDaySetting
            this.localParam.sendDayRadio = this.sendDayRadioItems.direct

            //回答期限日から〇営業日前の値を初期化
            this.localParam.sendDays.limitDaysBefore =
              initLocalParam.sendDays.limitDaysBefore

            // 当月の送信日を取得
            const thisMonthSendDay = format(
              new Date(
                now.getFullYear(),
                now.getMonth(),
                elm.initSendDaySetting
              ),
              'yyyy/MM/dd'
            )
            // 翌月の送信日を取得
            const nextMonthSendDay = format(
              new Date(
                nextMonth.getFullYear(),
                nextMonth.getMonth(),
                elm.initSendDaySetting
              ),
              'yyyy/MM/dd'
            )
            if (today === thisMonthSendDay) {
              // 送信日と当日が同じ場合、翌日を取得
              this.localParam.sendDays.direct = this.getMondayOfNextWeek(
                this.localParam.sendDays.direct,
                'today'
              )
            } else if (today > thisMonthSendDay) {
              // 送信日初期設定日が当日を過ぎている場合は翌月のその日を指定
              this.localParam.sendDays.direct = this.getMondayOfNextWeek(
                nextMonthSendDay,
                'thisMonthOrNextMonth'
              )
            } else if (today < thisMonthSendDay) {
              // 送信日初期設定日が当日を過ぎていない場合は当月のその日を指定
              this.localParam.sendDays.direct = this.getMondayOfNextWeek(
                thisMonthSendDay,
                'thisMonthOrNextMonth'
              )
            }
            this.localAddSurveySchedule.requestDate = elm.initSendDaySetting
          } else if (
            elm.initSendDaySetting >= -31 &&
            elm.initSendDaySetting <= -1
          ) {
            // 回答期限日から○営業日前（-30～-1）の場合
            this.localParam.sendDays.limitDaysBefore = -elm.initSendDaySetting
            this.localAddSurveySchedule.requestDate = String(
              elm.initSendDaySetting
            )
            this.localParam.sendDayRadio = this.sendDayRadioItems.manual

            //送信日「毎月〇日」の値を初期化
            this.localParam.sendDays.everyMonth =
              initLocalParam.sendDays.everyMonth
            //送信日「毎週〇曜日」の値を初期化
            this.localParam.sendDays.dayOfWeek =
              initLocalParam.sendDays.dayOfWeek
            //送信日「特定日指定（YYYY/MM/DD）」の値を初期化
            this.localParam.sendDays.direct = this.getMondayOfNextWeek(
              initLocalParam.sendDays.direct,
              'nextDay'
            )
          }

          // 回答期限日
          if (elm.initAnswerLimitDaySetting === 0) {
            // なしの場合
            this.localParam.limitAnswerDayRadio = this.limitDayRadioItems.none
            this.localAddSurveySchedule.limitDate = 0
            this.disabled = true

            //送信日から〇営業日後の値を初期化
            this.localParam.limitAnswerDays.sendDaysAfter =
              initLocalParam.limitAnswerDays.sendDaysAfter
            //送信月の翌月月初〇営業日の値を初期化
            this.localParam.limitAnswerDays.nextMonth =
              initLocalParam.limitAnswerDays.nextMonth
          } else if (elm.initAnswerLimitDaySetting === 99) {
            // 月末最終営業日の場合
            this.localParam.limitAnswerDayRadio =
              this.limitDayRadioItems.endOfMonth
            this.localAddSurveySchedule.limitDate = 99
            this.disabled = true

            //送信日から〇営業日後の値を初期化
            this.localParam.limitAnswerDays.sendDaysAfter =
              initLocalParam.limitAnswerDays.sendDaysAfter
            //送信月の翌月月初〇営業日の値を初期化
            this.localParam.limitAnswerDays.nextMonth =
              initLocalParam.limitAnswerDays.nextMonth
          } else if (
            elm.initAnswerLimitDaySetting >= 1 &&
            elm.initAnswerLimitDaySetting <= 30
          ) {
            // 送信日から〇営業日後(1～30)の場合
            this.localParam.limitAnswerDays.sendDaysAfter =
              elm.initAnswerLimitDaySetting
            this.localAddSurveySchedule.limitDate =
              elm.initAnswerLimitDaySetting
            this.localParam.limitAnswerDayRadio =
              this.limitDayRadioItems.sendDaysAfter
            this.disabled = false

            //送信月の翌月月初〇営業日の値を初期化
            this.localParam.limitAnswerDays.nextMonth =
              initLocalParam.limitAnswerDays.nextMonth
          } else if (elm.initAnswerLimitDaySetting > 100) {
            // 翌月月初○営業日(101～130)の場合
            this.localParam.limitAnswerDays.nextMonth =
              Number(elm.initAnswerLimitDaySetting) - 100
            this.localAddSurveySchedule.limitDate =
              elm.initAnswerLimitDaySetting
            this.localParam.limitAnswerDayRadio =
              this.limitDayRadioItems.nextMonth
            this.disabled = false

            //送信日から〇営業日後の値を初期化
            this.localParam.limitAnswerDays.sendDaysAfter =
              initLocalParam.limitAnswerDays.sendDaysAfter
          }
        }
      }
    },
    // フォームの変更をlocalAddSurveySchedule(Create APIリクエストボディ)に反映
    changeSurveyScheduleInfo() {
      this.localAddSurveySchedule.timing = this.localParam.timing
      this.setRadiosConditions()

      //送信日
      if (this.localParam.sendDayRadio === this.sendDayRadioItems.manual) {
        const limitDaysBefore = -this.localParam.sendDays.limitDaysBefore
        // 回答期限日から〇営業日前の場合
        this.localAddSurveySchedule.requestDate = String(limitDaysBefore)
        this.disabledTextFieldLimitDaysBefore = false
        this.disabledTextFieldEveryMonth = true
        this.disabledTextFieldEveryWeek = true
        this.disabledTextFieldDirect = true
      } else if (
        this.localParam.sendDayRadio === this.sendDayRadioItems.direct
      ) {
        this.disabledTextFieldLimitDaysBefore = true
        this.disabledTextFieldEveryMonth = false
        this.disabledTextFieldEveryWeek = false
        this.disabledTextFieldDirect = false

        if (this.localParam.timing === this.timing.monthly) {
          // 毎月
          this.localAddSurveySchedule.requestDate = String(
            this.localParam.sendDays.everyMonth
          )
        } else if (this.localParam.timing === this.timing.weekly) {
          // 毎週
          this.localAddSurveySchedule.requestDate = String(
            this.localParam.sendDays.dayOfWeek
          )
        } else if (this.localParam.timing === this.timing.once) {
          // 特定日
          this.localAddSurveySchedule.requestDate = String(
            this.localParam.sendDays.direct
          )
        }
      }

      // 回答期限日
      if (
        this.localParam.limitAnswerDayRadio === this.limitDayRadioItems.none
      ) {
        // なし
        this.localAddSurveySchedule.limitDate = 0
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = true
      } else if (
        this.localParam.limitAnswerDayRadio ===
        this.limitDayRadioItems.endOfMonth
      ) {
        // 月末最終営業日
        this.localAddSurveySchedule.limitDate = 99
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = true
      } else if (
        this.localParam.limitAnswerDayRadio ===
        this.limitDayRadioItems.sendDaysAfter
      ) {
        // 送信日から〇営業日後
        this.localAddSurveySchedule.limitDate =
          this.localParam.limitAnswerDays.sendDaysAfter
        this.disabledTextFieldSendDaysAfter = false
        this.disabledTextFieldNextMonth = true
      } else if (
        this.localParam.limitAnswerDayRadio ===
        this.limitDayRadioItems.nextMonth
      ) {
        // 翌月月初〇営業日
        this.localAddSurveySchedule.limitDate =
          Number(this.localParam.limitAnswerDays.nextMonth) + 100
        this.disabledTextFieldSendDaysAfter = true
        this.disabledTextFieldNextMonth = false
      }
    },
    /** 送信日の入力値を補正 */
    sendDaySettingValidationCorrect() {
      // 毎月◯日： 上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.timing === this.timing.monthly &&
        this.localParam.sendDayRadio === this.sendDayRadioItems.direct
      ) {
        if (
          this.localParam.sendDays.everyMonth < this.minSendDayEveryMonthSetting
        ) {
          this.localParam.sendDays.everyMonth = this.minSendDayEveryMonthSetting
        }

        if (
          this.localParam.sendDays.everyMonth > this.maxSendDayEveryMonthSetting
        ) {
          this.localParam.sendDays.everyMonth = this.maxSendDayEveryMonthSetting
        }
      }
      // 回答期限から◯営業日前：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (this.localParam.sendDayRadio === this.sendDayRadioItems.manual) {
        if (
          this.localParam.sendDays.limitDaysBefore <
          this.minSendDayDaysBeforeDeadLineSetting
        ) {
          this.localParam.sendDays.limitDaysBefore =
            this.minSendDayDaysBeforeDeadLineSetting
        }

        if (
          this.localParam.sendDays.limitDaysBefore >
          this.maxSendDayDaysBeforeDeadLineSetting
        ) {
          this.localParam.sendDays.limitDaysBefore =
            this.maxSendDayDaysBeforeDeadLineSetting
        }
      }
    },
    /** 回答期限日の入力値を補正 */
    answerLimitDaySettingValidationCorrect() {
      // 送信日から◯営業日後：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.limitAnswerDayRadio ===
        this.limitDayRadioItems.sendDaysAfter
      ) {
        if (
          this.localParam.limitAnswerDays.sendDaysAfter <
          this.minAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.limitAnswerDays.sendDaysAfter =
            this.minAnswerLimitDayAfterSentSetting
        }
        if (
          this.localParam.limitAnswerDays.sendDaysAfter >
          this.maxAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.limitAnswerDays.sendDaysAfter =
            this.maxAnswerLimitDayAfterSentSetting
        }
      }
      // 送信日の翌月月初◯営業日：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localParam.limitAnswerDayRadio ===
        this.limitDayRadioItems.nextMonth
      ) {
        if (
          this.localParam.limitAnswerDays.nextMonth <
          this.minAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.limitAnswerDays.nextMonth =
            this.minAnswerLimitDayAfterSentSetting
        }
        if (
          this.localParam.limitAnswerDays.nextMonth >
          this.maxAnswerLimitDayAfterSentSetting
        ) {
          this.localParam.limitAnswerDays.nextMonth =
            this.maxAnswerLimitDayAfterSentSetting
        }
      }
    },
    /** 送信日・回答期限日ラジオボタンの活性・非活性を制御 */
    setRadiosConditions() {
      const timingType = this.localParam.timing
      const sendDayType = this.localParam.sendDayRadio
      const limitAnswerDayType = this.localParam.limitAnswerDayRadio

      // 送信日「回答期限日から〇営業日前」ラジオボタンの制御
      if (
        (timingType === 'monthly' &&
          limitAnswerDayType === this.limitDayRadioItems.endOfMonth) ||
        (timingType === 'monthly' &&
          limitAnswerDayType === this.limitDayRadioItems.nextMonth) ||
        (timingType === 'once' &&
          limitAnswerDayType === this.limitDayRadioItems.nextMonth)
      ) {
        this.disabledLimitDaysBefore = false
      } else {
        this.disabledLimitDaysBefore = true
        //タイミングが毎週の場合、ラジオボタンが毎週〇曜日を選択
        this.localParam.sendDayRadio = this.sendDayRadioItems.direct
      }

      //回答期限日ラジオボタンの制御
      if (
        timingType === 'monthly' &&
        sendDayType === this.sendDayRadioItems.manual
      ) {
        this.disabledEndOfMonth = false
        this.disabledNextMonth = false
        this.disabledSendDaysAfter = true
        this.disabledNone = true
      } else if (
        timingType === 'weekly' &&
        (sendDayType === this.sendDayRadioItems.manual ||
          sendDayType === this.sendDayRadioItems.direct)
      ) {
        this.disabledEndOfMonth = false
        this.disabledNextMonth = false
        this.disabledSendDaysAfter = false
        this.disabledNone = false
      } else if (
        timingType === 'monthly' &&
        sendDayType === this.sendDayRadioItems.direct
      ) {
        this.disabledEndOfMonth = false
        this.disabledNextMonth = false
        this.disabledSendDaysAfter = false
        this.disabledNone = false
      } else if (
        timingType === 'once' &&
        sendDayType === this.sendDayRadioItems.manual
      ) {
        this.disabledEndOfMonth = true
        this.disabledNextMonth = false
        this.disabledSendDaysAfter = true
        this.disabledNone = true
      } else if (
        timingType === 'once' &&
        sendDayType === this.sendDayRadioItems.direct
      ) {
        this.disabledEndOfMonth = false
        this.disabledNextMonth = false
        this.disabledSendDaysAfter = false
        this.disabledNone = false
      }
    },
    /** 取得した日付が金曜・土曜である場合、翌週の月曜を返す*/
    getMondayOfNextWeek(sendDay: string, type: string) {
      // string型の日付をdate型に整形
      const formattedSendDay = new Date(sendDay)
      // 曜日を取得
      const weekDay = formattedSendDay.getDay()

      if (type === 'nextDay') {
        // 送信日初期値が当日の翌日の場合
        if (weekDay === 5) {
          // 金曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 2)
        } else if (weekDay === 6) {
          // 土曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 1)
        }
      } else if (type === 'today') {
        // 送信日初期値と当日が一致している場合
        if (weekDay === 5) {
          // 金曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 3)
        } else if (weekDay === 6) {
          // 土曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 2)
        } else {
          // 月曜から木曜の場合、翌日を取得・日曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 1)
        }
      } else if (type === 'thisMonthOrNextMonth') {
        if (weekDay === 6) {
          // 土曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 2)
        } else if (weekDay === 0) {
          // 日曜の場合、翌週の月曜日を取得
          formattedSendDay.setDate(formattedSendDay.getDate() + 1)
        }
      }

      // date型の日付をstring型に整形
      sendDay = format(new Date(formattedSendDay), 'yyyy/MM/dd')
      return sendDay
    },
  },
  watch: {
    localParam: {
      handler() {
        this.changeSurveyScheduleInfo()
        this.$logger.info('watch')
      },
      deep: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.radio-text {
  font-size: 0.875rem !important;
  color: $c-black;
  display: flex;
  align-items: center;
}
.m-survery-modal__unit {
  border-bottom: 1px solid $c-gray-line;
  color: $c-black;
  &:nth-child(n + 2) {
    margin-top: 20px;
  }
}
</style>
