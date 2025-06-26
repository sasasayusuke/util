<template>
  <RootTemplate>
    <template v-if="!isEditing">
      <DetailInPageHeader :back-to-list="toProjectList">
        {{ $t('project.pages.detail.pageName') }}
      </DetailInPageHeader>
      <CommonAnchor :items="anchorList" />
    </template>
    <ProjectDetailContainer
      :title="pageName()"
      :project="project"
      :service-types="serviceTypes"
      :suggest-users="suggestUsers"
      :suggest-sales-users="suggestSalesUsers"
      :suggest-customers="suggestCustomers"
      :suggest-supporter-users="suggestSupporterUsers"
      :suggest-main-supporter-users="suggestMainSupporterUsers"
      :supporter-organizations="supporterOrganizations.supporterOrganizations"
      :is-editing="isEditing"
      :is-loading="isLoadingItems"
      :is-loading-button="isLoadingButton"
      note-head="required"
      @click:positive="update"
      @click:negative="onClickNegative"
      @validWithChange="isValidWithChange = $event"
    >
      <template v-if="!isEditing" #footerButton>
        <Button
          style-set="large-primary"
          width="160px"
          @click="onClickPositive"
        >
          {{ $t('common.button.edit2') }}
        </Button>
      </template>
      <template v-else #footerButton>
        <Button
          style-set="large-tertiary"
          width="160px"
          outlined
          @click="onClickNegative"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="large-primary"
          width="160px"
          class="ml-2"
          :disabled="!isValidWithChange"
          @click="onClickPositive"
        >
          {{ $t('common.button.save2') }}
        </Button>
      </template>
    </ProjectDetailContainer>
    <template v-if="!isEditing">
      <ProjectSurveyContainer
        id="surveySchedules"
        :title="$t('project.pages.detail.surveySchedules.title')"
        :is-editing="isEditing"
        :is-survey-bulk-edit-modal-open="isSurveyBulkEditModalOpen"
        :is-survey-bulk-create-modal-open="isSurveyBulkCreateModalOpen"
        :is-survey-add-modal-open="isSurveyAddModalOpen"
        :is-survey-edit-modal-open="isSurveyEditModalOpen"
        :is-survey-delete-modal-open="isSurveyDeleteModalOpen"
        :survey-schedules="projectSurveySchedules"
        :survey-masters="surveyMasters.masters"
        :is-loading="isLoadingItems.surveySchedules"
        :support-date-to="project.supportDateTo"
        :support-date-from="project.supportDateFrom"
        :project="project"
        class="mt-8"
        @click:bulkEdit="onClickSurveyBulkEdit"
        @click:closeBulkEdit="isSurveyBulkEditModalOpen = false"
        @click:bulkCreate="onClickSurveyBulkCreate"
        @click:closeBulkCreate="isSurveyBulkCreateModalOpen = false"
        @click:add="onClickSurveyAdd"
        @click:closeAdd="isSurveyAddModalOpen = false"
        @click:edit="onClickSurveyEdit"
        @click:closeEdit="isSurveyEditModalOpen = false"
        @click:delete="onClickSurveyDelete"
        @click:closeDelete="isSurveyDeleteModalOpen = false"
        @refresh="refresh"
      />
      <ProjectSupportContainer
        id="supportSchedules"
        :title="$t('project.pages.detail.supportSchedules.title')"
        :is-editing="isEditing"
        :is-support-add-modal-open="isSupportAddModalOpen"
        :is-support-edit-modal-open="isSupportEditModalOpen"
        :is-support-delete-modal-open="isSupportDeleteModalOpen"
        :project-schedules="projectSupportSchedules"
        :is-loading="isLoadingItems.supportSchedules"
        class="mt-8"
        @click:add="onClickSupportAdd"
        @click:closeAdd="isSupportAddModalOpen = false"
        @click:edit="onClickSupportEdit"
        @click:closeEdit="isSupportEditModalOpen = false"
        @click:delete="onClickSupportDelete"
        @click:closeDelete="isSupportDeleteModalOpen = false"
        @refresh="refresh"
      />
      <ProjectFooter />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import CommonAnchor from '~/components/common/molecules/CommonAnchor.vue'
