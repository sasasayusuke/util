<template>
  <v-row class="pa-0 ma-0" justify="center">
    <CommonContainer :is-man-hour="true" is-hide-footer>
      <InContentHeader
        :title="
          `${queryYear}` +
          $t('man-hour.title.year') +
          `${queryMonth}` +
          $t('man-hour.title.monthTitle')
        "
        :level="2"
        :type="3"
        :is-confirm="manHour.isConfirm ? manHour.isConfirm : false"
        :total="
          isEditing
            ? localManHour.summaryManHour.total
            : manHour.summaryManHour.total
        "
        :is-editing="isEditing"
        :is-within-month-before-last="isWithinMonthBeforeLast"
        :is-valid="
          isValidDirectSupportManHours &&
          isValidPreSupportManHours &&
          isValidSalesSupportManHours &&
          isValidSsapManHours &&
          isValidHolidaysManHours
        "
        @buttonAction="buttonAction"
      />
      <ManHourContainerBlock
        :value="isValid"
        :man-hour="manHour"
        :type="'directSupportManHours'"
        :is-editing="isEditing"
        @change="onChange"
        @input="isValidDirectSupportManHours = $event"
      />
      <ManHourContainerBlock
        :value="isValid"
        :man-hour="manHour"
        :type="'preSupportManHours'"
        :is-editing="isEditing"
        @change="onChange"
        @input="isValidPreSupportManHours = $event"
      />
      <ManHourContainerBlock
        :value="isValid"
        :man-hour="manHour"
        :suggest-customers="suggestCustomers"
        :type="'salesSupportManHours'"
        :is-editing="isEditing"
        @change="onChange"
        @input="isValidSalesSupportManHours = $event"
      />
      <ManHourContainerBlock
        :value="isValid"
        :man-hour="manHour"
        :type="'ssapManHours'"
        :is-editing="isEditing"
        @change="onChange"
        @input="isValidSsapManHours = $event"
      />
      <ManHourContainerBlock
        :value="isValid"
        :man-hour="manHour"
        :type="'holidaysManHours'"
        :is-editing="isEditing"
        @change="onChange"
        @input="isValidHolidaysManHours = $event"
      />

      <InContentFooter
        :title="
          `${queryYear}` +
          $t('man-hour.title.year') +
          `${queryMonth}` +
          $t('man-hour.title.monthSum')
        "
        :is-confirm="manHour.isConfirm ? manHour.isConfirm : false"
        :total="
          isEditing
            ? localManHour.summaryManHour.total
            : manHour.summaryManHour.total
        "
        :is-editing="isEditing"
        :is-within-month-before-last="isWithinMonthBeforeLast"
        class="mt-5 mb-10"
        :is-valid="
          isValidDirectSupportManHours &&
          isValidPreSupportManHours &&
          isValidSalesSupportManHours &&
          isValidSsapManHours &&
          isValidHolidaysManHours
        "
        @buttonAction="buttonAction"
      />
    </CommonContainer>
  </v-row>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import { isBefore, sub } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import ManHourContainerBlock from '~/components/man-hour/organisms/ManHourContainerBlock.vue'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import InContentHeader from '~/components/man-hour/organisms/InContentHeader.vue'
import InContentFooter from '~/components/man-hour/organisms/InContentFooter.vue'
import { Button, Sheet, Chip } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    ManHourContainerBlock,
    InContentHeader,
    InContentFooter,
    Button,
    Sheet,
    Chip,
  },
  props: {
    /**
     * 支援工数データ
     */
    manHour: {
      type: Object,
      default: false,
    },
    /**
     * 編集中か否か
     */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /**
     * サジェストする顧客情報
     */
    suggestCustomers: {
      type: Array,
    },
  },
  data() {
    return {
      localManHour: cloneDeep(this.manHour),
      isValidDirectSupportManHours: true,
      isValidPreSupportManHours: true,
      isValidSalesSupportManHours: true,
      isValidSsapManHours: true,
      isValidHolidaysManHours: true,
      isFirstLoaded: false,
      isChanged: false,
    }
  },
  methods: {
    /**
     * ボタン押下時の挙動
     * @param ボタンのアクションタイプ
     */
    buttonAction(actionType: number = 0) {
      this.$emit('buttonAction', actionType)
    },
    /**
     * 入力値に変更があった場合の処理
     * @param 入力データ
     */
    onChange(data: any) {
      this.$set(this.localManHour, data.type, data.localManHourData)

      switch (data.type) {
        case 'directSupportManHours':
          this.localManHour.summaryManHour.direct = data.sum
          break
        case 'preSupportManHours':
          this.localManHour.summaryManHour.pre = data.sum
          break
        case 'salesSupportManHours':
          this.localManHour.summaryManHour.sales = data.sum
          break
        case 'ssapManHours':
          this.localManHour.summaryManHour.ssap = data.sum
          break
        case 'holidaysManHours':
          this.localManHour.summaryManHour.others = data.sum
          break
      }
      this.localManHour.summaryManHour.total = Object.keys(
        this.localManHour.summaryManHour
      ).reduce((sum: number, key: any) => {
        return key !== 'total'
          ? sum + this.localManHour.summaryManHour[key]
          : sum
      }, 0)
      this.$emit('change', this.localManHour)
      // 変更無しの場合に保存ボタンを非活性する場合は解放し保存ボタンの非活性条件に含めること。
      //this.isChanged = true
    },
  },
  computed: {
    /**
     * 基準の月よりも前か否か
     * @returns 基準の月よりも前か否か
     */
    isWithinMonthBeforeLast(): boolean {
      const dataDate = new Date(this.manHour.yearMonth)
      const forComparisonDate = sub(getCurrentDate(), { months: 3 })
      return isBefore(dataDate, forComparisonDate)
    },
    queryDate(): Date {
      if (!this.$route.query.year || !this.$route.query.month) {
        return getCurrentDate()
      }
      const queryYear = parseInt(this.$route.query.year as string)
      const queryMonth = parseInt(this.$route.query.month as string)
      return new Date(queryYear, queryMonth - 1)
    },
    queryYear(): number {
      return this.queryDate.getFullYear()
    },
    queryMonth(): number {
      return this.queryDate.getMonth() + 1
    },
  },
  watch: {
    /**
     * 編集中か否か
     * @param 新しい値か否か
     * @param 古い値か否か
     */
    isEditing(newVal: boolean, oldVal: boolean) {
      if (newVal === true && oldVal === false) {
        this.isFirstLoaded = false
        this.isChanged = false
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.a-manhour-total--foot {
  font-weight: bold;
  font-size: 1.25rem;
  span {
    font-size: 1.5rem;
    color: $c-primary-dark;
  }
}
</style>
