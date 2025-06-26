<template>
  <div class="m-man-hour-simple-table">
    <SimpleTable class="o-data-table" :type="'manHour'">
      <thead>
        <tr>
          <th
            v-for="(header, index) in headers[type]"
            :key="index"
            :class="'text-left ' + 'a-th-' + header.value"
          >
            {{ header.text }}
          </th>
        </tr>
      </thead>
      <tbody>
        <!-- 4．内部業務：お客様を特定せずSony Acceleration Platformの業務で発生した工数, 5．休憩・その他：有給休暇・休憩・雑務・その他Sony Acceleration Platformの業務以外で使用した工数 -->
        <template v-if="type === 'ssapManHours' || type === 'holidaysManHours'">
          <template v-for="(hour, key) in manHourData">
            <tr v-if="key !== 'memo'" :key="key">
              <!-- 分類名 -->
              <td class="m-manhour-simple-table-category">
                {{ $t(`man-hour.tables.${type}.menu.${key}`) }}
              </td>
              <td>
                <!-- 確定時実績工数 -->
                <nuxt-link
                  v-if="isConfirm && isInputManHourLinkVisible()"
                  :to="`/`"
                  event=""
                  @click.native="clickModalOpenObj(key)"
                >
                  {{ hour }}h
                </nuxt-link>
                <!-- 未確定時実績工数 -->
                <span v-else>{{ hour }}h</span>
                <!-- 編集モーダル -->
                <ManHourModal
                  v-if="isModalOpenObj[key]"
                  :man-hour-by-supporter-user-id="manHourBySupporterUserId"
                  :local-man-hour-by-supporter-user-id="
                    localManHourBySupporterUserId
                  "
                  :local-man-hour-data="localManHourData"
                  :index="localIndex"
                  :type="type"
                  :number="hour"
                  @closeModal="closeModal"
                />
              </td>
            </tr>
          </template>
        </template>
        <tr v-for="(item, index_i) in tableData().items" :key="index_i">
          <template v-if="index_i !== 'memo'">
            <!-- 3．商談/提案準備：新規・継続案件獲得に向けた資料作成・調査 -->
            <template v-if="type === 'salesSupportManHours'">
              <!-- 案件名 -->
              <td class="a-td-value--2 a-td-projectName">
                {{ item.projectName }}
              </td>
              <!-- お客様名 -->
              <td class="a-td-value--2">
                {{ item.customerName }}
              </td>
              <!-- 分類 -->
              <td class="a-td-value--2">
                <!-- 新規 -->
                <template v-if="item.type === 'new'">
                  {{ $t('man-hour.types.new') }}
                </template>
                <!-- 継続 -->
                <template v-else>
                  {{ $t('man-hour.types.continuation') }}
                </template>
              </td>
            </template>
            <!-- 1．対面支援：お客様に対する支援（直接）, 2．支援仕込：お客様に対する支援（仕込）,2．支援仕込：お客様に対する支援（仕込） -->
            <template v-else>
              <td class="a-td-key--2">
                <!-- {{ $t('man-hour.tables.' + type + '.menu.' + index_i) }} -->
                <!-- お客様名／案件名 -->
                <nuxt-link :to="forwardToUrl(`/project/${item.projectId}`)">
                  {{ `${item.customerName}／${item.projectName}` }}
                </nuxt-link>
              </td>
              <!-- 役割 -->
              <td class="a-td-value--2">
                {{ item.role }}
              </td>
            </template>
            <!-- 実績工数 -->
            <td class="a-td-value--2">
              <nuxt-link
                v-if="isConfirm && isInputManHourLinkVisible()"
                :to="`/`"
                event=""
                @click.native="clickModalOpenArr((key = index_i))"
              >
                {{ item.inputManHour }}h
              </nuxt-link>
              <span v-else> {{ item.inputManHour }}h </span>
              <span v-if="item.karteManHour !== undefined"
                >（カルテ側の入力時間合計：{{ item.karteManHour }}h）</span
              >
              <ManHourModal
                v-if="isModalOpenArr[index_i]"
                :man-hour-by-supporter-user-id="manHourBySupporterUserId"
                :local-man-hour-by-supporter-user-id="
                  localManHourBySupporterUserId
                "
                :local-man-hour-data="localManHourData"
                :index="localIndex"
                :type="type"
                :number="item.inputManHour"
                @closeModal="closeModal"
              />
            </td>
          </template>
        </tr>
        <!-- 案件別工数詳細 -->
        <template v-if="type === 'alertProjectDetail'">
          <tr v-for="(item, index) in manHourData.manHours" :key="index">
            <td>{{ item.role }}</td>
            <td>{{ item.supporterUserName }}</td>
            <td>
              {{ nullCheck(item.summarySupporterDirectSupportManHours) }}h
            </td>
            <td>{{ nullCheck(item.summarySupporterPreSupportManHours) }}h</td>
            <td>
              {{ nullCheck(item.thisMonthSupporterDirectSupportManHours) }}h
            </td>
            <td>{{ nullCheck(item.thisMonthSupporterPreSupportManHours) }}h</td>
          </tr>
          <!-- 各項目での合計計算 -->
          <tr v-if="manHourData" class="o-data-table__total">
            <th>{{ $t('man-hour.pages.alertDetail.total') }}</th>
            <td>&nbsp;</td>
            <td>{{ sumComputed.summarySupporterDirectSupportManHours }}h</td>
            <td>{{ sumComputed.summarySupporterPreSupportManHours }}h</td>
            <td>{{ sumComputed.thisMonthSupporterDirectSupportManHours }}h</td>
            <td>{{ sumComputed.thisMonthSupporterPreSupportManHours }}h</td>
          </tr>
        </template>
        <!-- メモ -->
        <tr v-if="manHourData.memo" class="m-manhour-simple-table-memo">
          <td :colspan="headers[type].length">
            <v-row no-gutters>
              <v-col cols="2" class="a-title py-2">メモ:</v-col>
              <v-col cols="10" class="a-text py-2"
                >{{ manHourData.memo }}
              </v-col>
            </v-row>
          </td>
        </tr>
      </tbody>
    </SimpleTable>
  </div>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import BaseComponent from '~/common/BaseComponent'
