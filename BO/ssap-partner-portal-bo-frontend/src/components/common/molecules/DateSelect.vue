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
    >
      <template #activator="{ on, attrs }">
        <TextField
          :value="value"
          :prepend-icon="icon"
          :placeholder="computedPlaceholder"
          role="textbox"
          readonly
          outlined
          dense
          :required="required"
          :style-set="styleSet"
          :disabled="disabled"
          v-bind="attrs"
          v-on="on"
          @input="input"
        ></TextField>
      </template>
      <DatePicker
        :value="pickedDate"
        :allowed-dates="allowedDates"
        no-title
        @input="input"
      ></DatePicker>
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
const DEFAULT_FORMAT = 'yyyy-MM-dd'
// 親コンポーネントとやりとりされるときに用いられる日付値の形式(rtnFormat propsで指定可)
const defaultRtnFormat = 'yyyy/MM/dd'

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
    disabled: {
      type: Boolean,
      default: false,
    },
    allowedDates: {
      type: Function,
    },
    required: {
      type: Boolean,
      default: false,
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
    type: {
      type: String,
      default: 'date',
    },
  },
  data() {
    return {
      isOpenMenu: false,
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
    input($event: string) {
      this.isOpenMenu = false

      const dateObj = parse($event, this.inComponentFormat, getCurrentDate())
      const formattedDate = format(dateObj, this.rtnFormat)

      this.$emit('change', formattedDate)
    },
    toDefaultFormat(rtnFormatStr: string): string {
      try {
        const dateObj = parse(rtnFormatStr, this.rtnFormat, getCurrentDate())
        const formattedDate = format(dateObj, this.inComponentFormat)

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
}
</style>
