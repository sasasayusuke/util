<!-- TODO: 日付複数選択には現在未対応なので、必要になったときに対応する -->

<template>
  <div class="a-date-select">
    <v-menu
      v-model="isOpenMenu"
      :close-on-content-click="false"
      :nudge-right="40"
      transition="scale-transition"
      offset-y
      min-width="auto"
      :disabled="disabled"
      @input="onMenuToggle"
    >
      <template #activator="{ on, attrs }">
        <TextField
          role="textbox"
          :value="internalValue"
          :prepend-icon="icon"
          :placeholder="computedPlaceholder"
          outlined
          dense
          :disabled="disabled"
          :readonly="isReadOnly"
          :required="required"
          :validate-date-format="true"
          :style-set="styleSet"
          :clearable="true"
          v-bind="attrs"
          v-on="on"
          @input="handleInput"
          @blur="applyFormatOnBlur"
          @click:clear="
            () => {
              $emit('change', '')
              previousValue = ''
            }
          "
        ></TextField>
      </template>
      <DatePicker
        ref="myDatePicker"
        :value="pickedDate"
        :allowed-dates="allowedDates"
        :min-date="minDate"
        :max-date="maxDate"
        :active-picker.sync="activePicker"
        no-title
        @input="handleDatePickerInput"
      >
      </DatePicker>
    </v-menu>
  </div>
</template>

