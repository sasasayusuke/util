<template>
  <div
    class="o-common-data-table"
    :class="{
      'is-scroll': isScroll,
      'mt-5': !showsPagination && showsHeaderText,
    }"
  >
    <v-data-table
      :class="className"
      :page="page"
      :headers="headers"
      :items="localItems"
      :items-per-page="limit"
      :loading="isLoading"
      :sort-by="sortBy"
      :hide-default-footer="hideDefaultFooter"
      :no-data-text="$t('common.label.noData')"
      :loading-text="$t('common.label.loading')"
      :custom-sort="customSortFunc"
      must-sort
      :disable-pagination="disablePagination"
      @update:options="$emit('sort', $event)"
    >
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
          :is-total-contract-time="isTotalContractTime"
          class="mb-4"
          @click:prev="clickPrev"
          @click:next="clickNext"
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
      <!-- カラム書き換え処理の順序に注意（後に書いたものが優先される） -->
      <!-- 最大表示文字数を超えた場合「...」を表示 -->
      <template v-for="header in headers" #[`item.${header.value}`]="{ item }">
        <template v-if="header.link">
          <nuxt-link
            :key="header.value"
            class="o-common-data-table__link"
            :to="
              header.link.prefix +
              item[header.link.value] +
              (header.link.suffix || '')
            "
            :target="header.link.target || '_self'"
          >
            <OverflowTooltip
              :text="item[header.value]"
              :max="header.maxLength"
            />
          </nuxt-link>
        </template>
        <template v-else-if="header.textBox">
          <div
            v-if="header.textBox.type === 'single'"
            :key="header.value"
            style="display: flex; align-items: center"
          >
            <TextField
              v-model.number="item[header.value]"
              role="textbox"
              :max-digits="header.maxLength"
              number
              :positive-number="header.textBox.positiveNumber || false"
              :range-number-to="header.textBox.rangeNumberTo || false"
              outlined
              dense
              class="text-right"
              @input="changeInput"
              @update:error="handleError($event, item, header.value)"
            />
            <div style="margin-left: 8px">
              {{ header.textBox.unit }}
            </div>
          </div>
          <!-- 文字列のテキストボックス -->
          <div
            v-else-if="header.textBox.type === 'text'"
            :key="header.value"
            style="display: flex; align-items: center"
          >
            <TextField
              v-model="item[header.value]"
              role="textbox"
              :max-length="header.maxLength"
              outlined
              dense
              @input="changeInput"
              @update:error="handleError($event, item, header.value)"
            />
            <div style="margin-left: 8px">
              {{ header.textBox.unit }}
            </div>
          </div>
          <div
            v-else
            :key="header.value"
            style="display: flex; align-items: center"
          >
            <TextField
              v-model.number="item[header.value].value1"
              role="textbox"
              :max-digits="header.maxLength"
              number
              :positive-number="header.textBox.positiveNumber || false"
              outlined
              dense
              class="text-right"
              :additional-rules="getAdditionalRules(item, header.value)"
              @input="validateInput(item, header.value)"
              @update:error="handleError($event, item, header.value, 'value1')"
            />
            <div style="margin-left: 8px">
              {{ header.textBox.unit }}
            </div>
            <div style="margin: 0 8px">〜</div>
            <TextField
              v-model.number="item[header.value].value2"
              role="textbox"
              :max-digits="header.maxLength"
              number
              :positive-number="header.textBox.positiveNumber || false"
              outlined
              dense
              class="text-right"
              :additional-rules="getAdditionalRules(item, header.value)"
              @input="validateInput(item, header.value)"
              @update:error="handleError($event, item, header.value, 'value2')"
            />
            <div style="margin-left: 8px">
              {{ header.textBox.unit }}
            </div>
          </div>
        </template>
        <!-- 一覧内ボタン（例：個人ソルバー登録ステータスなど） -->
        <template v-else-if="header.button">
          <div
            v-if="!isSolverList"
            :key="header.value"
            style="display: flex; align-items: center"
          >
            <Button
              v-if="item.buttonInfo.to"
              style-set="small-primary"
              width="120"
              outlined
              :to="item.buttonInfo.to"
            >
              {{ item.buttonInfo.label }}
            </Button>
            <Button
              v-else
              style-set="small-primary"
              width="120"
              @click="$emit('update', item.id, item.version)"
            >
              {{ item.buttonInfo.label }}
            </Button>
          </div>
          <div
            v-else
            :key="header.value"
            style="display: flex; align-items: center"
          >
            <!-- ボタンを表示 個人ソルバー登録 @update を発火 -->
            <Button
              v-if="item.registrationStatus === ENUM_REGISTRATION_STATUS.SAVED"
              style-set="small-primary"
              width="120"
              outlined
              @click="$emit('update', item.id, item.version)"
            >
              {{ item.buttonInfo.label }}
            </Button>
            <!-- テキストを表示 -->
            <span v-else>
              {{ item.buttonInfo.label }}
            </span>
          </div>
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
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import CommonDataTablePagination from '~/components/common/molecules/CommonDataTablePagination.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { TextField, Button } from '~/components/common/atoms/index'
import { ENUM_REGISTRATION_STATUS } from '~/types/Solver'

