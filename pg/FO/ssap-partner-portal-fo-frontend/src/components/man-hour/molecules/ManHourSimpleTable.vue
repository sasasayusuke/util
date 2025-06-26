<template>
  <v-form v-model="isValid" @input="$listeners['input']">
    <div class="o-man-hour-list-table">
      <SimpleTable class="xo-data-table elevation-1" :type="'manHour'">
        <thead>
          <tr>
            <th
              v-for="(header, index) in headers[type]"
              :key="index"
              :class="'text-left ' + 'a-th-' + header.value"
            >
              {{ header.text
              }}<span v-if="isEditing && header.required">*</span>
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-if="tableData().items">
            <tr v-for="(item, index_i) in tableData().items" :key="index_i">
              <td
                v-for="(header, index_j) in headers[type]"
                :key="index_j"
                :class="'a-td-' + header.value"
              >
                <template v-if="header.value === 'projectName'">
                  <template v-if="type === 'salesSupportManHours' && isEditing">
                    <TextField
                      v-model="localManHourData.items[index_i][header.value]"
                      role="textbox"
                      maxlength="120"
                      outlined
                      dense
                      @input="onChange()"
                    />
                  </template>
                  <template
                    v-else-if="type === 'salesSupportManHours' && !isEditing"
                  >
                    <span class="font-weight-bold">{{
                      item[header.value]
                    }}</span>
                  </template>
                  <template v-else>
                    <nuxt-link
                      class="o-admin-table__link m-manhour-table__link"
                      :to="forwardToUrl(`/project/${item.projectId}`)"
                    >
                      <span class="font-weight-bold">
                        {{ item[header.value] }}
                      </span>
                    </nuxt-link>
                  </template>
                </template>

                <template
                  v-else-if="header.value === 'customerNameAndProjectName'"
                >
                  <nuxt-link
                    class="o-admin-table__link m-manhour-table__link"
                    :to="forwardToUrl(`/project/${item.projectId}`)"
                  >
                    {{ item.customerName }}／{{ item.projectName }}
                  </nuxt-link>
                </template>

                <template
                  v-else-if="
                    type === 'directSupportManHours' &&
                    header.value === 'inputManHour'
                  "
                >
                  <template v-if="isEditing">
                    <v-row class="align-center ma-0">
                      <Sheet width="80">
                        <TextField
                          v-model.number="
                            localManHourData.items[index_i][header.value]
                          "
                          role="textbox"
                          :maxlength="6"
                          type="number"
                          :max-digits="3"
                          :decimal-number-digits="2"
                          :required="
                            localManHourData.items[index_i][header.value] !== 0
                              ? true
                              : false
                          "
                          number
                          outlined
                          dense
                          min="0"
                          max="999"
                          @input="onChange()"
                        />
                      </Sheet>
                      <span class="ml-2">h</span>
                      <span class="ml-4"
                        >({{
                          $t(
                            'man-hour.tables.directSupportManHours.karteManHour'
                          )
                        }}{{ item.karteManHour }}h)</span
                      >
                    </v-row>
                  </template>
                  <template v-else>
                    {{ item[header.value] + 'h' }}
                    <span class="ml-4"
                      >({{
                        $t(
                          'man-hour.tables.directSupportManHours.karteManHour'
                        )
                      }}{{ item.karteManHour }}h)</span
                    >
                  </template>
                </template>

                <template
                  v-else-if="
                    type === 'preSupportManHours' &&
                    header.value === 'inputManHour'
                  "
                >
                  <template v-if="isEditing">
                    <v-row class="align-center ma-0">
                      <Sheet width="80">
                        <TextField
                          v-model.number="
                            localManHourData.items[index_i][header.value]
                          "
                          role="textbox"
                          type="number"
                          :maxlength="6"
                          :max-digits="3"
                          :decimal-number-digits="2"
                          :required="
                            localManHourData.items[index_i][header.value] !== 0
                              ? true
                              : false
                          "
                          number
                          outlined
                          dense
                          min="0"
                          max="999"
                          @input="onChange()"
                        />
                      </Sheet>
                      <span class="ml-2">h</span>
                    </v-row>
                  </template>
                  <template v-else>
                    {{ item[header.value] + 'h' }}
                  </template>
                </template>

                <template
                  v-else-if="
                    type === 'salesSupportManHours' &&
                    header.value === 'inputManHour'
                  "
                >
                  <template v-if="isEditing">
                    <v-row class="align-center ma-0">
                      <Sheet v-if="localManHourData.items" width="80">
                        <TextField
                          v-model.number="
                            localManHourData.items[index_i][header.value]
                          "
                          role="textbox"
                          type="number"
                          :maxlength="6"
                          :max-digits="3"
                          :decimal-number-digits="2"
                          :required="
                            localManHourData.items[index_i][header.value] !== 0
                              ? true
                              : false
                          "
                          number
                          outlined
                          dense
                          min="0"
                          max="999"
                          @input="onChange()"
                        />
                      </Sheet>
                      <span class="ml-2">h</span>
                      <Sheet class="ml-8 a-minus">
                        <Img
                          :src="
                            require('~/assets/img/icon/org-icon-button-minus-outline.svg')
                          "
                          width="30"
                          height="30"
                          alt="remove salesSupportManHours"
                          :class="
                            localManHourData.items.length === 1
                              ? 'disabled'
                              : ''
                          "
                          @click="removeSalesSupportManHours(index_i)"
                        />
                      </Sheet>
                      <Sheet
                        v-if="index_i == localManHourData.items.length - 1"
                        class="ml-2 a-plus"
                      >
                        <Img
                          :src="
                            require('~/assets/img/icon/org-icon-button-plus-outline.svg')
                          "
                          width="30"
                          height="30"
                          alt="add salesSupportManHours"
                          @click="addSalesSupportManHours"
                        />
                      </Sheet>
                    </v-row>
                  </template>
                  <template v-else>
                    {{ item[header.value] + 'h' }}
                  </template>
                </template>

                <template
                  v-else-if="
                    type == 'salesSupportManHours' &&
                    header.value === 'customerName' &&
                    isEditing
                  "
                >
                  <v-combobox
                    v-model="localManHourData.items[index_i][header.value]"
                    :items="suggestCustomerList"
                    maxlength="255"
                    dense
                    outlined
                    solo
                    hide-details
                    flat
                    class="m-manhour-combobox"
                    @change="onChange()"
                  ></v-combobox>
                </template>
                <template v-else-if="header.value === 'type'">
                  <template v-if="isEditing">
                    <Sheet width="100">
                      <Select
                        v-model="localManHourData.items[index_i][header.value]"
                        dense
                        outlined
                        :items="[
                          { text: $t('man-hour.types.new'), value: 'new' },
                          {
                            text: $t('man-hour.types.continuation'),
                            value: 'continuation',
                          },
                        ]"
                        item-text="text"
                        item-value="value"
                        @change="onChange()"
                      />
                    </Sheet>
                  </template>
                  <template v-else>
                    <template v-if="item[header.value] === 'new'">
                      {{ $t('man-hour.types.new') }}
                    </template>
                    <template v-else>
                      {{ $t('man-hour.types.continuation') }}
                    </template>
                  </template>
                </template>

                <template v-else>{{ item[header.value] }}</template>
              </td>
            </tr>
            <tr v-if="isEditing" class="m-manhour-simple-table-memo">
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">
                    <template v-if="isEditing">
                      <Sheet>
                        <Textarea
                          v-model="localManHourData.memo"
                          outlined
                          maxlength="500"
                          placeholder=""
                          @input="onChange()"
                        />
                      </Sheet>
                    </template>
                    <template v-else>{{ manHourData.memo }}</template>
                  </v-col>
                </v-row>
              </td>
            </tr>
            <!-- 1．対面支援：お客様に対する支援（直接）: メモ -->
            <tr
              v-if="
                type === 'directSupportManHours' &&
                !isEditing &&
                manHourData.memo
              "
              class="m-manhour-simple-table-memo"
            >
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">{{ manHourData.memo }}</v-col>
                </v-row>
              </td>
            </tr>
            <!-- 2．支援仕込：お客様に対する支援（仕込): メモ -->
            <tr
              v-if="
                type === 'preSupportManHours' && !isEditing && manHourData.memo
              "
              class="m-manhour-simple-table-memo"
            >
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">{{ manHourData.memo }}</v-col>
                </v-row>
              </td>
            </tr>
            <!-- 3．商談/提案準備：新規・継続案件獲得に向けた資料作成・調査: メモ -->
            <tr
              v-if="
                type === 'salesSupportManHours' &&
                !isEditing &&
                manHourData.memo
              "
              class="m-manhour-simple-table-memo"
            >
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">{{ manHourData.memo }}</v-col>
                </v-row>
              </td>
            </tr>
          </template>

          <template v-else>
            <tr v-for="(item, index_i) in manHourData" :key="index_i">
              <template v-if="index_i !== 'memo'">
                <td class="a-td-key--2 font-weight-bold">
                  {{ $t('man-hour.tables.' + type + '.menu.' + index_i) }}
                  <Required v-if="isEditing" />
                </td>
                <td class="a-td-value--2" :class="isEditing ? 'pa-0' : ''">
                  <template v-if="isEditing">
                    <v-row class="align-center ma-0">
                      <Sheet width="80">
                        <TextField
                          v-model.number="localManHourData[index_i]"
                          role="textbox"
                          type="number"
                          :maxlength="6"
                          :max-digits="3"
                          :decimal-number-digits="2"
                          :required="
                            localManHourData[index_i] !== 0 ? true : false
                          "
                          number
                          outlined
                          dense
                          min="0"
                          max="999"
                          @input="onChange()"
                        />
                      </Sheet>
                      <span class="ml-2">h</span>
                    </v-row>
                  </template>
                  <template v-else>{{ item }} h</template>
                </td>
              </template>
            </tr>
            <tr v-if="isEditing" class="m-manhour-simple-table-memo">
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">
                    <template v-if="isEditing">
                      <Sheet>
                        <Textarea
                          v-model="localManHourData.memo"
                          maxlength="500"
                          outlined
                          placeholder=""
                          @input="onChange()"
                        />
                      </Sheet>
                    </template>
                    <template v-else>{{ manHourData.memo }}</template>
                  </v-col>
                </v-row>
              </td>
            </tr>
            <!-- 4.内部業務：お客様を特定せずSony Acceleration Platformの業務で発生した工数: メモ -->
            <tr
              v-if="type === 'ssapManHours' && !isEditing && manHourData.memo"
              class="m-manhour-simple-table-memo"
            >
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">{{ manHourData.memo }}</v-col>
                </v-row>
              </td>
            </tr>
            <!-- 5．休憩・その他：有給休暇・休憩・雑務・その他Sony Acceleration Platformの業務以外で使用した工数: メモ -->
            <tr
              v-if="
                type === 'holidaysManHours' && !isEditing && manHourData.memo
              "
              class="m-manhour-simple-table-memo"
            >
              <td :colspan="headers[type].length">
                <v-row no-gutters>
                  <v-col cols="auto" class="py-2 pr-9 font-weight-bold">{{
                    $t('man-hour.tables.memo')
                  }}</v-col>
                  <v-col class="a-text py-2">{{ manHourData.memo }}</v-col>
                </v-row>
              </td>
            </tr>
          </template>
        </tbody>
      </SimpleTable>
    </div>
  </v-form>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import {
  SimpleTable,
  Sheet,
  TextField,
  Textarea,
  Select,
  Img,
  Required,
} from '~/components/common/atoms/index'

