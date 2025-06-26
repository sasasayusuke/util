<template>
  <CommonSort @sort="sort" @clear="clear">
    <ManHourSortInput
      :param="param"
      :supporter-organizations="supporterOrganizations"
      :service-types="serviceTypes"
      :only-organization="onlyOrganization"
      :is-loading="isLoading"
      @update="update"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ManHourSortInput from '~/components/man-hour/molecules/ManHourSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class SearchParam {
  yearMonth: string = format(getCurrentDate(), 'yyyy/MM')
  supporterOrganizationId = []
  serviceTypeId = []
}

export class SummarySupporterOrganizationsManHoursSearchParam {
  public date = format(getCurrentDate(), 'yyyy/MM/')
  public supporterOrganizationId = ''
}

export default BaseComponent.extend({
  name: 'ManHourSort',
  components: {
    CommonSort,
    ManHourSortInput,
  },
  props: {
    /** 検索パラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** 支援者組織一覧情報 */
    supporterOrganizations: {
      type: Array,
    },
    /** サービス種別の一覧情報 */
    serviceTypes: {
      type: Array,
    },
    /** ソートで課のみ指定 */
    onlyOrganization: {
      type: Boolean,
      default: false,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  methods: {
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
    sort() {
      this.$emit('sort')
    },
    clear() {
      this.$emit('clear')
    },
  },
})
</script>
