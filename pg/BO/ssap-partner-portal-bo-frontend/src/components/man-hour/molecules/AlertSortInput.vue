<template>
  <Sheet class="d-flex align-center pr-7">
    <!-- 集計月指定 -->
    <p class="font-size-small mb-0 mr-2">
      {{ $t('man-hour.pages.alert.list.sort_input.date') }}
      <Required />
    </p>
    <Sheet width="100" mr-7>
      <MonthSelect
        v-model="param.yearMonth"
        :is-no-icon="true"
        style-set="bgWhite"
        @change="update('yearMonth', $event)"
      />
    </Sheet>
    <!-- 課 -->
    <p class="font-size-small mb-0 mr-2 ml-7">
      {{ $t('man-hour.pages.alert.list.sort_input.supporterOrganizationName') }}
    </p>
    <Sheet width="240">
      <Select
        v-model="param.supporterOrganizationId"
        dense
        outlined
        :items="filteredSupporterOrganizations"
        item-text="text"
        item-value="value"
        style-set="bgWhite"
        placeholder="選択してください（複数可）"
        multiple
        @change="update('supporterOrganizationId', $event)"
      />
    </Sheet>
    <!-- サービス名 -->
    <p class="font-size-small mb-0 mr-2 ml-7">
      {{ $t('man-hour.pages.alert.list.sort_input.serviceTypeId') }}
    </p>
    <Sheet width="240">
      <Select
        v-model="param.serviceTypeId"
        dense
        outlined
        :items="filteredServiceTypes"
        item-text="text"
        item-value="value"
        style-set="bgWhite"
        placeholder="選択してください（複数可）"
        multiple
        @change="update('serviceTypeId', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { TextField, Select, Sheet, Required } from '~/components/common/atoms'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'

export default BaseComponent.extend({
  name: 'AlertSortInput',
  components: {
    TextField,
    Sheet,
    Select,
    MonthSelect,
    Required,
  },
  props: {
    /** 検索パラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** サービス種別一覧情報 */
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
  computed: {
    /**
     * サービス種別の一覧情報を変換
     * @param serviceTypes  サービス種別一覧情報
     * @returns text:サービス種別名,value:サービス種別ID に変換されたサービス種別の一覧情報
     */
    filteredServiceTypes(): any {
      if (this.serviceTypes.length) {
        return this.serviceTypes.map((item: any) => {
          return { text: item.name, value: item.id }
        })
      } else {
        return []
      }
    },
    /**
     * 支援者組織一覧情報を変換
     * @param supporterOrganizations  支援者組織一覧情報
     * @returns text:支援者組織名,value:支援者組織ID に変換された支援者組織一覧情報
     */
    filteredSupporterOrganizations(): any {
      if (this.supporterOrganizations.length) {
        return this.supporterOrganizations.map((item: any) => {
          return { text: item.name, value: item.id }
        })
      } else {
        return []
      }
    },
  },
  methods: {
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>
