<template>
  <ScheduleTable
    :headers="supportHeaders"
    :items="formattedSchedules"
    :is-loading="isLoading"
    :project-id="projectId"
    :edit-and-delete="editAndDelete"
    type="support"
    is-hide-pagination
    link-prefix="/karte"
    class="pa-6"
    @click:edit="$emit('click:edit', $event)"
    @click:delete="$emit('click:delete', $event)"
  >
  </ScheduleTable>
</template>

<script lang="ts">
import { isBefore, isAfter } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ScheduleTable from '~/components/project/organisms/ScheduleTable.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SupportProjectSchedule } from '~/models/Schedule'

export default BaseComponent.extend({
  name: 'ProjectSupportListTable',
  components: {
    ScheduleTable,
  },
  props: {
    /** 支援スケジュール */
    supportSchedules: {
      type: Array as PropType<SupportProjectSchedule[]>,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 案件ID */
    projectId: {
      type: String,
      required: true,
    },
    editAndDelete: {
      type: Boolean,
    },
  },
  data() {
    return {
      supportHeaders: [
        {
          text: this.$t('project.pages.detail.supportSchedules.table.status'),
          align: 'start',
          value: 'status',
          sortable: false,
          width: '100px',
        },
        {
          text: this.$t('project.pages.detail.supportSchedules.table.day'),
          value: 'supportDate',
          sortable: false,
          width: '300px',
        },
        {
          text: this.$t('project.pages.detail.supportSchedules.table.karte'),
          value: 'karteId',
          sortable: false,
        },
        {
          text: this.$t(
            'project.pages.detail.supportSchedules.table.lastUpdate'
          ),
          value: 'lastUpdateDatetime',
          sortable: false,
        },
        {
          text: this.$t(
            'project.pages.detail.supportSchedules.table.updateUser'
          ),
          value: 'updateUser',
          sortable: false,
        },
        { text: '', value: 'actions', align: 'end', sortable: false },
      ],
    }
  },
  computed: {
    formattedSchedules(): any {
      const rtn = this.supportSchedules.map(
        (supportSchedule: SupportProjectSchedule) => {
          const copiedSurveySchedules = Object.assign({}, supportSchedule, {
            status: 'plan',
          })

          //ステータス
          copiedSurveySchedules.status = this.checkStatus(
            copiedSurveySchedules.supportDate +
              ' ' +
              copiedSurveySchedules.supportStartTime,
            copiedSurveySchedules.supportDate +
              ' ' +
              copiedSurveySchedules.supportEndTime
          )
          //支援日
          copiedSurveySchedules.supportDate =
            copiedSurveySchedules.supportDate +
            ' ' +
            copiedSurveySchedules.supportStartTime +
            '～' +
            copiedSurveySchedules.supportEndTime

          // 最終更新
          if (copiedSurveySchedules.lastUpdateDatetime) {
            copiedSurveySchedules.lastUpdateDatetime = this.formatDate(
              new Date(copiedSurveySchedules.lastUpdateDatetime),
              'Y/MM/DD'
            )
          } else {
            copiedSurveySchedules.lastUpdateDatetime = 'ー'
          }

          // 最終更新者
          if (!copiedSurveySchedules.updateUser) {
            copiedSurveySchedules.updateUser = 'ー'
          }

          return copiedSurveySchedules
        }
      )
      rtn.sort((a, b) => b.supportDate.localeCompare(a.supportDate))
      return rtn
    },
  },
  methods: {
    /** スケジュール送信日が今日より前か */
    checkStatus(startDate: string, endDate: string) {
      const supportStartDate = new Date(startDate)
      const supportEndDate = new Date(endDate)
      const today = getCurrentDate()
      if (
        !isBefore(today, supportStartDate) &&
        !isAfter(today, supportEndDate)
      ) {
        return 'progress'
      } else if (isBefore(supportEndDate, today)) {
        return 'performed'
      } else {
        return 'plan'
      }
    },
  },
})
</script>
