<template>
  <div class="a-date-select">
    <v-menu
      v-model="menu"
      :close-on-content-click="false"
      :nudge-right="40"
      transition="scale-transition"
      offset-y
      min-width="auto"
      :disabled="disabled"
      :value="shown"
    >
      <template #activator="{ on, attrs }">
        <TextField
          v-model="time"
          role="textbox"
          :prepend-icon="!isNoIcon ? 'mdi-clock-outline' : undefined"
          :placeholder="$t('common.placeholder.time_select')"
          readonly
          outlined
          dense
          :disabled="disabled"
          v-bind="attrs"
          v-on="on"
        ></TextField>
      </template>
      <TimePicker :value="time" @close="menu = false" @setTime="setTime" />
    </v-menu>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { TextField } from '~/components/common/atoms/index'

import TimePicker from '~/components/common/molecules/TimePicker.vue'

export default BaseComponent.extend({
  model: {
    prop: 'value',
    event: 'change',
  },
  components: {
    TextField,
    TimePicker,
  },
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    isNoIcon: {
      type: Boolean,
      default: false,
    },
    value: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      shown: false,
      time: this.value,
      menu: false,
      class: [],
    }
  },
  methods: {
    setTime(time: any) {
      this.time = `${String(time.hour).padStart(2, '0')}:${String(
        time.min
      ).padStart(2, '0')}`
      this.$emit('change', this.time)
      this.menu = false
    },
    formatDate(date: any) {
      if (!date) return null
      const [year, month, day] = date.split('-')
      return `${year}/${month}/${day}`
    },
    parseDate(date: any) {
      if (!date) return null
      const [month, day, year] = date.split('/')
      return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
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
}
</style>
