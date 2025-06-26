<template>
  <div
    class="o-common-data-table"
    :class="{
      'is-scroll': isScroll,
      'mt-5': !showsPagination && showsHeaderText,
      'is-select': isSelectRows,
      'is-show-select': isSelectRows && showSelect,
    }"
  >
    <v-data-table
      ref="localItems"
      :value="value"
      :page="page"
      :headers="headers"
      :items="localItems"
      :items-per-page="limit"
      :loading="isLoading"
      :sort-by="sortBy"
      :sort-desc="sortDesc"
      :hide-default-footer="hideDefaultFooter"
      :height="isLoading ? '100vh' : 'auto'"
      :show-select="isSelectRows /* && showSelect*/"
      :item-key="itemKey"
      must-sort
      :no-data-text="$t('common.label.noData')"
      :loading-text="$t('common.label.loading')"
      :disable-pagination="disablePagination"
      @update:options="$emit('sort', $event)"
      @update:sort-by="emitSortBy"
      @update:sort-desc="emitSortDesc"
      @input="$emit('change:selected-rows', $event)"
    >
      <!-- ※現在、:custom-sort="customSort" が存在するとソートが機能しない -->
      <!--
    <v-data-table
      :headers="headers"
      :items="items"
      :items-per-page="limit"
      :loading="isLoading"
      :sort-by="sortBy"
      :hide-default-footer="hideDefaultFooter"
      must-sort
      @update:options="$emit('sort', $event)"
    >
      -->
      <!-- ※現在、:custom-sort="customSort" が存在するとソートが機能しない -->
      <!--
    <v-data-table
      :headers="headers"
      :items="items"
      :items-per-page="limit"
      :loading="isLoading"
      :sort-by="sortBy"
      :custom-sort="customSort"
      :hide-default-footer="hideDefaultFooter"
      must-sort
      @update:options="$emit('sort', $event)"
    >
      -->
      <template #top="{ pagination, options, updateOptions }">
        <CommonDataTablePagination
          v-if="loadsEveryTime || !showsPagination"
          :offset-page="offsetPage"
          :total="total"
          :total-contract-time="totalContractTime"
          :items-per-page="limit"
          :max-page="maxPage"
          :is-loading="isLoading"
          :shows-text="showsHeaderText"
          :shows-pagination="showsPagination"
          :is-short-page-text="shortText"
          :is-csv-output="isCsvOutput"
          :show-select="showSelect"
          :is-select-rows="isSelectRows"
          :is-single-month="isSingleMonth"
          :edit-button-disabled="value.length === 0"
          :is-total-contract-time="isTotalContractTime"
          class="mb-4"
          :csv-button-disabled="csvButtonDisabled"
          @click:prev="clickPrev"
          @click:next="clickNext"
          @csvOutput="$emit('csvOutput')"
          @click:toggle-show-select="$emit('click:toggle-show-select')"
          @click:open-modal-bulk-edit="$emit('click:open-modal-bulk-edit')"
          @click:open-modal-bulk-delete="$emit('click:open-modal-bulk-delete')"
        />
        <v-data-footer
          v-else
          :pagination="pagination"
          :options="options"
          @update:options="updateOptions"
        >
          <template #page-text="{ pageStart, pageStop, itemsLength }">
            {{
              $t('common.table.pageText', { pageStart, pageStop, itemsLength })
            }}
          </template>
        </v-data-footer>
      </template>

      <template #[`header.data-table-select`]="{ props, on }">
        <SimpleCheckbox color="primary" v-bind="props" v-on="on" />
      </template>
      <template #[`item.data-table-select`]="{ isSelected, select }">
        <SimpleCheckbox
          color="primary"
          :value="isSelected"
          @input="select($event)"
        />
      </template>
      <!-- カラム書き換え処理の順序に注意（後に書いたものが優先される） -->
      <!-- 最大表示文字数を超えた場合「...」を表示 -->
      <template v-for="header in headers" #[`item.${header.value}`]="{ item }">
        <template v-if="header.link && !showSelect">
          <nuxt-link
            :key="header.value"
            class="o-common-data-table__link"
            :to="
              header.link.prefix +
              item[header.link.value] +
              (header.link.suffix || '')
            "
          >
            <OverflowTooltip
              :text="item[header.value]"
              :max="header.maxLength"
            />
          </nuxt-link>
        </template>
        <template v-else>
          <OverflowTooltip
            :key="header.value"
            :text="item[header.value]"
            :max="header.maxLength"
          />
        </template>
      </template>

      <!-- scopedSlotの貫通(カラムに特殊な加工が必要な時に用いる) -->
      <template v-for="(_, name) in $scopedSlots" #[name]="slotData">
        <slot :name="name" v-bind="slotData" />
      </template>

      <template v-if="loadsEveryTime" #footer>
        <template v-if="showsPagination">
          <CommonDataTablePagination
            :offset-page="offsetPage"
            :total="total"
            :items-per-page="limit"
            :max-page="maxPage"
            :is-loading="isLoading"
            :shows-text="false"
            :shows-pagination="true"
            class="mt-4"
            @click:prev="clickPrev"
            @click:next="clickNext"
          />
        </template>
      </template>

      <template #[`footer.page-text`]> </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import type { LocaleMessages } from 'vue-i18n/types'
