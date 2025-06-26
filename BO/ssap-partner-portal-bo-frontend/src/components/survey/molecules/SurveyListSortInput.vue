<template>
  <Sheet class="d-flex align-center m-SurveyListSortInput">
    <v-container pa-0>
      <v-row class="m-SurveyListSortInput__unit" no-gutters>
        <v-col class="d-flex align-center" cols="auto">
          <RadioGroup
            v-model="param.searchType"
            :labels="[$t('survey.pages.list.sort_input.summaryMonth')]"
            :values="[1]"
            :class="{ 'radio-display': !isSurveyOps }"
            horizontal
            hide-details
            @change="update('searchType', $event)"
          />
          <span v-if="!isSurveyOps">{{
            $t('survey.pages.list.sort_input.summaryMonth')
          }}</span>
          <Sheet width="260" class="d-flex align-center mr-7 ml-2">
            <MonthSelect
              v-model="param.summaryMonthFrom"
              style-set="bgWhite"
              @change="update('summaryMonthFrom', $event)"
            />&nbsp;〜&nbsp;
            <MonthSelect
              v-model="param.summaryMonthTo"
              style-set="bgWhite"
              is-no-icon
              @change="update('summaryMonthTo', $event)"
            />
          </Sheet>
        </v-col>
        <v-col v-if="isSurveyOps" class="d-flex align-center" cols="auto">
          <RadioGroup
            v-model="param.searchType"
            :labels="[$t('survey.pages.list.sort_input.terms')]"
            :values="[2]"
            horizontal
            hide-details
            @change="update('searchType', $event)"
          />
          <Sheet width="280" class="d-flex align-center mr-7 ml-2">
            <DateSelect
              v-model="param.planSurveyResponseDateFrom"
              style-set="bgWhite"
              @change="update('planSurveyResponseDateFrom', $event)"
            />&nbsp;〜&nbsp;
            <DateSelect
              v-model="param.planSurveyResponseDateTo"
              style-set="bgWhite"
              is-no-icon
              @change="update('planSurveyResponseDateTo', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
      <v-row
        no-gutters
        class="font-size-small m-SurveyListSortInput__unit mt-4"
        align="center"
      >
        <v-col cols="auto" class="mr-2">
          {{ $t('survey.pages.list.sort_input.customerName') }}
        </v-col>
        <v-col cols="auto">
          <Sheet width="240">
            <AutoComplete
              v-model="param.customerId"
              outlined
              dense
              :items="customers"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.autoComplete')"
              hide-details
              style-set="bgWhite"
              :max-length="255"
              @change="update('customerId', $event)"
            />
          </Sheet>
        </v-col>
        <v-col cols="auto" class="mr-2 ml-7">
          {{ $t('survey.pages.list.sort_input.projectName') }}
        </v-col>
        <v-col cols="auto" class="mr-7">
          <Sheet width="240">
            <AutoComplete
              v-model="param.projectId"
              outlined
              dense
              :items="projects"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.autoComplete')"
              hide-details
              style-set="bgWhite"
              :max-length="120"
              @change="update('projectId', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
      <v-row
        no-gutters
        class="font-size-small mt-4 m-SurveyListSortInput__unit"
        align="center"
      >
        <v-col cols="auto" class="mr-2">
          {{ $t('survey.pages.list.sort_input.serviceName') }}
        </v-col>
        <v-col cols="auto">
          <Sheet width="240">
            <Select
              v-model="param.serviceTypeId"
              style-set="outlined bgWhite"
              :aria-label="$t('survey.pages.list.sort_input.serviceName')"
              :items="serviceTypes"
              item-text="name"
              item-value="id"
              @change="update('serviceTypeId', $event)"
            />
          </Sheet>
        </v-col>
        <v-col cols="auto" class="mr-2 ml-7">
          {{ $t('survey.pages.list.sort_input.organization') }}
        </v-col>
        <v-col cols="auto" class="mr-7">
          <Sheet width="240">
            <Select
              v-model="param.organizationIds"
              style-set="outlined bgWhite"
              :aria-label="$t('survey.pages.list.sort_input.organization')"
              multiple
              :items="supporterOrganizations"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.selectMulti')"
              @change="update('organizationIds', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import { meStore } from '~/store'
import {
  TextField,
  Sheet,
  RadioGroup,
  AutoComplete,
  Select,
} from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    TextField,
    Sheet,
    RadioGroup,
    AutoComplete,
    Select,
    DateSelect,
    MonthSelect,
  },
  props: {
    /** サービス種別 */
    serviceTypes: {
      type: Array,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** 案件のサジェスト用情報一覧 */
    projects: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用情報一覧 */
    customers: {
      type: Array,
      required: true,
    },
    /** 検索条件 */
    param: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      searchType: 1,
      monthFrom: '',
      monthTo: '',
      dateFrom: '',
      dateTo: '',
      customerName: '',
      projectName: '',
      serviceName: '',
      organization: '',
    }
  },
  computed: {
    /**
     * SurveyOpsロールが含まれているアカウントか判定
     * @returns 判定結果真偽値
     */
    isSurveyOps() {
      return meStore.roles.includes('survey_ops')
    },
  },
  methods: {
    /**
     * 他コンポーネントへ検索条件の変更を受け渡す
     * @param item 変更項目
     * @param event 変更値
     */
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>

<style lang="scss" scoped>
.m-SurveyListSortInput__unit {
  &:nth-child(n + 2) {
    margin-left: -108px;
  }
  .radio-display {
    display: none;
  }
}
</style>
