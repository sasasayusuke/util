<template>
  <CommonSort class="is-tall" @sort="$emit('sort')" @clear="$emit('clear')">
    <ProjectSortInput
      :param="param"
      :suggest-customers="suggestCustomers"
      :suggest-sales-users="suggestSalesUsers"
      :supporter-organizations="supporterOrganizations"
      :service-types="serviceTypes"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ProjectSortInput from '~/components/project/molecules/ProjectSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'

export class ProjectSearchParam {
  status = 'during'
  fromYearMonth = format(getCurrentDate(), 'yyyy/MM')
  toYearMonth = ''
  customerId = ''
  mainSalesUserId = ''
  supporterOrganizationId = ''
  serviceTypeId = ''
}

export interface IProjectSort {
  customerName: string
  userName: string
  serviceType: string
  supporterOrganizations: string
  phase: string
  supportDateFrom: string
  supportDateTo: string
}

export default BaseComponent.extend({
  components: {
    CommonSort,
    ProjectSortInput,
  },
  props: {
    /** 検索条件 */
    param: {
      type: Object,
      required: true,
    },
    /** 取引先のサジェスト用顧客情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** サービス名の選択肢情報 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 課の選択肢情報 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Object,
      required: true,
    },
  },
})
</script>
