<template>
  <Sheet class="mx-2" width="110px">
    <v-menu
      ref="menu"
      v-model="isOpenMenu"
      :close-on-content-click="false"
      :nudge-right="40"
      :return-value.sync="localValue"
      transition="scale-transition"
      offset-y
      max-width="290px"
      min-width="290px"
    >
      <template #activator="{ on, attrs }">
        <v-text-field
          v-model="localValue"
          class="v-time-picker-icon-del"
          type="time"
          append-icon="mdi-clock-time-four-outline"
          :rules="validationCheck"
          v-bind="attrs"
          hide-details="auto"
          v-on="filteredOn(on)"
          @click:append="() => (isOpenMenu = true)"
          @input="handleInput"
        ></v-text-field>
      </template>
      <v-time-picker
        v-if="isOpenMenu"
        v-model="localValue"
        :allowed-minutes="allowedMinutes"
        format="24hr"
        @click:minute="handleInput"
      ></v-time-picker>
    </v-menu>
  </Sheet>
</template>

<script lang="ts">
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import BaseComponent from '~/common/BaseComponent'
import { TextField, Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    TextField,
    DateSelect,
    Sheet,
  },
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    value: {
      type: String,
      required: true,
    },
    step: {
      type: Number,
      default: null,
    },
    required: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    value(newVal) {
      this.localValue = newVal
    },
  },
  data() {
    return {
      isOpenMenu: false,
      localValue: this.value,
    }
  },
  computed: {
    allowedMinutes() {
      // 分刻みのステップが渡されていれば許可関数を返す
      if (this.step && this.step > 0 && this.step < 60) {
        return (min: number) => min % this.step === 0
      }
      // 無効な場合は制限なし（すべてtrue）
      return () => true
    },
    // 開催時間の必須チェック
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.required) {
        const rule = (value: string[]) => {
          return value.length > 0 || this.$t('common.rule.required')
        }
        rules.push(rule)
      }

      // 時刻フォーマット（HH:mm）チェック
      const timeFormatRule = (value: string) => {
        const regex = /^([01]\d|2[0-3]):[0-5]\d$/
        return regex.test(value) || this.$t('common.rule.invalidTimeFormat')
      }
      rules.push(timeFormatRule)

      return rules
    },
  },
  methods: {
    filteredOn(on: { [x: string]: any; click: any }) {
      return Object.fromEntries(
        Object.entries(on).filter(([key]) => key !== 'click')
      )
    },
    handleInput() {
      ;(this.$refs.menu as any).save(this.localValue)
      this.$emit('input', this.localValue)
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

.v-time-picker-icon-del {
  padding-top: 0px;
  margin-top: 0px;

  input {
    padding: 0;
  }
  input::-webkit-calendar-picker-indicator {
    display: none !important;
  }
}
</style>
