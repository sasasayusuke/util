<template>
  <div class="m-karte-schedules">
    <template v-for="(schedule, index) in schedules">
      <CommonScheduleRow
        :key="index"
        link-prefix="/karte"
        :schedule="schedule"
        :class="{ 'mt-5': index }"
      />
    </template>
  </div>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import { GetKartenResponse } from '~/models/Karte'
import { PropType } from '~/common/BaseComponent'

export default BasePage.extend({
  name: 'ProjectKarteList',
  components: {},
  middleware: ['roleCheck'],
  props: {
    karten: {
      type: Array as PropType<GetKartenResponse>,
    },
    isLoading: {
      type: Boolean,
      default: false,
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
