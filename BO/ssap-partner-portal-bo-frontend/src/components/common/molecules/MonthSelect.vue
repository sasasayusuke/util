<!-- TODO: 日付複数選択には現在未対応なので、必要になったときに対応する -->

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
          :value="value"
          :prepend-icon="!isNoIcon ? 'mdi-calendar' : undefined"
          :placeholder="computedPlaceholder"
          role="textbox"
          readonly
          outlined
          dense
          :disabled="disabled"
          :style-set="styleSet"
          v-bind="attrs"
          v-on="on"
          @input="input"
        ></TextField>
      </template>
      <DatePicker
        :value="pickedDate"
        month
        no-title
        :allowed-dates="allowedDates"
        @input="input"
      ></DatePicker>
    </v-menu>
  </div>
</template>

<script lang="ts">
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import { TextField } from '~/components/common/atoms'

// このコンポーネント内で扱う日付値の形式(inComponentFormat propsで指定可能)
// よくわからない場合は変更しないことを推奨
const DEFAULT_FORMAT = 'yyyy-MM'
// 親コンポーネントとやりとりされるときに用いられる日付値の形式(rtnFormat propsで指定可)
const defaultRtnFormat = 'yyyy/MM'

export default DateSelect.extend({
  components: {
    DateSelect,
    TextField,
  },
  props: {
    // custom props
    rtnFormat: {
      type: String,
      default: defaultRtnFormat,
    },
    inComponentFormat: {
      type: String,
      default: DEFAULT_FORMAT,
    },
  },
  computed: {
    computedPlaceholder(): string {
      return this.placeholder ?? this.$t('common.placeholder.month_select')
    },
  },
})
</script>
