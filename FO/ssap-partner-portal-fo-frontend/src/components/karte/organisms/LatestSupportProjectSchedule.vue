<template>
  <section>
    <v-container class="pa-0 ma-0 mb-2">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('project.pages.detail.latestKarten') }}
        </h2>
      </v-flex>
    </v-container>
    <v-container
      v-if="latestSupportProjectSchedule.length"
      class="pa-0 ma-0"
      style="max-width: 100% !important"
    >
      <Card elevation="3" class="latest-support-project-table">
        <SimpleTable>
          <template #default>
            <thead>
              <tr>
                <th class="text-left">ステータス</th>
                <th class="text-left">支援日</th>
                <th class="text-left">最終更新日時</th>
                <th class="text-left">最終更新者</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="item in formattedSchedules"
                :key="item.karteId"
                :class="{ 'no-link': !item.isAccessibleKarteDetail }"
                @click="
                  item.isAccessibleKarteDetail &&
                    $router.push(`/karte/${item.karteId}`)
                "
              >
                <td>
                  <Chip
                    v-if="item.status === 'progress'"
                    small
                    style-set="primary-60"
                  >
                    {{ $t('common.label.progress') }}
                  </Chip>
                  <Chip
                    v-else-if="item.status === 'performed'"
                    small
                    style-set="secondary-60"
                  >
                    {{ $t('common.label.performed2') }}
                  </Chip>
                  <Chip v-else small outlined style-set="secondary-60">
                    {{ $t('common.label.plan') }}
                  </Chip>
                </td>
                <td>
                  {{ item.supportDate }}
                </td>
                <td>{{ item.lastUpdateDatetime }}</td>
                <td>{{ item.updateUser }}</td>
                <td class="text-right">
                  <Icon size="16px">icon-org-arrow-right</Icon>
                </td>
              </tr>
            </tbody>
          </template>
        </SimpleTable>
      </Card>
    </v-container>
    <Alert v-else style-set="no_data">
      {{ $t('common.alert.no_data') }}
    </Alert>
  </section>
</template>

<script lang="ts">
import { isAfter, isBefore } from 'date-fns'
import HomeKartenCard from '~/components/top/molecules/HomeKartenCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import {
  Alert,
  Card,
  SimpleTable,
  Icon,
  Chip,
} from '~/components/common/atoms/index'
import { SupportProjectSchedule } from '~/models/Schedule'
import { getCurrentDate } from '~/utils/common-functions'

export default BaseComponent.extend({
  components: {
    HomeKartenCard,
    Alert,
    Card,
    SimpleTable,
    Icon,
    Chip,
  },
  computed: {
    formattedSchedules(): any {
      const rtn = this.latestSupportProjectSchedule.map(
        (supportSchedule: SupportProjectSchedule) => {
          const copiedSurveySchedules = Object.assign({}, supportSchedule, {
            status: 'plan',
          })

          //ステータス
          copiedSurveySchedules.status = this.checkStatus(
            copiedSurveySchedules.supportDate +
              ' ' +
              copiedSurveySchedules.supportStartTime,
            copiedSurveySchedules.supportDate +
              ' ' +
              copiedSurveySchedules.supportEndTime
          )
          //支援日
          copiedSurveySchedules.supportDate =
            copiedSurveySchedules.supportDate +
            ' ' +
            copiedSurveySchedules.supportStartTime +
            '～' +
            copiedSurveySchedules.supportEndTime

          // 最終更新
          if (copiedSurveySchedules.lastUpdateDatetime) {
            copiedSurveySchedules.lastUpdateDatetime = this.formatDate(
              new Date(copiedSurveySchedules.lastUpdateDatetime),
              'Y/MM/DD'
            )
          } else {
            copiedSurveySchedules.lastUpdateDatetime = 'ー'
          }

          // 最終更新者
          if (!copiedSurveySchedules.updateUser) {
            copiedSurveySchedules.updateUser = 'ー'
          }

          return copiedSurveySchedules
        }
      )
      rtn.sort((a, b) => b.supportDate.localeCompare(a.supportDate))
      return rtn
    },
  },

  props: {
    /**
     *カルテ情報
     */
    latestSupportProjectSchedule: {
      type: Array as PropType<SupportProjectSchedule[]>,
      default: () => [],
    },
  },
  methods: {
    /** スケジュール送信日が今日より前か */
    checkStatus(startDate: string, endDate: string) {
      const supportStartDate = new Date(startDate)
      const supportEndDate = new Date(endDate)
      const today = getCurrentDate()
      if (
        !isBefore(today, supportStartDate) &&
        !isAfter(today, supportEndDate)
      ) {
        return 'progress'
      } else if (isBefore(supportEndDate, today)) {
        return 'performed'
      } else {
        return 'plan'
      }
    },
  },
})
</script>

<style lang="scss">
.latest-support-project-table {
  thead {
    height: 50px !important;
  }
  thead th {
    height: 50px !important;
    line-height: 50px !important;
    font-size: 14px !important;
  }

  tbody {
    &:nth-child(n + 2) {
      th,
      td {
        border-top: 1px solid $c-gray-line;
      }
    }
  }
  .no-link {
    cursor: default !important;
  }
}
</style>
