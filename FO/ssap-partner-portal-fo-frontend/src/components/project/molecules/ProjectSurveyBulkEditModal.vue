<template>
  <ModalEdit :title="title">
    <!-- サービスアンケート -->
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValidService" @input="$listeners['input']">
        <v-row>
          <v-col cols="3" class="font-weight-bold">{{
            $t('project.pages.edit.survey.row.service.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- 送信日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.send.label')"
              :is-editing="isEditing"
              :value="localSurveyBulkEdit.service.requestDate"
              cols="4"
              required
              class="is-noborder"
              tall
            >
              <RadioGroup
                v-model="serviceAnswerRequestDateType"
                class="mt-0 pt-0"
                @change="changeServiceSendDaysRadio"
              >
                <template #unique>
                  <!-- :disabled="answerLimitDayTiming === 'daysAfter'" -->
                  <v-radio
                    :value="serviceAnswerRequestDateTypeItems[1]"
                    :disabled="
                      serviceAnswerLimitDayTiming === 'daysAfter' ||
                      serviceAnswerLimitDayTiming === 'none'
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
                            v-model="
                              serviceInput.requestDate.daysBeforeDeadline
                            "
                            required
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
                            :max-digits="2"
                            min="1"
                            max="20"
                            positive-number
                            :disabled="disabledServiceSendDayBeforeDeadline"
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
                  <v-radio :value="serviceAnswerRequestDateTypeItems[0]">
                    <template #label>
                      <!-- 毎月〇日（終了月除く） -->
                      <span class="radio-text">
                        {{ $t('project.pages.edit.survey.row.sub.send.text1') }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-if="isUpdatingServiceRequestDay === false"
                            v-model="serviceInput.requestDate.dayEveryMonth"
                            required
                            :range-number-from="1"
                            :range-number-to="31"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="31"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledServiceSendDayEveryMonth"
                            @focusout="sentDayEveryMonthCorrect"
                          />
                        </Sheet>
                        {{ $t('project.pages.edit.survey.row.sub.send.text3') }}
                      </span>
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
            <!-- 回答期限日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.deadline.label')"
              :is-editing="isEditing"
              cols="4"
              required
              tall
              class="is-noborder"
            >
              <RadioGroup
                v-model="serviceAnswerLimitDayTiming"
                required
                class="mt-0 pt-0"
                @change="getServiceAnswerLimitDay"
              >
                <!-- 月末 -->
                <template #unique>
                  <v-radio :value="answerLimitDayTimingItems[0]">
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
                    :disabled="serviceAnswerRequestDateType === 'auto'"
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
                            v-if="isUpdatingServiceAnswerLimitDay === false"
                            v-model="serviceInput.limitDate.daysAfterSent"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="
                              disabledServiceAnswerLimitDayAfterSendDays
                            "
                            :required="
                              !disabledServiceAnswerLimitDayAfterSendDays
                            "
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="15"
                            @change="getServiceAnswerLimitDay"
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
                              serviceInput.limitDate.daysAfterNextMonthStart
                            "
                            type="number"
                            number
                            :required="
                              serviceAnswerLimitDayTiming === 'nextMonth'
                                ? true
                                : false
                            "
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledServiceAnswerLimitDayNextMonth"
                            :range-number-from="1"
                            :range-number-to="15"
                            min="1"
                            max="15"
                            :max-digits="2"
                            positive-number
                            @change="getServiceAnswerLimitDay"
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
                    :disabled="serviceAnswerRequestDateType === 'auto'"
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
    <!-- 修了アンケート -->
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValidCompletion" @input="$listeners['input']">
        <v-row>
          <v-col cols="3" class="font-weight-bold">{{
            $t('project.pages.edit.survey.row.completion.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- 送信日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.send.label')"
              :is-editing="isEditing"
              :value="localSurveyBulkEdit.completion.requestDate"
              cols="4"
              required
              class="is-noborder"
              tall
            >
              <RadioGroup
                v-model="completionAnswerRequestDateType"
                class="mt-0 pt-0"
                @change="changeCompletionSendDaysRadio"
              >
                <template #unique>
                  <!-- :disabled="answerLimitDayTiming === 'daysAfter'" -->
                  <v-radio
                    :value="serviceAnswerRequestDateTypeItems[1]"
                    :disabled="
                      completionAnswerLimitDayTiming === 'daysAfter' ||
                      completionAnswerLimitDayTiming === 'none'
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
                            v-model="
                              completionInput.requestDate.daysBeforeDeadline
                            "
                            required
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
                            :disabled="disabledCompletionSendDayBeforeDeadline"
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
                  <v-radio :value="serviceAnswerRequestDateTypeItems[0]">
                    <template #label>
                      <!-- 終了月〇日 -->
                      <span class="radio-text">
                        {{
                          $t('project.pages.edit.survey.row.sub.send.endMonth')
                        }}
                        <Sheet width="60" class="mx-2">
                          <TextField
                            v-if="isUpdatingCompletionRequestDay === false"
                            v-model="completionInput.requestDate.dayEveryMonth"
                            required
                            :range-number-from="1"
                            :range-number-to="31"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="31"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="disabledCompletionSendDayEveryMonth"
                            @focusout="sentDayEveryMonthCorrect"
                          />
                        </Sheet>
                        {{ $t('project.pages.edit.survey.row.sub.send.date') }}
                      </span>
                    </template>
                  </v-radio>
                </template>
              </RadioGroup>
            </CommonDetailRow>
            <!-- 回答期限日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.survey.row.sub.deadline.label')"
              :is-editing="isEditing"
              :value="localSurveyBulkEdit.completion.limitDate"
              cols="4"
              required
              tall
              class="is-noborder"
            >
              <RadioGroup
                v-model="completionAnswerLimitDayTiming"
                required
                class="mt-0 pt-0"
                @change="getCompletionAnswerLimitDay"
              >
                <!-- 月末 -->
                <template #unique>
                  <v-radio :value="answerLimitDayTimingItems[0]">
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
                    :disabled="completionAnswerRequestDateType === 'auto'"
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
                            v-if="isUpdatingCompletionAnswerLimitDay === false"
                            v-model="completionInput.limitDate.daysAfterSent"
                            type="number"
                            number
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="
                              disabledCompletionAnswerLimitDayAfterSendDays
                            "
                            :required="
                              !disabledCompletionAnswerLimitDayAfterSendDays
                            "
                            :range-number-from="1"
                            :range-number-to="15"
                            :max-digits="2"
                            positive-number
                            min="1"
                            max="15"
                            @change="getCompletionAnswerLimitDay"
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
                              completionInput.limitDate.daysAfterNextMonthStart
                            "
                            type="number"
                            number
                            :required="
                              completionAnswerLimitDayTiming === 'nextMonth'
                                ? true
                                : false
                            "
                            style-set="outlined"
                            :placeholder="
                              $t(
                                'project.pages.edit.survey.row.sub.send.placeholder'
                              )
                            "
                            :disabled="
                              disabledCompletionAnswerLimitDayNextMonth
                            "
                            :range-number-from="1"
                            :range-number-to="15"
                            min="1"
                            max="15"
                            :max-digits="2"
                            positive-number
                            @change="getCompletionAnswerLimitDay"
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
                    :disabled="completionAnswerRequestDateType === 'auto'"
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
        @click="$emit('click:closeBulkEdit')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <!-- 保存ボタン -->
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isValidCompletion !== true || isValidService !== true"
        @click="$emit('click:save', localSurveyBulkEdit)"
      >
        {{ $t('common.button.save') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
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
import type { PropType } from '~/common/BaseComponent'
import { SurveyMasterItem } from '~/models/Master'

export class EditParams {
  requestDate: number = 0
  limitDate: number = 0
}

export class SurveyBulkEdit {
  service: EditParams = new EditParams()
  completion: EditParams = new EditParams()
}

/**
 * 空の値、数値以外、数値範囲下限を下回る場合は下限に合わせる
 * @param num 入力値
 * @param min 最小値
 * @param max 最大値
 * @return 補正済み数値
 */
function adjustNumber(num: number, min: number, max: number) {
  // 0で始まる値の場合に先頭の0を消去する
  num = parseInt(String(num))
  if (!num || isNaN(num) || Number(num) < Number(min)) {
    return min
  }

  if (Number(num) > Number(max)) {
    return max
  }

  return num
}

function adjustDay(day: number) {
  return adjustNumber(day, 1, 31)
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
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
      required: true,
    },
    /** 最新バージョンのアンケートひな形の一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterItem[]>,
      required: true,
    },
  },
  data() {
    return {
      isEditing: true,
      isValidService: false,
      isValidCompletion: false,
      isUpdatingServiceRequestDay: false,
      isUpdatingServiceAnswerLimitDay: false,
      isUpdatingCompletionRequestDay: false,
      isUpdatingCompletionAnswerLimitDay: false,
      serviceAnswerLimitDayTiming: 'nextMonth',
      completionAnswerLimitDayTiming: 'nextMonth',
      completionAnswerRequestDateType: 'auto',
      serviceAnswerRequestDateType: 'auto',
      answerLimitDayTimingItems: [
        'endOfMonth',
        'daysAfter',
        'nextMonth',
        'none',
      ],
      serviceAnswerRequestDateTypeItems: ['manual', 'auto'],
      daysAfter: 0,
      localSurveyBulkEdit: {
        service: {
          requestDate: 0,
          limitDate: 0,
        },
        completion: {
          requestDate: 0,
          limitDate: 0,
        },
      } as SurveyBulkEdit,
      serviceAnswerLimitDay: 0,
      completionAnswerLimitDay: 0,
      // サービスアンケート「回答期限日から〇営業日前」
      disabledServiceSendDayBeforeDeadline: true,
      // サービスアンケート「毎月〇日」
      disabledServiceSendDayEveryMonth: false,
      // サービスアンケート「送信日から〇営業日後」
      disabledServiceAnswerLimitDayAfterSendDays: true,
      // サービスアンケート「翌月月初〇営業日」
      disabledServiceAnswerLimitDayNextMonth: false,
      // 修了アンケート「回答期限日から〇営業日前」
      disabledCompletionSendDayBeforeDeadline: true,
      // 修了アンケート「修了月〇日」
      disabledCompletionSendDayEveryMonth: false,
      // 修了アンケート「送信日から〇営業日後」
      disabledCompletionAnswerLimitDayAfterSendDays: true,
      // 修了アンケート「翌月月初〇営業日」
      disabledCompletionAnswerLimitDayNextMonth: false,
      serviceInput: {
        requestDate: {
          daysBeforeDeadline: 7,
          dayEveryMonth: 1,
        },
        limitDate: {
          daysAfterSent: 1,
          daysAfterNextMonthStart: 2,
        },
      },
      completionInput: {
        requestDate: {
          daysBeforeDeadline: 7,
          dayEveryMonth: 1,
        },
        limitDate: {
          daysAfterSent: 1,
          daysAfterNextMonthStart: 2,
        },
      },
      minRequestDateSetting: 1,
      maxRequestDateSetting: 31,
      // 送信日入力範囲
      minSendDaySetting: 1,
      maxSendDaySetting: 20,
      // 回答期限日入力範囲
      minAnswerLimitDaySetting: 1,
      maxAnswerLimitDaySetting: 15,
    }
  },
  created() {
    this.localSurveyBulkEdit.service.requestDate =
      this.getInitSendDaySetting('service')
    this.localSurveyBulkEdit.service.limitDate =
      this.getInitAnswerLimitDaySetting('service')
    this.localSurveyBulkEdit.completion.requestDate =
      this.getInitSendDaySetting('completion')
    this.localSurveyBulkEdit.completion.limitDate =
      this.getInitAnswerLimitDaySetting('completion')
    this.serviceAnswerLimitDay = this.getInitAnswerLimitDaySetting('service')
    this.completionAnswerLimitDay =
      this.getInitAnswerLimitDaySetting('completion')

    this.setTextField()
    this.setLocalParam()
  },
  methods: {
    /**
     * 各テキストフィールドの初期値を設定
     */
    setTextField() {
      for (const i in this.surveyMasters) {
        // サービスアンケート
        if (this.surveyMasters[i].type === 'service') {
          // サービスアンケート 送信日 テキストフィールド
          if (
            this.surveyMasters[i].initSendDaySetting <= -1 &&
            this.surveyMasters[i].initSendDaySetting >= -30
          ) {
            //送信日初期設定「回答期限日から〇営業日前：-30~-1」
            this.serviceInput.requestDate.daysBeforeDeadline = Math.abs(
              this.surveyMasters[i].initSendDaySetting
            )
            //毎月〇日を非活性に
            this.disabledServiceSendDayBeforeDeadline = false
            this.disabledServiceSendDayEveryMonth = true
          } else if (
            this.surveyMasters[i].initSendDaySetting <= 31 &&
            this.surveyMasters[i].initSendDaySetting >= 1
          ) {
            //送信日初期設定「毎月〇日：1 ～31」
            this.serviceInput.requestDate.dayEveryMonth =
              this.surveyMasters[i].initSendDaySetting
            this.serviceAnswerRequestDateType =
              this.serviceAnswerRequestDateTypeItems[0]
            //回答期限日から〇営業日前を非活性に
            this.disabledServiceSendDayBeforeDeadline = true
            this.disabledServiceSendDayEveryMonth = false
          } else if (this.surveyMasters[i].initSendDaySetting === 0) {
            //送信日初期設定「なし:0」
            this.serviceInput.requestDate.dayEveryMonth = 1
            this.serviceAnswerRequestDateType =
              this.serviceAnswerRequestDateTypeItems[0]
            //回答期限日から〇営業日前を非活性に
            this.disabledServiceSendDayBeforeDeadline = true
            this.disabledServiceSendDayEveryMonth = false
          }

          // サービスアンケート 回答期限日 テキストフィールド
          // 〇営業日後:1～30の場合
          if (
            this.surveyMasters[i].initAnswerLimitDaySetting > 0 &&
            this.surveyMasters[i].initAnswerLimitDaySetting < 31
          ) {
            this.serviceInput.limitDate.daysAfterSent =
              this.surveyMasters[i].initAnswerLimitDaySetting
          } else {
            this.serviceInput.limitDate.daysAfterSent = 1
          }

          // 翌月月初○営業日:101～130(営業日に+100した数値)
          if (this.surveyMasters[i].initAnswerLimitDaySetting > 100) {
            this.serviceInput.limitDate.daysAfterNextMonthStart =
              this.surveyMasters[i].initAnswerLimitDaySetting - 100
          } else {
            this.serviceInput.limitDate.daysAfterNextMonthStart = 1
          }
        }

        // 修了アンケート
        if (this.surveyMasters[i].type === 'completion') {
          // 修了アンケート 送信日 テキストフィールド
          if (
            this.surveyMasters[i].initSendDaySetting <= -1 &&
            this.surveyMasters[i].initSendDaySetting >= -30
          ) {
            //送信日初期設定「回答期限日から〇営業日前：-30~-1」
            this.completionInput.requestDate.daysBeforeDeadline = Math.abs(
              this.surveyMasters[i].initSendDaySetting
            )
            //修了月〇日を非活性に
            this.disabledCompletionSendDayBeforeDeadline = false
            this.disabledCompletionSendDayEveryMonth = true
          } else if (
            this.surveyMasters[i].initSendDaySetting <= 31 &&
            this.surveyMasters[i].initSendDaySetting >= 1
          ) {
            //送信日初期設定「修了月〇日：1 ～31」
            this.completionInput.requestDate.dayEveryMonth =
              this.surveyMasters[i].initSendDaySetting
            this.completionAnswerRequestDateType =
              this.serviceAnswerRequestDateTypeItems[0]
            //回答期限日から〇営業日前を非活性に
            this.disabledCompletionSendDayBeforeDeadline = true
            this.disabledCompletionSendDayEveryMonth = false
          } else if (this.surveyMasters[i].initSendDaySetting === 0) {
            //送信日初期設定「なし:0」
            this.completionInput.requestDate.dayEveryMonth = 1
            this.completionAnswerRequestDateType =
              this.serviceAnswerRequestDateTypeItems[0]
            //回答期限日から〇営業日前を非活性に
            this.disabledCompletionSendDayBeforeDeadline = true
            this.disabledCompletionSendDayEveryMonth = false
          }

          // 修了アンケート 回答期限日 テキストフィールド
          // 〇営業日後:1～30の場合
          if (
            this.surveyMasters[i].initAnswerLimitDaySetting > 0 &&
            this.surveyMasters[i].initAnswerLimitDaySetting < 31
          ) {
            this.completionInput.limitDate.daysAfterSent =
              this.surveyMasters[i].initAnswerLimitDaySetting
          } else {
            this.completionInput.limitDate.daysAfterSent = 1
          }

          // 翌月月初○営業日:101～130(営業日に+100した数値)
          if (
            this.surveyMasters[i].initAnswerLimitDaySetting >= 101 &&
            this.surveyMasters[i].initAnswerLimitDaySetting <= 130
          ) {
            this.completionInput.limitDate.daysAfterNextMonthStart =
              this.surveyMasters[i].initAnswerLimitDaySetting - 100
          } else {
            this.completionInput.limitDate.daysAfterNextMonthStart = 1
          }
        }
      }
    },
    /**
     * サービスアンケート: 送信日テキストボックスの活性制御
     */
    changeServiceSendDaysRadio(elm: string) {
      if (elm === 'auto') {
        // 回答期限日から〇営業日前
        this.disabledServiceSendDayBeforeDeadline = false
        this.disabledServiceSendDayEveryMonth = true
      } else {
        // 毎月〇日
        this.disabledServiceSendDayBeforeDeadline = true
        this.disabledServiceSendDayEveryMonth = false
      }
    },
    /**
     * サービスアンケート: 送信日から●日後の定義
     * @param elm 回答期限日文字列
     */
    getServiceAnswerLimitDay(elm: string) {
      if (elm === 'endOfMonth') {
        this.localSurveyBulkEdit.service.limitDate = 99
        this.disabledServiceAnswerLimitDayAfterSendDays = true
        this.disabledServiceAnswerLimitDayNextMonth = true
      } else if (elm === 'none') {
        this.localSurveyBulkEdit.service.limitDate = 0
        this.disabledServiceAnswerLimitDayAfterSendDays = true
        this.disabledServiceAnswerLimitDayNextMonth = true
      } else if (elm === 'daysAfter') {
        this.localSurveyBulkEdit.service.limitDate =
          this.serviceInput.limitDate.daysAfterSent
        this.disabledServiceAnswerLimitDayAfterSendDays = false
        this.disabledServiceAnswerLimitDayNextMonth = true
      } else if (elm === 'nextMonth') {
        this.localSurveyBulkEdit.service.limitDate =
          this.serviceInput.limitDate.daysAfterNextMonthStart
        this.disabledServiceAnswerLimitDayAfterSendDays = true
        this.disabledServiceAnswerLimitDayNextMonth = false
      }
    },
    /**
     * 修了アンケート: 送信日テキストボックスの活性制御
     */
    changeCompletionSendDaysRadio(elm: string) {
      if (elm === 'manual') {
        // 回答期限日から〇営業日前
        this.disabledCompletionSendDayBeforeDeadline = true
        this.disabledCompletionSendDayEveryMonth = false
      } else {
        // 毎月〇日
        this.disabledCompletionSendDayBeforeDeadline = false
        this.disabledCompletionSendDayEveryMonth = true
      }
    },
    /**
     * 修了アンケート: 送信日から●日後の定義
     * @param elm 回答期限日文字列
     */
    getCompletionAnswerLimitDay(elm: string) {
      if (elm === 'endOfMonth') {
        this.localSurveyBulkEdit.completion.limitDate = 99
        this.disabledCompletionAnswerLimitDayAfterSendDays = true
        this.disabledCompletionAnswerLimitDayNextMonth = true
      } else if (elm === 'none') {
        this.localSurveyBulkEdit.completion.limitDate = 0
        this.disabledCompletionAnswerLimitDayAfterSendDays = true
        this.disabledCompletionAnswerLimitDayNextMonth = true
      } else if (elm === 'daysAfter') {
        this.localSurveyBulkEdit.completion.limitDate =
          this.completionInput.limitDate.daysAfterSent
        this.disabledCompletionAnswerLimitDayAfterSendDays = false
        this.disabledCompletionAnswerLimitDayNextMonth = true
      } else if (elm === 'nextMonth') {
        this.localSurveyBulkEdit.completion.limitDate =
          this.completionInput.limitDate.daysAfterNextMonthStart
        this.disabledCompletionAnswerLimitDayAfterSendDays = true
        this.disabledCompletionAnswerLimitDayNextMonth = false
      }
    },
    /** 送信日入力範囲の制御（1～31日） */
    serviceValidationCorrect() {
      this.isUpdatingServiceRequestDay = true
      this.localSurveyBulkEdit.service.requestDate = adjustDay(
        this.localSurveyBulkEdit.service.requestDate
      )
      // 小数点の削除を適用するために再描画する
      this.$nextTick(() => (this.isUpdatingServiceRequestDay = false))
    },
    /** 送信日入力範囲の制御（1～31日） */
    completionValidationCorrect() {
      this.isUpdatingCompletionRequestDay = true
      this.localSurveyBulkEdit.completion.requestDate = adjustDay(
        this.localSurveyBulkEdit.completion.requestDate
      )
      // 小数点の削除を適用するために再描画する
      this.$nextTick(() => (this.isUpdatingCompletionRequestDay = false))
    },
    /**
     * 送信日の初期値を設定
     * @param surveyType アンケート種別
     * @returns 送信日の初期値
     */
    getInitSendDaySetting(surveyType: string): number {
      const minSendDaySetting: number = 1
      const maxSendDaySetting: number = 31
      for (const i in this.surveyMasters) {
        if (this.surveyMasters[i].type === surveyType) {
          const initSendDaySetting: number =
            this.surveyMasters[i].initSendDaySetting
          if (minSendDaySetting > initSendDaySetting) {
            return minSendDaySetting
          } else if (maxSendDaySetting < initSendDaySetting) {
            return maxSendDaySetting
          } else {
            return initSendDaySetting
          }
        }
      }
      return 0
    },
    /**
     * 回答期限日の初期値を設定
     * @param surveyType アンケート種別
     * @returns 回答期限日の初期値
     */
    getInitAnswerLimitDaySetting(surveyType: string): number {
      const minAnswerLimitDaySetting: number = 0
      const maxAnswerLimitDaySetting: number = 99
      for (const i in this.surveyMasters) {
        if (this.surveyMasters[i].type === surveyType) {
          const initAnswerLimitDaySetting: number =
            this.surveyMasters[i].initAnswerLimitDaySetting
          if (minAnswerLimitDaySetting > initAnswerLimitDaySetting) {
            return minAnswerLimitDaySetting
          } else if (maxAnswerLimitDaySetting < initAnswerLimitDaySetting) {
            return maxAnswerLimitDaySetting
          } else {
            return initAnswerLimitDaySetting
          }
        }
      }
      return 0
    },
    /**
     * serviceInput変更をトリガーに送信用のパラメータを設定
     */
    setLocalParam() {
      /**
       * サービスアンケート
       */

      // 送信日
      if (
        this.serviceAnswerRequestDateType ===
        this.serviceAnswerRequestDateTypeItems[1]
      ) {
        // 回答期限日から○営業日前:-30～-1
        this.localSurveyBulkEdit.service.requestDate =
          Number(this.serviceInput.requestDate.daysBeforeDeadline) * -1
      } else {
        //毎月○日:1-31
        this.localSurveyBulkEdit.service.requestDate = Number(
          this.serviceInput.requestDate.dayEveryMonth
        )
      }

      // 回答期限日
      if (
        this.serviceAnswerLimitDayTiming === this.answerLimitDayTimingItems[0]
      ) {
        // 月末最終営業日:99
        this.localSurveyBulkEdit.service.limitDate = 99
      }
      if (
        this.serviceAnswerLimitDayTiming === this.answerLimitDayTimingItems[1]
      ) {
        // 〇営業日後:1～30
        this.localSurveyBulkEdit.service.limitDate = Number(
          this.serviceInput.limitDate.daysAfterSent
        )
      }
      if (
        this.serviceAnswerLimitDayTiming === this.answerLimitDayTimingItems[2]
      ) {
        // 翌月月初○営業日:101～130(営業日に+100した数値)
        this.localSurveyBulkEdit.service.limitDate =
          Number(this.serviceInput.limitDate.daysAfterNextMonthStart) + 100
      }
      if (
        this.serviceAnswerLimitDayTiming === this.answerLimitDayTimingItems[3]
      ) {
        // なし
        this.localSurveyBulkEdit.service.limitDate = 0
      }

      /**
       * 修了アンケート
       */

      // 送信日
      if (
        this.completionAnswerRequestDateType ===
        this.serviceAnswerRequestDateTypeItems[1]
      ) {
        // 回答期限日から○営業日前:-30～-1
        this.localSurveyBulkEdit.completion.requestDate =
          Number(this.completionInput.requestDate.daysBeforeDeadline) * -1
      } else {
        //毎月○日:1-31
        this.localSurveyBulkEdit.completion.requestDate = Number(
          this.completionInput.requestDate.dayEveryMonth
        )
      }

      // 回答期限日
      if (
        this.completionAnswerLimitDayTiming ===
        this.answerLimitDayTimingItems[0]
      ) {
        // 月末最終営業日:99
        this.localSurveyBulkEdit.completion.limitDate = 99
      }

      if (
        this.completionAnswerLimitDayTiming ===
        this.answerLimitDayTimingItems[1]
      ) {
        // 〇営業日後:1～30
        this.localSurveyBulkEdit.completion.limitDate = Number(
          this.completionInput.limitDate.daysAfterSent
        )
      }

      if (
        this.completionAnswerLimitDayTiming ===
        this.answerLimitDayTimingItems[2]
      ) {
        // 翌月月初○営業日:101～130(営業日に+100した数値)
        this.localSurveyBulkEdit.completion.limitDate =
          Number(this.completionInput.limitDate.daysAfterNextMonthStart) + 100
      }

      if (
        this.completionAnswerLimitDayTiming ===
        this.answerLimitDayTimingItems[3]
      ) {
        // なし
        this.localSurveyBulkEdit.completion.limitDate = 0
      }
    },
    // 送信日の入力値を補正
    sendDaySettingValidationCorrect() {
      // 回答期限から◯営業日前：上限・下限を外れた値はそれぞれ上限・下限に合わせる
      // サービスアンケート
      if (
        this.serviceInput.requestDate.daysBeforeDeadline <
        this.minSendDaySetting
      ) {
        this.serviceInput.requestDate.daysBeforeDeadline =
          this.minSendDaySetting
      } else if (
        this.serviceInput.requestDate.daysBeforeDeadline >
        this.maxSendDaySetting
      ) {
        this.serviceInput.requestDate.daysBeforeDeadline =
          this.maxSendDaySetting
      }
      // 修了アンケート
      if (
        this.completionInput.requestDate.daysBeforeDeadline <
        this.minSendDaySetting
      ) {
        this.completionInput.requestDate.daysBeforeDeadline =
          this.minSendDaySetting
      } else if (
        this.completionInput.requestDate.daysBeforeDeadline >
        this.maxSendDaySetting
      ) {
        this.completionInput.requestDate.daysBeforeDeadline =
          this.maxSendDaySetting
      }
    },
    // 送信日「毎月〇日」の入力値を補正
    sentDayEveryMonthCorrect() {
      // サービスアンケート
      if (
        this.serviceInput.requestDate.dayEveryMonth < this.minRequestDateSetting
      ) {
        this.serviceInput.requestDate.dayEveryMonth = this.minRequestDateSetting
      } else if (
        this.serviceInput.requestDate.dayEveryMonth > this.maxRequestDateSetting
      ) {
        this.serviceInput.requestDate.dayEveryMonth = this.maxRequestDateSetting
      }
      // 修了アンケート
      if (
        this.completionInput.requestDate.dayEveryMonth <
        this.minRequestDateSetting
      ) {
        this.completionInput.requestDate.dayEveryMonth =
          this.minRequestDateSetting
      } else if (
        this.completionInput.requestDate.dayEveryMonth >
        this.maxRequestDateSetting
      ) {
        this.completionInput.requestDate.dayEveryMonth =
          this.maxRequestDateSetting
      }
    },
    // 回答期限日の入力値を補正
    answerLimitDaySettingValidationCorrect() {
      //サービスアンケート
      // 送信日から〇営業日後
      if (
        this.serviceInput.limitDate.daysAfterSent <
        this.minAnswerLimitDaySetting
      ) {
        this.serviceInput.limitDate.daysAfterSent =
          this.minAnswerLimitDaySetting
      } else if (
        this.serviceInput.limitDate.daysAfterSent >
        this.maxAnswerLimitDaySetting
      ) {
        this.serviceInput.limitDate.daysAfterSent =
          this.maxAnswerLimitDaySetting
      }
      // 翌月月初〇営業日
      if (
        this.serviceInput.limitDate.daysAfterNextMonthStart <
        this.minAnswerLimitDaySetting
      ) {
        this.serviceInput.limitDate.daysAfterNextMonthStart =
          this.minAnswerLimitDaySetting
      } else if (
        this.serviceInput.limitDate.daysAfterNextMonthStart >
        this.maxAnswerLimitDaySetting
      ) {
        this.serviceInput.limitDate.daysAfterNextMonthStart =
          this.maxAnswerLimitDaySetting
      }
      // 修了アンケート
      // 送信日から〇営業日後
      if (
        this.completionInput.limitDate.daysAfterSent <
        this.minAnswerLimitDaySetting
      ) {
        this.completionInput.limitDate.daysAfterSent =
          this.minAnswerLimitDaySetting
      } else if (
        this.completionInput.limitDate.daysAfterSent >
        this.maxAnswerLimitDaySetting
      ) {
        this.completionInput.limitDate.daysAfterSent =
          this.maxAnswerLimitDaySetting
      }
      // 翌月月初〇営業日
      if (
        this.completionInput.limitDate.daysAfterNextMonthStart <
        this.minAnswerLimitDaySetting
      ) {
        this.completionInput.limitDate.daysAfterNextMonthStart =
          this.minAnswerLimitDaySetting
      } else if (
        this.completionInput.limitDate.daysAfterNextMonthStart >
        this.maxAnswerLimitDaySetting
      ) {
        this.completionInput.limitDate.daysAfterNextMonthStart =
          this.maxAnswerLimitDaySetting
      }
    },
  },
  watch: {
    serviceAnswerRequestDateType: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    completionAnswerRequestDateType: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    serviceAnswerLimitDayTiming: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    completionAnswerLimitDayTiming: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    serviceInput: {
      handler() {
        this.setLocalParam()
      },
      deep: true,
    },
    completionInput: {
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
