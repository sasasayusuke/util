<template>
  <div>
    <DetailContainer
      :title="date"
      :is-editing="isEditing"
      :is-karte="isKarte"
      :is-hide-required="true"
      :is-hide-button1="isCustomer || isSales || isSalesMgr || !regionalProject"
      :is-updating="isUpdating"
      :note-head="noteHead"
      is-hide-button2
      :is-valid="isValidWithChange"
      :is-attendees-valid="isAttendeesValid"
      :is-loading-button="isLoadingButton"
      @click:positive="$emit('click:positive', localParam)"
      @click:negative="$emit('click:negative')"
    >
      <template #term>
        {{ term }}
      </template>
      <!-- タイトル左のラベル -->
      <template #label>
        <Chip v-if="status === 'progress'" style-set="primary-70" class="mr-1">
          {{ $t('common.label.progress') }}
        </Chip>
        <Chip
          v-if="status === 'performed'"
          style-set="secondary-70"
          class="mr-1"
        >
          {{ $t('common.label.performed') }}
        </Chip>
        <Chip
          v-if="status === 'plan'"
          style-set="primary-70"
          outlined
          class="mr-1"
        >
          {{ $t('common.label.plan') }}
        </Chip>
      </template>
      <template #button>
        <!-- 個別カルテ入力画面 公開ステータス -->
        <template v-if="isEditing">
          <div class="is-karte is-master-karte">
            <Checkbox
              v-model="localParam.isNotifyUpdateKarte"
              :label="$t('karte.pages.list.header.checkbox.name')"
              :true-value="true"
              :false-value="false"
              hide-details
              class="pb-1 ml-4 pl-4 pr-6 a-checkbox--black border-right"
              @change="isChanged = true"
            />
            <div class="public-private-select">
              <span>{{
                $t('master-karte.pages.detail.container.save.customer')
              }}</span>
              <!-- お客様公開 -->
              <Select
                v-model="localParam.isDraft"
                dense
                outlined
                disabled
                :items="[
                  {
                    text: $t('common.detail.private'),
                    value: true,
                  },
                  {
                    text: $t('common.detail.public'),
                    value: false,
                  },
                ]"
                :class="localParam.isDraft ? 'private' : ''"
                @input="isChanged = true"
              />
            </div>
          </div>
        </template>
        <!-- 個別カルテ詳細画面 公開ステータス（お客様以外） -->
        <template v-else>
          <div v-if="!isCustomer" class="public-private-select">
            <span>
              {{ $t('karte.pages.detail.selects') }}
            </span>
            <span class="pr-5" :class="localParam.isDraft ? 'private' : ''">
              {{
                localParam.isDraft
                  ? $t('common.detail.private')
                  : $t('common.detail.public')
              }}
            </span>
          </div>
          <Sheet class="ml-8">
            <Button
              outlined
              style-set="small-tertiary detailHeaderNegative"
              @click="buttonAction2"
            >
              {{ $t('common.button.back') }}
            </Button>
          </Sheet>
        </template>
      </template>
      <!-- 最終更新情報 -->
      <LastUpdate
        :term="`${project.supportDateFrom} 〜 ${project.supportDateTo}`"
        :user="karte.draftSupporterName"
        :date="karte.lastUpdateDatetime"
        :support-term="true"
        :source-screen="'KarteDetail'"
        :is-public="localParam.isDraft ? false : true"
        :is-customer="isCustomer"
        class="mt-4 px-8"
      />
      <!-- =============================================
        メインコンテンツ
      ============================================= -->
      <v-container class="font-size-small" fluid pt-4 px-8>
        <v-form
          v-model="isValid"
          class="o-karte-detail-rows pt-5"
          @input="$listeners['input']"
        >
          <!-- カスタマーサクセス -->
          <v-row no-gutters class="o-karte-header">
            <v-col cols="12">
              <KarteDetailRow
                :title="$t('karte.pages.detail.customerSuccess')"
                :is-customer="isCustomer"
                :project-id="project.id"
                is-no-under-line
                class="customer-success mb-6"
              >
                <template #isNotEditing>
                  <!-- eslint-disable vue/no-v-html -->
                  <Sheet
                    style-set="text-box"
                    v-html="
                      $sanitize(
                        typeof project.customerSuccess === 'string'
                          ? project.customerSuccess.replace(/\r?\n/g, '<br />')
                          : project.customerSuccess
                      )
                    "
                  >
                  </Sheet>
                  <!-- eslint-enable -->
                </template>
              </KarteDetailRow>
            </v-col>
          </v-row>
          <v-row no-gutters class="o-karte-main">
            <v-col cols="9" class="pr-10">
              <!-- お客様 -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.customers.name')"
                :is-editing="isEditing"
                :value="karte.customers"
                is-no-under-line
                is-short
              >
                <TextField
                  v-model="localParam.customers"
                  style-set="outlined"
                  :placeholder="$t('karte.pages.detail.customers.placeholder')"
                  :max-length="120"
                  @input="isChanged = true"
                />
              </KarteDetailRow>

              <!-- SAP支援チーム -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.supportTeam.name')"
                :is-editing="isEditing"
                :value="karte.supportTeam"
                is-short
              >
                <TextField
                  v-model="localParam.supportTeam"
                  style-set="outlined"
                  :placeholder="
                    $t('karte.pages.detail.supportTeam.placeholder')
                  "
                  :max-length="120"
                  @input="isChanged = true"
                />
              </KarteDetailRow>
              <!-- 場所 -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.location.name')"
                :is-editing="isEditing"
                :value="locationValue()"
                class="karte-location"
              >
                <div style="width: 100%">
                  <RadioGroup
                    v-model="localParam.location.type"
                    :labels="$t('karte.pages.detail.location.radio.labels')"
                    :values="$t('karte.pages.detail.location.radio.values')"
                    horizontal
                    @change="locationRadioChange"
                  />
                  <TextField
                    v-model="localParam.location.detail"
                    :outlined="true"
                    :placeholder="$t('karte.pages.detail.location.placeholder')"
                    :max-length="100"
                    class="pt-2"
                    :rules="locationValidationCheck"
                    :required="locationRequire"
                    :disabled="!locationRequire"
                    @input="isChanged = true"
                  />
                </div>
              </KarteDetailRow>
              <!-- 支援実施内容 -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.detail.name')"
                :is-editing="isEditing"
                :value="karte.detail"
              >
                <Textarea
                  v-model="localParam.detail"
                  style-set="outlined"
                  :max-length="1000"
                  :placeholder="$t('karte.pages.detail.detail.placeholder')"
                  @input="isChanged = true"
                />
              </KarteDetailRow>
              <!-- フィードバック -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.feedback.name')"
                :is-editing="isEditing"
                :value="karte.feedback"
              >
                <Textarea
                  v-model="localParam.feedback"
                  style-set="outlined"
                  :max-length="1000"
                  :placeholder="$t('karte.pages.detail.feedback.placeholder')"
                  @input="isChanged = true"
                />
              </KarteDetailRow>
              <!-- ネクストアクション -->
              <KarteDetailRow
                :title="$t('karte.pages.detail.homework.name')"
                :is-editing="isEditing"
                :value="karte.homework"
              >
                <Textarea
                  v-model="localParam.homework"
                  style-set="outlined"
                  :max-length="1000"
                  :placeholder="$t('karte.pages.detail.homework.placeholder')"
                  @input="isChanged = true"
                />
              </KarteDetailRow>
              <KarteAdminContainer v-if="!isCustomer">
                <!-- 実施時間 -->
                <KarteDetailRow
                  hx="h3"
                  :title="$t('karte.pages.detail.manHour')"
                  is-short
                  :is-editing="isEditing"
                  :value="
                    karte.startSupportActualTime && karte.endSupportActualTime
                      ? `${karte.startSupportActualTime} 〜 ${
                          karte.endSupportActualTime
                        } （${$t('karte.pages.detail.total')} ${
                          karte.manHour
                        } h） `
                      : `（${$t('karte.pages.detail.total')} ${
                          karte.manHour
                        } h）`
                  "
                >
                  <v-container>
                    <v-row align="center">
                      <VueTimePicker
                        :value="localParam.startSupportActualTime"
                        :step="15"
                        @input="inputStartTime"
                      />
                      〜
                      <VueTimePicker
                        :value="localParam.endSupportActualTime"
                        :step="15"
                        @input="inputEndTime"
                      />
                      &emsp;（{{ $t('karte.pages.detail.total') }}&ensp;
                      <Sheet width="70">
                        <TextField
                          v-model="localParam.manHour"
                          role="textbox"
                          :aria-label="$t('karte.pages.detail.manHour')"
                          style-set="bgWhite"
                          number
                          :max-digits="2"
                          :range-number-from="minManHour"
                          :range-number-to="maxManHour"
                          @input="isChanged = true"
                        />
                      </Sheet>
                      &ensp;h）
                    </v-row>
                  </v-container>
                </KarteDetailRow>
                <!-- 現状の課題・所感・申し送り -->
                <KarteDetailRow
                  hx="h3"
                  :title="$t('karte.pages.detail.task.name')"
                  :is-editing="isEditing"
                  :value="karte.task"
                  is-short
                >
                  <Textarea
                    v-model="localParam.task"
                    style-set="outlined"
                    :placeholder="$t('karte.pages.detail.task.placeholder')"
                    :max-length="1000"
                    @input="isChanged = true"
                  />
                </KarteDetailRow>
                <!-- お客様に不足している人的リソース -->
                <KarteDetailRow
                  hx="h3"
                  :title="
                    $t('karte.pages.detail.humanResourceNeededForCustomer.name')
                  "
                  :is-editing="isEditing"
                  :value="karte.humanResourceNeededForCustomer"
                  is-short
                >
                  <Textarea
                    v-model="localParam.humanResourceNeededForCustomer"
                    style-set="outlined"
                    :rows="3"
                    :max-length="500"
                    :placeholder="
                      $t(
                        'karte.pages.detail.humanResourceNeededForCustomer.placeholder'
                      )
                    "
                    @input="isChanged = true"
                  />
                </KarteDetailRow>
                <!-- お客様に紹介したい企業や産業 -->
                <KarteDetailRow
                  hx="h3"
                  :title="
                    $t(
                      'karte.pages.detail.companyAndIndustryRecommendedToCustomer.name'
                    )
                  "
                  :is-editing="isEditing"
                  :value="karte.companyAndIndustryRecommendedToCustomer"
                  is-short
                >
                  <Textarea
                    v-model="localParam.companyAndIndustryRecommendedToCustomer"
                    style-set="outlined"
                    :rows="3"
                    :max-length="500"
                    :placeholder="
                      $t(
                        'karte.pages.detail.companyAndIndustryRecommendedToCustomer.placeholder'
                      )
                    "
                    @input="isChanged = true"
                  />
                </KarteDetailRow>
                <!-- SAP支援チームに補充したい人的リソース-->
                <KarteDetailRow
                  hx="h3"
                  :title="
                    $t('karte.pages.detail.humanResourceNeededForSupport.name')
                  "
                  :is-editing="isEditing"
                  :value="karte.humanResourceNeededForSupport"
                  is-short
                >
                  <Textarea
                    v-model="localParam.humanResourceNeededForSupport"
                    style-set="outlined"
                    :rows="3"
                    :max-length="500"
                    :placeholder="
                      $t(
                        'karte.pages.detail.humanResourceNeededForSupport.placeholder'
                      )
                    "
                    @input="isChanged = true"
                  />
                </KarteDetailRow>
                <!-- マネジメントへのリクエスト -->
                <KarteDetailRow
                  hx="h3"
                  :title="$t('karte.pages.detail.memo.name')"
                  :is-editing="isEditing"
                  :value="karte.memo"
                  is-short
                >
                  <Textarea
                    v-model="localParam.memo"
                    style-set="outlined"
                    :placeholder="$t('karte.pages.detail.memo.placeholder')"
                    :max-length="1000"
                    @input="isChanged = true"
                  />
                </KarteDetailRow>
              </KarteAdminContainer>
            </v-col>
            <v-col>
              <!-- 資料添付 -->
              <KarteAsideLink
                :title="$t('karte.pages.detail.documents')"
                :links="karte.documents"
                :is-editing="isEditing"
                :item="'Documents'"
                is-download
                is-deliverables
                class="mt-10"
                @clearFilesOnScreen="clearFilesOnScreen"
                @updateDocuments="
                  emitDocuments($event)
                  isChanged = true
                "
              />
              <!-- 成果物添付 -->
              <KarteAsideLink
                :title="$t('karte.pages.detail.deliverables')"
                :links="karte.deliverables"
                :is-editing="isEditing"
                :item="'Deliverables'"
                is-download
                is-deliverables
                class="mt-10"
                @clearFilesOnScreen="clearFilesOnScreen"
                @updateDeliverables="
                  emitDeliverables($event)
                  isChanged = true
                "
              />
              <!-- 関連アンケート -->
              <KarteAsideLink
                :title="$t('karte.pages.detail.surveys')"
                :surveys="surveys"
                :project="project"
                :karte="karte"
                :is-allow-surveys="isAllowSurveys"
                class="mt-10"
              />
            </v-col>
          </v-row>
          <!-- 詳細コンテナ内フッター -->
          <v-row
            no-gutters
            justify="center"
            align="center"
            class="o-karte-footer pt-10"
          >
            <v-col v-if="!isEditing" cols="auto">
              <Button
                style-set="large-tertiary"
                outlined
                width="160"
                @click="buttonAction2"
              >
                {{ $t('common.button.back') }}
              </Button>
            </v-col>
            <v-col
              v-if="!isCustomer && !isSales && !isSalesMgr && regionalProject"
              class="mr-7"
              cols="auto"
            >
              <Checkbox
                v-model="localParam.isNotifyUpdateKarte"
                :label="$t('karte.pages.list.header.checkbox.name')"
                :true-value="true"
                :false-value="false"
                hide-details
                class="pt-0 mt-0 pr-0 a-checkbox--black"
                @change="isChanged = true"
              />
            </v-col>
            <v-col
              v-if="!isCustomer && !isSales && !isSalesMgr && regionalProject"
              class="pl-0 pr-3"
              cols="auto"
            >
              <Sheet height="44">
                <v-divider height="44" vertical />
              </Sheet>
            </v-col>
            <v-col
              v-if="!isCustomer && !isSales && !isSalesMgr && regionalProject"
              class="mr-2"
              cols="auto"
            >
              <div class="public-private-select">
                <span>{{
                  $t('master-karte.pages.detail.container.save.customer')
                }}</span>
                <!-- お客様公開 -->
                <Select
                  v-model="localParam.isDraft"
                  dense
                  outlined
                  disabled
                  :items="[
                    {
                      text: $t('common.detail.private'),
                      value: true,
                    },
                    {
                      text: $t('common.detail.public'),
                      value: false,
                    },
                  ]"
                  style="width: 180px; height: 32px"
                  :class="localParam.isDraft ? 'private' : ''"
                  @input="isChanged = true"
                />
              </div>
            </v-col>
            <v-col
              v-if="!isCustomer && !isSales && !isSalesMgr && regionalProject"
              cols="auto pl-4"
            >
              <Button
                style-set="large-primary"
                width="160"
                :disabled="
                  !isValidWithChange ||
                  isUpdating ||
                  isReversalTime ||
                  !isAttendeesValid
                "
                :loading="isLoadingButton"
                @click="buttonAction1"
              >
                {{ $t('common.button.save2') }}
              </Button>
            </v-col>
          </v-row>
        </v-form>
      </v-container>
    </DetailContainer>
    <Modal v-if="isConfirmModalOpen">
      <p class="text-center mb-0" style="white-space: pre-wrap">
        {{ $t('master-karte.pages.detail.modal.message') }}
      </p>
      <v-row class="pt-4" justify="center">
        <Button
          outlined
          style-set="large-tertiary"
          @click="() => (isConfirmModalOpen = false)"
        >
          {{ $t('common.button.no') }}
        </Button>
        <Button
          class="ml-2"
          style-set="large-primary"
          @click="restoreTempKarte"
        >
          {{ $t('common.button.yes') }}
        </Button>
      </v-row>
    </Modal>
  </div>
