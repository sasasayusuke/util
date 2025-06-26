<template>
  <Sheet class="o-survey-sort-input d-flex align-center pr-7">
    <Sheet class="mx-2">
      <RadioGroup
        :value="param.isRefined"
        :labels="[
          $t('survey.options.xEnbFromTo'),
          $t('survey.options.enbFromTo'),
        ]"
        :values="[false, true]"
        horizontal
        hide-details
        @change="$emit('update', 'isRefined', $event)"
      />
    </Sheet>

    <Sheet width="280" class="d-flex align-center">
      <DateSelect
        v-model="localFrom"
        :allowed-dates="allowedDatesFrom"
        :disabled="!param.isRefined"
        style-set="bgWhite"
        @change="onChange"
      />
      <span class="mx-2">〜</span>
      <DateSelect
        v-model="localTo"
        :allowed-dates="allowedDatesTo"
        is-no-icon
        style-set="bgWhite"
        :disabled="!param.isRefined"
        @change="onChange"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import { Sheet, RadioGroup } from '~/components/common/atoms'
import DateSelect, {
  defaultRtnFormat,
} from '~/components/common/molecules/DateSelect.vue'

export class SurveySearchParam {
  isRefined = false
  from = format(getCurrentDate(), defaultRtnFormat)
  to = ''
}

export default BaseComponent.extend({
  components: {
    Sheet,
    RadioGroup,
    DateSelect,
  },
  props: {
    /** リクエストパラメーター */
    param: {
      type: SurveySearchParam,
      required: true,
    },
  },
  data() {
    return {
      localFrom: this.param.from,
      localTo: '',
    }
  },
  methods: {
    /**
     * リクエストパラメーターの入力変更
     */
    onChange() {
      const localData: SurveySearchParam = {
        isRefined: this.localFrom !== '' || this.localTo !== '',
        from: this.localFrom,
        to: this.localTo,
      }
      this.$emit('change', localData)
    },
    /**
     * fromで許容する日付設定:to以下とする
     * @param val 日付
     */
    allowedDatesFrom(val: string) {
      if (this.localTo !== '') {
        // 時刻部分は削除して比較
        const inputDate = new Date(val)
        inputDate.setHours(0)
        inputDate.setMinutes(0)
        inputDate.setSeconds(0)
        const toDate = new Date(this.localTo)
        toDate.setHours(0)
        toDate.setMinutes(0)
        toDate.setSeconds(0)
        return inputDate <= toDate
      } else {
        return true
      }
    },
    /**
     * toで許容する日付設定:from以上とする
     * @param val 日付
     */
    allowedDatesTo(val: string) {
      if (this.localFrom !== '') {
        // 時刻部分は削除して比較
        const inputDate = new Date(val)
        inputDate.setHours(0)
        inputDate.setMinutes(0)
        inputDate.setSeconds(0)
        const fromDate = new Date(this.localFrom)
        fromDate.setHours(0)
        fromDate.setMinutes(0)
        fromDate.setSeconds(0)
        return inputDate >= fromDate
      } else {
        return true
      }
    },
  },
})
</script>
