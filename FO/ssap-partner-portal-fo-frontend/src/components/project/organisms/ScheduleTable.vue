<template>
  <div class="o-data-table o-data-table--schedule">
    <v-data-table
      :headers="headers"
      :items="showAll ? items : items.slice(0, showDefault)"
      :loading="isLoading"
      :no-data-text="$t('common.label.noData')"
      :loading-text="$t('common.label.loading')"
      hide-default-footer
      elevation="0"
      disable-pagination
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
      <!-- カルテ表示 -->
      <template #[`item.karteId`]="{ item }">
        <nuxt-link
          class="o-data-table__link"
          :class="{ 'is-disabled': !item.isAccessibleKarteDetail }"
          :to="`${linkPrefix}/${item.karteId}`"
        >
          {{ $t('project.pages.detail.supportSchedules.table.viewKarte') }}
        </nuxt-link>
      </template>
      <!-- 編集ボタン -->
      <template v-if="editAndDelete" #[`item.actions`]="{ item }">
        <div class="d-flex justify-end">
          <TableActions
            v-if="type === 'support' || (!item.completed && type === 'survey')"
            bottom
            left
            :show-delete="
              type === 'support' || (!item.completed && type === 'survey')
            "
            :offset-y="true"
            @click:edit="$emit('click:edit', item)"
            @click:delete="$emit('click:delete', item)"
          />
        </div>
      </template>
    </v-data-table>
    <v-container ma-0 pa-0 mt-6>
      <v-row v-if="items.length >= 5" justify="center">
        <Button
          v-if="items.length > showDefault"
          text
          @click="showAll = !showAll"
        >
          <Icon
            size="16"
            class="o-data-table__button-icon mr-2"
            :class="{ 'is-open': showAll }"
            >icon-org-arrow-down</Icon
          >{{
            showAll ? $t('common.button.close') : $t('common.button.showMore')
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
import { SupportProjectSchedule } from '~/models/Schedule'

export default BaseComponent.extend({
  components: {
    TableActions,
    Icon,
    Button,
    Chip,
  },
  props: {
    headers: {
      type: Array,
      required: true,
    },
    items: {
      type: Array as PropType<SupportProjectSchedule[]>,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    linkPrefix: {
      type: String,
      default: '',
    },
    isHidePagination: {
      type: Boolean,
      default: false,
    },
    type: {
      type: String,
      default: '',
    },
    showDefault: {
      type: Number,
      default: 5,
    },
    editAndDelete: {
      type: Boolean,
    },
  },
  data() {
    return {
      showAll: false,
    }
  },
})
</script>

<style lang="scss" scoped>
.o-data-table__button-icon {
  &.is-open {
    transform: rotate(180deg);
    transition-duration: 0.3s;
  }
}
</style>
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
  }
  .o-data-table__link.is-disabled {
    pointer-events: none;
    text-decoration: none;
    color: rgba(0, 0, 0, 0.87) !important;
    font-weight: normal;
  }
}
</style>
