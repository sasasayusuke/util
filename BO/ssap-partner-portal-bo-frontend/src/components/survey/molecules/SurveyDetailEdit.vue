<template>
  <v-form class="pt-4 px-8 pb-0" :value="isValid" @input="$listeners['input']">
    <v-container pa-0>
      <v-row no-gutters>
        <v-col
          cols="auto"
          align-self="center"
          class="font-size-small font-weight-bold"
        >
          {{ $t('survey.pages.detail.summaryMonth') }}
        </v-col>
        <v-col cols="auto" align-self="center">
          <Sheet width="130">
            <MonthSelect
              v-model="localSurvey.summaryMonth"
              @change="update('summaryMonth', $event)"
            />
          </Sheet>
        </v-col>
        <v-col
          cols="auto"
          align-self="center"
          class="pl-8 font-size-small font-weight-bold"
        >
          {{ $t('survey.pages.detail.isNotSummary') }}
        </v-col>
        <v-col cols="auto" align-self="center">
          <RadioGroup
            v-model="localSurvey.isNotSummary"
            :labels="[
              $t('survey.pages.detail.target'),
              $t('survey.pages.detail.exclusion'),
            ]"
            :values="[false, true]"
            horizontal
            hide-details
            @change="update('isNotSummary', $event)"
          />
        </v-col>
        <v-col
          cols="auto"
          align-self="center"
          class="pl-8 font-size-small font-weight-bold"
        >
          {{ $t('survey.pages.detail.isSolverProject') }}
        </v-col>
        <v-col cols="auto" align-self="center">
          <RadioGroup
            v-model="localSurvey.isSolverProject"
            :labels="[$t('common.detail.yes'), $t('common.detail.no')]"
            :values="[true, false]"
            horizontal
            hide-details
            :disabled="!isSurveyOpsOrSystemAdmin()"
            @change="update('isSolverProject', $event)"
          />
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script lang="ts">
import {
  TextField,
  RadioGroup,
  Select,
  ToolTips,
  Sheet,
  Required,
} from '~/components/common/atoms'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import { GetSurveyByIdResponse } from '~/models/Survey'
import { meStore } from '~/store'

export default BaseComponent.extend({
  components: {
    TextField,
    RadioGroup,
    Select,
    ToolTips,
    Sheet,
    Required,
    MonthSelect,
  },
  props: {
    /** アンケート情報 */
    survey: {
      type: Object as PropType<GetSurveyByIdResponse>,
    },
  },
  data() {
    return {
      headButtons: [],
      footButtons: [],
      value: '',
      localSurvey: this.survey,
    }
  },
  computed: {},
  methods: {
    /**
     * 他コンポーネントへアンケート情報の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
    /**
     * アンケート事務局またはシステム管理者ロールが含まれているアカウントか判定
     * アンケート事務局またはシステム管理者ロールが含まれているアカウントかの真偽値
     */
    isSurveyOpsOrSystemAdmin() {
      return (
        meStore.roles.includes('survey_ops') ||
        meStore.roles.includes('system_admin')
      )
    },
  },
})
</script>

<style lang="scss" scoped></style>
