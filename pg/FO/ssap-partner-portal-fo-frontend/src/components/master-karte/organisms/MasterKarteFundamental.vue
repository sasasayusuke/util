<template>
  <div>
    <!-- ゴール・マイルストーン -->
    <MasterKarteDetailSection
      v-if="!isCustomer"
      :is-current-program="isCurrentProgram"
      :chip="!isCustomer"
      :title="
        $t('master-karte.pages.detail.container.fundamental.goalMileStone')
      "
    >
      <!-- 社長の方針 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.presidentPolicy')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.presidentPolicy"
        :subtext="subtext.presidentPolicy"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.presidentPolicy
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.presidentPolicy'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.presidentPolicy'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- ゴール・KPI -->
      <!-- <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.kpi')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.kpi"
        :subtext="subtext.kpi"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.kpi"
            style-set="outlined"
            :max-length="1000"
            :aria-label="
              $t('master-karte.pages.detail.container.fundamental.labels.kpi')
            "
          />
        </template>
      </MasterKarteDetailRow> -->

      <!-- 3年後のTo Be像 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.toBeThreeYears')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.toBeThreeYears"
        :subtext="subtext.toBeThreeYears"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.toBeThreeYears
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.toBeThreeYears'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.toBeThreeYears'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <!-- 現状・課題 -->
    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
      :chip="!isCustomer"
      :title="
        $t('master-karte.pages.detail.container.fundamental.currentIssue')
      "
    >
      <!-- 現状 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.currentSituation')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.currentSituation"
        :subtext="subtext.currentSituation"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.currentSituation
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.currentSituation'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.currentSituation'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 課題 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.issue')"
        :text="fundamental.issue"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.issue"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.issue"
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t('master-karte.pages.detail.container.fundamental.labels.issue')
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.issue'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- リクエスト -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.request')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.request"
        :subtext="subtext.request"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.request"
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.request'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.request'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <!-- ソリューション -->
    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
      :chip="!isCustomer"
      :title="$t('master-karte.pages.detail.container.fundamental.solution')"
    >
      <!-- カスタマーサクセス -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.customerSuccess')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.customerSuccess"
        :subtext="subtext.customerSuccess"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.customerSuccess
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.customerSuccess'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.customerSuccess'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- カスタマーサクセスリユース有無 -->
      <MasterKarteDetailRow
        v-if="!isCustomer && isCurrentProgram"
        :label="
          $t(
            'master-karte.pages.detail.container.fundamental.customerSuccessReuse'
          )
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="reuseText"
        :subtext="reuseSubText(subtext.customerSuccessReuse)"
        icon="void"
      >
        <template #editable>
          <RadioGroup
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.customerSuccessReuse
            "
            horizontal
            :values="customerSuccessReuseValues"
            :labels="customerSuccessReuseLabels"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 支援期間 -->
      <MasterKarteDetailRow
        :label="scheduleLabel"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.schedule"
        :subtext="subtext.schedule"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.schedule"
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.schedule'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.schedule'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- ラインナップ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.fundamental.lineup')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="textFormatLineup"
        :subtext="currentFormatLineup"
      >
        <template v-if="editableRole && !isCurrentProgram" #editable>
          <div class="lineup">
            <AutoComplete
              v-model="localParam.nextProgram.fundamentalInformation.lineup"
              outlined
              show-icon
              dense
              multiple
              :items="lineupValues"
              item-text="label"
              item-value="value"
              hide-details
              :placeholder="$t('common.placeholder.autoCompleteSelect')"
              :search-input.sync="searchInput"
              @change="searchInput = ''"
            />
          </div>
        </template>
      </MasterKarteDetailRow>

      <!-- 支援内容 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.supportContents')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.supportContents"
        :subtext="subtext.supportContents"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.supportContents
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.supportContents'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.supportContents'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <!-- 結合促進 -->
    <MasterKarteDetailSection
      v-if="!isCurrentProgram"
      :is-current-program="isCurrentProgram"
      :title="$t('master-karte.pages.detail.container.fundamental.integration')"
    >
      <!-- お客様に不足している人的リソース -->
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
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation
                .requiredPersonalSkill
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.requiredPersonalSkill'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- お客様に紹介したい企業や産業 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.requiredPartner')
        "
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.requiredPartner"
        :subtext="subtext.requiredPartner"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation.requiredPartner
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.requiredPartner'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- SAP支援チームに補充したい人的リソース -->
      <MasterKarteDetailRow
        v-if="!isCustomer"
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
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation
                .supplementHumanResourceToSap
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.supplementHumanResourceToSap'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- お客様の強み -->
      <MasterKarteDetailRow
        v-if="!isCustomer"
        :label="
          $t('master-karte.pages.detail.container.fundamental.ourStrength')
        "
        icon="void"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :text="fundamental.ourStrength"
        :subtext="subtext.ourStrength"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.ourStrength"
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.ourStrength'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 当該プロジェクトのお客様が現在持っている顧客（像） -->
      <MasterKarteDetailRow
        v-if="!isCustomer"
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
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation
                .currentCustomerProfile
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.currentCustomerProfile'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 当該プロジェクトのお客様が今後獲得したい顧客（像） -->
      <MasterKarteDetailRow
        v-if="!isCustomer"
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
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="
              localParam.nextProgram.fundamentalInformation
                .wantAcquireCustomerProfile
            "
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.wantAcquireCustomerProfile'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
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
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.fundamentalInformation.aspiration"
            style-set="outlined"
            :max-length="500"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.fundamental.labels.aspiration'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.fundamental.explanations.aspiration'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.fundamental.usageHistory')
        "
      >
        <template #uneditable>
          <!-- プログラム利用履歴 -->
          <div
            v-if="
              fundamental.usageHistory.length > 0 &&
              fundamental.usageHistory[0].npfProjectId
            "
            class="usage-history"
          >
            <ul
              v-for="item in fundamental.usageHistory"
              :key="item.npfProjectId"
            >
              <li class="master-karte-text__link master-karte-text">
                <a :href="'/master-karte/' + item.npfProjectId">
                  {{ projectName(item.serviceType, item.projectName) }}
                </a>
              </li>
            </ul>
          </div>
        </template>
      </MasterKarteDetailRow>
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
} from '~/models/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { Textarea } from '~/components/common/atoms'
import {
  RadioGroup,
  Select,
  AutoComplete,
} from '~/components/common/atoms/index'

