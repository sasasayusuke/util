<template>
  <div class="o-data-table o-data-table--question">
    <v-data-table
      :headers="headers"
      :items="formattedSurveyMaster"
      hide-default-footer
      :no-data-text="$t('common.label.noData')"
      :loading-text="$t('common.label.loading')"
      elevation="0"
    >
      <!-- ステータス -->
      <template #[`item.status`]="{ item }">
        <Chip
          v-if="item.status === 'in_operation'"
          small
          style-set="secondary-70"
        >
          {{ $t('common.label.in-operation') }}
        </Chip>
        <Chip
          v-else-if="item.status === 'draft'"
          small
          outlined
          style-set="secondary-70"
        >
          {{ $t('common.label.draft') }}
        </Chip>
        <Chip v-else small style-set="tertiary-70">
          {{ $t('common.label.archive') }}
        </Chip>
      </template>
      <!-- バージョン -->
      <template #[`item.revision`]="{ item }">
        <nuxt-link
          v-if="item.status === 'draft'"
          class="o-data-table__link"
          :to="`${linkPrefix}${item.id}/${item.revision}`"
        >
          {{ $t('survey.pages.masterDetail.version.row.draft'), }}
        </nuxt-link>
        <nuxt-link
          v-else
          class="o-data-table__link"
          :to="`${linkPrefix}${item.id}/${item.revision}`"
        >
          {{ $t('survey.pages.masterDetail.version.row.name')
          }}{{ item.revision }}
        </nuxt-link>
      </template>
      <!-- 編集ボタン -->
      <template #[`item.actions`]="{ item }">
        <div class="d-flex justify-end">
          <TableActions
            survey-question-table
            bottom
            left
            :item="item.status"
            :offset-y="true"
            :shows-copy-options="showsCopyOptions"
            @click:refer="$emit('click:refer', item.id, item.revision)"
            @click:edit="$emit('click:edit', item.id, item.revision)"
            @click:delete="$emit('click:delete', item.id, item.revision)"
            @click:draft="$emit('click:draft', item.id, item.revision)"
          />
        </div>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import TableActions from '~/components/common/molecules/TableActions.vue'
import { SurveyMasterListItem } from '~/models/Master'
import type { PropType } from '~/common/BaseComponent'
import { Icon, Button, Chip } from '~/components/common/atoms/index'
import { formatDateStr } from '~/utils/common-functions'

export default BaseComponent.extend({
  components: {
    TableActions,
    Icon,
    Button,
    Chip,
  },
  props: {
    /** アンケートマスター一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterListItem[]>,
      required: true,
    },
    /** テーブルヘッダー情報 */
    headers: {
      type: Array,
      required: true,
    },
    /** リンク先プレフィックス */
    linkPrefix: {
      type: String,
      default: '',
    },
    /** ページネーション無効化 */
    isHidePagination: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      showsCopyOptions: true,
    }
  },
  created() {
    this.surveyMasters.forEach((surveyMaster: any) => {
      if (surveyMaster.status === 'draft') {
        this.showsCopyOptions = false
      }
    })
  },
  computed: {
    /**
     * 表示用のフォーマットに変換したアンケートマスター配列を返す
     * @returns フォーマット済みアンケートマスター配列
     */
    formattedSurveyMaster(): SurveyMasterListItem[] {
      return this.surveyMasters.map((surveyMaster: SurveyMasterListItem) => {
        if (surveyMaster.updateAt) {
          surveyMaster.updateAt = formatDateStr(
            surveyMaster.updateAt,
            'Y/MM/dd HH:mm'
          )
        }
        // revisionが0の場合、ステータスは「下書き」で固定
        if (surveyMaster.revision === 0) {
          surveyMaster.status = 'draft'
        }
        return surveyMaster
      })
    },
  },
})
</script>

<style lang="scss">
.o-data-table--question {
  .v-data-table {
    .v-data-table__wrapper {
      border-radius: 0 !important;
      box-shadow: none !important;
      table {
        tr {
          td {
            padding: 6px 16px;
          }
        }
      }
    }
    th,
    td {
      border-bottom: 0 !important;
    }
    tr {
      &:nth-child(even) {
        background-color: $c-black-table-border;
      }
    }
  }
}
</style>
