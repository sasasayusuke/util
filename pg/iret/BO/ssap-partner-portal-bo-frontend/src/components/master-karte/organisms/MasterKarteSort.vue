<template>
  <dl id="master-karte-sort">
    <dt class="headline-title">案件関連の条件：</dt>
    <dd>
      <v-row class="main-sort-row">
        <Sheet width="240" class="mr-5">
          <TextFieldLabel
            :label="$t('master-karte.pages.list.sort.clientName')"
          />
          <AutoComplete
            :value="localSearchParams.customerId"
            outlined
            dense
            style-set="bgWhite"
            :items="autoCompleteItems"
            item-text="name"
            item-value="id"
            :placeholder="$t('common.placeholder.autoComplete')"
            hide-details
            :loading="issuggestCustomersLoading"
            :max-length="255"
            @input="localSearchParams.customerId = $event"
          />
        </Sheet>
        <Sheet width="260">
          <TextFieldLabel :label="$t('karte.pages.index.sort_input.support')" />
          <Sheet width="260" class="d-flex align-center">
            <MonthSelect
              v-model="localSearchParams.supportDateFrom"
              :allowed-dates="allowedDatesFrom"
              style-set="bgWhite"
            />
            <span class="mx-2">〜</span>
            <MonthSelect
              v-model="localSearchParams.supportDateTo"
              :allowed-dates="allowedDatesTo"
              is-no-icon
              style-set="bgWhite"
            />
          </Sheet>
        </Sheet>
      </v-row>
      <v-expansion-panels id="master-karte-sort-panel" v-model="isOpen" flat>
        <v-expansion-panel style="background: #e3e3e3 !important">
          <v-expansion-panel-header id="sort-panel-headline"
            ><p>{{ $t('master-karte.pages.list.sort.sort-panel.title') }}</p>
            <span v-if="isOpen !== 0" id="expansion-panel-header-desc">{{
              $t('master-karte.pages.list.sort.sort-panel.description.close')
            }}</span>
            <span v-else id="expansion-panel-header-desc">{{
              $t('master-karte.pages.list.sort.sort-panel.description.open')
            }}</span>
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <div class="text-fields-row mb-6">
              <TextFieldLabel
                :label="
                  $t(
                    'master-karte.pages.list.sort.sort-panel.radio-button-label.headline'
                  )
                "
              />
              <RadioGroup
                v-model="localDetailSearchParams.isCurrentProgram"
                :values="radioButtonValues"
                :labels="radioButtonLabels"
                :horizontal="true"
                :bg-white="true"
              />
            </div>
            <v-row class="mb-2">
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[0]')
                  "
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                />
                <Select
                  v-model="localDetailSearchParams.category"
                  :items="categoryItems"
                  item-text="label"
                  item-value="value"
                  :placeholder="$t('common.placeholder.selectMulti2')"
                  style-set="outlined bgWhite bgTransparent"
                  :bg-transparent="true"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                  :multiple="true"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[1]')
                  "
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                />
                <Select
                  v-model="localDetailSearchParams.industrySegment"
                  :items="industrySegmentItems"
                  item-text="label"
                  item-value="value"
                  :placeholder="$t('common.placeholder.selectMulti2')"
                  style-set="outlined bgWhite bgTransparent"
                  :bg-transparent="true"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                  :multiple="true"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[2]')
                  "
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.departmentName"
                  :max-length="20"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[2]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined bgTransparent"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[1]
                  "
                  :transmission-disabled="true"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[3]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.currentSituation"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[3]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[4]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.issue"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[4]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[5]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.customerSuccess"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[5]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined"
                />
              </v-col>
              <v-col cols="4" class="lineup-autocomplete-col">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[6]')
                  "
                />
                <AutoComplete
                  :value="localDetailSearchParams.lineup"
                  outlined
                  dense
                  :items="lineupItems"
                  item-text="label"
                  item-value="value"
                  :placeholder="$t('common.placeholder.selectMulti3')"
                  hide-details
                  :max-length="255"
                  :multiple="true"
                  show-icon
                  @input="localDetailSearchParams.lineup = $event"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[7]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.requiredPersonalSkill"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[7]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined bgTransparent"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[0]
                  "
                  :transmission-disabled="true"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[8]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.requiredPartner"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[8]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined bgTransparent"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[0]
                  "
                  :transmission-disabled="true"
                />
              </v-col>
              <v-col cols="4">
                <TextFieldLabel
                  :label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[9]')
                  "
                />
                <TextField
                  v-model="localDetailSearchParams.strength"
                  :max-length="50"
                  :min-length="2"
                  :aria-label="
                    $t('master-karte.pages.list.sort.sort-panel.labels[9]')
                  "
                  :placeholder="
                    $t('master-karte.pages.list.sort.sort-panel.placeholder')
                  "
                  style-set="outlined bgTransparent"
                  :disabled="
                    localDetailSearchParams.isCurrentProgram ===
                    radioButtonValues[0]
                  "
                  :transmission-disabled="true"
                />
              </v-col>
            </v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
      <div class="button-list-row">
        <div class="mr-6">
          <Button
            class="search-button"
            style-set="large-primary"
            :disabled="searchDisabled"
            @click="$emit('handleSearch')"
          >
            {{ $t('common.button.sort') }}
          </Button>
        </div>
        <div>
          <Button style-set="text" @click="handleReset">
            {{ $t('common.button.reset') }}
          </Button>
        </div>
      </div>
    </dd>
  </dl>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import {
  AutoComplete,
  TextField,
  Sheet,
  Button,
  Select,
  RadioGroup,
} from '~/components/common/atoms'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'
import TextFieldLabel from '~/components/master-karte/atoms/TextFieldLabel.vue'
import { SuggestCustomersResponse, SuggestCustomers } from '~/models/Customer'
import MonthSelect from '~/components/common/molecules/MonthSelect.vue'
import {
  GetMasterKartenSelectBox,
  MasterKarteListSearchParams,
  MasterKarteListDetailSearchParams,
} from '~/models/MasterKarte'
import { masterKarteListUrlStore } from '~/store'

