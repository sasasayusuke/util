<template>
  <ModalEdit :title="title" is-delete>
    <Paragraph class="mb-0" style-set="modal-text1">
      {{ $t('common.delete.text', { text }) }}
    </Paragraph>
    <template #foot>
      <v-container>
        <v-row justify="center">
          <Button style-set="large-error" width="160" @click="deleteSchedule">
            {{ $t('common.button.deleteSchedule') }}
          </Button>
        </v-row>
        <v-row class="pt-6" justify="center">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            @click="$emit('click:closeDelete')"
          >
            {{ $t('common.button.cancel') }}
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
import { DeleteSurveySchedulesByIdAndDate } from '~/models/Schedule'

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
    }
  },
  created() {
    this.text = this.modalSchedule.sendDate
  },
  methods: {
    /** アンケート送信スケジュール削除 */
    async deleteSchedule() {
      this.clearErrorBar()

      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const surveyId = this.modalSchedule.surveyId
      const version = this.modalSchedule.version

      await DeleteSurveySchedulesByIdAndDate(surveyId, version)
        .then(() => {
          this.$emit('click:closeDelete')
          this.$router.push(detailPage)
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeDelete')
          this.$router.push(detailPage)
          this.$emit('refresh')
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
