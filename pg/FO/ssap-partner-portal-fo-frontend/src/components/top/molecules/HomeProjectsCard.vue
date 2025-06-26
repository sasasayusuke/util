<template>
  <Card
    v-if="blank"
    class="m-card-project pa-5"
    color="#f0f0f0"
    elevation="0"
  ></Card>
  <Card
    v-else
    class="m-card-project pa-5"
    style-set="hover"
    :href="`/karte/list/${project.id}`"
    elevation="3"
  >
    <div class="m-card-project__body">
      <div class="m-card-project__unit">
        <div class="m-card-project__head">
          <ToolTips>
            <template #button>
              <p class="m-card-project__name">
                {{ project.name }}
              </p>
            </template>
            {{ project.name }}
          </ToolTips>
          <Icon size="16px" class="mt-1">icon-org-arrow-right</Icon>
        </div>
      </div>
      <div class="m-card-project__unit">
        <p class="m-card-project__organization">
          {{ project.customerName }}
        </p>
      </div>
      <div class="m-card-project__unit">
        <p class="m-card-project__terms">
          <span>
            <Icon size="20px">mdi-calendar</Icon>
            {{ $t('top.pages.home.projects.supportPeriod') }} :
            {{ project.supportDateFrom }}&nbsp;～&nbsp;{{
              project.supportDateTo
            }}
          </span>
          <!-- 支援状況ステータスフラグ（実施中・実施済み・予定） -->
          <Chip
            v-if="status === 'progress'"
            label
            small
            style-set="primary-70"
            >{{ $t('common.label.progress') }}</Chip
          >
          <Chip
            v-if="status === 'performed'"
            label
            small
            style-set="secondary-70"
            >{{ $t('common.label.performed') }}</Chip
          >
          <Chip
            v-if="status === 'plan'"
            label
            small
            style-set="primary-70"
            outlined
            >{{ $t('common.label.plan') }}</Chip
          >
        </p>
      </div>
    </div>
  </Card>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import { Card, Chip, Icon, ToolTips } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { ProjectListItem } from '~/models/Project'
import { ENUM_PROJECTS_STATUS } from '~/types/Project'

export default BaseComponent.extend({
  components: {
    Card,
    Chip,
    Icon,
    ToolTips,
  },
  computed: {
    /**
     * 案件の支援ステータスの取得
     */
    status() {
      let result = ''
      const intDateCurrent = parseInt(
        format(getCurrentDate(), this.$t('common.format.date_ymd7') as string)
      )
      if (
        this.project &&
        this.project.supportDateFrom &&
        this.project.supportDateTo
      ) {
        const strSupportDateFrom = this.project.supportDateFrom
        const strSupportDateTo = this.project.supportDateTo
        let intSupportDateFrom = 0
        let intSupportDateTo = 0
        if (
          strSupportDateFrom != null &&
          !Number.isInteger(strSupportDateFrom)
        ) {
          intSupportDateFrom = parseInt(
            format(
              new Date(strSupportDateFrom),
              this.$t('common.format.date_ymd7') as string
            )
          )
        }
        if (strSupportDateTo != null && !Number.isInteger(strSupportDateTo)) {
          intSupportDateTo = parseInt(
            format(
              new Date(strSupportDateTo),
              this.$t('common.format.date_ymd7') as string
            )
          )
        }

        if (intDateCurrent > intSupportDateTo) {
          result = ENUM_PROJECTS_STATUS.PERFORMED
        } else if (intDateCurrent < intSupportDateFrom) {
          result = ENUM_PROJECTS_STATUS.PLAN
        } else {
          result = ENUM_PROJECTS_STATUS.PROGRESS
        }
      }
      return result
    },
  },
  props: {
    /**
     * 案件情報
     */
    project: {
      type: Object as PropType<ProjectListItem>,
    },
    blank: {
      type: Boolean,
      default: false,
    },
  },
})
</script>
<style lang="scss" scoped>
.m-card-project {
  height: 100%;
  min-height: 152px;
  transition-property: background-color;
  transition-duration: 0.2s;
  &:hover,
  &:focus {
    background-color: $c-primary-8;
  }
}

.m-card-project__body {
  display: grid;
  height: 100%;
  grid-template-rows: auto auto auto;
}

.m-card-project__unit {
  &:nth-last-child(1) {
    align-self: end;
  }
}

.m-card-project__head {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  color: $c-primary-dark;
  @include fontSize('large');
  font-weight: bold;
  margin-bottom: 8px;
  // height: 55px;
}
.m-card-project__name {
  -webkit-line-clamp: 2;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  margin-bottom: 0;
}
.m-card-project__organization {
  @include fontSize('small');
  // margin-bottom: 32px;
}
.m-card-project__terms {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0;
  width: 100%;
  @include fontSize('small');
}
</style>
