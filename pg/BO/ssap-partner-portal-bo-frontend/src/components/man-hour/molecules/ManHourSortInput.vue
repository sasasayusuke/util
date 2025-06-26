<template>
  <Sheet class="m-project-sort-input">
    <Sheet
      v-if="onlyOrganization"
      class="m-project-sort-input__unit d-flex align-center pr-7"
    >
      <!-- 課 -->
      <p class="font-size-small mb-0 mr-2">
        {{ $t('project.pages.index.sortInput.division') }}
      </p>
      <Sheet width="240">
        <Select
          v-model="param.supporterOrganizationId"
          :items="supporterOrganizations"
          item-text="shortName"
          item-value="id"
          :placeholder="$t('common.placeholder.selectMulti')"
          style-set="outlined bgWhite"
          multiple
        />
      </Sheet>
    </Sheet>
    <Sheet v-else class="m-project-sort-input__unit d-flex align-center pr-7">
      <!-- 集計月指定 -->
      <p class="font-size-small mb-0 mr-2 ml-n1">
        {{ $t('man-hour.pages.index.monthSelect') }}
        <Required />
      </p>
      <Sheet class="mr-2" width="133">
        <DateSelect
          v-model="param.yearMonth"
          type="month"
          style-set="bgWhite"
          @change="update('yearMonth', $event)"
        />
      </Sheet>
      <!-- 課 -->
      <p class="font-size-small mb-0 mr-2 ml-6">
        {{ $t('project.pages.index.sortInput.division') }}
      </p>
      <Sheet width="225">
        <Select
          v-model="param.supporterOrganizationId"
          :items="supporterOrganizations"
          item-text="shortName"
          item-value="id"
          style-set="outlined bgWhite"
          :placeholder="$t('common.placeholder.selectMulti')"
          multiple
          @change="update('supporterOrganizationId', $event)"
        />
      </Sheet>
      <!-- サービス名 -->
      <p class="font-size-small mb-0 mr-2 ml-6">
        {{ $t('project.pages.index.sortInput.service') }}
      </p>
      <Sheet width="225">
        <Select
          v-model="param.serviceTypeId"
          :items="serviceTypes"
          item-text="name"
          item-value="id"
          style-set="outlined bgWhite"
          multiple
          :placeholder="$t('common.placeholder.selectMulti')"
          @change="update('serviceTypeId', $event)"
        />
      </Sheet>
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import {
  TextField,
  Select,
  AutoComplete,
  Sheet,
  Required,
} from '~/components/common/atoms'
import DateSelect from '~/components/common/molecules/MonthSelect.vue'

export default BaseComponent.extend({
  name: 'ManHourSortInput',
  components: {
    TextField,
    Select,
    AutoComplete,
    DateSelect,
    Sheet,
    Required,
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
      required: true,
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
  data() {
    return {
      selectedMonth: '',
    }
  },
  methods: {
    update(item: string, event: any) {
      this.$emit('update', item, event)
    },
  },
})
</script>

<style lang="scss" scoped>
.m-project-sort-input__unit {
  &:nth-child(2) {
    margin-left: -100px;
  }
}
</style>