</template>

<script lang="ts">
import { format } from 'date-fns'
import ja from 'date-fns/locale/ja'
import isEqual from 'lodash/isEqual'
import { getCurrentDate } from '~/utils/common-functions'
import {
  Button,
  Chip,
  Sheet,
  Title,
  Checkbox,
  Icon,
  Textarea,
  TextField,
  Select,
  RadioGroup,
} from '~/components/common/atoms/index'
import TimeSelect from '~/components/common/molecules/TimeSelect.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import KarteDetailRow from '~/components/karte/molecules/KarteDetailRow.vue'
import KarteAdminContainer from '~/components/karte/molecules/KarteAdminContainer.vue'
import KarteAsideCheckBox from '~/components/karte/molecules/KarteAsideCheckBox.vue'
import SupportersCheckBox from '~/components/karte/molecules/SupportersCheckBox.vue'
import KarteAsideLink from '~/components/karte/molecules/KarteAsideLink.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import {
  GetKarteByIdResponse,
  UpdateKarteByIdRequest as LocalKarte,
} from '~/models/Karte'
import { SurveyListItem } from '~/models/Survey'
import { deleteFile } from '~/utils/delete'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { hasRole } from '~/utils/role-authorizer'
import IndexedDB from '~/utils/indexedDb'
import Modal from '~/components/common/molecules/Modal.vue'
import { Tables } from '~/tables'
import VueTimePicker from '~/components/common/molecules/VueTimePicker.vue'

