<template>
  <div>
    <MasterKarteDetailSection
      :title="
        $t('master-karte.pages.detail.container.result.projectResultTitle')
      "
    >
      <!-- カスタマーサクセス結果 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.result.customerSuccessResult.name'
          )
        "
        :text="result.customerSuccessResult"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <RadioGroup
            v-model="localParam.currentProgram.result.customerSuccessResult"
            required
            :labels="
              $t(
                'master-karte.pages.detail.container.result.customerSuccessResult.radio'
              ).labels
            "
            :values="
              $t(
                'master-karte.pages.detail.container.result.customerSuccessResult.radio'
              ).values
            "
            horizontal
          />
        </template>
      </MasterKarteDetailRow>

      <!-- カスタマーサクセス達成/未達要因 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.result.customerSuccessResultFactor'
          )
        "
        :text="result.customerSuccessResultFactor"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <Textarea
            v-if="editableRole"
            v-model="
              localParam.currentProgram.result.customerSuccessResultFactor
            "
            style-set="outlined"
            :max-length="500"
            aria-label="カスタマーサクセス達成/未達要因"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 支援で生じた課題 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.result.supportIssue')"
        :text="result.supportIssue"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <Textarea
            v-if="editableRole"
            v-model="localParam.currentProgram.result.supportIssue"
            style-set="outlined"
            :max-length="500"
            aria-label="支援で生じた課題"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 解決できた要因/解決できなかった要因 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.supportSuccessFactor')
        "
        :text="result.supportSuccessFactor"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <Textarea
            v-if="editableRole"
            v-model="localParam.currentProgram.result.supportSuccessFactor"
            style-set="outlined"
            :max-length="500"
            aria-label="解決できた要因/解決できなかった要因"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 顧客評価に対する自己評価 -->
      <MasterKarteDetailRow
        v-show="isVisibleSupportingInfoAndSurveyInfo"
        :label="
          $t('master-karte.pages.detail.container.result.surveySsapAssessment')
        "
        :text="result.surveySsapAssessment"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <Textarea
            v-if="editableRole"
            v-model="localParam.currentProgram.result.surveySsapAssessment"
            style-set="outlined"
            :max-length="500"
            aria-label="顧客評価に対する自己評価"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 次期支援内容 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.nextSupportContent')
        "
        :text="result.nextSupportContent"
        icon="void"
      >
        <template v-if="editableRole" #editable>
          <Textarea
            v-if="editableRole"
            v-model="localParam.currentProgram.result.nextSupportContent"
            style-set="outlined"
            :max-length="500"
            aria-label="次期支援内容"
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 修了時アンケート -->
      <MasterKarteDetailRow
        v-show="isVisibleCompletionSurveyInfo"
        :subtitle="
          $t('master-karte.pages.detail.container.result.completedSurvey')
        "
        :label="
          $t('master-karte.pages.detail.container.result.completedSurveyDetail')
        "
        icon="void"
      >
        <template #uneditable>
          <span class="master-karte-text__link">
            <a
              v-if="masterKarteProject.currentProgram.result.surveyId"
              :href="completedSurveyUrl"
              >{{
                $t(
                  'master-karte.pages.detail.container.result.displayCompletedSurveyDetail'
                )
              }}</a
            >
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 満足度5段階評価 -->
      <MasterKarteDetailRow
        v-show="isVisibleCompletionSurveyInfo"
        :label="
          $t('master-karte.pages.detail.container.result.completedSurveyReview')
        "
        icon="void"
      >
        <!-- 満足度5段階評価 -->
        <template #uneditable>
          <v-radio-group
            v-if="result.satisfactionEvaluation"
            v-model="selectedRadio"
            row
            readonly
          >
            <v-radio
              v-for="radio in result.satisfactionEvaluation"
              :key="radio.title"
              :label="radio.title"
              :value="radio.title.slice(0, 1)"
              class="satisfaction__radio"
              disabled
              active-class="is-active"
            >
            </v-radio>
          </v-radio-group>
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
  Result,
  GetMasterKarteByIdResponse,
  GetSelectBoxResponse,
  SelectBox,
} from '~/types/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import {
  GetSelectBoxResponseClass,
  UpdateMasterKarteResponseClass,
} from '~/models/MasterKarte'
import { Select, Textarea, RadioGroup } from '~/components/common/atoms'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
    Textarea,
    RadioGroup,
    Select,
  },
  props: {
    /** 修正をしていないマスターカルテ詳細レスポンス */
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    /** 修正を行うマスターカルテ更新リクエスト */
    localParam: {
      type: Object as PropType<UpdateMasterKarteResponseClass>,
    },
    selectBoxItems: {
      type: Array as PropType<GetSelectBoxResponse[]>,
      default: new GetSelectBoxResponseClass(),
    },
  },
  computed: {
    /** ラジオボタンの選択状態 **/
    selectedRadio(): number | string {
      let tmp = ''
      this.result.satisfactionEvaluation &&
        this.result.satisfactionEvaluation.forEach((item: any) => {
          if (item.isAnswer === true) {
            tmp = item.title.slice(0, 1)
          }
        })
      return tmp
    },
    result(): Result {
      return this.masterKarteProject.currentProgram.result
    },
    /** 担当案件か否か **/
    isAssignedProject(): boolean {
      return meStore.projectIds?.includes(this.masterKarteProject.ppProjectId)
    },
    /**
     * 下記項目を閲覧可能にするかどうか
     * ・支援で生じた課題、解決できた要因/解決できなかった要因、Sony Acceleration Platform自己評価
     * 以下アクセス不可の条件
     * 1. 支援者・営業担当者かつ非担当案件
     * 2. 支援者責任者かつ非所属課案件
     */
    isVisibleSupportingInfoAndSurveyInfo(): boolean {
      if (
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER
      ) {
        // 支援者・営業担当者
        return this.isAssignedProject
      }
      return true
    },
    /**
     * 修了時アンケート情報を閲覧可能にするかどうか
     * 以下アクセス不可の条件
     * 1. 支援者・営業担当者かつ非担当案件
     * 2. 支援者かつ担当案件かつ開示NGアンケート
     */
    isVisibleCompletionSurveyInfo(): boolean {
      if (meStore.role === ENUM_USER_ROLE.SALES) {
        // 営業担当者
        return this.isAssignedProject
      } else if (meStore.role === ENUM_USER_ROLE.SUPPORTER) {
        // 支援者
        return (
          this.isAssignedProject &&
          this.masterKarteProject.currentProgram.result.isDisclosure
        )
      }
      return true
    },
    /** 修了アンケートのURL **/
    completedSurveyUrl(): string {
      return `/survey/${this.masterKarteProject.currentProgram.result.surveyId}`
    },
    /**
     * 編集可能かどうか
     * 案件を担当している支援者、営業ロールのみ編集可能
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
    /** Sony Acceleration Platform自己評価のセレクトボックスのアイテム */
    supportValues(): SelectBox[] {
      // @ts-ignore
      return this.selectBoxItems.filter(
        (item: any) => item.name === 'supportResult'
      )[0].items
    },
  },
})
</script>

<style lang="scss" scoped>
.master-karte-text__link {
  a {
    color: #008a19;
    font-weight: bold;
    text-decoration: underline;
  }
}

.satisfaction__radio::v-deep label {
  font-size: 14px;
  color: #8f8f8f;
}

.satisfaction__radio::v-deep .v-icon {
  color: #8f8f8f;
}

.is-active::v-deep label,
.is-active::v-deep .v-input--selection-controls__input .v-icon {
  color: #1867c0 !important;
  font-weight: bold;
}
</style>
