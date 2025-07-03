<template>
  <InPageHeader>
    <slot name="default" />
    <template #date>
      {{ $t('man-hour.pages.alert.list.updateAt') }}: {{ date }}
    </template>
    <template #button>
      <!-- CSV出力ボタン -->
      <Button
        outlined
        style-set="large-primary"
        class="mr-2"
        :disabled="csvButtonDisabled"
        @click="buttonAction(0)"
      >
        {{ $t('common.button.csvExport') }}
      </Button>
      <template v-if="hasRole(getAllowedRoles('/man-hour/alert/config'))">
        <!-- アラート設定ボタン -->
        <Button style-set="large-primary" :to="'/man-hour/alert/config'">
          {{ $t('common.button.alertConfig') }}
        </Button>
      </template>
    </template>
  </InPageHeader>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import InPageHeader from '~/components/man-hour/organisms/InPageHeader.vue'
import { hasRole, getAllowedRoles } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  name: 'ListInPageHeader',
  components: {
    Button,
    InPageHeader,
  },
  props: {
    /** 最終集計日時 */
    date: {
      type: String,
      default: '',
    },
    /** CSV出力ボタン無効判定 */
    csvButtonDisabled: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    getAllowedRoles,
    hasRole,
    buttonAction(type: number) {
      this.$emit('buttonAction', type)
    },
  },
})
</script>
<style lang="scss" scoped>
.v-btn:not(.v-btn--round).v-size--large {
  height: 30px;
}
</style>
