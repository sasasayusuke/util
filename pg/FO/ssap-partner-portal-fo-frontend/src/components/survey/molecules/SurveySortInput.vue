<template>
  <Sheet class="o-survey-sort-input d-flex align-center pr-7">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('survey.pages.list.sort_input.surveyType') }}
      <Required />
    </p>
    <Sheet width="224">
      <Select
        v-if="surveyTypes.length"
        v-model="localSurveyType"
        :aria-label="$t('survey.pages.list.sort_input.surveyType')"
        dense
        outlined
        :items="surveyTypes"
        item-text="text"
        item-value="value"
        :placeholder="$t('common.placeholder.select')"
        style-set="bgWhite"
        @change="onChange"
      />
    </Sheet>

    <Sheet class="ml-6 mr-2">
      <RadioGroup
        v-model="localEnbFromTo"
        :labels="[
          $t('survey.options.xEnbFromTo'),
          $t('survey.options.enbFromTo'),
        ]"
        :values="[false, true]"
        horizontal
        hide-details
        @change="onChange"
      />
    </Sheet>

    <Sheet width="280" class="d-flex align-center">
      <DateSelect
        v-model="localFrom"
        :disabled="!localEnbFromTo"
        :allowed-dates="allowedDatesFrom"
        style-set="bgWhite"
        @change="onChange"
      />
      <span class="mx-2">〜</span>
      <DateSelect
        v-model="localTo"
        is-no-icon
        :disabled="!localEnbFromTo"
        :allowed-dates="allowedDatesTo"
        style-set="bgWhite"
        @change="onChange"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import {
  TextField,
  Sheet,
  Select,
  RadioGroup,
  Required,
} from '~/components/common/atoms'
import DateSelect from '~/components/common/molecules/DateSelect.vue'

export default BaseComponent.extend({
  components: {
    TextField,
    Sheet,
    Select,
    RadioGroup,
    Required,
    DateSelect,
  },
  props: {
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
    /** 期間指定の有無 */
    enbFromTo: {
      type: Boolean,
      default: false,
    },
    /** 期間開始日 */
    from: {
      type: String,
      default: '',
    },
    /** 期間終了日 */
    to: {
      type: String,
      default: '',
    },
  },
  watch: {
    surveyType(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.localSurveyType = newVal
      }
    },
    enbFromTo(newVal, oldVal) {
      if (!newVal && newVal !== oldVal) {
        this.localEnbFromTo = newVal
      }
    },
    from(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.localFrom = newVal
      }
    },
    to(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.localTo = newVal
      }
    },
  },
  data() {
    return {
      localSurveyType: this.surveyType,
      localEnbFromTo: this.enbFromTo,
      localFrom: this.from,
      localTo: this.to,
      surveyTypes: [
        { text: this.$t('survey.group_info.type.all'), value: '' },
        { text: this.$t('survey.group_info.type.service'), value: 'service' },
        {
          text: this.$t('survey.group_info.type.completion'),
          value: 'completion',
        },
        { text: this.$t('survey.group_info.type.quick'), value: 'quick' },
      ],
    }
  },
  methods: {
    /**
     * 並び替え情報の入力変更
     */
    onChange() {
      const localData = {
        surveyType: this.localSurveyType,
        from: this.localFrom,
        to: this.localTo,
        enbFromTo: this.localEnbFromTo,
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
