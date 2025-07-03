<template>
  <DetailContainer
    :title="date"
    :is-editing="false"
    :is-hide-button1="true"
    @click:negative="$emit('click:negative')"
  >
    <template #term>
      {{ term }}
    </template>
    <!-- タイトル左のラベル -->
    <template #showStatus>
      <div class="public-private-select">
        <span>
          {{ $t('karte.pages.detail.selects') }}
        </span>
        <span class="pr-5" :class="karte.isDraft ? 'private' : ''">
          {{
            karte.isDraft
              ? $t('common.detail.private')
              : $t('common.detail.public')
          }}
        </span>
      </div>
    </template>
    <template #uniqueButtons>
      <Button style-set="detailHeaderNegative" @click="$emit('click:negative')">
        {{ $t('common.button.back') }}
      </Button>
    </template>
    <template #label>
      <Chip v-if="status === 'progress'" style-set="primary-70" class="mr-1">
        {{ $t('common.label.progress') }}
      </Chip>
      <Chip v-if="status === 'performed'" style-set="secondary-70" class="mr-1">
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
    <!-- 最終更新情報 -->
    <LastUpdate
      :term="`${project.supportDateFrom} 〜 ${project.supportDateTo}`"
      :user="karte.updateUserName"
      :date="karte.lastUpdateDatetime"
      class="mt-4 px-8"
      :support-term="true"
    />
    <!-- =============================================
      メインコンテンツ
    ============================================= -->
    <v-container class="font-size-small" fluid pt-4 px-8>
      <!-- カスタマーサクセス -->
      <KarteDetailRow
        :title="$t('karte.pages.detail.customerSuccess')"
        :project-id="project.id"
        is-no-under-line
        class="customer-success mt-3"
      >
        <!-- eslint-disable vue/no-v-html -->
        <Sheet
          style-set="text-box"
          class="break-word"
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
      </KarteDetailRow>
      <v-row no-gutters>
        <v-col cols="9" class="pr-10">
          <!-- お客様 -->
          <KarteDetailRow
            :title="$t('karte.pages.detail.customers')"
            is-no-under-line
            class="pb-0"
          >
            <p>
              {{ karteCustomersText }}
            </p>
          </KarteDetailRow>
          <!-- SAP支援チーム -->
          <KarteDetailRow :title="$t('karte.pages.detail.supportTeam')">
            <p>
              {{ karteSupportTeamText }}
            </p>
          </KarteDetailRow>
          <!-- 場所 -->
          <KarteDetailRow
            :title="$t('karte.pages.detail.location.name')"
            class="karte-location"
          >
            {{ locationValue() }}
          </KarteDetailRow>
          <!-- 支援実施内容 -->
          <KarteDetailRow :title="$t('karte.pages.detail.detail')">
            <p
              v-for="(text, index) in karteDetailText.split('\n')"
              :key="index"
            >
              {{ text }}
            </p>
          </KarteDetailRow>
          <!-- フィードバック -->
          <KarteDetailRow :title="$t('karte.pages.detail.feedback')">
            <p
              v-for="(text, index) in karteFeedbackText.split('\n')"
              :key="index"
            >
              {{ text }}
            </p>
          </KarteDetailRow>
          <!-- ネクストアクション -->
          <KarteDetailRow :title="$t('karte.pages.detail.homework')">
            <p
              v-for="(text, index) in karteHomeworkText.split('\n')"
              :key="index"
            >
              {{ text }}
            </p>
          </KarteDetailRow>
          <KarteAdminContainer>
            <!-- 実施時間 -->
            <KarteDetailRow
              hx="h3"
              :title="$t('karte.pages.detail.manHour')"
              is-short
            >
              {{ karte.startSupportActualTime }}
              {{
                karte.startSupportActualTime || karte.endSupportActualTime
                  ? '〜'
                  : ''
              }}
              {{ karte.endSupportActualTime }}&nbsp;&nbsp;&nbsp;({{
                $t('karte.pages.detail.total')
              }}&nbsp;{{ karte.manHour }}&nbsp;h)
            </KarteDetailRow>
            <!-- 現状の課題・所感・申し送り -->
            <KarteDetailRow
              hx="h3"
              :title="$t('karte.pages.detail.task')"
              is-short
            >
              <p
                v-for="(text, index) in karteTaskText.split('\n')"
                :key="index"
              >
                {{ text }}
              </p>
            </KarteDetailRow>
            <!-- お客様に不足している人的リソース -->
            <KarteDetailRow
              hx="h3"
              :title="$t('karte.pages.detail.humanResourceNeededForCustomer')"
              is-short
            >
              <p
                v-for="(
                  text, index
                ) in karteHumanResourceNeededForCustomerText.split('\n')"
                :key="index"
              >
                {{ text }}
              </p>
            </KarteDetailRow>
            <!-- お客様に紹介したい企業や産業 -->
            <KarteDetailRow
              hx="h3"
              :title="
                $t('karte.pages.detail.companyAndIndustryRecommendedToCustomer')
              "
              is-short
            >
              <p
                v-for="(
                  text, index
                ) in karteCompanyAndIndustryRecommendedToCustomerText.split(
                  '\n'
                )"
                :key="index"
              >
                {{ text }}
              </p>
            </KarteDetailRow>
            <!-- SAP支援チームに補充したい人的リソース-->
            <KarteDetailRow
              hx="h3"
              :title="$t('karte.pages.detail.humanResourceNeededForSupport')"
              is-short
            >
              <p
                v-for="(
                  text, index
                ) in karteHumanResourceNeededForSupportText.split('\n')"
                :key="index"
              >
                {{ text }}
              </p>
            </KarteDetailRow>
            <!-- マネジメントへのリクエスト -->
            <KarteDetailRow
              hx="h3"
              :title="$t('karte.pages.detail.memo')"
              is-short
            >
              <p
                v-for="(text, index) in karteMemoText.split('\n')"
                :key="index"
              >
                {{ text }}
              </p>
            </KarteDetailRow>
          </KarteAdminContainer>
        </v-col>
        <v-col>
          <!-- 資料添付 -->
          <KarteAsideLink
            :title="$t('karte.pages.detail.documents')"
            :links="karte.documents"
            is-download
            class="mt-10"
          />
          <!-- 成果物添付 -->
          <KarteAsideLink
            :title="$t('karte.pages.detail.deliverables')"
            :links="karte.deliverables"
            is-download
            class="mt-10"
          />
          <!-- 関連アンケート -->
          <KarteAsideLink
            :title="$t('karte.pages.detail.surveys')"
            :links="surveys"
            :project="project"
            :karte="karte"
            :is-allow-surveys="isAllowSurveys"
            class="mt-10"
          />
        </v-col>
      </v-row>
    </v-container>

    <template #footerButton>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:negative')"
      >
        {{ $t('common.button.back') }}
      </Button>
    </template>
  </DetailContainer>
