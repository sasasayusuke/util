<template>
  <CommonSort :title="sortTitle" @sort="sort" @clear="clear">
    <AdminSurveySortInput
      v-if="type === 1"
      :from="from"
      :to="to"
      @change="onChange"
    />
    <SurveySortInput
      v-else
      :survey-type="surveyType"
      :from="from"
      :to="to"
      :enb-from-to="enbFromTo"
      @change="onChange"
    />
  </CommonSort>
</template>

<script lang="ts">
import SurveySortInput from '../molecules/SurveySortInput.vue'
import AdminSurveySortInput from '../molecules/AdminSurveySortInput.vue'
import PPSurveySortInput from '../molecules/PPSurveySortInput.vue'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export default BaseComponent.extend({
  components: {
    CommonSort,
    SurveySortInput,
    AdminSurveySortInput,
    PPSurveySortInput,
  },
  props: {
    type: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      surveyType: '',
      from: (getCurrentDate().toISOString().substr(0, 7) + '-01').replaceAll(
        '-',
        '/'
      ),
      to: '',
      enbFromTo: false,
      supporterRange: this.type === 1 ? 1 : 0,
    }
  },
  computed: {
    /**
     * 表示期間の表示の有無
     */
    sortTitle(): any {
      return this.type === 1
        ? this.$t('survey.pages.admin.list.sort_input.display_period')
        : ''
    },
  },
  methods: {
    /**
     * 案件アンケート並び替え
     */
    sort() {
      const params = {
        surveyType: this.surveyType,
        enbFromTo: this.enbFromTo,
        from: this.from,
        to: this.to,
        supporterRange: this.supporterRange,
      }
      this.$emit('sort', params)
    },
    /**
     * 並び替え条件
     */
    clear() {
      this.surveyType = ''
      this.from = (
        getCurrentDate().toISOString().substr(0, 7) + '-01'
      ).replaceAll('-', '/')
      this.to = ''
      this.enbFromTo = false
      this.supporterRange = this.type === 1 ? 1 : 0
      this.$emit('clear')
    },
    /**
     * 並び替え条件送信
     * @param localData パラメーター
     */
    onChange(localData: any) {
      if (this.type === 1) {
        this.supporterRange = localData.supporterRange
      }
      this.surveyType = localData.surveyType
      this.from = localData.from
      this.to = localData.to
      this.enbFromTo = localData.enbFromTo
      this.$emit('update', 'type', localData.surveyType)
      this.$emit('update', 'enbFromTo', localData.enbFromTo)
      this.$emit('update', 'dateFrom', localData.from)
      this.$emit('update', 'dateTo', localData.to)
    },
  },
})
</script>