export default CommonDetailContainer.extend({
  components: {
    Select,
    RadioGroup,
    DetailContainer,
    MasterKarteDetailRow,
    Textarea,
    AutoComplete,
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    /** 当期支援内容のチェックボックスがチェック済か */
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    localParam: {
      type: Object,
      default: () => ({}),
    },
    selectBoxItems: {
      type: Array as PropType<GetSelectBoxResponse[]>,
      default: () => [],
    },
  },
  data() {
    return {
      serviceTypes: [],
      searchInput: '',
    }
  },
  methods: {
    /** サービス種別とプロジェクト名を合わせて表示する **/
    projectName(serviceType: string, projectName: string): string {
      return `${serviceType}_${projectName}`
    },
    // ラインナップの表示を整形する
    lineupFormat(lineup: string | string[]) {
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
    /** カスタマーサクセスリユースのサブテキスト表示 **/
    reuseSubText(customerSuccessReuse: boolean) {
      const customerSuccessReuseLabels = this.customerSuccessReuseLabels
      this.$logger.info(customerSuccessReuse)
      return customerSuccessReuse
        ? customerSuccessReuseLabels[1]
        : customerSuccessReuseLabels[0]
    },
  },
  computed: {
    fundamental(): FundamentalInformation {
      let program = this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
        : this.masterKarteProject.nextProgram.fundamentalInformation
      if (program === null) {
        program = new FundamentalInformationClass()
      }
      return program
    },
    /** 次期支援選択時に右に表示される当期支援 */
    subtext(): FundamentalInformation {
      return this.showCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
        : new FundamentalInformationClass()
    },
    /** お客様ロールかどうか **/
    isCustomer(): boolean {
      return meStore.role === ENUM_USER_ROLE.CUSTOMER
    },
    /**
     * 編集可能かどうか
     * 案件を担当している支援者、支援者責任者、営業ロール、事業者責任者のみ編集可能
     **/
    editableRole(): boolean {
      if (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.SALES_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
      ) {
        // 担当案件
        if (meStore.projectIds?.includes(currentPageDataStore.projectId)) {
          return true
        }
      }
      return false
    },
    /** サービス区分のセレクトボックスのアイテム */
    serviceTypeValues(): any[] {
      return this.selectBoxItems.length > 0
        ? this.selectBoxItems.filter((item: any) => item.name === 'service')[0]
            .items
        : [new SelectBoxClass()]
    },
    /** ラインナップのセレクトボックスのアイテム */
    lineupValues(): SelectBox[] {
      return this.selectBoxItems.length > 0
        ? this.selectBoxItems?.filter(
            (box: GetSelectBoxResponse) => box.name === 'lineup'
          )[0].items
        : [new SelectBoxClass()]
    },
    /** カスタマーサクセスリユースのラジオボタンアイテム */
    customerSuccessReuseLabels(): string[] {
      if (this.selectBoxItems.length > 0) {
        const customerSuccessReuseItems = this.selectBoxItems?.filter(
          (box: GetSelectBoxResponse) => box.name === 'customerSuccessReuse'
        )
        return [
          customerSuccessReuseItems[0].items[1].label, // 非該当
          customerSuccessReuseItems[0].items[0].label, // 該当
        ]
      }
      return []
    },
    /** カスタマーサクセスリユースのラジオボタンアイテム */
    customerSuccessReuseValues(): boolean[] {
      if (this.selectBoxItems.length > 0) {
        const customerSuccessReuseItems = this.selectBoxItems?.filter(
          (box: GetSelectBoxResponse) => box.name === 'customerSuccessReuse'
        )
        return [
          JSON.parse(customerSuccessReuseItems[0].items[1].value), // 非該当
          JSON.parse(customerSuccessReuseItems[0].items[0].value), // 該当
        ]
      }
      return []
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
    /** 今回・次回のカスタマーサクセスリユースの表示フォーマット */
    reuseText() {
      const customerSuccessReuseItems = this.selectBoxItems?.filter(
        (box: GetSelectBoxResponse) => box.name === 'customerSuccessReuse'
      )
      const customerSuccessReuse = this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.fundamentalInformation
            ?.customerSuccessReuse || ''
        : this.masterKarteProject.nextProgram.fundamentalInformation
            ?.customerSuccessReuse || ''
      if (customerSuccessReuseItems.length > 0) {
        return customerSuccessReuse
          ? customerSuccessReuseItems[0].items[0].label
          : customerSuccessReuseItems[0].items[1].label
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

<style lang="scss">
.lineup {
  width: 100%;
  .v-input__slot {
    width: 100%;
    height: 32px !important;
    max-height: 32px !important;
  }

  .v-input__append-inner {
    margin-top: 4px !important;
  }

  .v-select__selections {
    width: 100%;
    flex-wrap: nowrap !important;
  }
}
.usage-history {
  white-space: normal;
}
</style>