</template>

<script lang="ts">
import { format } from 'date-fns'
import ja from 'date-fns/locale/ja'
import { getCurrentDate } from '~/utils/common-functions'
import {
  Chip,
  Sheet,
  Title,
  Icon,
  Button,
} from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import KarteDetailRow from '~/components/karte/molecules/KarteDetailRow.vue'
import KarteAdminContainer from '~/components/karte/molecules/KarteAdminContainer.vue'
import KarteAsideLink from '~/components/karte/molecules/KarteAsideLink.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import { GetKarteByIdResponse } from '~/models/Karte'
import type { PropType } from '~/common/BaseComponent'
import { SurveyListItem } from '~/models/Survey'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { hasRole } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  name: 'KarteDetailContainer',
  components: {
    Chip,
    Sheet,
    Title,
    Icon,
    Button,
    DetailContainer,
    LastUpdate,
    KarteDetailRow,
    KarteAdminContainer,
    KarteAsideLink,
  },
  computed: {
    /**
     * 関連アンケートを表示するか判定
     * @returns 関連アンケートを表示するかの真偽値
     */
    isAllowSurveys() {
      // ManHourOpsロール以外の権限を所持しているアカウントの場合は関連アンケートを表示
      const allowRoles: string[] = []
      for (const i in ENUM_ADMIN_ROLE) {
        if (ENUM_ADMIN_ROLE[i] !== ENUM_ADMIN_ROLE.MAN_HOUR_OPS) {
          allowRoles.push(ENUM_ADMIN_ROLE[i])
        }
      }
      return hasRole(allowRoles)
    },
    /**
     * カルテAPIレスポンスのcustomersプロパティが文字列であれば返す
     */
    karteCustomersText(): string {
      if (typeof this.karte.customers !== 'string') {
        return ''
      } else {
        return this.karte.customers
      }
    },
    /**
     * カルテAPIレスポンスのsupportTeamプロパティが文字列であれば返す
     */
    karteSupportTeamText(): string {
      if (typeof this.karte.supportTeam !== 'string') {
        return ''
      } else {
        return this.karte.supportTeam
      }
    },
    /**
     * カルテAPIレスポンスのdetailプロパティが文字列であれば返す
     */
    karteDetailText(): string {
      if (typeof this.karte.detail !== 'string') {
        return ''
      } else {
        return this.karte.detail
      }
    },
    /**
     * カルテAPIレスポンスのfeedbackプロパティが文字列であれば返す
     */
    karteFeedbackText(): string {
      if (typeof this.karte.feedback !== 'string') {
        return ''
      } else {
        return this.karte.feedback
      }
    },
    karteHomeworkText(): string {
      if (typeof this.karte.homework !== 'string') {
        return ''
      } else {
        return this.karte.homework
      }
    },
    karteMemoText(): string {
      if (typeof this.karte.memo !== 'string') {
        return ''
      } else {
        return this.karte.memo
      }
    },
    karteTaskText(): string {
      if (typeof this.karte.task !== 'string') {
        return ''
      } else {
        return this.karte.task
      }
    },
    karteHumanResourceNeededForCustomerText(): string {
      if (typeof this.karte.humanResourceNeededForCustomer !== 'string') {
        return ''
      } else {
        return this.karte.humanResourceNeededForCustomer
      }
    },
    karteCompanyAndIndustryRecommendedToCustomerText(): string {
      if (
        typeof this.karte.companyAndIndustryRecommendedToCustomer !== 'string'
      ) {
        return ''
      } else {
        return this.karte.companyAndIndustryRecommendedToCustomer
      }
    },
    karteHumanResourceNeededForSupportText(): string {
      if (typeof this.karte.humanResourceNeededForSupport !== 'string') {
        return ''
      } else {
        return this.karte.humanResourceNeededForSupport
      }
    },
    date(): string {
      let date = ''
      if (this.karte.date) {
        const dateFormat = this.$t('karte.pages.list.schedule.date') as string
        date = format(new Date(this.karte.date), dateFormat, {
          locale: ja,
        }) as string
      }
      return date
    },
    term(): string {
      const startTime: string = this.karte.startTime
      const endTime: string = this.karte.endTime
      return startTime && endTime ? `${startTime} 〜 ${endTime}` : ''
    },
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
  },
  props: {
    /**
     * GetProjectById APIのレスポンス
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * GetKarteById APIのレスポンス
     */
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: true,
    },
    /**
     * GetSurveys APIのレスポンス
     */
    surveys: {
      type: Array as PropType<SurveyListItem[]>,
      required: true,
    },
  },
  methods: {
    locationValue() {
      if (!this.karte.location) {
        return ''
      }
      const labels = this.$t(
        'karte.pages.detail.location.radio.labels'
      ) as unknown as string[]
      const values = this.$t(
        'karte.pages.detail.location.radio.values'
      ) as unknown as string[]
      const index = values.findIndex((v) => v === this.karte.location.type)
      if (index < 0) {
        return ''
      }

      if (['client_office', 'other'].includes(this.karte.location.type)) {
        return `${labels[index]}（${this.karte.location.detail}）`
      } else {
        return labels[index]
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.break-word {
  word-break: break-word;
}
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}
.private {
  color: #308eef !important;
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
.is-master-karte {
  display: flex;
  gap: 16px;
}
.border-right {
  padding-right: 10px;
  border-right: #e5e5e5 3px solid;
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
