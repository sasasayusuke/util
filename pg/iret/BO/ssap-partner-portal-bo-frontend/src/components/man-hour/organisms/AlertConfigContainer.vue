<template>
  <DetailContainer
    :title="$t('man-hour.pages.alert.config.name')"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive')"
    @click:negative="$emit('click:negative')"
  >
    <AlertConfigContainerBlock
      v-model="isValid"
      :is-editing="isEditing"
      :service-types="serviceTypes"
      :alert-settings="alertSettings"
      @change="onChange"
    />
  </DetailContainer>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import AlertConfigContainerBlock from '~/components/man-hour/organisms/AlertConfigContainerBlock.vue'
import { GetAlertSettingsResponse, ServiceTypeItems } from '~/models/Master'

export default BaseComponent.extend({
  name: 'AlertConfigContainer',
  components: {
    DetailContainer,
    AlertConfigContainerBlock,
  },
  props: {
    /** 編集中判定 */
    isEditing: {
      type: Boolean,
    },
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
      isValid: true,
      isFirstLoaded: false,
      isChanged: false,
    }
  },
  methods: {
    buttonAction1() {
      this.$emit('buttonAction1')
    },
    buttonAction2() {
      this.$emit('buttonAction2')
    },
    /** 変更をemit */
    onChange(localAlertSettingAttributes: any) {
      this.$emit('change', localAlertSettingAttributes)
      if (this.isFirstLoaded === false) {
        this.isChanged = false
        this.isFirstLoaded = true
      } else {
        this.isChanged = true
      }
    },
  },
  computed: {
    isValidWithChange(): boolean {
      if (this.isEditing) {
        if (this.isFirstLoaded === false) {
          return false
        } else {
          return this.isChanged ? this.isValid : false
        }
      } else {
        return true
      }
    },
  },
  watch: {
    isEditing(newVal: boolean, oldVal: boolean) {
      if (newVal === true && oldVal === false) {
        this.isFirstLoaded = false
        this.isChanged = false
      }
    },
  },
})
</script>
