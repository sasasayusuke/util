<template>
  <div class="a-date-select">
    <v-menu
      ref="menu"
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
          slot="activator"
          v-model="value"
          :aria-label="$t('survey.pages.admin.list.sort_input.summaryYear')"
          :prepend-icon="!isNoIcon ? 'mdi-calendar' : undefined"
          role="textbox"
          outlined
          dense
          readonly
          :disabled="disabled"
          :style-set="styleSet"
          v-bind="attrs"
          v-on="on"
        ></TextField>
      </template>
      <template #default>
        <v-date-picker
          ref="picker"
          :value="formattedValue"
          class="a-date-picker"
          color="btn_primary"
          reactive
          show-current
          :min="min"
          :max="`${max}-01-01`"
          no-title
          :active-picker.sync="activePicker"
          @input="onChange"
        ></v-date-picker>
      </template>
    </v-menu>
  </div>
</template>

<script lang="ts">
import { Const } from '~/const'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import BaseComponent from '~/common/BaseComponent'
import { TextField } from '~/components/common/atoms/index'
import { toFiscalYear } from '~/utils/common-functions'
// このコンポーネント内で扱う日付値の形式(inComponentFormat propsで指定可能)
// よくわからない場合は変更しないことを推奨
const DEFAULT_FORMAT = 'yyyy'
// 親コンポーネントとやりとりされるときに用いられる日付値の形式(rtnFormat propsで指定可)
const defaultRtnFormat = 'yyyy'
const datePickerFormat = 'yyyy-MM-dd'
export default BaseComponent.extend({
  components: {
    TextField,
    DateSelect,
  },
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
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
    // custom props
    rtnFormat: {
      type: String,
      default: defaultRtnFormat,
    },
    datePickerFormat: {
      type: String,
      default: datePickerFormat,
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
    value: {
      type: String,
      default: '2022',
    },
  },
  watch: {
    isOpenMenu(val) {
      val && this.$nextTick(() => (this.activePicker = 'YEAR'))
    },
  },
  data() {
    return {
      isOpenMenu: false,
      activePicker: 'YEAR',
    }
  },
  computed: {
    min(): string {
      const fiscalYear = Const.MIN_FISCAL_YEAR
      return fiscalYear.toString()
    },
    max(): string {
      const thisFiscalYear = toFiscalYear()
      return thisFiscalYear.toString()
    },
    formattedValue(): string {
      return `${this.value}-01-01`
    },
  },
  methods: {
    onChange($event: string) {
      this.$logger.debug($event)
      if (this.$refs.menu) {
        const menu: any = this.$refs.menu
        menu.save(this.value)
        this.isOpenMenu = false
      }
      const year: string = $event.substring(0, 4)
      this.$emit('change', year)
    },
  },
})
</script>

<style lang="scss">
.v-date-picker-years {
  li {
    &:hover {
      background-color: $c-primary-8;
    }
  }
}
.v-date-picker-years {
  width: auto;
  height: auto;
  max-height: 290px;
  max-width: 290px;
}
</style>
