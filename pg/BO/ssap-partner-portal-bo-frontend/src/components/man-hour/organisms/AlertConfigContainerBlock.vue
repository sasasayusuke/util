<template>
  <div class="px-8 pt-8 pb-0 ma-0">
    <div class="pb-2 d-flex align-center">
      <Title slot="title" hx="h2" style-set="large">{{
        $t('man-hour.pages.alert.config.coefficientSetting')
      }}</Title>
    </div>
    <AlertSimpleTable
      v-model="isValid"
      :is-editing="isEditing"
      :service-types="serviceTypes"
      :alert-settings="alertSettings"
      @change="$listeners['change']"
      @input="$listeners['input']"
    ></AlertSimpleTable>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import AlertSimpleTable from '~/components/man-hour/molecules/AlertSimpleTable.vue'
import { Title } from '~/components/common/atoms/index'
export default BaseComponent.extend({
  name: 'AlertConfigContainerBlock',
  components: {
    AlertSimpleTable,
    Title,
  },
  model: {
    prop: 'isValid',
    event: 'input',
  },
  props: {
    /** 編集中判定 */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /** サービス種別一覧情報 */
    serviceTypes: {
      type: Array,
      default: [],
    },
    /** 工数アラート設定 */
    alertSettings: {
      type: Object,
      default: {},
    },
    /** バリデーション判定 */
    isValid: {
      type: Boolean,
      default: false,
    },
  },
})
</script>
