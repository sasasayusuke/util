<template>
  <Card
    class="o-common-schedule-rows"
    :class="{ 'is-plan': disabled }"
    :to="!disabled ? `${linkPrefix}/${schedule.karteId}` : ''"
    :style-set="!disabled ? 'hover' : ''"
  >
    <v-container>
      <v-row>
        <!-- 支援状況 -->
        <v-col
          class="o-common-schedule-rows__label"
          cols="auto"
          align-self="center"
        >
          <Chip v-if="status === 'progress'" style-set="primary-70">
            {{ $t('common.label.progress') }}
          </Chip>
          <Chip v-if="status === 'performed'" style-set="secondary-70">
            {{ $t('common.label.performed') }}
          </Chip>
          <Chip v-if="status === 'plan'" style-set="primary-70" outlined>
            {{ $t('common.label.plan') }}
          </Chip>
        </v-col>
        <!-- 支援実施日 -->
        <v-col
          class="o-common-schedule-rows__date"
          cols="auto"
          align-self="center"
        >
          {{ date }}
        </v-col>
        <v-col
          class="o-common-schedule-rows__time"
          cols="2"
          align-self="center"
        >
          <!-- {{ min2hour(schedule.startTime) }}〜{{ min2hour(schedule.endTime) }} -->
          {{ schedule.startTime }}〜{{ schedule.endTime }}
        </v-col>
        <!-- 個別カルテ公開状態 -->
        <v-col
          class="o-common-schedule-rows__public"
          cols="3"
          align-self="center"
        >
          <!-- お客様ロール以外の場合 -->
          <div v-if="!isCustomer">
            <!-- お客様向け公開 -->
            <v-row v-if="!schedule.isDraft" align-content="center">
              <v-col
                cols="5"
                align-self="center"
                class="d-flex justify-end pr-1"
              >
                {{ $t('karte.pages.detail.selects') }}
              </v-col>
              <v-col cols="1" align-self="center" class="px-0">
                <Icon size="22">mdi-eye</Icon>
              </v-col>
              <v-col cols="6" align-self="center" class="pl-1">{{
                $t('common.detail.public')
              }}</v-col>
            </v-row>
            <!-- お客様向け非公開 -->
            <v-row v-else class="is-private" align-content="center">
              <v-col
                cols="5"
                align-self="center"
                class="d-flex justify-end pr-1"
              >
                {{ $t('karte.pages.detail.selects') }}
              </v-col>
              <v-col cols="1" align-self="center" class="px-0">
                <Icon size="22">mdi-eye-off-outline</Icon>
              </v-col>
              <v-col cols="6" align-self="center" class="pl-1">{{
                $t('common.detail.private')
              }}</v-col>
            </v-row>
          </div>
          <!-- お客様ロールの場合 -->
          <div v-else>
            <v-row v-if="!schedule.isDraft" align-content="center">
              <v-col cols="auto" align-self="center" class="pr-0">
                <Icon size="22">mdi-eye</Icon>
              </v-col>
              <v-col class="pl-2">{{ $t('common.detail.public') }}</v-col>
            </v-row>
            <v-row v-else align-content="center" class="is-private">
              <v-col cols="auto" align-self="center" class="pr-0">
                <Icon size="22">mdi-eye-off-outline</Icon>
              </v-col>
              <v-col class="pl-2">{{ $t('common.detail.private') }}</v-col>
            </v-row>
          </div>
        </v-col>
        <!-- 最終更新日時・最終更新者 -->
        <v-col class="o-common-schedule-rows__update" align-self="center">
          <v-row>
            <v-col cols="auto" align-self="center" class="pr-0">
              <Icon size="22">mdi-pencil</Icon>
            </v-col>
            <v-col class="pl-2">
              <OverflowTooltip :text="lastUpdated" :max="38" />
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </Card>
</template>

<script lang="ts">
import { format, parse } from 'date-fns'
import ja from 'date-fns/locale/ja'
import { getCurrentDate } from '~/utils/common-functions'
import { Chip, Card, Icon } from '~/components/common/atoms/index'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import BaseComponent from '~/common/BaseComponent'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'
export default BaseComponent.extend({
  components: {
    Chip,
    Card,
    Icon,
    OverflowTooltip,
  },
  computed: {
    status() {
      const schedule = this.schedule
      // 日付の形式を再フォーマット
      const tempStartTime = schedule.startTime.split(':')
      const tempEndTime = schedule.endTime.split(':')
      schedule.startTime = `${String(tempStartTime[0]).trim()}:${String(
        tempStartTime[1]
      ).trim()}`
      schedule.endTime = `${String(tempEndTime[0]).trim()}:${String(
        tempEndTime[1]
      ).trim()}`
      // 比較とキャスト用にDateTimeフォーマットのStringデータ作成
      const stringStartDate = String(schedule.date + ' ' + schedule.startTime)
      const stringEndDate = String(schedule.date + ' ' + schedule.endTime)
      // 比較用にDate.getTime()で値を取得
      const startTime = new Date(stringStartDate).getTime()
      const endTime = new Date(stringEndDate).getTime()
      // 現在時刻
      const currentTime = getCurrentDate().getTime()
      // ステータス判定
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
    date(): string {
      const scheduleDate = parse(
        this.schedule.date,
        this.$t('common.format.date_ymd5') as string,
        getCurrentDate()
      )
      const dateFormat = this.$t('karte.pages.list.schedule.date') as string
      return format(scheduleDate, dateFormat, {
        locale: ja,
      })
    },
    lastUpdated(): string {
      const schedule = this.schedule
      if (!schedule.lastUpdateDatetime && !schedule.updateUser) {
        return this.$t('karte.pages.list.schedule.lastUpdate') + 'ー'
      } else {
        let lastUpdateDatetime: string
        if (schedule.lastUpdateDatetime) {
          lastUpdateDatetime =
            this.formatDate(new Date(schedule.lastUpdateDatetime), 'Y/MM/DD ') +
            '　' +
            schedule.updateUser
        } else {
          lastUpdateDatetime = '' + schedule.updateUser
        }
        return (
          this.$t('karte.pages.list.schedule.lastUpdate') + lastUpdateDatetime
        )
      }
    },
    isCustomer(): boolean {
      return hasRole([ENUM_USER_ROLE.CUSTOMER])
    },
  },
  props: {
    schedule: {
      type: Object,
      required: true,
    },
    linkPrefix: {
      type: String,
      default: '',
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss" scoped>
.o-common-schedule-rows {
  .container {
    max-width: 1200px;
  }
  &.is-plan {
    .o-common-schedule-rows__date {
      color: $c-black-20;
      text-decoration: none;
    }
  }
}
.o-common-schedule-rows__label {
  padding: 22px 0 22px 30px;
}
.o-common-schedule-rows__date {
  @include fontSize($size: 'xlarge');
  color: $c-primary-dark;
  font-weight: bold;
  text-decoration: underline;
}
.o-common-schedule-rows__time {
  @include fontSize($size: 'normal');
  font-weight: bold;
}
.o-common-schedule-rows__update {
  @include fontSize($size: 'small');
  line-height: 1.285;
  .row {
    align-items: center;
  }
}
.o-common-schedule-rows__public {
  @include fontSize($size: 'small');
  &.is-draft {
    color: #8f8f8f;
    .v-icon {
      color: #8f8f8f;
    }
  }
}
.is-private {
  color: #8f8f8f;
  .v-icon {
    color: #8f8f8f;
  }
}
</style>
