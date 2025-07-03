<template>
  <div class="o-alert-list-table o-monthly-project-list-table">
    <div
      class="o-alert-list-table__body o-monthly-project-list-table__body is-scroll"
    >
      <CommonDataTablePagination
        :total="total"
        :items-per-page="limit"
        :is-loading="isLoading"
        :shows-text="showsHeaderText"
        :shows-pagination="false"
        is-short-page-text
        is-csv-output
        class="mb-4"
        :csv-button-disabled="parseInt(total) <= 0"
        :last-count-date="lastCountDate"
        @csvOutput="csvOutput"
      />
      <v-data-table
        :headers="formattedHeaders"
        :items="items"
        :no-data-text="$t('common.label.noData')"
        :loading-text="$t('common.label.loading')"
        hide-default-header
        hide-default-footer
        :items-per-page="limit"
      >
        <template #header>
          <thead v-if="headers.length" class="v-data-table-header">
            <tr>
              <template v-for="(header, index) in headers[0]">
                <th
                  v-if="header && (header.rowspan || header.colspan)"
                  :key="'th0a' + index"
                  :rowspan="header.rowspan ? header.rowspan : 1"
                  :colspan="header.colspan ? header.colspan : 1"
                  :class="[
                    'th-' + header.value,
                    header.colspan && header.colspan > 0 ? 'th-col-0' : '',
                  ]"
                >
                  <p class="mb-0 pt-2 pb-2">
                    {{ header.text }}
                  </p>
                </th>
                <th v-else :key="'th0b' + index" :class="'th-' + header.value">
                  {{ header.text }}
                </th>
              </template>
            </tr>
            <tr>
              <th
                v-for="(header, index) in headers[1]"
                :key="'th1' + index"
                :class="['th-col-1', 'th-' + header.value]"
              >
                {{ header.text }}
              </th>
            </tr>
          </thead>
        </template>
        <!-- 合計の表示を追加 -->
        <template v-if="isShowTotal" slot="body.prepend">
          <tr class="o-monthly-project-list-table__total">
            <th colspan="3">&nbsp;</th>
            <th>{{ $t('common.label.total') }}</th>
            <th
              v-for="(header, index) in headers[1]"
              :key="'th1' + index"
              :class="['th-col-1', 'th-' + header.value]"
            >
              {{ sumField(header.value) }}
            </th>
          </tr>
        </template>
        <!-- / 合計の表示を追加 -->
      </v-data-table>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Sheet, Img, Chip } from '~/components/common/atoms/index'
import CommonDataTablePagination from '~/components/common/molecules/CommonDataTablePagination.vue'
export interface HeadersType {
  [key: string]: {
    text: string
    align?: string
    sortable?: boolean
    value: string
    required?: boolean
    colspan?: number
    rowspan?: number
  }
}
export default BaseComponent.extend({
  name: 'MonthlyProjectsDataTable',
  components: {
    Sheet,
    Img,
    Chip,
    CommonDataTablePagination,
  },
  props: {
    /** 整形されたヘッダ情報 */
    headers: {
      type: Array as PropType<HeadersType[]>,
      required: true,
    },
    /** 整形された月次工数分類別工数一覧 */
    items: {
      type: Array,
      required: true,
    },
    /** 全件表示判定 */
    isShowTotal: {
      type: Boolean,
      default: false,
    },
    /** 合計要素数 */
    total: {
      type: Number,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
    /** 表示リミット */
    limit: {
      type: Number,
      default: -1,
    },
    /** 全{total}件表示判定 */
    showsHeaderText: {
      type: Boolean,
      default: true,
    },
    lastCountDate: {
      type: String,
      default: null,
    },
  },
  computed: {
    /** ヘッダ情報の整形
     * @returns 整形されたヘッダ情報
     */
    formattedHeaders() {
      const headers: any = []
      this.headers.forEach((header: any) => {
        header.forEach((item: any) => {
          if (!item.colspan) {
            headers.push(item)
          }
        })
      })
      return headers
    },
  },
  methods: {
    /** 合計の表示を追加 */
    sumField(key: any) {
      return this.items.reduce((a: any, b: any) => a + (b[key] || 0), 0)
    },
    /** CSV出力 */
    csvOutput() {
      this.$emit('csvOutput')
    },
  },
  mounted() {
    this.setScrollHint(this.$t('common.table.scrollable') as string)
  },
})
</script>

<style lang="scss">
.o-monthly-project-list-table {
  .v-data-table {
    display: block;
    // overflow-x: scroll;
    white-space: nowrap;
    -webkit-overflow-scrolling: touch;
    .v-data-table-header {
      th,
      td {
        box-sizing: border-box;
        background-color: $c-black-80;
        color: $c-white !important;
        padding: 8px 16px;
        vertical-align: middle;
        width: 200px;
        &.th-subName,
        &.th-name {
          width: 382px;
        }
        &.th-serviceTypeName {
          width: 116px;
        }
        &.th-contractType {
          width: 92px;
        }
        &.th-mainSalesUserName {
          width: 122px;
        }
        &.th-col-0 {
          width: 350px;
          padding: 0 16px;
          height: 32px;
          p {
            border-bottom: 2px solid $c-black-60;
          }
          &:last-of-type {
            width: 480px;
          }
          &.th-undefined {
            width: 100px;
            p {
              border-bottom: 0;
            }
          }
        }
        &.th-col-1 {
          width: 120px;
          padding: 8px 16px;
          height: 32px;
        }
      }
    }
    .v-data-table__wrapper {
      border-radius: 4px;
      box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
        0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
      table {
        // table-layout: fixed;
        table-layout: auto;
      }
    }
    .v-data-footer {
      &:first-child {
        margin-bottom: 16px;
      }
      &:last-child {
        margin-top: 16px;
      }
    }
  }
}

// 合計行
.o-monthly-project-list-table__total {
  th,
  td {
    height: 34px !important;
    background-color: #707070;
    color: $c-white;
  }
}
</style>
