<template>
  <div>
    <!-- ゴール・マイルストーン -->
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="
        $t('master-karte.pages.detail.container.fundamental.goalMileStone')
      "
    >
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.presidentPolicy')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.presidentPolicy"
        :subtext="subtext.presidentPolicy"
        icon="void"
      />
      <!-- <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.kpi')"
        :is-current-program="isCurrentProgram"
        :text="fundamental.kpi"
        :subtext="subtext.kpi"
        icon="void"
      /> -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.toBeThreeYears')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.toBeThreeYears"
        :subtext="subtext.toBeThreeYears"
        icon="void"
      />
    </MasterKarteDetailSection>

    <!-- 現状・課題 -->
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="
        $t('master-karte.pages.detail.container.fundamental.currentIssue')
      "
    >
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.currentSituation')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.currentSituation"
        :subtext="subtext.currentSituation"
      />
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.issue')"
        :text="fundamental.issue"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.issue"
      />
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.request')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.request"
        :subtext="subtext.request"
      />
    </MasterKarteDetailSection>

    <!-- ソリューション -->
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="$t('master-karte.pages.detail.container.fundamental.solution')"
    >
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.customerSuccess')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.customerSuccess"
        :subtext="subtext.customerSuccess"
      />
      <MasterKarteDetailRow
        v-if="isCurrentProgram"
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.customerSuccessReuse'
          )
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="ReuseText"
        :subtext="reuseSubtext"
        icon="void"
      />
      <MasterKarteDetailRow
        :label="scheduleLabel"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.schedule"
        :subtext="subtext.schedule"
      />
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.lineup')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="textFormatLineup"
        :subtext="currentFormatLineup"
      />
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.supportContents')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.supportContents"
        :subtext="subtext.supportContents"
      />
    </MasterKarteDetailSection>
    <!-- 結合促進 -->
    <MasterKarteDetailSection
      v-if="!isCurrentProgram"
      :title="$t('master-karte.pages.detail.container.fundamental.integration')"
    >
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.requiredPersonalSkill'
          )
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.requiredPersonalSkill"
        :subtext="subtext.requiredPersonalSkill"
      />
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.requiredPartner')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.requiredPartner"
        :subtext="subtext.requiredPartner"
      />
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.supplementHumanResourceToSap'
          )
        "
        icon="void"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.supplementHumanResourceToSap"
        :subtext="subtext.supplementHumanResourceToSap"
      />
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.ourStrengths')
        "
        icon="void"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.ourStrengths"
        :subtext="subtext.ourStrengths"
      />
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.currentCustomerProfile'
          )
        "
        icon="void"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.currentCustomerProfile"
        :subtext="subtext.currentCustomerProfile"
      />
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.wantAcquireCustomerProfile'
          )
        "
        icon="void"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.wantAcquireCustomerProfile"
        :subtext="subtext.wantAcquireCustomerProfile"
      />
    </MasterKarteDetailSection>
    <MasterKarteDetailSection
      :title="$t('master-karte.pages.detail.container.fundamental.handOver')"
    >
      <MasterKarteDetailRow
        v-if="!isCurrentProgram"
        :label="
          $t('master-karte.pages.detail.container.fundamental.aspiration')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.aspiration"
        :subtext="subtext.aspiration"
      />
      <MasterKarteDetailRow
        :is-usage-history="true"
        :label="
          $t('master-karte.pages.detail.container.fundamental.usageHistory')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :usage-history="fundamental.usageHistory"
      />
    </MasterKarteDetailSection>
  </div>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import MasterKarteDetailRow from '~/components/master-karte/molecules/MasterKarteDetailRow.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  GetMasterKarteByIdResponse,
  FundamentalInformation,
  GetSelectBoxResponse,
  SelectBox,
} from '~/types/MasterKarte'
import {
  FundamentalInformationClass,
  SelectBoxClass,
  GetMasterKartenSelectBox,
} from '~/models/MasterKarte'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
  },
  data() {
    return {
      selectBoxItems: [] as GetSelectBoxResponse[],
    }
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    this.getSelectBoxItems()
  },
  methods: {
    /** セレクトボックスのアイテムを取得 */
    async getSelectBoxItems() {
      await GetMasterKartenSelectBox().then((res: any) => {
        this.selectBoxItems = res.data
      })
    },
    // ラインナップの表示を整形する
    lineupFormat(lineup: string) {
      let rtn = ''
      if (!lineup) return rtn

      // +が含まれていれば+繋ぎのラインナップのvalueを配列にする
      const arrayLineup = lineup.includes('+')
        ? // @ts-ignore
          lineup.split('+')
        : [lineup]
      // 詳細APIレスポンスのlineupのvalueとマッチするラインナップのselect-boxのvalueのラベルを表示
      if (typeof arrayLineup !== 'string') {
        this.lineupValues.forEach((lineupValue: any) => {
          arrayLineup?.length > 0 &&
            arrayLineup.forEach((lineupEl: string) => {
              if (lineupEl === lineupValue.value) {
                rtn += `${lineupValue.label} / `
              }
            })
        })
      }

      // 最後の" / "を削除
      return rtn.endsWith(' / ') ? rtn.slice(0, -3) : rtn
    },
  },
  computed: {
    fundamental(): FundamentalInformation {
      return this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
        : this.masterKarteProject.nextProgram.fundamentalInformation
    },
    /** 次期支援選択時に右に表示される当期支援 */
    subtext(): FundamentalInformation {
      return this.showCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
        : new FundamentalInformationClass()
    },
    /** 今回・次回のラインナップの表示フォーマット */
    textFormatLineup(): string {
      const fundamental = this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
        : this.masterKarteProject.nextProgram.fundamentalInformation
      return fundamental ? this.lineupFormat(fundamental.lineup) : ''
    },
    /** 次回の当期支援のラインナップの表示フォーマット */
    currentFormatLineup(): string {
      return this.lineupFormat(
        this.masterKarteProject.currentProgram.fundamentalInformation.lineup
      )
    },
    /** ラインナップのセレクトボックスのアイテム */
    lineupValues(): SelectBox[] {
      return this.selectBoxItems.length > 0
        ? this.selectBoxItems?.filter(
            (box: GetSelectBoxResponse) => box.name === 'lineup'
          )[0].items
        : [new SelectBoxClass()]
    },
    // リユースはBooleanでデフォルトがfalseであるため、今回非表示の場合は""を返す
    reuseSubtext(): any {
      const customerSuccessReuseItems = this.selectBoxItems?.filter(
        (box: GetSelectBoxResponse) => box.name === 'customerSuccessReuse'
      )
      if (!this.showCurrentProgram || customerSuccessReuseItems.length === 0) {
        return ''
      } else if (this.subtext.customerSuccessReuse) {
        return customerSuccessReuseItems[0].items[0].label
      } else if (!this.subtext.customerSuccessReuse) {
        return customerSuccessReuseItems[0].items[1].label
      }
    },
    /** カスタマーリユース用のテキスト */
    ReuseText(): any {
      const customerSuccessReuseItems = this.selectBoxItems?.filter(
        (box: GetSelectBoxResponse) => box.name === 'customerSuccessReuse'
      )
      // 当期支援表示
      if (this.isCurrentProgram && customerSuccessReuseItems.length > 0) {
        return this.fundamental.customerSuccessReuse
          ? customerSuccessReuseItems[0].items[0].label
          : customerSuccessReuseItems[0].items[1].label
      }
      // 次期支援表示
      if (!this.isCurrentProgram && customerSuccessReuseItems.length > 0) {
        if (
          this.masterKarteProject.nextProgram.fundamentalInformation
            .customerSuccessReuse !== null
        ) {
          return this.fundamental.customerSuccessReuse
            ? customerSuccessReuseItems[0].items[0].label
            : customerSuccessReuseItems[0].items[1].label
        } else {
          return ''
        }
      }
    },
    /** 今回・次回の支援時期の項目表示切り替え */
    scheduleLabel(): string {
      if (this.isCurrentProgram) {
        return this.$t(
          'master-karte.pages.detail.container.fundamental.scheduleCurrentProgram'
        ) as string
      } else {
        return this.$t(
          'master-karte.pages.detail.container.fundamental.scheduleNextProgram'
        ) as string
      }
    },
  },
})
</script>

<style></style>
