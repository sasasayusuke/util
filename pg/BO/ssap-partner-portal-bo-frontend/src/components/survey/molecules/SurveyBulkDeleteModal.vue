<template>
  <ModalEdit :title="title" is-delete>
    <Paragraph class="my-6 m-survery-modal__text" style-set="modal-text2">
      {{ $t('common.delete.text2') }}
    </Paragraph>
    <template #foot>
      <v-container class="pt-0 m-survery-modal__container">
        <v-row justify="center">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            @click="$emit('click:closeDelete')"
          >
            {{ $t('common.button.cancel') }}
          </Button>
          <Button
            class="ml-2"
            style-set="large-error"
            width="160"
            :loading="isDeleting"
            @click="deleteMultipleSurveySchedules"
          >
            {{ $t('common.button.delete') }}
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
import {
  DeleteMultipleSurveySchedules,
  DeleteMultipleSurveySchedulesRequest,
} from '~/models/Schedule'
import { SurveyPlanListItem } from '~/models/Survey'
import type { PropType } from '~/common/BaseComponent'

// 500件ずつに分割する
function splitAssociativeArray(
  obj: DeleteMultipleSurveySchedulesRequest,
  chunkSize: number
): DeleteMultipleSurveySchedulesRequest[] {
  const transformedData = []

  for (let i = 0; i < obj.surveyIds.length; i += chunkSize) {
    const chunk = obj.surveyIds.slice(i, i + chunkSize)
    transformedData.push({ surveyIds: chunk })
  }

  return transformedData
}

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
    selectedSurveys: {
      type: Array as PropType<SurveyPlanListItem[]>,
    },
  },
  data() {
    return {
      isDeleting: false,
    }
  },
  methods: {
    generateDeleteRequest(): DeleteMultipleSurveySchedulesRequest {
      const scheduleIds: string[] = []

      this.selectedSurveys.forEach((survey) => {
        // 送信実績があるアンケートはリクエストから排除
        if (survey.actualSurveyRequestDatetime) {
          return
        }
        // 回答実績があるアンケートはリクエストから排除
        if (survey.actualSurveyResponseDatetime) {
          return
        }

        if (survey.id) {
          scheduleIds.push(survey.id)
        }
      })

      const rtn = new DeleteMultipleSurveySchedulesRequest(scheduleIds)

      return rtn
    },
    async deleteMultipleSurveySchedules() {
      this.isDeleting = true

      const request = this.generateDeleteRequest()

      const splittedAssociativeArray = splitAssociativeArray(request, 500)

      //DeleteMultipleSurveySchedules実行関数
      await DeleteMultipleSurveySchedules(splittedAssociativeArray).then(() => {
        this.isDeleting = false
        this.$emit('click:delete')
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
.m-survery-modal__text {
  text-align: center;
  font-weight: bold;
}
// .m-survery-modal__container {
//   margin-top: -24px;
// }
</style>
