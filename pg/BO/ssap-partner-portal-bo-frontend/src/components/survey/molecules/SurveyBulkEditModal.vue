<template>
  <ModalEdit :title="title">
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValid" @input="$listeners['input']">
        <v-row>
          <v-col cols="3" class="font-weight-bold">{{
            $t('project.pages.edit.support.row.schedule.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- 送信日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.send.label')"
              :is-editing="true"
              :value="localSurveyBulkEdit.sendDate"
              cols="4"
              required
              class="is-noborder"
            >
              <RadioGroup
                v-model="localSurveyBulkEdit.sendDayTiming"
                required
                class="mt-0 pt-0"
                @change="getSendDay"
              >
                <!-- 回答期限日から◯営業日前 -->
                <template #unique>
                  <v-radio
                    :value="sendDayTimingItems.daysBefore"
                    :disabled="isDisabledRadioSendDays"
                  >
                    <template #label>
                      <span class="d-flex align-center radio-text">
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadlineDate'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="localSurveyBulkEdit.sendDays"
                            :disabled="disabledDaysBefore"
                            :required="!disabledDaysBefore"
                            :range-number-from="1"
                            :range-number-to="20"
                            :max-digits="2"
                            positive-number
                            :min="minSendDaySetting"
                            :max="maxSendDaySetting"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            @focusout="sendDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.businessDayBefore'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- 特定日時入力 -->
                  <v-radio :value="sendDayTimingItems.direct">
                    <template #label>
                      <DateSelect
                        v-model="localSurveyBulkEdit.sendDate"
                        :disabled="disabledDirectSendDate"
                        :required="!disabledDirectSendDate"
                        :date="
                          localSurveyBulkEdit.sendDate.replaceAll('/', '-')
                        "
                        :allowed-dates="allowedDates"
                      />
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
            <!-- 回答期限日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.deadline.label')"
              :is-editing="true"
              cols="4"
              required
              tall
              class="is-noborder"
            >
              <RadioGroup
                v-model="localSurveyBulkEdit.limitDayTiming"
                required
                class="mt-0 pt-0"
                @change="getAnswerLimitDay"
              >
                <!-- 月末 -->
                <template #unique>
                  <v-radio
                    :value="limitDayTimingItems.endOfMonth"
                    :disabled="disabledLimitDayEndOfMonth"
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.1'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- 送信日から〇営業日後 -->
                  <v-radio
                    :value="limitDayTimingItems.daysAfter"
                    :disabled="
                      localSurveyBulkEdit.sendDayTiming === 'daysBefore'
                    "
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.2[0]'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="limitDayFromSend"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledLimitDayFromSend"
                            :required="!disabledLimitDayFromSend"
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            :min="minAnswerLimitDaySetting"
                            :max="maxAnswerLimitDaySetting"
                            @focusout="answerLimitDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.2[1]'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- 翌月月初から〇営業日 -->
                  <v-radio :value="limitDayTimingItems.firstOfMonth">
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.3[0]'
                          )
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-model="limitDayFromStart"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledLimitDayFromStart"
                            :required="!disabledLimitDayFromStart"
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            :min="minAnswerLimitDaySetting"
                            :max="maxAnswerLimitDaySetting"
                            @focusout="answerLimitDaySettingValidationCorrect"
                          />
                        </Sheet>
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.3[1]'
                          )
                        }}
                      </span>
                    </template>
                  </v-radio>
                  <!-- なし -->
                  <v-radio
                    :value="limitDayTimingItems.none"
                    :disabled="
                      localSurveyBulkEdit.sendDayTiming === 'daysBefore'
                    "
                  >
                    <template #label>
                      <span class="radio-text">
                        {{
                          $t(
                            'survey.pages.forecast.scheduleModal.bulkEditModal.send.deadline.radio.labels.4'
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
        @click="$emit('click:closeBulkEdit')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <!-- 保存ボタン -->
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="!isValid"
        :loading="isSaving"
        @click="updateMultipleSurveySchedules"
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
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import BaseComponent from '~/common/BaseComponent'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import {
  UpdateMultipleSurveySchedules,
  UpdateMultipleSurveySchedulesRequest,
} from '~/models/Schedule'
import { SurveyPlanListItem } from '~/models/Survey'
import type { PropType } from '~/common/BaseComponent'

export class EditParams {
  sendDays: number = 0
  sendDate: string = ''
  sendDayTiming: string = ''
  limitDays: number = 0
  limitDayTiming: string = ''
}

// 500件ずつに分割する
function splitAssociativeArray(arr: any, chunkSize: number): any[] {
  const transformedData = []
  const combinedArray = arr.add.concat(arr.update)

  for (let i = 0; i < combinedArray.length; i += chunkSize) {
    const splitArray = combinedArray.slice(i, i + chunkSize)
    const addArray = splitArray.filter((item: any) => arr.add.includes(item))
    const updateArray = splitArray.filter((item: any) =>
      arr.update.includes(item)
    )

    const obj = {
      add: addArray,
      update: updateArray,
      sendDate: arr.sendDate,
      surveyLimitDate: arr.surveyLimitDate,
    }
    transformedData.push(obj)
  }

  return transformedData
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
    DateSelect,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
      required: true,
    },
    /** 選択したアンケート **/
    selectedSurveys: {
      type: Array as PropType<SurveyPlanListItem[]>,
    },
  },
  data() {
    // 送信日(特定)に当月24日を設定（※当日>= 当月24日の場合翌月24日を取得）
    const today = new Date()
    let sendDate: Date
    if (today.getDate() >= 24) {
      sendDate = new Date(today.getUTCFullYear(), today.getMonth() + 1, 24)
    } else {
      sendDate = new Date(today.getUTCFullYear(), today.getMonth(), 24)
    }

    const sendDateStr = format(sendDate, 'yyyy/MM/dd')

    return {
      isValid: false,

      // 送信日ラジオボタン
      sendDayTimingItems: {
        daysBefore: 'daysBefore',
        direct: 'direct',
      },

      // 回答期限日ラジオボタン
      limitDayTimingItems: {
        endOfMonth: 'endOfMonth',
        daysAfter: 'daysAfter',
        firstOfMonth: 'firstOfMonth',
        none: 'none',
      },

      // ローカルの送信日・回答期限日
      localSurveyBulkEdit: {
        sendDays: 7,
        sendDate: sendDateStr,
        sendDayTiming: 'daysBefore',
        limitDays: 0,
        limitDayTiming: 'firstOfMonth',
      } as EditParams,

      // 送信日から〇営業日後のフォームにv-model
      limitDayFromSend: 1,
      // 翌月月初から〇営業日のフォームにv-model
      limitDayFromStart: 2,

      //回答期限日から〇営業日前のテキストボックス活性/非活性
      disabledDaysBefore: false,
      //送信日（特定）のテキストボックス活性/非活性
      disabledDirectSendDate: false,
      // 月末最終営業日
      disabledLimitDayEndOfMonth: false,
      //送信日から〇営業日後のテキストボックス活性/非活性
      disabledLimitDayFromSend: false,
      //翌月月初〇営業日のテキストボックス活性/非活性
      disabledLimitDayFromStart: false,
      isSaving: false,

      // 送信日入力範囲
      minSendDaySetting: 1,
      maxSendDaySetting: 20,
      // 回答期限日入力範囲
      minAnswerLimitDaySetting: 1,
      maxAnswerLimitDaySetting: 15,
    }
  },
  created() {
    this.getSendDay(this.localSurveyBulkEdit.sendDayTiming)
    this.getAnswerLimitDay(this.localSurveyBulkEdit.limitDayTiming)
  },
  watch: {
    localSurveyBulkEdit: {
      deep: true,
      handler() {
        this.getSendDay(this.localSurveyBulkEdit.sendDayTiming)
        this.getAnswerLimitDay(this.localSurveyBulkEdit.limitDayTiming)
      },
    },
  },
  methods: {
    /**
     * アンケート送信日
     * @param elm 回答期限日文字列
     */
    getSendDay(elm: string): void {
      if (elm === 'daysBefore') {
        // 回答期限日から〇営業日前

        this.disabledDaysBefore = false
        this.disabledDirectSendDate = true
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayEndOfMonth = true

        // 回答期限日の「翌月月初〇営業日」が選ばれるように
        this.localSurveyBulkEdit.limitDayTiming =
          this.limitDayTimingItems.firstOfMonth

        if (
          this.localSurveyBulkEdit.limitDayTiming === 'daysAfter' ||
          this.localSurveyBulkEdit.limitDayTiming === 'none'
        ) {
          this.localSurveyBulkEdit.limitDayTiming = ''
        }
      } else {
        // 送信日（特定）
        this.disabledDaysBefore = true
        this.disabledDirectSendDate = false
        this.disabledLimitDayFromSend = false
        this.disabledLimitDayEndOfMonth = false
      }
    },
    /**
     * アンケート期限日
     * @param elm 回答期限日文字列
     */
    getAnswerLimitDay(elm: string) {
      if (elm === 'endOfMonth') {
        // 月末最終営業日
        this.localSurveyBulkEdit.limitDays = 99
        this.disabledLimitDayFromSend = this.disabledLimitDayFromStart = true
      } else if (elm === 'none') {
        // なし
        this.localSurveyBulkEdit.limitDays = 0
        this.disabledLimitDayFromSend = this.disabledLimitDayFromStart = true

        this.disabledDaysBefore = true
        this.disabledDirectSendDate = false
        if (this.localSurveyBulkEdit.sendDayTiming === 'daysBefore') {
          this.localSurveyBulkEdit.sendDayTiming = ''
        }
      } else if (elm === 'daysAfter') {
        // 送信日から〇営業日後
        this.localSurveyBulkEdit.limitDays = this.limitDayFromSend
        this.disabledLimitDayFromSend = false
        this.disabledLimitDayFromStart = true

        this.disabledDaysBefore = true
        this.disabledDirectSendDate = false
        if (this.localSurveyBulkEdit.sendDayTiming === 'daysBefore') {
          this.localSurveyBulkEdit.sendDayTiming = ''
        }
      } else {
        //翌月月初〇営業日
        this.localSurveyBulkEdit.limitDays = this.limitDayFromSend
        this.disabledLimitDayFromSend = true
        this.disabledLimitDayFromStart = false
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
      // 送信日は現在日より先かつ支援終了日までしか選択できない
      const now = getCurrentDate()

      return now < new Date(val)
    },
    // 送信日の入力値を補正
    sendDaySettingValidationCorrect() {
      // 回答期限から◯営業日前：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (this.localSurveyBulkEdit.sendDays < this.minSendDaySetting) {
        // 入力値が下限よりも小さい場合
        this.localSurveyBulkEdit.sendDays = this.minSendDaySetting
      }
      if (this.localSurveyBulkEdit.sendDays > this.maxSendDaySetting) {
        // 入力値が上限より大きい場合
        this.localSurveyBulkEdit.sendDays = this.maxSendDaySetting
      }
    },
    // 回答期限日の入力値を補正
    answerLimitDaySettingValidationCorrect() {
      // 送信日から◯営業日後：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localSurveyBulkEdit.limitDayTiming ===
        this.limitDayTimingItems.daysAfter
      ) {
        if (this.limitDayFromSend < this.minAnswerLimitDaySetting) {
          // 入力値が下限よりも小さい場合
          this.limitDayFromSend = this.minAnswerLimitDaySetting
        }
        if (this.limitDayFromSend > this.maxAnswerLimitDaySetting) {
          // 入力値が上限より大きい場合
          this.limitDayFromSend = this.maxAnswerLimitDaySetting
        }
      }

      // 送信日の翌月月初◯営業日：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      if (
        this.localSurveyBulkEdit.limitDayTiming ===
        this.limitDayTimingItems.firstOfMonth
      ) {
        if (this.limitDayFromStart < this.minAnswerLimitDaySetting) {
          // 入力値が下限よりも小さい場合
          this.limitDayFromStart = this.minAnswerLimitDaySetting
        }
        if (this.limitDayFromStart > this.maxAnswerLimitDaySetting) {
          // 入力値が上限より大きい場合
          this.limitDayFromStart = this.maxAnswerLimitDaySetting
        }
      }
    },
    // スケジュール一括更新リクエストの整形
    generateUpdateRequest(): UpdateMultipleSurveySchedulesRequest {
      let sendDate: string
      let surveyLimitDate: Number
      const add: string[] = []
      const update: string[] = []

      // 送信日
      if (
        this.localSurveyBulkEdit.sendDayTiming ===
        this.sendDayTimingItems.daysBefore
      ) {
        // 回答期限日から〇営業日前
        sendDate = `-${this.localSurveyBulkEdit.sendDays}`
      } else {
        // 送信日(特定)
        sendDate = this.localSurveyBulkEdit.sendDate
      }

      // 回答期限日
      if (
        this.localSurveyBulkEdit.limitDayTiming ===
        this.limitDayTimingItems.endOfMonth
      ) {
        // 月末最終営業日
        surveyLimitDate = 99
      } else if (
        this.localSurveyBulkEdit.limitDayTiming ===
        this.limitDayTimingItems.daysAfter
      ) {
        // 送信日から〇営業日後
        surveyLimitDate = this.limitDayFromSend
      } else if (
        this.localSurveyBulkEdit.limitDayTiming ===
        this.limitDayTimingItems.firstOfMonth
      ) {
        // 翌月月初〇営業日
        surveyLimitDate = Number(this.limitDayFromStart as number) + 100
      } else {
        // なし
        surveyLimitDate = 0
      }

      this.selectedSurveys.forEach((survey) => {
        // 送信実績があるアンケートはリクエストから排除
        if (survey.actualSurveyRequestDatetime) {
          return
        }
        // 回答実績があるアンケートはリクエストから排除
        if (survey.actualSurveyResponseDatetime) {
          return
        }

        if (survey.id) {
          // サービスアンケート・修了アンケートはアンケートidをリクエストに追加
          update.push(survey.id)
        } else {
          // 「送付予定なし」アンケートは案件idをリクエストに追加
          add.push(survey.projectId)
        }
      })

      const rtn = new UpdateMultipleSurveySchedulesRequest(
        sendDate,
        surveyLimitDate,
        add,
        update
      )

      return rtn
    },
    //保存ボタン押下後の更新処理
    async updateMultipleSurveySchedules() {
      // 保存ボタンをローディング状態に
      this.isSaving = true

      const request = this.generateUpdateRequest()

      const splittedAssociativeArray = splitAssociativeArray(request, 500)

      //DeleteMultipleSurveySchedules実行関数
      await UpdateMultipleSurveySchedules(splittedAssociativeArray)
        .then(() => {
          this.isSaving = false
          this.$emit('click:save')
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
          this.isSaving = false
          this.$emit('click:save')
        })
    },
  },
  computed: {
    isDisabledRadioSendDays(): boolean {
      return (
        this.localSurveyBulkEdit.limitDayTiming === 'daysAfter' ||
        this.localSurveyBulkEdit.limitDayTiming === 'none' ||
        this.localSurveyBulkEdit.limitDayTiming === 'endOfMonth'
      )
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
