<template>
  <ScheduleContainer
    :title="title"
    :is-all-edit="isAllEdit"
    :is-all-create="isAllCreate"
    :is-hide-button1="isHideButton1"
    :is-hide-button2="
      isSupporter ||
      isSupporterMgr ||
      isSalesMgr ||
      isBusinessMgr ||
      isNotBelongSales
    "
    @click:bulkCreate="$emit('click:bulkCreate')"
    @click:bulkEdit="$emit('click:bulkEdit')"
    @click:add="$emit('click:add')"
  >
    <ProjectSurveyListTable
      :survey-schedules="surveySchedules.projectSchedules"
      :is-loading="isLoading"
      @click:edit="edit"
      @click:delete="deleteSchedule"
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
        :title="$t('project.pages.edit.survey.title.edit')"
        :support-date-to="supportDateTo"
        :support-date-from="supportDateFrom"
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
import {
  CreateSurveySchedulesRequest as localAddSurveySchedule,
  CreateSurveySchedules,
  GetSurveySchedulesByIdResponse,
  BulkCreateSurveySchedules,
  BulkUpdateSurveySchedules,
  BulkUpdateSurveySchedulesRequest,
  BulkCreateSurveySchedulesRequest,
} from '~/models/Schedule'
import { GetProjectById } from '~/models/Project'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { SurveyMasterItem } from '~/models/Master'
export { localAddSurveySchedule }

export default BaseComponent.extend({
  name: 'ProjectSurveyContainer',
  components: {
    ScheduleContainer,
    ProjectSurveyListTable,
    ProjectSurveyBulkEditModal,
    ProjectSurveyBulkCreateModal,
    ProjectSurveyAddModal,
    ProjectSurveyEditModal,
    ProjectSurveyDeleteModal,
  },
  props: {
    title: {
      type: String,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** アンケート一括変更モーダルが開いているか */
    isSurveyBulkEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート一括作成モーダルが開いているか */
    isSurveyBulkCreateModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート追加モーダルが開いているか */
    isSurveyAddModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート変更モーダルが開いているか */
    isSurveyEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** アンケート削除モーダルが開いているか */
    isSurveyDeleteModalOpen: {
      type: Boolean,
      required: true,
    },
    isHideButton1: {
      type: Boolean,
      default: true,
    },
    /** アンケートスケジュール一覧 */
    surveySchedules: {
      type: Object as PropType<GetSurveySchedulesByIdResponse>,
      required: true,
    },
    /** アンケート雛形一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterItem[]>,
      required: true,
    },
    /** 支援期間終了日 */
    supportDateTo: {
      type: String,
      required: true,
    },
    /** 支援期間開始日 */
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
    modalSchedule: object
  } {
    return {
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
    /** アンケート送信日編集モーダル保存クリック */
    edit(item: any) {
      this.modalSchedule = item
      this.$emit('click:edit')
    },
    /** アンケート送信日削除モーダル個別予定の削除クリック */
    deleteSchedule(item: any) {
      this.modalSchedule = item
      this.$emit('click:delete')
    },
    /** アンケート送信スケジュールを新規作成 */
    async create() {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const res = await GetProjectById(id)

      // メールを配信するユーザーが指定されていない場合はモーダルを閉じてエラー表示
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
          (val: SurveyMasterItem) => val.id === surveyId
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
    /** アンケート送信日一括作成モーダル保存クリック */
    saveBulkCreate(elm: BulkCreateSurveySchedulesRequest) {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const request = elm

      BulkCreateSurveySchedules(id, request)
        .then((res) => {
          this.$logger.info(res.data.message)
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
    /** アンケート送信日一括変更モーダル保存クリック */
    saveBulkEdit(elm: BulkUpdateSurveySchedulesRequest) {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const detailPage = '/project/' + id
      const request = elm

      BulkUpdateSurveySchedules(id, request)
        .then((res) => {
          this.$logger.info(res.data.message)
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
    isAllCreate(): boolean {
      if (
        this.isSupporter ||
        this.isSupporterMgr ||
        this.isSalesMgr ||
        this.isBusinessMgr ||
        this.isNotBelongSales
      ) {
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
    isAllEdit(): boolean {
      if (
        this.isSupporter ||
        this.isSupporterMgr ||
        this.isSalesMgr ||
        this.isBusinessMgr ||
        this.isNotBelongSales
      ) {
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
    /** ログインユーザーが支援者か */
    isSupporter() {
      return meStore.role === ENUM_USER_ROLE.SUPPORTER
    },
    /** ログインユーザーが支援者責任者か */
    isSupporterMgr() {
      return meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR
    },
    /** ログインユーザーが営業責任者か */
    isSalesMgr() {
      return meStore.role === ENUM_USER_ROLE.SALES_MGR
    },
    /** ログインユーザーが事業者責任者か */
    isBusinessMgr() {
      return meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
    },
    /** ログインユーザーが案件に所属していない営業担当者か */
    isNotBelongSales() {
      if (meStore.projectIds) {
        return !meStore.projectIds.includes(this.$route.params.projectId)
      } else {
        return true
      }
    },
  },
})
</script>