<script lang="ts">
import { parse, format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import { TextField, DatePicker } from '~/components/common/atoms/index'

// このコンポーネント内で扱う日付値の形式(inComponentFormat propsで指定可能)
// よくわからない場合は変更しないことを推奨
export const DEFAULT_FORMAT = 'yyyy-MM-dd'
// 親コンポーネントとやりとりされるときに用いられる日付値の形式(rtnFormat propsで指定可)
export const defaultRtnFormat = 'yyyy/MM/dd'

export default BaseComponent.extend({
  model: {
    prop: 'value',
    event: 'change',
  },
  components: {
    TextField,
    DatePicker,
  },
  props: {
    // datePicker コンポーネントの持つprops
    value: {
      type: String,
      default: '',
    },
    placeholder: {
      type: String,
      required: false,
    },
    required: {
      type: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    allowedDates: {
      type: Function,
    },
    minDate: {
      type: String,
      default: null,
    },
    maxDate: {
      type: String,
      default: null,
    },

    // custom props
    rtnFormat: {
      type: String,
      default: defaultRtnFormat,
    },
    isNoIcon: {
      type: Boolean,
      default: false,
    },
    initDate: {
      type: String,
      default: '',
    },
    inComponentFormat: {
      type: String,
      default: DEFAULT_FORMAT,
    },
    styleSet: {
      type: String,
      default: '',
    },
    isReadOnly: {
      type: Boolean,
      default: true,
    },
  },
  watch: {
    isOpenMenu(val) {
      val && setTimeout(() => (this.activePicker = 'YEAR'))
    },
  },
  data() {
    return {
      isOpenMenu: false,
      internalValue: this.value || '', // 入力中の値を保持する変数
      previousValue: this.value, // 直前の値を保持
      activePicker: 'YEAR',
    }
  },
  computed: {
    icon() {
      return !this.isNoIcon ? 'mdi-calendar' : undefined
    },
    pickedDate(): string {
      return this.toDefaultFormat(this.value)
    },
    computedPlaceholder(): string {
      return this.placeholder ?? this.$t('common.placeholder.date_select')
    },
  },
  methods: {
    /**
     * 入力途中の値を保持する
     * @param {string} $event ユーザーの入力内容
     */
    handleInput($event: string) {
      this.internalValue = $event
    },

    onMenuToggle(isOpen: boolean) {
      if (isOpen) {
        this.activePicker = 'YEAR' // メニューを開いた瞬間に YEAR に設定
        this.$nextTick(() => {
          // 1フレーム遅らせる
          setTimeout(() => {
            // 実際にスクロールする関数
            this.scrollToActiveYear()
          }, 100)
        })
      }
    },

    scrollToActiveYear() {
      // ref で取ってきた DatePicker コンポーネント (あるいは DOM) を参照
      const datePickerComp = this.$refs.myDatePicker as Vue
      const el = datePickerComp.$el as HTMLElement

      // 「li.active」を探す
      const yearItemEl = el.querySelector('li.active')
      if (yearItemEl) {
        yearItemEl.scrollIntoView({ block: 'center', behavior: 'auto' })
      }
    },

    /**
     * DatePickerの値をスラッシュ形式に変換
     */
    handleDatePickerInput($event: string) {
      try {
        // 年→月→日へ移行する
        if (this.activePicker === 'YEAR') {
          this.activePicker = 'MONTH' // 次は月選択
          return // まだ値を確定しない
        } else if (this.activePicker === 'MONTH') {
          this.activePicker = 'DATE' // 次は日選択
          return // まだ値を確定しない
        }
        const dateObj = parse($event, this.inComponentFormat, getCurrentDate())
        const formattedDate = format(dateObj, this.rtnFormat)

        // フォーマット済みの値を反映
        this.internalValue = formattedDate
        // フォーマット済みの値を一時保存
        this.previousValue = formattedDate
        this.$emit('change', formattedDate)
        // 変更されたことを親コンポーネントに伝える
        this.$emit('input', 'change')
      } catch (error) {
        this.$logger.info('Invalid date format from DatePicker:', error)
        this.internalValue = '' // 不正な値の場合は空にする
        this.$emit('change', '')
      }
    },

    /**
     * フォーカスアウト時にフォーマットを適用する
     */
    applyFormatOnBlur() {
      try {
        // 年選択モードの場合
        if (this.internalValue.length === 4) {
          const year = parseInt(this.internalValue, 10)
          if (isNaN(year)) {
            throw new TypeError('Invalid year format')
          }
          this.internalValue = `${year}` // 年をそのまま保持
          this.previousValue = `${year}`
          this.$emit('change', this.internalValue)
          return
        }

        // 月選択モードの場合
        if (this.internalValue.length === 7) {
          // yyyy-MM形式
          const [year, month] = this.internalValue.split('-').map(Number)
          if (isNaN(year) || isNaN(month) || month < 1 || month > 12) {
            throw new TypeError('Invalid month format')
          }
          this.internalValue = `${year}-${String(month).padStart(2, '0')}` // yyyy-MM形式
          this.previousValue = this.internalValue
          this.$emit('change', this.internalValue)
          return
        }

        // 通常の日付 (yyyy/MM/dd) の処理
        const dateObj = parse(
          this.internalValue,
          this.rtnFormat,
          getCurrentDate()
        )
        const formattedDate = format(dateObj, this.rtnFormat)

        // 最大値チェック
        if (this.maxDate) {
          const maxYear = parseInt(this.maxDate.split('-')[0])
          const formattedYear = parseInt(formattedDate.split('/')[0])
          if (formattedYear > maxYear) {
            this.internalValue = this.previousValue
            return
          }
        }

        this.internalValue = formattedDate
        this.previousValue = formattedDate
        this.$emit('change', formattedDate)
      } catch {
        this.internalValue = this.previousValue
      }
    },
    toDefaultFormat(rtnFormatStr: string): string {
      try {
        const dateObj = parse(rtnFormatStr, this.rtnFormat, getCurrentDate())
        const formattedDate = format(dateObj, this.inComponentFormat)
        this.internalValue = rtnFormatStr

        return formattedDate
      } catch {
        this.$logger.info('cannot format date')

        return ''
      }
    },
  },
})
</script>

<style lang="scss">
.a-date-select {
  &.is-error {
    fieldset {
      color: $c-red !important;
    }
    .v-input__slot {
      background-color: $c-red-10 !important;
    }
  }
  &.is-no-icon {
    .v-input__prepend-outer {
      display: none !important;
    }
  }
  .v-text-field--enclosed.v-input--dense:not(.v-text-field--solo).v-text-field--outlined
    .v-input__prepend-outer {
    margin-top: 4px;
  }
  .v-text-field--outlined.v-input--dense.v-text-field--outlined
    > .v-input__control
    > .v-input__slot {
    min-height: 30px !important;
    font-size: 0.875rem !important;
  }
  .a-text-field {
    &.v-input--is-readonly {
      pointer-events: auto;
      touch-action: auto;
    }
  }
  .v-input__append-inner {
    margin-top: 5px !important;
  }
}
</style>
