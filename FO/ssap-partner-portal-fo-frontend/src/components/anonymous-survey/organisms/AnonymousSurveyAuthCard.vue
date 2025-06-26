<template>
  <v-row justify="center">
    <v-col cols="auto">
      <Sheet
        class="o-AnonymousSurveyCard mt-7 px-7 py-12"
        width="664"
        elevation="3"
        rounded
        color="white"
      >
        <h1 class="o-AnonymousSurveyCard__title">
          {{ $t('anonymous-survey.pages.password.title') }}
        </h1>
        <p class="o-AnonymousSurveyCard__text">
          <span>{{ $t('anonymous-survey.pages.password.text') }}</span>
          <span class="font-size-xsmall m-AnonymousSurveyCard__require-text">{{
            $t('common.label.required')
          }}</span>
        </p>
        <v-row no-gutters align="center" justify="center">
          <v-card tile elevation="0" width="400px">
            <TextField
              v-model="localParam.password"
              :append-icon="showText ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showText ? 'text' : 'password'"
              required
              @click:append="showText = !showText"
            />
          </v-card>
        </v-row>
        <v-row no-gutters align="center" justify="center" class="mt-6">
          <v-col cols="auto">
            <Button
              style-set="xlarge-primary"
              width="400"
              :disabled="localParam.password.length === 0 || isLoading"
              @click="buttonAction"
            >
              {{ $t('common.button.send2') }}
            </Button>
          </v-col>
        </v-row>
      </Sheet>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import { Button, Sheet, TextField } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { CheckSurveyByIdPasswordRequest } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    Button,
    Sheet,
    TextField,
  },
  props: {
    /**
     * 匿名アンケートの認証情報
     */
    param: {
      type: Object as PropType<CheckSurveyByIdPasswordRequest>,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      localParam: cloneDeep(this.param),
      showText: false,
    }
  },
  methods: {
    buttonAction() {
      this.$emit('auth', this.localParam)
    },
  },
})
</script>

<style lang="scss" scoped>
.o-AnonymousSurveyCard__title {
  text-align: center;
  margin-bottom: 32px;
  font-size: 1.5rem;
}
.o-AnonymousSurveyCard__text {
  text-align: center;
  @include fontSize('small');
  line-height: 1.5;
  font-weight: bold;
  margin-bottom: 36px;
  span {
    white-space: pre-line;
  }
  .m-AnonymousSurveyCard__require-text {
    color: $c-red;
  }
}
</style>
