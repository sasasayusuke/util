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
          <!-- 一括予定を全て削除を追加する場合は「class="ml-4"」付きで -->
          <!-- <Button
            style-set="large-error"
            class="ml-4"
            width="160"
            @click="deleteSchedule"
          >
            {{ $t('common.button.deleteScheduleAll') }}
          </Button> -->
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
import { DeleteSupportSchedulesByIdAndDate } from '~/models/Schedule'

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
    /** 選択された支援日スケジュール情報 */
    modalSchedule: {
      type: Object,
      required: true,
    },
    /** 個別カルテ・スケジュール一覧画面か */
    isKarteList: {
      type: Boolean,
      required: false,
    },
  },
  data() {
    return {
      text: '',
    }
  },
  created() {
    const scheduleAry = this.modalSchedule.supportDate.split(' ')
    this.text = scheduleAry[0]
  },
  methods: {
    /** 個別予定の削除 */
    async deleteSchedule() {
      this.clearErrorBar()

      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const karteListPage = '/karte/list/' + id
      const karteId = this.modalSchedule.karteId
      const version = this.modalSchedule.version

      await DeleteSupportSchedulesByIdAndDate(karteId, version)
        .then(() => {
          this.$emit('click:closeDelete')
          this.$emit('refresh')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeDelete')
          this.$emit('refresh')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
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
