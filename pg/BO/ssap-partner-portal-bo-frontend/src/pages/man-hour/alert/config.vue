<template>
  <TemplateManHourAlertSetting
    :alert-settings="alertSettings"
    :service-types="serviceTypes"
    :is-loading-button="isLoadingButton"
    @change="onChange"
    @click:positive="onClickPositive"
    @click:negative="onClickNegative"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateManHourAlertSetting from '~/components/man-hour/templates/AlertSetting.vue'
import {
  GetAlertSettings,
  GetServiceTypes,
  UpdateAlertSettings,
  UpdateAlertSettingsResponse,
  UpdateAlertSettingsRequest,
  ServiceTypeItems,
} from '@/models/Master'

export default BasePage.extend({
  name: 'ManHourAlertConfig',
  middleware: ['roleCheck'],
  components: {
    TemplateManHourAlertSetting,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.alert.config.name') as string,
    }
  },
  data() {
    return {
      alertSettings: new UpdateAlertSettingsResponse(),
      serviceTypes: [] as ServiceTypeItems[],
      localAlertSettingAttributes: new UpdateAlertSettingsRequest(),
      isLoadingButton: false,
    }
  },
  mounted() {
    this.displayLoading([this.getAlertSetting(), this.getServiceTypes()])
  },
  methods: {
    /**
     * UpdateAlertSettingAPIを叩いて、工数アラート設定を更新
     * @param { UpdateAlertSettingsRequest } request UpdateAlertSettingAPIのリクエストパラメータ
     */
    async updateAlertSetting() {
      this.isLoadingButton = true
      const version = this.alertSettings.version
      const request: UpdateAlertSettingsRequest =
        this.localAlertSettingAttributes
      await UpdateAlertSettings(version, request)
        .then((res: any) => {
          this.alertSettings = res.data
          this.$router.push('/man-hour/alert/list')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    /**
     * GetAlertSettingAPIを叩いて、工数アラート設定を取得
     */
    async getAlertSetting() {
      await GetAlertSettings().then((res: any) => {
        this.alertSettings = res.data
      })
    },
    /**
     * GetServiceTypesAPIを叩いて、サービス種別の一覧を取得
     */
    async getServiceTypes() {
      await GetServiceTypes().then((res: any) => {
        this.serviceTypes = res.data.serviceTypes
      })
    },
    onChange(localAlertSettingAttributes: any) {
      this.localAlertSettingAttributes = localAlertSettingAttributes
    },
    onClickPositive() {
      // 反映保存処理
      // 編集内容はlocalAlertSettingAttributesで吸い取り。
      this.updateAlertSetting()
    },
    onClickNegative() {
      this.$router.push('/man-hour/alert/list')
    },
  },
})
</script>
