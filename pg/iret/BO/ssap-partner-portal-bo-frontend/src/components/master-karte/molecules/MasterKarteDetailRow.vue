<template>
  <div>
    <v-row v-show="subtitle" class="row-subtitle">{{ subtitle }} </v-row>
    <v-row
      class="o-common-detail-rows__unit"
      :class="[disabled ? 'is-disabled' : '']"
    >
      <v-col md="auto" class="icon">
        <div v-if="icon === 'void'" style="width: 16px"></div>
        <svg
          v-else
          xmlns="http://www.w3.org/2000/svg"
          width="17"
          height="13"
          viewBox="0 0 17 13"
        >
          <path
            d="M11.409 9.555a4.4 4.4 0 0 0 0-5.044A2.843 2.843 0 0 1 13.05 4a3.034 3.034 0 0 1 0 6.067 2.843 2.843 0 0 1-1.641-.511M4.975 7.033a2.976 2.976 0 1 1 2.975 3.034 3 3 0 0 1-2.975-3.034m1.7 0a1.275 1.275 0 1 0 1.275-1.3 1.288 1.288 0 0 0-1.275 1.3m7.225 8.234V17H2v-1.733S2 11.8 7.95 11.8s5.95 3.467 5.95 3.467m-1.7 0c-.119-.676-1.13-1.733-4.25-1.733s-4.19 1.135-4.25 1.733M13.857 11.8a4.64 4.64 0 0 1 1.743 3.467V17H19v-1.733s0-3.146-5.151-3.467z"
            transform="translate(-2 -4)"
            style="fill: #1867c0"
          />
        </svg>
      </v-col>
      <v-col
        :cols="cols"
        align-self="start"
        class="o-common-detail-rows__title pr-4"
      >
        <slot name="label">
          {{ label }}
        </slot>
      </v-col>
      <v-col align-self="start" class="o-common-detail-rows__data">
        <div class="o-common-detail-rows__data__text">
          <slot name="isNotEditing">
            <!-- eslint-disable-next-line vue/no-v-html -->
            <span v-if="!escapeValue" v-html="$sanitize(value)"></span>
            <span v-else class="master-karte-text">{{ value }}</span>
            <!-- 次期支援選択時 -->
            <div v-if="!isCurrentProgram" style="display: flex">
              <!-- 次期支援 -->
              <div
                style="margin-right: 28px"
                :style="showCurrentProgram ? 'width: 58%' : 'width: 100%'"
              >
                <!-- プログラム利用履歴 -->
                <div v-if="usageHistory">
                  <ul v-for="item in usageHistory" :key="item.npfProjectId">
                    <li class="master-karte-text__link master-karte-text">
                      <a :href="'/master-karte/' + item.npfProjectId">
                        {{ projectName(item.serviceType, item.projectName) }}
                      </a>
                    </li>
                  </ul>
                </div>
                <!-- その他の項目 -->
                <div v-else>
                  <span :class="{ 'pre-wrap-text': text !== '' }">{{
                    text
                  }}</span>
                </div>
              </div>
              <!-- 当期支援 -->
              <div
                v-show="showCurrentProgram"
                style="width: 38%; color: #666; font-size: 12px"
              >
                {{
                  $t('master-karte.pages.detail.container.currentProgramTitle')
                }}<br />
                {{ currentText }}
              </div>
            </div>

            <!-- 当期支援 -->
            <div v-if="isCurrentProgram">
              <!-- URL -->
              <span
                v-if="text && isUrl"
                class="master-karte-text master-karte-text__link"
              >
                <a target="_blank" :href="text">
                  {{ text }}
                </a>
                <a
                  class="ml-1"
                  target="_blank"
                  :href="text"
                  style="border: none"
                >
                  <Icon size="15" color="btn_primary" class=""
                    >icon-org-blank</Icon
                  >
                </a>
              </span>
              <!-- プログラム利用履歴 -->
              <div v-if="!isUrl && usageHistory">
                <ul v-for="item in usageHistory" :key="item.npfProjectId">
                  <li class="master-karte-text__link master-karte-text">
                    <a :href="'/master-karte/' + item.npfProjectId">
                      {{ projectName(item.serviceType, item.projectName) }}
                    </a>
                  </li>
                </ul>
              </div>
              <!-- その他 -->
              <span
                v-if="text && !isUrl && !usageHistory"
                class="master-karte-text"
                style="white-space: pre-wrap"
                >{{ text }}</span
              >
              <!-- 終了アンケート -->
              <span v-if="surveyId" class="master-karte-text__link">
                <a :href="completedSurveyUrl">{{
                  $t(
                    'master-karte.pages.detail.container.result.displayCompletedSurveyDetail'
                  )
                }}</a>
              </span>
              <!-- 満足度5段階評価 -->
              <v-radio-group v-if="radios" v-model="selectedRadio" row readonly>
                <v-radio
                  v-for="radio in radios"
                  :key="radio.title"
                  :label="radio.title"
                  :value="radio.title.slice(0, 1)"
                  class="satisfaction__radio"
                  disabled
                  active-class="is-active"
                >
                </v-radio>
              </v-radio-group>
            </div>
          </slot>
        </div>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { ToolTips, Chip, Icon } from '../../common/atoms/index'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { UsageHistoryClass } from '~/models/MasterKarte'

