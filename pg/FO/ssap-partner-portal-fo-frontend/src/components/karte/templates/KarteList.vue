<template>
  <RootTemPlate pt-0 pb-0 mb-0>
    <!-- 直近のカルテ -->
    <LatestSupportProjectSchedule
      :latest-support-project-schedule="latestSupportProjectSchedules"
    />
    <!-- 支援日スケジュール -->
    <ProjectSupportContainer
      id="supportSchedules"
      :title="$t('project.pages.detail.supportSchedules.titleKarteList')"
      :is-editing="false"
      :is-support-add-modal-open="isSupportAddModalOpen"
      :is-support-edit-modal-open="isSupportEditModalOpen"
      :is-support-delete-modal-open="isSupportDeleteModalOpen"
      :project-schedules="projectSupportSchedules"
      :is-loading="isLoading.supportSchedules"
      :is-karte-list="true"
      class="mt-6"
      @click:add="onClickSupportAdd"
      @click:closeAdd="isSupportAddModalOpen = false"
      @click:edit="onClickSupportEdit"
      @click:closeEdit="isSupportEditModalOpen = false"
      @click:delete="onClickSupportDelete"
      @click:closeDelete="isSupportDeleteModalOpen = false"
      @refresh="refresh"
    />
    <KarteInformation
      class="mt-6"
      :project="project"
      :is-loading="isLoading.project"
      :is-customer="isCustomer"
    />
    <KarteManHour
      v-show="!isCustomer"
      class="mt-6"
      :man-hour-status="manHourStatus"
      :batch-control="batchControl"
      :is-loading="isLoading.manHourStatus"
    />
    <!-- <KarteSort @click="sort($event)" />
    <KarteListTable :projects="projects" /> -->
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import { GetSummaryProjectSupporterManHourStatusResponse } from '~/models/ManHour'
import { GetProjectByIdResponse } from '~/models/Project'
import { GetBatchControlByIdResponse } from '~/models/Master'
import {
  GetSupportSchedulesByIdResponse,
  SupportProjectSchedule,
} from '~/models/Schedule'
import KarteInformation from '~/components/karte/organisms/KarteInformation.vue'
import KarteManHour from '~/components/karte/organisms/KarteManHour.vue'
import ProjectSupportContainer from '~/components/project/organisms/ProjectSupportContainer.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import CommonList from '~/components/common/templates/CommonList'
import LatestSupportProjectSchedule from '~/components/karte/organisms/LatestSupportProjectSchedule.vue'

export interface isLoading {
  project: boolean
  manHourStatus: boolean
  batchControl: boolean
  supportSchedules: boolean
}

export default CommonList.extend({
  name: '',
  components: {
    RootTemPlate,
    ListInPageHeader,
    KarteInformation,
    KarteManHour,
    ProjectSupportContainer,
    LatestSupportProjectSchedule,
  },
  props: {
    /**
     * プロジェクト情報
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * 支援工数状況
     */
    manHourStatus: {
      type: Object as PropType<GetSummaryProjectSupporterManHourStatusResponse>,
      required: true,
    },
    /**
     * 各種集計バッチ処理の最終完了日時
     */
    batchControl: {
      type: Object as PropType<GetBatchControlByIdResponse>,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
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
      /** 支援日スケジュール追加モーダルが開いているか */
      isSupportAddModalOpen: false,
      /** 支援日スケジュール編集モーダルが開いているか */
      isSupportEditModalOpen: false,
      /** 支援日スケジュール削除モーダルが開いているか */
      isSupportDeleteModalOpen: false,
    }
  },
  computed: {
    latestSupportProjectSchedules(): SupportProjectSchedule[] {
      const today = new Date()
      const oneWeekAgo = new Date()
      oneWeekAgo.setDate(oneWeekAgo.getDate() - 7)

      const data = this.projectSupportSchedules.projectSchedules
        .filter((item) => {
          const date = new Date(item.supportDate)
          return date >= oneWeekAgo && date <= today
        })
        .sort((a, b) => b.supportDate.localeCompare(a.supportDate))
        .slice(0, 10)
      return data
    },
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
