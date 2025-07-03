<template>
  <ScheduleContainer
    :title="title"
    :is-all-edit="isAllEdit"
    :is-all-create="isAllCreate"
    :is-hide-button2="!includeExceptOfManHourOps"
    @click:bulkCreate="$emit('click:bulkCreate')"
    @click:bulkEdit="$emit('click:bulkEdit')"
    @click:add="$emit('click:add')"
  >
    <ProjectSurveyListTable
      :survey-schedules="surveySchedules.projectSchedules"
      :project-id="projectId"
      :is-loading="isLoading"
      @click:edit="edit"
      @click:delete="deleteSchedule"
      @click:resend="resend"
    />
    <!-- 一括編集モーダル -->
    <template v-if="isSurveyBulkEditModalOpen">
      <ProjectSurveyBulkEditModal
        :title="$t('project.pages.edit.survey.title.bulkEdit')"
        :survey-schedules="surveySchedules.projectSchedules"
        :survey-masters="surveyMasters"
        @click:save="saveBulkEdit"
        @click:closeBulkEdit="$emit('click:closeBulkEdit')"
      />
    </template>
    <!-- 一括登録モーダル -->
    <template v-if="isSurveyBulkCreateModalOpen">
      <ProjectSurveyBulkCreateModal
        :title="$t('project.pages.edit.survey.title.bulkCreate')"
        :survey-schedules="surveySchedules.projectSchedules"
        :survey-masters="surveyMasters"
        @click:save="saveBulkCreate"
        @click:closeBulkCreate="$emit('click:closeBulkCreate')"
      />
    </template>
    <!-- 追加モーダル -->
    <template v-if="isSurveyAddModalOpen">
      <ProjectSurveyAddModal
        :title="$t('project.pages.edit.survey.title.add')"
        :survey-masters="surveyMasters"
        :local-add-survey-schedule="localAddSurveySchedule"
        :support-date-to="supportDateTo"
        :support-date-from="supportDateFrom"
        @click:closeAdd="$emit('click:closeAdd')"
        @click:save="create"
      />
    </template>
    <!-- 編集モーダル -->
    <template v-if="isSurveyEditModalOpen">
      <ProjectSurveyEditModal
        :modal-schedule="modalSchedule"
        :support-date-to="supportDateTo"
        :support-date-from="supportDateFrom"
        :title="$t('project.pages.edit.survey.title.edit')"
        @click:closeEdit="$emit('click:closeEdit')"
        @refresh="$emit('refresh')"
      />
    </template>
    <!-- 削除モーダル -->
    <template v-if="isSurveyDeleteModalOpen">
      <ProjectSurveyDeleteModal
        :modal-schedule="modalSchedule"
        :title="$t('project.pages.edit.survey.title.delete')"
        @click:closeDelete="$emit('click:closeDelete')"
        @refresh="$emit('refresh')"
      />
    </template>
    <!-- 再送信モーダル -->
    <template v-if="isSurveyResendModalOpen">
      <ProjectSurveyResendModal
        :modal-schedule="modalSchedule"
        :title="$t('project.pages.edit.survey.title.resend')"
        @click:closeResend="$emit('click:closeResend')"
        @refresh="$emit('refresh')"
      />
    </template>
  </ScheduleContainer>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import ProjectSurveyListTable from '~/components/project/organisms/ProjectSurveytListTable.vue'
import ScheduleContainer from '~/components/common/organisms/ScheduleContainer.vue'
import ProjectSurveyBulkEditModal from '~/components/project/molecules/ProjectSurveyBulkEditModal.vue'
import ProjectSurveyBulkCreateModal from '~/components/project/molecules/ProjectSurveyBulkCreateModal.vue'
import ProjectSurveyAddModal from '~/components/project/molecules/ProjectSurveyAddModal.vue'
import ProjectSurveyEditModal from '~/components/project/molecules/ProjectSurveyEditModal.vue'
import ProjectSurveyDeleteModal from '~/components/project/molecules/ProjectSurveyDeleteModal.vue'
import ProjectSurveyResendModal from '~/components/project/molecules/ProjectSurveyResendModal.vue'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_ADMIN_ROLE } from '@/types/Admin'

import { GetProjectById } from '~/models/Project'
import {
  CreateSurveySchedulesRequest as localAddSurveySchedule,
  CreateSurveySchedules,
  GetSurveySchedulesByIdResponse,
  BulkCreateSurveySchedules,
  BulkUpdateSurveySchedules,
  BulkUpdateSurveySchedulesRequest,
  BulkCreateSurveySchedulesRequest,
} from '~/models/Schedule'
import { SurveyMasterListItem } from '~/models/Master'

export { localAddSurveySchedule }