export interface IHeaderLinkInfo {
  prefix: string
  value: string
  suffix?: string
  target?: string
}

export interface IHeaderTextBoxInfo {
  type: string
  unit?: string
  positiveNumber?: boolean
  rangeNumberTo?: number
}

export interface IHeaderButton {
  label: string
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
  textBox?: IHeaderTextBoxInfo
  button?: IHeaderButton
}

export default BaseComponent.extend({
  components: {
    OverflowTooltip,
    CommonDataTablePagination,
    TextField,
    Button,
  },
  data() {
    return {
      ENUM_REGISTRATION_STATUS,
      // 人月単価用ルール配列
      personMonthRules: {} as { [itemId: string]: Function[] },
      // 時間単価用ルール配列
      hourlyRateRules: {} as { [itemId: string]: Function[] },
      // エラーフラグ（人月単価・時間単価）用配列
      errorFlags: {} as {
        [itemId: string]: {
          hourlyRate?: boolean
          pricePerPersonMonth?: boolean
        }
      },
    }
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
    isOnlyPageText: {
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
      type: String,
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
    className: {
      type: String,
      required: false,
    },
    // 個人ソルバー一覧かどうか
    isSolverList: {
      type: Boolean,
      default: false,
    },
    customSortFunc: {
      type: Function,
      default: undefined,
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
    // 入力された項目に応じて追加のバリデーションルールを返す
    getAdditionalRules(item: any, headerValue: string) {
      if (headerValue === 'hourlyRate') {
        return this.personMonthRules[item.id]
      } else if (headerValue === 'pricePerPersonMonth') {
        return this.hourlyRateRules[item.id]
      } else {
        return []
      }
    },
    // 人月単価・時間単価が[下限 > 上限]の場合のバリデーションチェック
    validateInput(item: any, headerValue: string) {
      const value1 = Number(item[headerValue].value1)
      const value2 = Number(item[headerValue].value2)

      // エラーフラグが存在しない場合、オブジェクトを初期化
      if (!this.errorFlags) {
        this.errorFlags = {}
      }
      // エラーフラグのオブジェクトに値が存在しない場合、オブジェクトを初期化
      if (!this.errorFlags[item.id]) {
        this.errorFlags[item.id] = {}
      }

      const isErrorCheckSkipped =
        (headerValue === 'hourlyRate' ||
          headerValue === 'pricePerPersonMonth') &&
        (item[headerValue].value1 === '' ||
          item[headerValue].value2 === '' ||
          value1 === 0 ||
          value2 === 0)

      const isError = !isErrorCheckSkipped && value1 > value2

      // 項目毎にエラーフラグ・バリデーションエラーを設定
      if (headerValue === 'hourlyRate') {
        this.errorFlags[item.id].hourlyRate = isError
        this.personMonthRules[item.id] = isError
          ? [() => this.$t('common.rule.additionalRuleRangeNumber')]
          : []
      } else if (headerValue === 'pricePerPersonMonth') {
        this.errorFlags[item.id].pricePerPersonMonth = isError
        this.hourlyRateRules[item.id] = isError
          ? [() => this.$t('common.rule.additionalRuleRangeNumber')]
          : []
      }

      // 全エラーフラグを確認し、エラーがなければ親コンポーネントに値を渡す
      const hasErrors = Object.values(this.errorFlags).some((itemErrors) => {
        if (itemErrors.hourlyRate || itemErrors.pricePerPersonMonth) {
          return true
        }
        return false
      })

      if (!hasErrors) {
        this.$emit('changeInput', this.localItems)
      }
    },
    handleError(
      error: boolean,
      item: any,
      headerValue: string,
      valueKey: string | null | undefined = null
    ) {
      this.$emit('handle-error', item, headerValue, error, valueKey)
    },
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
    changeInput() {
      this.$emit('changeInput', this.localItems)
    },
  },
  mounted(this: any) {
    this.setScrollHint(this.$t('common.table.scrollable') as string)
  },
})
</script>

<style lang="scss">
.o-common-data-table {
  a {
    color: $c-primary-dark !important;
  }
  &.is-scroll {
    th,
    td {
      white-space: nowrap !important;
    }
  }
  .v-data-table {
    background-color: $c-black-page-bg;
    color: $c-black;
    tr {
      background-color: $c-white;
      td {
        border-bottom: 0 !important;
      }
      &:nth-child(even) {
        background-color: $c-black-table-border;
      }
    }
    .v-data-table-header {
      th,
      td {
        background-color: $c-black-80;
        color: $c-white !important;
      }
      .v-icon {
        color: $c-gray-line-dark !important;
        margin-left: 5px !important;
        opacity: 1;
      }
      th {
        &.active {
          .v-icon {
            color: $c-primary !important;
          }
        }
      }
    }
    .v-data-table__wrapper {
      border-radius: 4px;
      box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
        0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
      table {
        tbody {
          tr {
            transition-property: background-color;
            transition-duration: 0.2s;
            &:hover,
            &:focus {
              background: $c-primary-8 !important;
              a {
                color: $c-primary-over !important;
              }
            }
            td {
              font-size: 0.75rem;
              padding: 16px;
            }
          }
        }
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
  .v-data-footer {
    width: 100%;
    padding: 0 !important;
    justify-content: flex-end;
    border: 0 !important;
    position: relative;
  }
  .v-data-footer__select {
    display: none !important;
  }
  .v-data-table__wrapper {
    .v-data-footer {
      width: 100%;
    }
  }
  .v-data-footer__pagination {
    font-size: 0.875rem;
    position: absolute;
    left: 0;
    margin: 0;
  }
  .v-data-footer__icons-before,
  .v-data-footer__icons-after {
    .v-btn {
      width: 42px;
      height: 42px;
      box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
        0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
      border-radius: 4px;
      transition-property: background-color;
      transition-duration: 0.2s;
      .v-ripple__container,
      &::before {
        display: none !important;
      }
      &:hover,
      &:focus {
        background-color: $c-primary-over;
        .v-icon {
          color: $c-white !important;
        }
      }
    }
    .v-icon {
      width: 30px;
      height: 30px;
      font-size: 30px;
      color: $c-primary-dark !important;
    }
    .v-btn--disabled {
      .v-icon {
        color: #8f8f8f !important;
      }
    }
  }
  .o-only-page-text {
    padding-top: 40px !important;
    .v-data-footer__icons-before,
    .v-data-footer__icons-after {
      display: none;
    }
  }
}
.o-common-data-table__link {
  color: $c-primary-dark !important;
  font-weight: bold;
}
// .o-common-data-table__number {
//   margin: 0 !important;
// }
// .o-common-data-table__header {
//   display: flex;
//   justify-content: space-between;
//   align-items: flex-end;
//   margin-bottom: 16px;
// }
// .o-common-data-table__footer {
//   display: flex;
//   justify-content: flex-end;
//   margin-top: 16px;
// }
.text-right {
  input {
    text-align: right;
  }
}
</style>
