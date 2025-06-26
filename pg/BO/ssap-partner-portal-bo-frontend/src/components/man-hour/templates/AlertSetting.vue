<template>
  <RootTemplate pt-8 pb-10 mb-10>
    <AlertConfigContainer
      :is-editing="isEditing"
      :service-types="serviceTypes"
      :alert-settings="alertSettings"
      :is-loading-button="isLoadingButton"
      @click:positive="$emit('click:positive')"
      @click:negative="$emit('click:negative')"
      @change="$listeners['change']"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import AlertConfigContainer from '~/components/man-hour/organisms/AlertConfigContainer.vue'
import { GetAlertSettingsResponse, ServiceTypeItems } from '~/models/Master'

export default BaseComponent.extend({
  name: 'AlertSetting',
  components: {
    RootTemplate,
    AlertConfigContainer,
  },
  props: {
    /** サービス種別一覧情報 */
    serviceTypes: {
      type: Array as PropType<ServiceTypeItems[]>,
    },
    /** 工数アラート設定 */
    alertSettings: {
      type: Object as PropType<GetAlertSettingsResponse>,
    },
    /** 工数アラート設定を更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isEditing: true,
    }
  },
})
</script>