export { LocalKarte }

class LocalDocuments {
  path: string = ''
  fileName: string = ''
}

class LocalDeliverables {
  path: string = ''
  fileName: string = ''
}

export default BaseComponent.extend({
  components: {
    Button,
    Chip,
    Sheet,
    Title,
    Checkbox,
    Icon,
    Textarea,
    TextField,
    DetailContainer,
    LastUpdate,
    Select,
    KarteDetailRow,
    KarteAdminContainer,
    KarteAsideCheckBox,
    KarteAsideLink,
    SupportersCheckBox,
    TimeSelect,
    RadioGroup,
    Modal,
    VueTimePicker,
  },
  computed: {
    /**
     * 関連アンケートを表示するか判定
     * @returns 関連アンケートを表示するかの真偽値
     */
    isAllowSurveys(): boolean {
      // 支援者以外の権限または支援者かつ所属している案件の場合のみ関連アンケートを表示
      return (
        !hasRole([ENUM_USER_ROLE.SUPPORTER]) ||
        (hasRole([ENUM_USER_ROLE.SUPPORTER]) &&
          meStore.projectIds?.includes(this.project.id))
      )
    },
    /**
     * ファイル選択に変更があった場合はバリデーション結果を返す
     * @returns バリデーション結果とファイル選択に変更があったか否か
     */
    isValidWithChange(): boolean {
      if (this.isChanged) {
        return this.isValid
      } else {
        return false
      }
    },
    /**
     * 出席者情報に入力があるかをチェックし、バリデーション結果を返す
     * @returns お客様とSAP支援チームの両方に入力があるか否か
     */
    isAttendeesValid(): boolean {
      const hasCustomers = this.localParam.customers?.trim().length > 0
      const hasSupportTeam = this.localParam.supportTeam?.trim().length > 0
      return hasCustomers && hasSupportTeam
    },
    /**
     * フォーマット化された日付を生成
     * @returns フォーマット化された日付
     */
    date(): string {
      const dateFormat = this.$t('karte.pages.list.schedule.date') as string
      const date = format(new Date(this.karte.date), dateFormat, {
        locale: ja,
      }) as string
      return date
    },
    /**
     * 期間を表示
     * @returns 支援期間
     */
    term(): string {
      const startTime: string = this.karte.startTime
      const endTime: string = this.karte.endTime
      return startTime && endTime ? `${startTime} 〜 ${endTime}` : ''
    },
    /**
     * カルテのステータスを表示
     * @returns カルテのステータス
     */
    status(): string {
      const karte: GetKarteByIdResponse = this.karte as GetKarteByIdResponse
      // 日付の形式を再フォーマット
      const tempStartTime = karte.startTime.split(':')
      const tempEndTime = karte.endTime.split(':')
      karte.startTime = `${String(tempStartTime[0]).trim()}:${String(
        tempStartTime[1]
      ).trim()}`
      karte.endTime = `${String(tempEndTime[0]).trim()}:${String(
        tempEndTime[1]
      ).trim()}`
      // 比較とキャスト用にDateTimeフォーマットのStringデータ作成
      const stringStartDate = String(karte.date + ' ' + karte.startTime)
      const stringEndDate = String(karte.date + ' ' + karte.endTime)
      // 比較用にDate.getTime()で値を取得
      const startTime = new Date(stringStartDate).getTime()
      const endTime = new Date(stringEndDate).getTime()
      // 現在時刻
      const currentTime = getCurrentDate().getTime()

      let status: string = ''
      if (currentTime < startTime) {
        status = 'plan'
      } else if (currentTime >= startTime && currentTime < endTime) {
        status = 'progress'
      } else {
        status = 'performed'
      }
      return status
    },
    /**
     * 下書き、公開のセレクトボックス
     * @returns 下書き、公開のセレクトボックス
     */
    isDraftSelectItems(this: any): string[] {
      const items = []
      for (const i in this.$t('karte.pages.list.header').select) {
        const elm = this.$t('karte.pages.list.header').select[i]
        if (String(elm.value).toLowerCase() === 'true') {
          elm.value = true
        } else {
          elm.value = false
        }
        items.push(elm)
      }
      return items
    },
    // 実施時間の必須チェック
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      const rule = (value: string[]) => {
        return value.length > 0 || this.$t('common.rule.required')
      }
      rules.push(rule)
      return rules
    },
    locationValidationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.locationRequire) {
        const rule = (value: string[]) => {
          return value.length > 0 || this.$t('common.rule.required')
        }
        rules.push(rule)
      }

      const rule = (value: any) => {
        const length: number = 120
        return (
          value.length <= length || this.$t('common.rule.maxLength', { length })
        )
      }
      rules.push(rule)
      return rules
    },
  },
  props: {
    /**
     * プロジェクト情報
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * カルテ情報
     */
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: true,
    },
    /**
     * アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyListItem[]>,
      required: true,
    },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
    },
    /**
     * 営業ロールか否か
     */
    isSales: {
      type: Boolean,
      default: false,
    },
    /**
     * 営業責任ロールか否か
     */
    isSalesMgr: {
      type: Boolean,
      default: false,
    },
    /**
     * 編集か否か
     */
    isEditing: { type: Boolean },
    noteHead: {
      type: String,
      default: '',
    },
    /**
     * 更新中か否か
     */
    isUpdating: {
      type: Boolean,
      default: false,
    },
    /**
     * 一覧画面へ戻るボタンか否か
     */
    backToListButton: {
      type: Boolean,
      default: true,
    },
    /**
     * 支援者・支援者責任者の所属案件か否か
     */
    regionalProject: {
      type: Boolean,
    },
    /** カルテ更新中か否か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    //出席責任者にプロデューサーを追加
    if (this.project.mainSupporterUserId) {
      const mainSupporterUsers = {
        id: this.project.mainSupporterUserId,
        name: this.project.mainSupporterUserName,
      }
      if (this.project.supporterUsers === null) {
        this.project.supporterUsers = []
      }
      this.project.supporterUsers.unshift(mainSupporterUsers)
    }

    //出席お客様メンバーにお客様(代表)を追加
    if (this.project.mainCustomerUserId) {
      const mainCustomerUsers = {
        id: this.project.mainCustomerUserId,
        name: this.project.mainCustomerUserName,
      }
      if (this.project.customerUsers === null) {
        this.project.customerUsers = []
      }
      this.project.customerUsers.unshift(mainCustomerUsers)
    }

    // 支援実績時間（自）の初期表示に支援開始予定時間を設定
    if (this.localParam.startSupportActualTime === null) {
      this.localParam.startSupportActualTime = this.karte.startTime
    }

    // 支援実績時間（至）の初期表示に支援終了予定時間を設定
    if (this.localParam.endSupportActualTime === null) {
      this.localParam.endSupportActualTime = this.karte.endTime
    }

    this.locationRequire = ['client_office', 'other'].includes(
      this.localParam.location.type
    )
  },
  async mounted() {
    this.tempKarte = await this.indexedDB.get<LocalKarte>(
      Tables.karte,
      this.karte.karteId
    )
    if (this.tempKarte && this.isEditing) {
      this.isConfirmModalOpen = true
    }
  },
  data(): {
    localParam: LocalKarte
    localDocuments: LocalDocuments
    localDeliverables: LocalDeliverables
    value: string
    title: string
    isKarte: boolean
    keysToDeleteFiles: string[]
    isValid: boolean
    isChanged: boolean
    minManHour: number
    maxManHour: number
    isReversalTime: boolean
    locationRequire: boolean
    indexedDB: IndexedDB
    isConfirmModalOpen: boolean
    tempKarte: LocalKarte | undefined
  } {
    return {
      localParam: Object.assign(new LocalKarte(), this.karte),
      localDocuments: new LocalDocuments(),
      localDeliverables: new LocalDeliverables(),
      value: '',
      title: this.karte.startDatetime,
      isKarte: true,
      keysToDeleteFiles: [] as string[],
      isValid: true,
      isChanged: false,
      minManHour: 0,
      maxManHour: 99,
      isReversalTime: false,
      locationRequire: false,
      isConfirmModalOpen: false,
      indexedDB: new IndexedDB(meStore.id),
      tempKarte: undefined,
    }
  },
  watch: {
    localParam: {
      deep: true,
      handler: 'handleLocalParamChange',
    },
  },
  methods: {
    async handleLocalParamChange(val: any) {
      this.locationRequire = ['client_office', 'other'].includes(
        val?.location?.type
      )

      if (!isEqual(val, this.karte)) {
        await this.indexedDB.put(Tables.karte, val)
      }
    },
    /**
     * ×押下時に、保存時に削除するファイルのパスを保存する
     * @param key 削除するファイルのkey
     */
    clearFilesOnScreen(key: string): void {
      this.keysToDeleteFiles.push(key)
      this.isChanged = true
    },
    /**
     * 保存ボタン押下時にS3から削除する
     */
    clearFilesOnS3(): void {
      // keysToDeleteFiles配列をつかって一気に削除したい
      this.keysToDeleteFiles.forEach((keyToDeleteFile: string) => {
        deleteFile(keyToDeleteFile).then(() => {
          this.$logger.info('Files on S3 has been deleted')
        })
      })
    },
    /**
     * 添付資料をセットする
     * @param files 添付資料
     */
    emitDocuments(files: any): void {
      // @ts-ignore
      this.localDocuments = []
      for (const i in files) {
        // @ts-ignore
        this.$set(this.localDocuments, i, files[i])
      }
      // @ts-ignore
      this.$emit('updateDocuments', this.localDocuments)
    },
    /**
     * 成果物をセットする
     * @param files 成果物
     */
    emitDeliverables(files: any): void {
      // @ts-ignore
      this.localDeliverables = []
      for (const i in files) {
        // @ts-ignore
        this.$set(this.localDeliverables, i, files[i])
      }
      // @ts-ignore
      this.$emit('updateDeliverables', this.localDeliverables)
    },
    /**
     * 保存ボタン動作
     */
    buttonAction1(this: any): void {
      this.clearFilesOnS3()
      this.$emit('click:positive', this.localParam)
    },
    /**
     * 戻るボタン動作
     */
    buttonAction2(this: any): void {
      this.$emit('click:negative')
    },
    /**
     * 支援実績時間（自）を設定
     */
    inputStartTime(elm: string) {
      this.localParam.startSupportActualTime = elm
      this.timeReversalCheck(
        this.localParam.startSupportActualTime,
        this.localParam.endSupportActualTime
      )
      this.isChanged = true
    },
    /**
     * 支援実績時間（至）を設定
     */
    inputEndTime(elm: string) {
      this.localParam.endSupportActualTime = elm
      this.timeReversalCheck(
        this.localParam.startSupportActualTime,
        this.localParam.endSupportActualTime
      )
      this.isChanged = true
    },
    /**
     * 支援実績時間（自）と支援実績時間（至）に矛盾が生じていないか確認
     */
    timeReversalCheck(startTime: string, endTime: string) {
      const localStartHourMin = startTime.split(':')
      const localStartHour = String(localStartHourMin[0])
      const localStartMin = String(localStartHourMin[1])
      const localEndHourMin = endTime.split(':')
      const localEndHour = String(localEndHourMin[0])
      const localEndMin = String(localEndHourMin[1])
      const intStartHourMin = parseInt(`${localStartHour}${localStartMin}`)
      const intEndHourMin = parseInt(`${localEndHour}${localEndMin}`)
      if (intStartHourMin > intEndHourMin) {
        this.isReversalTime = true
      } else {
        this.isReversalTime = false
      }
    },
    /**
     * 場所のラジオボタン変更時の処理
     */
    locationRadioChange(newValue: string) {
      this.locationRequire = ['client_office', 'other'].includes(newValue)
      this.isChanged = !['client_office', 'other'].includes(newValue)
      this.localParam.location.detail = ''
    },
    locationValue() {
      const labels = this.$t(
        'karte.pages.detail.location.radio.labels'
      ) as unknown as string[]
      const values = this.$t(
        'karte.pages.detail.location.radio.values'
      ) as unknown as string[]
      const index = values.findIndex((v) => v === this.localParam.location.type)
      if (index < 0) {
        return ''
      }

      if (['client_office', 'other'].includes(this.localParam.location.type)) {
        return `${labels[index]}（${this.localParam.location.detail}）`
      } else {
        return labels[index]
      }
    },
    restoreTempKarte() {
      const tempKarte = this.tempKarte as LocalKarte
      this.localParam = tempKarte
      this.tempKarte = undefined
      this.isChanged = true
      this.isConfirmModalOpen = false
    },
  },
})
</script>