export default BaseComponent.extend({
  components: {
    ScheduleContainer,
    ProjectSurveyListTable,
    ProjectSurveyBulkEditModal,
    ProjectSurveyBulkCreateModal,
    ProjectSurveyAddModal,
    ProjectSurveyEditModal,
    ProjectSurveyDeleteModal,
    ProjectSurveyResendModal,
  },
  props: {
    title: {
      type: String,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール一括編集モーダルが開かれているか */
    isSurveyBulkEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール一括追加モーダルが開かれているか */
    isSurveyBulkCreateModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール追加モーダルが開かれているか */
    isSurveyAddModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール編集モーダルが開かれているか */
    isSurveyEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール削除モーダルが開かれているか */
    isSurveyDeleteModalOpen: {
      type: Boolean,
      required: true,
    },
    /** 匿名アンケート再送信モーダルが開かれているか */
    isSurveyResendModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート送信スケジュール情報 */
    surveySchedules: {
      type: Object as PropType<GetSurveySchedulesByIdResponse>,
      required: true,
    },
    /** アンケートタイプ選択肢情報 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterListItem[]>,
      required: true,
    },
    /** 選択中の案件情報の支援開始日 */
    supportDateTo: {
      type: String,
      required: true,
    },
    /** 選択中の案件情報の支援終了日 */
    supportDateFrom: {
      type: String,
      required: true,
    },
  },
  watch: {
    isSurveyAddModalOpen(newVal) {
      if (newVal === true) {
        this.localAddSurveySchedule.surveyType = ''
        this.localAddSurveySchedule.timing = 'monthly'
        this.localAddSurveySchedule.requestDate = 1
        this.localAddSurveySchedule.limitDate = 5
      }
    },
  },
  data(): {
    localAddSurveySchedule: localAddSurveySchedule
    projectId: string
    modalSchedule: object
  } {
    return {
      projectId: this.$route.params.projectId,
      localAddSurveySchedule: {
        surveyMasterId: '',
        surveyType: '',
        timing: 'monthly',
        requestDate: 1,
        limitDate: 5,
      },
      modalSchedule: {},
    }
  },
  methods: {
    /**
     * アンケート送信スケジュール編集モーダルを開く
     * @param item 選択されたアンケート送信スケジュール
     */
    edit(item: any) {
      this.modalSchedule = item
      this.$emit('click:edit')
    },
    /**
     * アンケート送信スケジュール削除モーダルを開く
     * @param item 選択されたアンケート送信スケジュール
     */
    deleteSchedule(item: any) {
      this.modalSchedule = item
      this.$emit('click:delete')
    },
    resend(item: any) {
      this.modalSchedule = item
      this.$emit('click:resend')
    },
    /** アンケート送信スケジュールを新規作成 */
    async create() {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const res = await GetProjectById(id)

      if (
        res.data.dedicatedSurveyUserEmail === '' &&
        res.data.mainCustomerUserId === ''
      ) {
        this.showErrorBarWithScrollPageTop(
          this.$t('project.pages.create.customer.errors.requiredCustomerName')
        )
        this.$emit('click:closeAdd')
      } else {
        // surveyIdがマウントされて渡ってくるのでfindメソッドでsurveyを特定する
        const surveyId = this.localAddSurveySchedule.surveyType
        const findSurvey = this.surveyMasters.find(
          (val: SurveyMasterListItem) => val.id === surveyId
        )
        this.localAddSurveySchedule.surveyType = findSurvey!.type
        this.localAddSurveySchedule.surveyMasterId = findSurvey!.id
        const request = this.localAddSurveySchedule
        const detailPage = '/project/' + id

        CreateSurveySchedules(id, request)
          .then(() => {
            this.$emit('click:closeAdd')
            this.$emit('getSurveySchedulesByIdResponse')
            this.$router.push(detailPage)
            this.$emit('refresh')
          })
          .catch((error) => {
            if (
              error.response.data.detail ===
              'You cannot set a send date that is earlier than the current date.'
            ) {
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.survey.errorMessage')
              )
            } else {
              this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
            }
            this.$emit('click:closeAdd')
            this.$emit('getSurveySchedulesByIdResponse')
            this.$router.push(detailPage)
          })
      }
    },
    /**
     * 入力したアンケート送信スケジュール一括作成情報を保存
     * @param elm 入力したアンケート送信スケジュール一括作成情報
     */
    saveBulkCreate(elm: BulkCreateSurveySchedulesRequest) {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const request = elm

      BulkCreateSurveySchedules(id, request)
        .then(() => {
          this.$emit('click:closeBulkCreate')
          this.$router.push(detailPage)
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeBulkCreate')
          this.$router.push(detailPage)
        })
    },
    /**
     * 入力したアンケート送信スケジュール一括編集情報を保存
     * @param elm 入力したアンケート送信スケジュールを一括編集情報
     */
    saveBulkEdit(elm: BulkUpdateSurveySchedulesRequest) {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const request = elm

      BulkUpdateSurveySchedules(id, request)
        .then(() => {
          this.$emit('click:closeBulkEdit')
          this.$router.push(detailPage)
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeBulkEdit')
          this.$router.push(detailPage)
        })
    },
  },
  computed: {
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
    /**
     * 一括追加ボタンを表示するか判定
     * @returns 一括追加ボタンを表示するかの真偽値
     */
    isAllCreate(): boolean {
      if (!this.includeExceptOfManHourOps) {
        return false
      }
      if (
        this.surveySchedules.projectSchedules &&
        this.surveySchedules.projectSchedules.length > 0
      ) {
        return false
      }
      if (this.surveySchedules.surveyGroupId) {
        return false
      }
      return true
    },
    /**
     * 一括編集ボタンを表示するか判定
     * @returns 一括編集ボタンを表示するかの真偽値
     */
    isAllEdit(): boolean {
      if (!this.includeExceptOfManHourOps) {
        return false
      }
      if (
        !this.surveySchedules.projectSchedules ||
        this.surveySchedules.projectSchedules.length === 0
      ) {
        return false
      }
      if (!this.surveySchedules.surveyGroupId) {
        return false
      }
      return true
    },
  },
})
</script>
