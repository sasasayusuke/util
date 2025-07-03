<template>
  <v-stepper
    v-model="step"
    vertical
    tile
    elevation="0"
    pa-0
    class="o-otp-stepper"
  >
    <v-stepper-step class="o-otp-stepper__unit" step="1">
      <v-layout class="flex-column">
        <v-flex>
          <p class="text-body-2 font-weight-bold">
            {{ $t('top.pages.otp.descriptions.sendOtp') }}
          </p>
        </v-flex>
        <v-flex class="pt-6 mr-16 ml-n8" align-self-center>
          <Button
            class="white--text font-weight-bold"
            x-large
            color="primary"
            width="300px"
            :loading="isSendingMail"
            @click="onClickSendMail"
          >
            {{ $t('common.button.otp') }}
          </Button>
        </v-flex>
      </v-layout>
    </v-stepper-step>
    <v-stepper-step class="o-otp-stepper__unit" step="2">
      <v-layout class="flex-column">
        <v-flex>
          <p class="text-body-2 font-weight-bold">
            {{ $t('top.pages.otp.descriptions.inputOtp') }}
            <span class="a-required mr-1 font-size-xsmall">{{
              $t('common.label.required3')
            }}</span>
          </p>
        </v-flex>
        <v-flex class="mr-16 ml-n8" align-self-center>
          <v-card tile elevation="0" width="400px">
            <TextField
              v-model="otp"
              :placeholder="$t('top.pages.otp.textFieldLabel')"
              :append-icon="showsOtp ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showsOtp ? 'text' : 'password'"
              :rules="otpInputRules"
              max-length="10"
              required
              @click:append="showsOtp = !showsOtp"
            />
          </v-card>
        </v-flex>
        <v-flex class="mr-16 ml-n8" align-self-center>
          <Button
            class="white--text font-weight-bold mt-6"
            x-large
            width="300px"
            color="primary"
            :loading="isAuthorizing"
            :disabled="!otp"
            @click="$emit('next', otp)"
          >
            {{ $t('common.button.next') }}
          </Button>
        </v-flex>
      </v-layout>
    </v-stepper-step>
  </v-stepper>
</template>

<script lang="ts">
import { Button, TextField } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    TextField,
  },
  props: {
    /**
     * メールを送信したか否か
     */
    isSendingMail: {
      type: Boolean,
    },
    /**
     * 認証したか否か
     */
    isAuthorizing: {
      type: Boolean,
    },
  },
  data() {
    return {
      otp: '',
      step: 1,
      otpInputRules: [
        (value: string) =>
          (value && value.length >= 8) ||
          this.$t('top.pages.otp.errorMessages.tooShort'),
      ],
      showsOtp: false,
    }
  },
  methods: {
    /**
     * OTPが含まれるメールを送信
     */
    onClickSendMail() {
      this.$emit('generateOtp')
      this.step = 2
    },
  },
})
</script>

<style lang="scss">
.o-otp-stepper {
  .v-stepper__step__step {
    width: 70px !important;
    height: 70px !important;
    margin-right: 0 !important;
    font-size: 32px !important;
    font-weight: bold !important;
    position: relative !important;
    z-index: 2;
    background-color: $c-primary-dark !important;
    &.primary {
      background-color: $c-primary-dark !important;
    }
  }
  .v-stepper__label {
    width: calc(532px - 95px + 25px) !important;
    padding-left: 25px !important;
  }
  .o-otp-stepper__unit {
    &:nth-child(1) {
      .v-stepper__step__step {
        top: -12px;
      }
    }
    &:nth-child(2) {
      .v-stepper__step__step {
        top: -24px;
      }
    }
    &::after {
      content: '';
      display: block;
      width: 2px;
      height: 100%;
      position: absolute;
      top: 0;
      left: 35px;
      background-color: $c-primary-dark;
    }
  }
}
</style>
<style lang="scss" scoped>
.o-otp-stepper {
  position: relative;
  padding-top: 40px;
  padding-bottom: 0;
}
.o-otp-stepper__unit {
  position: relative;
  width: calc(553px + (95px * 2));
  padding: 0 0 0 0;
  align-items: flex-start;
  &:nth-child(n + 2) {
    padding-top: 82px;
    &::before {
      top: 60px;
    }
  }
}
.text-body-2 {
  word-break: keep-all;
  color: $c-black;
}
.a-required {
  color: $c-red;
  font-weight: normal;
}
</style>
