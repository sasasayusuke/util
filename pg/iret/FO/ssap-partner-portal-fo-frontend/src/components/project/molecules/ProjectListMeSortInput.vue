<template>
  <Sheet class="m-project-sort-input">
    <Sheet class="m-project-sort-input__unit d-flex align-center pr-7">
      <!-- 期間指定 -->
      <!-- <p class="font-size-small mb-0 mr-2">
        {{ $t('project.pages.me.sort_input.display_period') }}
      </p> -->
      <Sheet class="mr-2" width="150">
        <MonthSelect
          :value="param.fromYearMonth"
          :allowed-dates="allowedDatesFrom"
          style-set="bgWhite"
          @change="$emit('update', 'fromYearMonth', $event)"
        />
      </Sheet>
      〜
      <Sheet class="ml-2" width="117">
        <MonthSelect
          :value="param.toYearMonth"
          :allowed-dates="allowedDatesTo"
          style-set="bgWhite"
          class="is-no-icon"
          @change="$emit('update', 'toYearMonth', $event)"
        />
      </Sheet>
      <Sheet class="mt-1 ml-6">
        <RadioGroup
          v-model="param.allAssigned"
          :labels="[
            $t('project.pages.index.sort_input.types.main'),
            $t('project.pages.index.sort_input.types.all'),
          ]"
          :values="[false, true]"
          horizontal
          hide-details
        />
      </Sheet>
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import { format } from 'date-fns'
import BaseComponent from '~/common/BaseComponent'
import { Sheet, RadioGroup } from '~/components/common/atoms'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'

export default BaseComponent.extend({
  name: 'ProjectListMeSortInput',
  components: {
    MonthSelect,
    Sheet,
    RadioGroup,
  },
  props: {
    /** 絞り込みパラメータ */
    param: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      /** 「期間指定」開始日時 */
      localFrom: '',
      /** 「期間指定」修了日時 */
      localTo: '',
    }
  },
  watch: {
    param: {
      deep: true,
      handler(newValue: any) {
        this.localFrom = newValue.fromYearMonth
        this.allowedDatesFrom(newValue.fromYearMonth)
        this.localTo = newValue.toYearMonth
        this.allowedDatesTo(newValue.toYearMonth)
      },
    },
  },
  methods: {
    /**
     * 「期間指定」開始日時が「期間指定」修了日時よりも前ならtrueを返す
     * @param val 絞り込みパラメータの「期間指定」開始日時
     */
    allowedDatesFrom(val: string) {
      if (val !== '' && this.localTo !== '') {
        const inputDate = parseInt(format(new Date(val), 'yyyyMM'))
        const toDate = parseInt(format(new Date(this.localTo), 'yyyyMM'))
        return inputDate <= toDate
      } else {
        return true
      }
    },
    /**
     * 「期間指定」修了日時が「期間指定」開始日時よりも後ならtrueを返す
     * @param val 絞り込みパラメータの「期間指定」修了日時
     */
    allowedDatesTo(val: string) {
      if (val !== '' && this.localFrom !== '') {
        const inputDate = parseInt(format(new Date(val), 'yyyyMM'))
        const fromDate = parseInt(format(new Date(this.localFrom), 'yyyyMM'))
        return inputDate >= fromDate
      } else {
        return true
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.m-project-sort-input__unit {
  &:nth-child(2) {
    margin-left: -100px;
  }
}
</style>

<style lang="scss">
.m-project-sort-input {
  .a-radio.horizontal .v-radio {
    &:last-child {
      margin-right: 0 !important;
    }
  }
}
</style>