interface ManHourDataType {
  [key: string]: any
}

interface HeadersType {
  [key: string]: {
    text: string
    align: string
    sortable?: boolean
    value: string
    required?: boolean
  }[]
}

interface DataType {
  localManHourData: ManHourDataType
  headers: HeadersType
}

export default BaseComponent.extend({
  components: {
    SimpleTable,
    Sheet,
    TextField,
    Textarea,
    Select,
    Img,
    Required,
  },
  model: {
    prop: 'isValid',
    event: 'input',
  },
  props: {
    /**
     * 支援工数データ
     */
    manHourData: {
      type: Object as PropType<ManHourDataType>,
      default: {},
      required: false,
    },
    /**
     * 支援工数データの種類
     */
    type: {
      type: String,
      default: '',
      required: true,
    },
    /**
     * 編集中か否か
     */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /**
     * サジェストする顧客情報
     */
    suggestCustomers: {
      type: Array,
    },
    /**
     * データが有効か否か
     */
    isValid: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    /**
     * 編集中か否か
     */
    isEditing: {
      handler(newVal, oldVal) {
        if (newVal && !oldVal) {
          // 初期取り込み
          this.onChange()
        }
      },
      /**
       * immediateオプション
       */
      immediate: false,
    },
  },
  data(): DataType {
    return {
      localManHourData: cloneDeep(this.manHourData),
      headers: {
        directSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.customerNameAndProjectName'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'customerNameAndProjectName',
          },
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.role'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'role',
          },
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.inputManHour'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        preSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.customerNameAndProjectName'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'customerNameAndProjectName',
          },
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.role'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'role',
          },
          {
            text: this.$t(
              'man-hour.tables.directSupportManHours.texts.inputManHour'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        salesSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.salesSupportManHours.texts.projectName'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'projectName',
          },
          {
            text: this.$t(
              'man-hour.tables.salesSupportManHours.texts.customerName'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'customerName',
          },
          {
            text: this.$t(
              'man-hour.tables.salesSupportManHours.texts.type'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'type',
          },
          {
            text: this.$t(
              'man-hour.tables.salesSupportManHours.texts.inputManHour'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        ssapManHours: [
          {
            text: this.$t('man-hour.tables.ssapManHours.texts.key').toString(),
            align: 'start',
            sortable: false,
            value: 'key',
          },
          {
            text: this.$t(
              'man-hour.tables.ssapManHours.texts.value'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'value',
          },
        ],
        holidaysManHours: [
          {
            text: this.$t(
              'man-hour.tables.holidaysManHours.texts.key'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'key',
          },
          {
            text: this.$t(
              'man-hour.tables.holidaysManHours.texts.value'
            ).toString(),
            align: 'start',
            sortable: false,
            value: 'value',
          },
        ],
      },
    }
  },
  methods: {
    /**
     * 特定の配列の値を返す
     * @param 配列情報
     * @param 配列のkey名
     * @returns 配列の値
     */
    getItemValue(item: any, value: string) {
      return item[value]
    },
    /**
     * 入力値に変更があった場合の動作
     */
    onChange() {
      // v-comboboxが入力を消去してblurした時にnullになるので空文字に変換
      if (this.localManHourData.items) {
        this.localManHourData.items.forEach((obj: any) => {
          if (obj.customerName == null) {
            obj.customerName = ''
          }
        })
      }
      this.$emit('change', this.localManHourData)
    },
    /**
     * 支援工数のテーブルデータ
     * @returns 支援工数のテーブルデータ
     */
    tableData(): ManHourDataType {
      if (
        this.type === 'salesSupportManHours' &&
        this.localManHourData.items.length === 0
      ) {
        this.addSalesSupportManHours()
      }
      this.localManHourData = this.manHourData
      return this.isEditing ? this.localManHourData : this.notEditingManHour
    },
    /**
     * 案件情報を削除
     * @param 案件情報のインデックス番号
     */
    removeSalesSupportManHours(index: number) {
      this.localManHourData.items.splice(index, 1)
      this.onChange()
    },
    /**
     * 案件情報を追加
     */
    addSalesSupportManHours() {
      this.localManHourData.items.push({
        projectName: '',
        customerId: '',
        customerName: '',
        type: 'new',
        inputManHour: 0,
      })
      this.onChange()
    },
  },
  computed: {
    /**
     * サジェストする顧客情報リスト
     * @returns サジェストする顧客情報
     */
    suggestCustomerList(): string[] {
      return this.suggestCustomers.map((elm: any) => elm.name)
    },
    /**
     * 確認画面用に、新規かつ時間0の要素を削る（商談/提案準備用）
     * @returns 特定要素を削ったmanHour
     */
    notEditingManHour(): ManHourDataType {
      const manHour = cloneDeep(this.manHourData)
      if (!manHour.items) {
        return manHour
      }
      manHour.items = manHour.items.filter((item: any) => {
        // TODO: itemの型をなんとかする
        const isNew = item.type === 'new'
        const is0hr = item.inputManHour === 0
        if (isNew && is0hr) {
          return false
        }
        return true
      })
      return manHour
    },
  },
})
</script>
<style lang="scss" scoped>
.a-text {
  overflow-wrap: break-word;
  white-space: pre-wrap;
  word-break: break-word;
}
.a-th-projectName,
.a-td-projectName {
  width: 360px;
}
.a-th-inputManHour,
.a-td-inputManHour {
  width: 320px;
}
.a-td-inputManHour {
  font-size: 0.875rem !important;
  span {
    font-size: 0.75rem !important;
  }
}
.a-th-value,
.a-td-value--2 {
  width: 120px;
}
.a-td-key--2 {
  font-size: 0.875rem !important;
  font-weight: bold;
}
.a-td-value--2 {
  font-size: 0.875rem !important;
}
.a-th-type,
.a-td-type {
  width: 120px;
}
.a-minus,
.a-plus {
  .v-image {
    cursor: pointer;
    border-radius: 50%;
    &.disabled {
      pointer-events: none;
    }
  }
  &:hover,
  &:focus {
    .v-image {
      background-color: $c-primary-8;
      &.disabled {
        background-color: transparent;
      }
    }
  }
}
.m-manhour-table__link {
  @include fontSize('small');
  color: $c-primary-dark;
  font-weight: bold;
}
</style>
<style lang="scss">
.m-manhour-combobox {
  &.v-text-field--outlined {
    &:not(.v-input--is-focused) {
      &:not(.v-input--has-state) {
        .v-input__control {
          .v-input__slot {
            fieldset {
              background-color: $c-black-page-bg;
              color: $c-gray-line-dark;
            }
          }
        }
      }
    }
  }
  &.v-input {
    input {
      &::placeholder {
        color: $c-gray-line-dark;
      }
    }
  }
}
.o-man-hour-list-table {
  .v-data-table {
    box-shadow: none !important;
    border-bottom: 1px solid $c-gray-line;
    .v-data-table__wrapper {
      border-radius: 0px;
      box-shadow: none !important;
      table {
        tbody {
          tr {
            background: $c-white;
            &:nth-child(even) {
              background-color: $c-black-table-border;
            }
            &:not(:last-child) {
              td {
                &:not(.v-data-table__mobile-row) {
                  border-bottom: 0;
                }
              }
            }
          }
          tr:hover {
            background: $c-white !important;
            &:nth-child(even) {
              background-color: $c-black-table-border !important;
            }
          }
        }
      }
    }
    tr {
      &.m-manhour-simple-table-memo {
        td {
          background-color: $c-white;
          border-top: 1px solid $c-gray-line;
        }
        .row {
          flex-wrap: nowrap;
        }
      }
    }
  }
}
.a-simple-table--manHour {
  border-radius: 0;
}
</style>