export default BaseComponent.extend({
  name: 'MasterKarteSort',
  components: {
    CommonSort,
    AutoComplete,
    TextField,
    Sheet,
    Button,
    TextFieldLabel,
    MonthSelect,
    Select,
    RadioGroup,
  },
  props: {
    /** 案件関連の条件 */
    searchParams: {
      type: MasterKarteListSearchParams,
      required: true,
    },
    /** マスターカルテ関連の条件 */
    detailSearchParams: {
      type: MasterKarteListDetailSearchParams,
      required: true,
    },
  },
  data() {
    return {
      /** 案件関連の条件（変更用） */
      localSearchParams: this.searchParams,
      /** マスターカルテ関連の条件（変更用） */
      localDetailSearchParams: this.detailSearchParams,
      /** サジェスト一覧 */
      autoCompleteItems: new SuggestCustomersResponse(),
      /** 取引先のサジェスト取得 ローディングフラグ */
      issuggestCustomersLoading: false,
      /** セレクターアイテム取得 ローディングフラグ */
      isSelectBoxLoading: false,
      /** 検索対象のラジオボタン ラベル一覧 */
      radioButtonLabels: [
        this.$t(
          'master-karte.pages.list.sort.sort-panel.radio-button-label.current'
        ),
        this.$t(
          'master-karte.pages.list.sort.sort-panel.radio-button-label.next'
        ),
      ],
      /** 検索対象のラジオボタン バリュー一覧 */
      radioButtonValues: [true, false],
      /** 顧客セグメントのセレクターアイテム一覧 */
      categoryItems: [],
      /** 業界セグメントのセレクターアイテム一覧 */
      industrySegmentItems: [],
      /** ラインナップのセレクターアイテム一覧 */
      lineupItems: [],
      /** アコーディオン開閉フラグ */
      isOpen: undefined,
      /** 絞り込み検索の非活性フラグ */
      searchDisabled: false,
      /** 「支援期間」From */
      localFrom: format(getCurrentDate(), 'yyyy/MM'),
      /** 「支援期間」To */
      localTo: '',
    }
  },
  created() {
    this.suggestCustomers()
    this.getMasterKartenSelectBox()
  },
  methods: {
    /** 前後5年のみを選択可能とする */
    allowedFiveYears(val: Date) {
      const beforeDate = getCurrentDate()
      const afterDate = getCurrentDate()
      beforeDate.setDate(beforeDate.getDate() - 1825)
      afterDate.setDate(afterDate.getDate() + 1825)

      const yearMonth = Number(format(new Date(val), 'yyyyMM'))
      const minYearMonth = Number(format(beforeDate, 'yyyyMM'))
      const maxYearMonth = Number(format(afterDate, 'yyyyMM'))

      return maxYearMonth >= yearMonth && minYearMonth <= yearMonth
    },
    /**
     * 「支援期間」開始日時が「支援期間」修了日時よりも前ならtrueを返す
     * @param val 絞り込みパラメータの「支援期間」開始日時
     */
    allowedDatesFrom(val: Date) {
      if (this.localTo !== '') {
        const inputDate = new Date(val)
        const toDate = new Date(this.localTo)
        return inputDate < toDate && this.allowedFiveYears(val)
      } else {
        return this.allowedFiveYears(val)
      }
    },
    /**
     * 「支援期間」修了日時が「支援期間」開始日時よりも後ならtrueを返す
     * @param val 絞り込みパラメータの「支援期間」修了日時
     */
    allowedDatesTo(val: Date) {
      if (this.localFrom !== '') {
        const inputDate = new Date(val)
        const fromDate = new Date(this.localFrom)
        return inputDate > fromDate && this.allowedFiveYears(val)
      } else {
        return this.allowedFiveYears(val)
      }
    },
    /** リセットボタン押下時の処理 */
    handleReset() {
      this.localSearchParams = new MasterKarteListSearchParams()
      this.localDetailSearchParams = new MasterKarteListDetailSearchParams()

      // vuexに保存されたURLのパラメーターを削除
      masterKarteListUrlStore.clear()
      this.$emit('handleReset')
    },
    /**
     * SuggestCustomersAPIを叩き、取引先のサジェスト用情報を取得
     */
    async suggestCustomers() {
      this.issuggestCustomersLoading = true

      await SuggestCustomers().then((res) => {
        this.autoCompleteItems = res.data
        this.issuggestCustomersLoading = false
      })
    },
    /** 親コンポーネントに変更用イベントを発火 */
    updateDetailSearchParams(newObject: MasterKarteListDetailSearchParams) {
      this.$emit('update-detail-search-params', newObject)
    },
    /** 取得したセレクトボックスから該当のアイテムを取得 */
    findSelectBox(object: any, name: string) {
      const foundObject = object.find((obj: any) => obj.name === name)
      return foundObject.items
    },
    /**
     * 各セレクトボックスのサジェストアイテムを取得
     */
    async getMasterKartenSelectBox(): Promise<void> {
      this.isSelectBoxLoading = true

      await GetMasterKartenSelectBox().then((res) => {
        this.categoryItems = this.findSelectBox(res.data, 'category')
        this.industrySegmentItems = this.findSelectBox(
          res.data,
          'industrySegment'
        )
        this.lineupItems = this.findSelectBox(res.data, 'lineup')
        this.isSelectBoxLoading = false
      })
    },
    /** マスターカルテ関連の条件の各項目が入力されていた場合2文字以上かチェック */
    /** 入力かつ2文字以内であれば非活性化 */
    checkMasterKarteDetailSearchParams(): void {
      for (const key in this.localDetailSearchParams) {
        if (key === 'isCurrentProgram') continue
        if (key === 'category') continue
        if (key === 'industrySegment') continue
        if (key === 'lineup') continue
        // @ts-ignore
        if (this.localDetailSearchParams[key] !== '') {
          // @ts-ignore
          const length = this.localDetailSearchParams[key].length
          if (length < 2 || length > 50) {
            this.searchDisabled = true
            break
          } else {
            this.searchDisabled = false
          }
        } else {
          this.searchDisabled = false
        }
      }
    },
  },
  watch: {
    /**
     * localSearchParamsの変更を親コンポーネントに変更を通知
     */
    localSearchParams: {
      handler(newObject: any) {
        this.$emit('update-search-params', newObject)
        this.localFrom = newObject.supportDateFrom
        this.allowedDatesFrom(newObject.supportDateFrom)
        this.localTo = newObject.supportDateTo
        this.allowedDatesTo(newObject.supportDateTo)
      },
      deep: true,
    },
    /**
     * localDetailSearchParamsの変更を親コンポーネントに変更を通知
     */
    localDetailSearchParams: {
      handler(newObject: MasterKarteListDetailSearchParams) {
        this.$emit('update-detail-search-params', newObject)
        this.checkMasterKarteDetailSearchParams()
      },
      deep: true,
    },
  },
})
</script>

