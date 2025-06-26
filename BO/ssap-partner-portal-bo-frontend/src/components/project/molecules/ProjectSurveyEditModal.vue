<template>
  <ModalEdit :title="title">
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValid" @input="$listeners['input']">
        <v-row>
          <v-col cols="4" class="font-weight-bold">{{
            $t('project.pages.edit.survey.row.schedule.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- 送信日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.send.label')"
              :is-editing="isEditing"
              :value="localSurveySchedule.sendDate"
              cols="3"
              required
              class="is-noborder"
            >
              <RadioGroup
                v-model="sendDateType"
                class="mt-0 pt-0"
                @change="getSendDay"
              >
                <template #unique>
                  <v-radio
                    :value="sendDateTypeItems[1]"
                    :disabled="
                      answerLimitDayTiming === 'daysAfter' ||
                      answerLimitDayTiming === 'none' ||
                      answerLimitDayTiming === 'endOfMonth'
                        ? true
                        : false
                    "
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
                            v-model="inputParam.requestDate.daysBeforeDeadline"
                            :disabled="disabledDaysBefore"
                            :required="!disabledDaysBefore"
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
                  <v-radio :value="sendDateTypeItems[0]">
                    <template #label>
                      <DateSelect
                        v-model="inputParam.requestDate.day"
                        :disabled="disabledDirectSendDate"
                        required
                        :date="
                          localSurveySchedule.sendDate.replaceAll('/', '-')
                        "
                        :allowed-dates="allowedDates"
                        @change="isChanged = true"
                      />
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
            <!-- 回答期限日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.deadline.label')"
              :is-editing="isEditing"
              cols="3"
              required
              tall
              class="is-noborder"
            >
              <RadioGroup
                v-model="answerLimitDayTiming"
                required
                class="mt-0 pt-0"
                @change="getAnswerLimitDay"
              >
                <!-- 月末 -->
                <template #unique>
                  <v-radio
                    :value="answerLimitDayTimingItems[0]"
                    :disabled="sendDateType === 'auto'"
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
                  <!-- 送信日から〇日後 -->
                  <v-radio
                    :value="answerLimitDayTimingItems[1]"
                    :disabled="sendDateType === 'auto'"
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
                            v-model="inputParam.limitDate.daysAfterSent"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledLimitDayFromSend"
                            :required="
                              answerLimitDayTiming ===
                              answerLimitDayTimingItems[1]
                                ? true
                                : false
                            "
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="15"
                            @input="isChanged = true"
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
                  <v-radio :value="answerLimitDayTimingItems[2]">
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'project.pages.edit.survey.row.sub.deadline.radio.labels.3[0]'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="
                              inputParam.limitDate.daysAfterNextMonthStart
                            "
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
                            :disabled="disabledLimitDayFromStart"
                            :range-number-from="1"
                            :range-number-to="20"
                            min="1"
                            max="20"
                            positive-number
                            :max-digits="2"
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
                    :value="answerLimitDayTimingItems[3]"
                    :disabled="sendDateType === 'auto'"
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
        @click="$emit('click:closeEdit')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isChanged !== true || isValid !== true"
        @click="update"
      >
        {{ $t('common.button.save') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import {
  parse,
  startOfMonth,
  eachDayOfInterval,
  isWeekend,
  differenceInCalendarDays,
  isSameMonth,
  addDays,
  endOfMonth,
  isSameDay,
  subDays,
} from 'date-fns'
import ja from 'date-fns/locale/ja'
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
import BaseComponent from '~/common/BaseComponent'
import {
  UpdateSurveySchedulesByIdAndDateRequest,
  UpdateSurveySchedulesByIdAndDate,
} from '~/models/Schedule'

export class UpdateParams {
  sendDate: string = ''
  surveyLimitDate: number | null = null
}

class InputParam {
  requestDate = {
    daysBeforeDeadline: 7,
    day: '2000/1/1',
  }

  limitDate = {
    daysAfterSent: 1,
    daysAfterNextMonthStart: 2,
  }
}

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
    /** タイトル */
    title: {
      type: String,
      required: true,
    },
    /** 選択されたアンケート送信スケジュール情報 */
    modalSchedule: {
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
  data(): {
    isEditing: boolean
    isValid: boolean
    isChanged: boolean
    isUpdatingAnswerLimitDay: boolean
    sendDateType: string
    sendDateTypeItems: string[]
    localSurveySchedule: UpdateParams
    answerLimitDayTiming: string
    answerLimitDayTimingItems: string[]
    answerLimitDay: number | null
    daysAfter: number
    disabled: boolean
    disabledDaysBefore: boolean
    disabledDirectSendDate: boolean
    disabledLimitDayEndOfMonth: boolean
    disabledLimitDayFromSend: boolean
    disabledLimitDayFromStart: boolean
    inputParam: InputParam
    minSendDaySetting: number
    maxSendDaySetting: number
    minAnswerLimitDaySetting: number
    maxAnswerLimitDaySetting: number
  } {
    return {
      isEditing: true,
      isValid: false,
      isChanged: false,
      isUpdatingAnswerLimitDay: false,
      sendDateType: 'manual',
      sendDateTypeItems: ['manual', 'auto'],
      localSurveySchedule: new UpdateParams(),
      answerLimitDayTiming: 'nextMonth',
      answerLimitDayTimingItems: [
        'endOfMonth',
        'daysAfter',
        'nextMonth',
        'none',
      ],
      answerLimitDay: null,
      daysAfter: 0,
      disabled: false,
      //回答期限日から〇営業日前のテキストボックス活性/非活性
      disabledDaysBefore: true,
      //送信日（特定）のテキストボックス活性/非活性
      disabledDirectSendDate: false,
      // 月末最終営業日
      disabledLimitDayEndOfMonth: false,
      //送信日から〇営業日後のテキストボックス活性/非活性
      disabledLimitDayFromSend: false,
      //翌月月初〇営業日のテキストボックス活性/非活性
      disabledLimitDayFromStart: false,

      inputParam: new InputParam(),
      // 送信日入力範囲
      minSendDaySetting: 1,
      maxSendDaySetting: 20,
      // 回答期限日入力範囲
      minAnswerLimitDaySetting: 1,
      maxAnswerLimitDaySetting: 15,
    }
  },
  created() {
    this.answerLimitDay = this.getDifferenceInCalendarDays()
    this.localSurveySchedule.surveyLimitDate =
      this.getDifferenceInCalendarDays()
    this.localSurveySchedule.sendDate = this.modalSchedule.sendDate

    this.setInitDate()
  },
  methods: {
    /**
     * アンケート送信日
     * @param elm 回答期限日文字列
     */
    getSendDay(elm: string): void {
      if (elm === 'auto') {
        // 回答期限日から〇営業日前
        this.disabledDaysBefore = false
        this.disabledDirectSendDate = true
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayEndOfMonth = true
      } else {
        // 送信日（特定）
        this.disabledDaysBefore = true
        this.disabledDirectSendDate = false
        this.disabledLimitDayFromSend = false
        this.disabledLimitDayEndOfMonth = false
      }
    },
    /**
     * 送信日、送信日から●日後の値または当日との差を返す
     * @returns 送信日、送信日から●日後の値または当日との差
     */
    getDifferenceInCalendarDays(): number | null {
      if (
        this.modalSchedule.surveyLimitDate === null ||
        this.modalSchedule.surveyLimitDate === '' ||
        this.modalSchedule.surveyLimitDate === 0
      ) {
        return null
      } else {
        return differenceInCalendarDays(
          this.modalSchedule.surveyLimitDate
            ? new Date(this.modalSchedule.surveyLimitDate)
            : getCurrentDate(),
          new Date(this.modalSchedule.sendDate)
        )
      }
    },
    /** 送信日から●日後の期限 */
    getAnswerLimitDay(elm: string) {
      this.isChanged = true
      if (elm === 'endOfMonth') {
        // 月末最終営業日の場合
        this.localSurveySchedule.surveyLimitDate = 99
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayFromStart = true
      } else if (elm === 'none') {
        // なしの場合
        this.localSurveySchedule.surveyLimitDate = 0
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayFromStart = true
      } else if (elm === 'daysAfter') {
        // 送信日から〇営業日後の場合
        this.localSurveySchedule.surveyLimitDate = this.answerLimitDay
        this.disabledLimitDayFromSend = false
        this.disabledLimitDayFromStart = true
      } else if (elm === 'nextMonth') {
        // 送信日の翌月月初〇営業日の場合
        this.localSurveySchedule.surveyLimitDate = this.answerLimitDay
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayFromStart = false
      }
    },
    /** アンケート送信スケジュールを更新 */
    async update() {
      this.clearErrorBar()
      const surveyId = this.modalSchedule.surveyId
      const version = this.modalSchedule.version
      const request: UpdateSurveySchedulesByIdAndDateRequest = this
        .localSurveySchedule as UpdateSurveySchedulesByIdAndDateRequest
      const detailPage = '/project/' + this.$route.params.projectId

      await UpdateSurveySchedulesByIdAndDate(surveyId, version, request)
        .then(() => {
          this.$emit('click:closeEdit')
          this.$router.push(detailPage)
          this.$emit('refresh')
        })
        .catch((error) => {
          if (
            error.response.data.detail ===
            'You cannot set a send date that is earlier than the current date.'
          ) {
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.edit.survey.errorMessage')
            )
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
          this.$emit('click:closeEdit')
          this.$router.push(detailPage)
        })
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
      const minimumAllowedDay = this.getMinimumAllowedDay()

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
     * 月初から何営業日経過しているかを返す
     */
    daysSinceStartOfMonth(dateString: string): number {
      if (dateString) {
        const date = parse(dateString, 'yyyy/MM/dd', new Date(), { locale: ja })
        const start = startOfMonth(date)
        const dateRange = eachDayOfInterval({ start, end: date })

        let count = 0
        for (const day of dateRange) {
          if (!isWeekend(day)) {
            count++
          }
        }

        return count
      } else {
        return 1
      }
    },
    /**
     * 送信日と回答期限日が同じ月かどうかを返す
     * @param sendDate 送信日
     * @param surveyLimitDate 回答期限日
     */
    areDatesInSameMonth(sendDate: string, surveyLimitDate: string): boolean {
      const parsedSendDate = parse(sendDate, 'yyyy/MM/dd', new Date(), {
        locale: ja,
      })
      const parsedSurveyLimitDate = parse(
        surveyLimitDate,
        'yyyy/MM/dd',
        new Date(),
        { locale: ja }
      )

      return isSameMonth(parsedSendDate, parsedSurveyLimitDate)
    },
    /**
     * 送信日と回答期限日の間に何営業日あるかを返す
     * @param sendDate 送信日
     * @param surveyLimitDate 回答期限日
     */
    getDaysBetweenDates(sendDate: string, surveyLimitDate: string): number {
      const parsedSendDate = parse(sendDate, 'yyyy/MM/dd', new Date(), {
        locale: ja,
      })
      const parsedSurveyLimitDate = parse(
        surveyLimitDate,
        'yyyy/MM/dd',
        new Date(),
        { locale: ja }
      )

      let dayCount = 0

      for (
        let date = parsedSendDate;
        date < parsedSurveyLimitDate;
        date = addDays(date, 1)
      ) {
        if (!isWeekend(date)) {
          dayCount++
        }
      }

      return dayCount
    },
    /**
     * 特定の日が土日を抜いた月末の最終営業日かどうかを返す
     * @param targetDate 文字列の日付
     */
    isLastBusinessDayOfMonth(targetDate: string): boolean {
      const parsedDate = parse(targetDate, 'yyyy/MM/dd', new Date(), {
        locale: ja,
      })

      // dateが週末であればfalseを返す
      if (isWeekend(parsedDate)) {
        return false
      }

      // その月の最終日から遡り、最初の営業日を見つける
      let endDayOfMonth = endOfMonth(parsedDate)
      while (isWeekend(endDayOfMonth)) {
        endDayOfMonth = subDays(endDayOfMonth, 1)
      }

      // dateがその月の最終営業日であればtrueを返す
      if (isSameDay(parsedDate, endDayOfMonth)) {
        return true
      }

      return false
    },
    /**
     * リクエストパラメーター
     */
    setLocalParam() {
      this.isChanged = true

      // --- 送信日 ---

      // 回答期限日から○営業日前の場合
      if (this.sendDateType === this.sendDateTypeItems[1]) {
        this.localSurveySchedule.sendDate =
          '-' + String(this.inputParam.requestDate.daysBeforeDeadline)
      }

      // 日付の場合
      if (this.sendDateType === this.sendDateTypeItems[0]) {
        this.localSurveySchedule.sendDate = this.inputParam.requestDate.day
      }

      // --- 回答期限日 ---

      // 送信日の月末最終営業日の場合
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[0]) {
        this.localSurveySchedule.surveyLimitDate = 99
      }

      // 送信日から○営業日後の場合
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[1]) {
        this.localSurveySchedule.surveyLimitDate = Number(
          this.inputParam.limitDate.daysAfterSent
        )
      }

      // 送信日の翌月月初○営業日
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[2]) {
        this.localSurveySchedule.surveyLimitDate =
          Number(this.inputParam.limitDate.daysAfterNextMonthStart) + 100
      }

      // なし
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[3]) {
        this.localSurveySchedule.surveyLimitDate = 0
      }
    },
    /**
     * 送信日初期設定・回答期限日初期設定
     * @param targetId survey-masters[]内のID
     */
    setInitDate() {
      // 送信日

      // 入力フォーム用
      this.inputParam.requestDate.day = this.modalSchedule.sendDate
      // リクエストパラメーター用
      this.localSurveySchedule.sendDate = this.modalSchedule.sendDate

      // 回答期限日
      if (
        this.localSurveySchedule.surveyLimitDate === null ||
        this.localSurveySchedule.surveyLimitDate === 0
      ) {
        // なしの場合
        this.localSurveySchedule.surveyLimitDate = 0
        this.answerLimitDayTiming = 'none'

        this.disabledLimitDayFromSend = true
        this.disabledLimitDayEndOfMonth = true
      } else if (
        this.areDatesInSameMonth(
          this.modalSchedule.sendDate,
          this.modalSchedule.surveyLimitDate
        )
      ) {
        ///////////////////////////////
        //送信日と回答期限日が同じ月の場合
        ///////////////////////////////
        if (this.isLastBusinessDayOfMonth(this.modalSchedule.surveyLimitDate)) {
          // 送信日の月末最終営業日の場合

          // --- ラジオボタン ---
          this.answerLimitDayTiming = this.answerLimitDayTimingItems[0]

          // --- リクエストパラメーター用 ---
          this.localSurveySchedule.surveyLimitDate = 99

          this.disabledLimitDayFromSend = true
          this.disabledLimitDayFromStart = true
        } else {
          // --- ラジオボタン ---
          this.answerLimitDayTiming = this.answerLimitDayTimingItems[1]

          // --- 入力フォーム ---

          // 送信日から○営業日後の場合
          this.inputParam.limitDate.daysAfterSent = this.getDaysBetweenDates(
            this.modalSchedule.sendDate,
            this.modalSchedule.surveyLimitDate
          )

          // 送信日の翌月月初○営業日には初期値を表示
          this.inputParam.limitDate.daysAfterNextMonthStart = 2

          // --- リクエストパラメーター用 ---
          this.localSurveySchedule.surveyLimitDate = this.getDaysBetweenDates(
            this.modalSchedule.sendDate,
            this.modalSchedule.surveyLimitDate
          )

          this.disabledLimitDayFromSend = false
          this.disabledLimitDayFromStart = true
        }
      } else {
        ///////////////////////////////
        //送信日と回答期限日が異なる月の場合
        ///////////////////////////////

        // --- ラジオボタン ---
        this.answerLimitDayTiming = this.answerLimitDayTimingItems[2]

        // --- 入力フォーム ---

        // 送信日から○営業日後には初期値を表示
        this.inputParam.limitDate.daysAfterSent = 1

        // 送信日の翌月月初○営業日の場合
        this.inputParam.limitDate.daysAfterNextMonthStart =
          this.daysSinceStartOfMonth(this.modalSchedule.surveyLimitDate)

        // --- リクエストパラメーター用 ---
        this.localSurveySchedule.surveyLimitDate =
          this.daysSinceStartOfMonth(this.modalSchedule.surveyLimitDate) + 100

        this.disabledLimitDayFromSend = true
        this.disabledLimitDayFromStart = false
      }
    },
    // 送信日の入力値を補正
    sendDaySettingValidationCorrect() {
      // 回答期限から◯営業日前：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.inputParam.requestDate.daysBeforeDeadline < this.minSendDaySetting
      ) {
        // 入力値が下限よりも小さい場合
        this.inputParam.requestDate.daysBeforeDeadline = this.minSendDaySetting
      }
      if (
        this.inputParam.requestDate.daysBeforeDeadline > this.maxSendDaySetting
      ) {
        // 入力値が上限より大きい場合
        this.inputParam.requestDate.daysBeforeDeadline = this.maxSendDaySetting
      }
    },
    // 回答期限日の入力値を補正
    answerLimitDaySettingValidationCorrect() {
      // 送信日から◯営業日後：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[1]) {
        if (
          this.inputParam.limitDate.daysAfterSent <
          this.minAnswerLimitDaySetting
        ) {
          // 入力値が下限よりも小さい場合
          this.inputParam.limitDate.daysAfterSent =
            this.minAnswerLimitDaySetting
        }
        if (
          this.inputParam.limitDate.daysAfterSent >
          this.maxAnswerLimitDaySetting
        ) {
          // 入力値が上限より大きい場合
          this.inputParam.limitDate.daysAfterSent =
            this.maxAnswerLimitDaySetting
        }
      }

      // 送信日の翌月月初◯営業日：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (this.answerLimitDayTiming === this.answerLimitDayTimingItems[2]) {
        if (
          this.inputParam.limitDate.daysAfterNextMonthStart <
          this.minAnswerLimitDaySetting
        ) {
          // 入力値が下限よりも小さい場合
          this.inputParam.limitDate.daysAfterNextMonthStart =
            this.minAnswerLimitDaySetting
        }
        if (
          this.inputParam.limitDate.daysAfterNextMonthStart >
          this.maxSendDaySetting
        ) {
          // 入力値が上限より大きい場合
          this.inputParam.limitDate.daysAfterNextMonthStart =
            this.maxSendDaySetting
        }
      }
    },
  },
  watch: {
    sendDateType: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    answerLimitDayTiming: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    inputParam: {
      handler() {
        this.setLocalParam()
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
