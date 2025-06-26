<template>
  <div class="m-karte-schedules">
    <template v-for="(schedule, index) in schedules">
      <CommonScheduleRow
        :key="index"
        link-prefix="/karte"
        :project="project"
        :schedule="schedule"
        :user="user"
        :class="{ 'mt-5': index }"
      />
    </template>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { GetProjectByIdResponse } from '~/models/Project'
import { GetKartenResponse } from '~/models/Karte'
import type { PropType } from '~/common/BaseComponent'
import CommonScheduleRow from '~/components/common/molecules/CommonScheduleRow.vue'
import { UserListItem } from '~/models/User'

export default BaseComponent.extend({
  name: 'KarteSchedules',
  components: {
    CommonScheduleRow,
  },
  props: {
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * GetKarten APIのレスポンス
     */
    karten: {
      type: Array as PropType<GetKartenResponse>,
      required: true,
    },
    user: {
      type: Object as PropType<UserListItem>,
      required: true,
    },
  },
  computed: {
    /**
     * カルテの更新者を最終更新者とする
     */
    schedules() {
      const results = []
      for (const i in this.karten) {
        const schedule: any = this.karten[i]
        schedule.lastUpdateUserName = schedule.updateUser
        results.push(schedule)
      }
      return results
    },
  },
})
</script>