import Button from '~/components/common/atoms/Button.vue'
import {
  SimpleTable,
  Sheet,
  TextField,
  Textarea,
  Select,
  Img,
} from '~/components/common/atoms/index'
import { meStore } from '~/store'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'

interface ManHourDataType {
  [key: string]: any
}

export default BaseComponent.extend({
  name: 'ManHourSimpleTable',
  components: {
    SimpleTable,
    Sheet,
    TextField,
    Textarea,
    Select,
    Img,
    Button,
  },
  props: {
    /** 表示項目で絞られた支援者単位での支援工数情報 */
    manHourData: {
      type: Object,
      default: {},
      required: false,
    },
    /** 支援者単位での支援工数情報 */
    manHourBySupporterUserId: {
      type: Object,
      default: {},
    },
    /** 表示項目 */
    type: {
      type: String,
      default: '',
      required: true,
    },
    /** 確定or未確定判定 */
    isConfirm: {
      type: Boolean,
    },
  },
  data() {
    return {
      localManHourData: cloneDeep(this.manHourData),
      localManHourBySupporterUserId: cloneDeep(this.manHourBySupporterUserId),
      isModalOpen: false,
      isModalOpenArr: [] as boolean[],
      isModalOpenObj: {} as { string: boolean },
      localIndex: '',
      headers: {
        directSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.headers.directSupportManHours.projectName'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'projectName',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.directSupportManHours.role'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'role',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.directSupportManHours.inputManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        preSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.headers.preSupportManHours.projectName'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'projectName',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.preSupportManHours.role'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'role',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.preSupportManHours.inputManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        salesSupportManHours: [
          {
            text: this.$t(
              'man-hour.tables.headers.salesSupportManHours.projectName'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'projectName',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.salesSupportManHours.customerName'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'customerName',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.salesSupportManHours.type'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'type',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.salesSupportManHours.inputManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'inputManHour',
            required: true,
          },
        ],
        ssapManHours: [
          {
            text: this.$t('man-hour.tables.headers.ssapManHours.key') as string,
            align: 'start',
            sortable: false,
            value: 'key',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.ssapManHours.value'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'value',
          },
        ],
        holidaysManHours: [
          {
            text: this.$t(
              'man-hour.tables.headers.holidaysManHours.key'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'key',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.holidaysManHours.value'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'value',
          },
        ],
        alertProjectDetail: [
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.serviceTypeName'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'serviceTypeName',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.mainSupporterUser'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'mainSupporterUser',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.summaryDirectSupportManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'summaryDirectSupportManHour',
            width: '212px',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.summaryPreSupportManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'summaryPreSupportManHour',
            width: '212px',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.thisMonthDirectSupportManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'thisMonthDirectSupportManHour',
          },
          {
            text: this.$t(
              'man-hour.tables.headers.alertProjectDetail.thisMonthPreSupportManHour'
            ) as string,
            align: 'start',
            sortable: false,
            value: 'thisMonthPreSupportManHour',
          },
        ],
      },
      customerNameList: ['customer1', 'customer2'],
    }
  },
  watch: {
    manHourData: {
      handler(newVal) {
        const _this = this
        if (newVal.items && newVal.items.length) {
          this.isModalOpenArr = Array(newVal.items.length)
          this.isModalOpenArr.fill(false)
        } else if (
          this.type === 'ssapManHours' ||
          this.type === 'holidaysManHours'
        ) {
          Object.keys(newVal).forEach((key: string) => {
            _this.$set(_this.isModalOpenObj, key, false)
          })
        }
      },
      deep: true,
      immediate: true,
    },
  },
  computed: {
    sumComputed(): any {
      return {
        summarySupporterDirectSupportManHours: this.summarize(
          'summarySupporterDirectSupportManHours'
        ),
        summarySupporterPreSupportManHours: this.summarize(
          'summarySupporterPreSupportManHours'
        ),
        thisMonthSupporterDirectSupportManHours: this.summarize(
          'thisMonthSupporterDirectSupportManHours'
        ),
        thisMonthSupporterPreSupportManHours: this.summarize(
          'thisMonthSupporterPreSupportManHours'
        ),
      }
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
  methods: {
    click(key: string) {
      this.isModalOpen = true
      this.localIndex = key
    },
    /**
     * 1．対面支援：お客様に対する支援（直接）,
     * 2．支援仕込：お客様に対する支援（仕込）,2．支援仕込：お客様に対する支援（仕込）
     * 3．商談/提案準備：新規・継続案件獲得に向けた資料作成・調査
     * 編集モーダルオープン
     */
    clickModalOpenArr(key: number) {
      this.localIndex = String(key)
      this.isModalOpenArr[key] = true
      this.$set(this.isModalOpenArr, key, true)
    },
    /**
     * 4．内部業務：お客様を特定せずSony Acceleration Platformの業務で発生した工数,
     * 5．休憩・その他：有給休暇・休憩・雑務・その他Sony Acceleration Platformの業務以外で使用した工数
     * 編集モーダルオープン
     */
    clickModalOpenObj(key: string) {
      if (!this.isModalOpen) {
        this.isModalOpen = true
        this.localIndex = key
        this.$set(this.isModalOpenObj, key, true)
      }
    },
    /**
     * 要素がNullの場合0を返す
     * @return 0 || 元の要素
     */
    nullCheck(item: number) {
      if (item == null) return 0
      else return item
    },
    /**
     * 入力値に変更があった場合の動作
     */
    onChange() {
      this.$emit('change', this.localManHourData)
    },
    /** 編集モーダルクローズ */
    closeModal() {
      const _this = this
      this.isModalOpen = false
      this.isModalOpenArr.splice(0, this.isModalOpenArr.length, false)
      if (Object.keys(this.isModalOpenObj).length) {
        Object.keys(this.isModalOpenObj).forEach((key) => {
          _this.$set(this.isModalOpenObj, key, false)
        })
      }
    },
    /**
     * 各項目毎合計時間計算
     * @param count 項目毎合計時間
     * @return 項目毎合計時間
     */
    summarize(item: string) {
      let count = 0
      if (this.manHourData.manHours) {
        this.manHourData.manHours.forEach((manHour: any) => {
          count += manHour[item]
        })
      }
      return count
    },
    /**
     * 支援工数のテーブルデータ
     * @returns 支援工数のテーブルデータ
     */
    tableData(): ManHourDataType {
      return this.notEditingManHour
    },
    /**
     * 担当別月次工数編集リンクを活性化するか否か
     * @return 活性化するのであればtrue
     */
    isInputManHourLinkVisible(): boolean {
      if (
        meStore.roles.includes(ENUM_ADMIN_ROLE.SYSTEM_ADMIN) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.MAN_HOUR_OPS)
      ) {
        // システム管理者、稼働率調査事務局
        return true
      } else {
        return false
      }
    },
  },
})
</script>
<style lang="scss">
.m-man-hour-simple-table {
  .v-data-table {
    border-bottom: 1px solid $c-gray-line;
    border-radius: 0;
    tr {
      &:nth-child(even) {
        background-color: $c-black-table-border;
      }
      &.m-manhour-simple-table-memo {
        td {
          background-color: $c-white;
          border-top: 1px solid $c-gray-line;
        }
      }
    }
    th {
      background-color: $c-black-80;
      color: $c-white !important;
      height: 32px !important;
    }
    td {
      border-bottom: 0;
      a {
        color: $c-primary-dark;
        font-weight: bold;
      }
    }
    .v-icon {
      color: $c-primary !important;
      margin-left: 5px !important;
    }
    .v-data-table__wrapper {
      table {
        tbody {
          tr {
            transition-property: background-color;
            transition-duration: 0.2s;
            &:not(:last-child) {
              td {
                &:not(.v-data-table__mobile-row) {
                  border-bottom: 0;
                }
              }
            }
            &:hover {
              &:not(.v-data-table__expanded__content) {
                &:not(.v-data-table__empty-wrapper) {
                  background: $c-primary-8;
                }
                &:last-child {
                  &:hover {
                    background: $c-white;
                  }
                }
              }
              a {
                color: $c-primary-over !important;
              }
            }
          }
        }
      }
    }
    .o-data-table__total {
      th,
      td {
        border-top: 1px solid $c-gray-line;
        background-color: $c-white !important;
      }
      th {
        background-color: $c-white !important;
        color: rgba(0, 0, 0, 0.87) !important;
        @include fontSize('normal');
      }
      td {
        @include fontSize('xlarge');
        color: $c-primary-dark !important;
        font-weight: bold;
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
}
.a-title {
  font-weight: bold;
}
.a-text {
  white-space: pre-wrap;
  overflow-wrap: break-word;
}
.a-th-projectName,
.a-td-projectName {
  width: 520px;
}
.a-td-projectName {
  font-weight: bold;
}
.a-th-inputManHour,
.a-td-inputManHour {
  width: 320px;
}
.a-th-role,
.a-td-role {
  width: 170px;
}
.a-th-value,
.a-td-value--2 {
  width: 120px;
}
.a-th-type,
.a-td-type {
  width: 120px;
}
.a-th-summaryDirectSupportManHour,
.a-th-summaryPreSupportManHour {
  width: 212px;
}
.a-th-thisMonthDirectSupportManHour,
.a-th-thisMonthPreSupportManHour {
  width: 152px;
}
.a-th-serviceTypeName {
  width: 172px;
}
.m-manhour-simple-table-category {
  font-weight: bold;
}
</style>
