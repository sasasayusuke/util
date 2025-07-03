<template>
  <RootTemplate>
    <ManHourAlertProjectDetailContainer
      :title="pageName"
      :summary-project-man-hour-alerts="summaryProjectManHourAlerts"
      :alert-settings="alertSettings"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import ManHourAlertProjectDetailContainer from '~/components/man-hour/organisms/ManHourAlertProjectDetailContainer.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'ManHourAlertProjectDetail',
  components: {
    RootTemplate,
    ManHourAlertProjectDetailContainer,
  },
  props: {
    /** 案件指定の工数状況とアラート */
    summaryProjectManHourAlerts: {
      type: Object,
      required: true,
    },
    /** 工数アラート設定 */
    alertSettings: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      pageName: this.$t('man-hour.pages.alertDetail.name'),
    }
  },
  methods: {
    /** 一覧へ戻るボタン押下時案件別工数確認・アラート設定へ遷移 */
    onClickNegative() {
      this.$router.push('/man-hour/alert/list')
    },
  },
})
</script>