import ProjectDetailContainer, {
  LocalProject,
} from '~/components/project/organisms/ProjectDetailContainer.vue'
import ProjectSupportContainer from '~/components/project/organisms/ProjectSupportContainer.vue'
import ProjectSurveyContainer from '~/components/project/organisms/ProjectSurveyContainer.vue'
import ProjectFooter from '~/components/project/organisms/ProjectFooter.vue'
import ProjectSurveyBulkEditModal from '~/components/project/molecules/ProjectSurveyBulkEditModal.vue'
import { Button } from '~/components/common/atoms/index'
import { PropType } from '~/common/BaseComponent'
import { SuggestUsersResponse } from '~/models/User'
import {
  GetProjectByIdResponse,
  UpdateProjectById,
  UpdateProjectRequest,
} from '~/models/Project'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
  GetSurveyMastersResponse,
} from '~/models/Master'
import {
  GetSupportSchedulesByIdResponse,
  GetSurveySchedulesByIdResponse,
} from '~/models/Schedule'
import { masterKarteListUrlStore } from '~/store'
import { ENUM_UPDATE_PROJECT_RESPONSE_ERROR } from '~/types/Project'

export interface isLoading {
  project: boolean
  surveySchedules: boolean
  supportSchedules: boolean
  suggestUsers: boolean
  suggestSalesUsers: boolean
  suggestCustomers: boolean
  serviceTypes: boolean
  userById: boolean
  surveyMaster: boolean
}