export default BaseComponent.extend({
  components: {
    ToolTips,
    Chip,
    Icon,
  },
  props: {
    label: { type: String },
    text: { type: [Number, String] },
    disabled: { type: Boolean, default: false },
    tooltip: { type: String, default: '' },
    value: { default: '' },
    icon: { type: String, default: '' },
    chipWidth: { type: Number, required: false, default: 33 },
    tall: { type: Boolean, default: false },
    cols: { type: String, default: '3' },
    escapeValue: { type: Boolean, default: true },
    subtitle: { type: String, default: '', required: false },
    radios: { type: Array },
    surveyId: { type: String },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    subtext: {
      type: [String, Number],
    },
    usageHistory: { type: Array as PropType<UsageHistoryClass[]> },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
  },
  computed: {
    chipSize() {
      return {
        width: `${this.chipWidth}px`,
      }
    },
    /** 修了アンケートのURL **/
    completedSurveyUrl(): string {
      return `/survey/${this.surveyId}`
    },
    /** URLが"http"で始まるかどうか **/
    isUrl(): boolean {
      return (
        this.label === 'URL' &&
        typeof this.text === 'string' &&
        this.text.startsWith('http')
      )
    },
    selectedRadio(): number | string {
      let tmp = ''
      this.radios &&
        this.radios.forEach((item: any) => {
          if (item.isAnswer === true) {
            tmp = item.title.slice(0, 1)
          }
        })
      return tmp
    },
    /** 当期支援表示用のテキスト **/
    currentText(): string {
      if (typeof this.subtext === 'string') {
        return this.subtext.includes('+')
          ? this.subtext.split('+').join(' / ')
          : this.subtext
      }
      return this.subtext?.toString()
    },
  },
  methods: {
    /** サービス種別とプロジェクト名を合わせて表示する **/
    projectName(serviceType: string, projectName: string): string {
      return `${serviceType}_${projectName}`
    },
  },
})
</script>

<style lang="scss" scoped>
.o-common-detail-rows__unit {
  border-bottom: 1px solid $c-gray-line;
  margin: 0 !important;
  padding-top: 20px;
  &.is-disabled {
    .o-common-detail-rows__title,
    .o-common-detail-rows__data {
      color: #8f8f8f;
    }
  }
  &.is-noborder {
    border-bottom: 0;
  }
}
.o-common-detail-rows__label {
  padding-top: 0;
}
.o-common-detail-rows__title {
  padding-top: 0;
  font-size: 0.875rem;
  font-weight: bold;
  align-items: center;
  padding-right: 0;
}
.o-common-detail-rows__data {
  padding-top: 0;
  align-items: center;
  font-size: 0.875rem;
  padding-right: 0;
  table {
    th,
    td {
      padding-bottom: 12px;
    }
    tr {
      &:last-child {
        th,
        td {
          padding-bottom: 0;
        }
      }
    }
    th {
      text-align: left;
      padding-right: 24px;
      padding-left: 0;
      width: 150px;
    }
  }
  .o-common-detail-rows__table__edit {
    padding-left: 24px;
    text-align: left;
  }
}
.o-common-detail-rows__data__text {
  word-break: break-all;
  .pre-wrap-text {
    white-space: pre-wrap;
  }
}
.row-subtitle {
  font-size: 1.125rem;
  font-weight: bold;
  margin-top: 1.125rem;
  margin-left: 0rem;
  margin-bottom: 0rem;
  border-left: 7px solid $c-primary;
  padding-left: 1.125rem;
}
.master-karte-text {
  font-weight: normal;
  font-size: 14px;
}

.master-karte-text__link {
  a {
    color: #008a19;
    font-weight: bold;
    text-decoration: underline;
  }
}

.satisfaction__radio::v-deep label {
  font-size: 14px;
  color: #8f8f8f;
}

.satisfaction__radio::v-deep .v-icon {
  color: #8f8f8f;
}

.is-active::v-deep label,
.is-active::v-deep .v-input--selection-controls__input .v-icon {
  color: #1867c0 !important;
  font-weight: bold;
}

.icon {
  padding-top: 0;
}
</style>
