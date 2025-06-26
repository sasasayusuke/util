<template>
  <ScheduleContainer
    :title="title"
    :is-editing="isEditing"
    :is-hide-button2="!isSetSupportSchedules"
    is-hide-button1
    @click:add="$emit('click:add')"
  >
    <ProjectSupportListTable
      :support-schedules="projectSchedules.projectSchedules"
      :is-loading="isLoading"
      :project-id="projectSchedules.projectId"
      :is-edit-delete-display="isSetSupportSchedules"
      @click:edit="edit"
      @click:delete="deleteSchedule"
    />
    <!-- 追加モーダル -->
    <template v-if="isSupportAddModalOpen">
      <ProjectSupportAddModal
        :title="$t('project.pages.edit.support.title.add')"
        :local-add-support-schedule="localAddSupportSchedule"
        @click:closeAdd="$emit('click:closeAdd')"
        @click:save="create"
      />
    </template>
    <!-- 編集モーダル -->
    <template v-if="isSupportEditModalOpen">
      <ProjectSupportEditModal
        :modal-schedule="modalSchedule"
        :title="$t('project.pages.edit.support.title.edit')"
        :is-karte-list="isKarteList"
        @click:closeEdit="$emit('click:closeEdit')"
        @refresh="$emit('refresh')"
      />
    </template>
    <!-- 削除モーダル -->
    <template v-if="isSupportDeleteModalOpen">
      <ProjectSupportDeleteModal
        :modal-schedule="modalSchedule"
        :title="$t('project.pages.edit.support.title.delete')"
        :is-karte-list="isKarteList"
        @click:closeDelete="$emit('click:closeDelete')"
        @refresh="$emit('refresh')"
      />
    </template>
  </ScheduleContainer>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import ProjectSupportListTable from '~/components/project/organisms/ProjectSupportListTable.vue'
import ScheduleContainer from '~/components/common/organisms/ScheduleContainer.vue'
import ProjectSupportAddModal from '~/components/project/molecules/ProjectSupportAddModal.vue'
import ProjectSupportEditModal from '~/components/project/molecules/ProjectSupportEditModal.vue'
import ProjectSupportDeleteModal from '~/components/project/molecules/ProjectSupportDeleteModal.vue'

import {
  CreateSupportScheduleRequest as localAddSupportSchedule,
  CreateSupportSchedules,
  GetSupportSchedulesByIdResponse,
} from '~/models/Schedule'
import { GetProjectByIdResponse } from '~/models/Project'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { meStore } from '~/store'
export { localAddSupportSchedule }

export default BaseComponent.extend({
  components: {
    ScheduleContainer,
    ProjectSupportListTable,
    ProjectSupportAddModal,
    ProjectSupportEditModal,
    ProjectSupportDeleteModal,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
    /** 選択した案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /** 編集中か */
    isEditing: {
      type: Boolean,
      required: true,
    },
    /** 支援日スケジュール追加モーダルが開かれているか */
    isSupportAddModalOpen: {
      type: Boolean,
      required: true,
    },
    /** 支援日スケジュール編集モーダルが開かれているか */
    isSupportEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** 支援日スケジュール削除モーダルが開かれているか */
    isSupportDeleteModalOpen: {
      type: Boolean,
      required: true,
    },
    /** 支援日スケジュール情報 */
    projectSchedules: {
      type: Object as PropType<GetSupportSchedulesByIdResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 個別カルテ・スケジュール一覧画面か */
    isKarteList: {
      type: Boolean,
      required: false,
    },
  },
  watch: {
    isSupportAddModalOpen(newVal) {
      if (newVal === true) {
        this.localAddSupportSchedule.timing = 'once'
        this.localAddSupportSchedule.supportDate = this.today()
        this.localAddSupportSchedule.startTime = '09:00'
        this.localAddSupportSchedule.endTime = '10:00'
      }
    },
  },
  data(): {
    localAddSupportSchedule: localAddSupportSchedule
    modalSchedule: Object
  } {
    return {
      localAddSupportSchedule: {
        timing: 'once',
        supportDate: '',
        startTime: '00:00',
        endTime: '00:00',
      },
      modalSchedule: {},
    }
  },
  methods: {
    /**
     * 支援日スケジュール編集モーダルを開く
     * @param item 選択された支援日スケジュール
     */
    edit(item: any) {
      this.modalSchedule = item
      this.$emit('click:edit')
    },
    /**
     * 支援日スケジュール削除モーダルを開く
     * @param item 選択された支援日スケジュール
     */
    deleteSchedule(item: any) {
      this.modalSchedule = item
      this.$emit('click:delete')
    },
    /** 支援日スケジュールを新規作成 */
    create() {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const request = this.localAddSupportSchedule
      const detailPage = '/project/' + id
      const karteListPage = '/karte/list/' + id

      CreateSupportSchedules(id, request)
        .then(() => {
          this.$emit('click:closeAdd')
          this.$emit('getSupportSchedulesByIdResponse')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:closeAdd')
          this.$emit('getSupportSchedulesByIdResponse')
          if (this.isKarteList) {
            // 個別カルテ・スケジュール一覧画面に遷移
            this.$router.push(karteListPage)
          } else {
            this.$router.push(detailPage)
          }
        })
    },
    today() {
      const day = new Date().toLocaleDateString('ja-JP', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
      })
      return day
    },
  },
  computed: {
    /**
     * 支援日設定（変更）が可能であるか判定
     * @returns 可能であればtrue
     */
    isSetSupportSchedules(): boolean {
      const project = this.project

      if (
        meStore.roles.includes(ENUM_ADMIN_ROLE.SALES) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.SALES_MGR) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.SYSTEM_ADMIN) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.BUSINESS_MGR)
      ) {
        // 営業担当者、営業責任者、システム管理者、事業者責任者
        return true
      }
      if (meStore.roles.includes(ENUM_ADMIN_ROLE.SUPPORTER_MGR)) {
        // 支援者責任者
        const isOrganizationIdInclude = meStore.supporterOrganizations.some(
          function (supporterOrganization) {
            return supporterOrganization.id === project.supporterOrganizationId
          }
        )
        if (isOrganizationIdInclude || !project.isSecret) {
          // 所属課案件/その他公開案件
          return true
        }
      }
      return false
    },
  },
})
</script>
