<template>
  <v-form v-model="isValid" @input="$listeners['input']">
    <div class="o-alert-list-table">
      <div class="o-alert-list-table__body">
        <SimpleTable class="o-data-table" :type="'manHour'">
          <thead>
            <tr>
              <th
                v-for="(header, index) in headers"
                :key="index"
                :class="'text-left ' + 'a-th-' + header.value"
              >
                {{ header.text }}<span v-if="header.required"> *</span>
              </th>
            </tr>
          </thead>
          <tbody :newAlertSetting="newAlertSettings">
            <template v-for="(item, index_i) in serviceTypes">
              <tr :key="index_i">
                <!-- サービス名 -->
                <td>
                  <strong>{{ item.name }}</strong>
                </td>
                <!-- 対面支援工数 -->
                <td>
                  <v-row class="align-center ma-0">
                    <span>{{
                      $t('man-hour.pages.alert.config.item.contractTime')
                    }}</span>
                    <Sheet width="85" class="ml-2">
                      <TextField
                        v-model.number="
                          localAlertSettings.attributes.factorSetting[index_i]
                            .directSupportManHour
                        "
                        type="number"
                        aria-label="%"
                        outlined
                        dense
                        required
                        :max-digits="6"
                        :positive-number-digits="3"
                        :decimal-number-digits="2"
                        @input="onChange"
                      />
                    </Sheet>
                    <span class="ml-2">%</span>
                  </v-row>
                </td>
                <!-- 対面支援工数＋対面支援仕込工数 -->
                <td>
                  <v-row class="align-center ma-0">
                    <span>{{
                      $t('man-hour.pages.alert.config.item.totalManHour')
                    }}</span>
                    <Sheet width="85" class="ml-2">
                      <TextField
                        v-model.number="
                          localAlertSettings.attributes.factorSetting[index_i]
                            .directAndPreSupportManHour
                        "
                        type="number"
                        aria-label="%"
                        outlined
                        dense
                        required
                        :max-digits="6"
                        :positive-number-digits="3"
                        :decimal-number-digits="2"
                        @input="onChange"
                      />
                    </Sheet>
                    <span class="ml-2">%</span>
                  </v-row>
                </td>
              </tr>
            </template>
          </tbody>
        </SimpleTable>
      </div>
    </div>
  </v-form>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import {
  SimpleTable,
  Sheet,
  TextField,
  Textarea,
  Select,
  Img,
} from '~/components/common/atoms/index'
import {
  FactorSetting,
  GetAlertSettingsResponse,
  ServiceTypeItems,
} from '~/models/Master'

export default BaseComponent.extend({
  name: 'AlertSimpleTable',
  components: {
    SimpleTable,
    Sheet,
    TextField,
    Textarea,
    Select,
    Img,
  },
  model: {
    prop: 'isValid',
    event: 'input',
  },
  props: {
    /** 編集中判定 */
    isEditing: {
      type: Boolean,
      default: false,
    },
    /** サービス種別一覧情報 */
    serviceTypes: {
      type: Array as PropType<ServiceTypeItems[]>,
    },
    /** 工数アラート設定 */
    alertSettings: {
      type: Object as PropType<GetAlertSettingsResponse>,
    },
    /** バリデーション判定 */
    isValid: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    alertSettings: {
      handler(newVal, oldVal) {
        if (newVal !== oldVal) {
          this.initialize()
        }
      },
      immediate: true,
      deep: true,
    },
  },
  data() {
    return {
      headers: [
        {
          text: this.$t('man-hour.pages.index.serviceTypeName') as string,
          align: 'start',
          sortable: false,
          value: 'serviceTypeName',
        },
        {
          text: this.$t('man-hour.pages.index.faceToFaceSupport') as string,
          align: 'start',
          sortable: false,
          value: 'A',
          required: true,
        },
        {
          text: this.$t(
            'man-hour.pages.index.plusSupportPreparation'
          ) as string,
          align: 'start',
          sortable: false,
          value: 'B',
          required: true,
        },
      ],
      localAlertSettings: this.alertSettings,
    }
  },
  computed: {
    /** localAlertSettingsを生成する */
    newAlertSettings() {
      this.serviceTypes.forEach((elm: ServiceTypeItems, index: number) => {
        if (!this.localAlertSettings.attributes.factorSetting[index]) {
          // アラート情報が空のオブジェクトを作成
          const newAlertSetting = new FactorSetting()
          newAlertSetting.serviceTypeId = elm.id

          this.localAlertSettings.attributes.factorSetting.push(newAlertSetting)
        }
      })
    },
  },
  methods: {
    /** 初期化 */
    initialize() {
      this.localAlertSettings = this.alertSettings
      this.onChange()
    },
    onChange() {
      if (this.localAlertSettings.attributes) {
        this.$emit('change', this.localAlertSettings.attributes)
      }
    },
  },
})
</script>
<style lang="scss" scoped>
.o-alert-list-table {
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
.a-text {
  white-space: pre-wrap;
  overflow-wrap: break-word;
}
.a-th-projectName,
.a-td-projectName {
  width: 360px;
}
.a-th-inputManHour,
.a-td-inputManHour {
  width: 320px;
}
.a-th-value,
.a-td-value--2 {
  width: 120px;
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
  &:hover {
    .v-image {
      background-color: $c-primary-8;
      &.disabled {
        background-color: transparent;
      }
    }
  }
}
</style>