import { SimpleCheckbox } from '~/components/common/atoms/index'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import CommonDataTablePagination from '~/components/common/molecules/CommonDataTablePagination.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'

export interface IHeaderLinkInfo {
  prefix: string
  value: string
  suffix?: string
}

export interface IDataTableHeader {
  // Vuetify指定のprops
  text: string | LocaleMessages
  value: string
  align?: 'start' | 'center' | 'end'
  sortable?: boolean
  filterable?: boolean
  groupable?: boolean
  divider?: boolean
  class?: string | string[]
  cellClass?: string | string[]
  width?: string | number
  filter?: (value: any, search: string, item: any) => boolean
  sort?: (a: any, b: any) => number
  // 自作のprops
  maxLength?: number
  link?: IHeaderLinkInfo
}
export interface RowClickInfo {
  isSelected: boolean
  select: (value: boolean) => void
}

export default BaseComponent.extend({
  components: {
    SimpleCheckbox,
    OverflowTooltip,
    CommonDataTablePagination,
  },
  props: {
    loadsEveryTime: {
      type: Boolean,
      default: true, // falseのとき、あらかじめ全件読み込む(v-data-tableデフォルトの動作をする)
    },
    headers: {
      type: Array as PropType<IDataTableHeader[]>,
      required: true,
    },
    items: {
      type: Array,
      required: true,
    },
    linkPrefix: {
      type: String,
      default: '',
    },
    showsHeaderText: {
      type: Boolean,
      default: true,
    },
    showsPagination: {
      type: Boolean,
      default: true,
    },
    isScroll: {
      type: Boolean,
      default: false,
    },
    offsetPage: {
      type: Number,
    },
    total: {
      type: Number,
    },
    isHideButton: {
      type: Boolean,
      default: false,
    },
    isCsvOutput: {
      type: Boolean,
      default: false,
    },
    shortText: {
      type: Boolean,
      default: false,
    },
    limit: {
      type: Number,
      default: -1,
    },
    sortBy: {
      type: Array as PropType<String[]> | String as PropType<String>,
    },
    sortDesc: {
      type: Array as PropType<Boolean[]> | Boolean as PropType<Boolean>,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    additionalText: {
      type: String,
      default: '',
    },
    maxPage: {
      type: Number,
      default: -1,
    },
    page: {
      type: Number,
      default: -1,
    },
    totalContractTime: {
      type: Number,
      default: 0,
    },
    isTotalContractTime: {
      type: Boolean,
      default: false,
    },
    disablePagination: {
      type: Boolean,
      default: false,
    },
    csvButtonDisabled: {
      type: Boolean,
      default: false,
    },
    showSelect: {
      type: Boolean,
      default: false,
    },
    isSelectRows: {
      type: Boolean,
      default: false,
    },
    itemKey: {
      type: String,
      default: '',
    },
    value: {
      type: Array,
      default: [],
    },
    isSingleMonth: {
      type: Boolean,
    },
  },
  computed: {
    linkedHeaders() {
      const rtn: IDataTableHeader[] = []
      this.headers.forEach((header: IDataTableHeader) => {
        if (header.link) {
          rtn.push(header)
        }
      })

      return rtn
    },
    hideDefaultFooter() {
      // ページネーションの動作をカスタマイズしているなら、デフォルトのフッターを出さない
      if (this.loadsEveryTime) {
        return true
      }

      return !this.showsPagination
    },
    localItems() {
      // データ型が一定ではないのでany型を使用
      const localItems: any = this.items
      for (const i in localItems) {
        for (const i2 in localItems[i]) {
          // 値が文字列型の場合のみトリミング
          if (typeof localItems[i][i2] === 'string') {
            localItems[i][i2] = localItems[i][i2].trim()
          }
        }
      }
      return localItems
    },
  },
  methods: {
    // ソート関数を動作しないものにすりかえる（ソートボタンをapi発火ボタンにするため）
    customSort(items: any[]) {
      return items
    },
    clickPrev() {
      this.$emit('click:prev')
    },
    clickNext() {
      this.$emit('click:next')
    },
    // 親コンポーネントにソート済みの値配列を渡す
    emitSortItems(this: any) {
      const filteredItems = this.$refs.localItems.$children[0].filteredItems
      const sortItems =
        this.$refs.localItems.$children[0].sortItems(filteredItems)
      this.$emit('update:sort-items', sortItems)
    },
    // 親コンポーネントにソート対象変更イベントを渡す
    emitSortBy(event: Event) {
      this.emitSortItems()
      const sortParams = String(event).split(',')
      this.$emit('update:sort-by', sortParams)
    },
    // 親コンポーネントにソート並び順変更イベントを渡す
    emitSortDesc(event: Event) {
      this.emitSortItems()
      const orderParams = []
      const splitOrders = String(event).split(',')
      for (const i in splitOrders) {
        orderParams.push(this.toBoolean(splitOrders[i]))
      }
      this.$emit('update:sort-desc', orderParams)
    },
    // 文字列から真偽値を判定
    toBoolean(value: string | Event): boolean {
      return String(value).toLowerCase() === 'true'
    },
  },
  mounted() {
    this.setScrollHint(this.$t('common.table.scrollable') as string)
  },
  updated() {
    this.emitSortItems()
  },
})
</script>
