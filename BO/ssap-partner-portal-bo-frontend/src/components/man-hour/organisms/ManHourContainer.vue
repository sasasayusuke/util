<template>
  <Sheet
    v-if="!isLoading"
    elevation="3"
    rounded
    width="1200"
    class="mt-8 pb-10"
    color="white"
  >
    <!-- <v-row class="pa-0 ma-0" justify="center"> -->
    <InContentHeader
      :title="`${year()}年${month()}月 支援工数`"
      :total="manHourBySupporterUserId.summaryManHour.total"
      :level="2"
      :type="3"
      :is-confirm="
        manHourBySupporterUserId.isConfirm
          ? manHourBySupporterUserId.isConfirm
          : false
      "
    />
    <!-- 1．対面支援：お客様に対する支援（直接） -->
    <ManHourContainerBlock
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :type="'directSupportManHours'"
      @change="onChange"
    />
    <!-- 2．支援仕込：お客様に対する支援（仕込） -->
    <ManHourContainerBlock
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :type="'preSupportManHours'"
      @change="onChange"
    />
    <!-- 3．商談/提案準備：新規・継続案件獲得に向けた資料作成・調査 -->
    <ManHourContainerBlock
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :type="'salesSupportManHours'"
      @change="onChange"
    />
    <!-- 4．内部業務：お客様を特定せずSony Acceleration Platformの業務で発生した工数 -->
    <ManHourContainerBlock
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :type="'ssapManHours'"
      @change="onChange"
    />
    <!-- 5．休憩・その他：有給休暇・休憩・雑務・その他Sony Acceleration Platformの業務以外で使用した工数 -->
    <ManHourContainerBlock
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
      :type="'holidaysManHours'"
      @change="onChange"
    />
    <!-- 支援工数合計 -->
    <InContentFooter
      :title="`${year()}年${month()}月 支援工数合計`"
      :total="manHourBySupporterUserId.summaryManHour.total"
      :is-confirm="
        manHourBySupporterUserId.isConfirm
          ? manHourBySupporterUserId.isConfirm
          : false
      "
      class="mt-6"
    />
    <!-- </v-row> -->
  </Sheet>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import BaseComponent from '~/common/BaseComponent'
import ManHourContainerBlock from '~/components/man-hour/organisms/ManHourContainerBlock.vue'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import InContentHeader from '~/components/man-hour/organisms/InContentHeader.vue'
import InContentFooter from '~/components/man-hour/organisms/InContentFooter.vue'
import { Button, Sheet, Chip } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'ManHourContainer',
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
    /** 支援者単位での支援工数情報 */
    manHourBySupporterUserId: {
      type: Object,
      default: {},
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
  },
  data() {
    return {
      isValid: false,
      localManHour: cloneDeep(this.manHourBySupporterUserId),
    }
  },
  methods: {
    /**
     * ルートパラメータから年を取得
     * @return ルートパラメータから取得した年
     */
    year() {
      return this.$route.params.year ? this.$route.params.year : ''
    },
    /**
     * ルートパラメータから月を取得
     * @return ルートパラメータから取得した月
     */
    month() {
      return this.$route.params.month
        ? this.sliceMonth(this.$route.params.month)
        : ''
    },
    sliceMonth(month: string): string {
      if (month.slice(0, 1) === '0') {
        return month.replace('0', '')
      }
      return month
    },
    /**
     * 実績工数の変更を合計に反映
     * @return 変更された実績工数の合計
     */
    onChange(data: any) {
      this.$set(this.localManHour, data.type, data.localManHourData)

      switch (data.type) {
        case 'directSupportManHours':
          this.localManHour.summarySupportManHours.direct = data.sum
          break
        case 'preSupportManHours':
          this.localManHour.summarySupportManHours.pre = data.sum
          break
        case 'salesSupportManHours':
          this.localManHour.summarySupportManHours.salse = data.sum
          break
        case 'ssapManHours':
          this.localManHour.summarySupportManHours.ssap = data.sum
          break
        case 'holidaysManHours':
          this.localManHour.summarySupportManHours.others = data.sum
          break
      }
      this.localManHour.summarySupportManHours.total = Object.keys(
        this.localManHour.summarySupportManHours
      ).reduce((sum: number, key: any) => {
        return key !== 'total'
          ? sum + this.localManHour.summarySupportManHours[key]
          : sum
      }, 0)
      this.$emit('change', this.localManHour)
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