export default CommonDetail.extend({
  name: 'ProjectDetailTemplate',
  components: {
    RootTemplate,
    DetailInPageHeader,
    CommonAnchor,
    ProjectDetailContainer,
    ProjectSupportContainer,
    ProjectSurveyContainer,
    ProjectFooter,
    ProjectSurveyBulkEditModal,
    Button,
  },
  props: {
    /** 選択した案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /** サービス種別 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 取引先のサジェスト用情報 */
    suggestUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用営業情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用顧客情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** 案件区分が「アンケート」の案件スケジュール */
    projectSurveySchedules: {
      type: Object as PropType<GetSurveySchedulesByIdResponse>,
      required: true,
    },
    /** 案件区分が「アンケート」の案件スケジュール */
    projectSupportSchedules: {
      type: Object as PropType<GetSupportSchedulesByIdResponse>,
      required: true,
    },
    /** 最新バージョンのアンケートひな形の一覧 */
    surveyMasters: {
      type: Object as PropType<GetSurveyMastersResponse>,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** ローディング中か */
    isLoadingItems: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  head() {
    /** 編集モードか */
    const isEditing = this.isEditing as boolean
    const edit = this.$t('project.pages.edit.name') as string
    const detail = this.$t('project.pages.detail.name') as string
    return {
      title: isEditing ? edit : detail,
    }
  },
  data() {
    return {
      id: '',
      /** 編集モードか */
      isEditing: false,
      isValid: false,
      isModalOpen: false,
      /** アンケート一括変更モーダルが開いているか */
      isSurveyBulkEditModalOpen: false,
      /** アンケート一括変作成モーダルが開いているか */
      isSurveyBulkCreateModalOpen: false,
      /** アンケート追加モーダルが開いているか */
      isSurveyAddModalOpen: false,
      /** アンケート編集モーダルが開いているか */
      isSurveyEditModalOpen: false,
      /** アンケート削除モーダルが開いているか */
      isSurveyDeleteModalOpen: false,
      /** 支援日スケジュール追加モーダルが開いているか */
      isSupportAddModalOpen: false,
      /** 支援日スケジュール編集モーダルが開いているか */
      isSupportEditModalOpen: false,
      /** 支援日スケジュール削除モーダルが開いているか */
      isSupportDeleteModalOpen: false,
      anchorList: [
        {
          text: this.$t('project.pages.detail.surveySchedules.title'),
          target: '#surveySchedules',
        },
        {
          text: this.$t('project.pages.detail.supportSchedules.title'),
          target: '#supportSchedules',
        },
      ],
      isValidWithChange: false,
      /** 取引先のサジェスト用情報 */
      suggestSupporterUsers: new SuggestUsersResponse(),
      /** 取引先のサジェスト用支援者責任者情報 */
      suggestMainSupporterUsers: new SuggestUsersResponse(),
      /** 案件更新中か否か */
      isLoadingButton: false,
    }
  },
  created() {
    this.id = this.$route.params.id
  },
  computed: {
    /**
     * 案件一覧へのリンク
     */
    toProjectList() {
      const masterKarteListParams = masterKarteListUrlStore.params
        ? new URLSearchParams(
            JSON.parse(masterKarteListUrlStore.params)
          ).toString()
        : ''
      return `/project/list?${masterKarteListParams}`
    },
  },
  methods: {
    pageName() {
      const isEditing = this.isEditing as boolean
      const edit = this.$t('project.pages.edit.name') as string
      const detail = this.$t('project.pages.detail.name') as string
      return isEditing ? edit : detail
    },
    /** 編集画面の情報をもとに案件情報を更新 */
    update(localProject: LocalProject) {
      this.clearErrorBar()
      if (this.isEditing) {
        this.isLoadingButton = true
        const request = new UpdateProjectRequest()
        Object.assign(request, localProject)
        if (!request.customerUsers) {
          request.customerUsers = []
        }
        if (!request.supporterUsers) {
          request.supporterUsers = []
        }

        const customerUsers = request.customerUsers
        const supporterUsers = request.supporterUsers
        request.customerUsers = []
        request.supporterUsers = []
        for (const i in customerUsers) {
          // @ts-ignore
          request.customerUsers.push({ id: customerUsers[i].id })
        }
        for (const i in supporterUsers) {
          // @ts-ignore
          request.supporterUsers.push({ id: supporterUsers[i].id })
        }
        const id = localProject.id
        const version = localProject.version
        UpdateProjectById(id, version, request)
          .then(this.updateThen)
          .catch((error) => {
            if (
              error?.response?.data?.detail ===
              ENUM_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINSALES
            ) {
              // 商談所有者
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.errors.disabledMainSales')
              )
            } else if (
              error?.response?.data?.detail ===
              ENUM_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINCUSTOMER
            ) {
              // お客様（代表）
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.errors.disabledMainCustomer')
              )
            } else if (
              error?.response?.data?.detail ===
              ENUM_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_CUSTOMER
            ) {
              // お客様
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.errors.disabledCustomer')
              )
            } else if (
              error?.response?.data?.detail ===
              ENUM_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINSUPPORTER
            ) {
              // プロデューサー
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.errors.disabledMainSupporter')
              )
            } else if (
              error?.response?.data?.detail ===
              ENUM_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_SUPPORTER
            ) {
              // アクセラレーター
              this.showErrorBarWithScrollPageTop(
                this.$t('project.pages.edit.errors.disabledSupporter')
              )
            } else {
              this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
            }
          })
          .finally(() => {
            this.$router.push(`${this.$route.params.projectId}`)
            this.$emit('refresh')
            this.isLoadingButton = false
          })
      } else {
        this.isEditing = !this.isEditing
      }
    },
    onClickPositive() {
      this.isEditing = !this.isEditing
    },
    onClickNegative() {
      this.clearErrorBar()
      if (this.isEditing) {
        this.isEditing = false
        this.$emit('refresh')
      } else {
        this.$router.push(`${this.$route.params.projectId}`)
      }
    },
    onClickSurveyBulkEdit() {
      this.isSurveyBulkEditModalOpen = true
    },
    onClickSurveyBulkCreate() {
      this.isSurveyBulkCreateModalOpen = true
    },
    onClickSurveyAdd() {
      this.isSurveyAddModalOpen = true
    },
    onClickSurveyEdit() {
      this.isSurveyEditModalOpen = true
    },
    onClickSurveyDelete() {
      this.isSurveyDeleteModalOpen = true
    },
    onClickDelete() {
      this.isModalOpen = true
    },
    onClickSupportAdd() {
      this.isSupportAddModalOpen = true
    },
    onClickSupportEdit() {
      this.isSupportEditModalOpen = true
    },
    onClickSupportDelete() {
      this.isSupportDeleteModalOpen = true
    },
  },
})
</script>