<style lang="scss" scoped>
.o-karte-main {
  flex-wrap: nowrap;
}
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}

.inside-footer {
  display: flex;
  align-items: center;
  gap: 15px;
}
.private {
  color: #308eef !important;
}
.private {
  .v-select__selections {
    color: #308eef !important;
  }
}
.public-private-select {
  display: flex;
  align-items: center;
  gap: 5px;
  span {
    @include fontSize('small');
    color: #666666;
    font-weight: bold;
  }
}
#checkboxLabel {
  color: black;
  font-size: 14px;
}
.is-master-karte {
  display: flex;
  gap: 16px;
}
.border-right {
  padding-right: 10px;
  border-right: #e5e5e5 3px solid;
}
.current-checkbox {
  height: 40px;
  display: flex;
  align-items: center;
}
.m-heading__button {
  .a-select {
    width: 180px !important;
    max-width: 100% !important;
  }
}
.customer-success {
  background-color: #ebf7ed;
  padding: 12px !important;
  border-radius: 4px;
  .v-sheet {
    background-color: #ebf7ed !important;
    padding: 0 !important;
  }
}
</style>
<style lang="scss">
.karte-location {
  .v-input__slot {
    min-height: 32px !important;
    font-size: 0.875rem !important;
  }
}
</style>
