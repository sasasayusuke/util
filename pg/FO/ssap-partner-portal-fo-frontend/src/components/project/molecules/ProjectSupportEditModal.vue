<template>
  <ModalEdit :title="title">
    <v-container py-2 px-0 pb-6 class="m-survery-modal__unit font-size-small">
      <v-form v-model="isValid" @input="$listeners['input']">
        <v-row>
          <v-col cols="4" class="font-weight-bold">{{
            $t('project.pages.edit.support.row.schedule.label')
          }}</v-col>
          <v-col class="pa-0">
            <!-- 開催日 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.sub.eventDate.label')"
              :is-editing="isEditing"
              :value="localSupportSchedule.supportDate"
              cols="4"
              required
              class="is-noborder"
            >
              <DateSelect
                v-model="localSupportSchedule.supportDate"
                required
                :date="localSupportSchedule.supportDate.replaceAll('/', '-')"
                @change="isChanged = true"
              />
            </CommonDetailRow>
            <!-- 開催時間 -->
            <CommonDetailRow
              :label="$t('project.pages.edit.sub.openTime.label')"
              :is-editing="isEditing"
              cols="4"
              required
              tall
              class="is-noborder"
            >
              <v-container pa-0 pt-2>
                <v-row align="center">
                  <VueTimePicker
                    :value="localSupportSchedule.supportStartTime"
                    :step="15"
                    @input="inputStartTime"
                  />
                  〜
                  <VueTimePicker
                    :value="localSupportSchedule.supportEndTime"
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
        @click="$emit('click:closeEdit')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="
          isReversalTime !== false || isChanged !== true || isValid !== true
        "
        @click="update"
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
import {
  UpdateSupportSchedulesByIdAndDate,
  UpdateSupportSchedulesByIdAndDateRequest as localSupportSchedule,
} from '~/models/Schedule'
export { localSupportSchedule }

export default BaseComponent.extend({
  name: 'ProjectSupportEditModal',
  components: {
    ModalEdit,
    CommonDetailRow,
    Button,
    Sheet,
    Paragraph,
    AutoComplete,
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
    modalSchedule: {
      type: Object,
      required: true,
    },
    /** 個別カルテ・スケジュール一覧画面か */
    isKarteList: {
      type: Boolean,
      required: false,
    },
  },
  data() {
    return {
      isEditing: true,
      isValid: false,
      isChanged: false,
      isReversalTime: false,
      localSupportSchedule: {
        supportDate: '',
        supportStartTime: '',
        supportEndTime: '',
      },
      daysAfter: 0,
    }
  },
  created() {
    const supportDateAry = this.modalSchedule.supportDate.split(' ')
    this.localSupportSchedule.supportDate = supportDateAry[0]
    this.localSupportSchedule.supportStartTime =
      this.modalSchedule.supportStartTime
    this.localSupportSchedule.supportEndTime = this.modalSchedule.supportEndTime
  },
  computed: {
    isValidWithChange(): boolean {
      if (this.isChanged) {
        return this.isValid
      } else {
        return false
      }
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
    /** 支援期間開始日時を設定 */
    inputStartTime(elm: string) {
      this.localSupportSchedule.supportStartTime = elm
      this.timeReversalCheck(
        this.localSupportSchedule.supportStartTime,
        this.localSupportSchedule.supportEndTime
      )
      this.isChanged = true
    },
    /** 支援期間修了日時を設定 */
    inputEndTime(elm: string) {
      this.localSupportSchedule.supportEndTime = elm
      this.timeReversalCheck(
        this.localSupportSchedule.supportStartTime,
        this.localSupportSchedule.supportEndTime
      )
      this.isChanged = true
    },
    /** 支援期間開始日と終了日に矛盾が生じていないか確認 */
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
    /** 案件区分が「支援」の案件スケジュールを案件IDとスケジュールIDを元に更新 */
    async update() {
      const karteId = this.modalSchedule.karteId
      const version = this.modalSchedule.version
      const request = this.localSupportSchedule
      const detailPage = '/project/' + this.$route.params.projectId
      const karteListPage = '/karte/list/' + this.$route.params.projectId

      await UpdateSupportSchedulesByIdAndDate(karteId, version, request)
        .then(() => {
          this.$emit('click:closeEdit')
          this.$emit('refresh')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeEdit')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
        })
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
