<template>
  <ScheduleTable
    :headers="surveyHeaders"
    :project-id="projectId"
    :items="formattedSchedules"
    :is-loading="isLoading"
    :is-edit-delete-display="includeExceptOfManHourOps"
    type="survey"
    is-hide-pagination
    link-prefix="/karte"
    class="pa-6"
    @click:edit="$emit('click:edit', $event)"
    @click:delete="$emit('click:delete', $event)"
    @click:resend="$emit('click:resend', $event)"
  >
  </ScheduleTable>
</template>

<script lang="ts">
import { isBefore } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ScheduleTable from '~/components/project/organisms/ScheduleTable.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SurveyProjectSchedule } from '~/models/Schedule'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { hasRole } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  components: {
    ScheduleTable,
  },
  props: {
    /** アンケート送信スケジュール情報 */
    surveySchedules: {
      type: Array as PropType<SurveyProjectSchedule[]>,
      required: true,
    },
    /** 選択中の案件ID */
    projectId: {
      type: String,
      required: true,
    },
    /** 読み込み中か */
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
            'project.pages.detail.surveySchedules.table.surveyType'
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
    /**
     * 表示用のフォーマットに変換したアンケート送信スケジュール情報を返す
     * @returns フォーマット済みアンケート送信スケジュール情報配列
     */
    formattedSchedules(): any {
      const rtn = this.surveySchedules.map((surveySchedule: any) => {
        const copiedSurveySchedules = Object.assign({}, surveySchedule, {
          completed: false,
          surveyName: '',
          resendSurveyName: '',
        })
        //ステータス
        copiedSurveySchedules.completed = this.checkStatus(
          copiedSurveySchedules.sendDate
        )
        //アンケート名
        const yearMonth = String(copiedSurveySchedules.sendDate).split('/')
        const year = yearMonth[0]
        const month = yearMonth[1]
        //sendDateを〇年〇月に変換
        const yearMonthJa = this.$t('common.format.yearMonth', {
          year: String(year),
          month: String(month).replace(/^0/, ''),
        }) as string
        //sendDateを〇月に変換
        const monthJa = this.$t('common.format.month', {
          month: String(month).replace(/^0/, ''),
        }) as string
        copiedSurveySchedules.surveyName =
          yearMonthJa +
          ' ' +
          this.$t(
            'project.pages.detail.surveySchedules.table.surveyNameList.' +
              surveySchedule.surveyName
          )
        copiedSurveySchedules.resendSurveyName =
          monthJa +
          ' ' +
          this.$t(
            'project.pages.detail.surveySchedules.table.surveyNameList.' +
              surveySchedule.surveyName
          )
        return copiedSurveySchedules
      })
      return rtn
    },
    /**
     * 営業担当者・営業責任者・アンケート事務局・システム管理者・事業者責任者のいずれかであるか判定
     * @returns 営業担当者・営業責任者・アンケート事務局・システム管理者・事業者責任者のいずれかであるかの真偽値
     */
    includeExceptOfManHourOps() {
      return hasRole([
        ENUM_ADMIN_ROLE.SALES,
        ENUM_ADMIN_ROLE.SALES_MGR,
        ENUM_ADMIN_ROLE.SURVEY_OPS,
        ENUM_ADMIN_ROLE.SYSTEM_ADMIN,
        ENUM_ADMIN_ROLE.BUSINESS_MGR,
      ])
    },
  },
  methods: {
    /**
     * 支援日時からステータスを返す
     * @returns ステータス真偽値
     */
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
