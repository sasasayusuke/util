<template>
  <div class="o-data-table o-data-table--schedule">
    <v-data-table
      :headers="headers"
      :items="showMore ? addedItems : addedItems.slice(0, showDefault)"
      :loading="isLoading"
      disable-pagination
      hide-default-footer
      :no-data-text="$t('common.label.noData')"
      :loading-text="$t('common.label.loading')"
      elevation="0"
    >
      <template #[`item.name`]="{ item }">
        <nuxt-link class="o-data-table__link" :to="`${linkPrefix}/${item.id}`">
          {{ item.name }}
        </nuxt-link>
      </template>
      <!-- ステータス -->
      <template #[`item.status`]="{ item }">
        <template v-if="type === 'support'">
          <Chip v-if="item.status === 'progress'" small style-set="primary-60">
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
        </template>
        <template v-else>
          <Chip v-if="item.completed" small style-set="secondary-60">
            {{ $t('common.label.completed') }}
          </Chip>
          <Chip v-else small outlined style-set="secondary-60">
            {{ $t('common.label.unanswered') }}
          </Chip>
        </template>
      </template>
      <!-- 支援日 -->
      <template #[`item.supportDate`]="{ item }">
        <span class="mr-2">{{ item.supportDate.split(' ')[0] }}</span>
        <span>{{ item.supportDate.split(' ')[1] }}</span>
      </template>
      <!-- カルテ表示 -->
      <template #[`item.karteId`]="{ item }">
        <nuxt-link
          class="o-data-table__link font-size-xsmall"
          :class="{ 'is-disabled': !item.isAccessibleKarteDetail }"
          :to="`${linkPrefix}/${item.karteId}`"
        >
          {{ $t('project.pages.detail.supportSchedules.table.viewKarte') }}
        </nuxt-link>
      </template>

      <!-- 編集ボタン / 削除ボタン -->
      <template v-if="isEditDeleteDisplay" #[`item.actions`]="{ item }">
        <div class="d-flex justify-end">
          <TableActions
            v-if="
              type === 'support' ||
              (!item.completed && type === 'survey') ||
              (item.isResendAnonymousSurvey === true &&
                type === 'survey' &&
                isAllowConfigureAnonymous)
            "
            bottom
            left
            :show-update="
              type === 'support' || (!item.completed && type === 'survey')
            "
            :show-delete="
              type === 'support' || (!item.completed && type === 'survey')
            "
            :show-resend="
              type === 'survey' &&
              item.isResendAnonymousSurvey === true &&
              isAllowConfigureAnonymous
            "
            :offset-y="true"
            @click:edit="$emit('click:edit', item)"
            @click:delete="$emit('click:delete', item)"
            @click:resend="$emit('click:resend', item)"
          />
        </div>
      </template>
    </v-data-table>
    <v-container v-if="addedItems.length > showDefault" ma-0 pa-0 mt-6>
      <v-row justify="center">
        <Button text @click="toggleShowProjects">
          <Icon size="16" class="mr-2" :class="{ 'is-rotate-180': showMore }"
            >icon-org-arrow-down</Icon
          >{{
            showMore
              ? $t('common.button.close')
              : $t('common.button.showContinuation')
          }}
        </Button>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import TableActions from '~/components/common/molecules/TableActions.vue'
import { Icon, Button, Chip } from '~/components/common/atoms/index'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { hasRole } from '~/utils/role-authorizer'
import { ISupportProjectSchedule } from '~/types/Schedule'

export default BaseComponent.extend({
  components: {
    TableActions,
    Icon,
    Button,
    Chip,
  },
  props: {
    /** テーブルヘッダー情報 */
    headers: {
      type: Array,
      required: true,
    },
    /** 表示項目情報 */
    items: {
      type: Array as PropType<ISupportProjectSchedule[]>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** リンク先接頭語 */
    linkPrefix: {
      type: String,
      default: '',
    },
    /** ページネーションを非表示とするか */
    isHidePagination: {
      type: Boolean,
      default: false,
    },
    /** 表示対象 */
    type: {
      type: String,
    },
    /** デフォルトの表示件数 */
    showDefault: {
      type: Number,
      default: 5,
    },
    /** 選択中の案件ID */
    projectId: {
      type: String,
      required: true,
    },
    /** 編集および削除ボタンを表示するか */
    isEditDeleteDisplay: {
      type: Boolean,
      required: true,
    },
  },
  data(): {
    showMore: boolean
    addedItems: ISupportProjectSchedule[]
  } {
    return {
      showMore: false,
      addedItems: [],
    }
  },
  watch: {
    items: {
      deep: true,
      handler() {
        const addedItems: any = this.items
        addedItems.forEach((addedItem: any) => {
          addedItem.projectId = this.projectId
        })
        this.addedItems = addedItems
      },
    },
  },
  computed: {
    /**
     * 匿名アンケート関連の設定が可能なアカウントか判定 (営業担当者・営業責任者・アンケート事務局・システム管理者・事業者責任者)
     * @returns 匿名アンケート関連の設定が可能なアカウントかの真偽値
     */
    isAllowConfigureAnonymous() {
      return hasRole([
        ENUM_ADMIN_ROLE.SALES,
        ENUM_ADMIN_ROLE.SALES_MGR,
        ENUM_ADMIN_ROLE.SURVEY_OPS,
        ENUM_ADMIN_ROLE.SYSTEM_ADMIN,
        ENUM_ADMIN_ROLE.BUSINESS_MGR,
      ])
    },
  },
  methods: {
    /** 続きを表示する */
    toggleShowProjects() {
      if (this.showMore === true) {
        this.showMore = false
      } else {
        this.showMore = true
      }
    },
  },
})
</script>

<style lang="scss">
.o-data-table--schedule {
  .v-data-table {
    .v-data-table__wrapper {
      border-radius: 0 !important;
      box-shadow: none !important;
      table {
        tbody {
          tr {
            &:hover {
              a {
                color: $c-primary-over !important;
                &.is-disabled {
                  color: rgba(0, 0, 0, 0.87) !important;
                }
              }
            }
          }
          td {
            padding: 0 16px;
            height: 36px;
          }
        }
        thead {
          tr {
            th {
              height: 32px;
            }
          }
        }
      }
    }
    th,
    td {
      border-bottom: 0 !important;
    }
    tr {
      transition-property: background-color;
      transition-duration: 0.2s;
      &:hover {
        background: $c-primary-8 !important;
      }
      &:nth-child(even) {
        background: $c-black-table-border;
        &:hover {
          background: $c-primary-8 !important;
        }
      }
    }
    .o-data-table__link.is-disabled {
      pointer-events: none;
      text-decoration: none;
      color: rgba(0, 0, 0, 0.87) !important;
      font-weight: normal;
    }
  }
}
</style>
