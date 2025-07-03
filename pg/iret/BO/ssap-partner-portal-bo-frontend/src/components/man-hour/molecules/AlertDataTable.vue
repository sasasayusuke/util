<template>
  <div class="o-alert-list-table">
    <div class="o-alert-list-table__body" :class="{ 'is-scroll': isScroll }">
      <CommonDataTablePagination
        :offset-page="offsetPage"
        :max-page="maxPage"
        :total="total"
        :total-contract-time="totalContractTime"
        :limit="maxPage"
        :is-loading="isLoading"
        is-short-page-text
        :shows-pagination="false"
        class="mb-4"
        @click:prev="$emit('click:prev')"
        @click:next="$emit('click:next')"
      />
      <v-data-table
        :headers="formattedHeaders"
        :items="items"
        :items-per-page="limit"
        :no-data-text="$t('common.label.noData')"
        :loading-text="$t('common.label.loading')"
        hide-default-header
        hide-default-footer
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

        <template #[`footer.page-text`]></template>
        <template #[`item.projectName`]="{ item }">
          <nuxt-link
            class="o-alert-table__link"
            :to="{
              path: `/man-hour/alert/${year}/${month}/${item.projectId}`,
            }"
          >
            {{ item.projectName }}
          </nuxt-link>
        </template>

        <template #[`item.alertDirectSupportManHour`]="{ item }">
          <template v-if="item.alertDirectSupportManHour === 2">
            <Sheet class="d-flex justify-start align-center m-alert-label">
              <div>
                <Img
                  :src="require('~/assets/img/icon/org-icon-alert.svg')"
                  width="21"
                  height="18"
                  alt="alert-possiblity"
                />
              </div>
              <p class="m-alert-label--red ma-0 pl-2">
                {{ $t('common.label.alertDirectSupportManHour') }}
              </p>
            </Sheet>
          </template>
          <template v-else-if="item.alertDirectSupportManHour === 1">
            <Sheet class="d-flex justify-start align-center m-alert-label">
              <div>
                <Img
                  :src="
                    require('~/assets/img/icon/org-icon-alert-probability.svg')
                  "
                  width="18"
                  height="18"
                  alt="alert-possiblity"
                />
              </div>
              <p class="m-alert-label--blue ma-0 pl-2">
                {{ $t('common.label.alertProspectDirectSupportManHour') }}
              </p>
            </Sheet>
          </template>
        </template>

        <template #[`item.alertPreSupportManHour`]="{ item }">
          <template v-if="item.alertPreSupportManHour === 2">
            <Sheet class="d-flex justify-start align-center m-alert-label">
              <div>
                <Img
                  :src="require('~/assets/img/icon/org-icon-alert.svg')"
                  width="18"
                  height="18"
                  alt="alert-possiblity"
                />
              </div>
              <p class="m-alert-label--red ma-0 pl-2">
                {{ $t('common.label.alertPreSupportManHour') }}
              </p>
            </Sheet>
          </template>
          <template v-else-if="item.alertPreSupportManHour === 1">
            <Sheet class="d-flex justify-start align-center m-alert-label">
              <div>
                <Img
                  :src="
                    require('~/assets/img/icon/org-icon-alert-probability.svg')
                  "
                  width="18"
                  height="18"
                  alt="alert-possiblity"
                />
              </div>
              <p class="m-alert-label--blue ma-0 pl-2">
                {{ $t('common.label.alertProspectPreSupportManHour') }}
              </p>
            </Sheet>
          </template>
        </template>
      </v-data-table>
      <CommonDataTablePagination
        :offset-page="offsetPage"
        :max-page="maxPage"
        :limit="maxPage"
        :is-loading="isLoading"
        :shows-text="false"
        :shows-pagination="false"
        class="mt-4"
        @click:prev="$emit('click:prev')"
        @click:next="$emit('click:next')"
      />
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Sheet, Img } from '~/components/common/atoms/index'
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
  name: 'AlertDataTable',
  components: {
    Sheet,
    Img,
    CommonDataTablePagination,
  },
  props: {
    /** 現在の年 || 選択された年 */
    year: {
      type: Number,
    },
    /** 現在の月 || 選択された月 */
    month: {
      type: Number,
    },
    /** ヘッダ */
    headers: {
      type: Array as PropType<HeadersType[]>,
      required: true,
      default: [],
    },
    /** 案件毎の工数状況とアラート一覧情報 */
    items: {
      type: Array,
      required: true,
    },
    linkPrefix: {
      type: String,
      default: '',
    },
    /** ページネーションの表示判定 */
    isHidePagination: {
      type: Boolean,
      default: false,
    },
    isScroll: {
      type: Boolean,
      default: false,
    },
    offsetPage: {
      type: Number,
    },
    maxPage: {
      type: Number,
    },
    total: {
      type: Number,
    },
    /** 延べ契約時間の合計 */
    totalContractTime: {
      type: Number,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    limit: {
      type: Number,
      default: -1,
    },
  },
  computed: {
    /** ヘッダ情報の整形 */
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
  mounted() {
    this.setScrollHint(this.$t('common.table.scrollable') as string)
  },
})
</script>
