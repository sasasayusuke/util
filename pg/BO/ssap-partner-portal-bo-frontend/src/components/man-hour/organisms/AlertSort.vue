<template>
  <v-form ref="form" v-model="valid" lazy-validation>
    <CommonSort @sort="sort" @clear="clear">
      <AlertSortInput
        :supporter-organizations="supporterOrganizations"
        :service-types="serviceTypes"
        :param="param"
        @update="update"
      />
    </CommonSort>
  </v-form>
</template>

<script lang="ts">
import { format } from 'date-fns'
import AlertSortInput from '../molecules/AlertSortInput.vue'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'

export class SearchParam {
  yearMonth: string = format(getCurrentDate(), 'yyyy/MM')
  supporterOrganizationId = []
  serviceTypeId = []
}

export default BaseComponent.extend({
  name: 'AlertSort',
  components: {
    CommonSort,
    AlertSortInput,
    DateSelect,
  },
  props: {
    /** 検索パラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** サービス種別の一覧情報 */
    serviceTypes: {
      type: Array,
      default: [],
    },
    /** 支援者組織一覧情報 */
    supporterOrganizations: {
      type: Array,
      default: [],
    },
  },
  data() {
    return {
      valid: true,
    }
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