<style lang="scss" scoped>
#master-karte-sort {
  width: 100%;
  height: 100%;
  padding: 1rem;
  margin: 1rem 0 2rem 0;
  box-sizing: border-box;
  background-color: #f0f0f0;

  .headline-title {
    font-size: 1rem;
    font-weight: bold;
  }

  .main-sort-row {
    width: 100%;
    display: flex;
    align-items: center;
    margin: 1rem 0 2rem 0;
    box-sizing: border-box;
  }

  .button-list-row {
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 2rem 0 1rem 0;
  }
}

#master-karte-sort-panel {
  margin: 1rem 0;

  #sort-panel-headline {
    position: relative;

    p {
      font-size: 1rem;
      font-weight: 600;
      margin-bottom: 0;
    }

    #expansion-panel-header-desc {
      font-size: 0.75rem;
      margin-bottom: 0;
      position: absolute;
      top: 50%;
      right: 55px;
      transform: translateY(-50%);
      -webkit-transform: translateY(-50%);
      -ms-transform: translateY(-50%);
    }
  }
}

.search-button {
  padding: 0 5rem !important;
}
</style>

<style lang="scss">
.lineup-autocomplete-col {
  .v-input__slot {
    height: 32px !important;
    max-height: 32px !important;
  }

  .v-input__append-inner {
    margin-top: 4px !important;
  }

  .v-select__selections {
    flex-wrap: nowrap !important;
  }
}
</style>
