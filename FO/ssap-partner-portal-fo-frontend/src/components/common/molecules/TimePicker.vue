<template>
  <div class="m-time-picker">
    <div class="m-time-picker__body">
      <div class="m-time-picker__time">
        <span class="m-time-picker__hour">
          <Sheet width="45">
            <Select
              v-model="hour"
              style-set="outlined"
              append-icon=""
              class="m-time-picker__input"
              :default-time="showDefaultTime"
              :items="hourOptions"
              item-text="label"
              item-value="value"
            />
          </Sheet>
        </span>
        <span class="m-time-picker__separator">:</span>
        <span class="m-time-picker__min">
          <Sheet width="45">
            <Select
              v-model="min"
              style-set="outlined"
              append-icon=""
              class="m-time-picker__input"
              :default-time="showDefaultTime"
              :items="minOptions"
              item-text="label"
              item-value="value"
            />
          </Sheet>
        </span>
      </div>
    </div>
    <div class="m-time-picker__bottom">
      <Button text small color="btn_tertiary" @click="$emit('close')">
        キャンセル
      </Button>
      <Button text small color="btn_primary" @click="setTime"> OK </Button>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, Sheet, Select } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Button,
    Sheet,
    Select,
  },
  props: {
    value: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      hour: 0,
      min: 0,
    }
  },
  computed: {
    showDefaultTime() {
      const splitTime = String(this.value).split(':')
      const trimmedHour = splitTime[0].replace(/^0+/, '')
      this.hour = Number(trimmedHour)
      const trimmedMin = splitTime[1].replace(/^0+/, '')
      this.min = Number(trimmedMin)
    },
    hourOptions() {
      const hourItems: object[] = []
      const hourItem = { label: '', value: 0 }
      for (let hour = 0; hour < 24; hour++) {
        hourItem.label = String(hour).padStart(2, '0')
        hourItem.value = hour
        const newElm = Object.assign({}, hourItem)
        hourItems.push(newElm)
      }
      return hourItems
    },
    minOptions() {
      const minItems: object[] = []
      const minItem = { label: '', value: 0 }
      for (let min = 0; min < 60; min++) {
        minItem.label = String(min).padStart(2, '0')
        minItem.value = min
        const newElm = Object.assign({}, minItem)
        minItems.push(newElm)
      }
      return minItems
    },
  },
  methods: {
    setTime() {
      this.$emit('setTime', { hour: this.hour, min: this.min })
    },
  },
})
</script>

<style lang="scss">
.m-time-picker {
  width: 200px;
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: $c-white;
}
.m-time-picker__body {
  width: 122px;
  padding: 16px 0;
}
.m-time-picker__time {
  font-size: 30px;
  font-weight: bold;
  display: flex;
  justify-content: space-between;
  line-height: 1;
  padding: 10px 0;
}
.m-time-picker__input {
  position: relative;
  width: 45px;
  text-align: center;
  border: 0;
  padding: 0;
  margin: 0;
  top: -1px;
  &::-webkit-outer-spin-button,
  &::-webkit-inner-spin-button {
    -webkit-appearance: none;
    -moz-appearance: textfield;
  }
  > .v-input__control
    > .v-input__slot
    > .v-select__slot
    > .v-select__selections
    > input {
    display: none;
  }
}
.m-time-picker__bottom {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  height: 52px;
  width: 100%;
  border-top: 1px solid $c-black-question;
  padding-right: 15px;
}
</style>
