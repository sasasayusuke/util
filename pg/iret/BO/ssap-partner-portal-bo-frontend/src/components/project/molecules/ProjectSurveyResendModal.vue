<template>
  <ModalEdit :title="title" is-resend>
    <Paragraph class="mb-0" style-set="modal-text1">
      {{ $t('common.resend.text', { text, name }) }}
    </Paragraph>
    <template #foot>
      <v-container>
        <v-row justify="center">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            @click="$emit('click:closeResend')"
          >
            {{ $t('common.button.cancel') }}
          </Button>
          <Button
            class="ml-2"
            style-set="large-primary"
            width="160"
            @click="resendSchedule"
          >
            {{ $t('common.button.resend') }}
          </Button>
        </v-row>
      </v-container>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import {
  Button,
  Sheet,
  Paragraph,
  AutoComplete,
  TextField,
  ErrorText,
  RadioGroup,
  Required,
  Select,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import BaseComponent from '~/common/BaseComponent'
import { ResendSurveyById } from '~/models/Schedule'

export default BaseComponent.extend({
  components: {
    ModalEdit,
    CommonDetailRow,
    Button,
    Sheet,
    Paragraph,
    AutoComplete,
    TextField,
    ErrorText,
    RadioGroup,
    Required,
    Select,
    DateSelect,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
      required: true,
    },
    /** 選択されたアンケート送信スケジュール情報 */
    modalSchedule: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      text: '',
      name: '',
    }
  },
  created() {
    this.text = this.modalSchedule.sendDate
    this.name = this.modalSchedule.resendSurveyName
  },
  methods: {
    /** 匿名アンケート再送信 */
    async resendSchedule() {
      this.clearErrorBar()
      const surveyId = this.modalSchedule.surveyId
      const projectId = this.$route.params.projectId
      const detailPage = '/project/' + projectId

      await ResendSurveyById(surveyId)
        .then(() => {
          this.$emit('click:closeResend')
          this.$router.push(detailPage)
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeResend')
        })
    },
  },
})
</script>

<style lang="scss" scoped>
.radio-text {
  font-size: 0.875rem !important;
  color: $c-black;
  display: flex;
  align-items: center;
}
.m-survery-modal__unit {
  border-bottom: 1px solid $c-gray-line;
  color: $c-black;
  &:nth-child(n + 2) {
    margin-top: 20px;
  }
}
</style>
