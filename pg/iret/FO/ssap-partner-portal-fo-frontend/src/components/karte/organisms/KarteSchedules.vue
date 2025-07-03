<template>
  <div class="m-karte-schedules">
    <template v-for="(schedule, index) in karten">
      <CommonScheduleRow
        :key="index"
        :disabled="disabled(schedule)"
        link-prefix="/karte"
        :schedule="schedule"
        :class="{ 'mt-5': index }"
      />
    </template>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { GetProjectByIdResponse } from '~/models/Project'
import { KarteListItem, GetKartenResponse } from '~/models/Karte'
import type { PropType } from '~/common/BaseComponent'
import CommonScheduleRow from '~/components/common/molecules/CommonScheduleRow.vue'
import { meStore } from '~/store'
import { SupporterUser } from '@/models/Project'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  name: '',
  components: {
    CommonScheduleRow,
  },
  props: {
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    karten: {
      type: Array as PropType<GetKartenResponse>,
      required: true,
    },
  },
  computed: {
    /**
     * カルテスケジュールをフォーマット化
     * @returns カルテスケジュール
     */
    schedules() {
      const results = []
      for (const i in this.karten) {
        const schedule: any = this.karten[i]
        // TODO 意図しない代入が起きている状態かもしれないので要調査
        schedule.lastUpdateUserName = schedule.updateUser
        results.push(schedule)
      }
      return results
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * isDraftの値と権限によりクリッカブルを制御
     * @returns disabledか否か
     */
    disabled(schedule: KarteListItem): boolean {
      const project = this.project

      if (
        meStore.role === ENUM_USER_ROLE.SALES_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR
      ) {
        // 営業責任者または事業者責任者または支援者責任者
        return false
      }

      if (meStore.role === ENUM_USER_ROLE.SALES) {
        // 営業担当者
        if (meStore.id && meStore.id === project.mainSalesUserId) {
          // 担当案件
          return false
        }
        return true
      }

      if (meStore.role === ENUM_USER_ROLE.SUPPORTER) {
        // 支援者
        // プロデューサー
        if (project.mainSupporterUserId === meStore.id) {
          // 担当案件
          return false
        }
        // アクセラレーター
        for (const i in project.supporterUsers) {
          const supporterUser: SupporterUser = project.supporterUsers[i]
          if (supporterUser && supporterUser.id.includes(meStore.id)) {
            // 担当案件
            return false
          }
        }
        return true
      }

      if (meStore.role === ENUM_USER_ROLE.CUSTOMER) {
        // お客様
        for (const i in meStore.projectIds) {
          const projectId: String = meStore.projectIds[i]
          if (projectId === project.id && !schedule.isDraft) {
            // 参加している案件かつお客様向けステータス公開
            return false
          }
        }
        return true
      }
      return true
    },
  },
})
</script>
