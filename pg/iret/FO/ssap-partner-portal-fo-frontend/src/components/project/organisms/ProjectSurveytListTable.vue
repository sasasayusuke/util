<template>
  <ScheduleTable
    :headers="surveyHeaders"
    :items="formattedSchedules"
    :is-loading="isLoading"
    :edit-and-delete="
      isNotSupporter &&
      isNotSupporter_mgr &&
      isNotSales_mgr &&
      isNotBusinessMgr &&
      isBelongSales
    "
    type="survey"
    is-hide-pagination
    link-prefix="/karte"
    class="pa-6"
    @click:edit="$emit('click:edit', $event)"
    @click:delete="$emit('click:delete', $event)"
  >
  </ScheduleTable>
</template>

<script lang="ts">
import { isBefore } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ScheduleTable from '~/components/project/organisms/ScheduleTable.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SurveyProjectSchedule } from '~/models/Schedule'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  name: 'ProjectSurveyListTable',
  components: {
    ScheduleTable,
  },
  props: {
    /** アンケートスケジュール */
    surveySchedules: {
      type: Array as PropType<SurveyProjectSchedule[]>,
      required: true,
    },
    /** ローディング中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      surveyHeaders: [
        {
          text: this.$t('project.pages.detail.surveySchedules.table.status'),
          align: 'start',
          value: 'status',
          sortable: false,
          width: '100px',
        },
        {
          text: this.$t(
            'project.pages.detail.surveySchedules.table.surveyName'
          ),
          value: 'surveyName',
          sortable: false,
          width: '300px',
        },
        {
          text: this.$t('project.pages.detail.surveySchedules.table.day'),
          value: 'sendDate',
          sortable: false,
        },
        {
          text: this.$t(
            'project.pages.detail.surveySchedules.table.surveyLimitDate'
          ),
          value: 'surveyLimitDate',
          sortable: false,
        },
        { text: '', value: 'actions', align: 'end', sortable: false },
      ],
    }
  },
  computed: {
    formattedSchedules(): any {
      const rtn = this.surveySchedules.map((surveySchedule: any) => {
        const copiedSurveySchedules = Object.assign({}, surveySchedule, {
          completed: false,
          surveyName: '',
        })
        //ステータス
        copiedSurveySchedules.completed = this.checkStatus(
          copiedSurveySchedules.sendDate
        )
        //アンケート名
        const yearMonth = String(copiedSurveySchedules.sendDate).split('/')
        const year = yearMonth[0]
        const month = yearMonth[1].replace(/^0/, '')
        //sendDateを〇年〇月に変換
        const yearMonthJa = this.$t('common.format.yearMonth', {
          year: String(year),
          month: String(month),
        }) as string
        copiedSurveySchedules.surveyName =
          yearMonthJa +
          ' ' +
          this.$t(
            'survey.group_info.surveyNameList.' + surveySchedule.surveyName
          )
        return copiedSurveySchedules
      })
      return rtn
    },
    /**
     * @return boolean ログインユーザーが支援者ではないか
     */
    isNotSupporter() {
      return meStore.role !== ENUM_USER_ROLE.SUPPORTER
    },
    /**
     * @return boolean ログインユーザーが支援者責任者ではないか
     */
    isNotSupporter_mgr() {
      return meStore.role !== ENUM_USER_ROLE.SUPPORTER_MGR
    },
    /**
     * @return boolean ログインユーザーが営業責任者ではないか
     */
    isNotSales_mgr() {
      return meStore.role !== ENUM_USER_ROLE.SALES_MGR
    },
    /**
     * @return boolean ログインユーザーが事業者責任者ではないか
     */
    isNotBusinessMgr() {
      return meStore.role !== ENUM_USER_ROLE.BUSINESS_MGR
    },
    /**
     * @return boolean ログインユーザーが案件に所属している営業担当者か
     */
    isBelongSales() {
      if (meStore.projectIds) {
        return meStore.projectIds.includes(this.$route.params.projectId)
      } else {
        return false
      }
    },
  },
  methods: {
    /** スケジュール送信日が今日より前か */
    checkStatus(date: string) {
      const sendDate = new Date(date)
      const today = getCurrentDate()
      if (isBefore(sendDate, today)) {
        return true
      } else {
        return false
      }
    },
  },
})
</script>
