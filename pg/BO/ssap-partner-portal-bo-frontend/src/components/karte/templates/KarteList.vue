<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <KarteInformation :project="response" :is-loading="isLoading.project" />
    <!-- 支援日スケジュール -->
    <ProjectSupportContainer
      id="supportSchedules"
      :title="$t('project.pages.detail.supportSchedules.titleKarteList')"
      :project="response"
      :is-editing="false"
      :is-support-add-modal-open="isSupportAddModalOpen"
      :is-support-edit-modal-open="isSupportEditModalOpen"
      :is-support-delete-modal-open="isSupportDeleteModalOpen"
      :project-schedules="projectSupportSchedules"
      :is-loading="isLoading.supportSchedules"
      :is-karte-list="true"
      class="mt-8"
      @click:add="onClickSupportAdd"
      @click:closeAdd="isSupportAddModalOpen = false"
      @click:edit="onClickSupportEdit"
      @click:closeEdit="isSupportEditModalOpen = false"
      @click:delete="onClickSupportDelete"
      @click:closeDelete="isSupportDeleteModalOpen = false"
      @refresh="refresh"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import { GetProjectByIdResponse } from '~/models/Project'
import { GetSupportSchedulesByIdResponse } from '~/models/Schedule'
import KarteInformation from '~/components/karte/organisms/KarteInformation.vue'
import ProjectSupportContainer from '~/components/project/organisms/ProjectSupportContainer.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import CommonList from '~/components/common/templates/CommonList'

export interface isLoading {
  project: boolean
  supportSchedules: boolean
}

export default CommonList.extend({
  name: 'KarteList',
  components: {
    RootTemPlate,
    KarteInformation,
    ProjectSupportContainer,
  },
  props: {
    /**
     * GetProjectById APIのレスポンス
     */
    response: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * ページがリロード中か否か
     */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 案件区分が「アンケート」の案件スケジュール */
    projectSupportSchedules: {
      type: Object as PropType<GetSupportSchedulesByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      pageName: this.$t('karte.pages.list.name'),
      project: this.response,
      /** 支援日スケジュール追加モーダルが開いているか */
      isSupportAddModalOpen: false,
      /** 支援日スケジュール編集モーダルが開いているか */
      isSupportEditModalOpen: false,
      /** 支援日スケジュール削除モーダルが開いているか */
      isSupportDeleteModalOpen: false,
    }
  },
  methods: {
    /** 支援日スケジュール追加モーダルを開く */
    onClickSupportAdd() {
      this.isSupportAddModalOpen = true
    },
    /** 支援日スケジュール編集モーダルを開く */
    onClickSupportEdit() {
      this.isSupportEditModalOpen = true
    },
    /** 支援日スケジュール削除モーダルを開く */
    onClickSupportDelete() {
      this.isSupportDeleteModalOpen = true
    },
    refresh() {
      this.$emit('refresh')
    },
  },
})
</script>
