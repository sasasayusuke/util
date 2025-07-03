<template>
  <TemplateMonthlyManHourBySupporterDetail
    :is-loading="isLoading"
    :man-hour-by-supporter-user-id="manHourBySupporterUserId"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateMonthlyManHourBySupporterDetail, {
  isLoading,
} from '~/components/man-hour/templates/MonthlyManHourBySupporterDetail.vue'
import {
  GetManHourBySupporterUserIdRequest,
  GetManHourBySupporterUserIdResponse,
  GetManHourBySupporterUserId,
} from '~/models/ManHour'

export default BasePage.extend({
  name: 'ManHourSupporterUserDetail',
  middleware: ['roleCheck'],
  components: {
    TemplateMonthlyManHourBySupporterDetail,
  },
  head() {
    return {
      title: this.$t('man-hour.pages.supporter.detail.name') as string,
    }
  },
  data(): {
    manHourBySupporterUserId: GetManHourBySupporterUserIdResponse
    isLoading: isLoading
  } {
    return {
      manHourBySupporterUserId: new GetManHourBySupporterUserIdResponse(),
      isLoading: {
        manHourBySupporterUserId: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getManHourBySupporterUserId()])
  },
  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year(this: any) {
      return parseInt(this.$route.params.year)
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month(this: any) {
      return parseInt(this.$route.params.month)
    },
    /**
     * GetManHourBySupporterUserIdAPIを叩いて、支援者単位で支援工数を取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     * @param {string} supporterUserId ルートパラメータから取得した支援者ユーザーID
     */
    async getManHourBySupporterUserId() {
      const supporterUserId = this.$route.params.userId
      const params: GetManHourBySupporterUserIdRequest =
        new GetManHourBySupporterUserIdRequest()
      params.year = this.year()
      params.month = this.month()
      await GetManHourBySupporterUserId(supporterUserId, params).then((res) => {
        this.manHourBySupporterUserId = res.data
        this.isLoading.manHourBySupporterUserId = false
      })
    },
  },
})
</script>
