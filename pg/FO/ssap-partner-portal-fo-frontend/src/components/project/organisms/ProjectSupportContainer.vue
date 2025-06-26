<template>
  <ScheduleContainer
    :title="title"
    :is-editing="isEditing"
    :is-hide-button1="isHideButton1"
    :is-hide-button2="!isEditable"
    @click:add="$emit('click:add')"
  >
    <ProjectSupportListTable
      :support-schedules="projectSchedules.projectSchedules"
      :is-loading="isLoading"
      :project-id="projectSchedules.projectId"
      :edit-and-delete="isEditable"
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
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
export { localAddSupportSchedule }

export default BaseComponent.extend({
  name: 'ProjectSupportContainer',
  components: {
    ScheduleContainer,
    ProjectSupportListTable,
    ProjectSupportAddModal,
    ProjectSupportEditModal,
    ProjectSupportDeleteModal,
  },
  props: {
    title: {
      type: String,
    },
    /** 編集中か */
    isEditing: {
      type: Boolean,
      required: true,
    },
    /** プロデューサー/アクセラレーター追加モーダルが開いているか */
    isSupportAddModalOpen: {
      type: Boolean,
      required: true,
    },
    /** プロデューサー/アクセラレーター編集モーダルが開いているか */
    isSupportEditModalOpen: {
      type: Boolean,
      required: true,
    },
    /** プロデューサー/アクセラレーター削除モーダルが開いているか */
    isSupportDeleteModalOpen: {
      type: Boolean,
      required: true,
    },
    isHideButton1: {
      type: Boolean,
      default: true,
    },
    /** 案件スケジュール情報 */
    projectSchedules: {
      type: Object as PropType<GetSupportSchedulesByIdResponse>,
      required: true,
    },
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
      } as localAddSupportSchedule,
      modalSchedule: {},
    }
  },
  methods: {
    /** 支援スケジュール編集 */
    edit(item: any) {
      this.modalSchedule = item
      this.$emit('click:edit')
    },
    /** 支援スケジュール削除 */
    deleteSchedule(item: any) {
      this.modalSchedule = item
      this.$emit('click:delete')
    },
    /** 支援スケジュール作成 */
    create() {
      this.clearErrorBar()
      const id = this.$route.params.projectId
      const request = this.localAddSupportSchedule
      const detailPage = '/project/' + id
      const karteListPage = '/karte/list/' + id

      CreateSupportSchedules(id, request)
        .then((res) => {
          this.$logger.info(res.data.message)
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
    /** 支援者・支援者責任者・営業ロールの所属案件のみ案件変更ができるチェック処理 */
    isEditable(): boolean {
      const projectsIds = this.$store.state.me.projectIds

      if (meStore.role === ENUM_USER_ROLE.SALES_MGR) {
        return false
      } else if (!projectsIds) {
        return false
      } else if (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
      ) {
        const id = this.$route.params.projectId
        return projectsIds.includes(id)
      } else {
        return false
      }
    },
  },
})
</script>
