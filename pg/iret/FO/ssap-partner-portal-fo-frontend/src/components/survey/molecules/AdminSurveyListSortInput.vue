<template>
  <Sheet class="d-flex align-center m-SurveyListSortInput">
    <v-container pa-0>
      <v-row class="m-SurveyListSortInput__unit" no-gutters>
        <v-col class="d-flex align-center pr-6" cols="auto">
          <span class="font-size-small mr-2"
            >{{ $t('survey.pages.admin.list.sort_input.summaryYear') }}
            <Required
          /></span>
          <Sheet class="mr-2" width="130">
            <YearSelect
              v-model="param.year"
              style-set="bgWhite"
              @change="$emit('update', 'year', $event)"
            />
          </Sheet>
        </v-col>
        <v-col class="d-flex align-center pr-6" cols="auto">
          <span class="font-size-small mr-2">{{
            $t('survey.pages.admin.list.sort_input.customerName')
          }}</span>
          <Sheet class="mr-2" width="240">
            <AutoComplete
              v-model="param.customerName"
              outlined
              dense
              :items="customers"
              item-text="name"
              item-value="id"
              hide-details
              style-set="bgWhite"
              :placeholder="$t('common.placeholder.autoComplete')"
              @change="$emit('update', 'customerName', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
      <v-row class="m-SurveyListSortInput__unit pt-4" no-gutters>
        <v-col class="d-flex align-center pr-6" cols="auto">
          <span class="font-size-small mr-2">{{
            $t('survey.pages.admin.list.sort_input.projectName')
          }}</span>
          <Sheet class="mr-2" width="240">
            <AutoComplete
              v-model="param.projectName"
              outlined
              dense
              :items="projects"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.autoComplete')"
              hide-details
              style-set="bgWhite"
              @change="$emit('update', 'projectName', $event)"
            />
          </Sheet>
        </v-col>
        <v-col class="d-flex align-center pr-6" cols="auto">
          <span class="font-size-small mr-2">{{
            $t('survey.pages.admin.list.sort_input.serviceName')
          }}</span>
          <Sheet class="mr-2" width="240">
            <Select
              v-model="param.serviceName"
              style-set="outlined bgWhite"
              :aria-label="$t('survey.pages.admin.list.sort_input.serviceName')"
              :items="serviceTypes"
              item-text="name"
              item-value="id"
              @change="$emit('update', 'serviceName', $event)"
            />
          </Sheet>
        </v-col>
        <v-col v-if="!isSupporter" class="d-flex align-center pr-6" cols="auto">
          <span class="font-size-small mr-2">{{
            $t('survey.pages.admin.list.sort_input.organization')
          }}</span>
          <Sheet class="mr-2" width="240">
            <Select
              v-model="param.organization"
              multiple
              style-set="outlined bgWhite"
              :aria-label="
                $t('survey.pages.admin.list.sort_input.organization')
              "
              :items="supporterOrganizations"
              item-text="name"
              item-value="id"
              @change="$emit('update', 'organization', $event)"
            />
          </Sheet>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import { toFiscalYear } from '~/utils/common-functions'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import YearSelect from '~/components/common/molecules/YearSelect.vue'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import {
  TextField,
  Sheet,
  RadioGroup,
  AutoComplete,
  Select,
  Required,
} from '~/components/common/atoms'
import { SuggestProjectsResponse } from '~/models/Project'
import { SuggestCustomersResponse } from '~/models/Customer'
import { SupporterOrganizationItems, ServiceTypeItems } from '~/models/Master'
import { meStore } from '~/store'

export class InputCandidateLoading {
  customers = false
  projects = false
  serviceTypes = false
  supporterOrganizations = false
}

export class SearchParam {
  year: string = toFiscalYear().toString()
  customerName: string = ''
  projectName: string = ''
  serviceName: string = ''
  organization = []
}

export default BaseComponent.extend({
  components: {
    TextField,
    Sheet,
    RadioGroup,
    AutoComplete,
    Select,
    DateSelect,
    YearSelect,
    MonthSelect,
    Required,
  },
  props: {
    /** リクエストパラメーター */
    param: {
      type: SearchParam,
      required: true,
    },
    /** サービス種別 */
    serviceTypes: {
      type: Array as PropType<ServiceTypeItems[]>,
      required: true,
    },
    /** 案件別の支援工数状況 */
    supporterOrganizations: {
      type: Array as PropType<SupporterOrganizationItems[]>,
      required: true,
    },
    /** 案件一覧 */
    projects: {
      type: SuggestProjectsResponse,
      required: true,
    },
    /** お客様一覧 */
    customers: {
      type: SuggestCustomersResponse,
      required: true,
    },
    /** API呼び出しの有無 */
    isLoading: {
      type: InputCandidateLoading,
    },
  },
  computed: {
    /**
     * ロール(役職)チェック
     * @return お客様ロールであればtrue
     */
    isSupporter(): boolean {
      if (meStore.role === 'supporter') {
        return true
      } else {
        return false
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.m-SurveyListSortInput__unit {
  &:nth-child(n + 2) {
    margin-left: -96px;
  }
}
</style>
