<template>
  <Sheet class="o-survey-sort-input d-flex align-center pr-7">
    <Sheet width="280" class="d-flex align-center">
      <DateSelect
        v-model="localFrom"
        :init-date="getCurrentDate().toISOString().substr(0, 7) + '-01'"
        :disabled="false"
        style-set="bgWhite"
        @change="onChange"
      />
      <span class="mx-2">〜</span>
      <DateSelect
        v-model="localTo"
        is-no-icon
        :disabled="false"
        style-set="bgWhite"
        @change="onChange"
      />
    </Sheet>
    <Sheet class="mx-6">
      <RadioGroup
        v-model="localSupporterRange"
        :labels="[
          $t('survey.options.mainSupporters'),
          $t('survey.options.anySupporters'),
        ]"
        :values="[1, 0]"
        horizontal
        hide-details
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
    /**期間開始日 */
    from: {
      type: String,
      default: '',
    },
    /** 期間終了日 */
    to: {
      type: String,
      default: '',
    },
    supporterRange: {
      type: Number,
      default: 1,
    },
  },
  watch: {
    from(newVal) {
      this.localFrom = newVal
    },
    to(newVal) {
      this.localTo = newVal
    },
    supporterRange(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.localSupporterRange = newVal
      }
    },
  },
  data() {
    return {
      localEnbFromTo: true,
      localFrom: '',
      localTo: '',
      localSupporterRange: 1,
      rstFlg2: false,
    }
  },
  methods: {
    /** 並び替え条件の入力変更 */
    onChange() {
      const localData = {
        surveyType: '',
        from: this.localFrom,
        to: this.localTo,
        enbFromTo: true,
        supporterRange: this.localSupporterRange,
      }
      this.$emit('change', localData)
    },
  },
})
</script>
