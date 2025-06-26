<template>
  <div class="m-common-anchor">
    <Sheet class="d-flex justify-center py-10" width="100%">
      <Button
        style-set="large-tertiary"
        outlined
        width="160"
        :to="backToUrl(`/project/list`)"
      >
        {{ $t('common.button.backToList') }}
      </Button>
    </Sheet>
    <Sheet
      v-if="isSurveyOps || isManHourOps || isSystemAdmin"
      class="d-flex justify-center py-6"
      width="100%"
      color="#e3e3e3"
      rounded
    >
      <Button
        style-set="large-tertiary"
        outlined
        width="160"
        @click="$emit('click:delete')"
      >
        {{ $t('common.button.deleteProject') }}
      </Button>
    </Sheet>
  </div>
</template>

<script lang="ts">
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { meStore } from '~/store'

export default BaseComponent.extend({
  components: {
    Button,
    Sheet,
  },
  computed: {
    /**
     * SurveyOpsロールが含まれているアカウントか判定
     * @returns SurveyOpsロールが含まれているアカウントかの真偽値
     */
    isSurveyOps() {
      return meStore.roles.includes('survey_ops')
    },
    /**
     * ManHourOpsロールが含まれているアカウントか判定
     * @returns ManHourOpsロールが含まれているアカウントかの真偽値
     */
    isManHourOps() {
      return meStore.roles.includes('man_hour_ops')
    },
    /**
     * SystemAdminロールが含まれているアカウントか判定
     * @returns SystemAdminロールが含まれているアカウントかの真偽値
     */
    isSystemAdmin() {
      return meStore.roles.includes('system_admin')
    },
  },
  props: {},
})
</script>
