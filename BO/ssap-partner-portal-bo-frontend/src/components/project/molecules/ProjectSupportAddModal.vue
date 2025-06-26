<template>
  <ModalEdit :title="title">
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValid" @input="$listeners['input']">
        <v-row>
          <v-col cols="4" class="font-weight-bold">{{
            $t('project.pages.edit.support.row.schedule.label')
          }}</v-col>
          <v-col class="pa-0">
            <CommonDetailRow
              :label="$t('project.pages.edit.sub.timing.label')"
              :is-editing="isEditing"
              cols="4"
              required
              class="is-noborder"
            >
              <RadioGroup
                v-model="localAddSupportSchedule.timing"
                required
                :labels="
                  $t('project.pages.edit.survey.row.sub.timing.radio.labels')
                "
                :values="
                  $t('project.pages.edit.survey.row.sub.timing.radio.values')
                "
                horizontal
                @change="resetSupportDate"
              />
            </CommonDetailRow>
            <!-- 開催日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.sub.eventDate.label')"
              :is-editing="isEditing"
              cols="4"
              required
              class="is-noborder"
            >
              <!-- 毎月 -->
              <template v-if="localAddSupportSchedule.timing === 'monthly'">
                <span class="d-flex align-center">
                  {{ $t('project.pages.edit.sub.send.monthly') }}
                  <Sheet width="60" class="mx-2">
                    <TextField
                      v-model="localAddSupportSchedule.supportDate"
                      :required="
                        localAddSupportSchedule.timing === 'monthly'
                          ? true
                          : false
                      "
                      type="number"
                      number
                      style-set="outlined"
                      :placeholder="
                        $t('project.pages.edit.sub.send.placeholder')
                      "
                      :max-digits="2"
                      positive-number
                      min="1"
                      max="31"
                      @focusout="validationCorrect"
                    />
                  </Sheet>
                  {{ $t('project.pages.edit.sub.send.date') }}
                </span>
              </template>
              <!-- 毎週 -->
              <template v-if="localAddSupportSchedule.timing === 'weekly'">
                <span class="d-flex align-center">
                  {{ $t('project.pages.edit.sub.send.weekly') }}
                  <Sheet width="150" class="mx-2">
                    <Select
                      v-model="localAddSupportSchedule.supportDate"
                      style-set="outlined"
                      :items="weekDay"
                      item-text="label"
                      item-value="value"
                      :placeholder="
                        $t('project.pages.edit.sub.send.dayPlaceholder')
                      "
                    />
                  </Sheet>
                </span>
              </template>
              <!-- 1回 -->
              <template v-if="localAddSupportSchedule.timing === 'once'">
                <DateSelect
                  v-model="localAddSupportSchedule.supportDate"
                  :required="
                    localAddSupportSchedule.timing === 'once' ? true : false
                  "
                  :date="
                    localAddSupportSchedule.supportDate
                      ? localAddSupportSchedule.supportDate.replaceAll('/', '-')
                      : ''
                  "
                />
              </template>
            </CommonDetailRow>
            <!-- 開催時間 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.sub.openTime.label')"
              :is-editing="isEditing"
              :value="`${localAddSupportSchedule.startTime}～${localAddSupportSchedule.endTime}`"
              cols="4"
              required
              tall
              class="is-noborder"
            >
              <v-container pa-0 pt-2>
                <v-row align="center">
                  <VueTimePicker
                    :value="localAddSupportSchedule.startTime"
                    :step="15"
                    @input="inputStartTime"
                  />
                  〜
                  <VueTimePicker
                    :value="localAddSupportSchedule.endTime"
                    :step="15"
                    @input="inputEndTime"
                  />
                </v-row>
              </v-container>
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
        :disabled="isReversalTime !== false || isValid !== true"
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
import TimeSelect from '~/components/common/molecules/TimeSelect.vue'
import BaseComponent from '~/common/BaseComponent'
import VueTimePicker from '~/components/common/molecules/VueTimePicker.vue'

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
    TimeSelect,
    VueTimePicker,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    /** 支援期間スケジュール */
    localAddSupportSchedule: {
      type: Object,
      required: true,
    },
  },
  data(): {
    isEditing: boolean
    isValid: boolean
    isReversalTime: boolean
    maxSupportDate: number
    minSupportDate: number
  } {
    return {
      isEditing: true,
      isValid: false,
      isReversalTime: false,
      maxSupportDate: 31,
      minSupportDate: 1,
    }
  },
  computed: {
    weekDay() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: 0 }

      // @ts-ignore
      this.$t('project.pages.edit.sub.send.day').forEach(
        (_elm: string, index: number) => {
          // @ts-ignore
          selectItem.label = this.$t('project.pages.edit.sub.send.day')[index]
          selectItem.value = index
          const newElm = Object.assign({}, selectItem)
          selectItems.push(newElm)
        }
      )
      return selectItems
    },
    // 開催時間の必須チェック
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      const rule = (value: string[]) => {
        return value.length > 0 || this.$t('common.rule.required')
      }
      rules.push(rule)
      return rules
    },
  },
  methods: {
    /**
     * 支援開始時間をセット
     * @param elm 支援開始時間
     */
    inputStartTime(elm: string) {
      this.localAddSupportSchedule.startTime = elm
      this.timeReversalCheck(
        this.localAddSupportSchedule.startTime,
        this.localAddSupportSchedule.endTime
      )
    },
    /**
     * 支援終了時間をセット
     * @param elm 支援終了時間
     */
    inputEndTime(elm: string) {
      this.localAddSupportSchedule.endTime = elm
      this.timeReversalCheck(
        this.localAddSupportSchedule.startTime,
        this.localAddSupportSchedule.endTime
      )
    },
    /**
     * 支援期間に矛盾がないか確認
     * @param startTime 支援開始時間
     * @param endTime 支援終了時間
     */
    timeReversalCheck(startTime: string, endTime: string) {
      const localStartHourMin = startTime.split(':')
      const localStartHour = String(localStartHourMin[0])
      const localStartMin = String(localStartHourMin[1])
      const localEndHourMin = endTime.split(':')
      const localEndHour = String(localEndHourMin[0])
      const localEndMin = String(localEndHourMin[1])
      const intStartHourMin = parseInt(`${localStartHour}${localStartMin}`)
      const intEndHourMin = parseInt(`${localEndHour}${localEndMin}`)
      if (intStartHourMin > intEndHourMin) {
        this.isReversalTime = true
      } else {
        this.isReversalTime = false
      }
    },
    /** 支援期間をリセット */
    resetSupportDate() {
      if (this.localAddSupportSchedule.timing === 'monthly') {
        this.localAddSupportSchedule.supportDate = 1
      } else if (this.localAddSupportSchedule.timing === 'weekly') {
        this.localAddSupportSchedule.supportDate = 0
      } else {
        this.localAddSupportSchedule.supportDate = format(
          getCurrentDate(),
          'yyyy/MM/dd'
        )
      }
    },
    /** 送信日入力範囲の制御（1～31日） */
    validationCorrect() {
      // 空の値、数値以外、数値範囲下限を下回る場合は下限に合わせる
      if (
        !this.localAddSupportSchedule.supportDate ||
        isNaN(this.localAddSupportSchedule.supportDate) ||
        Number(this.localAddSupportSchedule.supportDate) <
          Number(this.minSupportDate)
      ) {
        this.localAddSupportSchedule.supportDate = this.minSupportDate
      }

      //数値範囲上限を上回る場合は上限に合わせる
      if (
        Number(this.localAddSupportSchedule.supportDate) >
        Number(this.maxSupportDate)
      ) {
        this.localAddSupportSchedule.supportDate = this.maxSupportDate
      }
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
